using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

public partial class RiskManagement_ViewImpactMatrix : System.Web.UI.Page
{
    private string eID
    {
        get
        { 
            return Request.QueryString["eID"];
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetReport();
    }

    private void SetReport()
    {
        uiRptRiskProfileMatrix.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptRiskProfileMatrix.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();
        
        uiRptRiskProfileMatrix.ServerReport.ReportPath = "/SKDReport/RptRiskProfileMatrix";

        List<ReportParameter> rp = new List<ReportParameter>();
        rp.Add(new ReportParameter("riskProfileId", eID));

        uiRptRiskProfileMatrix.ServerReport.SetParameters(rp);

        //uiRptRiskProfileMatrix.DataBind();
        uiRptRiskProfileMatrix.ServerReport.Refresh();
    }
}
