using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryAccessPage : System.Web.UI.Page
{
    private string pageName
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (pageName != null)
            {
                BindData();
            }
        }
    }

    private void BindData()
    {
        try
        {
            ApplicationPageData.ApplicationPageDataTable dt = new ApplicationPageData.ApplicationPageDataTable();
            dt = ApplicationPage.FillByExactPageName(pageName);

            if (dt.Count > 0)
            {
                uiTxbPageName.Text = dt[0].PageName;
                if (!dt[0].IsDescriptionNull())
                {
                    uiTxbDescription.Text = dt[0].Description;
                }
                if (!dt[0].IsApplicationNameNull())
                {
                    uiDdlApplicationName.SelectedValue = dt[0].ApplicationName;
                }
                //if (!dt[0].IsURLNull())
                //{
                //    uiTxbUrlName.Text = dt[0].URL;
                //}
                uiTxbUrlName.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                if (pageName == null)
                {
                    ApplicationPage.Insert(uiTxbPageName.Text, uiTxbDescription.Text,
                                      uiDdlApplicationName.SelectedValue,
                                      uiTxbUrlName.Text.ToLower().Trim(), User.Identity.Name);
                }
                else
                {
                    ApplicationPage.Update(uiTxbPageName.Text, uiTxbDescription.Text,
                                     uiDdlApplicationName.SelectedValue,
                                     uiTxbUrlName.Text.ToLower().Trim(), User.Identity.Name, pageName);
                }
                Response.Redirect("ViewAccessPage.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAccessPage.aspx");
    }
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (pageName != null)
            {
                ApplicationPage.Delete(uiTxbPageName.Text, uiTxbDescription.Text,
                                 uiDdlApplicationName.SelectedValue,
                                 uiTxbUrlName.Text.ToLower().Trim(), User.Identity.Name, pageName);
            }

            Response.Redirect("ViewAccessPage.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBlError.Visible = false;
        uiBlError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (string.IsNullOrEmpty(uiTxbPageName.Text))
        {
            uiBlError.Items.Add("Page name is required.");
        }

        if (string.IsNullOrEmpty(uiTxbUrlName.Text))
        {
            uiBlError.Items.Add("Url name is required.");
        }

        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;

    }
    
    #endregion
}
