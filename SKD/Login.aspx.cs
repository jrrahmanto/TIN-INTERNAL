using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Configuration;
using System.Web.Security;
using Newtonsoft.Json;

public partial class Login : System.Web.UI.Page
{
    private string currentReason
    {
        get
        {
            return Request.QueryString["reason"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiLblReason.Visible = false;
        uiLblReason.Text = "";

        if (currentReason != null && !IsPostBack)
        {
            uiLblReason.Visible = false;
            if (currentReason == "1")
            {
                uiLblReason.Text = "You have been logged out, because your session has timeout.";
                uiLblReason.Visible = true;
            }
            else if (currentReason == "2")
            {
                uiLblReason.Text = "You have been logged out, because you are logged in on a different computer.";
                uiLblReason.Visible = true;
            }
            else if (currentReason == "3")
            {
                uiLblReason.Text = "You have been logged out, because you are not authorized to login at this time.";
                uiLblReason.Visible = true;
            }
        }
    }
   
    private void AuthenticateToActiveDirectory(string username, string password)
    {
        string connString = ConfigurationManager.ConnectionStrings["SKDActiveDirectory"].ConnectionString;
        DirectoryEntry entry = new DirectoryEntry(connString, username, password);
        Object obj = entry.NativeObject;
    }

    private  bool AuthenticateToDatabase(string username, string password) 
    {  
            bool loginResult = Membership.ValidateUser(username, password);
            return loginResult;
    }

    private bool UserLogin(string internalFlag)
    {
        bool result = false;

        // Authenticate with Active Directory for internal users
        if (internalFlag == "Y")
        {
            try
            {
                AuthenticateToActiveDirectory(uiAuthLogin.UserName, uiAuthLogin.Password);
                result = true;
            }
            catch (Exception ex2)
            {
                result = false;
                uiAuthLogin.FailureText = ex2.Message;
            }
        }
        // Authenticate with database for external users
        else if (internalFlag == "N")
        {
            try
            {
                if (AuthenticateToDatabase(uiAuthLogin.UserName, uiAuthLogin.Password))
                {
                    result = true;
                }
            }
            catch (Exception ex2)
            {
                result = false;
                throw new Exception("Error authenticating user: " + ex2.Message);
            }
        }
        return result;
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        bool result = true;
        System.Web.UI.WebControls.Login uiLogin = (System.Web.UI.WebControls.Login)sender;

        // Validate for user existence
        uiAuthLogin.FailureText = "";
        if  (!SKDUser.isUserExist(uiAuthLogin.UserName))
        {
            uiAuthLogin.FailureText = "There is no user in the database with the username " + uiAuthLogin.UserName;
            result = false;
            return;
        }

        // Validate for authorized time
        UserData.UserAttributesDataTable dt = new UserData.UserAttributesDataTable();
        dt = SKDUser.FillAttributeByUserName(uiAuthLogin.UserName);
        DateTime userLoginStartTime = DateTime.MinValue;
        DateTime userLoginEndTime = DateTime.MinValue;


        if (dt.Count > 0)
        {
            string startTime = "";
            string endTime = "";
            if (!dt[0].IsStartTimeNull() && !dt[0].IsEndTimeNull() && dt[0].StartTime.Trim() != "" && dt[0].EndTime.Trim() != "")
            {
                startTime = dt[0].StartTime;
                endTime = dt[0].EndTime;
                string todayDate = DateTime.Now.ToString("yyyy/MM/dd");
                userLoginStartTime = DateTime.Parse(todayDate + " " + startTime);
                userLoginEndTime = DateTime.Parse(todayDate + " " + endTime);

                if (DateTime.Now >= userLoginStartTime && DateTime.Now <= userLoginEndTime)
                {
                    result = UserLogin(dt[0].InternalFlag);

                }
                else
                {
                    uiAuthLogin.FailureText = "You are not authorized to login at this time (valid time is " + startTime + " " + endTime + " " + ").";
                    result = false;
                    return;
                }
            }
            else
            {
                result = UserLogin(dt[0].InternalFlag);
            }
        }
        else
        {
            result = false;
            uiAuthLogin.FailureText = "There is no user in the database with the username " + uiAuthLogin.UserName;
        }
       
        e.Authenticated = result;

        // Save to session
        if (result)
        {
            MembershipUser userInfo = Membership.GetUser(uiAuthLogin.UserName);
            Session["UserNameDesc"] = userInfo.UserName;
            if (userLoginStartTime != DateTime.MinValue)
            {
                Session["StartTime"] = userLoginStartTime;
                Session["EndTime"] = userLoginEndTime;
            }
            else
            {
                Session["StartTime"] = null;
                Session["EndTime"] = null;
            }

            Session["UserName"] = uiAuthLogin.UserName;
            Session["BusinessDate"] = Parameter.GetParameterBusinessDate();

            string username = uiAuthLogin.UserName;
            string timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            Session[username] = timeStamp;
            Application[username] = timeStamp;

            CeilingPriceData.CeilingPriceRow dr = CeilingPrice.GetCeilingPriceById(Convert.ToDecimal(13));
            Session["SettPrice"] = (dr.FloorPrice + 400).ToString("#,###.##");
            Session["CeilingPrice"] = dr.CeilingPrice.ToString("#,###.##");
            Session["FloorPrice"] = dr.FloorPrice.ToString("#,###.##");

            decimal kurs = AXExchangeRate.getKursUsd(Parameter.GetParameterBusinessDate() == null ? Parameter.GetParameterLastEOD().ToString() : Parameter.GetParameterBusinessDate().ToString());
            Session["KursUSD"] = kurs.ToString("#,###.##");
        }
        Session["ClientIPAddress"] = Common.GetIPAddress(this.Request);

        // Log to application log
        if (result)
        {
            ApplicationLog.Insert(DateTime.Now, "Login", "I", "User Login Success", uiAuthLogin.UserName, Session["ClientIPAddress"].ToString());
        }
        else
        {
            ApplicationLog.Insert(DateTime.Now, "Login", "E", "User Login Failed", uiAuthLogin.UserName, Session["ClientIPAddress"].ToString());
        }

        
    }

    protected void uiAuthLogin_LoginError(object sender, EventArgs e)
    {
        MembershipUser userInfo = Membership.GetUser(uiAuthLogin.UserName);
        if (userInfo == null)
        {
            uiAuthLogin.FailureText = "There is no user in the database with the username " + uiAuthLogin.UserName;
        }
        else
        {
            if (uiAuthLogin.FailureText == "")
            {
                if (!userInfo.IsApproved)
                {
                    uiAuthLogin.FailureText = "Your account has not yet been approved by the site's administrators. Please try again later...";
                }
                else if (userInfo.IsLockedOut)
                {
                    uiAuthLogin.FailureText = "Your account has been locked out because of a maximum number of incorrect login attempts. You will NOT be able to login until you contact a site administrator and have your account unlocked.";
                }
                else
                {
                    uiAuthLogin.FailureText = "Your login attempt was not successful. Please try again.";
                }  
            }
         }
    }

    protected void uiAuthLogin_LoggedIn(object sender, EventArgs e)
    {
        UserData.UserAttributesDataTable dt = new UserData.UserAttributesDataTable();
        dt = SKDUser.FillAttributeByUserName(uiAuthLogin.UserName);

        if (dt.Count > 0)
        {
            // Validate password aging for internal users
            if (dt[0].InternalFlag == "N")
            {
                MembershipUser userInfo1 = Membership.GetUser(uiAuthLogin.UserName);
                DateTime today = DateTime.Now;
                TimeSpan interval;

                interval = today - userInfo1.LastPasswordChangedDate;

                // Redirect to changepassword page for user who has not been logged in for certain period of time defined in web.config
                if (interval.Days > int.Parse(ConfigurationManager.ConnectionStrings["PasswordAging"].ConnectionString))
                    //Response.Redirect("ChangePassword.aspx");
                    Response.Redirect("Default.aspx");
            }
        }
        
    }
}
