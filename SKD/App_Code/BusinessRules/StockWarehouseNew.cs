using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StockWarehouse
/// </summary>
public class StockWarehouseNew
{
    public StockWarehouseNew()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DailyStockData.StockWarehouseDataTable GetStockWarehouse(Nullable<DateTime> businessDate, Nullable<decimal> commID)
    {
        DailyStockDataTableAdapters.StockWarehouseTableAdapter ta = new DailyStockDataTableAdapters.StockWarehouseTableAdapter();
        DailyStockData.StockWarehouseDataTable dt = new DailyStockData.StockWarehouseDataTable();

        try
        {
            ta.FillBySearchCriteria(dt, businessDate, commID);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DailyStockData.StockWarehouseRow GetStockWarehouseById(decimal id)
    {
        DailyStockDataTableAdapters.StockWarehouseTableAdapter ta = new DailyStockDataTableAdapters.StockWarehouseTableAdapter();
        DailyStockData.StockWarehouseDataTable dt = new DailyStockData.StockWarehouseDataTable();
        DailyStockData.StockWarehouseRow dr = null;

        try
        {
            ta.FillByStockID(dt, id);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load daily stock data");
        }
    }

    public static void UpdateStockWarehouse(Decimal commodityId, DateTime businessDate, string location, Decimal volume, Decimal stockID)
    {
        DailyStockDataTableAdapters.StockWarehouseTableAdapter ta = new DailyStockDataTableAdapters.StockWarehouseTableAdapter();

        try
        {
            ta.UpdateQuery(commodityId, location, volume, businessDate, stockID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void InsertStockWarehouse(Decimal commodityId, DateTime businessDate, string location, Decimal volume)
    {
        DailyStockDataTableAdapters.StockWarehouseTableAdapter ta = new DailyStockDataTableAdapters.StockWarehouseTableAdapter();

        try
        {
            ta.InsertQuery(commodityId, location, volume, businessDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}