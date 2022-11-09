using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

public partial class WebUI_New_EntryUser : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    #region "   Use Case   "
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                BindData();
                SetControlAccessByMakerChecker();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex);
            }
        }
        MasterPage master = (MasterPage)this.Master;

        bool isMaker = master.IsMaker;

        string[] roles = Roles.GetRolesForUser();
    }

    // Handler for save button
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        List<string> checkedItem = new List<string>();
        foreach (ListItem item in uiChkGroup.Items)
        {
            if (item.Selected)
            {
                checkedItem.Add(item.Text);
            }
        }
        try
        {
            if (!IsValidEntry())
            {
                string isInternal = "";
                string isLocked = "";
                string isDisabled = "";
                Nullable<decimal> cmid = null;

                // Internal / external user
                if (uiChkInternal.Checked)
                {
                    isInternal = "Y";
                }
                else
                {
                    isInternal = "N";
                    if (uiCtlClearingMember.LookupTextBox != "")
                    {
                        cmid = Convert.ToDecimal(uiCtlClearingMember.LookupTextBoxID);
                    }
                }

                // Lock / Unlock user
                if (uiChkLocked.Checked)
                {
                    isLocked = "Y";
                }
                else
                {
                    isLocked = "N";
                }

                // Disable / enable user
                if (uiChkDisabled.Checked)
                {
                    isDisabled = "Y";
                }
                else
                {
                    isDisabled = "N";
                }

                string pwd = uiTxbPassword.Text;
                if (uiChkInternal.Checked)
                {
                    if (pwd == "")
                    {
                        Random RandomClass = new Random();
                        pwd = "P@ssw0rd" + RandomClass.Next();
                    }
                    
                }

                if (currentID == null)
                {
                    
                    SKDUser.SaveUser(currentID, uiTxbUserName.Text, uiTxbName.Text,
                                     isLocked, isDisabled, pwd, isInternal,
                                     uiTxbStartTime.Text, uiTxbEndTIme.Text, checkedItem, cmid, User.Identity.Name);
                }
                else
                {

                    SKDUser.Update(currentID, uiTxbUserName.Text, uiTxbName.Text,
                                     isLocked, isDisabled, pwd, isInternal,
                                     uiTxbStartTime.Text, uiTxbEndTIme.Text, checkedItem, cmid, User.Identity.Name);
                }

                // Redirect to summary page
                Response.Redirect("ViewUser.aspx");
            }
          

        }
        catch (Exception ex)
        {
            // Display error message;
            uiBlError.Items.Clear();
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    // ???
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        //ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();
        //ApplicationPageData.ApplicationPageDataTable dt = new ApplicationPageData.ApplicationPageDataTable();

        //ta.Fill(dt);

        //foreach (ApplicationPageData.ApplicationPageRow dr in dt)
        //{
        //    if (dr.URL == "/FinanceAndAccounting/Parameter/EntryBankPDM.aspx")
        //    {
        //        string aaaaa = "";
        //    }
        //    System.Web.Security.Roles.CreateRole(Tools.ComputeMD5(dr.URL, "") + "_Checker");
        //    System.Web.Security.Roles.CreateRole(Tools.ComputeMD5(dr.URL, "") + "_Maker");
        //    System.Web.Security.Roles.CreateRole(Tools.ComputeMD5(dr.URL, "") + "_Viewer");
        //}

        // Redirect to summary page
        Response.Redirect("ViewUser.aspx");
    }

    #endregion

    #region "   Supporting Method   "

    private void BindData()
    {
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();
        UserData.UserProfileDataTable userDT = new UserData.UserProfileDataTable();
        UserData.UserAttributesDataTable attributeDT = new UserData.UserAttributesDataTable();
        UserData.UserGroupDataTable userGroupDT = new UserData.UserGroupDataTable();
        
        dt = SKDUser.fillGroup();
        uiChkGroup.DataSource = dt;
        uiChkGroup.DataTextField = "GroupName";
        uiChkGroup.DataValueField = "GroupID";
        uiChkGroup.DataBind();
        Guid userGuid;

        if (currentID != null)
        {
            userGuid = new Guid(currentID);
            userDT = SKDUser.GetUserByUserID(userGuid);
            attributeDT = SKDUser.FillAttribute(userGuid);
            userGroupDT = SKDUser.FillUserGroupByUserName(uiTxbUserName.Text);
            string[] userGroups = SKDUser.GetUserGroups(userGuid, userDT[0].UserName);
            System.Collections.ArrayList listUserGroups = new System.Collections.ArrayList(userGroups);

            foreach (ListItem item in uiChkGroup.Items)
            {
                //item.Selected = userGroups.Contains(item.Text);
                item.Selected = listUserGroups.Contains(item.Text);
            }

            if (userDT.Count > 0)
            {
                uiTxbUserName.Text = userDT[0].UserName;
                if (!userDT[0].IsCommentNull())
                {
                    uiTxbName.Text = userDT[0].Comment;
                }
                if (userDT[0].IsLockedOut)
                {
                    uiChkLocked.Checked = true;
                }
                else
                {
                    uiChkLocked.Checked = false;
                }
                if (userDT[0].IsApproved)
                {
                    uiChkDisabled.Checked = false;
                }
                else
                {
                    uiChkDisabled.Checked = true;
                }
            }
            if (attributeDT.Count > 0)
            {
                if (attributeDT[0].InternalFlag == "Y")
                {
                    uiChkInternal.Checked = true;
                }
                else
                {
                    uiChkInternal.Checked = false;
                }
                if (!attributeDT[0].IsStartTimeNull())
                {
                    uiTxbStartTime.Text = attributeDT[0].StartTime;
                }
                if (!attributeDT[0].IsEndTimeNull())
                {
                    uiTxbEndTIme.Text = attributeDT[0].EndTime;
                }
                if (!attributeDT[0].IsCMIDNull())
                {
                    uiCtlClearingMember.SetClearingMemberValue(attributeDT[0].CMID.ToString(), attributeDT[0].Code);
                }

            }
            foreach (UserData.UserGroupRow dr in userGroupDT)
            {
                uiChkGroup.Items.FindByValue(dr.GroupID.ToString()).Selected = true;
            }
        }
        
       

        

        

    }

    // DisplayErrorMessage
    // Purpose      : Validate entry
    // Parameter    : -
    // Return       : Boolean whether the entry is valid or not
    private bool IsValidEntry()
    {
        bool isError = false;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();

        // ---------- Validate required field ----------
        if (uiTxbUserName.Text == "")
        {
            uiBlError.Items.Add("User name is required.");
        }
        if (currentID == null)
        {
            if (!uiChkInternal.Checked)
            {
                if (uiTxbPassword.Text == "")
                {
                    uiBlError.Items.Add("Password is required.");
                }
            }
        }

        // Validate to update logged in user
        string userName = null;
        if (Session["UserName"] != null)
        {
            userName = Session["UserName"].ToString();
        }
        if (userName == uiTxbUserName.Text)
            uiBlError.Items.Add("Unable to update logged in user");

        // Show visibility of error message
        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isError = true;
        }

        return isError;
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -

    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }

    private void SetControlAccessByMakerChecker()
    {
        MasterPage mp = (MasterPage)this.Master;

        bool pageMaker = mp.IsMaker;
        bool pageChecker = mp.IsChecker;
        bool pageViewer = mp.IsViewer;

        // Set control visibility
        uiBtnSave.Visible = pageMaker;
    }

    #endregion


}
