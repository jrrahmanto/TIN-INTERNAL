using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

public partial class WebUI_New_ViewIRCA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ApplicationLog.Insert(DateTime.Now, "Report IRCA", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptViewIRCA";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(uiDdlExchange.SelectedValue))
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("exchangeId", new string[] { uiDdlExchange.SelectedValue }));
        }
        if (string.IsNullOrEmpty(CtlExchangeMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("exchangeMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("exchangeMemberId", new string[] { CtlExchangeMemberLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("investorId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("investorId", new string[] { CtlInvestorLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlCommodityLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("commodityId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("commodityId", new string[] { CtlCommodityLookup1.LookupTextBoxID }));
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
        if (string.IsNullOrEmpty(CtlMonthYear1.Month))
        {
            rp.Add(new ReportParameter("contractMonth", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractMonth", new string[] { CtlMonthYear1.Month }));
        }
        if (string.IsNullOrEmpty(CtlMonthYear1.Year))
        {
            rp.Add(new ReportParameter("contractYear", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractYear", new string[] { CtlMonthYear1.Year }));
        }
        //if (string.IsNullOrEmpty(uiDdlCurrency.SelectedValue))
        //{
        //    rp.Add(new ReportParameter("currencyId", new string[] { null }));
        //}
        //else
        //{
        //    rp.Add(new ReportParameter("currencyId", new string[] { uiDdlCurrency.SelectedValue }));
        //}


        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

}
