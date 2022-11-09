using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PengelolaGudang_DailyStock_ViewDailyStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
        }
        catch (Exception ex)
        {
            
        }
    }

    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgStockWarehouse.DataSource = ObjectDataSourceStockWarehouse;
        uiDgStockWarehouse.DataBind();
    }

    protected void uiDgStockWarehouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgStockWarehouse.PageIndex = e.NewPageIndex;
        FillStockWarehouseDataGrid();
    }

    protected void uiDgStockWarehouse_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (string.IsNullOrEmpty(SortOrder))
        {
            SortOrder = e.SortExpression + " " + "DESC";
        }
        else
        {
            string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
            if (arrSortOrder[1] == "ASC")
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else if (arrSortOrder[1] == "DESC")
            {
                SortOrder = e.SortExpression + " " + "ASC";
            }
        }

        FillStockWarehouseDataGrid();
    }

    private void FillStockWarehouseDataGrid()
    {
        uiDgStockWarehouse.DataSource = ObjectDataSourceStockWarehouse;
        IEnumerable dv = (IEnumerable)ObjectDataSourceStockWarehouse.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgStockWarehouse.DataSource = dve;
        uiDgStockWarehouse.DataBind();
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryDailyStock.aspx?eType=add");
    }
}