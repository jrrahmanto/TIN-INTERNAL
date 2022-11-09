using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClearingAndSettlement_MasterData_ViewComodity : System.Web.UI.Page
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

    #region "   Use Case   "

    // Handler for page load
    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxbCommCode.Focus();
        try
        {
            SetControlAccessByMakerChecker();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    // Handler for create button
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryCommodity.aspx");
    }

    // Handler for search button
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgCommodity.DataSource = ObjectDataSourceCommodity;
        uiDgCommodity.DataBind();
    }

    // Handler for page index
    protected void uiDgCommodity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgCommodity.PageIndex = e.NewPageIndex;
        FillCommodityDataGrid();
    }

    // Handler for sorting
    protected void uiDgCommodity_Sorting(object sender, GridViewSortEventArgs e)
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

        FillCommodityDataGrid();
    }

    #endregion

    #region "   Supporting Method   "

    // SetControlAccessByMakerChecker
    // Purpose      : Set Control visibility / enabled based on maker checker privilege
    // Parameter    : -
    // Return       : -
    private void SetControlAccessByMakerChecker()
    {
        MasterPage mp = (MasterPage)this.Master;

        bool pageMaker = mp.IsMaker;

        // Set control visibility
        uiBtnCreate.Visible = pageMaker;
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }

    // FillCommodityDataGrid
    // Purpose      : Fill commodity data grid
    // Parameter    : -
    // Return       : -
    private void FillCommodityDataGrid()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceCommodity.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgCommodity.DataSource = dve;
        uiDgCommodity.DataBind();
    }

    #endregion

}
