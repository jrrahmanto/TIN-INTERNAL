using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using AjaxControlToolkit;

public partial class FinanceAndAccounting_ViewMutasiDepositDFS : System.Web.UI.Page
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
        uiDdlApprovalStatus.Focus();
        if (!Page.IsPostBack)
        {
            SetAccessPage();
            SetDefaultValues();
        }
    }
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnBulkApproval.Visible = mp.IsChecker;
        uiBtnCreate.Visible = mp.IsMaker;
    }

    private void SetDefaultValues()
    {
        CtlCalendarPickUp1.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        //uiTxtReceiveTime.Text = DateTime.Now.ToString("HH:mm");

    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryMutasiDepositDFS.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillBankDataGrid();
    }

    protected void uiBtnImport_Click(object sender, EventArgs e)
    {
        Response.Redirect("ImportMutasiDepositDFS.aspx");
    }
    protected void uiDgBankTransException_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            //Change SourceAcctID to Code
            //Label sourceAcc = (Label)e.Row.FindControl("uiLblSourceAccount");
            //sourceAcc.Text = Bank.GetBankCodeByBankID(int.Parse(sourceAcc.Text));

            ////Change approval status to description
            //Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            //switch (approvalStatus.Text)
            //{ 
            //    case "A" :
            //        approvalStatus.Text = "Approved";
            //        break;
            //    case "P" :
            //        approvalStatus.Text = "Proposed";
            //        break;
            //    case "R" :
            //        approvalStatus.Text = "Rejected";
            //        break;
            //    default :
            //        approvalStatus.Text = approvalStatus.Text;
            //        break;
            //}
        }
    }

    protected void uiDgBankTransException_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgBankTransaction.PageIndex = e.NewPageIndex;
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
        uiDgBankTransaction.DataSource = ObjectDataSourceBankTransaction;
        IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgBankTransaction.DataSource = dve;
        uiDgBankTransaction.DataBind();
    }
    
    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (mp.IsChecker)
        {
            foreach (GridViewRow GridRow in uiDgBankTransaction.Rows)
            {
                TextBox txtApprovalDesc = new TextBox();
                txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));
                CheckBox selectedCheckBox = new CheckBox();
                selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));

                if (selectedCheckBox.Checked == true)
                {
                    if (txtApprovalDesc.Text == "")
                    {
                        txtApprovalDesc.BorderColor = System.Drawing.Color.Red;
                        txtApprovalDesc.ToolTip = "Please fill approval desctiption.";
                        uiBLError.Items.Add("Approval description is required.");
                    }
                }
            }
            uiBLError.Visible = true;
        }
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    protected void uiBtnBulkApproval_Click(object sender, EventArgs e)
    {
        Response.Redirect("BulkApproveBankTransaction.aspx");
    }
}
