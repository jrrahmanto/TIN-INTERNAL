using System;
using System.Collections;
using System.Collections.Generic;
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



public partial class ClearingAndSettlement_ViewClearingMember_ViewRptExchangeDailyTrxDcml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidGet())
            {
                GetReport();
                ApplicationLog.Insert(DateTime.Now, "Report Exchange Daily Decimal Transaction", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
    }

    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for start date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            uiBLError.Items.Add("Start Date is required.");
        }

        // Check for end date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            uiBLError.Items.Add("End Date is required.");
        }

        // Display if only error exist       
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptViewExchangeDailyTransactionDcml";
 
        List<ReportParameter> rp = new List<ReportParameter>();
        
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            rp.Add(new ReportParameter("StartDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("StartDate", new string[] { CtlCalendarPickUpStartDate.Text }));
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            rp.Add(new ReportParameter("EndDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("EndDate", new string[] { CtlCalendarPickUpEndDate.Text }));
        }
        rp.Add(new ReportParameter("typeSpa", new string[] { uiDdlTypeSpa.SelectedValue.ToString() }));

       
        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
}
