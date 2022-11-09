using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class WebUI_New_ViewCollateral : System.Web.UI.Page
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
        uiTxtLodgementNo.Focus();
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryCollateral.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillCollateralDataGrid();
    }

    private void FillCollateralDataGrid()
    {
        uiDgCollateral.DataSource = ObjectDataSourceCollateral;
        IEnumerable dv = (IEnumerable)ObjectDataSourceCollateral.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgCollateral.DataSource = dve;
        uiDgCollateral.DataBind();
    }

    #region "---------Datagrid Event-----------"

    protected void uiDgCollateral_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgCollateral.PageIndex = e.NewPageIndex;
        FillCollateralDataGrid();
    }

    protected void uiDgCollateral_Sorting(object sender, GridViewSortEventArgs e)
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
               
        FillCollateralDataGrid();
    }
    
    #endregion



    #region "-------------Set Authentication---------------"

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
        uiDgCollateral.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

    #endregion



    
}
