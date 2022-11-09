using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class FinanceAndAccounting_ViewBankTransactionException : System.Web.UI.Page
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
        uiDdlBank.Focus();
        if (!Page.IsPostBack)
        {

        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillBankDataGrid();
    }

    protected void uiDgBankTransException_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Change bank id to code
            Label bank = (Label)e.Row.FindControl("uiLblBank");
            bank.Text = Bank.GetBankCodeByBankID(int.Parse(bank.Text));

            //Change approval status to descritpion
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            switch (approvalStatus.Text)
            { 
                case "A" :
                    approvalStatus.Text = "Approved";
                    break;
                case "P" :
                    approvalStatus.Text = "Proposed";
                    break;
                case "R" :
                    approvalStatus.Text = "Rejected";
                    break;
                default :
                    approvalStatus.Text = approvalStatus.Text;
                    break;
            }
        }
    }

    protected void uiDgBankTransException_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgBankTransException.PageIndex = e.NewPageIndex;
        FillBankDataGrid();
    }

    protected void uiDgBankTransException_Sorting(object sender, GridViewSortEventArgs e)
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

        FillBankDataGrid();
    }

    private void FillBankDataGrid()
    {
        uiDgBankTransException.DataSource = ObjectDataSourceBankTransException;
        IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransException.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgBankTransException.DataSource = dve;
        uiDgBankTransException.DataBind();
    }
    
}
