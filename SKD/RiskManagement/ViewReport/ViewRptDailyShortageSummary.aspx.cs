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
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

public partial class RiskManagement_ViewReport_ViewRptDailyShortageSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptDailyShortageSummary";

        string username = this.User.Identity.Name;

        List<ReportParameter> rp = new List<ReportParameter>();
        rp.Add(new ReportParameter("businessDate", new string[] { null }));
        rp.Add(new ReportParameter("username", new string[] { username }));
        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    protected void uiBtnView_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();

            ApplicationLog.Insert(DateTime.Now, "Report Daily Shortage Summary", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }
}
