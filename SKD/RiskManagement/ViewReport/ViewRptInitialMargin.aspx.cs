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

public partial class RiskManagement_ViewReport_InitialMargin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for start date parameter
        if (string.IsNullOrEmpty(CtlCalendarPeriodStart.Text))
        {
            uiBLError.Items.Add("Start Date is required.");
        }

        // Check for end date parameter
        if (string.IsNullOrEmpty(CtlCalendarPeriodEnd.Text))
        {
            uiBLError.Items.Add("End Date is required.");
        }

        if (!string.IsNullOrEmpty(CtlCalendarPeriodStart.Text) && !string.IsNullOrEmpty(CtlCalendarPeriodEnd.Text))
        {
            
            if (DateTime.Parse(CtlCalendarPeriodStart.Text) > DateTime.Parse(CtlCalendarPeriodEnd.Text))
            uiBLError.Items.Add("Start Date should be earlier than End Date.");
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
        if (IsValidGet())
        {
            GetReport();
        }
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptInitialMarginSummary";
        
        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPeriodStart.Text))
        {
            rp.Add(new ReportParameter("startDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("startDate", new string[] { CtlCalendarPeriodStart.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPeriodEnd.Text))
        {
            rp.Add(new ReportParameter("endDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPeriodEnd.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
