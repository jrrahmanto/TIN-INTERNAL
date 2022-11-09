using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Tender
/// </summary>
public class Tender
{
	public Tender()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region "-- Tender --"

    public static void ProcessTenderRequest(decimal tenderId, decimal tenderNo, decimal contractId,
        decimal sellerInvId, DateTime tenderDate, string deliveryLocation, string tenderReqType,
        string approvalStatus, string approvalDesc, string createdBy, DateTime createdDate,
        string lastUpdatedBy, DateTime lastUpdatedDate, TenderData.TenderRequestDataTable dtNewTenderRequest,
        DateTime businessDate)
    {
        TenderDataTableAdapters.TenderTableAdapter taTender = new TenderDataTableAdapters.TenderTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (tenderNo == 0)
                {
                    TenderData.MaxTenderNoDataTable dtMaxTenderNo = new TenderData.MaxTenderNoDataTable();
                    TenderDataTableAdapters.MaxTenderNoTableAdapter taMaxTenderNo = new TenderDataTableAdapters.MaxTenderNoTableAdapter();
                    taMaxTenderNo.Fill(dtMaxTenderNo);
                    if (dtMaxTenderNo[0].IsTenderNoNull())
                    {
                        tenderNo = 1;
                    }
                    else
                    {
                        tenderNo = dtMaxTenderNo[0].TenderNo + 1;
                    }

                    tenderId = (decimal)taTender.InsertTender(tenderNo, approvalStatus, contractId, sellerInvId,
                        tenderDate, deliveryLocation, createdBy, createdDate, lastUpdatedBy,
                        lastUpdatedDate, approvalDesc, tenderReqType,businessDate);
                }
                else
                {
                    //skip
                    //not yet implement
                    TenderData.TenderDataTable dtTender = new TenderData.TenderDataTable();
                    TenderData.TenderRow drTender = null;
                    taTender.FillByTenderId(dtTender, tenderId);
                    drTender = dtTender[0];
                    taTender.Update(tenderNo, approvalStatus, drTender.ContractID, drTender.SellerInvID, businessDate, 
                                    drTender.TenderDate, drTender.DeliveryLocation, drTender.CreatedBy, 
                                    drTender.CreatedDate, lastUpdatedBy, lastUpdatedDate, approvalDesc, 
                                    drTender.TenderReqType,drTender.TenderNo, drTender.ApprovalStatus);

                }


                //Tender Request
                TenderData.TenderRequestDataTable dtTenderRequest = new TenderData.TenderRequestDataTable();
                TenderData.TenderRequestRow drNewTenderRequest = null;
                TenderDataTableAdapters.TenderRequestTableAdapter taTenderRequest = new TenderDataTableAdapters.TenderRequestTableAdapter();
                taTenderRequest.FillByTenderId(dtTenderRequest, tenderId);

                foreach (TenderData.TenderRequestRow drTenderRequest in dtNewTenderRequest)
                {
                    drNewTenderRequest = dtTenderRequest.FindByPriceTradePositionTenderID(drTenderRequest.Price, drTenderRequest.TradePosition, tenderId);

                    if (drNewTenderRequest == null)
                    {
                        drNewTenderRequest = dtTenderRequest.NewTenderRequestRow();
                    }

                    drNewTenderRequest.TenderID = tenderId;
                    drNewTenderRequest.Price = drTenderRequest.Price;
                    drNewTenderRequest.TradePosition = drTenderRequest.TradePosition;
                    drNewTenderRequest.Quantity = drTenderRequest.Quantity;

                    if (drNewTenderRequest.RowState == System.Data.DataRowState.Detached)
                    {
                        dtTenderRequest.AddTenderRequestRow(drNewTenderRequest);
                    }
                }

                taTenderRequest.Update(dtTenderRequest);

                string log = log = string.Format("TenderNo:{0}|ContractID:{1}|SellerInvID:{2}|" +
                        "TenderID:{3}|TenderDate:{4}|DeliveryLocation:{5}",
                        tenderNo, contractId, sellerInvId, tenderId, tenderDate, deliveryLocation); 
                if (approvalStatus == "A")
                {                    
                    AuditTrail.AddAuditTrail("Tender", AuditTrail.APPROVE, log, lastUpdatedBy, "Process Tender Request");
                }
                else if (approvalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("Tender", AuditTrail.REJECT, log, lastUpdatedBy, "Process Tender Request");
                }
                

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }

