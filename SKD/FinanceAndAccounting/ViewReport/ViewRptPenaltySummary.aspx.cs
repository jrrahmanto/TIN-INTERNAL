using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptPenaltySummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidGetReport())
            {
                GetReport();
                ApplicationLog.Insert(DateTime.Now, "Report Penalty Summary", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptPenaltySummary";

        List<ReportParameter> rp = new List<ReportParameter>();

        // set parameter for start date
        if (string.IsNullOrEmpty(CtlCalendarStartDate.Text))
        {
            rp.Add(new ReportParameter("StartDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("StartDate", new string[] { CtlCalendarStartDate.Text }));
        }

        // set parameter for end date
        if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
        {
            rp.Add(new ReportParameter("EndDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("EndDate", new string[] { CtlCalendarEndDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }

    public bool isValidGetReport()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        if (string.IsNullOrEmpty(CtlCalendarStartDate.Text))
        {
            uiBLError.Items.Add("Start Date is required.");
        }

        if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
        {
            uiBLError.Items.Add("End Date is required.");
        }

        if (!string.IsNullOrEmpty(CtlCalendarStartDate.Text) && !string.IsNullOrEmpty(CtlCalendarEndDate.Text))
        {
            if (DateTime.Parse(CtlCalendarStartDate.Text) > DateTime.Parse(CtlCalendarEndDate.Text))
                uiBLError.Items.Add("Start date should be earlier than end date");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
}
