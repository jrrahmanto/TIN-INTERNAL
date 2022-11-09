using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Controls_CtlYearMonthBuktiPotong : System.Web.UI.UserControl
{
    public string Month
    {
        get { return uiDdlMonth.SelectedValue; }
    }

    public string Year
    {
        get { return uiTxbYear.Text.Trim(); }
    }

    public string YearMonth
    {
        get { return uiTxbYear.Text.Trim() + uiDdlMonth.SelectedValue; }
    }

    public Nullable<bool> DisabledMonthYear
    {
        get
        {
            if (ViewState["DisabledMonthYear"] != null)
            {
                return bool.Parse(ViewState["DisabledMonthYear"].ToString());
            }
            else
            {
                return null;
            }
        }
        set { ViewState["DisabledMonthYear"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //Tools.RegisterGenericScript(this.Page);
        if (!Page.IsPostBack)
        {
            //FillYearDropDown();
            //string month = "";
            //month = DateTime.Now.Month.ToString();
            //month = string.Format("{1:00}", month);

            uiTxbYear.Text = DateTime.Now.Year.ToString();
            uiDdlMonth.SelectedValue = string.Format("{0:MM}", DateTime.Now.Month.ToString());

        }
    }

    public void SetDisabledMonthYear(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiDdlMonth.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiTxbYear.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
    
        if (!Page.ClientScript.IsStartupScriptRegistered("MonthYearDisabled" + uiDdlMonth.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "MonthYearDisabled" + uiDdlMonth.ClientID, sb.ToString(), true);
        }
    }

    public void SetMonthYear(int month, int year)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiDdlMonth.ClientID + ".value='" + month + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxbYear.ClientID + ".value='" + year + "';");
        //sb.AppendLine("window.document.forms[0]." +
            //uiDdlYear.ClientID + ".value='" + year + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("monthYearValue" + uiDdlMonth.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "monthYearValue" + uiDdlMonth.ClientID, sb.ToString(), true);
        }        
    }

    //private void FillYearDropDown()
    //{
        
    //    int startYear = DateTime.Now.Year - 5;
    //    int endYear = DateTime.Now.Year + 5;

    //    for (int ii = startYear; ii < endYear; ii++)
    //    {
    //        ListItem li = new ListItem();
    //        li.Text = ii.ToString();
    //        li.Value = ii.ToString();
    //        uiDdlYear.Items.Add(li);
    //    }
    //}
}
