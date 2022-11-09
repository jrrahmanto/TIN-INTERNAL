using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SOD
/// </summary>
public class SOD
{
    public SOD()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void ProcessSOD(DateTime businessDate, string username, string ipAddress)
    {
        SellerAllocationDataTableAdapters.QueriesTableAdapter ta = new SellerAllocationDataTableAdapters.QueriesTableAdapter();
        ta.CommandTimeOut = 120000;

        try
        {
            ta.uspProcessSOD(businessDate, username, ipAddress);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessHighLowPrice(HighLowPriceResult.RootObject objHighLowPrice
        , string userName)
    {
        CeilingPriceData.CeilingPriceDataTable dtCeilingPrice = new CeilingPriceData.CeilingPriceDataTable();
        CeilingPriceData.CeilingPriceRow drCeilingPrice = null;
        CeilingPriceDataTableAdapters.CeilingPriceTableAdapter taCeilingPrice = new CeilingPriceDataTableAdapters.CeilingPriceTableAdapter();
        try
        {
            taCeilingPrice.FillByEffectiveStartDateApprovalStatus(dtCeilingPrice, objHighLowPrice.result.startDate, "A");
            if (dtCeilingPrice.Count > 0)
            {
                drCeilingPrice = dtCeilingPrice[0];
                drCeilingPrice.CeilingPrice = decimal.Parse(objHighLowPrice.result.ceilPrice);
                drCeilingPrice.FloorPrice = decimal.Parse(objHighLowPrice.result.floorPrice);
                drCeilingPrice.LastUpdatedBy = userName;
                drCeilingPrice.LastUpdatedDate = DateTime.Now;
                taCeilingPrice.Update(drCeilingPrice);
            }
            else
            {
                drCeilingPrice = dtCeilingPrice.NewCeilingPriceRow();
                drCeilingPrice.EffectiveStartDate = objHighLowPrice.result.startDate;
                drCeilingPrice.ApprovalStatus = "A";
                drCeilingPrice.CeilingPrice = decimal.Parse(objHighLowPrice.result.ceilPrice);
                drCeilingPrice.FloorPrice = decimal.Parse(objHighLowPrice.result.floorPrice);
                drCeilingPrice.CreatedBy = userName;
                drCeilingPrice.CreatedDate = DateTime.Now;
                drCeilingPrice.LastUpdatedBy = userName;
                drCeilingPrice.LastUpdatedDate = DateTime.Now;
                dtCeilingPrice.AddCeilingPriceRow(drCeilingPrice);
                taCeilingPrice.Update(dtCeilingPrice);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProcessSellerAllocation(SellerAllocationResult.RootObject objSellerAllocation
        , DateTime businessDate , string userName)
    {
        SellerAllocationData.SellerAllocationDataTable dtSellerAllocation = new SellerAllocationData.SellerAllocationDataTable();
        SellerAllocationData.SellerAllocationRow drSellerAllocation = null;
        SellerAllocationDataTableAdapters.SellerAllocationTableAdapter taSellerAllocation = new SellerAllocationDataTableAdapters.SellerAllocationTableAdapter();
        try
        {
            taSellerAllocation.FillByBusinessDate(dtSellerAllocation, businessDate);
            if (dtSellerAllocation.Count > 0)
            {
                taSellerAllocation.DeleteByBusinessDate(businessDate);
            }

            foreach (var obj in objSellerAllocation.result.items)
            {
                drSellerAllocation = dtSellerAllocation.NewSellerAllocationRow();
                drSellerAllocation.BusinessDate = businessDate;
                drSellerAllocation.SellerID = obj.sellerId;
                drSellerAllocation.ProductID = obj.productId;
                drSellerAllocation.Quantity = int.Parse(obj.volume);
                drSellerAllocation.CreatedBy = userName;
                drSellerAllocation.CreatedDate = DateTime.Now;
                drSellerAllocation.LastUpdatedBy = userName;
                drSellerAllocation.LastUpdatedDate = DateTime.Now;
                drSellerAllocation.AccountID = obj.accountId;
                dtSellerAllocation.AddSellerAllocationRow(drSellerAllocation);
                taSellerAllocation.Update(dtSellerAllocation);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        
    }
}