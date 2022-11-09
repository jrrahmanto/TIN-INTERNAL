using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class DecisionSupportSystem_ViewReport_ViewRptException : System.Web.UI.Page
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptException";

        List<ReportParameter> rp = new List<ReportParameter>();
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
