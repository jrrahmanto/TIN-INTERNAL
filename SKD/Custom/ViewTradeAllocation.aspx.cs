﻿using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Custom_ViewTradeAllocation : System.Web.UI.Page
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
            ApplicationLog.Insert(DateTime.Now, "Report Open Position Listing", "I", "View", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    public void DisableUnwantedExportFormat(ReportViewer ReportViewerID, string strFormatName)
    {
        FieldInfo info;
        foreach (RenderingExtension extension in ReportViewerID.ServerReport.ListRenderingExtensions())
        {
            if (extension.Name == strFormatName)
            {
                info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                info.SetValue(extension, false);
            }
        }
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptEODTradeRegisterForCustom";

        DisableUnwantedExportFormat(uiRptViewer, "XML");
        DisableUnwantedExportFormat(uiRptViewer, "CSV");
        DisableUnwantedExportFormat(uiRptViewer, "IMAGE");
        DisableUnwantedExportFormat(uiRptViewer, "MHTML");
        DisableUnwantedExportFormat(uiRptViewer, "EXCELOPENXML");
        DisableUnwantedExportFormat(uiRptViewer, "WORDOPENXML");

        List<ReportParameter> rp = new List<ReportParameter>();

        // Set parameter reportName
        if (string.IsNullOrEmpty(CtlCalendarPickUpBusinessDate.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpBusinessDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }
}