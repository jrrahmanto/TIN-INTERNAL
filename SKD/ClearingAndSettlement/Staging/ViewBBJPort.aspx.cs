using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_Staging_ViewBBJPort : System.Web.UI.Page
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

    protected void uiDgPort_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgPort.PageIndex = e.NewPageIndex;
        FillPortDataGrid();
    }

    protected void uiDgPort_Sorting(object sender, GridViewSortEventArgs e)
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

        FillPortDataGrid();
    }

    private void FillPortDataGrid()
    {
        uiDgPort.DataSource = ObjectDataSource1;
        IEnumerable dv = (IEnumerable)ObjectDataSource1.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgPort.DataSource = dve;
        uiDgPort.DataBind();
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgPort.DataSource = ObjectDataSource1;
        uiDgPort.DataBind();
    }
}