using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for Inquery
/// </summary>
public class Inquery
{
    #region Account Receivable
    public static InqueryData.AccountReceivableDataTable
       GetAccReceivableByBusinessDate( Nullable<DateTime> startBussDate,Nullable<DateTime> endBussDate)
    {
        InqueryData.AccountReceivableDataTable dt = new InqueryData.AccountReceivableDataTable();
        InqueryDataTableAdapters.AccountReceivableTableAdapter ta = new InqueryDataTableAdapters.AccountReceivableTableAdapter();

        try
        {
            ta.FillByBusinessDate(dt, startBussDate, endBussDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion
    #region ForceCloseTrade
    public static InqueryData.EODTradeProgressDataTable
       GetForceClosedTradeByBusinessDate(Nullable <DateTime> startBussDate,Nullable<DateTime> endBussDate)
    {
        InqueryData.EODTradeProgressDataTable dt = new InqueryData.EODTradeProgressDataTable();
        InqueryDataTableAdapters.EODTradeProgressTableAdapter ta = new InqueryDataTableAdapters.EODTradeProgressTableAdapter();

        try
        {
            ta.FillByBusDate(dt, startBussDate, endBussDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion
    

    //public Inquery()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
}