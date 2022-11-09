using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class ClearingAndSettlement_ViewClearingMember_ViewRptCurrentCHTradeRegistry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        try
        {
            ApplicationLog.Insert(DateTime.Now, "Report Current Clearing House Trade Registry", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptViewCurrentCHTradeRegistry";

        List<ReportParameter> rp = new List<ReportParameter>();       
        rp.Add(new ReportParameter("userName", new string[] { User.Identity.Name }));



        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
