using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;


/// <summary>
/// Summary description for SettlementPriceException
/// </summary>
public class SettlementPriceException
{
    #region "   Member Variables   "
    SettlementPriceExceptionDataTableAdapters.SPExceptionTableAdapter _adapterException = new SettlementPriceExceptionDataTableAdapters.SPExceptionTableAdapter();
    SettlementPriceExceptionDataTableAdapters.RawTradeFeedSPTableAdapter _adapterRaw = new SettlementPriceExceptionDataTableAdapters.RawTradeFeedSPTableAdapter();
    
    #endregion


    public static SettlementPriceExceptionData.SPExceptionDataTable
           GetSettlementPriceExceptionByExchangeIdBusinessDateApproval(decimal exchangeId,
           DateTime businessDate, string approvalStatus)
    {
        SettlementPriceExceptionDataTableAdapters.SPExceptionTableAdapter ta = new  SettlementPriceExceptionDataTableAdapters.SPExceptionTableAdapter();
        SettlementPriceExceptionData.SPExceptionDataTable dt = new SettlementPriceExceptionData.SPExceptionDataTable();

        try
        {
            return ta.GetDataByCriteria(exchangeId, businessDate, approvalStatus);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public SettlementPriceExceptionData.SPExceptionDataTable GetData(decimal exchangeID, DateTime businessDate, string approvalStatus)
    {
        return _adapterException.GetDataByCriteria(exchangeID, businessDate, approvalStatus);
    }

    public SettlementPriceExceptionData.SPExceptionDataTable GetData(decimal exchangeID, DateTime businessDate, decimal SPID)
    {
        return _adapterException.GetDataByPK(SPID, businessDate, exchangeID);
    }

    public void Reject(decimal exchangeID, DateTime businessDate, decimal SPID, string approvalDesc,
            string userName)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _adapterException.Reject(approvalDesc,SPID, businessDate, exchangeID);

                string logMsg = string.Format("SettlementPriceID:{0}|ExchangeID:{1}|BusinessDate:{2}",
                    SPID, exchangeID, businessDate);
                AuditTrail.AddAuditTrail("SettlementPriceException", "Update", logMsg, userName, "Update");

                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }

    public int Approve(decimal exchangeID, DateTime businessDate, decimal SPID, string userName)
    {
        // get exception data
        SettlementPriceExceptionData.SPExceptionDataTable dtExc = _adapterException.GetDataByPK(SPID, businessDate, exchangeID);

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (dtExc.Count > 0)
                {
                    // note: perlu cek nggak kalau statusnya sekarang sudah "A"?

                    // get exception record
                    SettlementPriceExceptionData.SPExceptionRow drExc = dtExc[0];

                    // update status to approved "A"
                    drExc.ApprovalStatus = "A";
                    _adapterException.Update(drExc);

                    // get existing raw data
                    SettlementPriceExceptionData.RawTradeFeedSPDataTable dtRaw = _adapterRaw.GetDataByPK(businessDate, exchangeID, SPID);

                    // call sp
                    if (dtRaw.Count > 0)
                    {
                        // get raw data
                        SettlementPriceExceptionData.RawTradeFeedSPRow drRaw = dtRaw[0];
                        SettlementPriceExceptionDataTableAdapters.QueriesTableAdapter adapterValidator = new SettlementPriceExceptionDataTableAdapters.QueriesTableAdapter();

                        int? retVal = 0;

                        // run validator
                        adapterValidator.ValidateSettlementPrice(ref retVal, drRaw.BusinessDate, drRaw.ExchangeId,
                            drRaw.SPID, drRaw.MonthContract, drRaw.ProductCode, drRaw.SettlementPrice, userName);

                        // if error still exist, then remove record with approved status = "A"
                        if (retVal != 0)
                        {
                            // remove
                            _adapterException.Delete(SPID, businessDate, exchangeID, "A");
                        }


                        string logMsg = string.Format("SettlementPriceID:{0}|ExchangeID:{1}|BusinessDate:{2}",
                           SPID, exchangeID, businessDate);
                        AuditTrail.AddAuditTrail("SettlementPriceException", "Update", logMsg, userName, "Update");

                        scope.Complete();

                        return (int)retVal;
                    }
                    else
                    {
                        throw new ApplicationException("RawSettlementPriceException record not found");
                    }
                }
                else
                {
                    throw new ApplicationException("SettlementPriceException record not found");
                }

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }


    }

}
