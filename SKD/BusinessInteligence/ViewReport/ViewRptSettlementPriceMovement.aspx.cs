using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class BusinessInteligence_ViewReport_ViewRptSettlementPriceMovement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptSettlementPriceMovement";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDateFrom.Text))
        {
            rp.Add(new ReportParameter("businessDateFrom", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDateFrom", new string[] { CtlCalendarPickUpBusinessDateFrom.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDateFrom.Text))
        {
            rp.Add(new ReportParameter("businessDateTo", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDateTo", new string[] { CtlCalendarPickUpBusinessDateTo.Text }));
        }
        if (string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("contractId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractId", new string[] { CtlContractCommodityLookup1.LookupTextBoxID }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
