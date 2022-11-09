using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Controls_CtContractlCalenderPickUp : System.Web.UI.UserControl
{
    public string Text
    {
        get { return uiTxtCalendar.Text; }
    }

    public Nullable<bool> DisabledCalendar
    {
        get
        {
            if (ViewState["DisabledCalendar"] != null)
            {
                return bool.Parse(ViewState["DisabledCalendar"].ToString());
            }
            else
            {
                return null;
            }
        }
        set { ViewState["DisabledCalendar"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Tools.RegisterGenericScript(this.Page);

        uiTxtCalendar.Attributes.Add("readonly", "readonly");
        uiImgEmpty.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtCalendar.ClientID + ".value='';");
        if (!Page.IsPostBack)
        {
            if (Session["BusinessDate"] != null)
            {
                if (uiTxtCalendar.Text == "")
                {
                    uiTxtCalendar.Text = DateTime.Parse(Session["BusinessDate"].ToString()).ToString("dd-MMM-yyyy");
                }
            }
        }

        if (DisabledCalendar != null)
        {
            SetDisabledCalendar(DisabledCalendar.Value);
        }
    }

    public void SetDisabledCalendar(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiImg.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiImgEmpty.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        //sb.AppendLine(string.Format("enable('{0}', {1});", uiTxtCalendar.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable('{0}', {1});", uiImg.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable('{0}', {1});", uiImgEmpty.ClientID, b.ToString().ToLower()));


        if (!Page.ClientScript.IsStartupScriptRegistered("calendarDisabled" + uiImg.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "calendarDisabled" + uiImg.ClientID, sb.ToString(), true);
        }
    }



    public void SetCalendarValue(string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtCalendar.ClientID + ".value='" + value + "';");

        //sb.AppendLine(string.Format("setValue('{0}', '{1}');", uiTxtCalendar.ClientID, value));

        if (!Page.ClientScript.IsStartupScriptRegistered("calendarValue" + uiTxtCalendar.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "calendarValue" + uiTxtCalendar.ClientID, sb.ToString(), true);
        }
    }
    protected void uiBtnSetMaxYear_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtCalendar.ClientID + ".value='" + "31-Dec-9999" + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("calendarValue" + uiTxtCalendar.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "calendarValue" + uiTxtCalendar.ClientID, sb.ToString(), true);
        }
    }
}
