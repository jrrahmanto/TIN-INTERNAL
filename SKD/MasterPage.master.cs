using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Security;
using System.Xml;
using System.Web.Security;
using System.IO;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private const string _MAKER = "_MAKER";
    private const string _CHECKER = "_CHECKER";
    private const string _VIEWER = "_VIEWER";

    private string _headerTitle;
    public string HeaderTitle
    {
        get { return _headerTitle; }
        set { _headerTitle = value; }
    }

    private bool _showMainTable = true;
    public bool ShowMainTable
    {
        get { return _showMainTable; }
        set { _showMainTable = value; }
    }

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    public bool IsMaker
    {
        get
        {
            return CheckHashedRoleName(_MAKER);
        }
    }

    public bool IsChecker
    {
        get
        {
            return CheckHashedRoleName(_CHECKER);
        }
    }

    public bool IsViewer
    {
        get
        {
            return CheckHashedRoleName(_VIEWER);
        }
    }

    private bool CheckHashedRoleName(string rolename)
    {
        string root = Path.GetDirectoryName(Path.Combine(Server.MapPath("~"), "dummy"));
        string pageName = Server.MapPath(Request.Url.AbsolutePath).Replace(root, "");
        string strippedPageName = pageName.Trim().ToLower().Replace('\\', '/');
        string hashedPageName = Tools.ComputeMD5(strippedPageName, "");

        return Page.User.IsInRole(hashedPageName + rolename);        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            uiMainMenu.Visible = false;
        }

        SetDefaultCulture();

        uiMainMenu.DataSource = XmlDataSourceMenu;
        uiMainMenu.DataBind();

        SetHeaderTitle();
        SetMainTable();
        CheckMenuItems();

        if (Session["BusinessDate"] != null)
        {
            uiLblBusinessDate.Text = Session["BusinessDate"].ToString();
            uiLblBusinessDate.Text = DateTime.Parse(uiLblBusinessDate.Text).ToString("dd-MMM-yyyy");
        }

        if (!IsViewer && !IsChecker && !IsMaker)
        {
            //throw new AuthorizationException();
        }

        NotificationDataTableAdapters.QueriesTableAdapter queryTa = new NotificationDataTableAdapters.QueriesTableAdapter();

        Session["Notification"] = "" + (queryTa.CalcCounterCM() + queryTa.CalcCounterBA() + queryTa.CalcCounterBT() + queryTa.CalcCounterTF());
    }

    public void CheckSession()
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    public void SetHeaderTitle()
    {
        //uiLblHeaderTitle.Text = _headerTitle;
        //uiLblWelcome.Text = "Welcome " + Session["UserName"] + ", ";
    }

    public void SetMainTable()
    {
        //tblMain.Visible = _showMainTable;

    }

    private void SetDefaultCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("id-id");
    }

    private void CheckMenuItems()
    {

        for (int i = uiMainMenu.Items.Count - 1; i >= 0; i--)
        {
            CheckMenuItems(uiMainMenu.Items[i], i);

            string navigateUrl = uiMainMenu.Items[i].NavigateUrl.ToString();
            if (uiMainMenu.Items[i].ChildItems.Count == 0 && navigateUrl == "")
            {
                uiMainMenu.Items.RemoveAt(i);
            }

        }
    }

    private void CheckMenuItems(MenuItem item, int index)
    {
        string navigateUrl;
        for (int ii = item.ChildItems.Count - 1; ii >= 0; ii--)
        {
            navigateUrl = item.ChildItems[ii].NavigateUrl.ToString();

            CheckMenuItems(item.ChildItems[ii], ii);

            if (navigateUrl != "")
            {
                navigateUrl = navigateUrl.Replace("~", "").Trim().ToLower();
                //if (navigateUrl == "/Administration/UserManagement/ViewUser.aspx".ToLower() ||
                //    navigateUrl == "/Administration/UserManagement/EntryUser.aspx".ToLower() ||
                //    navigateUrl == "/Administration/UserManagement/ViewRole.aspx".ToLower() ||
                //    navigateUrl == "/Administration/UserManagement/EntryRole.aspx".ToLower() ||
                //    navigateUrl == "/Administration/UserManagement/ViewAccessPage.aspx".ToLower() ||
                //    navigateUrl == "/Administration/UserManagement/EntryAccessPage.aspx".ToLower() ||
                //    navigateUrl == "/Default.aspx".ToLower()
                //    )
                //{
                //    // do nothing
                //}
                //else 
                if (!this.Page.User.IsInRole(Tools.ComputeMD5(navigateUrl.Trim().ToLower(), "") + _MAKER)
                    && !this.Page.User.IsInRole(Tools.ComputeMD5(navigateUrl.Trim().ToLower(), "") + _CHECKER)
                    && !this.Page.User.IsInRole(Tools.ComputeMD5(navigateUrl.Trim().ToLower(), "") + _VIEWER))
                {
                    item.ChildItems.RemoveAt(ii);
                }
            }
        }

        navigateUrl = item.NavigateUrl.ToString();
        if (item.Parent != null && item.ChildItems.Count == 0 && navigateUrl == "")
        {
            item.Parent.ChildItems.RemoveAt(index);
        }

    }

    private void FilterMenu(XmlDataSource ds)
    {
        XmlDocument doc = ds.GetXmlDocument();
        FilterMenu(doc.ChildNodes[1]);
    }

    private void FilterMenu(XmlNode node)
    {
        System.Security.Principal.IPrincipal user = this.Page.User;
        bool isRemoved = false;

        if (node == null)
        {
            return;
        }

        if (node.Attributes == null)
        {
            // skip
        }
        else if (node.Attributes["url"] != null)
        {
            string url = node.Attributes["url"].Value.Trim().ToLower();
            if (url != ""
                && !user.IsInRole(Tools.ComputeMD5(url.Replace("~", ""), "") + "_Maker")
                && !user.IsInRole(Tools.ComputeMD5(url.Replace("~", ""), "") + "_Checker")
                && !user.IsInRole(Tools.ComputeMD5(url.Replace("~", ""), "") + "_Viewer"))
            {
                isRemoved = true;
            }
        }

        if (node.HasChildNodes)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                FilterMenu(childNode);
            }
        }

        if (!node.HasChildNodes)
        {
            isRemoved = true;
        }

        if (isRemoved)
        {
            node.ParentNode.RemoveChild(node);
        }

    }


    protected void uiBtnLogout_Click(object sender, EventArgs e)
    {
        ApplicationLog.Insert(DateTime.Now, "Logout", "I", "User Logout", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        Session.Clear();
        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.DefaultUrl);
        //FormsAuthentication.RedirectToLoginPage();
    }
}
