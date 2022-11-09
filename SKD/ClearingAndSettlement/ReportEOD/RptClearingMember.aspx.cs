using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Net;

public partial class ClearingAndSettlement_ReportEOD_ClearingMemberReport_RptClearingMember : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        if (!Page.IsPostBack)
        {
            Session["ListReportFile"] = Server.MapPath("~/App_Data/ListReports.xml");

            FillReportDropDown();
        }
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);

        Session.Remove("ListReportFile");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        //PdfViewer1.FilePath = "file:///C:/Share/EODReports/All/IRCA20091223.pdf";

        //PdfViewer1.FilePath = @"http://localhost/SKD/TradeRegistryListing20091223.pdf";
        //PdfViewer1.FilePath = @"http://localhost/EODReports/All/TradeRegistryListing20091223.pdf";
        
        if (IsValidDownload() == true)
        {
            ApplicationLog.Insert(DateTime.Now, "Report Clearing Member", "I", "User Download Report " + uiDdlReport.Text, Page.User.Identity.Name, Common.GetIPAddress(this.Request));
            DownloadReport();
        }
    }

    private bool IsValidDownload()
    {
        bool isValid = true;
        decimal revision;
        
        uiBLError.Items.Clear();

        if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
        {
            isValid = false;
            uiBLError.Items.Add("Business date is required.");
        }

        if (uiDdlReport.SelectedValue != "Collateral Issuer")
        {
            if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBox))
            {
                isValid = false;
                uiBLError.Items.Add("Clearing member is required.");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(CtlBondIssuer.LookupTextBox))
            {
                isValid = false;
                uiBLError.Items.Add("Bond Issuer is required.");
            }
        }
        
        if (string.IsNullOrEmpty(uiTxtRevision.Text))
        {
            // Get last revision from InvContractPositionEOD
            if (!string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                revision = Parameter.GetMaxRevision(Convert.ToDateTime(CtlCalendarPickUp1.Text));
                if (revision == 0)
                {
                    isValid = false;
                    uiBLError.Items.Add("No available revision for the report to be generated.");
                }
                else
                {
                    uiTxtRevision.Text = revision.ToString();
                }
            }
        }

        if (isValid == false)
        {
            uiBLError.Visible = true;
        }

        return isValid;    
    }

    private void FillReportDropDown()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceReport.Select();
        DataView dve = (DataView)dv;

        dve.RowFilter = "GenerateType = 'AK'";
        dve.Sort = "Type, ReportDescription ASC";

        uiDdlReport.DataSource = dve;
        uiDdlReport.DataTextField = "ReportDescription";
        uiDdlReport.DataValueField = "Name";
        uiDdlReport.DataBind();
    }

    private void DownloadReport()
    {

        if (uiDdlReport.SelectedValue == "NotaPemberitahuan" && CtlClearingMemberLookup2.LookupTextBox != "")
        {
            uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptEDONotaPemberitahuanRevision";

            List<ReportParameter> rp = new List<ReportParameter>();

            // Set parameter reportName
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                rp.Add(new ReportParameter("businessDate", new string[] { null }));
                rp.Add(new ReportParameter("sellerid", new string[] { null }));
                rp.Add(new ReportParameter("buyerid", new string[] { null }));
            }
            else
            {
                rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUp1.Text }));
                rp.Add(new ReportParameter("sellerid", new string[] { CtlClearingMemberLookup2.LookupTextBox }));
                rp.Add(new ReportParameter("buyerid", new string[] { CtlClearingMemberLookup1.LookupTextBox }));
            }
            uiRptViewer.ServerReport.SetParameters(rp);
            byte[] result = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;
            result = uiRptViewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            string fileName = "Nota Pemberitahuan.pdf";

            System.IO.FileStream file_report = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Report\\" + fileName);
            file_report.Write(result, 0, result.Length);
            file_report.Close();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Report\\" + fileName;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=Nota Pemberitahuan.pdf");
            response.BinaryWrite(result);
            response.End();
        }
        else if (uiDdlReport.SelectedValue == "TradeRegister" && CtlClearingMemberLookup2.LookupTextBox != "")
        {
            uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptEODTradeRegisterForWA";

            List<ReportParameter> rp = new List<ReportParameter>();

            // Set parameter reportName
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                rp.Add(new ReportParameter("businessDate", new string[] { null }));
                rp.Add(new ReportParameter("codeSeller", new string[] { null }));
                rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
            }
            else
            {
                rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUp1.Text }));
                rp.Add(new ReportParameter("codeSeller", new string[] { CtlClearingMemberLookup2.LookupTextBox }));
                rp.Add(new ReportParameter("clearingMemberId", new string[] { CtlClearingMemberLookup1.LookupTextBox }));
            }
            uiRptViewer.ServerReport.SetParameters(rp);
            byte[] result = null;
            string mimeType = "";
            string encoding = "";
            string filenameExtension = "";
            string[] streamids = null;
            Warning[] warnings = null;
            result = uiRptViewer.ServerReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            string fileName = "Trade Register.pdf";

            System.IO.FileStream file_report = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Report\\" + fileName);
            file_report.Write(result, 0, result.Length);
            file_report.Close();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Report\\" + fileName;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=Trade Register.pdf");
            response.BinaryWrite(result);
            response.End();
        }
        else
        {
            DateTime businessDate = DateTime.Parse(CtlCalendarPickUp1.Text);
            string clearingMemberId = CtlClearingMemberLookup1.LookupTextBoxID;
            string revision = uiTxtRevision.Text.Trim(); ;
            string clearingMemberCode = "";

            string formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_AK].ToString();
            string formatType = "";
            string filename = "";

            switch (formatRender.ToUpper())
            {
                case "PDF":
                    formatType = "pdf";
                    break;
                case "EXCEL":
                    formatType = "xls";
                    break;
                case "CSV":
                    formatType = "csv";
                    break;
                default:
                    formatType = "pdf";
                    break;
            }

            if (uiDdlReport.SelectedValue != "Collateral Issuer")
            {
                clearingMemberCode =
                ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID));
                filename = string.Format("{0}-{1}_{2}_{3}.{4}", businessDate.ToString("yyyyMMdd"), revision, clearingMemberCode, uiDdlReport.SelectedValue, formatType);
            }
            else
            {
                filename = string.Format("{0}-{1}_{2}_{3}.{4}", businessDate.ToString("yyyyMMdd"), revision, CtlBondIssuer.LookupTextBox, uiDdlReport.SelectedValue, formatType);
            }

            //string filepath = string.Format("http://ecom/EODReports/AK/{0}/{1}/{2}/{3}", businessDate.Year, businessDate.Month, businessDate.Day, filename);


            string fileP = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), businessDate.Year, businessDate.Month, businessDate.Day, filename);

            if (File.Exists(fileP))
            {
                try
                {
                    FileStream fsSource = new FileStream(fileP, FileMode.Open, FileAccess.Read);
                    //FileStream fsSource = new FileStream(fileP, FileMode.Create, FileAccess.Write);
                    byte[] bytes = new byte[fsSource.Length];
                    fsSource.Read(bytes, 0, bytes.Length);

                    Response.ContentType = string.Format("application/{0}", formatType);
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                    bw.Write(bytes, 0, bytes.Length);
                    bw.Flush();
                    bw.Close();
                }
                catch (Exception ex)
                {
                    uiBLError.Visible = true;
                    uiBLError.Items.Add(ex.Message);
                }
            }
            else
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add("No available data for the report to be generated.");
            }

        }
    }
}
