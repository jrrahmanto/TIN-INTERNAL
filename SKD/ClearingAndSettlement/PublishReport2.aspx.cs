using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections;
using Newtonsoft.Json;
using System.Configuration;

public partial class ClearingAndSettlement_PublishReport2 : System.Web.UI.Page
{

    private List<string> ErrMessage
    {
        get { return (List<string>)ViewState["ErrMessage"]; }
        set { ViewState["ErrMessage"] = value; }
    }

     private int Revision
    {
        get { return (int)ViewState["Revision"]; }
        set { ViewState["Revision"] = value; }
    }

     private DateTime BusinessDateReport
    {
        get { return (DateTime)ViewState["BusinessDateReport"]; }
        set { ViewState["BusinessDateReport"] = value; }
    }

     private ManualResetEvent[] resetEvents
     {
         get { return (ManualResetEvent[])ViewState["resetEvents"]; }
         set { ViewState["resetEvents"] = value; }
     }

     private int? CurrentResetEvents
     {
         get { return (int)ViewState["CurrentResetEvents"]; }
         set { ViewState["CurrentResetEvents"] = value; }
     }
     private int CurrentThread
     {
         get { return (int)ViewState["CurrentThread"]; }
         set { ViewState["CurrentThread"] = value; }
     }

     private StreamWriter sw
     {
         get { return (StreamWriter)ViewState["sw"]; }
         set { ViewState["sw"] = value; }
     }

     private ClearingMemberData.ClearingMemberDataTable dtCM
     {
         get { return (ClearingMemberData.ClearingMemberDataTable)ViewState["dtCM"]; }
         set { ViewState["dtCM"] = value; }
     }

    private BondIssuerData.BondIssuerDataTable dtBI
    {
        get { return (BondIssuerData.BondIssuerDataTable)ViewState["dtBI"]; }
        set { ViewState["dtBI"] = value; }
    }

    private DataView dvReports
     {
         get { return (DataView)ViewState["dvReports"]; }
         set { ViewState["dvReports"] = value; }
     }

    string pkjBaseUrl = ConfigurationManager.AppSettings["PKJ_BASE_URL"];
    string pkjUserName = ConfigurationManager.AppSettings["PKJ_USERNAME"];
    string pkjPassword = ConfigurationManager.AppSettings["PKJ_PASSWORD"];
    string maxTotalError = ConfigurationManager.AppSettings["MAX_TOTAL_ERROR"];

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Items.Clear();
        uiBlError.Visible = false;

        if (!Page.IsPostBack)
        {
            ErrMessage = new List<string>();
        }

