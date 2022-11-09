using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

/// <summary>
/// Summary description for EODTradeProgress
/// </summary>
public class EODTradeProgress
{
    public EODTradeProgress()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Check trade status P by BusDate
    /// </summary>
    /// <param name="busDate"></param>
    /// <returns></returns>
    public static bool GetByBusDateAndTradeStatus(DateTime busDate)
    {
        EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
        bool isValid = false;
        try
        {
            ta.FillByBusDateNTradeStatus(dt, busDate);
            if (dt.Rows.Count > 0)
            {
                isValid = true;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        return isValid;
    }
    public static EODTradeProgressData.EODTradeProgressDataTable GetBySettlementSubmittedTimeNull()
    {
        EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();

        try
        {
            ta.FillBySettlementSubmittedTimeNull(dt);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static EODTradeProgressData.EODTradeProgressDataTable GetByFullPaymentSubmittedTimeNull()
    {
        EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();

        try
        {
            ta.FillByFullPaymentSubmittedTimeNull(dt);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        
        return dt;
    }

    public static void UpdateProgress(EODTradeProgressData.EODTradeProgressRow dr)
    {
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
       

        try
        {
            ta.Update(dr);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODTradeProgressData.ViewDueDateSettlementDataTable GetDueDateSettlement(DateTime businessDate)
    {
        EODTradeProgressData.ViewDueDateSettlementDataTable dt = new EODTradeProgressData.ViewDueDateSettlementDataTable();
        EODTradeProgressDataTableAdapters.ViewDueDateSettlementTableAdapter ta = new EODTradeProgressDataTableAdapters.ViewDueDateSettlementTableAdapter();

        try
        {
            ta.Fill(dt, businessDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static void UpdateEodTradeProgress(string dueType, string dueAction, decimal progressId, 
        DateTime defaultDate, Nullable<DateTime> carryForwadDate, string carryForwardRemarks,
        string userName, DateTime updateDate)
    {
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
        EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();
        EODTradeProgressData.EODTradeProgressRow dr = null;

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByProgressId(dt, progressId);
                if (dt.Count > 0)
                {
                    dr = dt[0];
                    if (dueType.Equals("Fulfillment"))
                    {
                        if (dueAction.Equals("Default"))
                        {
                            dr.SellerDefault = defaultDate;
                        }
                        else if (dueAction.Equals("CarryForward"))
                        {
                            dr.SellerDuePeriod = carryForwadDate.Value;
                            dr.SellerCarryForward = carryForwadDate.Value;
                            dr.SellerCarryForwardNote = carryForwardRemarks;
                        }
                    }
                    else if (dueType.Equals("Payment"))
                    {
                        if (dueAction.Equals("Default"))
                        {
                            dr.BuyerDefault = defaultDate;
                        }
                        else if (dueAction.Equals("CarryForward"))
                        {
                            dr.BuyerPaymentDue = carryForwadDate.Value;
                            dr.BuyerCarryForward = carryForwadDate.Value;
                            dr.BuyerCarryForwardNote = carryForwardRemarks;
                        }
                    }

                    dr.TradeStatus = "E";
                    dr.LastUpdatedBy = userName;
                    dr.LastUpdatedDate = updateDate;

                    ta.Update(dr);
                }


                string logMessage = string.Format("ProgressID:{0}", progressId);
                AuditTrail.AddAuditTrail("EODTradeProgress", "Update", logMessage, userName, "Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODTradeProgressData.EODTradeProgressRow GetEodTradeProgressByExchangeRef(string exchangeRef)
    {
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
        EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();
        EODTradeProgressData.EODTradeProgressRow dr = null;

        try
        {
            ta.FillByExchangeRef(dt, exchangeRef);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dr;
    }

    public static DateTime GetDefaultDateByExchangeRef(string dueType, string exchangeRef)
    {
        DateTime defaultDate;
        EODTradeProgressData.EODTradeProgressRow dr;

        try
        {
            dr = GetEodTradeProgressByExchangeRef(exchangeRef);

            if (dr != null)
            {
                if (dueType.Equals("Fulfillment"))
                {
                    defaultDate = dr.SellerDuePeriod;

                }
                else if (dueType.Equals("Payment"))
                {
                    defaultDate = dr.BuyerPaymentDue;
                }
                else
                    throw new Exception("Unknown Due Type: " + dueType);

                return defaultDate;
            }
            else 
                return DateTime.MinValue;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static EODTradeProgressData.EODTradeProgressDataTable FillAPP(String exchangeRef, DateTime bussDate)
    {
        try
        {
            EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
            return ta.GetDataAppByExchangeRefAndBusinessDate(bussDate);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ApproveApp(decimal ProgressId, string bussDate)
    {
        try
        {
            EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
            ta.ApproveAPP(bussDate, ProgressId);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.InnerException);
        }
    }

    public static void RejectApp(decimal ProgressId, string bussDate)
    {
        EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
        ta.RejectAPP(ProgressId, bussDate);
    }
}