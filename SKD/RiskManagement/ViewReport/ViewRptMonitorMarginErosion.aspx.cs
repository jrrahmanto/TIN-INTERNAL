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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

public partial class RiskManagement_ViewReport_MonitorMarginErosion : System.Web.UI.Page
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptMarginErosion";

        List<ReportParameter> rp = new List<ReportParameter>();        
        if (string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("contractId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractId", new string[] { CtlContractCommodityLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartBusinessDate.Text))
        {
            rp.Add(new ReportParameter("startBusinessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("startBusinessDate", new string[] { CtlCalendarPickUpStartBusinessDate.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndBusinessDate.Text))
        {
            rp.Add(new ReportParameter("endBusinessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("endBusinessDate", new string[] { CtlCalendarPickUpEndBusinessDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
