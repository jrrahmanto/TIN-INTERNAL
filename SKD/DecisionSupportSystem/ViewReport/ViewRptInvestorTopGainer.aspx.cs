using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class DecisionSupportSystem_ViewReport_ViewRptInvestorTopGainer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        if (IsValidGet() == true)
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
    }

    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();
        uiBLError.Visible = false;

        if (string.IsNullOrEmpty(uiTxtTopSelect.Text))
        {
            uiBLError.Items.Add("Top select is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptInvestorTopGainer";

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
        if (string.IsNullOrEmpty(uiTxtTopSelect.Text))
        {
            rp.Add(new ReportParameter("topSelect", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("topSelect", new string[] { uiTxtTopSelect.Text }));
        }


        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
