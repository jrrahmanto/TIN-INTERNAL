using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Data;

public partial class ClearingAndSettlement_ReportEOD_ClearingMemberReport_RptAllClearingMember : System.Web.UI.Page
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
            DownloadReport();
            ApplicationLog.Insert(DateTime.Now, "Report All Clearing Member", "I", "User Download Report " + uiDdlReport.Text, Page.User.Identity.Name, Common.GetIPAddress(this.Request));
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
        if (string.IsNullOrEmpty(uiTxtRevision.Text))
        {
            // Get last revision from InvestorContractPositionEOD
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

        dve.RowFilter = "GenerateType = 'All'";
        dve.Sort = "Type, ReportDescription ASC";

        uiDdlReport.DataSource = dve;
        uiDdlReport.DataTextField = "ReportDescription";
        uiDdlReport.DataValueField = "Name";
        uiDdlReport.DataBind();
    }

    private void DownloadReport()
    {
        DateTime businessDate = DateTime.Parse(CtlCalendarPickUp1.Text);
        string revision = uiTxtRevision.Text.Trim();

        string formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_ALL].ToString();
        string formatType = "";
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

        if (uiDdlReport.SelectedValue == "ClosingBalance")
        {
            formatType = "xlsx";
        }

        string filename = string.Format("{0}-{1}_{2}.{3}", businessDate.ToString("yyyyMMdd"), revision, uiDdlReport.SelectedValue, formatType);
        string fileP = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), businessDate.Year, businessDate.Month, businessDate.Day, filename);

        if (File.Exists(fileP))
        {
            try
            {
                FileStream fsSource = new FileStream(fileP, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[fsSource.Length];
                fsSource.Read(bytes, 0, bytes.Length);

                Response.ContentType = string.Format("application/{0}", formatType == "xlsx" ? "vnd.openxmlformats-officedocument.spreadsheetml.sheet" : formatType);
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
