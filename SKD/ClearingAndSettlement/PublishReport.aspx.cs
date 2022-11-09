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

public partial class ClearingAndSettlement_PublishReport : System.Web.UI.Page
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
        //DateTime currentBusinessDate = DateTime.Parse(CtlCalendarPickUpEOD.Text);
        if (IsValidPublish())
        {
            // Generate EOD Reports
            try
            {
                BusinessDateReport = DateTime.Parse(CtlCalendarPickUpEOD.Text);
                Revision = EOD.GetMaxRevisionInvContractPositionEOD(BusinessDateReport);


                // Generating EOD Reports
                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "Start of process generating Report", User.Identity.Name, Common.GetIPAddress(this.Request));

                GenerateEODReports();

                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "End of process generating Report", User.Identity.Name, Common.GetIPAddress(this.Request));

                if (ErrMessage.Count > 0)
                    throw new ApplicationException("");

                // Replicate EOD Reports
                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "Start of process replicate EOD reports to servers", User.Identity.Name, Common.GetIPAddress(this.Request));

                SendFileByFTP(BusinessDateReport);

                ApplicationLog.Insert(DateTime.Now, "Publish Report", "I", "End of process replicate EOD reports to servers", User.Identity.Name, Common.GetIPAddress(this.Request));

                if (ErrMessage.Count > 0)
                    throw new ApplicationException("");

                // Finish running EOD, set flag so external user can log in
                Parameter.UpdateParameterDateValueByCode("IsRunningEOD", null,
                    User.Identity.Name, DateTime.Now);


                // Send EOD reports to Clearing Member
                EventType.AddEventQueue("MessagingEODReport",
                    string.Format("%BDATE%={0};%REVISION%={1};%DATE%={2}", BusinessDateReport.ToString("dd-MMM-yyyy"), Revision.ToString(), BusinessDateReport.ToString("dd MMMM yyyy")), Common.GetIPAddress(this.Request));

            }
            catch (Exception ex)
            {
                uiBlError.Visible = true;

                // display multiple error message
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
        //Get list of reports
        EODData.ReportDataTable dtListReports = EOD.GetListReports(Server.MapPath("~/App_Data/ListReports.xml"));
        DataView dv = new DataView(dtListReports);
        dv.RowFilter = "Type = 'EOD' or Type = 'EOM'";

        //Get list of Clearing Member
        dtCM = ClearingMember.GetActiveClearingMember(BusinessDateReport);

        try
        {
            CurrentThread = 0;

            //resetEvents = new ManualResetEvent[dv.Count];

            ThreadPool.SetMaxThreads(20, 1000);
            foreach (EODData.ReportRow drListReports in (EODData.ReportDataTable)dv.Table)
            {
                //Check wheather is end of month
                //to generate end of month reports
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
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAKEODReport), drListReports);
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
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAllEODReport), drListReports);
                        Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Unable to process All AK EOD Reports in parallel due to: {0}", ex));
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
    }

    private void ProcessAKEODReport(Object cls)//EODData.ReportRow drListReports)
    {
        foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        {
            // Skip for NotaPemberitahuanMarginCall report if condition is not met
            if (((EODData.ReportRow)cls).Name == "NotaPemberitahuanMarginCall")
            {
                if (EOD.IsGenerateMarginCall(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            // Skip for Penalty report if condition is not met
            if (((EODData.ReportRow)cls).Name == "Penalty")
            {
                if (EOD.IsGeneratePenalty(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            SetEODReport((EODData.ReportRow)cls, CtlCalendarPickUpEOD.Text, drCM.CMID, drCM.Code);
        }
        
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessAllEODReport(Object cls)// EODData.ReportRow drListReports)
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
                rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
            }
            else if (drListReports.GenerateType == "All")
            {
                rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
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

            string formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE].ToString();

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

            if (!File.Exists(filepath))
            {
                Common.CreateFileByFileStream(bytes, filepath);
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
                ErrMessage.Add(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  due to {3} ",
                   drListReports.Name, drListReports.GenerateType, "", ex.Message));
            }
        }
    }

    private void SendFileByFTP(DateTime currentBusinessDate)
    {
        DataView dv = null;
        string dir = "";

        // Load list of servers to synch of published report
        try
        {
            EODData.FTPServerDataTable dtListFTP = EOD.GetListFTP(Server.MapPath("~/App_Data/ListFTP.xml"));
            dv = new DataView(dtListFTP);
        }
        catch (Exception ex)
        {
            ErrMessage.Add(string.Format("Fail to load list of servers: {0} ", ex.Message));
        }

        // Loop through all registered servers
        foreach (EODData.FTPServerRow drListFTP in (EODData.FTPServerDataTable)dv.Table)
        {
            // Only process active configuration
            if (drListFTP.ActiveFlag == "true")
            {
                try
                {
                    // FTP Per Clearing Member report
                    try
                    {
                        // Create folder on remote site to be published
                        EOD.CheckEODReportFTPDirectory(drListFTP.FTPFolderAK, currentBusinessDate, drListFTP.FTPAddress, drListFTP.FTPUserName, drListFTP.FTPPassword);

                        dir = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString()
                            + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day;
                        
                        // Only publish the already processed reports
                        if (Directory.Exists(dir))
                        {
                            FileInfo fi;
                            string[] str = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day);
                            FtpWebRequest fwr;

                            // Publish all reports in the directory specified
                            for (int ii = 0; ii < str.Length; ii++)
                            {
                                fi = new FileInfo(str[ii]);
                                try
                                {
                                    fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                                        "ftp://" + drListFTP.FTPAddress + drListFTP.FTPFolderAK + "/" +
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
                    catch (Exception ex)
                    {
                        throw new ApplicationException(string.Format("Per Clearing Member failed due to: {0}", ex.Message));
                    }

                    //FTP All Clearing Member report
                    try
                    {
                        // Create folder on remote site to be published
                        EOD.CheckEODReportFTPDirectory(drListFTP.FTPFolderAll, currentBusinessDate, drListFTP.FTPAddress, drListFTP.FTPUserName, drListFTP.FTPPassword);

                        dir = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day;

                        // Only publish the already processed reports
                        if (Directory.Exists(dir))
                        {
                            FileInfo fi;
                            string[] str = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString() + "\\" + currentBusinessDate.Year + "\\" + currentBusinessDate.Month + "\\" + currentBusinessDate.Day);
                            FtpWebRequest fwr;

                            // Publish all reports in the directory specified
                            for (int ii = 0; ii < str.Length; ii++)
                            {
                                fi = new FileInfo(str[ii]);
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

}
