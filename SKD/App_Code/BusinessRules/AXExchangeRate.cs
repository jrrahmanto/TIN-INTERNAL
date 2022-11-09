using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AXExchangeRate
/// </summary>
public class AXExchangeRate
{
    public static decimal getKursUsd(string BusinessDate)
    {
        AxaptaDataTableAdapters.EXCHANGERATETableAdapter ta = new AxaptaDataTableAdapters.EXCHANGERATETableAdapter();
        decimal kurs;
        try
        {
            kurs = (decimal)ta.GetExchangeRate(BusinessDate);

            return kurs;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Kurs ax data : " + ex.Message);
        }

        return 0;
    }
}