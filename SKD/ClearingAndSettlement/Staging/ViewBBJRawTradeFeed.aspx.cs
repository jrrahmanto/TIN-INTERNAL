using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_Staging_ViewBBJRawTradeFeed : System.Web.UI.Page
{
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

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiDgRawTradeFeed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgRawTradeFeed.PageIndex = e.NewPageIndex;
        FillRawTradeFeedDataGrid();
    }

    protected void uiDgRawTradeFeed_Sorting(object sender, GridViewSortEventArgs e)
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

        FillRawTradeFeedDataGrid();
    }

    private void FillRawTradeFeedDataGrid()
    {
        uiDgRawTradeFeed.DataSource = ObjectDataSource1;
        IEnumerable dv = (IEnumerable)ObjectDataSource1.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgRawTradeFeed.DataSource = dve;
        uiDgRawTradeFeed.DataBind();
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgRawTradeFeed.DataSource = ObjectDataSource1;
        uiDgRawTradeFeed.DataBind();
    }
}