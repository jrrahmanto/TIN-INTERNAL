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

public partial class RiskManagement_ViewReport_MonitorOpenInterest : System.Web.UI.Page
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptOpenInterest";

        List<ReportParameter> rp = new List<ReportParameter>();
       
        if (string.IsNullOrEmpty(uiDdlExchange.SelectedValue))
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { uiDdlExchange.SelectedValue }));
        }
        if (string.IsNullOrEmpty(uiDdlCurrency.SelectedValue))
        {
            rp.Add(new ReportParameter("currencyId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("currencyId", new string[] { uiDdlCurrency.SelectedValue }));
        }
        if (string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("contractId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractId", new string[] { CtlContractCommodityLookup1.LookupTextBoxID }));
        }
        rp.Add(new ReportParameter("contractMonth", new string[] { null }));
        rp.Add(new ReportParameter("contractYear", new string[] { null }));
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDateFrom.Text))
        {
            rp.Add(new ReportParameter("businessDateFrom", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDateFrom", new string[] { CtlCalendarPickUpBusinessDateFrom.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDateTo.Text))
        {
            rp.Add(new ReportParameter("businessDateTo", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDateTo", new string[] { CtlCalendarPickUpBusinessDateTo.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
