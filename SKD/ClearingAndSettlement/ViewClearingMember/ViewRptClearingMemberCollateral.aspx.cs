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
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

public partial class ClearingAndSettlement_ReportEOD_ViewClearingMember_ViewClearingMemberCollateral : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        if (IsValidGet())
        {
            try
            {
                ApplicationLog.Insert(DateTime.Now, "Report Clearing Member Collateral", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));

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

        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            uiBLError.Items.Add("Clearing member is required.");
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            uiBLError.Items.Add("Business date is required.");
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptClearingMemberCollateral";

        List<ReportParameter> rp = new List<ReportParameter>();
        rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpBusinessDate.Text }));
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpLodgement.Text))
        {
            rp.Add(new ReportParameter("lodgementDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("lodgementDate", new string[] { CtlCalendarPickUpLodgement.Text }));
        }
        if (string.IsNullOrEmpty(uiDdlCollateralType.SelectedValue))
        {
            rp.Add(new ReportParameter("function", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("function", new string[] { uiDdlCollateralType.SelectedValue }));
        }
        if (string.IsNullOrEmpty(uiDdlLodgementType.SelectedValue))
        {
            rp.Add(new ReportParameter("lodgementType", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("lodgementType", new string[] { uiDdlLodgementType.SelectedValue }));
        }
        if (string.IsNullOrEmpty(uiDdlIssuerType.SelectedValue))
        {
            rp.Add(new ReportParameter("issuerType", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("issuerType", new string[] { uiDdlIssuerType.SelectedValue }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpIssuer.Text))
        {
            rp.Add(new ReportParameter("issuerDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("issuerDate", new string[] { CtlCalendarPickUpIssuer.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpMaturity.Text))
        {
            rp.Add(new ReportParameter("maturityDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("maturityDate", new string[] { CtlCalendarPickUpMaturity.Text }));
        }
        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
