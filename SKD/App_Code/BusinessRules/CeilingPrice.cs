using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CeilingPrice
/// </summary>
public class CeilingPrice
{
    public CeilingPrice()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static CeilingPriceData.CeilingPriceDataTable GetCeilingPrice()
    {
        CeilingPriceDataTableAdapters.CeilingPriceTableAdapter ta = new CeilingPriceDataTableAdapters.CeilingPriceTableAdapter();
        CeilingPriceData.CeilingPriceDataTable dt = new CeilingPriceData.CeilingPriceDataTable();

        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static CeilingPriceData.CeilingPriceRow GetCeilingPriceById(decimal id)
    {
        CeilingPriceDataTableAdapters.CeilingPriceTableAdapter ta = new CeilingPriceDataTableAdapters.CeilingPriceTableAdapter();
        CeilingPriceData.CeilingPriceDataTable dt = new CeilingPriceData.CeilingPriceDataTable();
        CeilingPriceData.CeilingPriceRow dr = null;

        try
        {
            ta.FillByCeilingPriceID(dt, id);

            if(dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Failed to load ceiling price data : " + ex.Message);
        }
    }

    public static void UpdateCeilingPrice(Decimal ceilingPrice, Decimal floorPrice, Decimal ceilingPriceID, string userName)
    {
        CeilingPriceDataTableAdapters.CeilingPriceTableAdapter ta = new CeilingPriceDataTableAdapters.CeilingPriceTableAdapter();

        try
        {
            ta.UpdateQuery(ceilingPrice, floorPrice, ceilingPriceID);
            AuditTrail.AddAuditTrail("CeilingPrice", "Update", "{ceilingPrice:" + ceilingPrice + ",floorPrice:" + floorPrice + ", tanggal:" + DateTime.Now + "}", userName, "Insert");
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static CeilingPriceData.SampleDataTable getDataSampleByName(String name)
    {
        CeilingPriceDataTableAdapters.SampleTableAdapter ta = new CeilingPriceDataTableAdapters.SampleTableAdapter();

        return ta.GetDataByNama(name);
    }
}