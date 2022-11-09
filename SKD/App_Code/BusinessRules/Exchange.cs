using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
using ExchangeDataTableAdapters;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for Exchange
/// </summary>
public class Exchange
{

    //get all data from dataset to datagrid
    public static ExchangeData.ExchangeDataTable SelectExchangeByExchangeCode(string exchCode)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();

        try
        {

            ta.FillByExchangeCode(dt, exchCode);
            return dt;


        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data");
        }
    }

    public static string GetExchangeNameByExchangeId(decimal exchangeId)
    {
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        ExchangeData.ExchangeRow dr = null;
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeTableAdapter();
        string exchangeName = "";
        try
        {
            ta.FillByExchangeID(dt, exchangeId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                exchangeName = dr.ExchangeName;
            }

            return exchangeName;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex); ;
        }
    }

    private ExchangeTableAdapter _exchangeAdapter = new ExchangeTableAdapter();

    /// <summary>
    /// Get list of all exchange ==> it should be in class exchange
    /// </summary>
    /// <param name="dt">list of exchange</param>
    public ExchangeData.ExchangeDataTable GetExchanges()
    {
        return _exchangeAdapter.GetActiveOnly();
    }

    //get all data from dataset to datagrid
    public static ExchangeData.ExchangeRow SelectExchangeByExchangeID(decimal exchID)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        ExchangeData.ExchangeRow dr = null;
        try
        {
            ta.FillByExchangeID(dt, exchID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data");
        }
    }

    public static ExchangeData.ExchangeDdlDataTable SelectExchange()
    {
        ExchangeDataTableAdapters.ExchangeDdlTableAdapter ta = new ExchangeDataTableAdapters.ExchangeDdlTableAdapter();
        ExchangeData.ExchangeDdlDataTable dt = new ExchangeData.ExchangeDdlDataTable();
        try
        {
            dt.AddExchangeDdlRow("");
            dt.AcceptChanges();
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data");
        }

    }

    //get all data from dataset to datagrid
    public static ExchangeData.ExchangeDataTable SelectExchangeByAction(string Approval, string exchCode)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();

        try
        {
        
            ta.FillByExchCodeAndApprovalStatus(dt, exchCode, Approval);
            return dt;

            
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data");
        }
    }

    public static void ProposeExchange(string exchangeCode, string exchangeIpAddress, string localIpAddress,
                                      int localPort, string exchangeType, string exchangeName, string action,
                                      string tenderFlag, string transferFlag, string createdBy, DateTime createdDate,
                                      string lastUpdateBy, DateTime lastUpdateDate,
                                      string userName, decimal originalId, 
                                      string exchangeIPAddressOutbound, string localIPAddressOutbound, int localPortOutbound)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(exchangeCode, "P", exchangeIpAddress, localIpAddress, localPort, createdBy,  createdDate, lastUpdateBy,
                      lastUpdateDate, null, exchangeType, exchangeName, action, tenderFlag,
                      transferFlag, originalId, exchangeIPAddressOutbound, localIPAddressOutbound, localPortOutbound);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: exchangeCode={0}|exchangeIpAddress={1}|localIpAddress={2}" +
                                        "|localPort={3}|exchangeType={4}|exchangeName={5}|tenderFlag={6}|transferFlag={7}",
                                            exchangeCode, exchangeIpAddress,
                                            localIpAddress,
                                            localPort.ToString(),
                                            exchangeType,
                                            exchangeName, tenderFlag, transferFlag);
                AuditTrail.AddAuditTrail("Exchange", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist. Please input new record.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static void ProposedDeletedExchange(List<string> deletedList, string userName)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                {
                    foreach (string item in deletedList)
                    {
                        //proposed delete
                        ta.FillByExchangeID(dt, Convert.ToDecimal(item));
                        if (dt.Count > 0)
                        {
                            ta.Insert(dt[0].ExchangeCode, "P", dt[0].ExchangeIPAddress, dt[0].LocalIPAddress,
                             dt[0].LocalPort, "", DateTime.Now, "", DateTime.Now, "", dt[0].ExchangeType, dt[0].ExchangeName,
                             "D", null, null, dt[0].ExchangeId, dt[0].ExchangeIPAddressOutbound, dt[0].LocalIPAddressOutbound, dt[0].LocalPortOutbound);
                            logMessage = string.Format("Approved Delete: exchangeCode={0}|exchangeIpAddress={1}|localIpAddress={2}" +
                                        "|localPort={3}|exchangeType={4}|exchangeType={5}|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
                                                              dt[0].ExchangeCode,
                                                              dt[0].ExchangeIPAddress,
                                                              dt[0].LocalIPAddress,
                                                              dt[0].LocalPort,
                                                              dt[0].ApprovalDesc,
                                                              dt[0].ExchangeType,
                                                              dt[0].ExchangeName,
                                                              dt[0].TenderFlag,
                                                              dt[0].TransferFlag);
                        }
                        string ActionFlagDesc = "";
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                        //add audit trail
                        AuditTrail.AddAuditTrail("Exchange", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                    }
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                errorMessage = "Record is already exist or in pending approval";
            }
            throw new ApplicationException(errorMessage);
        }
    }

    //Approved data 
    public static void ApproveExchange(decimal exchangeId, string userName, string approvalDesc, string lastUpdateBy,
                                       DateTime lastUpdateDate)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();

        try
        {
            try
            {
                ta.FillByExchangeID(dt, exchangeId);
                
                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";
                  
                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApprovedProposedItem(dt[0].ExchangeCode, "A", dt[0].ExchangeIPAddress,
                                               dt[0].LocalIPAddress, dt[0].LocalPort, lastUpdateBy, lastUpdateDate, 
                                               approvalDesc, dt[0].ExchangeType, dt[0].ExchangeName,
                                                null, dt[0].IsTenderFlagNull() ? "" : dt[0].TenderFlag, dt[0].IsTransferFlagNull() ? "" : dt[0].TransferFlag,
                                                null, dt[0].ExchangeIPAddressOutbound, dt[0].LocalIPAddressOutbound, dt[0].LocalPortOutbound, dt[0].ExchangeId);

                        logMessage = string.Format("Approved Insert: exchangeCode={0}|exchangeIpaddress={1}|" +
                                            "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
                                            "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
                                                              dt[0].ExchangeCode,
                                                              dt[0].ExchangeIPAddress,
                                                              dt[0].LocalIPAddress,
                                                              dt[0].LocalPort,
                                                              approvalDesc,
                                                              dt[0].ExchangeType,
                                                              dt[0].ExchangeName,
                                                              dt[0].IsTenderFlagNull() ? "" : dt[0].TenderFlag,
                                                              dt[0].IsTransferFlagNull() ? "" : dt[0].TransferFlag);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].ExchangeCode, "A", dt[0].ExchangeIPAddress,
                                                       dt[0].LocalIPAddress, dt[0].LocalPort, userName,
                                                       lastUpdateDate, approvalDesc, dt[0].ExchangeType, dt[0].ExchangeName,
                                                        null, dt[0].IsTenderFlagNull() ? "" : dt[0].TenderFlag, dt[0].IsTransferFlagNull() ? "" : dt[0].TransferFlag, 
                                                        dt[0].ExchangeIPAddressOutbound, dt[0].LocalIPAddressOutbound, dt[0].LocalPortOutbound, dt[0].OriginalId);
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].ExchangeId);
                        logMessage = string.Format("Approved Update: exchangeCode={0}|exchangeIpaddress={1}|" +
                                            "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
                                            "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
                                                              dt[0].ExchangeCode,
                                                              dt[0].ExchangeIPAddress,
                                                              dt[0].LocalIPAddress,
                                                              dt[0].LocalPort,
                                                              approvalDesc,
                                                              dt[0].ExchangeType,
                                                              dt[0].ExchangeName,
                                                              dt[0].IsTenderFlagNull() ? "" : dt[0].TenderFlag,
                                                              dt[0].IsTransferFlagNull() ? "" : dt[0].TransferFlag);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalId);
                        ta.DeleteProposedItem(dt[0].ExchangeId);
                        logMessage = string.Format("Approved Delete: exchangeCode={0}|exchangeIpaddress={1}|" +
                                            "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
                                            "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
                                                              dt[0].ExchangeCode,
                                                              dt[0].ExchangeIPAddress,
                                                              dt[0].LocalIPAddress,
                                                              dt[0].LocalPort,
                                                              approvalDesc,
                                                              dt[0].ExchangeType,
                                                              dt[0].ExchangeName,
                                                              dt[0].IsTenderFlagNull() ? "" : dt[0].TenderFlag,
                                                              dt[0].IsTransferFlagNull() ? "" : dt[0].TransferFlag);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Exchange", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                errorMessage = "Record is already exist. Please input new record.";
            }
            throw new ApplicationException(errorMessage);
        }
    }

    //Approved data list
    //public static void ApproveExchange(List<string> approvalList, string userName)
    //{
    //    ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
    //    ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();

    //    try
    //    {
    //        using (TransactionScope scope = new TransactionScope())
    //            foreach (string exchangeID in approvalList)
    //            {
    //                string logMessage = "";
    //                ta.FillByExchangeID(dt, Convert.ToDecimal(exchangeID));

    //                if (dt.Count > 0)
    //                {
    //                    //update record
    //                    if (dt[0].ActionFlag == "I")
    //                    {
    //                        ta.ApprovedProposedItem(dt[0].ExchangeCode, "A", dt[0].ExchangeIPAddress,
    //                                                dt[0].LocalIPAddress, dt[0].LocalPort, "", DateTime.Now, "",
    //                                                DateTime.Now, dt[0].ApprovalDesc, dt[0].ExchangeType, dt[0].ExchangeName,
    //                                                null, null, null, null,dt[0].ExchangeId);
    //                        logMessage = string.Format("Approved Insert: exchangeCode={0}|exchangeIpaddress={1}|" +
    //                                        "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
    //                                        "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
    //                                                          dt[0].ExchangeCode,
    //                                                          dt[0].ExchangeIPAddress,
    //                                                          dt[0].LocalIPAddress,
    //                                                          dt[0].LocalPort,
    //                                                          dt[0].ApprovalDesc,
    //                                                          dt[0].ExchangeType,
    //                                                          dt[0].ExchangeName,
    //                                                          dt[0].TenderFlag,
    //                                                          dt[0].TransferFlag);
    //                    }
    //                    else if (dt[0].ActionFlag == "U")
    //                    {
    //                        ta.ApprovedUpdateProposedItem(dt[0].ExchangeCode, "A", dt[0].ExchangeIPAddress,
    //                                                      dt[0].LocalIPAddress, dt[0].LocalPort, "", DateTime.Now, "",
    //                                                      DateTime.Now, dt[0].ApprovalDesc, dt[0].ExchangeType, dt[0].ExchangeName,
    //                                                      null, null, null, dt[0].OriginalId);
    //                        //delete proposed record
    //                        ta.DeleteProposedItem(dt[0].ExchangeId);
    //                        logMessage = string.Format("Approved Update: exchangeCode={0}|exchangeIpaddress={1}|" +
    //                                        "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
    //                                        "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
    //                                                          dt[0].ExchangeCode,
    //                                                          dt[0].ExchangeIPAddress,
    //                                                          dt[0].LocalIPAddress,
    //                                                          dt[0].LocalPort,
    //                                                          dt[0].ApprovalDesc,
    //                                                          dt[0].ExchangeType,
    //                                                          dt[0].ExchangeName,
    //                                                          dt[0].TenderFlag,
    //                                                          dt[0].TransferFlag);
    //                    }
    //                    else if (dt[0].ActionFlag == "D")
    //                    {
    //                        ta.DeleteProposedItem(dt[0].OriginalId);
    //                        ta.DeleteProposedItem(dt[0].ExchangeId);
    //                        logMessage = string.Format("Approved Delete: exchangeCode={0}|exchangeIpaddress={1}|" +
    //                                        "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
    //                                        "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
    //                                                          dt[0].ExchangeCode,
    //                                                          dt[0].ExchangeIPAddress,
    //                                                          dt[0].LocalIPAddress,
    //                                                          dt[0].LocalPort,
    //                                                          dt[0].ApprovalDesc,
    //                                                          dt[0].ExchangeType,
    //                                                          dt[0].ExchangeName,
    //                                                          dt[0].TenderFlag,
    //                                                          dt[0].TransferFlag);
    //                    }

    //                    //save to audittrail
    //                    AuditTrail.AddAuditTrail("Exchange", AuditTrail.APPROVE, logMessage, userName);
    //                }
    //                scope.Complete();
    //            }
    //    }

    //    catch (Exception ex)
    //    {
    //        string errorMessage = ex.Message;
    //        if (ex.Message.Contains("Violation of PRIMARY KEY"))
    //        {
    //            errorMessage = "Record is already exist or in pending approval";
    //        }
    //        throw new ApplicationException(errorMessage);
    //    }
    //}

    public static void RejectProposedExchange(decimal exchangeId, string userName)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByExchangeID(dt, exchangeId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Approved Delete: exchangeCode={0}|exchangeIpaddress={1}|"+
                                            "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}"+
                                            "|exchangeName={6}|tenderFlag={7}|transferFlag={8}",
                                                              dt[0].ExchangeCode,
                                                              dt[0].ExchangeIPAddress,
                                                              dt[0].LocalIPAddress,
                                                              dt[0].LocalPort,
                                                              dt[0].IsApprovalDescNull() ? "" : dt[0].ApprovalDesc,
                                                              dt[0].ExchangeType,
                                                              dt[0].ExchangeName,
                                                              dt[0].TenderFlag,
                                                              dt[0].TransferFlag);
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }
                ta.DeleteRejectItem(exchangeId);

                AuditTrail.AddAuditTrail("Exchange", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void RejectExchange(List<string> rejectedList, string userName)
    {
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeDataTableAdapters.ExchangeTableAdapter();
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string item in rejectedList)
                {
                    string logMessage = "";
                    ta.FillByExchangeID(dt, Convert.ToDecimal(item));
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Rejected: exchangeCode={0}|exchangeIpaddress={1}|" +
                                            "localIPAddress={2}|localPort={3}|approvalDesc={4}|exchangeType={5}" +
                                            "|exchangeName={6}",
                                                          dt[0].ExchangeCode,
                                                          dt[0].ExchangeIPAddress,
                                                          dt[0].LocalIPAddress,
                                                          dt[0].LocalPort,
                                                          dt[0].ApprovalDesc,
                                                          dt[0].ExchangeType,
                                                          dt[0].ExchangeName);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    ta.DeleteRejectItem(Convert.ToDecimal(item));
                    AuditTrail.AddAuditTrail("Exchange", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static ExchangeData.ExchangeDataTable GetExchangeByExchangeCodeAndApprovalStatus(string exchangeCode, string approvalStatus)
    {
        ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
        ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeTableAdapter();

        try
        {
            ta.FillByExchCodeAndApprovalStatus(dt, exchangeCode, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetExchangeType(decimal exchangeID)
    {
        try
        {
            ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeTableAdapter();
            return ta.GetExchangeTypeByID(exchangeID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string GetExchangeCode(decimal exchangeID)
    {
        try
        {
            ExchangeDataTableAdapters.ExchangeTableAdapter ta = new ExchangeTableAdapter();
            return ta.GetExchangeCodeByID(exchangeID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}

