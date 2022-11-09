using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class RiskManagement_ViewIntradayMargin : System.Web.UI.Page
{
    private string BusinessDate
    {
        get 
        {
            if (Request.QueryString["date"] == null)
            {
                return DateTime.Now.ToString("d-MMM-yyyy");
            }
            else
            {
                try
                {
                    return DateTime.Parse(Request.QueryString["date"]).ToString("d-MMM-yyyy");
                }
                catch 
                {
                    throw new ApplicationException("Invalid date format: " +Request.QueryString["date"]);
                }
            }
        }
    }

    private string Recalculate
    {
        get
        {
            if (Request.QueryString["r"] == null)
            {
                return false.ToString();
            }
            else if (Request.QueryString["r"] == "1")
            {
                return true.ToString();
            }
            else if (Request.QueryString["r"] == "0")
            {
                return false.ToString();
            }
            else
            {
                try
                {
                    return Boolean.Parse(Request.QueryString["r"]).ToString();
                }
                catch
                {
                    return false.ToString();
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiDtBusinessDate.SetCalendarValue(BusinessDate);
        SetReport();
    }

    private void SetReport()
    {
        uiRptIntradayMargin.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptIntradayMargin.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptIntradayMargin.ServerReport.ReportPath = "/SKDReport/RptIntraDayMargin";

        List<ReportParameter> rp = new List<ReportParameter>();
        //rp.Add(new ReportParameter("businessDate", BusinessDate));
        rp.Add(new ReportParameter("businessDate", DateTime.ParseExact(BusinessDate, "d-MMM-yyyy", System.Threading.Thread.CurrentThread.CurrentCulture).ToString("yyyy/MM/dd")));
        rp.Add(new ReportParameter("recalculate", Recalculate));
        rp.Add(new ReportParameter("userName", User.Identity.Name));
        rp.Add(new ReportParameter("ipaddress", Request.UserHostAddress));

        uiRptIntradayMargin.ServerReport.SetParameters(rp);

        //uiRptRiskProfileMatrix.DataBind();
        uiRptIntradayMargin.ServerReport.Refresh();
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewIntradayMargin.aspx?date=" + uiDtBusinessDate.Text);
    }
}
