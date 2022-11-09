using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptOffBalanceSheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidGet())
            {
                GetReport();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    private bool IsValidGet()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(CtlCalendarPickUpTransactionDate.Text))
        {
            isValid = false;
        }

        return isValid;
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptOffBalanceSheet";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPickUpTransactionDate.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpTransactionDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
