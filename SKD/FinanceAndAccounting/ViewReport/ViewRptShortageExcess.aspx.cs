using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptShortageExcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

    }

    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for end date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            uiBLError.Items.Add("Business Date is required.");
        }

        // Display if only error exist       
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
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

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptShortageExcess";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            rp.Add(new ReportParameter("BusinessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("BusinessDate", new string[] { CtlCalendarPickUpBusinessDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
