using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiBLError.Visible = false;
        if (!Page.IsPostBack)
        {

        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
        UserData.UserAttributesDataTable dt = new UserData.UserAttributesDataTable();
        MembershipUser user = Membership.GetUser(User.Identity.Name);
        ta.FillByUserID(dt, (Guid)user.ProviderUserKey);

        try
        {
            if (IsValidEntry() == true)
            {
                    string pass = Tools.ComputeMD5(uiTxtNewPassword.Text, "");
                    if ((!dt[0].IsHistory01Null() && dt[0].History01 == pass) || 
                        (!dt[0].IsHistory02Null() && dt[0].History02 == pass) || 
                        (!dt[0].IsHistory03Null() && dt[0].History03 == pass) ||
                        (!dt[0].IsHistory04Null() && dt[0].History04 == pass) || 
                        (!dt[0].IsHistory05Null() && dt[0].History05 == pass))
                    {
                        uiBLError.Items.Add("Password has been used previously");
                        uiBLError.Visible = true;
                        return;
                    }

                    dt[0].History05 = dt[0].History04;
                    dt[0].History04 = dt[0].History03;
                    dt[0].History03 = dt[0].History02;
                    dt[0].History02 = dt[0].History01;
                    dt[0].History01 = pass;

                    // Try to change password
                    try
                    {
                        if (Membership.ValidateUser(user.UserName, uiTxtOldPassword.Text))
                        {
                            user.ChangePassword(uiTxtOldPassword.Text, uiTxtNewPassword.Text);
                            ta.Update(dt);
                            Response.Redirect("Default.aspx");
                            ApplicationLog.Insert(DateTime.Now, "Change Password", "I", "User Change Password Successfully", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
                        }
                        else
                        {
                            throw new ApplicationException("Wrong password");
                            ApplicationLog.Insert(DateTime.Now, "Change Password", "E", "User Failed to Change Password", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
                        }
                    }
                    catch (Exception ex)
                    {
                        uiBLError.Items.Add(ex.Message);
                        uiBLError.Visible = true;
                    }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    #region "------- Supporting method ------"

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        MembershipUser user = Membership.GetUser(User.Identity.Name);
        UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
        UserData.UserAttributesDataTable dtUser = new UserData.UserAttributesDataTable();
        
        //string oldPass  = taUser.getPasswordByUserID((Guid)user.ProviderUserKey);
        //string pass = Tools.ComputeMD5(uiTxtOldPassword.Text, "");

        if (user == null)
        {
            // Todo user not found
            uiBLError.Items.Add("User not found.");
            uiBLError.Visible = true;
        }


        //if (oldPass != pass)
        //{
        //    // Todo user not found
        //    uiBLError.Items.Add("Old password does not match.");
        //    uiBLError.Visible = true;
        //}
        
        // Cek password di attributes
        ta.FillByUserID(dtUser, (Guid)user.ProviderUserKey);

        if (dtUser.Count == 0)
        {
            // Todo user not found
            uiBLError.Items.Add("User not found.");
            uiBLError.Visible = true;
        }

        if (string.IsNullOrEmpty(uiTxtOldPassword.Text))
        {
            uiBLError.Items.Add("Old password is required.");
        }

        if (string.IsNullOrEmpty(uiTxtNewPassword.Text))
        {
            uiBLError.Items.Add("New password is required.");
        }

        if (string.IsNullOrEmpty(uiTxtReTypePassword.Text))
        {
            uiBLError.Items.Add("Retype password is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        //uiBtnSave.Visible = mp.IsMaker;
    }

    #endregion
  
}
