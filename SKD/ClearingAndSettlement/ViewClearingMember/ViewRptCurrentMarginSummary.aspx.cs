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

public partial class ClearingAndSettlement_ReportEOD_ViewClearingMember_ViewCurrentMarginSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Insert(DateTime.Now, "Report Current Margin Summary", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptViewMarginSummary";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("investorId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("investorId", new string[] { CtlInvestorLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlProductLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("commodityId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("commodityId", new string[] { CtlProductLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            rp.Add(new ReportParameter("startDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("startDate", new string[] { CtlCalendarPickUpStartDate.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            rp.Add(new ReportParameter("endDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpEndDate.Text }));
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
