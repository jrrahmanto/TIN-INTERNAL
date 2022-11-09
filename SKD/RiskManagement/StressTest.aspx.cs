using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Microsoft.Reporting.WebForms;

public partial class WebUI_Fajar_StressTest : System.Web.UI.Page
{
    private decimal scenarioId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ID"] == null)
        {
            throw new ApplicationException("Invalid ID");
        }
        else
        {
            try
            {
                scenarioId = decimal.Parse(Request.QueryString["ID"]);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid ID", ex);
            }
        }

        if (!Page.IsPostBack)
        {
            //LoadData(scenarioId);

        }

    }

    private void LoadData(decimal scenarioId)
    {
        if (scenarioId == 0)
        {
            throw new ApplicationException("Invalid Scenario");
        }

        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter taScenario = 
            new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();

        StressTestScenarioDataTableAdapters.QueriesTableAdapter ta =
            new StressTestScenarioDataTableAdapters.QueriesTableAdapter();

        int? scenarioExists = taScenario.CountScenario(scenarioId);

        if (!scenarioExists.HasValue)
        {
            throw new ApplicationException("Invalid Scenario");
        }
        else if (scenarioExists == 0)
        {
            throw new ApplicationException("Invalid Scenario");
        }

        //ta.EOD_ProcessEOD(DateTime.Now.Date, "STRESS", "GLOBAL", null, null,
        //                    scenarioId, User.Identity.Name, Request.UserHostAddress);

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        //string filter = "";

        //if (uiLookupClearingMember.LookupTextBoxID != "")
        //{
        //    filter += "CMID = " + uiLookupClearingMember.LookupTextBoxID;
        //}

        //if (uiLookupContract.LookupTextBoxID != "")
        //{
        //    if (filter != "")
        //    {
        //        filter += " AND ";
        //    }
        //    filter += "ContractID = " + uiLookupClearingMember.LookupTextBoxID;

        //}

        //ContractResultData.SelectParameters["ScenarioId"].DefaultValue = Request.QueryString["ID"];
        //ContractResultData.FilterExpression = filter;

        //ContractResultData.Select();
        GetReport();
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptStressTest";

        List<ReportParameter> rp = new List<ReportParameter>();
        rp.Add(new ReportParameter("scenarioID", new string[] { scenarioId.ToString() }));
        if (string.IsNullOrEmpty(uiLookupContract.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("contractId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractId", new string[] { uiLookupContract.LookupTextBoxID }));
        }
        if (string.IsNullOrEmpty(uiLookupClearingMember.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("clearingMemberId", new string[] { uiLookupClearingMember.LookupTextBoxID }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();

    }
}