using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


public partial class RiskManagement_ViewReport_ViewRptLastMarkToMarketPosition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        GetReport();
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptLastMarkToMarketPostion";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUp1.Text }));
        }
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