    public static TenderData.TenderRow GetTenderByTenderId(decimal tenderId)
    {
        TenderData.TenderDataTable dt = new TenderData.TenderDataTable();
        TenderData.TenderRow dr = null;
        TenderDataTableAdapters.TenderTableAdapter ta = new TenderDataTableAdapters.TenderTableAdapter();

        try
        {
            ta.FillByTenderId(dt, tenderId);
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

    public static TenderData.TenderDataTable GetTenderByDateContractInvApproval(Nullable<DateTime> tenderDate,
        Nullable<decimal> contractId, Nullable<decimal> sellerInvId, string approvalStatus)
    {
        TenderData.TenderDataTable dt = new TenderData.TenderDataTable();
        TenderDataTableAdapters.TenderTableAdapter ta = new TenderDataTableAdapters.TenderTableAdapter();

        try
        {
            ta.FillByDateContractInvApproval(dt, tenderDate, contractId, sellerInvId, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessTenderRandomized(DateTime businessDate, Nullable<int> tenderId)
    {
        TenderDataTableAdapters.ProcessTenderTableAdapter ta = new TenderDataTableAdapters.ProcessTenderTableAdapter();

        try
        {
            ta.uspProcessTender(businessDate, tenderId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static TenderData.TenderRandomizedDetailTableAdapterDataTable
        GetTenderRandomizedDetail(int tenderId)
    {
        TenderData.TenderRandomizedDetailTableAdapterDataTable dt = new TenderData.TenderRandomizedDetailTableAdapterDataTable();
        TenderDataTableAdapters.TenderRandomizedDetailTableAdapterTableAdapter ta = new TenderDataTableAdapters.TenderRandomizedDetailTableAdapterTableAdapter();

        try
        {
            ta.Fill(dt, tenderId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static TenderData.TenderRandomizedTableAdapterDataTable
        GetTenderRandomized(string exchangeName, string sellerName, string productCode)
    {
        TenderData.TenderRandomizedTableAdapterDataTable dt = new TenderData.TenderRandomizedTableAdapterDataTable();
        TenderDataTableAdapters.TenderRandomizedTableAdapterTableAdapter ta = new TenderDataTableAdapters.TenderRandomizedTableAdapterTableAdapter();

        try
        {
            ta.Fill(dt, exchangeName, sellerName, productCode);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }
    #endregion

    #region "-- TenderRequest --"
    public static TenderData.TenderRequestDataTable GetTenderRequestByTenderId(decimal tenderId)
    {
        TenderData.TenderRequestDataTable dt = new TenderData.TenderRequestDataTable();
        TenderDataTableAdapters.TenderRequestTableAdapter ta = new TenderDataTableAdapters.TenderRequestTableAdapter();

        try
        {
            ta.FillByTenderId(dt, tenderId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion

    #region "-- TenderResult --"
    public static TenderData.TenderResultDataTable GetTenderResultByTenderId(decimal tenderId)
    {
        TenderData.TenderResultDataTable dt = new TenderData.TenderResultDataTable();
        TenderDataTableAdapters.TenderResultTableAdapter ta = new TenderDataTableAdapters.TenderResultTableAdapter();

        try
        {
            ta.FillByTenderId(dt, tenderId);

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion

    #region "  Tukar Fisik Dengan Berjangka   "
    
    public static void SaveTukarFisik(DateTime tenderDate, decimal contractID, decimal sellerInv, decimal sellerPrice,
                                 int sellerQuantity, string sellerPosition, decimal buyerInv, 
                                 decimal buyerPrice, int buyerQuantity, string buyerPosition,
                                 string deliveryLocation, string userName, DateTime businessDate)
    {
        TenderDataTableAdapters.TenderTableAdapter taTender = new TenderDataTableAdapters.TenderTableAdapter();

        try
        {
            decimal tenderID;
            decimal tenderNo;
            TenderData.MaxTenderNoDataTable dtMaxTenderNo = new TenderData.MaxTenderNoDataTable();
            TenderDataTableAdapters.MaxTenderNoTableAdapter taMaxTenderNo = new TenderDataTableAdapters.MaxTenderNoTableAdapter();
            taMaxTenderNo.Fill(dtMaxTenderNo);
            if (dtMaxTenderNo[0].IsTenderNoNull())
            {
                tenderNo = 1;
            }
            else
            {
                tenderNo = dtMaxTenderNo[0].TenderNo + 1;
            }
           

            using (TransactionScope scope = new TransactionScope())
            {
                tenderID = (decimal)taTender.InsertTender(tenderNo, "P", contractID, sellerInv,
                       tenderDate, deliveryLocation, userName, DateTime.Now, userName,
                       DateTime.Now, null, "T",businessDate);
                TenderDataTableAdapters.TenderRequestTableAdapter taTenderRequest = new TenderDataTableAdapters.TenderRequestTableAdapter();
                taTenderRequest.Insert(sellerPrice, sellerPosition, tenderID, sellerQuantity);
                TenderDataTableAdapters.TenderResultTableAdapter taTenderResult = new TenderDataTableAdapters.TenderResultTableAdapter();
                taTenderResult.Insert(buyerInv, businessDate, buyerPrice, buyerPosition, tenderID, "T",
                                      buyerQuantity, userName, DateTime.Now, userName, DateTime.Now);
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
            
       
    }

    public static decimal SellerOpenPosition(string tradePosition, DateTime businessDate, decimal investorID,
                                             decimal contractID, decimal cmid)
    {
        TenderDataTableAdapters.SellerOpenPositionTableAdapter ta = new TenderDataTableAdapters.SellerOpenPositionTableAdapter();
        TenderData.SellerOpenPositionDataTable dt = new TenderData.SellerOpenPositionDataTable();
        decimal openPosition = 0;
        ta.Fill(dt, tradePosition, businessDate, investorID, contractID, cmid);

        if (dt.Rows.Count > 0)
        {
            openPosition = dt[0].OpenPosition;
        }
        return openPosition;
    }

    public static decimal BuyerOpenPosition(string tradePosition, DateTime businessDate, decimal investorID,
                                            decimal contractID, decimal cmid)
    {
        TenderDataTableAdapters.BuyerOpenPositionTableAdapter ta = new TenderDataTableAdapters.BuyerOpenPositionTableAdapter();
        TenderData.BuyerOpenPositionDataTable dt = new TenderData.BuyerOpenPositionDataTable();
        decimal openPosition = 0;
        ta.Fill(dt, tradePosition, businessDate, investorID, contractID, cmid);

        if (dt.Rows.Count > 0)
        {
            openPosition = dt[0].OpenPosition;
        }
        return openPosition;
    }

    public static int CountTenderReqQty(decimal contractID, decimal sellerInvID, decimal price,
                                        string tradePosition, DateTime businessDate)
    {
        TenderDataTableAdapters.TenderRequestTableAdapter ta = new TenderDataTableAdapters.TenderRequestTableAdapter();

        return int.Parse(ta.CountTenderReqQty(contractID, sellerInvID, price, tradePosition, businessDate).ToString());
    }

    public static int CountTenderRequsetQty(decimal contractID, decimal sellerInvID,
                                        string tradePosition, DateTime businessDate)
    {
        TenderDataTableAdapters.TenderRequestTableAdapter ta = new TenderDataTableAdapters.TenderRequestTableAdapter();

        return int.Parse(ta.CountAvailTenderReq(contractID, sellerInvID, tradePosition, businessDate).ToString());
    }

    public static int CountTenderResultReqQty(decimal contractID, decimal sellerInvID,
                                       string tradePosition, DateTime businessDate)
    {
        TenderDataTableAdapters.TenderResultTableAdapter ta = new TenderDataTableAdapters.TenderResultTableAdapter();

        return int.Parse(ta.CountTenderQty(contractID, sellerInvID, tradePosition).ToString());
    }

    public static bool IsValidNewTradeSellerTukarFisikBerjangka(decimal contractID, 
        decimal sellerCMID, decimal sellerInvID, decimal price, DateTime businessDate)
    {
        bool isValid = false;

        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();
        TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();

        try
        {
            ta.FillByCheckSellerTukarFisikBerjangka(dt, contractID, 
                sellerCMID, sellerInvID, price, businessDate);
            if (dt.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsValidNewTradeBuyerTukarFisikBerjangka(decimal contractID,
        decimal buyerCMID, decimal buyerInvID, decimal price, DateTime businessDate)
    {
        bool isValid = false;

        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();
        TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();

        try
        {
            ta.FillByCheckBuyerTukarFisikBerjangka(dt, contractID, 
                buyerCMID, buyerInvID, price, businessDate);
            if (dt.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsValidBroughtForwardSellerTukarFisikBerjangka(decimal contractID,
        decimal sellerCMID, decimal sellerInvID, decimal price, DateTime businessDate)
    {
        bool isValid = false;

        TenderData.InvConPosFisikBerjangkaDataTable dt = new TenderData.InvConPosFisikBerjangkaDataTable();
        TenderDataTableAdapters.InvConPosFisikBerjangkaTableAdapter ta = new TenderDataTableAdapters.InvConPosFisikBerjangkaTableAdapter();

        try
        {
            ta.FillByCheckSellerFisikBerjangka(dt, businessDate, contractID,
                sellerInvID, price, sellerCMID);
            if (dt.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsValidBroughtForwardBuyerTukarFisikBerjangka(decimal contractID,
        decimal buyerCMID, decimal buyerInvID, decimal price, DateTime businessDate)
    {
        bool isValid = false;

        TenderData.InvConPosFisikBerjangkaDataTable dt = new TenderData.InvConPosFisikBerjangkaDataTable();
        TenderDataTableAdapters.InvConPosFisikBerjangkaTableAdapter ta = new TenderDataTableAdapters.InvConPosFisikBerjangkaTableAdapter();

        try
        {
            ta.FillByCheckBuyerTukarFisikBerjangka(dt, businessDate, contractID,
                buyerInvID, price, buyerCMID);
            if (dt.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion
}
