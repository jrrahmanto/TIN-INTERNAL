using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for Fee
/// </summary>
public class Fee
{
    //get all data from fee by code and status
    public static FeeData.FeeDataTable SelectFeeByCommodityCodeAndStatus(Nullable<decimal> commodityID, string approvalStatus)
    {
        FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();
        FeeData.FeeDataTable dt = new FeeData.FeeDataTable();

        try
        {
            ta.FillByCommodityAndStatus(dt, commodityID, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load fee data");
        }

    }

    public static FeeData.FeeRow SelectFeeByFeeID(decimal FeeID)
    {
        FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();
        FeeData.FeeDataTable dt = new FeeData.FeeDataTable();
        FeeData.FeeRow dr = null;
        try
        {
            ta.FillByFeeID(dt, FeeID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load fee data");
        }

    }


    public static void ProposeFee(decimal commodityID, Nullable<decimal> tnClearingFee,
                                          Nullable<decimal> tnExchangeFee, Nullable<decimal> tnCompensationFund,
                                          DateTime startDate, Nullable<DateTime> endDate,
                                          Nullable<decimal> tnThirdPartyFee, Nullable<decimal> clearingFund,
                                          string approvalDesc, string action, string userName, decimal OriginalID)
    {
        FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(commodityID, startDate, "P", tnClearingFee, tnExchangeFee, tnCompensationFund, tnThirdPartyFee,
                            clearingFund, userName, DateTime.Now, 
                         userName, DateTime.Now, approvalDesc,endDate , action, OriginalID);

                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                logMessage = string.Format("Proposed Value: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|tnClearingFee={3}" +
                                           "|tnExchangeFee={4}|tnCompensationFund={5}|tnThirdPartyFee={6}|clearingFund={7}",
                                         Convert.ToDecimal(commodityID), startDate.ToString("dd-MMM-yyyy"),
                                         Convert.ToDateTime(endDate).Date, Convert.ToDecimal(tnClearingFee),
                                         Convert.ToDecimal(tnExchangeFee), Convert.ToDecimal(tnCompensationFund),
                                         Convert.ToDecimal(tnThirdPartyFee),  Convert.ToDecimal(clearingFund));
                AuditTrail.AddAuditTrail("Fee", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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

    public static void ApproveFee(decimal feeID, string userName, string approvalDesc)
    {
        FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();
        FeeData.FeeDataTable dt = new FeeData.FeeDataTable();

        try
        {
            try
            {
                ta.FillByFeeID(dt, feeID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevStartDate(dt[0].EffectiveStartDate,
                              Convert.ToDecimal(dt[0].CommodityID), dt[0].OriginalID));

                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(Convert.ToDecimal(dt[0].CommodityID),
                            dt[0].EffectiveStartDate, dt[0].OriginalID));

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByFeeId(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
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
                                                   dt[0].TNClearingFee, dt[0].TNExchangeFee, dt[0].TNCompensationFund, dt[0].TNThirdPartyFee,
                                                   dt[0].ClearingFund, userName, DateTime.Now, approvalDesc, dt[0].EffectiveEndDate,  
                                                   null, null, dt[0].FeeID);
                        }
                        else
                        {
                            ta.ApprovedProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                  dt[0].TNClearingFee, dt[0].TNExchangeFee, dt[0].TNCompensationFund, dt[0].TNThirdPartyFee,
                                                  dt[0].ClearingFund, userName, DateTime.Now, approvalDesc, null,
                                                  null, null, dt[0].FeeID);
                        }

                        logMessage = string.Format("Approved Insert: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|tnClearingFee={3}" +
                                           "|tnExchangeFee={4}|tnCompensationFund={5}|tnThirdPartyFee={6}|clearingFund={7}",
                                            dt[0].CommodityID,
                                            dt[0].EffectiveStartDate,
                                            dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                            dt[0].IsTNClearingFeeNull() ? "" : dt[0].TNClearingFee.ToString(),
                                            dt[0].IsTNExchangeFeeNull() ? "" : dt[0].TNExchangeFee.ToString(),
                                            dt[0].IsTNCompensationFundNull() ? "" : dt[0].TNCompensationFund.ToString(),
                                            dt[0].IsTNThirdPartyFeeNull() ? "" : dt[0].TNThirdPartyFee.ToString(),
                                            dt[0].IsClearingFundNull() ? "" : dt[0].ClearingFund.ToString());
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                   dt[0].TNClearingFee, dt[0].TNExchangeFee, dt[0].TNCompensationFund, dt[0].TNThirdPartyFee,
                                                   dt[0].ClearingFund, userName, DateTime.Now, approvalDesc, dt[0].EffectiveEndDate,
                                                   null, dt[0].OriginalID);
                        }
                        else
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                                   dt[0].TNClearingFee, dt[0].TNExchangeFee, dt[0].TNCompensationFund, dt[0].TNThirdPartyFee,
                                                   dt[0].ClearingFund, userName, DateTime.Now, approvalDesc, null,
                                                   null, dt[0].OriginalID);
                        }

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].FeeID);
                        logMessage = string.Format("Approved Update: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|tnClearingFee={3}" +
                                           "|tnExchangeFee={4}|tnCompensationFund={5}|tnThirdPartyFee={6}|clearingFund={7}",
                                            dt[0].CommodityID,
                                            dt[0].EffectiveStartDate,
                                            dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                            dt[0].IsTNClearingFeeNull() ? "" : dt[0].TNClearingFee.ToString(),
                                            dt[0].IsTNExchangeFeeNull() ? "" : dt[0].TNExchangeFee.ToString(),
                                            dt[0].IsTNCompensationFundNull() ? "" : dt[0].TNCompensationFund.ToString(),
                                            dt[0].IsTNThirdPartyFeeNull() ? "" : dt[0].TNThirdPartyFee.ToString(),
                                            dt[0].IsClearingFundNull() ? "" : dt[0].ClearingFund.ToString());
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].FeeID);
                        logMessage = string.Format("Approved Delete: CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|tnClearingFee={3}" +
                                           "|tnExchangeFee={4}|tnCompensationFund={5}|tnThirdPartyFee={6}|clearingFund={7}",
                                            dt[0].CommodityID,
                                            dt[0].EffectiveStartDate,
                                            dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                            dt[0].IsTNClearingFeeNull() ? "" : dt[0].TNClearingFee.ToString(),
                                            dt[0].IsTNExchangeFeeNull() ? "" : dt[0].TNExchangeFee.ToString(),
                                            dt[0].IsTNCompensationFundNull() ? "" : dt[0].TNCompensationFund.ToString(),
                                            dt[0].IsTNThirdPartyFeeNull() ? "" : dt[0].TNThirdPartyFee.ToString(),
                                            dt[0].IsClearingFundNull() ? "" : dt[0].ClearingFund.ToString());
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Fee", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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

    public static void RejectProposedFee(decimal FeeId, string userName, string approvalDesc)
    {
        FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();
        FeeData.FeeDataTable dt = new FeeData.FeeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByFeeID(dt, FeeId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : CommodityID={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|tnClearingFee={3}" +
                                           "|tnExchangeFee={4}|tnCompensationFund={5}|tnThirdPartyFee={6}|clearingFund={7}",
                                            dt[0].CommodityID,
                                            dt[0].EffectiveStartDate,
                                            dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"),
                                            dt[0].IsTNClearingFeeNull() ? "" : dt[0].TNClearingFee.ToString(),
                                            dt[0].IsTNExchangeFeeNull() ? "" : dt[0].TNExchangeFee.ToString(),
                                            dt[0].IsTNCompensationFundNull() ? "" : dt[0].TNCompensationFund.ToString(),
                                            dt[0].IsTNThirdPartyFeeNull() ? "" : dt[0].TNThirdPartyFee.ToString(),
                                            dt[0].IsClearingFundNull() ? "" : dt[0].ClearingFund.ToString());
                }
                //delete Fee ID
                ta.DeleteRejectItem(FeeId);

                AuditTrail.AddAuditTrail("Fee", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

}
