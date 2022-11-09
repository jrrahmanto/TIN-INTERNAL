using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class ClearingAndSettlement_ViewClearingMember_ViewRptSellerTenderAnnouncement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidGet() == true)
            {
                ApplicationLog.Insert(DateTime.Now, "Report Seller Tender Announcement", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
                GetReport();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            uiBLError.Items.Add("Clearing member is required.");
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            uiBLError.Items.Add("Business date is required.");
        }
        if (string.IsNullOrEmpty(CtlCommodityLookup1.LookupTextBoxID))
        {
            uiBLError.Items.Add("Commodity is required.");
        }
        if (string.IsNullOrEmpty(uiTxtLetterNo.Text))
        {
            uiBLError.Items.Add("Letter No is required.");
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptSellerTenderAnnouncement";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpBusinessDate.Text }));
        }
        if (string.IsNullOrEmpty(CtlCommodityLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("commodityId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("commodityId", new string[] { CtlCommodityLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(uiTxtLetterNo.Text))
        {
            rp.Add(new ReportParameter("letterNo", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("letterNo", new string[] { uiTxtLetterNo.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
