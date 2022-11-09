using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptFeesSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        if (isValidGetReport())
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

    private void GetReport()
    {
        string monthDescription = "";
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptFeesSummary";

        List<ReportParameter> rp = new List<ReportParameter>();

        // set parameter for clearing member
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
            rp.Add(new ReportParameter("clearingMemberName", new string[] { "All" }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
            rp.Add(new ReportParameter("clearingMemberName", new string[] { CtlClearingMemberLookup1.LookupTextBox + " - " + ClearingMember.GetClearingMemberNameByClearingMemberID(Decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID)) }));
            
        }

        // set parameter for month
        if (string.IsNullOrEmpty(CtlMonthYear1.Month))
        {
            rp.Add(new ReportParameter("monthTrans", new string[] { null }));
        }
        else 
        {
            rp.Add(new ReportParameter("monthTrans", new string[] { CtlMonthYear1.Month }));

            switch (CtlMonthYear1.Month)
            {
                case "1": monthDescription = "January"; break;
                case "2": monthDescription = "February"; break;
                case "3": monthDescription = "March"; break;
                case "4": monthDescription = "April"; break;
                case "5": monthDescription = "May"; break;
                case "6": monthDescription = "June"; break;
                case "7": monthDescription = "July"; break;
                case "8": monthDescription = "August"; break;
                case "9": monthDescription = "September"; break;
                case "10": monthDescription = "October"; break;
                case "11": monthDescription = "November"; break;
                case "12": monthDescription = "December"; break;
                default: break;
            }
            if (monthDescription != "")
                rp.Add(new ReportParameter("monthName", new string[] { monthDescription }));
            else
                rp.Add(new ReportParameter("monthName", new string[] { null }));
        }

        if (string.IsNullOrEmpty(CtlMonthYear1.Year))
        {
            rp.Add(new ReportParameter("yearTrans", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("yearTrans", new string[] { CtlMonthYear1.Year }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }

    public bool isValidGetReport()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        if (string.IsNullOrEmpty(CtlMonthYear1.Month))
        {
            uiBLError.Items.Add("Month is required.");
        }
        if (string.IsNullOrEmpty(CtlMonthYear1.Year))
        {
            uiBLError.Items.Add("Year is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
}
