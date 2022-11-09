using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_ViewRole : System.Web.UI.Page
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
        uiTxtGroup.Focus();
        if (!IsPostBack)
        {
            uiTxtGroup.Text = Request.QueryString["Group"];
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryRole.aspx");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillRoleDataGrid();
    }

    protected void uiDGGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDGGroup.PageIndex = e.NewPageIndex;
        FillRoleDataGrid();
    }

    protected void FillRoleDataGrid()
    { 

        IEnumerable dv = (IEnumerable)ObjectDataSource1.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDGGroup.DataSource = dve;
        uiDGGroup.DataBind();
    }

    protected void uiDGGroup_Sorting(object sender, GridViewSortEventArgs e)
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

        FillRoleDataGrid();
    }
}
