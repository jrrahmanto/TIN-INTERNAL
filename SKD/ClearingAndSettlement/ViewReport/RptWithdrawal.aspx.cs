using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_ViewReport_RptWithdrawal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();
            ApplicationLog.Insert(DateTime.Now, "Report Withdrawal", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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
        uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptWithdrawal";

        List<ReportParameter> rp = new List<ReportParameter>();

        var exchangeReff = " ";
        var seller = " ";
        if (uiExchReff.Text != "")
        {
            exchangeReff = uiExchReff.Text;
        }
        if (uiSeller.Text != "")
        {
            seller = uiSeller.Text;
        }
        // Set parameter reportName
        rp.Add(new ReportParameter("BusinessDate", new string[] { CtlCalendarPickUpBusinessDate.Text }));
        rp.Add(new ReportParameter("exchangeReff", new string[] { exchangeReff }));
        rp.Add(new ReportParameter("seller", new string[] { seller }));

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }
}