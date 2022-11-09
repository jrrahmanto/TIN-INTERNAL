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

public partial class RiskManagement_ViewReport_ViewRptClearingHouseSuspensionCommand : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }


    protected void uiBtnView_Click(object sender, EventArgs e)
    {
        try
        {
            GetReport();

            ApplicationLog.Insert(DateTime.Now, "Report Clearing House Suspension Command", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptClearingHouseSuspensionCommand";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlCalendarPeriod.Text))
        {
            rp.Add(new ReportParameter("bussDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("bussDate", new string[] { CtlCalendarPeriod.Text }));
        }
        if (string.IsNullOrEmpty(uiDdlSuspension.Text))
        {
            rp.Add(new ReportParameter("suspenseType", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("suspenseType", new string[] { uiDdlSuspension.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
