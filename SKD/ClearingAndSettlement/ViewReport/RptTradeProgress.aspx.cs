using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_ViewReport_RptTradeProgress : System.Web.UI.Page
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
            ApplicationLog.Insert(DateTime.Now, "Report Trade Progress", "I", "Report", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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

        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptTradeProgress";

        List<ReportParameter> rp = new List<ReportParameter>();

        // Set parameter reportName
        if(ddlTradeProgressType.SelectedValue == "")
        {
            rp.Add(new ReportParameter("tradeProgress", new string[] { CtlCalendarPickUpBusinessDateStart.Text }));
            rp.Add(new ReportParameter("buyerFullPayment", new string[] { null }));
            rp.Add(new ReportParameter("sellerFulfillment", new string[] { null }));
            rp.Add(new ReportParameter("fullDelivery", new string[] { null }));
            rp.Add(new ReportParameter("forcedClosed", new string[] { null }));
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpBusinessDateEnd.Text }));
            rp.Add(new ReportParameter("reportName", new string[] { "" }));
        }
        else if(ddlTradeProgressType.SelectedValue == "B")
        {
            rp.Add(new ReportParameter("tradeProgress", new string[] { null }));
            rp.Add(new ReportParameter("buyerFullPayment", new string[] { CtlCalendarPickUpBusinessDateStart.Text }));
            rp.Add(new ReportParameter("sellerFulfillment", new string[] { null }));
            rp.Add(new ReportParameter("fullDelivery", new string[] { null }));
            rp.Add(new ReportParameter("forcedClosed", new string[] { null }));
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpBusinessDateEnd.Text }));
            rp.Add(new ReportParameter("reportName", new string[] { "Buyer Full Payment" }));
        }
        else if (ddlTradeProgressType.SelectedValue == "F")
        {
            rp.Add(new ReportParameter("tradeProgress", new string[] { null }));
            rp.Add(new ReportParameter("buyerFullPayment", new string[] { null }));
            rp.Add(new ReportParameter("sellerFulfillment", new string[] { null }));
            rp.Add(new ReportParameter("fullDelivery", new string[] { null }));
            rp.Add(new ReportParameter("forcedClosed", new string[] { CtlCalendarPickUpBusinessDateStart.Text }));
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpBusinessDateEnd.Text }));
            rp.Add(new ReportParameter("reportName", new string[] { "Forced Closed" }));
        }
        else if (ddlTradeProgressType.SelectedValue == "U")
        {
            rp.Add(new ReportParameter("tradeProgress", new string[] { null }));
            rp.Add(new ReportParameter("buyerFullPayment", new string[] { null }));
            rp.Add(new ReportParameter("sellerFulfillment", new string[] { null }));
            rp.Add(new ReportParameter("fullDelivery", new string[] { CtlCalendarPickUpBusinessDateStart.Text }));
            rp.Add(new ReportParameter("forcedClosed", new string[] { null }));
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpBusinessDateEnd.Text }));
            rp.Add(new ReportParameter("reportName", new string[] { "Full Delivery" }));
        }
        else if (ddlTradeProgressType.SelectedValue == "E")
        {
            rp.Add(new ReportParameter("tradeProgress", new string[] { null }));
            rp.Add(new ReportParameter("buyerFullPayment", new string[] { null }));
            rp.Add(new ReportParameter("sellerFulfillment", new string[] { CtlCalendarPickUpBusinessDateStart.Text }));
            rp.Add(new ReportParameter("fullDelivery", new string[] { null }));
            rp.Add(new ReportParameter("forcedClosed", new string[] { null }));
            rp.Add(new ReportParameter("endDate", new string[] { CtlCalendarPickUpBusinessDateEnd.Text }));
            rp.Add(new ReportParameter("reportName", new string[] { "Seller Fulfillment" }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }
}