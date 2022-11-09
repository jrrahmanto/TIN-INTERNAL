using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for TradefeedException
/// </summary>
public class TradefeedException
{
    #region "   Member Variables   "
        TradefeedDataTableAdapters.TradefeedExceptionTableAdapter _adapterException = new TradefeedDataTableAdapters.TradefeedExceptionTableAdapter();
        TradefeedDataTableAdapters.RawTradeFeedTableAdapter _adapterRaw = new TradefeedDataTableAdapters.RawTradeFeedTableAdapter();
    
    #endregion


    #region "   Constructor   "
        public TradefeedException()
        {
            //
            // TODO: Add constructor logic here
            //
        } 
    #endregion

    #region "   Use Case   "

        public static TradefeedData.TradefeedExceptionDataTable 
            GetTradeFeedExceptionByExchangeIdBusinessDateApproval(Nullable<decimal> exchangeId, 
            DateTime businessDate, string approvalStatus)
        {
            TradefeedDataTableAdapters.TradefeedExceptionTableAdapter ta = new TradefeedDataTableAdapters.TradefeedExceptionTableAdapter();
            TradefeedData.TradefeedExceptionDataTable dt = new TradefeedData.TradefeedExceptionDataTable();

            try
            {
                return ta.GetDataByCriteria(exchangeId, businessDate, approvalStatus);
            }
            catch (Exception ex)
            {	
                throw new ApplicationException(ex.Message, ex);
            }
        }

        public TradefeedData.TradefeedExceptionDataTable GetData(decimal exchangeID, DateTime businessDate, string approvalStatus)
        {
            return _adapterException.GetDataByCriteria(exchangeID, businessDate, approvalStatus);
        }

        public TradefeedData.TradefeedExceptionDataTable GetData(decimal exchangeID, DateTime businessDate, decimal tradefeedID)
        {
            return _adapterException.GetDataByPK(tradefeedID, businessDate, exchangeID);
        }

        public void Reject(decimal exchangeID, DateTime businessDate, decimal tradefeedID,
            string userName)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _adapterException.Reject(tradefeedID, businessDate, exchangeID);

                    string logMsg = string.Format("TradeFeedID:{0}|ExchangeID:{1}|BusinessDate:{2}", 
                        tradefeedID, exchangeID, businessDate);
                    AuditTrail.AddAuditTrail("TradeFeedException", "Update", logMsg, userName,"Update");

                    scope.Complete();
                }
                
            }
            catch (Exception ex)
            {	
                throw new ApplicationException(ex.Message, ex);
            }
            
        }

        public int Approve(decimal exchangeID, DateTime businessDate, decimal tradefeedID, string userName)
        {
            // get exception data
            TradefeedData.TradefeedExceptionDataTable dtExc = _adapterException.GetDataByPK(tradefeedID, businessDate, exchangeID);
            string debugMsg = "";

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (dtExc.Count > 0)
                    {
                        // TODO: perlu cek nggak kalau statusnya sekarang sudah "A"?

                        // get exception record
                        TradefeedData.TradefeedExceptionRow drExc = dtExc[0];

                        debugMsg = "Updating status to approval";
                        // update status to approved "A"
                        drExc.ApprovalStatus = "A";
                        _adapterException.Update(drExc);

                        debugMsg = "Get raw data";
                        // get existing raw data
                        TradefeedData.RawTradeFeedDataTable dtRaw = _adapterRaw.GetDataByPK(exchangeID, tradefeedID, businessDate);

                        // call sp
                        if (dtRaw.Count > 0)
                        {
                            // get raw data
                            TradefeedData.RawTradeFeedRow drRaw = dtRaw[0];
                            TradefeedDataTableAdapters.QueriesTableAdapter adapterValidator = new TradefeedDataTableAdapters.QueriesTableAdapter();

                            int? retVal = 0;

                            debugMsg = "Run validator";
                            // run validator
                            adapterValidator.ValidateTradeExc(ref retVal, drRaw.ExchangeId, drRaw.TradeFeedID,
                                drRaw.BusinessDate, drRaw.TradeTime, drRaw.TradeReceivedTime, drRaw.TradeTimeOffset,
                                drRaw.ProductCode, drRaw.MonthContract.ToString(), drRaw.TradeOptType, drRaw.TradeStrikePrice,
                                drRaw.TradeVersion, drRaw.Price, drRaw.Qty, drRaw.ContraIndicator, drRaw.ExchangeRef,

                                drRaw.SellerCMCode, drRaw.SellerEMCode, drRaw.SellerTrdType, drRaw.SellerInvCode,
                                drRaw.SellerInvGiveUpCode, drRaw.SellerGiveUpComm, drRaw.SellerCompTradeType, drRaw.SellerTotalLeg,
                                drRaw.SellerRef,

                                drRaw.BuyerCMCode, drRaw.BuyerEMCode, drRaw.BuyerTrdType, drRaw.BuyerInvCode,
                                drRaw.BuyerInvGiveUpCode, drRaw.BuyerGiveUpComm, drRaw.BuyerCompTradeType, drRaw.BuyTotLeg,
                                drRaw.BuyerRef, userName, drExc.ForceApproveOperationsType);

                            // if error still exist, then remove record with approved status = "A"
                            if (retVal != 0)
                            {
                                debugMsg = "Delete exception";
                                // remove
                                _adapterException.Delete(tradefeedID, businessDate, exchangeID, "A");
                            }

                            debugMsg = "Create audit trail record";
                            string logMsg = string.Format("TradeFeedID:{0}|ExchangeID:{1}|BusinessDate:{2}",
                               tradefeedID, exchangeID, businessDate);
                            AuditTrail.AddAuditTrail("TradeFeedException", "Update", logMsg, userName, "Update");

                            debugMsg = "Completing transaction scope";
                            scope.Complete();

                            return (int)retVal;
                        }
                        else
                        {
                            throw new ApplicationException("RawTradefeed record not found");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("TradefeedException record not found");
                    }
                   
                }
            }
            catch (Exception ex)
            {	
                throw new ApplicationException(ex.Message + string.Format(" ({0}).", debugMsg), ex);
            }
            

        }

        public string GetErrorMessage(decimal exchangeID, DateTime businessDate, decimal tradefeedID)
        {
            // get exception data
            TradefeedData.TradefeedExceptionDataTable dtExc = GetData(exchangeID, businessDate, "P");
            if (dtExc.Count > 0)
            {
                return dtExc[0].Message;
            }
            else
            {
                return "";
            }
        }

        public static void ApproveAll(Nullable<int> exchangeID, Nullable<DateTime> businessDate, string approvalDesc, string userName)
        {
            // TODO: Timeout if too many records needed to Approve --> need to be tested
            TradefeedDataTableAdapters.QueriesTableAdapter adapterValidator = new TradefeedDataTableAdapters.QueriesTableAdapter();
            TableAdapterHelper.SetAllCommandTimeouts2(adapterValidator, 300);
            try
            {
                adapterValidator.TradeFeedExceptionApproveAll(exchangeID, businessDate, "A", approvalDesc, userName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to approve: " + ex.Message);
            }

        }

    #endregion

}
