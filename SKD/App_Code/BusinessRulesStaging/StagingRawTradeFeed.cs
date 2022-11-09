using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StagingRawTradeFeed
/// </summary>
public class StagingRawTradeFeed
{
    public static StagingData.RawTradeFeedDataTable SelectRawTradeFeedByBusinessDate(DateTime BusinessDate)
    {
        StagingDataTableAdapters.RawTradeFeedTableAdapter ta = new StagingDataTableAdapters.RawTradeFeedTableAdapter();
        StagingData.RawTradeFeedDataTable dt = new StagingData.RawTradeFeedDataTable();

        try
        {
            ta.Fill(dt, BusinessDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Member Account data");
        }
    }
}