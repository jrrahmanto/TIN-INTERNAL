using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Tradefeed
/// </summary>
public class Tradefeed
{
    TradefeedDataTableAdapters.RawTradeFeedTableAdapter _tfRawHandler = new TradefeedDataTableAdapters.RawTradeFeedTableAdapter();

    public static string GetCurrencyCodeByContractID(decimal contractId)
    {
        TradefeedData.CurrencyByContractIDDataTable dt = new TradefeedData.CurrencyByContractIDDataTable();
        TradefeedData.CurrencyByContractIDRow dr = null;
        TradefeedDataTableAdapters.CurrencyByContractIDTableAdapter ta = new TradefeedDataTableAdapters.CurrencyByContractIDTableAdapter();
        string currencyCode = "";
        try
        {
            ta.Fill(dt, contractId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                currencyCode = dr.CurrencyCode;
            }

            return currencyCode;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public TradefeedData.RawTradeFeedDataTable GetData(decimal exchangeID, decimal tradefeedID, DateTime businessDate)
    {
        return _tfRawHandler.GetDataByPK(exchangeID, tradefeedID, businessDate);  
    }

    public static TradefeedData.TradeFeedDataTable FillBySearchCriteria(Nullable<decimal> exchangeID,
                                                                        Nullable<DateTime> bussDate,
                                                                        Nullable<decimal> sellerCMID,
                                                                        Nullable<decimal> buyerCMID,
                                                                        Nullable<decimal> tradeFeedID,
                                                                        Nullable<decimal> contractID)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            return ta.GetDataBySearchCriteria(exchangeID, bussDate, sellerCMID, buyerCMID, tradeFeedID, contractID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static TradefeedData.TradeFeedDataTable FillBySearchCrit(Nullable<decimal> exchangeID,
                                                                        Nullable<DateTime> bussDate,
                                                                        Nullable<decimal> CMID,
                                                                        Nullable<decimal> tradeFeedID,
                                                                        Nullable<decimal> contractID)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            return ta.GetDataBySearchCrit(exchangeID, bussDate, CMID, tradeFeedID, contractID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static TradefeedData.TradeFeedDataTable FillShippingInstruction(DateTime bussDate)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            return ta.GetDataShippingInstruction(bussDate);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static TradefeedData.TradeFeedDataTable FillNoticeOfShipment(Nullable<decimal> exchangeID,
                                                                        DateTime bussDate, string nosi)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            return ta.GetDataNoticeOfShipment(bussDate, nosi);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static TradefeedData.TradeFeedRow FillByNoticeOfShipment(string nosi)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();
            TradefeedData.TradeFeedRow dr = null;

            ta.FillByNosTKS(dt, nosi);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static TradefeedData.TradeFeedDataTable LoopNoticeOfShipment(DateTime bussDate, string nosi)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            return ta.GetDataNoticeOfShipment(bussDate, nosi);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void InsertRawTradeFeed(TradefeedData.RawTradeFeedDataTable dt)
    {
        try
        {
            TradefeedDataTableAdapters.RawTradeFeedTableAdapter ta = new TradefeedDataTableAdapters.RawTradeFeedTableAdapter();

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 15, 0); //15 minutes

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (TradefeedData.RawTradeFeedRow dr in dt)
                {
                    ta.Insert(dr.ExchangeId, dr.TradeFeedID, dr.BusinessDate, dr.TradeTime, 
                              dr.TradeReceivedTime,
                              dr.TradeTimeOffset, dr.TradeOptType, dr.MonthContract, dr.Price, 
                              dr.Qty, dr.ContraIndicator,
                              dr.ExchangeRef, dr.SellerCMCode, dr.SellerInvGiveUpCode, 
                              dr.SellerGiveUpComm,dr.SellerRef,dr.SellerTrdType,dr.SellerCompTradeType,
                              dr.SellerTotalLeg,dr.BuyerInvCode,dr.BuyerTrdType,dr.BuyerInvGiveUpCode,
                              dr.BuyerGiveUpComm,dr.BuyerCompTradeType,dr.BuyerRef,
                              dr.TradeStrikePrice,dr.TradeVersion,dr.BuyTotLeg,dr.SellerInvCode,
                              dr.BuyerCMCode,dr.ProductCode,dr.SellerEMCode,dr.BuyerEMCode,dr.CreatedBy);
                }

                string logMessage = string.Format("Import trade feed", "");
                AuditTrail.AddAuditTrail("RawTradeFeed", "Insert", logMessage, dt[0].CreatedBy,"Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static decimal GetMaxTradeFeedIDByBusinessDate(DateTime businessDate)
    {
        TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
        decimal tradeFeedId = 0;
        try
        {
            tradeFeedId = ta.GetMaxTradeFeedIDByBusinessDate(businessDate).Value;

            return tradeFeedId;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void InsertTradeFeed(TradefeedData.TradeFeedDataTable dt)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            TradefeedData.TradeFeedRow dr = null;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(dt);

                string logMessage = string.Format("TradeFeedID:{0}", dt[0].TradeFeedID);
                AuditTrail.AddAuditTrail("TradeFeed", "Insert", logMessage, dr.CreatedBy,"Insert");

                scope.Complete();
            }          
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void UpdateTradeFeed(decimal exchangeID,decimal tradeFeedID, DateTime bussinessDate
                   , DateTime tradeTime, DateTime tradeReceivedTime, 
                   Nullable<int> tradeTimeOffset, string tradeOptType,
                   decimal contractID, decimal price, int qty,
                   string contraIndicator, string exchangeRef,
                   decimal sellerCMID, string sellerInvGiveUpCode,
                   Nullable<decimal> sellerGiveUpComm, string sellerRef,
                   string sellerTrdType, string sellerCompTradeType,
                   Nullable<int> sellerTotalLeg, decimal buyerInvCode, 
                   string buyerTrdType, string buyerInvGiveUpCode,
                   Nullable<decimal> buyerGiveUpComm, string buyerCompTradeType,
                   string buyerRef, Nullable<decimal> tradeStrikePrice,
                   Nullable<int> tradeVersion, Nullable<int> buyTotLeg, decimal sellerInvCode,
                   decimal buyerCMID, string productCode, decimal sellerEMID,
                   decimal BuyerEMID,  string approvalStatus, string approvalDesc, string createdBy, 
                   DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate)
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(tradeTime, tradeReceivedTime, tradeTimeOffset, tradeOptType,
                    contractID, price, qty, contraIndicator, exchangeRef, sellerCMID,
                    sellerEMID, sellerInvCode, buyerCMID, BuyerEMID, buyerInvCode, 
                    sellerInvGiveUpCode, sellerGiveUpComm, sellerRef, sellerTrdType, sellerCompTradeType, 
                    sellerTotalLeg, buyerTrdType, buyerInvGiveUpCode, buyerGiveUpComm, buyerCompTradeType,
                    buyerRef, buyTotLeg, tradeStrikePrice, tradeVersion, createdBy,
                    createdDate, lastUpdatedBy, lastUpdatedDate, approvalDesc, null, null,
                    null, null, null, null, null, null, null, null, null, null, null, null,
                    null, null, exchangeID, tradeFeedID, bussinessDate, null);
                
                string logMessage = string.Format("TradeFeedID:{0}", tradeFeedID);
                AuditTrail.AddAuditTrail("TradeFeed", "Update", logMessage, lastUpdatedBy,"Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// InsertRawTradeFeedSimulation will bulk copy from datatable to physical table (RawTradeFeedSimulation) 
    /// </summary>
    /// <param name="dt"></param>
    public static void InsertRawTradeFeedSimulation(TradefeedData.RawTradeFeedSimulationDataTable dt)
    {
        try
        {
            TradefeedDataTableAdapters.RawTradeFeedSimulationTableAdapter ta = new TradefeedDataTableAdapters.RawTradeFeedSimulationTableAdapter();
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (TradefeedData.RawTradeFeedSimulationRow dr in dt)
                {
                    ta.Insert(dr.ExchangeId, dr.TradeFeedIDSimulation, dr.TradeFeedID, dr.BusinessDate, dr.TradeTime,
                              dr.TradeReceivedTime,
                              dr.TradeTimeOffset, dr.TradeOptType, dr.ProductCode, dr.MonthContract, dr.Price,
                              dr.Qty, dr.ExchangeRef, dr.SellerCMCode, dr.SellerEMCode, dr.SellerInvCode,
                              dr.BuyerCMCode, dr.BuyerEMCode, dr.BuyerInvCode, dr.TradeStrikePrice,
                              dr.ContraIndicator, dr.SellerInvGiveUpCode, dr.SellerGiveUpComm, dr.SellerRef,
                              dr.SellerTrdType, dr.SellerCompTradeType, dr.SellerTotalLeg, dr.BuyerTrdType,
                              dr.BuyerInvGiveUpCode, dr.BuyerGiveUpComm, dr.BuyerCompTradeType, dr.BuyerRef,
                              dr.TradeVersion, dr.BuyTotLeg, dr.CreatedBy, dr.SessionId);
                }

                string logMessage = string.Format("Import trade feed", "");
                AuditTrail.AddAuditTrail("RawTradeFeed", "Insert", logMessage, dt[0].CreatedBy, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ApproveShippingInstruction(decimal exchangeID, string ExchangeRef, DateTime bussinessDate, string lastUpdatedBy, string shippingInstructionFlag)
    {
        TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
        using (TransactionScope scope = new TransactionScope())
        {
            ta.ApprovalShippingInstruction(shippingInstructionFlag, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ExchangeRef, bussinessDate);

            string logMessage = string.Format("ExchangeRef:{0}", ExchangeRef);
            AuditTrail.AddAuditTrail("TradeFeed", "Approval SI", logMessage, lastUpdatedBy, "Approve");
            scope.Complete();
        }
            
    }

    public static void RejectShippingInstruction(decimal exchangeID, string ExchangeRef, DateTime bussinessDate, string lastUpdatedBy, string shippingInstructionFlag)
    {
        TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
        using (TransactionScope scope = new TransactionScope())
        {
            ta.ApprovalShippingInstruction(shippingInstructionFlag, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ExchangeRef, bussinessDate);

            string logMessage = string.Format("ExchangeRef:{0}", ExchangeRef);
            AuditTrail.AddAuditTrail("TradeFeed", "Approval SI", logMessage, lastUpdatedBy, "Reject");
            scope.Complete();
        }

    }

}
