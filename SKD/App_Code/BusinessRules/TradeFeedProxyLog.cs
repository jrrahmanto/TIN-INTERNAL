using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TradeFeedProxyLog
/// </summary>
public class TradeFeedProxyLog
{
	public TradeFeedProxyLog()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static TradeFeedProxyLogData.TradefeedProxyLogDataTable 
        GetTradeFeedProxyLogByExchangeLogDate(Nullable<decimal> exchangeId, Nullable<DateTime> logDate)
    {
        TradeFeedProxyLogData.TradefeedProxyLogDataTable dt = new TradeFeedProxyLogData.TradefeedProxyLogDataTable();
        TradeFeedProxyLogDataTableAdapters.TradefeedProxyLogTableAdapter ta = new TradeFeedProxyLogDataTableAdapters.TradefeedProxyLogTableAdapter();

        try
        {
            ta.FillByExchangeLogDate(dt, exchangeId, logDate);

            return dt;
        }
        catch (Exception ex)
        {            
            throw new ApplicationException (ex.Message,ex);
        }
    }

    public static byte[] GetLogFile(decimal exchangeId, DateTime logDate, int logSeq)
    {
        TradeFeedProxyLogData.TradefeedProxyLogDataTable dt = new TradeFeedProxyLogData.TradefeedProxyLogDataTable();
        TradeFeedProxyLogData.TradefeedProxyLogRow dr = null;
        TradeFeedProxyLogDataTableAdapters.TradefeedProxyLogTableAdapter ta = new TradeFeedProxyLogDataTableAdapters.TradefeedProxyLogTableAdapter();
        byte[] logFile = null;
        try
        {
            ta.FillByExchangeLogDateSeq(dt, exchangeId, logDate, logSeq);
            if (dt.Count > 0)
            {
                dr = dt[0];
                logFile = dr.TradefeedLog;
            }

            return logFile;
        }
        catch (Exception ex)
        {            
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
