using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Contract
/// </summary>
public class Contract
{

    public static ContractData.ContractDataTable GetContract()
    {
        ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

        try
        {
            ta.Fill(dt);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static ContractData.ContractDataTable 
        GetContractByCommodityIdContractMonthYearApprovalActive(decimal commodityId, 
        int contractMonth, int contractYear, string approvalStatus, DateTime businessDate)
    {
        try
        {
            ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
            ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

            ta.FillByCommodityIdContractMonthYearApprovalActive(dt, commodityId, 
                contractMonth, contractYear, approvalStatus, businessDate);

            if (dt.Count > 1)
            {
                throw new ApplicationException("Duplicate contract data.");
            }

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ContractData.ContractDataTable GetContractByEffectiveStartAndEndDate(DateTime businessDate)
    {
        ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

        try
        {
            ta.FillByEffectiveStartAndEndDate(dt, businessDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static ContractData.ContractDataTable GetContractByCommodityCode(string CommodityCode, decimal exchangeID, string commName, Nullable<decimal> homeCurrency)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        try
        {
            return ta.GetDataByCommodityCode(commName,CommodityCode, exchangeID, homeCurrency);
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    public static ContractData.ContractStressTestDataTable GetContractStressTestByCommodityCode(string CommodityCode, decimal exchangeID, string commName, DateTime businessDate)
    {
        ContractDataTableAdapters.ContractStressTestTableAdapter ta = new ContractDataTableAdapters.ContractStressTestTableAdapter();
        return ta.GetData(CommodityCode, commName, exchangeID);
    }

    public static decimal GetContractID(decimal commID, int month, int year)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        try
        {
            return Convert.ToDecimal(ta.GetContractID(commID, month, year));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ContractData.ContractCommodityRow GetContractByContractID2(decimal contractID)
    {
        ContractData.ContractCommodityDataTable dt = new ContractData.ContractCommodityDataTable();
        ContractData.ContractCommodityRow dr = null;
        ContractDataTableAdapters.ContractCommodityTableAdapter ta = 
            new ContractDataTableAdapters.ContractCommodityTableAdapter();
        try
        {
            ta.FillByContractID(dt, contractID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }
            return dr;
        }
        catch (Exception ex)
        {
            
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ContractData.ContractRow GetContractByContractID(decimal contractID)
    {
        ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
        ContractData.ContractRow dr = null;
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

        try
        {
            ta.FillByContractID(dt, contractID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetContractDescByContractID(decimal contractID)
    {
        ContractData.ContractCommodityDataTable dt = new ContractData.ContractCommodityDataTable();
        ContractData.ContractCommodityRow dr = null;
        ContractDataTableAdapters.ContractCommodityTableAdapter ta = new ContractDataTableAdapters.ContractCommodityTableAdapter();

        string ContractDesc = "";

        try
        {
            ta.FillByContractID(dt, contractID);
            if (dt.Count > 0)
            {
                dr = dt[0];
               ContractDesc = dt[0].CommodityCode + " " + dt[0].ContractYear.ToString() + string.Format("{0:00}",dt[0].ContractMonth.ToString());
            }

            return ContractDesc;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ContractData.ContractCommodityDataTable FillByContractID2(decimal contractID)
    {
        ContractDataTableAdapters.ContractCommodityTableAdapter ta = new ContractDataTableAdapters.ContractCommodityTableAdapter();
        try
        {
            return ta.GetDataByContractID(contractID);
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ContractData.ContractDataTable FillByContractID(decimal contractID)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

        try
        {
           return ta.GetDataByContractID(contractID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ContractData.ContractDataTable FillBySearchCriteria(Nullable<decimal> commID,
                                                                      Nullable<int> month,
                                                                      Nullable<int> year,
                                                                      string approvalStatus)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
        try
        {
            dt = ta.GetDataBySearchCriteria(commID,month, year, approvalStatus);

            return dt;
            //return ta.GetDataBySearchCriteria(commID.Value, month.Value, year.Value, approvalStatus);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ValidateRecord(decimal EditedContractID, string ApprovalStatus)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        ContractData.ContractDataTable dt = new ContractData.ContractDataTable();

        ta.FillByContractID(dt, EditedContractID);

        if (dt.Count > 0)
        {
            if (dt[0].ApprovalStatus == "P" && ApprovalStatus == "P")
            {
                throw new ApplicationException("This record is not allowed to be edited / deleted. Please wait for checker approval.");
            }

            if (dt[0].ApprovalStatus == "A" && (ApprovalStatus == "A" || ApprovalStatus == "R"))
            {
                throw new ApplicationException("Approved row is not allowed to be approved or rejected.");
            }
        }
    }

    public static void Proposed(decimal commodityID, int contractMonth, int contractYear,
                                DateTime effStartDate, decimal contractSize,
                                string description, string unit, DateTime startDate, 
                                DateTime endSpot,DateTime startSpot,
                                Nullable<decimal> PEG, string vmircaCalType, string crossCurr,
                                string settlementFactor, Nullable<int> dayRef, 
                                Nullable<decimal> divisor, 
                                Nullable<decimal> marginTender, decimal marginSpot, 
                                decimal marginRemote, Nullable<decimal> calSpreadRemoteMargin, 
                                string crossCurrencyProduct, decimal homeCurencyID, 
                                Nullable<DateTime> endDate, string newContract,
                                string SettlementType,      
                                string userName,
                                string action, Nullable<decimal> originalID, string tenderReqType, decimal subCategoryID,string modeK1,
                                decimal valueK1, long contractRefK1, string modeK2,decimal valueK2, long contractRefK2, string modeD, decimal valueD, long contractRefD, string modeIM,
                                decimal percentageRemoteIM, decimal percentageSpotIM, string modeFee, string quality, decimal regionID, string commodityType)

    {
        try
        {
            ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();

            // Validate record if the newly proposed record can be inserted
            if (originalID.HasValue)
            {
                ValidateRecord(Convert.ToDecimal(originalID), "P");
            }

            TradefeedDataTableAdapters.TradeFeedTableAdapter taTF = new TradefeedDataTableAdapters.TradeFeedTableAdapter();

            decimal countContractID = decimal.Parse(taTF.CountCountractID(Convert.ToDecimal(originalID)).ToString());

            if (countContractID > 0)
            {
                throw new ApplicationException("Contract already used for trade.");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                string actionDesc = "";

                switch (action)
                {
                    case "I" : 
                        actionDesc = "Insert";
                        break;
                    case "U" :
                        actionDesc = "Update";
                        break;
                    case "D" :
                        actionDesc = "Delete";
                        break;
                    default :
                        throw new ApplicationException("Undefined action");
                }

                string logMessage = string.Format("Proposed " + actionDesc + " CommodityID:{0} | " +
                                                  "ContractMonth:{1} | " +
                                                  "ContractYear{2} | " +
                                                  "StartDate:{3} | " +
                                                  "StartSpot:{4} | " +
                                                  "EndSpot:{5} | " +
                                                  "Unit:{6} | " +
                                                  "PEG:{7} | " +
                                                  "VM/IRCA Calculation Type:{8} | " +
                                                  "Description:{9} | " +
                                                  "CrossCurr:{10} | " +
                                                  "SettlementFactor:{11} | " +
                                                  "DayRef:{12} | " +
                                                  "Divisor:{13} | " +
                                                  "Margin Tender:{14} | " +
                                                  "Margin Spot:{15} | " +
                                                  "Margin Remote:{16} | " +
                                                  "Calendar Spread Remote Margin:{17} | " +
                                                  "Cross Curency Product:{18} | " +
                                                  "Home Currency ID:{19} | " +
                                                  "Effective Start Date:{20} | " +
                                                  "Effective End Date:{21} | " +
                                                  "NewContract:{22} | " +
                                                  "Settlemet Type:{23} | " +
                                                  "Contract Size:{24} " , commodityID,
                                                  contractMonth,
                                                  contractYear,
                                                  startDate.ToShortDateString(),
                                                  startSpot.ToShortDateString(),
                                                  endSpot.ToShortDateString(),
                                                  unit,
                                                  (PEG == null) ? "" : PEG.ToString(),
                                                  vmircaCalType,
                                                  description,
                                                  crossCurr,
                                                  settlementFactor,
                                                  (dayRef == null) ? "" : dayRef.ToString(),
                                                  (divisor == null) ? "" : divisor.ToString(),
                                                  (marginTender == null) ? "" : marginTender.ToString(),
                                                  marginSpot,
                                                  marginRemote,
                                                  (calSpreadRemoteMargin == null) ? "" : calSpreadRemoteMargin.ToString(),
                                                  crossCurrencyProduct,
                                                  homeCurencyID,
                                                  (startDate == null) ? "" : startDate.ToShortDateString(),
                                                  (endDate == null) ? "" : endDate.ToString(),
                                                  newContract,
                                                  SettlementType,
                                                  contractSize);

                // Insert proposed record
                ta.Insert(commodityID, contractMonth, contractYear, "P", effStartDate, contractSize,
                      SettlementType, description, unit, startDate, endSpot, startSpot, PEG, vmircaCalType,
                      settlementFactor, dayRef, divisor, marginTender, marginSpot, marginRemote, calSpreadRemoteMargin,
                      newContract, userName, DateTime.Now, userName, DateTime.Now, endDate, null, homeCurencyID,
                      crossCurr, crossCurrencyProduct, originalID, action, tenderReqType, subCategoryID,modeK1,valueK1,
                      contractRefK1, modeK2, valueK2,contractRefK2, modeD, valueD, contractRefD, modeIM, percentageRemoteIM, 
                      percentageSpotIM, modeFee, quality, regionID, commodityType);

                // Add to audit trail
                AuditTrail.AddAuditTrail("Contract", AuditTrail.PROPOSE, logMessage, userName, actionDesc);

                // Commit transaction
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


    public static void Approve(decimal contractID, string approvalDesc, string action, string userName)
    {
        try
        {
            ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
            ContractData.ContractDataTable dt = new ContractData.ContractDataTable();

            //set table adapter timeout for 20 minutes
            TableAdapterHelper.SetAllCommandTimeouts2(ta, 1200);

            ValidateRecord(contractID, "A");

            dt = ta.GetDataByContractID(contractID);

            //set transaction scope timeout for 20 minutes
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(20)))
            {
                if (dt.Count > 0)
                {
                    string VMIRCACalType = null;
                    string CrossCurr = "NA"; //Default Karena Sudah tidak digunakan lagi
                    string SettlementFactor = null;
                    string CrossCurrProduct = null;
                    string description = null;

                    string HomeCurID = "";
                    Nullable<decimal> HomeCurIDVal = null;
                    string PEG = "";
                    Nullable<decimal> PEGVal = 0; //Default Karena Sudah tidak digunakan lagi
                    string DayRef = "";
                    Nullable<int> DayRefVal = null;
                    string Divisor = "";
                    Nullable<decimal> DivisorVal = 0; //Default Karena Sudah tidak digunakan lagi
                    string MarginTender = "";
                    Nullable<decimal> MarginTenderVal = null;
                    string CalSpreadRemoteMargin = "";
                    Nullable<decimal> CalSpreadRemoteMarginVal = null;
                    string endDate = "";
                    Nullable<DateTime> endDateVal = null;
                    decimal subCategoryID = 0;
                    //--Add By Ramadhan 2014-07-04---\\\
                    //-- Penambahan 10 variable--\\
                    string modeK1 = null;
                    Nullable<decimal> valueK1 = 0;
                    Nullable<long> contractRefK1 = 0;
                    string modeK2 = null;
                    Nullable<decimal> valueK2 = 0;
                    Nullable<long> contractRefK2 = 0;
                    string modeD = null;
                    Nullable<decimal> valueD = 0;
                    Nullable<long> contractRefD = 0;
                    string modeIM = null;
                    Nullable<decimal> percentageSpotIM = 0;
                    Nullable<decimal> percentageRemoteIM = 0;
                    string modeFee = null;
                    //--End--\\

                    if (!dt[0].IsVMIRCACalTypeNull())
                    {
                        VMIRCACalType = dt[0].VMIRCACalType;
                    }
                   
                    CrossCurr = dt[0].CrossCurr;
                    
                    if (!dt[0].IsSettlementFactorNull())
                    {
                        SettlementFactor = dt[0].SettlementFactor;
                    }
                    if (!dt[0].IsCrossCurrProductNull())
                    {
                        CrossCurrProduct = dt[0].CrossCurrProduct.ToString();
                    }
                    if (!dt[0].IsDescriptionNull())
                    {
                        description = dt[0].Description;
                    }

                    if (!dt[0].IsHomeCurrencyIDNull())
                    {
                        HomeCurID = dt[0].HomeCurrencyID.ToString();
                        HomeCurIDVal = dt[0].HomeCurrencyID;
                    }
                    if (!dt[0].IsPEGNull())
                    {
                        PEG = dt[0].PEG.ToString();
                        PEGVal = dt[0].PEG;
                    }
                    if (!dt[0].IsDayRefNull())
                    {
                        DayRef = dt[0].DayRef.ToString();
                        DayRefVal = dt[0].DayRef;
                    }
                    if (!dt[0].IsDivisorNull())
                    {
                        Divisor = dt[0].Divisor.ToString();
                        DivisorVal = dt[0].Divisor;
                    }
                    if (!dt[0].IsMarginTenderNull())
                    {
                        MarginTender = dt[0].MarginTender.ToString();
                    }
                    if (!dt[0].IsCalSpreadRemoteMarginNull())
                    {
                        CalSpreadRemoteMargin = dt[0].CalSpreadRemoteMargin.ToString();
                        CalSpreadRemoteMarginVal = dt[0].CalSpreadRemoteMargin;
                    }
                    if (!dt[0].IsEffectiveEndDateNull())
                    {
                        endDate = dt[0].EffectiveEndDate.ToString("dd-MM-yyyy");
                        endDateVal = dt[0].EffectiveEndDate;
                    }
                    if (!dt[0].IsModeK1Null())
                    {
                        modeK1 = dt[0].ModeK1;
                    }
                    if (!dt[0].IsModeK2Null())
                    {
                        modeK2 = dt[0].ModeK2;
                    }
                    if (!dt[0].IsValueK1Null())
                    {
                        valueK1 = dt[0].ValueK1; 
                    }
                    if (!dt[0].IsValueK2Null())
                    {
                        valueK2 = dt[0].ValueK2;
                    }
                    if (!dt[0].IsContractRefK1Null())
                    {
                        contractRefK1 = (long)dt[0].ContractRefK1;
                    }
                    if (!dt[0].IsContractRefK2Null())
                    {
                        contractRefK2 = (long)dt[0].ContractRefK2;
                    }
                    if (!dt[0].IsModeK1Null())
                    {
                        modeD = dt[0].ModeD;
                    }
                    if (!dt[0].IsPercentageSpotIMNull())
                    {
                        percentageSpotIM = dt[0].PercentageSpotIM;
                    }
                    if (!dt[0].IsValueDNull())
                    {
                        valueD = dt[0].ValueD;
                    }
                    if (!dt[0].IsContractRefDNull())
                    {
                        contractRefD = (long)dt[0].ContractRefD;
                    }
                    if (!dt[0].IsModeIMNull())
                    {
                        modeIM = dt[0].ModeIM;
                    }
                    if (!dt[0].IsPercentageRemoteIMNull())
                    {
                        percentageRemoteIM = dt[0].PercentageRemoteIM;
                    }
                    if (!dt[0].IsModeFeeNull())
                    {
                        modeFee = dt[0].ModeFee;
                    }
                    if (!dt[0].IsSubCategoryIDNull())
                    {
                        subCategoryID = dt[0].SubCategoryID;
                    }

                    string logMessage = string.Format("Approve " + action + " CommodityID:{0} | " +
                             "ContractMonth:{1} | " +
                             "ContractYear{2} | " +
                             "StartDate:{3} | " +
                             "StartSpot:{4} | " +
                             "EndSpot:{5} | " +
                             "Unit:{6} | " +
                             "PEG:{7} | " +
                             "VMIRCACALType:{8} | " +
                             "Description:(9) | " +
                             "CrossCurr:{10} | " +
                             "SettlementFactor:{11} | " +
                             "DayRef:{12} | " +
                             "Divisor:{13} | " +
                             "MarginTender:{14} | " +
                             "MarginSpot:{15} | " +
                             "Margin Remote:{16} | " +
                             "CalSpredRemoteMargin:{17} | " +
                             "CrossCurencyProduct:{18} | " +
                             "HomeCurrencyID:{19} | " +
                             "Effective StartDate:{20} | " +
                             "Effective EndDate:{21} | " +
                             "NewContract:{22} | " +
                             "SettlemetType:{23} | " +
                             "ContractSize:{24} | " +
                             "SubCategoryID:{25} ",
                             dt[0].CommodityID,
                             dt[0].ContractMonth,
                             dt[0].ContractYear,
                             dt[0].EffectiveStartDate.ToShortDateString(),
                             dt[0].StartSpot.ToShortDateString(),
                             dt[0].EndSpot.ToShortDateString(),
                             dt[0].Unit,
                             PEG,
                             (VMIRCACalType == null) ? "" : VMIRCACalType,
                             (description == null) ? "" : description,
                             (CrossCurr == null) ? "" : CrossCurr,
                             (SettlementFactor == null) ? "" : SettlementFactor,
                             DayRef,
                             Divisor,
                             MarginTender,
                             dt[0].MarginSpot,
                             dt[0].MarginRemote,
                             CalSpreadRemoteMargin,
                             (CrossCurrProduct == null) ? "" : CrossCurrProduct,
                             HomeCurID,
                             dt[0].EffectiveStartDate.ToShortDateString(),
                             endDate,
                             dt[0].NewContract,
                             dt[0].SettlementType,
                             dt[0].ContractSize,
                             dt[0].SubCategoryID);

                    if (action == "Insert")
                    {
                        // Add new proposed record
                        ta.ApproveInsert("A", null, null,approvalDesc, userName, DateTime.Now, contractID);
                    }
                    else if (action == "Update")
                    {
                        ta.ApproveUpdate(dt[0].CommodityID, dt[0].ContractMonth, dt[0].ContractYear,
                                         "A", dt[0].StartDate, dt[0].ContractSize, dt[0].SettlementType,
                                         description, dt[0].Unit, dt[0].StartDate, dt[0].EndSpot,
                                         dt[0].StartSpot, PEGVal, VMIRCACalType, SettlementFactor, DayRefVal,
                                         DivisorVal, MarginTenderVal, dt[0].MarginSpot, dt[0].MarginRemote,
                                         CalSpreadRemoteMarginVal, dt[0].NewContract, userName, DateTime.Now,
                                         endDateVal, approvalDesc, HomeCurIDVal, CrossCurr, CrossCurrProduct,
                                         null, modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD,
                                         percentageSpotIM, valueD, contractRefD, modeIM,percentageRemoteIM,
                                         modeFee, subCategoryID, null,dt[0].OriginalContractID);
                                        
                         ta.DeleteByContractiD(contractID);
                    }
                    else  if (action == "Delete")
                    {
                        // Delete the new proposed record
                        ta.DeleteByContractiD(contractID);
                        // Delete target delete record
                        ta.DeleteByContractiD(dt[0].OriginalContractID);
                    }

                    // Add to audit trail
                    //AuditTrail.AddAuditTrail("Contract", AuditTrail.APPROVE, logMessage, userName, "Approve " + action);
                }


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

    public static void Reject(decimal contractID, string approvalDesc, string action, string userName)
    {
        try
        {
            ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
            ContractData.ContractDataTable dt = new ContractData.ContractDataTable();

            //set table adapter timeout for 20 minutes
            TableAdapterHelper.SetAllCommandTimeouts2(ta, 1200);

            ValidateRecord(contractID, "R");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(20)))
            {
                dt = ta.GetDataByContractID(contractID);
                if (dt.Count > 0)
                {
                    
                    ta.DeleteByContractiD(contractID);
                    string PEG = "";
                    Nullable<decimal> PEGVal = null;
                    string VMIRCACalType = "";
                    string CrossCurr = "";
                    string SettlementFactor = "";
                    string DayRef = "";
                    string Divisor = "";
                    string MarginTender = "";
                    string CalSpreadRemoteMargin = "";
                    string CrossCurrProduct = "";
                    string endDate = "";
                    string description = "";
                    string HomeCurID = "";

                    if (!dt[0].IsHomeCurrencyIDNull())
                    {
                        HomeCurID = dt[0].HomeCurrencyID.ToString();
                    }
                    if (!dt[0].IsPEGNull())
                    {
                        PEG = dt[0].PEG.ToString();
                    }
                    if (!dt[0].IsVMIRCACalTypeNull())
                    {
                        VMIRCACalType = dt[0].VMIRCACalType;
                    }
                   
                    CrossCurr = dt[0].CrossCurr;
                   
                    if (!dt[0].IsSettlementFactorNull())
                    {
                        SettlementFactor = dt[0].SettlementFactor;
                    }
                    if (!dt[0].IsDayRefNull())
                    {
                        DayRef = dt[0].DayRef.ToString();
                    }
                    if (!dt[0].IsDivisorNull())
                    {
                        Divisor = dt[0].Divisor.ToString();
                    }
                    if (!dt[0].IsMarginTenderNull())
                    {
                        MarginTender = dt[0].MarginTender.ToString();
                    }
                    if (!dt[0].IsCalSpreadRemoteMarginNull())
                    {
                        CalSpreadRemoteMargin = dt[0].CalSpreadRemoteMargin.ToString();
                    }
                    if (!dt[0].IsCrossCurrProductNull())
                    {
                        CrossCurrProduct = dt[0].CrossCurrProduct.ToString();
                    }
                    if (!dt[0].IsEffectiveEndDateNull())
                    {
                        endDate = dt[0].EffectiveEndDate.ToString("dd-MM-yyyy");
                    }
                    if (!dt[0].IsDescriptionNull())
                    {
                        description = dt[0].Description;
                    }

                    string logMessage = string.Format("Reject " + action + " CommodityID:{0} | " +
                                                 "ContractMonth:{1} | " +
                                                 "ContractYear{2} | " +
                                                 "StartDate:{3} | " +
                                                 "EndSpot:{4} | " +
                                                 "StartSpot:{5} | " +
                                                 "Unit:{6} | " +
                                                 "PEG:{7} | " +
                                                 "VMIRCACALType:{8} | " +
                                                 "Description:(9) | " +
                                                 "CrossCurr:{10} | " +
                                                 "SettlementFactor:{11} | " +
                                                 "DayRef:{12} | " +
                                                 "Divisor:{13} | " +
                                                 "MarginTender:{14} | " +
                                                 "MarginSpot:{15} | " +
                                                 "Margin Remote:{16} | " +
                                                 "CalSpredRemoteMargin:{17} | " +
                                                 "CrossCurencyProduct:{18} | " +
                                                 "HomeCurrencyID:{19} | " +
                                                 "StartDate:{20} | " +
                                                 "NewContract:{21} | " +
                                                 "SettlemetType:{22} | " +
                                                 "ContractSize:{23} | ", dt[0].CommodityID,
                                                 dt[0].ContractMonth,
                                                 dt[0].ContractYear,
                                                 dt[0].EffectiveStartDate.ToShortDateString(),
                                                 dt[0].EndSpot.ToShortDateString(),
                                                 dt[0].StartSpot,
                                                 dt[0].Unit,
                                                 PEG,
                                                 VMIRCACalType,
                                                 description,
                                                 CrossCurr,
                                                 SettlementFactor,
                                                 DayRef,
                                                 Divisor,
                                                 MarginTender,
                                                 dt[0].MarginSpot,
                                                 dt[0].MarginRemote,
                                                 CalSpreadRemoteMargin,
                                                 CrossCurrProduct,
                                                 HomeCurID,
                                                 endDate,
                                                 dt[0].NewContract,
                                                 dt[0].SettlementType,
                                                 dt[0].ContractSize);

                    string ActionFlagDesc = "";
                    switch (action)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }

                    AuditTrail.AddAuditTrail("Contract", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
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

    public static decimal GetExchangeID(decimal contractID)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        try
        {
            return Convert.ToDecimal(ta.GetExchangeIDByContractID(contractID));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DateTime GetEndSpot(decimal contractID)
    {
        ContractDataTableAdapters.ContractTableAdapter ta = new ContractDataTableAdapters.ContractTableAdapter();
        return DateTime.Parse(ta.GetEndSpot(contractID).ToString());

    }

    /// <summary>
    /// Update Contract of column Peg and Div 
    /// TODO: This function is temporary purpose because it updates master data Contract 
    ///       which it is recommended NOT TO!
    /// </summary>
    /// <param name="dt">Data Contract to be update</param>
    public static void ImportPegDiv(ContractData.ContractPegDivDataTable dt)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ContractDataTableAdapters.ContractPegDivTableAdapter ta = new ContractDataTableAdapters.ContractPegDivTableAdapter();

                ta.Update(dt);

                string logMessage = string.Format("Import Contract Peg and Div", "");
                AuditTrail.AddAuditTrail("Contract", "Update", logMessage, dt[0].UserName, "Update");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

}
