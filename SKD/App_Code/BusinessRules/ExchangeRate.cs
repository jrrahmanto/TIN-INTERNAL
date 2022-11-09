using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for ExchangeRate
/// </summary>
/// 
public class ExchangeRate
{

    public static ExchangeRateData.ExchangeRateDataTable SelectExchangeRateByExchangeRateID(ExchangeRateData.ExchangeRateDataTable dt,
                                                                                            decimal exchRateID)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
       
        try
        {
            ta.FillByExchangeRateID(dt,  exchRateID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange rate data");
        }
    }

    public static ExchangeRateData.ExchangeRateRow SelectExchangeRateByExchangeRateID(decimal exchRateID)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        ExchangeRateData.ExchangeRateRow dr = null;
        try
        {
            ta.FillByExchangeRateID(dt, exchRateID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange rate data");
        }
    }

    public static ExchangeRateData.ExchangeRateDataTable SelectExchangeRateByAction(Nullable<DateTime> EffectiveDate, string Approval,string exchRateType)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();

        try
        {
            ta.FillByExchTypeAndApprovalStatus(dt,exchRateType,Approval, EffectiveDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange rate data");
        }
    }


    public static void ProposeExchangeRate(decimal sourceCurrency, decimal destinationCurrency,
                                          DateTime startDate, DateTime endDate, string createdBy,
                                          DateTime createdDate, string lastUpdateBy, DateTime lastUpdateDate,
                                          string exchangeRateType, decimal rate, decimal originalID,
                                          string approvalDesc, string action, string userName)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta2 = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
       
        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                // Guard for editing proposed record
                if (action != "I")
                {
                    ExchangeRateData.ExchangeRateRow dr = ExchangeRate.SelectExchangeRateByExchangeRateID(originalID);
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be edited. Please wait for checker approval.");
                }
               
                ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
                decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(startDate,sourceCurrency,
                                                            destinationCurrency,originalID));
                if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");


                ta.Insert(sourceCurrency, destinationCurrency, startDate.Date, endDate.Date,
                      "P", exchangeRateType, rate, createdBy, createdDate,lastUpdateBy, lastUpdateDate, approvalDesc, originalID, action);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value, Source currency:{0}| Destination currency:{1}" +
                                           "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                           "| Rate: {5}", 
                                            sourceCurrency.ToString()
                                            ,destinationCurrency.ToString(),
                                            startDate.ToString("dd-MM-yyyy"),
                                            endDate.ToString("dd-MM-yyyy"),
                                            exchangeRateType,rate.ToString());
                AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);

               
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist or in pending approval.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static void ProposedDeletedExchangeRate(List<string> deletedList, string userName)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                {
                    foreach (string item in deletedList)
                    {
                        //proposed delete
                        ta.FillByExchangeRateID(dt, Convert.ToDecimal(item));
                        if (dt.Count > 0)
                        {
                            ta.Insert(dt[0].SourceCurrID, dt[0].DestinationCurrID, dt[0].ExchRateStartDate, dt[0].ExchRateEndDate,
                             "P", dt[0].ExchRateType, dt[0].Rate, "", DateTime.Now, "", DateTime.Now, "", dt[0].ExchangeRateID, "D");
                            logMessage = string.Format("Proposed Delete, Source currency:{0}| Destination currency:{1}" +
                                                       "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                       "| Rate: {5}",
                                                        dt[0].SourceCurrID,
                                                        dt[0].DestinationCurrID,
                                                        dt[0].ExchRateStartDate,
                                                        dt[0].ExchRateEndDate,
                                                        dt[0].ExchRateType,
                                                        dt[0].Rate);
                        }
                        //add audit trail
                        AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.PROPOSE, logMessage, userName,"Delete");
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

    public static void ApproveExchangeRate(decimal exchangeRateID, string userName, string approvalDesc)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
       
        try
        {
            //check effective start date proposed item
            //if in between
            //1. if action insert new then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //2. if action updated then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //3. if action deleted then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            
            try
            {
                ta.FillByExchangeRateID(dt,exchangeRateID);

                decimal prevOriginalID = Convert.ToDecimal(ta.getOriginalIDPrevStartDate(dt[0].ExchRateStartDate,
                                         Convert.ToDecimal(dt[0].SourceCurrID), Convert.ToDecimal(dt[0].DestinationCurrID),dt[0].OriginalID));
                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(dt[0].ExchRateStartDate,
                                         Convert.ToDecimal(dt[0].SourceCurrID), Convert.ToDecimal(dt[0].DestinationCurrID), dt[0].OriginalID));
 
                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByExchangeRateID(dt[0].ExchRateStartDate.AddDays(-1),prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].ExchRateEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApprovedProposedItem(dt[0].SourceCurrID, dt[0].DestinationCurrID, dt[0].ExchRateStartDate, 
                                                dt[0].ExchRateEndDate, "A", dt[0].ExchRateType, dt[0].Rate, 
                                                userName, DateTime.Now, approvalDesc, null, null, dt[0].ExchangeRateID);

                        logMessage = string.Format("Approved Insert, Source currency:{0}| Destination currency:{1}" +
                                                       "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                       "| Rate: {5}",
                                                           dt[0].SourceCurrID,
                                                           dt[0].DestinationCurrID,
                                                           dt[0].ExchRateStartDate,
                                                           dt[0].ExchRateEndDate,
                                                           dt[0].ExchRateType,
                                                           dt[0].Rate);
                    }else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].SourceCurrID, dt[0].DestinationCurrID, dt[0].ExchRateStartDate,
                                                      dt[0].ExchRateEndDate, "A", dt[0].ExchRateType, dt[0].Rate, userName,
                                                      DateTime.Now, approvalDesc, null, null, dt[0].OriginalID);
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].ExchangeRateID);

                        logMessage = string.Format("Approved Update, Source currency:{0}| Destination currency:{1}" +
                                                       "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                       "| Rate: {5}",
                                                           dt[0].SourceCurrID,
                                                           dt[0].DestinationCurrID,
                                                           dt[0].ExchRateStartDate,
                                                           dt[0].ExchRateEndDate,
                                                           dt[0].ExchRateType,
                                                           dt[0].Rate);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].ExchangeRateID);

                        logMessage = string.Format("Approved Delete: , Source currency:{0}| Destination currency:{1}" +
                                                       "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                       "| Rate: {5}",
                                                           dt[0].SourceCurrID,
                                                           dt[0].DestinationCurrID,
                                                           dt[0].ExchRateStartDate,
                                                           dt[0].ExchRateEndDate,
                                                           dt[0].ExchRateType,
                                                           dt[0].Rate);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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
                errorMessage = "Record is already exist or in pending approval";
            }
            throw new ApplicationException(errorMessage);
        }
    }

    public static void ApproveExchangeRate(List<string> approvalList, string userName)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        
       
        try
        {
            //check effective start date proposed item
            //if in between
            //1. if action insert new then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //2. if action updated then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //3. if action deleted then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string exchangeRateID in approvalList)
                {
                    string logMessage = "";
                    ta.FillByExchangeRateID(dt, Convert.ToDecimal(exchangeRateID));
                    if (dt.Count > 0)
                    {
                        {
                            //update record
                            if (dt[0].ActionFlag == "I")
                            {
                                ta.ApprovedProposedItem(dt[0].SourceCurrID, dt[0].DestinationCurrID, dt[0].ExchRateStartDate,
                                                        dt[0].ExchRateEndDate, "A", dt[0].ExchRateType, dt[0].Rate, userName,
                                                        DateTime.Now, "", null, null, dt[0].ExchangeRateID);
                                logMessage = string.Format("Approved Insert: {0}|{1}|{2}|{3}|{4}|{5}",
                                                               dt[0].SourceCurrID,
                                                               dt[0].DestinationCurrID,
                                                               dt[0].ExchRateStartDate,
                                                               dt[0].ExchRateEndDate,
                                                               dt[0].ExchRateType,
                                                               dt[0].Rate);
                            }
                            else if (dt[0].ActionFlag == "U")
                            {
                                ta.ApprovedUpdateProposedItem(dt[0].SourceCurrID, dt[0].DestinationCurrID, dt[0].ExchRateStartDate,
                                                              dt[0].ExchRateEndDate, "A", dt[0].ExchRateType, dt[0].Rate, "",
                                                              DateTime.Now, "", null, null, dt[0].OriginalID);
                                //delete proposed record
                                ta.DeleteProposedItem(dt[0].ExchangeRateID);
                                logMessage = string.Format("Approved Update: {0}|{1}|{2}|{3}|{4}|{5}",
                                                               dt[0].SourceCurrID,
                                                               dt[0].DestinationCurrID,
                                                               dt[0].ExchRateStartDate,
                                                               dt[0].ExchRateEndDate,
                                                               dt[0].ExchRateType,
                                                               dt[0].Rate);
                            }
                            else if (dt[0].ActionFlag == "D")
                            {
                                ta.DeleteProposedItem(dt[0].OriginalID);
                                ta.DeleteProposedItem(dt[0].ExchangeRateID);
                                logMessage = string.Format("Approved Delete: {0}|{1}|{2}|{3}|{4}|{5}",
                                                               dt[0].SourceCurrID,
                                                               dt[0].DestinationCurrID,
                                                               dt[0].ExchRateStartDate,
                                                               dt[0].ExchRateEndDate,
                                                               dt[0].ExchRateType,
                                                               dt[0].Rate);
                            }
                        }
                        string ActionFlagDesc = "";
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                        AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);
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

    public static void RejectProposedExchangeRate(decimal exchangeRateID, string userName)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByExchangeRateID(dt, exchangeRateID);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Rejected, Source currency:{0}| Destination currency:{1}" +
                                                       "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                       "| Rate: {5}",
                                                       dt[0].SourceCurrID,
                                                       dt[0].DestinationCurrID,
                                                       dt[0].ExchRateStartDate,
                                                       dt[0].ExchRateEndDate,
                                                       dt[0].ExchRateType,
                                                       dt[0].Rate);
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }
               
                ta.DeleteRejectItem(exchangeRateID);

                AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.REJECT, logMessage, userName,"Reject " + ActionFlagDesc);
                scope.Complete();
            }
          
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void RejectExchangeRate(List<string> rejectedList, string userName)
    {
        ExchangeRateDataTableAdapters.ExchangeRateTableAdapter ta = new ExchangeRateDataTableAdapters.ExchangeRateTableAdapter();
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string item in rejectedList)
                {
                    string logMessage = "";
                    ta.FillByExchangeRateID(dt, Convert.ToDecimal(item));
                    string ActionFlagDesc = "";
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Rejected, Source currency:{0}| Destination currency:{1}" +
                                                   "| Effective start date: {2}| Effective end date: {3}| Exchange Rate Type: {4}" +
                                                   "| Rate: {5}",
                                                           dt[0].SourceCurrID,
                                                           dt[0].DestinationCurrID,
                                                           dt[0].ExchRateStartDate,
                                                           dt[0].ExchRateEndDate,
                                                           dt[0].ExchRateType,
                                                           dt[0].Rate);
                        
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                    }
                    ta.DeleteRejectItem(Convert.ToDecimal(item));
                    AuditTrail.AddAuditTrail("ExchangeRate", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    #region supporting method

    public static ExchangeRateData.CurrencyDataTable getCurrency()
    {
        ExchangeRateDataTableAdapters.CurrencyTableAdapter ta = new ExchangeRateDataTableAdapters.CurrencyTableAdapter();
        ExchangeRateData.CurrencyDataTable dt = new ExchangeRateData.CurrencyDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load currency code data");
        }
    }
     

    #endregion
  


}
