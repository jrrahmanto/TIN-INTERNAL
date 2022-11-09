using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_ViewManualPosting : System.Web.UI.Page
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
        uiTxtPostingNo.Focus();
        try
        {
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManualPosting.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillPostingDataGrid();
    }

    protected void uiDgPosting_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            approvalStatus.Text = Common.GetApprovalDescription(approvalStatus.Text);
        }
    }

    protected void uiDgPosting_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgPosting.PageIndex = e.NewPageIndex;
        FillPostingDataGrid();
    }

    protected void uiDgPosting_Sorting(object sender, GridViewSortEventArgs e)
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

        FillPostingDataGrid();
    }

    private void FillPostingDataGrid()
    {
        try
        {
            uiDgPosting.DataSource = ObjectDataSourcePosting;
            IEnumerable dv = (IEnumerable)ObjectDataSourcePosting.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgPosting.DataSource = dve;
            uiDgPosting.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(3000);

        MasterPage mp = ( MasterPage)this.Master;
        
    }
}
