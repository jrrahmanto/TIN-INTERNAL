using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for Currency
/// </summary>
public class Currency
{

    public static CurrencyData.CurrencyDataTable GetCurrency()
    {
        CurrencyData.CurrencyDataTable dt = new CurrencyData.CurrencyDataTable();
        CurrencyDataTableAdapters.CurrencyTableAdapter ta = new CurrencyDataTableAdapters.CurrencyTableAdapter();

        try
        {
            ta.Fill(dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetCurrencyCodeById(decimal currencyId)
    {
        CurrencyData.CurrencyDataTable dt = new CurrencyData.CurrencyDataTable();
        CurrencyData.CurrencyRow dr;
        CurrencyDataTableAdapters.CurrencyTableAdapter ta = new CurrencyDataTableAdapters.CurrencyTableAdapter();
        string currencyCode = "";

        try
        {
            ta.FillByCurrencyId(dt, currencyId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                currencyCode = dr.CurrencyCode;
            }

            return currencyCode;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

	public static CurrencyData.CurrencyDataTable SelectCurrencyByCurrencyCode(string currencyCode)
	{
        CurrencyDataTableAdapters.CurrencyTableAdapter ta = new CurrencyDataTableAdapters.CurrencyTableAdapter();
        CurrencyData.CurrencyDataTable dt = new CurrencyData.CurrencyDataTable();

        try
        {
            ta.FillbyCurrencyCode(dt, currencyCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load currency data");
        }
	}

}
