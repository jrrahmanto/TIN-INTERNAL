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

public partial class ClearingAndSettlement_Tender_ViewDeliveryListing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
    }

    private void SetReport()
    {
        uiRptTenderReport.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptTenderReport.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();
        if (uiDdlListBy.Text == "Seller")
        {
            uiRptTenderReport.ServerReport.ReportPath = "/SKDReport/RptDeliveriesListingSeller";
        }
        else
        {
            uiRptTenderReport.ServerReport.ReportPath = "/SKDReport/RptDeliveriesListingBuyer";
        }


        List<ReportParameter> rp = new List<ReportParameter>();
        rp.Add(new ReportParameter("businessDate", uiDtpTenderDate.Text));
        rp.Add(new ReportParameter("clearingMemberId", CtlClearingMemberLookup1.LookupTextBoxID));
        rp.Add(new ReportParameter("contractID", CtlContractCommodityLookup1.LookupTextBoxID));


        uiRptTenderReport.ServerReport.SetParameters(rp);

        //uiRptRiskProfileMatrix.DataBind();
        uiRptTenderReport.ServerReport.Refresh();
    }
    protected void uiBtnViewReport_Click(object sender, EventArgs e)
    {
        if (IsValidViewReport())
        {
            SetReport();
        }

    }
    

    private bool IsValidViewReport()
    {
        bool isValid = true;
        uiBLError.Items.Clear();
        uiBLError.Visible = false;

        if (uiDtpTenderDate.Text == "")
        {
            uiBLError.Items.Add("Tender Date is required");
        }

        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBox))
        {
            isValid = false;
            uiBLError.Items.Add("Clearing member is required.");
        }

        if (string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBox))
        {
            isValid = false;
            uiBLError.Items.Add("Contract is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
}
