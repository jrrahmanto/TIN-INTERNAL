using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_ViewJournal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SetAccessPage();
        }
    }
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        //uiBtnBulkApproval.Visible = mp.IsChecker;
    }
    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            var ta_pembayaranIDR = new ExportJournalTableAdapters.uspCreateJournalPembayaranIDRTableAdapter();
            var ta_pembayaranUSD = new ExportJournalTableAdapters.uspCreateJournalPembayaranUSDTableAdapter();
            var ta_penghasilanIDR = new ExportJournalTableAdapters.uspCreateJournalPendapatanIDRTableAdapter();
            var ta_penghasilanUSD = new ExportJournalTableAdapters.uspCreateJournalPendapatanUSDTableAdapter();

            var dt_pembayaranIDR = new ExportJournal.uspCreateJournalPembayaranIDRDataTable();
            var dt_pembayaranUSD = new ExportJournal.uspCreateJournalPembayaranUSDDataTable();
            var dt_penghasilanIDR = new ExportJournal.uspCreateJournalPendapatanIDRDataTable();
            var dt_penghasilanUSD = new ExportJournal.uspCreateJournalPendapatanUSDDataTable();

            dt_pembayaranIDR = ta_pembayaranIDR.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));
            dt_pembayaranUSD = ta_pembayaranUSD.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));
            dt_penghasilanIDR = ta_penghasilanIDR.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));
            dt_penghasilanUSD = ta_penghasilanUSD.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));

            uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();
            //get rdlc for seller
            uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/JournalSKD";

            List<ReportParameter> rp = new List<ReportParameter>();

            // Set parameter reportName
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                rp.Add(new ReportParameter("Tanggal", new string[] { null }));
            }
            else
            {
                rp.Add(new ReportParameter("Tanggal", new string[] { CtlCalendarPickUp1.Text }));
            }
            uiRptViewer.ServerReport.SetParameters(rp);

            string format = "PDF", devInfo = @"<DeviceInfo><Toolbar>True</Toolbar></DeviceInfo>";
            string mimeType = "", encoding = "", fileNameExtn = ""; string[] stearms = null;
            Microsoft.Reporting.WebForms.Warning[] warnings = null;

            byte[] result = null;

            result = uiRptViewer.ServerReport.Render(format, devInfo, out mimeType, out encoding, out fileNameExtn, out stearms, out warnings);

        }
        catch (Exception)
        {

            throw;
        }
    }
}