        ErrMessage.Clear();

    }

    protected void uiBtnPublish_Click(object sender, EventArgs e)
    {
        try
        {
                
            BusinessDateReport = DateTime.Parse(CtlCalendarPickUpEOD.Text);
            Revision = EOD.GetMaxRevisionInvContractPositionEOD(BusinessDateReport);

            Parameter.UpdateParameterDateValueByCode("IsRunningEOD", BusinessDateReport,
                User.Identity.Name, DateTime.Now);

            if (cbGenerateReport.Checked)
            {
                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", string.Format("Start of process generating Report {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));

                GenerateEODReports();

                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", string.Format("End of process generating Report {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));

                if (ErrMessage.Count > 0)
                    throw new ApplicationException("");
            }
                
            if (cbReplicate.Checked)
            {
                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "Start of process replicate EOD reports to servers", User.Identity.Name, Common.GetIPAddress(this.Request));

                SendFileByFTP(BusinessDateReport);

                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "End of process replicate EOD reports to servers", User.Identity.Name, Common.GetIPAddress(this.Request));

                if (ErrMessage.Count > 0)
                    throw new ApplicationException("");
            }
        }
        catch (Exception ex)
        {
            uiBlError.Visible = true;
            
            if (ex.Message == "")
            {
                foreach (string err in ErrMessage)
                    uiBlError.Items.Add(err);
            }
            else
            {
                uiBlError.Items.Add(ex.Message);
            }
        }
        finally
        {
            Parameter.UpdateParameterDateValueByCode("IsRunningEOD", null,
                User.Identity.Name, DateTime.Now);

        }
    }

    private bool IsValidPublish()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(CtlCalendarPickUpEOD.Text))
        {
            uiBlError.Items.Add("EOD Date is required.");
        }
        else
        {
            if (EOD.IsHoliday(DateTime.Parse(CtlCalendarPickUpEOD.Text)))
            {
                uiBlError.Items.Add("EOD report cannot be published during holiday.");
            }
        }

        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }


    private void GenerateEODReports()
    {
        EODData.ReportDataTable dtListReports = EOD.GetListReports(Server.MapPath("~/App_Data/ListReports.xml"));
        DataView dv = new DataView(dtListReports);
        dv.RowFilter = "Type = 'EOD' or Type = 'EOM'";
        dvReports = dv;
        
        dtCM = ClearingMember.GetActiveClearingMember(BusinessDateReport);
        dtBI = BondIssuer.GetActiveBondIssuer();

        try
        {
            CurrentThread = 0;
            
            ThreadPool.SetMaxThreads(20, 600000);
            foreach (EODData.ReportRow drListReports in (EODData.ReportDataTable)dv.Table)
            {
                if (EOD.IsMonthLastDay(BusinessDateReport) == false)
                {
                    if (drListReports.Type == "EOM")
                    {
                        int currentThread = CurrentThread;
                        Interlocked.Increment(ref currentThread);
                        CurrentThread = currentThread;
                        continue;
                    }
                }

                if (drListReports.GenerateType == "AK")
                {
                    try
                    {
                       
                        if (drListReports.Name != "Collateral Issuer")
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAKEODReport), drListReports);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAKEODReportBondIssuer), drListReports);
                        }
                        
                       Thread.Sleep(500);

                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Unable to process AK EOD Reports in parallel due to: {0}", ex));
                    }
                }
                else if (drListReports.GenerateType == "All")
                {
                    try
                    {
                        
                        if (drListReports.Name != "Collateral Issuer")
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAllEODReport), drListReports);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAllEODReportBondIssuer), drListReports);
                        }
                        
                        Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Unable to process All AK EOD Reports in parallel due to: {0}", ex));
                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void ProcessAKEODReport(Object cls)
    {
        foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        {
            /*
            if (((EODData.ReportRow)cls).Name == "ListeDO")
            {
                if (RptEOD.IsGenerateMarginCall(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "NotaPemberitahuanPembayaran")
            {
                if (RptEOD.IsGenerateNotaPemberitahuan(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "eDO")
            {
                if (RptEOD.IsGenerateeDO(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "TradeRegister")
            {
                if (RptEOD.IsGenerateTradeReg(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            
            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiSeller")
            {
                if (RptEOD.IsGenerateRincianDFSSeller(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiBuyer")
            {
                if (RptEOD.IsGenerateRincianDFSBuyer(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "TradeConfirmationBuyer")
            {
                if (RptEOD.IsGenerateTradeConfirmationBuyer(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "TradeConfirmationSeller")
            {
                if (RptEOD.IsGenerateTradeConfirmationSeller(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "Collateral Member")
            {
                if (RptEOD.IsGenerateTradeConfirmationSeller(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "CollateralIssuer")
            {
                if (RptEOD.IsGenerateTradeConfirmationSeller(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "DFS")
            {
                if (RptEOD.IsGenerateDFS(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            */
            SetEODReport((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, drCM.CMID, drCM.Code);
        }
        
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessAllEODReport(Object cls)
    {
        SetEODReport((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, null, null);
        
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void SetEODReport(EODData.ReportRow drListReports,
        string businessDate, Nullable<decimal> cmId, string cmCode)
    {
        try
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            rp.Add(new ReportParameter("businessDate", businessDate));
            if (drListReports.GenerateType == "AK")
            {
                if (drListReports.Name == "RincianDFSperTransaksiSeller")
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "RincianDFSperTransaksiBuyer")
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "eDO")
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "NotaPemberitahuan")
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "TradeConfirmationSeller")
                {
                    rp.Add(new ReportParameter("CMID", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "TradeConfirmationBuyer")
                {
                    rp.Add(new ReportParameter("CMID", new string[] { cmId.ToString() }));
                }
                else
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
                }
            }
            else if (drListReports.GenerateType == "All")
            {
                if (drListReports.Name == "DFS")
                {
                    rp.Add(new ReportParameter("Code", new string[] { null }));
                    rp.Add(new ReportParameter("cmId", new string[] { null }));
                }
                else if (drListReports.Name == "TradeRegister")
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
                }
                else if (drListReports.Name == "DailyTradeSummary")
                {
                    
                }
                else
                {
                    rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
                }
            }

            ReportViewer rptViewer = new ReportViewer();

            rptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            rptViewer.ServerReport.ReportPath = drListReports.Path;
            rptViewer.ServerReport.SetParameters(rp);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string formatRender = "";

            if (drListReports.GenerateType == "AK")
            {
                if(drListReports.Name == "TradeRegistryListing" || drListReports.Name == "RincianKeuanganHarian")
                {
                    formatRender = "EXCELOPENXML";
                }
                else
                {
                    formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_AK].ToString();
                }
            }
            else if (drListReports.GenerateType == "All")
            {
                if (drListReports.Name == "ClosingBalance" || drListReports.Name == "TradeRegistryListing")
                {
                    formatRender = "EXCELOPENXML"; 
                }
                else if (drListReports.Name == "KontrakElektronik" || drListReports.Name == "NotaPemberitahuan")
                {
                    formatRender = "WORDOPENXML";
                }
                else
                {
                    formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_ALL].ToString();
                }

            }

            byte[] bytes = rptViewer.ServerReport.Render(
              formatRender,
              null, out mimeType, out encoding, out extension,
              out streamids, out warnings);

            string filename = "";
            string filepath = "";

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
                case "EXCELOPENXML":
                    formatType = "xlsx";
                    break;
                case "WORDOPENXML":
                    formatType = "docx";
                    break;
                default:
                    formatType = "pdf";
                    break;
            }

            try
            {
                if (drListReports.GenerateType == "AK")
                {
                    if (drListReports.Name == "eDO")
                    {
                        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();
                        dt = Tradefeed.FillBySearchCriteria(1,
                                DateTime.Parse(CtlCalendarPickUpEOD.Text),
                                null,
                                cmId,
                                null,
                                null);

                        String sellerCmCode = "";
                        for(int i=0; i<dt.Rows.Count; i++)
                        {
                            if(dt.Rows[i]["buyerCMID"].Equals(cmId))
                            {
                                sellerCmCode = dt.Rows[i]["SellerCMCode"].ToString();
                            }
                        }

                        EOD.CheckEODReportDirectory(
                        System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                        filename = string.Format("{0}-{1}_{2}_{3}_{4}.{5}",
                            DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"), Revision,
                            cmCode, drListReports.Name, sellerCmCode, formatType);
                        filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                            BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                    }
                    else
                    {
                        EOD.CheckEODReportDirectory(
                        System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                        filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                            DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"), Revision,
                            cmCode, drListReports.Name, formatType);
                        filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                            BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                    }
                }
                else if (drListReports.GenerateType == "All")
                {
                    EOD.CheckEODReportDirectory(
                        System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), BusinessDateReport);

                    filename = string.Format("{0}-{1}_{2}.{3}",
                        DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"),
                        Revision, drListReports.Name, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to create directory ({0})", ex.Message));
            }

            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to delete file ({0})", ex.Message));
            }
            
            try
            {
                Common.CreateFileByFileStream(bytes, filepath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to create file ({0})", ex.Message));
            }
        }
        catch (Exception ex)
        {
            if (drListReports.GenerateType == "AK")
            {
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  due to {3} ",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message));             
            }
            else if (drListReports.GenerateType == "All")
            {
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, due to {2} ",
                   drListReports.Name, drListReports.GenerateType, ex.Message));
            }

            //ApplicationLog.Insert(DateTime.Now, "Publish Report", "E", string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  due to {3} ",
            //      drListReports.Name, drListReports.GenerateType, cmCode, ex.Message), User.Identity.Name, Common.GetIPAddress(this.Request));

        }
    }

    private void ProcessAKEODReportBondIssuer(Object cls)
    {
        foreach (BondIssuerData.BondIssuerRow drBI in dtBI)
        {

            SetEODReportBondIssuer((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, drBI.BondIssuerID,drBI.BondIssuerName);
        }

        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessAllEODReportBondIssuer(Object cls)
    {
        SetEODReportBondIssuer((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, null, null);
        
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void SetEODReportBondIssuer(EODData.ReportRow drListReports,
        string businessDate, Nullable<decimal> bondIssuerId, string issuerName)
    {
        try
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            rp.Add(new ReportParameter("BusinessDate", businessDate));
            if (drListReports.GenerateType == "AK")
            {
               
                rp.Add(new ReportParameter("IssuerId", new string[] { bondIssuerId.ToString() }));

            }
            else if (drListReports.GenerateType == "All")
            {
                
                rp.Add(new ReportParameter("IssuerId", new string[] { null }));

            }

            ReportViewer rptViewer = new ReportViewer();

            rptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            rptViewer.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            rptViewer.ServerReport.ReportPath = drListReports.Path;
            rptViewer.ServerReport.SetParameters(rp);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string formatRender = "";

            if (drListReports.GenerateType == "AK")
            {
                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_AK].ToString();
            }
            else if (drListReports.GenerateType == "All")
            {
                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_ALL].ToString();
            }

            byte[] bytes = rptViewer.ServerReport.Render(
              formatRender,
              null, out mimeType, out encoding, out extension,
              out streamids, out warnings);

            string filename = "";
            string filepath = "";

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

            try
            {
                if (drListReports.GenerateType == "AK")
                {
                    EOD.CheckEODReportDirectory(
                        System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);

                    filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                        DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"), Revision,
                        issuerName, drListReports.Name, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }
                else if (drListReports.GenerateType == "All")
                {
                    EOD.CheckEODReportDirectory(
                        System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), BusinessDateReport);

                    filename = string.Format("{0}-{1}_{2}.{3}",
                        DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"),
                        Revision, drListReports.Name, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to create directory ({0})", ex.Message));
            }

            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to delete file ({0})", ex.Message));
            }
            
            try
            {
                Common.CreateFileByFileStream(bytes, filepath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Unable to create file ({0})", ex.Message));
            }
        }
        catch (Exception ex)
        {
            if (drListReports.GenerateType == "AK")
            {
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  due to {3} ",
                    drListReports.Name, drListReports.GenerateType, issuerName, ex.Message));
            }
            else if (drListReports.GenerateType == "All")
            {
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, due to {2} ",
                   drListReports.Name, drListReports.GenerateType, ex.Message));
            }

            ApplicationLog.Insert(DateTime.Now, "Publish Report", "E", string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  due to {3} ",
                  drListReports.Name, drListReports.GenerateType, issuerName, ex.Message), User.Identity.Name, Common.GetIPAddress(this.Request));

        }
    }

    private void SendFileByFTP(DateTime currentBusinessDate)
    {
        DataView dv = null;
        string dir = "";
        string LatestRev;
        
        LatestRev = string.Format("{0}-{1}", DateTime.Parse(currentBusinessDate.ToString()).ToString("yyyyMMdd"), 1);
        
        try
        {
            EODData.FTPServerDataTable dtListFTP = EOD.GetListFTP(Server.MapPath("~/App_Data/ListFTP.xml"));
            dv = new DataView(dtListFTP);
        }
        catch (Exception ex)
        {
            ErrMessage.Add(string.Format("Fail to load list of servers: {0} ", ex.Message));
        }
        
        foreach (EODData.FTPServerRow drListFTP in (EODData.FTPServerDataTable)dv.Table)
        {
            if (drListFTP.ActiveFlag == "true")
            {
                try
                {
                    try
                    {
                        EOD.CheckEODReportFTPDirectory(drListFTP.FTPFolderAK, currentBusinessDate, drListFTP.FTPAddress, drListFTP.FTPUserName, drListFTP.FTPPassword);

                        dir = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString()
                            + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day;
                        
                        if (Directory.Exists(dir))
                        {
                            FileInfo fi;
                            string[] str = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day);
                            FtpWebRequest fwr;
                            
                            for (int ii = 0; ii < str.Length; ii++)
                            {
                                fi = new FileInfo(str[ii]);
                                
                                if (LatestRev.Substring(0, LatestRev.Length) == fi.Name.Substring(0, LatestRev.Length))
                                {
                                    try
                                    {
                                        fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                                            "ftp://" + drListFTP.FTPAddress + drListFTP.FTPFolderAK + "/" +
                                                currentBusinessDate.Year + "/" + currentBusinessDate.Month + "/" + currentBusinessDate.Day + "/" + fi.Name));
                                        fwr.Credentials = new NetworkCredential(drListFTP.FTPUserName, drListFTP.FTPPassword);
                                        //
                                        fwr.UsePassive = true;
                                        fwr.UseBinary = true;
                                        fwr.KeepAlive = false;
                                        //
                                        fwr.Method = WebRequestMethods.Ftp.UploadFile;
                                        //
                                        fwr.Timeout= 600000;
                                        fwr.ReadWriteTimeout= 600000;

                                        FileStream fs = File.OpenRead(fi.FullName);
                                        byte[] buffer = new byte[fs.Length];
                                        fs.Read(buffer, 0, buffer.Length);
                                        fs.Close();
                                        Stream ftpstream = fwr.GetRequestStream();
                                        ftpstream.Write(buffer, 0, buffer.Length);
                                        ftpstream.Close();
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new ApplicationException(string.Format("Failed processing {0} ({1})", fi.FullName, ex.Message));
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Per Clearing Member failed due to: {0}", ex.Message));
                    }
                    
                    try
                    {
                        if (!String.IsNullOrEmpty(drListFTP.FTPFolderAll))
                        {
                            EOD.CheckEODReportFTPDirectory(drListFTP.FTPFolderAll, currentBusinessDate, drListFTP.FTPAddress, drListFTP.FTPUserName, drListFTP.FTPPassword);

                            dir = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day;
                            
                            if (Directory.Exists(dir))
                            {
                                FileInfo fi;
                                string[] str = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day);
                                FtpWebRequest fwr;
                                LatestRev = string.Format("{0}-{1}", DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"), Revision);
                                
                                for (int ii = 0; ii < str.Length; ii++)
                                {
                                    fi = new FileInfo(str[ii]);
                                    
                                    if (LatestRev.Substring(0, LatestRev.Length) == fi.Name.Substring(0, LatestRev.Length))
                                    {
                                        try
                                        {
                                            fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                                                "ftp://" + drListFTP.FTPAddress + drListFTP.FTPFolderAll + "/" +
                                                    currentBusinessDate.Year + "/" + currentBusinessDate.Month + "/" + currentBusinessDate.Day + "/" + fi.Name));
                                            fwr.Credentials = new NetworkCredential(drListFTP.FTPUserName, drListFTP.FTPPassword);
                                            fwr.Method = WebRequestMethods.Ftp.UploadFile;

                                            FileStream fs = File.OpenRead(fi.FullName);
                                            byte[] buffer = new byte[fs.Length];
                                            fs.Read(buffer, 0, buffer.Length);
                                            fs.Close();
                                            Stream ftpstream = fwr.GetRequestStream();
                                            ftpstream.Write(buffer, 0, buffer.Length);
                                            ftpstream.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new ApplicationException(string.Format("Failed processing {0} ({1})", fi.FullName, ex.Message));
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("All Clearing Member failed due to: {0}", ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    ErrMessage.Add(string.Format("Failed publishing report to server {0} ({1}), {2} ", drListFTP.FTPName, drListFTP.FTPAddress, ex.Message));
                }
            }
        }
    }

    private string DoLogin()
    {
        string token = "";
        List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
        listParam.Add(new KeyValuePair<string, string>("email", pkjUserName));
        listParam.Add(new KeyValuePair<string, string>("password", pkjPassword));
        string resLogin = APIHelper.Post(
            string.Format("{0}/{1}", pkjBaseUrl, "api/auth/kliring"),
            null, listParam);
        LoginResult.Rootobject objLogin =
            JsonConvert.DeserializeObject<LoginResult.Rootobject>(resLogin);
        if (objLogin.error == null)
        {
            token = objLogin.result.accessToken;
        }
        else
        {
            throw new ApplicationException(objLogin.message);
        }

        return token;
    }

    private void SendMessageToLimaKilo(string token)
    {
        try
        {
            string username = this.User.Identity.Name;
            int totalErrorSettlement = 0;
            int totalErrorPayment = 0;
            int totalSuccessSettlement = 0;
            int totalSuccessPayment = 0;

            List<KeyValuePair<string, string>> listHeader = new List<KeyValuePair<string, string>>();
            listHeader.Add(new KeyValuePair<string, string>("x-access-token", token));
           
            EODTradeProgressData.EODTradeProgressDataTable dtProgressSettlementSubmittedNull = 
                EODTradeProgress.GetBySettlementSubmittedTimeNull();
            foreach (EODTradeProgressData.EODTradeProgressRow drProgress in dtProgressSettlementSubmittedNull)
            {
                if (totalErrorSettlement >= int.Parse(maxTotalError))
                {
                    ErrMessage.Add(
                       string.Format("Number of errors exceed total limit errors, the remaining message will not be submitted. {0} of {1} settlement messages are submitted.",
                           totalSuccessSettlement, dtProgressSettlementSubmittedNull.Count));
                    break;
                }

                try
                {
                    List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
                    listParam.Add(new KeyValuePair<string, string>("exchangeRef", drProgress.ExchangeRef));
                    listParam.Add(new KeyValuePair<string, string>("buyerId", drProgress.BuyerId));
                    listParam.Add(new KeyValuePair<string, string>("sellerId", drProgress.SellerId));
                    listParam.Add(new KeyValuePair<string, string>("productCode", drProgress.ProductCode));
                    listParam.Add(new KeyValuePair<string, string>("contractMonth", drProgress.ContractMonth.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("volume", drProgress.Volume.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("price", drProgress.Price.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("date", drProgress.BusinessDate.ToString("yyyy-MM-dd")));

                    string resSettlement = APIHelper.Post(
                        string.Format("{0}/{1}", pkjBaseUrl, "api/settlements"),
                        listHeader, listParam);

                    SettlementResult.Rootobject objSettlement =
                        JsonConvert.DeserializeObject<SettlementResult.Rootobject>(resSettlement);
                    if (objSettlement.error == null)
                    {
                        DateTime now = DateTime.Now;
                        drProgress.SettlementSubmittedTime = now;
                        drProgress.LastUpdatedBy = username;
                        drProgress.LastUpdatedDate = now;
                        EODTradeProgress.UpdateProgress(drProgress);
                        totalSuccessSettlement += 1;
                    }
                    else
                    {
                        totalErrorSettlement += 1;
                        ErrMessage.Add(string.Format("Settlement ExRef {0} failed: {1}", drProgress.ExchangeRef,
                            objSettlement.message));
                    }
                }
                catch (Exception ex)
                {
                    totalErrorSettlement += 1;
                    ErrMessage.Add(string.Format("Settlement ExRef {0} failed: {1}", drProgress.ExchangeRef,
                        ex.Message));
                }
                
            }
            ApplicationLog.Insert(DateTime.Now, "Publish Report", "I",
                    string.Format("{0} settlement messages are submitted", totalSuccessSettlement),
                    User.Identity.Name, Common.GetIPAddress(this.Request));
            
            EODTradeProgressData.EODTradeProgressDataTable dtProgressFullPaymentSubmittedDateNull=
                EODTradeProgress.GetByFullPaymentSubmittedTimeNull();
            foreach (EODTradeProgressData.EODTradeProgressRow drProgress in dtProgressFullPaymentSubmittedDateNull)
            {
                if (totalErrorPayment >= int.Parse(maxTotalError))
                {
                    ErrMessage.Add(
                       string.Format("Number of errors exceed total limit errors, the remaining message will not be submitted. {0} of {1} payment messages are submitted.",
                           totalSuccessPayment, dtProgressFullPaymentSubmittedDateNull.Count));
                    break;
                }

                try
                {
                    List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
                    listParam.Add(new KeyValuePair<string, string>("exchangeRef", drProgress.ExchangeRef));
                    listParam.Add(new KeyValuePair<string, string>("amount", drProgress.Amount.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("date", drProgress.SellerReceive90Percent.ToString("yyyy-MM-dd")));

                    string resPayment = APIHelper.Post(
                        string.Format("{0}/{1}", pkjBaseUrl, "api/payments"),
                        listHeader, listParam);

                    PaymentResult.Rootobject objPayment =
                        JsonConvert.DeserializeObject<PaymentResult.Rootobject>(resPayment);
                    if (objPayment.error == null)
                    {
                        DateTime now = DateTime.Now;
                        drProgress.FullPaymentSubmittedTime = now;
                        drProgress.LastUpdatedBy = username;
                        drProgress.LastUpdatedDate = now;
                        EODTradeProgress.UpdateProgress(drProgress);
                        totalSuccessPayment += 1;
                    }
                    else
                    {
                        totalErrorPayment += 1;
                        ErrMessage.Add(string.Format("Payment ExRef {0} failed: {1}", drProgress.ExchangeRef,
                            objPayment.message));
                    }
                }
                catch (Exception ex)
                {
                    totalErrorPayment += 1;
                    ErrMessage.Add(string.Format("Payment ExRef {0} failed: {1}", drProgress.ExchangeRef,
                        ex.Message));
                }
                
            }
            ApplicationLog.Insert(DateTime.Now, "Publish Report", "I",
                     string.Format("{0} payment messages are submitted", totalSuccessPayment),
                     User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            ErrMessage.Add(string.Format("Failed to Send Message to LimaKilo: {0}", ex.Message));
        }
    }

    #region New Rpt EOD
    private void GenerateNewEODReports()
    {
        EODData.ReportDataTable dtListReports = EOD.GetListReports(Server.MapPath("~/App_Data/ListReports.xml"));
        DataView dv = new DataView(dtListReports);
        dv.RowFilter = "Type = 'EOD' or Type = 'EOM'";
        
        dtCM = ClearingMember.GetActiveClearingMember(BusinessDateReport);
        try
        {

            CurrentThread = 0;
            
            ThreadPool.SetMaxThreads(20, 1000);
            foreach (EODData.ReportRow drListReports in (EODData.ReportDataTable)dv.Table)
            {
                if (drListReports.Type == "EOM")
                {
                    if (EOD.IsMonthLastDay(BusinessDateReport) == false)
                    {
                        int currentThread = CurrentThread;
                        Interlocked.Increment(ref currentThread);
                        CurrentThread = currentThread;
                        continue;
                    }
                }

                string info = string.Format("ReportName:{0}, GenerateType:{1}, StartTime:{2}", drListReports.Name,
                    drListReports.GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

                if (drListReports.GenerateType == "AK")
                {
                    try
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessNewAKEODReport), drListReports);
                        Thread.Sleep(500);
                        
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process AK EOD Reports in parallel", ex);
                    }
                }
                else if (drListReports.GenerateType == "All")
                {
                    try
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessNewAllEODReport), drListReports);
                        Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process All AK EOD Reports in parallel", ex);
                    }
                }
                
            }

            while (dv.Count != CurrentThread)
            {
                Thread.Sleep(3000);
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        finally
        {
        }
    }

    private void ProcessNewAKEODReport(Object cls)
    {

        foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        {
            if (((EODData.ReportRow)cls).Name == "ListeDO")
            {
                if (RptEOD.IsGenerateMarginCall(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "NotaPemberitahuanPembayaran")
            {
                if (RptEOD.IsGenerateNotaPemberitahuan(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "eDO")
            {

                if (RptEOD.IsGenerateeDO(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "TradeRegister")
            {
                if (RptEOD.IsGenerateTradeReg(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "DFS")
            {
                if (RptEOD.IsGenerateDFS(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiSeller")
            {
                if (RptEOD.IsGenerateRincianDFSSeller(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiBuyer")
            {
                if (RptEOD.IsGenerateRincianDFSBuyer(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "TradeRegistryListing")
            {
                if (RptEOD.IsGeneratTradeRegistryListing(drCM.CMID, BusinessDateReport, drCM.Code) == false)
                {
                    continue;
                }
            }

            SetNewEODReport((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, drCM.CMID, drCM.Code);
        }

        string info = "";
        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessNewAllEODReport(Object cls)// EODData.ReportRow drListReports)
    {

        SetNewEODReport((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, null, null);
        string info = "";

        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
        
        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void SetNewEODReport(EODData.ReportRow drListReports,
        string businessDate, Nullable<decimal> cmId, string cmCode)
    {
        try
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            ReportViewer rptViewer = new ReportViewer();

            string formatRender = "";

            string nmRpt = drListReports.Name;
            
            if (drListReports.Name != "TradeRegister")
            {
                rp.Add(new ReportParameter("businessDate", businessDate));
            }

            if (drListReports.GenerateType == "AK")
            {
                if (drListReports.Name == "eDO")
                {
                    rp.Add(new ReportParameter("ExRef", new string[] { null }));
                }
                else if (drListReports.Name == "TradeRegister")
                {
                    rp.Add(new ReportParameter("BusinessDate", businessDate));
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("cmID", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "DFS")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("cmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "NotaPemberitahuanPembayaran")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "RincianDFSperTransaksiSeller")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("ClearingMemberId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "RincianDFSperTransaksiBuyer")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("ClearingMemberId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "TradeConfirmationSeller")
                {
                    rp.Add(new ReportParameter("BusinessDate", businessDate));
                    rp.Add(new ReportParameter("CMID", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "TradeConfirmationBuyer")
                {
                    rp.Add(new ReportParameter("BusinessDate", businessDate));
                    rp.Add(new ReportParameter("CMID", new string[] { cmId.ToString() }));
                }
                else
                {
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }

                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_AK].ToString();
            }
            else if (drListReports.GenerateType == "All")
            {
                if (drListReports.Name == "TradeRegister")
                {
                    rp.Add(new ReportParameter("businessDate", businessDate));
                    rp.Add(new ReportParameter("code", new string[] { null }));
                    rp.Add(new ReportParameter("CmId", new string[] { null }));
                }
                else if (drListReports.Name == "DFS")
                {
                    rp.Add(new ReportParameter("Code", new string[] { null }));
                    rp.Add(new ReportParameter("cmId", new string[] { null }));
                }
                else if (drListReports.Name == "NotaPemberitahuanPembayaran")
                {
                    rp.Add(new ReportParameter("Code", new string[] { null }));
                    rp.Add(new ReportParameter("CmId", new string[] { null }));
                }
                else if (drListReports.Name == "RincianDFSperTransaksiSeller")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("ClearingMemberId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "RincianDFSperTransaksiBuyer")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                    rp.Add(new ReportParameter("ClearingMemberId", new string[] { cmId.ToString() }));
                }
                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_ALL].ToString();

            }


            rptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());

            rptViewer.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();
            rptViewer.ServerReport.ReportPath = drListReports.Path;
            rptViewer.ServerReport.SetParameters(rp);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            
            byte[] bytes = rptViewer.ServerReport.Render(
              formatRender,
              null, out mimeType, out encoding, out extension,
              out streamids, out warnings);

            string filename = "";
            string filepath = "";

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

            if (drListReports.GenerateType == "AK")
            {
                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                    DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"), Revision,
                    cmCode, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);

            }
            else if (drListReports.GenerateType == "All")
            {
                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), BusinessDateReport);
                filename = string.Format("{0}-{1}_{2}.{3}",
                    DateTime.Parse(CtlCalendarPickUpEOD.Text).ToString("yyyyMMdd"),
                    Revision, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);

            }
            Common.CreateFileByFileStream(bytes, filepath);
        }
        catch (Exception ex)
        {
            if (drListReports.GenerateType == "AK")
            {
                ApplicationLog.CreateAndWriteLog(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"), "ReportEOD");
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"));
            }
            else if (drListReports.GenerateType == "All")
            {
                ApplicationLog.CreateAndWriteLog(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"), "ReportEOD");
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                   drListReports.Name, drListReports.GenerateType, "", ex.Message, "<br>"));
            }

        }
    }
    #endregion
}
