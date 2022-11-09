using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for TradeDefault
/// </summary>
public class TradeDefault
{
    public TradeDefault()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static TradeDefaultData.uspRptTradeDefaultDataTable GetTradeDefault(Nullable<DateTime> busDate, string contractType)
    {
        try
        {
            TradeDefaultData.uspRptTradeDefaultDataTable dt = new TradeDefaultData.uspRptTradeDefaultDataTable();
            TradeDefaultDataTableAdapters.uspRptTradeDefaultTableAdapter ta = new TradeDefaultDataTableAdapters.uspRptTradeDefaultTableAdapter();

            ta.Fill(dt, busDate,contractType);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
     }

    public static TradeDefaultData.DdlContractTypeDataTable GetContractType()
    {
        try
        {
            TradeDefaultData.DdlContractTypeDataTable dt = new TradeDefaultData.DdlContractTypeDataTable();
            TradeDefaultDataTableAdapters.DdlContractTypeTableAdapter ta = new TradeDefaultDataTableAdapters.DdlContractTypeTableAdapter();

            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}