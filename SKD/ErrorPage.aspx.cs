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
using System.Text;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Exception objErr;
        string lastErrorUrl = "Unknown Url";
        
        // objErr = (Exception) Session["LastError"];
        //if (Session["LastErrorUrl"] != null)
        //{
        //    lastErrorUrl = (string) Session["LastErrorUrl"];
        //}

        objErr = Server.GetLastError();
        lastErrorUrl = Request.Url.ToString();

        while (objErr is HttpUnhandledException || objErr is System.Reflection.TargetInvocationException)
        {
            objErr = objErr.InnerException;
        } 

        if (objErr == null)
        {
            uiLblMessage.Text = "Unknown Error";
            uiLblErrorMessage.Text = "An error occured while trying to load page";
            uiLblDescription.Text = "This error might occured because of invalid configuration. Please check the web.config file.";
            uiLblOriginalUrl.Text = lastErrorUrl;
            uiLblStackTrace.Text = "";
        }
        else if (objErr is AuthorizationException)
        {
            uiLblMessage.Text = "Unauthorized Access";
            uiLblErrorMessage.Text = objErr.Message;
            uiLblDescription.Text = "If you think you should have access to the page, please contact your administrator to fix this problem.";
            uiLblOriginalUrl.Text = lastErrorUrl;
            uiLblStackTrace.Text = objErr.StackTrace.Replace("\n", "<br />");
        }
        else if (objErr is System.Data.SqlClient.SqlException)
        {
            uiLblMessage.Text = "SQL Exception";
            uiLblErrorMessage.Text = objErr.Message;
            uiLblDescription.Text = "There might be connection error to the database server.";
            uiLblOriginalUrl.Text = lastErrorUrl;
            uiLblStackTrace.Text = objErr.StackTrace;
        }
        else if (objErr is ApplicationException)
        {
            uiLblMessage.Text = "General Application Error";
            uiLblErrorMessage.Text = objErr.Message;
            uiLblDescription.Text = "";
            uiLblOriginalUrl.Text = lastErrorUrl;
            uiLblStackTrace.Text = objErr.StackTrace;
        }
        else
        {
            uiLblMessage.Text = "General Error";
            uiLblErrorMessage.Text = objErr.Message;
            uiLblDescription.Text = "";
            uiLblOriginalUrl.Text = lastErrorUrl;
            uiLblStackTrace.Text = objErr.StackTrace;
        }

        //Session["LastError"] = "";
        //Session["LastErrorUrl"] = "";

        Server.ClearError();

    }

}
