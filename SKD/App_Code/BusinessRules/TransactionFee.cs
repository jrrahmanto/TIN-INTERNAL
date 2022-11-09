using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for TransactionFee
/// </summary>
public class TransactionFee
{
    public static BankData.TransactionFeeRow SelectTransactionFeeByTransactionFeeID(decimal transactionFeeID)
    {
        BankDataTableAdapters.TransactionFeeTableAdapter ta = new BankDataTableAdapters.TransactionFeeTableAdapter();
        BankData.TransactionFeeDataTable dt = new BankData.TransactionFeeDataTable();
        BankData.TransactionFeeRow dr = null;
        try
        {
            ta.FillByTransactionFeeID(dt, transactionFeeID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee data");
        }

    }

    //get all data from bank pdm by code and status
    public static BankData.TransactionFeeDataTable SelectTransactionFeeByCommodityCodeCmTypeAndStatus(Nullable<decimal> commodityID, string CMType, string approvalStatus)
    {
        BankDataTableAdapters.TransactionFeeTableAdapter ta = new BankDataTableAdapters.TransactionFeeTableAdapter();
        BankData.TransactionFeeDataTable dt = new BankData.TransactionFeeDataTable();

        try
        {
            ta.FillByCommodityCodeCMTypeAndStatus(dt, commodityID, CMType, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee data");
        }

    }

    public static void ProposeTransactionFee(decimal commodityID, string cmType, decimal upperLimit,
                                          decimal clearingFee, Nullable<decimal> exchangeFee,
                                          DateTime startDate, Nullable<DateTime> endDate,
                                          Nullable<decimal> compensationFund, Nullable<decimal> thirdPartyFee,
                                          string approvalDesc, string action, string userName, decimal OriginalID)
    {
        BankDataTableAdapters.TransactionFeeTableAdapter ta = new BankDataTableAdapters.TransactionFeeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                   ta.Insert(commodityID, startDate, "P", cmType, upperLimit, clearingFee,
                            exchangeFee, compensationFund, thirdPartyFee, userName, DateTime.Now, userName, DateTime.Now, endDate, 
                            approvalDesc, action, OriginalID);

                   string ActionFlagDesc = "";
                   switch (action)
                   {
                       case "I": ActionFlagDesc = "Insert"; break;
                       case "U": ActionFlagDesc = "Update"; break;
                       case "D": ActionFlagDesc = "Delete"; break;
                   }

                   logMessage = string.Format("Proposed Value: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|UpperLimit={4}|ClearingFee={5}|ExchangeFee={6}|CompensationFund={7}|ThirdPartyFee={8}",
                                            Convert.ToDecimal(commodityID), Convert.ToDecimal(upperLimit),
                                            cmType.ToString(), startDate.ToString("dd-MMM-yyyy"),
                                            Convert.ToDateTime(endDate).Date,Convert.ToDecimal(compensationFund),
                                            Convert.ToDecimal(clearingFee), Convert.ToDecimal(exchangeFee),
                                            Convert.ToDecimal(thirdPartyFee));
                   AuditTrail.AddAuditTrail("TransactionFee", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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

    public static void ApproveTransactionFee(decimal transactionFeeID, string userName, string approvalDesc)
    {
        BankDataTableAdapters.TransactionFeeTableAdapter ta = new BankDataTableAdapters.TransactionFeeTableAdapter();
        BankData.TransactionFeeDataTable dt = new BankData.TransactionFeeDataTable();

        try
        {
           try
            {
                ta.FillByTransactionFeeID(dt, transactionFeeID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevStartDate(dt[0].EffectiveStartDate,
                              Convert.ToDecimal(dt[0].CommodityID), dt[0].CMType, dt[0].OriginalID));
           
                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(Convert.ToDecimal(dt[0].CommodityID),
                   dt[0].CMType, dt[0].EffectiveStartDate, dt[0].OriginalID));

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByTransactionFeeId(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                   dt[0].CMType, dt[0].UpperLimit, dt[0].ClearingFee, dt[0].ExchangeFee,
                                                   dt[0].CompensationFund, dt[0].ThirdPartyFee, userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null, null, dt[0].TransactionFeeID);
                        }
                        else 
                        {
                            ta.ApprovedProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                            dt[0].CMType, dt[0].UpperLimit, dt[0].ClearingFee, dt[0].ExchangeFee,
                                            dt[0].CompensationFund, dt[0].ThirdPartyFee,
                                           userName, DateTime.Now, null, approvalDesc, null, null, dt[0].TransactionFeeID);
                        }
                 
                        logMessage = string.Format("Approved Insert: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|UpperLimit={4}|ClearingFee={5}|ExchangeFee={6}|CompensationFund={7}|ThirdPartyFee={8}",
                                                         dt[0].CommodityID,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                         dt[0].UpperLimit,
                                                         dt[0].ClearingFee,
                                                         dt[0].IsExchangeFeeNull() ? "" : dt[0].ExchangeFee.ToString(),
                                                         dt[0].IsCompensationFundNull() ? "" : dt[0].CompensationFund.ToString(),
                                                         dt[0].IsThirdPartyFeeNull() ? "" : dt[0].ThirdPartyFee.ToString());

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                       dt[0].CMType, dt[0].UpperLimit, dt[0].ClearingFee, dt[0].ExchangeFee,
                                                       dt[0].CompensationFund, dt[0].ThirdPartyFee,
                                                      userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null, dt[0].OriginalID);
                        }
                        else
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                      dt[0].CMType, dt[0].UpperLimit, dt[0].ClearingFee, dt[0].ExchangeFee,
                                                      dt[0].CompensationFund, dt[0].ThirdPartyFee,
                                                     userName, DateTime.Now, null, approvalDesc, null, dt[0].OriginalID);
                        }
                        
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].TransactionFeeID);
                        logMessage = string.Format("Approved Update: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|UpperLimit={4}|ClearingFee={5}|ExchangeFee={6}|CompensationFund={7}|ThirdPartyFee={8}",
                                                         dt[0].CommodityID,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                         dt[0].UpperLimit,
                                                         dt[0].ClearingFee,
                                                         dt[0].IsExchangeFeeNull() ? "" : dt[0].ExchangeFee.ToString(),
                                                         dt[0].IsCompensationFundNull() ? "" : dt[0].CompensationFund.ToString(),
                                                         dt[0].IsThirdPartyFeeNull() ? "" : dt[0].ThirdPartyFee.ToString());
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].TransactionFeeID);
                        logMessage = string.Format("Approved Delete: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|UpperLimit={4}|ClearingFee={5}|ExchangeFee={6}|CompensationFund={7}|ThirdPartyFee={8}",
                                                         dt[0].CommodityID,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                         dt[0].UpperLimit,
                                                         dt[0].ClearingFee,
                                                         dt[0].IsExchangeFeeNull() ? "" : dt[0].ExchangeFee.ToString(),
                                                         dt[0].IsCompensationFundNull() ? "" : dt[0].CompensationFund.ToString(),
                                                         dt[0].IsThirdPartyFeeNull() ? "" : dt[0].ThirdPartyFee.ToString());
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("TransactionFee", AuditTrail.APPROVE, logMessage, userName,"Approve " + ActionFlagDesc);

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

    public static void RejectProposedTransactionFee(decimal transactionFeeId, string userName, string approvalDesc)
    {
        BankDataTableAdapters.TransactionFeeTableAdapter ta = new BankDataTableAdapters.TransactionFeeTableAdapter();
        BankData.TransactionFeeDataTable dt = new BankData.TransactionFeeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByTransactionFeeID(dt, transactionFeeId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                   
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|UpperLimit={4}|ClearingFee={5}|ExchangeFee={6}|CompensationFund={7}|ThirdPartyFee={8}",
                                                        dt[0].CommodityID,
                                                        dt[0].EffectiveStartDate,
                                                        dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                        dt[0].CMType,
                                                        dt[0].UpperLimit,
                                                        dt[0].ClearingFee,
                                                        dt[0].IsExchangeFeeNull() ? "" : dt[0].ExchangeFee.ToString(),
                                                        dt[0].IsCompensationFundNull() ? "" : dt[0].CompensationFund.ToString(),
                                                        dt[0].IsThirdPartyFeeNull() ? "" : dt[0].ThirdPartyFee.ToString());
                }
                ta.DeleteRejectItem(transactionFeeId);

                AuditTrail.AddAuditTrail("TransactionFee", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }



    #region "--Transaction Fee Primer--"

    public static TransactionFeeData.TransactionFeePrimerRow SelectTransactionFeePrimerByTransactionFeePrimerID(decimal transactionFeePrimerID)
    {
        TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();
        TransactionFeeData.TransactionFeePrimerDataTable dt = new TransactionFeeData.TransactionFeePrimerDataTable();
        TransactionFeeData.TransactionFeePrimerRow dr = null;
        try
        {
            ta.FillByTransactionFeePrimerID(dt, transactionFeePrimerID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee primer data");
        }

    }

    //get all data from bank pdm by code and status
    public static TransactionFeeData.TransactionFeePrimerDataTable SelectTransactionFeePrimerByCommodityCodeCmTypeAndStatus(decimal commodityID, string CMType, string approvalStatus)
    {
        TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();
        TransactionFeeData.TransactionFeePrimerDataTable dt = new TransactionFeeData.TransactionFeePrimerDataTable();

        try
        {
            ta.FillByCMTypeAndApproval(dt, commodityID, CMType, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee primer data");
        }

    }

    public static void ProposeTransactionFeePrimer(decimal exchangeID, decimal commodityID, string cmType,decimal upperLimit, 
                                          decimal transClearingFee, decimal transExchangeFee,
                                          decimal transCompensationFund, decimal transThirdPartyFee,
                                          decimal csClearingFee, decimal csExchangeFee,
                                          decimal csCompensationFund, decimal csThirdPartyFee,
                                          DateTime startDate, Nullable<DateTime> endDate,
                                          string approvalDesc, string action, string userName, 
                                          decimal OriginalID)
    {
        TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(exchangeID, commodityID, startDate, "P", cmType, upperLimit, 0, transClearingFee, 
                          transExchangeFee, transCompensationFund, transThirdPartyFee, csClearingFee, csExchangeFee, csCompensationFund, csThirdPartyFee,
                         userName, DateTime.Now, userName, DateTime.Now, endDate,
                         approvalDesc, action, OriginalID);

                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                logMessage = string.Format("Proposed Value: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|"+
                                            "TransClearingFee={4}|TransExchangeFee={5}|" +
                                            "TransCompensationFund={6}|TransThirdPartyeFee={7}|" +
                                            "CSClearingFee={8}|CSExchangeFee={9}|" +
                                            "CSCompensationFund={10}|CSThirdPartyeFee={11}|",
                                         Convert.ToDecimal(commodityID), 
                                         startDate.ToString("dd-MMM-yyyy"),
                                         Convert.ToDateTime(endDate).Date, cmType.ToString(), 
                                         Convert.ToDecimal(transClearingFee),
                                         Convert.ToDecimal(transExchangeFee), 
                                         Convert.ToDecimal(transCompensationFund),
                                         Convert.ToDecimal(transThirdPartyFee), 
                                         Convert.ToDecimal(csClearingFee),
                                         Convert.ToDecimal(csExchangeFee), 
                                         Convert.ToDecimal(csCompensationFund),
                                         Convert.ToDecimal(csThirdPartyFee));
                AuditTrail.AddAuditTrail("TransactionFeePrimer", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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


    public static void ApproveTransactionFeePrimer(decimal transactionFeePrimerID, string userName, string approvalDesc)
    {
        TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();
        TransactionFeeData.TransactionFeePrimerDataTable dt = new TransactionFeeData.TransactionFeePrimerDataTable();

        try
        {
            try
            {
                ta.FillByTransactionFeePrimerID(dt, transactionFeePrimerID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevDate(dt[0].EffectiveStartDate,
                              Convert.ToDecimal(dt[0].CommodityID), dt[0].CMType, dt[0].OriginalID));

                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(Convert.ToDecimal(dt[0].CommodityID),
                   dt[0].CMType, dt[0].EffectiveStartDate, dt[0].OriginalID));

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByTransactionFeePrimerID(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                   dt[0].CMType,dt[0].UpperLimit, dt[0].TransClearingFee, 
                                                   dt[0].TransExchangeFee,dt[0].TransCompensationFund,dt[0].TransThirdPartyFee,
                                                   dt[0].CSClearingFee,dt[0].CSExchangeFee,dt[0].CSCompensationFund,dt[0].CSThirdPartyFee,
                                                   userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null, null, dt[0].TransactionFeePrimerID);
                        }
                        else
                        {
                            ta.ApprovedProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                            dt[0].CMType, dt[0].UpperLimit, dt[0].TransClearingFee,
                                                   dt[0].TransExchangeFee, dt[0].TransCompensationFund, dt[0].TransThirdPartyFee,
                                                   dt[0].CSClearingFee, dt[0].CSExchangeFee, dt[0].CSCompensationFund, dt[0].CSThirdPartyFee,
                                           userName, DateTime.Now, null, approvalDesc, null, null, dt[0].TransactionFeePrimerID);
                        }

                        logMessage = string.Format("Approved Insert: CommodityID={0}|EffectiveStartDate={1}|" +
                                                   "EffectiveEndDate={2}|CMType={3}|" +
                                                   "TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyeFee={7}|" +
                                                   "CSClearingFee={8}|CSExchangeFee={9}|" +
                                                   "CSCompensationFund={10}|CSThirdPartyeFee={11}|" ,
                                                    dt[0].CommodityID,
                                                    dt[0].EffectiveStartDate,
                                                    dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                    dt[0].CMType,dt[0].UpperLimit.ToString(),
                                                    dt[0].TransClearingFee.ToString(),
                                                    dt[0].TransExchangeFee.ToString(),
                                                    dt[0].TransCompensationFund.ToString(),
                                                    dt[0].TransThirdPartyFee.ToString(),
                                                    dt[0].CSClearingFee.ToString(),
                                                    dt[0].CSExchangeFee.ToString(),
                                                    dt[0].CSCompensationFund.ToString(),
                                                    dt[0].CSThirdPartyFee.ToString());
                        ta.UspTransactionFeePrimerUpdateTiering(dt[0].TransactionFeePrimerID);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                       dt[0].CMType,dt[0].UpperLimit, dt[0].TransClearingFee, 
                                                       dt[0].TransExchangeFee,dt[0].TransCompensationFund,
                                                       dt[0].TransThirdPartyFee,dt[0].CSClearingFee,
                                                       dt[0].CSExchangeFee,dt[0].CSCompensationFund,
                                                       dt[0].CSThirdPartyFee,
                                                      userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null, dt[0].OriginalID);
                        }
                        else
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                      dt[0].CMType,dt[0].UpperLimit, dt[0].TransClearingFee, 
                                                       dt[0].TransExchangeFee,dt[0].TransCompensationFund,
                                                       dt[0].TransThirdPartyFee,dt[0].CSClearingFee,
                                                       dt[0].CSExchangeFee,dt[0].CSCompensationFund,
                                                       dt[0].CSThirdPartyFee,
                                                     userName, DateTime.Now, null, approvalDesc, null, dt[0].OriginalID);
                        }

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].TransactionFeePrimerID);
                        logMessage = string.Format("Approved Update: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|"+
                                                    "CMType={3}|" + 
                                                    "TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyeFee={7}|" +
                                                   "CSClearingFee={8}|CSExchangeFee={9}|" +
                                                   "CSCompensationFund={10}|CSThirdPartyeFee={11}|" ,
                                                    dt[0].CommodityID,
                                                    dt[0].EffectiveStartDate,
                                                    dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                    dt[0].CMType,dt[0].UpperLimit.ToString(),
                                                    dt[0].TransClearingFee.ToString(),
                                                    dt[0].TransExchangeFee.ToString(),
                                                    dt[0].TransCompensationFund.ToString(),
                                                    dt[0].TransThirdPartyFee.ToString(),
                                                    dt[0].CSClearingFee.ToString(),
                                                    dt[0].CSExchangeFee.ToString(),
                                                    dt[0].CSCompensationFund.ToString(),
                                                    dt[0].CSThirdPartyFee.ToString());
                        ta.UspTransactionFeePrimerUpdateTiering(dt[0].TransactionFeePrimerID);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.UspTransactionFeePrimerDeleteTiering(dt[0].TransactionFeePrimerID);
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].TransactionFeePrimerID);
                        logMessage = string.Format("Approved Delete: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|"+
                                                    "CMType={3}|" + 
                                                    "TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyeFee={7}|" +
                                                   "CSClearingFee={8}|CSExchangeFee={9}|" +
                                                   "CSCompensationFund={10}|CSThirdPartyeFee={11}|" ,
                                                    dt[0].CommodityID,
                                                    dt[0].EffectiveStartDate,
                                                    dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                    dt[0].CMType,dt[0].UpperLimit.ToString(),
                                                    dt[0].TransClearingFee.ToString(),
                                                    dt[0].TransExchangeFee.ToString(),
                                                    dt[0].TransCompensationFund.ToString(),
                                                    dt[0].TransThirdPartyFee.ToString(),
                                                    dt[0].CSClearingFee.ToString(),
                                                    dt[0].CSExchangeFee.ToString(),
                                                    dt[0].CSCompensationFund.ToString(),
                                                    dt[0].CSThirdPartyFee.ToString());
                        
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("TransactionFeePrimer", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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


    public static void RejectProposedTransactionFeePrimer(decimal transactionFeePrimerId, string userName, string approvalDesc)
    {
        TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();
        TransactionFeeData.TransactionFeePrimerDataTable dt = new TransactionFeeData.TransactionFeePrimerDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByTransactionFeePrimerID(dt, transactionFeePrimerId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|"+ 
                                                    "TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyeFee={7}|" +
                                                   "CSClearingFee={8}|CSExchangeFee={9}|" +
                                                   "CSCompensationFund={10}|CSThirdPartyeFee={11}|" ,
                                                    dt[0].CommodityID,
                                                    dt[0].EffectiveStartDate,
                                                    dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                    dt[0].CMType,dt[0].UpperLimit.ToString(),
                                                    dt[0].TransClearingFee.ToString(),
                                                    dt[0].TransExchangeFee.ToString(),
                                                    dt[0].TransCompensationFund.ToString(),
                                                    dt[0].TransThirdPartyFee.ToString(),
                                                    dt[0].CSClearingFee.ToString(),
                                                    dt[0].CSExchangeFee.ToString(),
                                                    dt[0].CSCompensationFund.ToString(),
                                                    dt[0].CSThirdPartyFee.ToString());
                }
                ta.DeleteRejectItem(transactionFeePrimerId);

                AuditTrail.AddAuditTrail("TransactionFeePrimer", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


    #endregion




    #region "--- Transaction Fee SPA ---"

    public static TransactionFeeData.TransactionFeeSPARow SelectTransactionFeeSPAByTransactionFeeSPAID(decimal transactionFeeSPAID)
    {
        TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();
        TransactionFeeData.TransactionFeeSPADataTable dt = new TransactionFeeData.TransactionFeeSPADataTable();
        TransactionFeeData.TransactionFeeSPARow dr = null;
        try
        {
            ta.FillByTransactionFeeSPAID(dt, transactionFeeSPAID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee spa data");
        }

    }

    //get data from Contract Lookup
    public static TransactionFeeData.ContractLookupRow SelectContractLookupByContractID(decimal ContractID)
    {
        TransactionFeeDataTableAdapters.ContractLookupTableAdapter ta = new TransactionFeeDataTableAdapters.ContractLookupTableAdapter();
        TransactionFeeData.ContractLookupDataTable dt = new TransactionFeeData.ContractLookupDataTable();
        TransactionFeeData.ContractLookupRow dr = null;
        try
        {
            ta.FillByContractID(dt, ContractID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }
            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load contract ref kurs data");
        }
    }

    //get all data from bank pdm by code and status
    public static TransactionFeeData.TransactionFeeSPADataTable SelectTransactionFeeSPAByCmTypeAndStatus(string CMType, string approvalStatus)
    {
        TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();
        TransactionFeeData.TransactionFeeSPADataTable dt = new TransactionFeeData.TransactionFeeSPADataTable();

        try
        {
            ta.FillByCMTypeAndApproval(dt, CMType, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load transaction fee spa data");
        }

    }
    
    public static void ProposeTransactionFeeSPA(decimal exchangeID, string ModeFee, string cmType, decimal UpperLimit,
                                         string ModeKurs, decimal FixedKurs,long ContractRefKurs,
                                         decimal transClearingFee, decimal transExchangeFee,
                                         decimal transCompensationFund, decimal transThirdPartyFee,
                                         decimal PctClearingFee, decimal PctExchangeFee, 
                                         decimal PctCompensationFee, decimal PctThirdPartyFee,
                                         decimal csClearingFee, decimal csExchangeFee,
                                         decimal csCompensationFund, decimal csThirdPartyFee,
                                         DateTime startDate, Nullable<DateTime> endDate,                                        
                                         string approvalDesc, string action, string userName, decimal OriginalID)
    {
        TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();

        try
        {
            //PctClearingFee = 0;
            //PctExchangeFee = 0;
            //PctCompensationFee = 0;
            //PctThirdPartyFee = 0;
            //transClearingFee = 0;
            //transExchangeFee = 0;
            //transCompensationFund = 0;
            //transThirdPartyFee = 0;
            //csClearingFee = 0;
            //csExchangeFee = 0;
            //csCompensationFund = 0;
            //csThirdPartyFee = 0;

            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(cmType, startDate, "P", UpperLimit,  transClearingFee, transExchangeFee,
                            transCompensationFund,transThirdPartyFee, userName, DateTime.Now, userName, startDate,
                            endDate,approvalDesc,action,OriginalID,csClearingFee,csExchangeFee,csCompensationFund,
                            csThirdPartyFee,0,exchangeID,ModeFee, PctClearingFee,PctExchangeFee, PctCompensationFee,
                            PctThirdPartyFee, ModeKurs, FixedKurs, ContractRefKurs);

                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                logMessage = string.Format("Proposed Value: UpperLimit={0}|EffectiveStartDate={1}|"+
                                           "EffectiveEndDate={2}|CMType={3}" +
                                           "|TransClearingFee={4}|TransExchangeFee={5}|"+
                                           "TransCompensationFund={6}|TransThirdPartyFee={7}"+
                                           "|csClearingFee={8}|csExchangeFee={9}|" +
                                           "csCompensationFund={10}|csThirdPartyFee={11}",
                                           UpperLimit.ToString(),
                                           startDate.ToString("dd-MMM-yyyy"),
                                           Convert.ToDateTime(endDate).Date, 
                                           cmType.ToString(), 
                                           transClearingFee.ToString(), 
                                           transExchangeFee.ToString(),
                                           transCompensationFund.ToString(),
                                           transThirdPartyFee.ToString(),
                                           csClearingFee.ToString(),
                                           csExchangeFee.ToString(),
                                           csCompensationFund.ToString(),
                                           csThirdPartyFee.ToString());
                AuditTrail.AddAuditTrail("TransactionFeeSPA", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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


    public static void ApproveTransactionFeeSPA(decimal transactionFeeSPAID, string userName, string approvalDesc)
    {
        TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();
        TransactionFeeData.TransactionFeeSPADataTable dt = new TransactionFeeData.TransactionFeeSPADataTable();

        try
        {
            try
            {
                ta.FillByTransactionFeeSPAID(dt, transactionFeeSPAID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevDate(dt[0].EffectiveStartDate, dt[0].CMType, dt[0].UpperLimit, dt[0].OriginalID));

                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(dt[0].CMType, dt[0].EffectiveStartDate, dt[0].UpperLimit, dt[0].OriginalID));

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByTransactionFeeSPAID(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedProposedItem(dt[0].CMType, dt[0].EffectiveStartDate, "A", dt[0].UpperLimit,
                                                    dt[0].TransClearingFee, dt[0].TransExchangeFee,
                                                   dt[0].TransCompensationFund, dt[0].TransThirdPartyFee, 
                                                   dt[0].CSClearingFee,dt[0].CSExchangeFee,dt[0].CSCompensationFund,
                                                   dt[0].CSThirdPartyFee,userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null, null,dt[0].ModeFee,
                                                   dt[0].PctClearingFee,dt[0].PctExchangeFee,dt[0].PctCompensationFund,dt[0].PctThirdPartyFee,
                                                   dt[0].ModeKurs, dt[0].FixedKurs,dt[0].ContractRefKurs,dt[0].TransactionFeeSPAID);
                        }
                        else
                        {
                            ta.ApprovedProposedItem(dt[0].CMType, dt[0].EffectiveStartDate, "A",dt[0].UpperLimit,
                                                dt[0].TransClearingFee, dt[0].TransExchangeFee,
                                                   dt[0].TransCompensationFund, dt[0].TransThirdPartyFee,
                                                   dt[0].CSClearingFee, dt[0].CSExchangeFee, dt[0].CSCompensationFund,
                                                   dt[0].CSThirdPartyFee, userName, DateTime.Now, null, approvalDesc, null, null, dt[0].ModeFee,
                                                   dt[0].PctClearingFee, dt[0].PctExchangeFee, dt[0].PctCompensationFund, dt[0].PctThirdPartyFee,
                                                   dt[0].ModeKurs, dt[0].FixedKurs, dt[0].ContractRefKurs,dt[0].TransactionFeeSPAID);
                        }

                        logMessage = string.Format("Approved Insert: UpperLimit={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}"+
                                                     "|TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyFee={7}" +
                                                   "|csClearingFee={8}|csExchangeFee={9}|" +
                                                   "csCompensationFund={10}|csThirdPartyFee={11}",
                                                         dt[0].UpperLimit,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                        dt[0].TransClearingFee.ToString(),
                                                        dt[0].TransExchangeFee.ToString(),
                                                        dt[0].TransCompensationFund.ToString(),
                                                        dt[0].TransThirdPartyFee.ToString(),
                                                        dt[0].CSClearingFee.ToString(),
                                                        dt[0].CSExchangeFee.ToString(),
                                                        dt[0].CSCompensationFund.ToString(),
                                                        dt[0].CSThirdPartyFee.ToString());
                        ta.UspTransactionFeeSPAUpdateTiering(dt[0].TransactionFeeSPAID);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].EffectiveStartDate, "A",
                                                       dt[0].CMType, dt[0].UpperLimit, dt[0].TransClearingFee, 
                                                       dt[0].TransExchangeFee,dt[0].TransCompensationFund,
                                                       dt[0].TransThirdPartyFee, dt[0].CSClearingFee,dt[0].CSExchangeFee,dt[0].CSCompensationFund,
                                                      dt[0].CSThirdPartyFee, userName, DateTime.Now, dt[0].EffectiveEndDate, approvalDesc, null,dt[0].ModeFee,
                                                      dt[0].PctClearingFee,dt[0].PctExchangeFee,dt[0].PctCompensationFund, dt[0].PctThirdPartyFee,
                                                      dt[0].ModeKurs, dt[0].FixedKurs, dt[0].ContractRefKurs,dt[0].OriginalID);
                        }
                        else
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].EffectiveStartDate, "A",
                                                      dt[0].CMType, dt[0].UpperLimit, dt[0].TransClearingFee,
                                                       dt[0].TransExchangeFee, dt[0].TransCompensationFund,
                                                       dt[0].TransThirdPartyFee, dt[0].CSClearingFee, dt[0].CSExchangeFee, dt[0].CSCompensationFund,
                                                      dt[0].CSThirdPartyFee,
                                                     userName, DateTime.Now, null, approvalDesc, null,dt[0].ModeFee, dt[0].PctClearingFee,
                                                     dt[0].PctExchangeFee,dt[0].PctCompensationFund,dt[0].PctThirdPartyFee, dt[0].ModeKurs,
                                                     dt[0].FixedKurs,dt[0].ContractRefKurs, dt[0].OriginalID);
                        }

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].TransactionFeeSPAID);
                        logMessage = string.Format("Approved Update: UpperLimit={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|"+
                                                 "|TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyFee={7}" +
                                                   "|csClearingFee={8}|csExchangeFee={9}|" +
                                                   "csCompensationFund={10}|csThirdPartyFee={11}",
                                                         dt[0].UpperLimit,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                        dt[0].TransClearingFee.ToString(),
                                                        dt[0].TransExchangeFee.ToString(),
                                                        dt[0].TransCompensationFund.ToString(),
                                                        dt[0].TransThirdPartyFee.ToString(),
                                                        dt[0].CSClearingFee.ToString(),
                                                        dt[0].CSExchangeFee.ToString(),
                                                        dt[0].CSCompensationFund.ToString(),
                                                        dt[0].CSThirdPartyFee.ToString());
                        ta.UspTransactionFeeSPAUpdateTiering(dt[0].TransactionFeeSPAID);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.UspTransactionFeeSPADeleteTiering(dt[0].TransactionFeeSPAID);
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].TransactionFeeSPAID);
                        logMessage = string.Format("Approved Delete: UpperLimit={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|" + "|TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyFee={7}" +
                                                   "|csClearingFee={8}|csExchangeFee={9}|" +
                                                   "csCompensationFund={10}|csThirdPartyFee={11}",
                                                         dt[0].UpperLimit,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                        dt[0].TransClearingFee.ToString(),
                                                        dt[0].TransExchangeFee.ToString(),
                                                        dt[0].TransCompensationFund.ToString(),
                                                        dt[0].TransThirdPartyFee.ToString(),
                                                        dt[0].CSClearingFee.ToString(),
                                                        dt[0].CSExchangeFee.ToString(),
                                                        dt[0].CSCompensationFund.ToString(),
                                                        dt[0].CSThirdPartyFee.ToString());
                        
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("TransactionFeeSPA", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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

    public static void RejectProposedTransactionFeeSPA(decimal transactionFeeSPAId, string userName, string approvalDesc)
    {
        TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();
        TransactionFeeData.TransactionFeeSPADataTable dt = new TransactionFeeData.TransactionFeeSPADataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByTransactionFeeSPAID(dt, transactionFeeSPAId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : UpperLimit={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|CMType={3}|" + "|TransClearingFee={4}|TransExchangeFee={5}|" +
                                                   "TransCompensationFund={6}|TransThirdPartyFee={7}" +
                                                   "|csClearingFee={8}|csExchangeFee={9}|" +
                                                   "csCompensationFund={10}|csThirdPartyFee={11}",
                                                         dt[0].UpperLimit,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                                         dt[0].CMType,
                                                        dt[0].TransClearingFee.ToString(),
                                                        dt[0].TransExchangeFee.ToString(),
                                                        dt[0].TransCompensationFund.ToString(),
                                                        dt[0].TransThirdPartyFee.ToString(),
                                                        dt[0].CSClearingFee.ToString(),
                                                        dt[0].CSExchangeFee.ToString(),
                                                        dt[0].CSCompensationFund.ToString(),
                                                        dt[0].CSThirdPartyFee.ToString());
                }
                ta.DeleteRejectItem(transactionFeeSPAId);

                AuditTrail.AddAuditTrail("TransactionFeeSPA", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }




    #endregion

}




