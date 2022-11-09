using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptClearingHouseAccountBalance : System.Web.UI.Page
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptClearingHouseAccountBalance";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPickUpTransactionDate.Text))
        {
            rp.Add(new ReportParameter("transactionDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("transactionDate", new string[] { CtlCalendarPickUpTransactionDate.Text }));
        }
        if (string.IsNullOrEmpty(uiDdlCurrency.SelectedValue))
        {
            rp.Add(new ReportParameter("currencyId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("currencyId", new string[] { uiDdlCurrency.SelectedValue }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
