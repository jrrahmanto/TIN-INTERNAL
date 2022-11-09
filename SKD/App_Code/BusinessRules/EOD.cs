using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for EOD
/// </summary>
public class EOD
{
    private const string FOLDER_AK = "c:\\Share\\EODReports\\AK\\";
    private const string FOLDER_All = "c:\\Share\\EODReports\\All\\";

	public EOD()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}

    public static EODData.ReportDataTable GetListReports(string mapPath)
    {
        EODData.ReportDataTable dt = new EODData.ReportDataTable();

        try
        {
            dt.ReadXml(mapPath);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.FTPServerDataTable GetListFTP(string mapPath)
    {
        EODData.FTPServerDataTable dt = new EODData.FTPServerDataTable();

        try
        {
            dt.ReadXml(mapPath);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.EODExchangeInfoDataTable GetEODExchangeInfo(DateTime businessDate)
    {
        EODData.EODExchangeInfoDataTable dt = new EODData.EODExchangeInfoDataTable();
        EODDataTableAdapters.EODExchangeInfoTableAdapter ta = new EODDataTableAdapters.EODExchangeInfoTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            ta.Fill(dt, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.uspEODValidationDataTable GetNewPrerequisitesEOD(
        DateTime businessDate, string EODType, bool isRedo)
    {
        EODData.uspEODValidationDataTable dt = new EODData.uspEODValidationDataTable();
        EODDataTableAdapters.uspEODValidationTableAdapter ta = new EODDataTableAdapters.uspEODValidationTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            ta.Fill(dt, businessDate, EODType, true, isRedo);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.uspEODValidationDataTable GetPrerequisitesEOD(
        DateTime businessDate, string EODType, bool isReRunRandomize, bool isRedo)
    {
        EODData.uspEODValidationDataTable dt = new EODData.uspEODValidationDataTable();
        EODDataTableAdapters.uspEODValidationTableAdapter ta = new EODDataTableAdapters.uspEODValidationTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            ta.Fill(dt, businessDate, EODType, isReRunRandomize, isRedo);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.ContractEODDataTable GetContractEOD(DateTime businessDate)
    {
        EODData.ContractEODDataTable dt = new EODData.ContractEODDataTable();
        EODDataTableAdapters.ContractEODTableAdapter ta = new EODDataTableAdapters.ContractEODTableAdapter();

        try
        {
            ta.Fill(dt, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.SummaryEODTradeFeedDataTable GetSummaryEODTradefeed(DateTime businessDate)
    {
        EODData.SummaryEODTradeFeedDataTable dt = new EODData.SummaryEODTradeFeedDataTable();
        EODDataTableAdapters.SummaryEODTradeFeedTableAdapter ta = new EODDataTableAdapters.SummaryEODTradeFeedTableAdapter();

        try
        {
            ta.Fill(dt, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODData.SummaryEODSettlementPriceDataTable GetSummaryEODSettlementPrice(DateTime businessDate)
    {
        EODData.SummaryEODSettlementPriceDataTable dt = new EODData.SummaryEODSettlementPriceDataTable();
        EODDataTableAdapters.SummaryEODSettlementPriceTableAdapter ta = new EODDataTableAdapters.SummaryEODSettlementPriceTableAdapter();
        
        try
        {
            ta.Fill(dt, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessEOD(DateTime businessDate, string processType,
        string redoType, string contractList, Nullable<DateTime> tradeChangeDate,
        Nullable<decimal> scenarioId, string userName, string ipAddress, bool isReRun)
    {
       // ApplicationLog.CreateAndWriteLog("Start Process EOD", "EOD");
        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();
        ta.CommandTimeOut = 120000; //milisecond
        //ApplicationLog.CreateAndWriteLog("Start Process EOD- Parameter", "EOD");
        ParameterDataTableAdapters.ParameterTableAdapter taParam = 
            new ParameterDataTableAdapters.ParameterTableAdapter();
       // ApplicationLog.CreateAndWriteLog("Start Process EOD- Variable", "EOD");
        SqlTransaction trans =null;
        SqlConnection conn = new SqlConnection(taParam.Connection.ConnectionString);
        SqlCommand cmdUpdateParameter;
        SqlCommand cmdEOD;
        SqlCommand cmdUpdateParameterAfterEOD;
        
        try
        {
           // ApplicationLog.CreateAndWriteLog("Open Connection", "EOD");
            conn.Open();
            //ApplicationLog.CreateAndWriteLog("Begin Transaction", "EOD");
            trans = conn.BeginTransaction();

            //update Parameter before EOD
            //ApplicationLog.CreateAndWriteLog("update Parameter before EOD", "EOD");
            cmdUpdateParameter = new SqlCommand("SKD.UpdateParameterDateValueByCode",conn,trans);
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode", "EOD");
            cmdUpdateParameter.CommandType = System.Data.CommandType.StoredProcedure;

            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-dateValue", "EOD");
            cmdUpdateParameter.Parameters.Add(new SqlParameter("@dateValue",System.Data.SqlDbType.DateTime));
            cmdUpdateParameter.Parameters["@dateValue"].Value = businessDate;
           // ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-lastUpdatedBy", "EOD");
            cmdUpdateParameter.Parameters.Add(new SqlParameter("@lastUpdatedBy", System.Data.SqlDbType.NVarChar,50));
            cmdUpdateParameter.Parameters["@lastUpdatedBy"].Value = userName;
            ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-lastUpdatedDate", "EOD");
            cmdUpdateParameter.Parameters.Add(new SqlParameter("@lastUpdatedDate", System.Data.SqlDbType.DateTime));
            cmdUpdateParameter.Parameters["@lastUpdatedDate"].Value = DateTime.Now;
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-code", "EOD");
            cmdUpdateParameter.Parameters.Add(new SqlParameter("@code", System.Data.SqlDbType.NVarChar,50));
            cmdUpdateParameter.Parameters["@code"].Value = "IsRunningEOD";

            //Process EOD
            //ApplicationLog.CreateAndWriteLog("SP-EOD_ProcessEOD", "EOD");
            cmdEOD = new SqlCommand("SKD.EOD_ProcessEOD", conn,trans);
            cmdEOD.CommandType = System.Data.CommandType.StoredProcedure;
            //ApplicationLog.CreateAndWriteLog("SP-EOD_ProcessEOD-businessDate", "EOD");
            cmdEOD.Parameters.Add(new SqlParameter("@businessDate", System.Data.SqlDbType.DateTime));
            cmdEOD.Parameters["@businessDate"].Value = businessDate;
           // ApplicationLog.CreateAndWriteLog("SP-EOD_ProcessEOD-userName", "EOD");
            cmdEOD.Parameters.Add(new SqlParameter("@userName", System.Data.SqlDbType.NVarChar, 50));
            cmdEOD.Parameters["@userName"].Value = userName;
            //ApplicationLog.CreateAndWriteLog("SP-EOD_ProcessEOD-ipaddress", "EOD");
            cmdEOD.Parameters.Add(new SqlParameter("@ipaddress", System.Data.SqlDbType.NVarChar,50));
            cmdEOD.Parameters["@ipaddress"].Value = ipAddress;

            //update Parameter after EOD
            //ApplicationLog.CreateAndWriteLog("update Parameter after EOD", "EOD");
            cmdUpdateParameterAfterEOD = new SqlCommand("SKD.UpdateParameterDateValueByCode", conn, trans);
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode", "EOD");
            cmdUpdateParameterAfterEOD.CommandType = System.Data.CommandType.StoredProcedure;

            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-dateValue", "EOD");
            cmdUpdateParameterAfterEOD.Parameters.Add(new SqlParameter("@dateValue", System.Data.SqlDbType.DateTime));
            cmdUpdateParameterAfterEOD.Parameters["@dateValue"].Value = DBNull.Value;
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-lastUpdatedBy", "EOD");
            cmdUpdateParameterAfterEOD.Parameters.Add(new SqlParameter("@lastUpdatedBy", System.Data.SqlDbType.NVarChar, 50));
            cmdUpdateParameterAfterEOD.Parameters["@lastUpdatedBy"].Value = userName;
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-lastUpdatedDate", "EOD");
            cmdUpdateParameterAfterEOD.Parameters.Add(new SqlParameter("@lastUpdatedDate", System.Data.SqlDbType.DateTime));
            cmdUpdateParameterAfterEOD.Parameters["@lastUpdatedDate"].Value = DateTime.Now;
            //ApplicationLog.CreateAndWriteLog("SP-UpdateParameterDateValueByCode-code", "EOD");
            cmdUpdateParameterAfterEOD.Parameters.Add(new SqlParameter("@code", System.Data.SqlDbType.NVarChar, 50));
            cmdUpdateParameterAfterEOD.Parameters["@code"].Value = "IsRunningEOD";

            ApplicationLog.CreateAndWriteLog("cmdUpdateParameter.ExecuteNonQuery", "EOD");
            cmdUpdateParameter.ExecuteNonQuery();
            ApplicationLog.CreateAndWriteLog("cmdEOD.ExecuteNonQuery", "EOD");
            cmdEOD.ExecuteNonQuery();
            ApplicationLog.CreateAndWriteLog("cmdUpdateParameterAfterEOD.ExecuteNonQuery", "EOD");
            cmdUpdateParameterAfterEOD.ExecuteNonQuery();

            trans.Commit();
            conn.Dispose();
        }
        catch (Exception ex)
        {
            ApplicationLog.CreateAndWriteLog("Process EOD error = " + ex.Message, "EOD");
            trans.Rollback();
            throw new ApplicationException(ex.Message, ex);
        }
        finally
        {
            conn.Close();
        }
    }

    public static void ProcessOutgoingFeed()
    {
        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            ta.uspInsertOutgoingFeedMarginBalance();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessEOD(DateTime businessDate, string processType,
        string redoType,  string contractList, Nullable<DateTime> tradeChangeDate,
        Nullable<decimal> scenarioId, string userName, string ipAddress)
    {
        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            using (TransactionScope scope = 
                new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0,60,0)))
            {
                ta.uspEOD_ProcessEOD(businessDate, processType, redoType,
                     contractList, tradeChangeDate, scenarioId, userName, ipAddress);

               
                scope.Complete();
            }            
        }
        catch (SqlException ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessMarkToMarket(DateTime businessDate, string processType,
        string redoType, Nullable<decimal> suggestedPrice, string contractList, Nullable<DateTime> tradeChangeDate, 
        string userName)
    {
        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();

        try
        {
            ta.uspProcessMarkToMarket(businessDate, processType, redoType, 
                suggestedPrice, contractList, tradeChangeDate, userName);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static int GetMaxRevisionInvContractPositionEOD(DateTime businessDate)
    {
        EODDataTableAdapters.MaxRevisionInvContractPositionEODTableAdapter ta = new EODDataTableAdapters.MaxRevisionInvContractPositionEODTableAdapter();
        EODData.MaxRevisionInvContractPositionEODDataTable dt = new EODData.MaxRevisionInvContractPositionEODDataTable();
        int revision = 0;
        try
        {
            ta.Fill(dt, businessDate);

            if (!dt[0].IsRevisionNull())
            {
                revision = dt[0].Revision;
            }

            return revision;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void CheckEODReportDirectory(string folder, DateTime businessDate)
    {
        if (!Directory.Exists(string.Format("{0}{1}", folder, businessDate.Year)))
        {
            Directory.CreateDirectory(string.Format("{0}{1}", folder, businessDate.Year));
            if (!Directory.Exists(string.Format("{0}{1}\\{2}", folder, businessDate.Year, businessDate.Month)))
            {
                Directory.CreateDirectory(string.Format("{0}{1}\\{2}", folder, businessDate.Year, businessDate.Month));
                if (!Directory.Exists(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day));
                }
            }
        }
        else if (!Directory.Exists(string.Format("{0}{1}\\{2}", folder, businessDate.Year, businessDate.Month)))
        {
            Directory.CreateDirectory(string.Format("{0}{1}\\{2}", folder, businessDate.Year, businessDate.Month));
            if (!Directory.Exists(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day)))
            {
                Directory.CreateDirectory(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day));
            }
        }
        else if (!Directory.Exists(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day)))
        {
            Directory.CreateDirectory(string.Format("{0}{1}\\{2}\\{3}", folder, businessDate.Year, businessDate.Month, businessDate.Day));
        }
    }

    public static void CheckEODReportFTPDirectory(string folder, DateTime businessDate, string FTPAddress, string FTPUserName, string FTPPassword)
    {
        FtpWebRequest fwr;       
        StreamReader sr;
        StringCollection scYear, scMonth, scDay;
        bool ftpDir;
        string subFolder = null;
        try
        {
            scYear = scMonth = scDay = new StringCollection();

            fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                  "ftp://" + FTPAddress + "/" +
                  folder));
            sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));            
            ProcessStreamToCollection(sr, out scYear);

            if (!scYear.Contains(string.Format("{0}", businessDate.Year)))
            {
                fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                     "ftp://" + FTPAddress +
                     folder + "/" + businessDate.Year));

                subFolder = businessDate.Year.ToString();

                ftpDir = directoryExists(FTPAddress, folder,subFolder, FTPUserName,FTPPassword);

                if (ftpDir == false)
                {
                    sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword));
                }
                //sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword));

                fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                    "ftp://" + FTPAddress + "/" +
                    folder + "/" + businessDate.Year));
                sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));
                ProcessStreamToCollection(sr, out scMonth);

                if (!scMonth.Contains(string.Format("{0}", businessDate.Month)))
                {
                    subFolder = null;
                    fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                         "ftp://" + FTPAddress +
                         folder + "/" + businessDate.Year + "/" + businessDate.Month));
                    subFolder = businessDate.Year + "/" + businessDate.Month;

                    ftpDir = directoryExists(FTPAddress, folder, subFolder, FTPUserName, FTPPassword);

                    if (ftpDir == false)
                    {
                        sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword));
                    }
                    

                    fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                        "ftp://" + FTPAddress + "/" +
                        folder + "/" + businessDate.Year + "/" + businessDate.Month));
                    sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));
                    ProcessStreamToCollection(sr, out scDay);
                    if (!scDay.Contains(string.Format("{0}", businessDate.Day)))
                    {
                        subFolder = null;

                        fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
                             "ftp://" + FTPAddress +
                             folder + "/" + businessDate.Year + "/" + businessDate.Month + "/" + businessDate.Day));
                        subFolder = businessDate.Year + "/" + businessDate.Month + "/" + businessDate.Day;
                        ftpDir = directoryExists(FTPAddress, folder, subFolder, FTPUserName, FTPPassword);
                        if (ftpDir == false)
                        {
                            ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword);
                        }
                        
                    }
                }
            }
            //else
            //{
            //    fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //            "ftp://" + FTPAddress + "/" +
            //            folder + "/" + businessDate.Year));
            //    sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));
            //    ProcessStreamToCollection(sr, out scMonth);

            //    if (!scMonth.Contains(string.Format("{0}", businessDate.Month)))
            //    {
            //        fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //             "ftp://" + FTPAddress +
            //             folder + "/" + businessDate.Year + "/" + businessDate.Month));
            //        sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword));

            //        fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //            "ftp://" + FTPAddress + "/" +
            //            folder + "/" + businessDate.Year + "/" + businessDate.Month));
            //        sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));
            //        ProcessStreamToCollection(sr, out scDay);
            //        if (!scDay.Contains(string.Format("{0}", businessDate.Day)))
            //        {
            //            fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //                 "ftp://" + FTPAddress +
            //                 folder + "/" + businessDate.Year + "/" + businessDate.Month + "/" + businessDate.Day));
            //            ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword);
            //        }
            //    }
            //    else
            //    {
            //        fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //                "ftp://" + FTPAddress + "/" +
            //                folder + "/" + businessDate.Year + "/" + businessDate.Month));
            //        sr = new StreamReader(ProcessFtp(fwr, WebRequestMethods.Ftp.ListDirectory, FTPUserName, FTPPassword));
            //        ProcessStreamToCollection(sr, out scDay);
            //        if (!scDay.Contains(string.Format("{0}", businessDate.Day)))
            //        {
            //            fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(
            //                 "ftp://" + FTPAddress +
            //                 folder + "/" + businessDate.Year + "/" + businessDate.Month + "/" + businessDate.Day));
            //            ProcessFtp(fwr, WebRequestMethods.Ftp.MakeDirectory, FTPUserName, FTPPassword);
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }        
    }

    public static bool directoryExists(string FTPAddress, string folder, string subFolder, string user, string pass)
    {
        FtpWebRequest ftpRequest;
        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(
                     "ftp://" + FTPAddress +
                     folder + "/" + subFolder + "/");
        ftpRequest.Credentials = new NetworkCredential(user, pass);
        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory; //.PrintWorkingDirectory;//.ListDirectoryDetails;

        try
        {
            using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        /* Resource Cleanup */
        finally
        {
            ftpRequest = null;
        }
    }
    public static Stream ProcessFtp(FtpWebRequest fwr, string WebRequestMethod, string FTPUserName, string FTPPassword)
    {
        try
        {
            FtpWebResponse ftpResp;
            fwr.Credentials = new NetworkCredential(FTPUserName,
               FTPPassword);
            switch (WebRequestMethod)
            {
                case WebRequestMethods.Ftp.ListDirectory:
                    fwr.Method = WebRequestMethods.Ftp.ListDirectory;
                    break;
                case WebRequestMethods.Ftp.MakeDirectory:
                    fwr.Method = WebRequestMethods.Ftp.MakeDirectory;
                    break;
                case WebRequestMethods.Ftp.DeleteFile:
                    fwr.Method = WebRequestMethods.Ftp.DeleteFile;
                    break;
                case WebRequestMethods.Ftp.ListDirectoryDetails:
                    fwr.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    break;
            }

            ftpResp = (FtpWebResponse)fwr.GetResponse();

            return ftpResp.GetResponseStream();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        
    }

    public static void ProcessStreamToCollection(StreamReader sr, out StringCollection sc)
    {
        sc = new StringCollection();
        while (sr.Peek() > -1)
        {
            sc.Add(sr.ReadLine());
        }
    }

    public static bool IsMonthLastDay(DateTime inputDate)
    {
        bool ret;

        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();

        try
        {
            ret = ta.GetMonthLastDay(inputDate).Value;

            return ret;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsHoliday(DateTime inputDate)
    {
        bool isHoliday;

        EODDataTableAdapters.QueryEODTableAdapter ta = new EODDataTableAdapters.QueryEODTableAdapter();

        try
        {
            isHoliday = ta.IsHoliday(inputDate).Value;

            return isHoliday;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsGenerateMarginCall(DateTime businessDate, 
        decimal clearingMemberID)
    {
        bool isGenerate = false;

        EODData.SegDFSDataTable dtSeg = new EODData.SegDFSDataTable();
        EODData.UnsegDFSDataTable dtUnseg = new EODData.UnsegDFSDataTable();

        EODDataTableAdapters.SegDFSTableAdapter taSeg = new EODDataTableAdapters.SegDFSTableAdapter();
        EODDataTableAdapters.UnsegDFSTableAdapter taUnseg = new EODDataTableAdapters.UnsegDFSTableAdapter();

        try
        {
            taSeg.FillByCMIDBusinessDate(dtSeg, clearingMemberID, businessDate);
            taUnseg.FillByCMIDBusinessDate(dtUnseg, clearingMemberID, businessDate);

            if (dtSeg.Count > 0 || dtUnseg.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message,ex);
        }
    }

    public static bool IsGeneratePenalty(DateTime businessDate,
        decimal clearingMemberID)
    {
        bool isGenerate = false;

        EODDataTableAdapters.uspRptPenaltyTableAdapter taPenalty = new EODDataTableAdapters.uspRptPenaltyTableAdapter();
        EODData.uspRptPenaltyDataTable dtPenalty = new EODData.uspRptPenaltyDataTable();

        try
        {
            taPenalty.Fill(dtPenalty, businessDate, clearingMemberID);

            if (dtPenalty.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException("Unable to call procedure uspRptPenalty", ex);
        }
    }
    
    //20150826 Zainab check if EODRevision = null
    public static bool IsEODGenerated(DateTime businessDate)
    {
        EODDataTableAdapters.EODRevisionTableAdapter ta = new EODDataTableAdapters.EODRevisionTableAdapter();
        EODData.EODRevisionDataTable dt = new EODData.EODRevisionDataTable();

        try
        {
            bool isEODGenerated;
            ta.Fill(dt, businessDate);

            if (dt.Count > 0)
            {
                isEODGenerated = true;
            }
            else
            {
                isEODGenerated = false;
            }

            return isEODGenerated;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    
    
}
