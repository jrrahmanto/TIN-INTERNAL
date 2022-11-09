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



public partial class ClearingAndSettlement_ViewClearingMember_ViewRptCashSettlement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillSettlementDate();
    }

    private void FillSettlementDate()
    {
        uiDdlCashSettlement.DataSource = odsCashSettlement;
        IEnumerable dv = (IEnumerable)odsCashSettlement.Select();
        DataView dve = (DataView)dv;

        uiDdlCashSettlement.DataSource = dve;
        uiDdlCashSettlement.DataBind();
    }

    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for end date parameter
        if (uiDdlCashSettlement.SelectedValue == "")
        {
            uiBLError.Items.Add("Cash Settlement Date is required.");
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

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptViewCashSettlement";

        List<ReportParameter> rp = new List<ReportParameter>();

        if (!string.IsNullOrEmpty(uiDdlCashSettlement.SelectedValue))
        {

            rp.Add(new ReportParameter("BusinessDate", new string[] { (DateTime.Parse(uiDdlCashSettlement.SelectedValue).Date).ToShortDateString() }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    protected void uiBtnView_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidGet())
            {
                GetReport();
                //string ss = DateTime.Parse(uiDdlCashSettlement.SelectedValue).Date.ToString();


                ApplicationLog.Insert(DateTime.Now, "Report Cash Settlement", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }   
    }
}
