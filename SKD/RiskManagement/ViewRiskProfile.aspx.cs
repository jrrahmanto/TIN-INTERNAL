using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;

public partial class RiskManagement_ViewRiskProfile : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
            
            
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryRiskProfile.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillRiskProfileDataGrid();
    }

    protected void uiDgRiskProfile_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = ClearingMember.GetClearingMemberCodeByClearingMemberID(int.Parse(e.Row.Cells[3].Text));
        }
    }

    protected void uiDgRiskProfile_Sorting(object sender, GridViewSortEventArgs e)
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

        FillRiskProfileDataGrid();
    }

    protected void uiDgRiskProfile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgRiskProfile.PageIndex = e.NewPageIndex;
        FillRiskProfileDataGrid();
    }

    private void FillRiskProfileDataGrid()
    {        
        uiDgRiskProfile.DataSource = ObjectDataSourceRiskProfile;
        IEnumerable dv = (IEnumerable)ObjectDataSourceRiskProfile.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgRiskProfile.DataSource = dve;
        uiDgRiskProfile.DataBind();
    }

    
}
