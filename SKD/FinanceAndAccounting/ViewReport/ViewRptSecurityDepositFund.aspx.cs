using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class FinanceAndAccounting_ViewReport_ViewRptSecurityDepositFund : System.Web.UI.Page
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
        uiBLError.Visible = false;

        decimal iiOut = 0;
        if (!string.IsNullOrEmpty(uiTxtIDRAdjusment.Text))
        { 
             if (decimal.TryParse(uiTxtIDRAdjusment.Text, out iiOut) == false)
            {
                uiBLError.Items.Add("Invalid numeric in IDR adjusment.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxtUSDAdjusment.Text))
        {
            if (decimal.TryParse(uiTxtUSDAdjusment.Text, out iiOut) == false)
            {
                uiBLError.Items.Add("Invalid numeric in USD adjusment.");
            }
        }        

        if (uiBLError.Items.Count > 0)
        {
            uiBLError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptSDF";

        List<ReportParameter> rp = new List<ReportParameter>();
        if (string.IsNullOrEmpty(uiTxtIDRAdjusment.Text))
        {
            rp.Add(new ReportParameter("adjusmentIDR", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("adjusmentIDR", new string[] { uiTxtIDRAdjusment.Text }));
        }
        if (string.IsNullOrEmpty(uiTxtUSDAdjusment.Text))
        {
            rp.Add(new ReportParameter("adjusmentUSD", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("adjusmentUSD", new string[] { uiTxtUSDAdjusment.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}
