using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StagingFinancialInfo
/// </summary>
public class StagingFinancialInfo
{
    public static StagingData.FinancialInfoDataTable SelectFinancialInfoByBusinessDate(DateTime BusinessDate)
    {
        StagingDataTableAdapters.FinancialInfoTableAdapter ta = new StagingDataTableAdapters.FinancialInfoTableAdapter();
        StagingData.FinancialInfoDataTable dt = new StagingData.FinancialInfoDataTable();

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