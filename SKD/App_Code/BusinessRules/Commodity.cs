using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Transactions;

/// <summary>
/// Summary description for Commodity
/// </summary>
public class Commodity
{
    public static CommodityData.CommodityDataTable GetCommodityByCommodityCodeAndName(string commodityCode, string commodityName)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
        CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();

        try
        {
            ta.FillByCommodityCodeAndName(dt, commodityCode, commodityName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data", ex);
        }
    }

    public static CommodityData.CommodityDataTable FillBySearchCriteria(string commodityCode, 
                                                                        string commodityName,
                                                                        string approvalStatus)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
        try
        {
            return ta.GetDataBySearchCriteria(commodityCode, commodityName, approvalStatus);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static CommodityData.CommProductExchDataTable FillByCommodityID(decimal commodityID)
    {
        CommodityDataTableAdapters.CommProductExchTableAdapter ta = new CommodityDataTableAdapters.CommProductExchTableAdapter();

        try
        {
            return ta.GetDataByCommodityID(commodityID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static CommodityData.CommodityDataTable FillByCommodityID2(decimal commodityID)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();

        try
        {
            return ta.GetDataByCommodityId(commodityID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static string GetCommodityName(decimal contractID)
    {
        CommodityDataTableAdapters.CommodityNameTableAdapter ta = new CommodityDataTableAdapters.CommodityNameTableAdapter();
        CommodityData.CommodityNameDataTable dt = new CommodityData.CommodityNameDataTable();
        try
        {
            dt = ta.GetDataByContractId(contractID);
            return ((CommodityData.CommodityNameRow)dt[0]).CommName;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public static string GetCommodityNameByCommID(decimal commID)
    {
        CommodityDataTableAdapters.CommodityNameTableAdapter ta = new CommodityDataTableAdapters.CommodityNameTableAdapter();
        CommodityData.CommodityNameDataTable dt = new CommodityData.CommodityNameDataTable();
        try
        {
            dt = ta.GetDataByCommodityId(commID);
            return ((CommodityData.CommodityNameRow)dt[0]).CommName;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static CommodityData.CommodityDataTable GetCommodityByExchangeIdApprovalCommCodeActive(
        decimal exchangeid, string ApprovalStatus, string commCode, DateTime businessDate)
    {
        try
        {
            CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();
            CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();

            ta.FillByExchangeIdApprovalStatusCommCodeActive(dt, exchangeid, 
                ApprovalStatus, commCode, businessDate);

            if (dt.Count > 1)
            {
                throw new ApplicationException("Duplicate commodity data.");
            }

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }

    }

    public static decimal GetCommodityID(decimal exchangeID,string commName)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
        try
        {
            return Convert.ToDecimal(ta.GetCommodityID(exchangeID, commName));
        }
        catch (Exception)
        {

            throw;
        }
    }

    public static void ValidateRecord(decimal EditedCommodityID, string ApprovalStatus)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
        CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();

        ta.FillByCommodityId(dt, EditedCommodityID);

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

    public static void Proposed(decimal productID, decimal exchangeID, DateTime startDate, string commCode,
                                 Nullable<decimal> refComID, string unit, Nullable<decimal> PEG, string vmircaCalType, string crossCurr,
                                 string settlementFactor, Nullable<int> dayRef, Nullable<decimal> divisor, string commName, Nullable<decimal> marginTender,
                                 decimal marginSpot, decimal marginRemote, Nullable<decimal> calSpreadRemoteMargin, string crossCurrencyProduct,
                                 decimal HomeCurrencyID, Nullable<DateTime> endDate, string isKIE, string settlementType, int contractSize,
                                 string isIncentive, string userName, string action, Nullable<decimal> originalID, string tenderReqRequest,
                                 string modeK1, decimal valueK1, long contractRefK1, string modeK2, decimal valueK2, long contractRefK2, string modeD, decimal valueD, long contractRefD, string modeIM,
                                 decimal percentageSpotIM, decimal percentageRemoteIM, string modeFee, string quality, decimal regionID, string commodityType)
    {
        try
        {
            CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
            string ActionFlagDesc = "";

            if (originalID.HasValue)
            {
                // Validate for propose/approve/reject record
                ValidateRecord(Convert.ToDecimal(originalID), "P");
            }
            //if (CountExistingRecord(productID,exchangeID,startDate) > 0)
            //{
            //    throw new ApplicationException("Record already exist.");
            //}
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(productID, exchangeID, startDate, "P", commCode, refComID, unit, PEG, vmircaCalType, crossCurr,
                          settlementFactor, dayRef, divisor, commName, marginTender, marginSpot, marginRemote, calSpreadRemoteMargin,
                          crossCurrencyProduct, userName, DateTime.Now, HomeCurrencyID, userName, DateTime.Now, endDate, null, isKIE,
                          settlementType, contractSize, modeFee, percentageRemoteIM, modeIM, valueD, contractRefD, modeD, valueK1, contractRefK1, modeK1,
                          valueK2, contractRefK2, modeK2,tenderReqRequest, action, originalID, isIncentive, percentageSpotIM,
                          quality, regionID, commodityType);


                switch (action)
                {
                    case "I": ActionFlagDesc = "Proposed Insert"; break;
                    case "U": ActionFlagDesc = "Proposed Update"; break;
                    case "D": ActionFlagDesc = "Proposed Delete"; break;
                }

                string logMessage = string.Format(ActionFlagDesc + " ProductID:{0} | " +
                                                  "ExchangeID:{1} | " + 
                                                  "StartDate:{2} | " + 
                                                  "CommCode:{3} | " + 
                                                  "refComID:{4} | " + 
                                                  "Unit:{5} | " + 
                                                  "PEG:{6} | " + 
                                                  "VMIRCACALType:{7} | " + 
                                                  "CrossCurr:{8} | " + 
                                                  "SettlementFactor:{9} | " +
                                                  "DayRef:{10} | " + 
                                                  "Divisor:{11} | " + 
                                                  "CommName:{12} | " +
                                                  "MarginTender:{13} | " +
                                                  "MarginSpot:{14} | " +
                                                  "Margin Remote:{15} | " +
                                                  "CalSpredRemoteMargin:{16} | " +
                                                  "CrossCurencyProduct:{17} | " +
                                                  "HomeCurrencyID:{18} | " +
                                                  "EndDate:{19} | " +
                                                  "IsKIE:{20} | " +
                                                  "SettlemetType:{21} | " +
                                                  "ContractSize:{22} | " +
                                                  "IsIncencitive:{23} | ", productID,
                                                  exchangeID,
                                                  startDate,
                                                  commCode,
                                                  (refComID == null) ? "" : refComID.ToString(),
                                                  unit,
                                                  (PEG == null) ? "" : PEG.ToString(),
                                                  vmircaCalType,
                                                  crossCurr,
                                                  settlementFactor,
                                                  (dayRef == null) ? "" : dayRef.ToString(),
                                                  (divisor == null) ? "" : divisor.ToString(),
                                                  commName,
                                                  (marginTender == null) ? "" : marginTender.ToString(),
                                                  marginSpot,
                                                  marginRemote,
                                                  (calSpreadRemoteMargin == null) ? "" : calSpreadRemoteMargin.ToString(),
                                                  crossCurrencyProduct,
                                                  HomeCurrencyID,
                                                  (endDate == null) ? "" : endDate.ToString(),
                                                  isKIE,
                                                  settlementType,
                                                  contractSize,
                                                  isIncentive);
                AuditTrail.AddAuditTrail("Commodity", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);
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

    public static void Approve(decimal commodityID, string approvalDesc, string action, string userName)
    {
        try
        {
            CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
            CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();

            ValidateRecord(commodityID, "A");

            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByCommodityId(dt, commodityID);

                string actionDesc = "";
                if (dt.Count > 0)
                {
                    string isIncentive = null;
                    string SettlementFactor = null;
                    string VMIRCACalType = null;
                    string CrossCurr = "NA";/* Default because it didn't used anymore in entry commodity */
                    string CrossCurrProduct = null;
                    string unit = null;
                    string ActionFlagDesc = "";
                    string tenderReqType = null;
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

                    string refCOmID = "";
                    Nullable<decimal> refCOmIDVal = null;
                    string PEG = "";
                    Nullable<decimal> PEGVal = 1;/* Default because it didn't used anymore in entry commodity */
                    string DayRef = "";
                    Nullable<int> DayRefVal = null;
                    string Divisor = "";
                    Nullable<decimal> DivisorVal = 1;/* Default because it didn't used anymore in entry commodity */
                    string MarginTender = "";
                    Nullable<decimal> MarginTenderVal = null;
                    string CalSpreadRemoteMargin = "";
                    Nullable<decimal> CalSpreadRemoteMarginVal = null;
                    string endDate = "";
                    Nullable<DateTime> endDateVal = null;
                    decimal originalCommID = 0;
                    if (!dt[0].IsOriginalCommodityIDNull())
                    {
                        originalCommID = dt[0].OriginalCommodityID;
                    }
                    if (!dt[0].IsReffCommIDNull())
                    {
                        refCOmID = dt[0].ReffCommID.ToString();
                        refCOmIDVal = dt[0].ReffCommID;
                    }
                    if (!dt[0].IsUnitNull())
                    {
                        unit = dt[0].Unit;
                    }
                    if (!dt[0].IsPEGNull())
                    {
                        PEG = dt[0].PEG.ToString();
                        PEGVal = dt[0].PEG;
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
                    if (!dt[0].IsCrossCurrProductNull())
                    {
                        CrossCurrProduct = dt[0].CrossCurrProduct.ToString();
                    }
                    if (!dt[0].IsIsIncentiveNull())
                    {
                        isIncentive = dt[0].IsIncentive;
                    }
                    if (!dt[0].IsEffectiveEndDateNull())
                    {
                        endDate = dt[0].EffectiveEndDate.ToString("dd-MM-yyyy");
                    }
                    if (!dt[0].IsSettlementFactorNull())
                    {
                        SettlementFactor = dt[0].SettlementFactor;
                    }

                    CrossCurr = dt[0].CrossCurr;
                    
                    if (!dt[0].IsTenderReqTypeNull())
                    {
                        tenderReqType = dt[0].TenderReqType;
                    }
                    if (!dt[0].IsCrossCurrProductNull())
                    {
                        CrossCurrProduct = dt[0].CrossCurrProduct;
                    }
                    /* Add by Zainab */
                    if (!dt[0].IsModeK1Null())
                    {
                        modeK1 = dt[0].ModeK1.ToString();
                    }
                    if (!dt[0].IsValueK1Null())
                    {
                        valueK1 = dt[0].ValueK1;
                    }
                    if (!dt[0].IsContractRefK1Null())
                    {
                        contractRefK1 = (long)dt[0].ContractRefK1;
                    }
                    if (!dt[0].IsModeK2Null())
                    {
                        modeK2 = dt[0].ModeK2.ToString();
                    }
                    if (!dt[0].IsValueK2Null())
                    {
                        valueK2 = dt[0].ValueK2;
                    }
                    if (!dt[0].IsContractRefK2Null())
                    {
                        contractRefK2 = (long)dt[0].ContractRefK2;
                    }
                    if (!dt[0].IsModeDNull())
                    {
                        modeD = dt[0].ModeD;
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
                    
                    if (!dt[0].IsPercentageSpotIMNull())
                    {
                        percentageSpotIM = dt[0].PercentageSpotIM;
                    }
                    if (!dt[0].IsPercentageRemoteIMNull())
                    {
                        percentageRemoteIM = dt[0].PercentageRemoteIM;
                    }
                    if (!dt[0].IsModeFeeNull())
                    {
                        modeFee = dt[0].ModeFee;
                    }
                    /*-----end-----*/
                    if (action == "Insert")
                    {
                        ta.ApproveInsert("A", null, null, approvalDesc,commodityID);
                        ActionFlagDesc = "Approved Insert";
                    }                    
                    else if (action == "Update")
                    {
                        ta.ApproveUpdate(dt[0].ProductID, dt[0].ExchangeId, dt[0].EffectiveStartDate, "A", dt[0].CommodityCode,
                                         refCOmIDVal, dt[0].Unit, PEGVal, VMIRCACalType,
                                         CrossCurr,
                                         SettlementFactor, DayRefVal, DivisorVal,
                                         dt[0].CommName,
                                         MarginTenderVal,
                                         dt[0].MarginSpot, dt[0].MarginRemote,
                                         CalSpreadRemoteMarginVal, CrossCurrProduct,
                                         dt[0].HomeCurrencyID, userName, DateTime.Now,
                                         endDateVal,
                                         approvalDesc, dt[0].IsKIE, dt[0].SettlementType,
                                         dt[0].ContractSize,
                                         isIncentive, null, null, tenderReqType, modeK1, valueK1, contractRefK1,
                                         modeK2, valueK2,contractRefK2, modeD, valueD,contractRefD, modeIM, percentageSpotIM,
                                         percentageRemoteIM, modeFee, dt[0].OriginalCommodityID);
                        ta.DeleteByCommodityID(commodityID);
                        ActionFlagDesc = "Approved Update";
                    }
                    else if (action == "Delete")
                    {
                        // Delete proposed record
                        ta.DeleteByCommodityID(commodityID);

                        // Delete target record
                        ta.DeleteByCommodityID(originalCommID);

                        ActionFlagDesc = "Approved Delete";
                    }

                    string logMessage = string.Format(ActionFlagDesc + " ProductID:{0} | " +
                                                  "ExchangeID:{1} | " +
                                                  "StartDate:{2} | " +
                                                  "CommCode:{3} | " +
                                                  "refComID:{4} | " +
                                                  "Unit:{5} | " +
                                                  "PEG:{6} | " +
                                                  "VMIRCACALType:{7} | " +
                                                  "CrossCurr:{8} | " +
                                                  "SettlementFactor:{9} | " +
                                                  "DayRef:{10} | " +
                                                  "Divisor:{11} | " +
                                                  "CommName:{12} | " +
                                                  "MarginTender:{13} | " +
                                                  "MarginSpot:{14} | " +
                                                  "Margin Remote:{15} | " +
                                                  "CalSpredRemoteMargin:{16} | " +
                                                  "CrossCurencyProduct:{17} | " +
                                                  "HomeCurrencyID:{18} | " +
                                                  "EndDate:{19} | " +
                                                  "IsKIE:{20} | " +
                                                  "SettlemetType:{21} | " +
                                                  "ContractSize:{22} | " +
                                                  "IsIncentive:{23} | ", dt[0].ProductID.ToString(),
                                                  dt[0].ExchangeId,
                                                  dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"),
                                                  dt[0].CommodityCode,
                                                  refCOmID,
                                                  unit,
                                                  PEG,
                                                  VMIRCACalType,
                                                  CrossCurr,
                                                  SettlementFactor,
                                                  DayRef,
                                                  Divisor,
                                                  dt[0].CommName,
                                                  MarginTender,
                                                  dt[0].MarginSpot,
                                                  dt[0].MarginRemote,
                                                  CalSpreadRemoteMargin,
                                                  CrossCurrProduct,
                                                  dt[0].HomeCurrencyID,
                                                  endDate,
                                                  dt[0].IsKIE,
                                                  dt[0].SettlementType,
                                                  dt[0].ContractSize,
                                                  isIncentive);

                    AuditTrail.AddAuditTrail("Commodity", AuditTrail.APPROVE, logMessage, userName, ActionFlagDesc);
                    scope.Complete();
                }
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
                if (exMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    exMessage = "Please delete the related contract.";
                }
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static void Reject(decimal commodityID, string approvalDesc, string action, string userName)
    {
        try
        {
            CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();
            CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();
            string ActionFlagDesc = "";

            ValidateRecord(commodityID, "R");

            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByCommodityId(dt, commodityID);

                string actionDesc = "";
                if (dt.Count > 0)
                {
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Reject Insert"; break;
                        case "U": ActionFlagDesc = "Reject Update"; break;
                        case "D": ActionFlagDesc = "Reject Delete"; break;
                    }      

                    string refCOmID = "";
                    string unit = "";
                    string PEG = "";
                    string VMIRCACalType = "";
                    string CrossCurr = "";
                    string SettlementFactor = "";
                    string DayRef = "";
                    string Divisor = "";
                    string MarginTender = "";
                    string CalSpreadRemoteMargin = "";
                    string CrossCurrProduct = "";
                    string isIncentive = "";
                    string endDate = "";
                    if (!dt[0].IsReffCommIDNull())
                    {
                        refCOmID = dt[0].ReffCommID.ToString();
                    }
                    if (!dt[0].IsUnitNull())
                    {
                        unit = dt[0].Unit;
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
                    if (!dt[0].IsIsIncentiveNull())
                    {
                        isIncentive = dt[0].IsIncentive;
                    }
                    if (!dt[0].IsEffectiveEndDateNull())
                    {
                        endDate = dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy");
                    }
                    string logMessage = string.Format(ActionFlagDesc + " ProductID:{0} | " +
                                                  "ExchangeID:{1} | " +
                                                  "StartDate:{2} | " +
                                                  "CommCode:{3} | " +
                                                  "refComID:{4} | " +
                                                  "Unit:{5} | " +
                                                  "PEG:{6} | " +
                                                  "VMIRCACALType:{7} | " +
                                                  "CrossCurr:{8} | " +
                                                  "SettlementFactor:{9} | " +
                                                  "DayRef:{10} | " +
                                                  "Divisor:{11} | " +
                                                  "CommName:{12} | " +
                                                  "MarginTender:{13} | " +
                                                  "MarginSpot:{14} | " +
                                                  "Margin Remote:{15} | " +
                                                  "CalSpredRemoteMargin:{16} | " +
                                                  "CrossCurencyProduct:{17} | " +
                                                  "HomeCurrencyID:{18} | " +
                                                  "EndDate:{19} | " +
                                                  "IsKIE:{20} | " +
                                                  "SettlemetType:{21} | " +
                                                  "ContractSize:{22} | " +
                                                  "IsIncentive:{23} | ", 
                                                  dt[0].ProductID,
                                                  dt[0].ExchangeId,
                                                  dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"),
                                                  dt[0].CommodityCode,
                                                  refCOmID,
                                                  unit,
                                                  PEG,
                                                  VMIRCACalType,
                                                  CrossCurr,
                                                  SettlementFactor,
                                                  DayRef,
                                                  Divisor,
                                                  dt[0].CommName,
                                                  MarginTender,
                                                  dt[0].MarginSpot,
                                                  dt[0].MarginRemote,
                                                  CalSpreadRemoteMargin,
                                                  CrossCurrProduct,
                                                  dt[0].HomeCurrencyID,
                                                  endDate,
                                                  dt[0].IsKIE,
                                                  dt[0].SettlementType,
                                                  dt[0].ContractSize,
                                                  isIncentive);

                    ta.DeleteByCommodityID(commodityID);

                    AuditTrail.AddAuditTrail("Commodity", AuditTrail.REJECT, logMessage, userName, ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static decimal CountExistingRecord(decimal productID, decimal exchangeID, DateTime startDate)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta = new CommodityDataTableAdapters.CommodityTableAdapter();

        return decimal.Parse(ta.CountExistingRecord(productID, exchangeID, startDate).ToString());
    }

    public static CommodityData.CommodityDataTable GetCommodityByCode(string CommodityCode)
    {
        CommodityDataTableAdapters.CommodityTableAdapter ta =
            new CommodityDataTableAdapters.CommodityTableAdapter();

        return ta.GetDataByCommodityCode(CommodityCode);
    }
}
