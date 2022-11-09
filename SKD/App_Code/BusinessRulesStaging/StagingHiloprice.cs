using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StagingHiloprice
/// </summary>
public class StagingHiloprice
{
    public static StagingData.HiLoPriceDataTable SelectHiloprice(DateTime BusinessDate)
    {
        StagingDataTableAdapters.HiLoPriceTableAdapter ta = new StagingDataTableAdapters.HiLoPriceTableAdapter();
        StagingData.HiLoPriceDataTable dt = new StagingData.HiLoPriceDataTable();

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