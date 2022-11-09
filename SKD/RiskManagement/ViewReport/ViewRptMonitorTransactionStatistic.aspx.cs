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

public partial class RiskManagement_ViewReport_MonitorTransactionStatistic : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        { 
            
        }
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptMonitorTransactionStatistic";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] {null }));                
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));                
        }
        if (string.IsNullOrEmpty(uiDdlExchange.SelectedValue))
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { null }));                
        }
        else
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { uiDdlExchange.SelectedValue }));                
        }
        rp.Add(new ReportParameter("exchangeMemberId", new string[] { null }));
        if (string.IsNullOrEmpty(uiDdlCurrency.SelectedValue))
        {
            rp.Add(new ReportParameter("currencyId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("currencyId", new string[] { uiDdlCurrency.SelectedValue }));
        }
        if (string.IsNullOrEmpty(CtlProductLookupascx1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("commodityId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("commodityId", new string[] { CtlProductLookupascx1.LookupTextBoxID }));
        }
        

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    
    }
}
