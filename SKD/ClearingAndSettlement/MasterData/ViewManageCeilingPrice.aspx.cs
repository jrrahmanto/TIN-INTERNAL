using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_MasterData_ViewManageCeilingPrice : System.Web.UI.Page
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
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgCeilingPrice.DataSource = ObjectDataSourceCeilingPrice;
        uiDgCeilingPrice.DataBind();
    }

    protected void uiDgCeilingPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgCeilingPrice.PageIndex = e.NewPageIndex;
        FillCeilingPriceDataGrid();
    }

    protected void uiDgCeilingPrice_Sorting(object sender, GridViewSortEventArgs e)
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

        FillCeilingPriceDataGrid();
    }

    private void FillCeilingPriceDataGrid()
    {
        uiDgCeilingPrice.DataSource = ObjectDataSourceCeilingPrice;
        IEnumerable dv = (IEnumerable)ObjectDataSourceCeilingPrice.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgCeilingPrice.DataSource = dve;
        uiDgCeilingPrice.DataBind();
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManageCeilingPrice.aspx?eType=add");
    }
}