using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using AjaxControlToolkit;

public partial class FinanceAndAccounting_BulkApproveBankTransaction : System.Web.UI.Page
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
             //SetAccessPage();
            //uiBtnApprove.Visible = false;
            //uiBtnReject.Visible = false;
        }
    }
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryBankTransaction.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillBankDataGrid();
        //uiBtnApprove.Visible = true;
        //uiBtnReject.Visible = true;
    }

    protected void uiDgBankTransException_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            //Change SourceAcctID to Code
            Label sourceAcc = (Label)e.Row.FindControl("uiLblSourceAccount");
            sourceAcc.Text = Bank.GetBankCodeByBankID(int.Parse(sourceAcc.Text));

            //Change approval status to description
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
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        List<string> bankTransactionValueList = new List<string>();
        MasterPage mp = (MasterPage)this.Master;
        if (mp.IsChecker)
        {
            if (IsValidEntry() == true)
            {
                foreach (GridViewRow GridRow in uiDgBankTransaction.Rows)
                {

                    CheckBox selectedCheckBox = new CheckBox();
                    selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                    TextBox txtApprovalDesc = new TextBox();
                    txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));
                    string TransactionNo = uiDgBankTransaction.DataKeys[GridRow.RowIndex].Values[0].ToString();
                    // Guard for editing proposed record

                    Label uiLblSettleId = (Label)GridRow.FindControl("uiLblSettleId");
                    //SettlementPriceData.SettlementPriceRow dr = SettlementPrice.SelectSettlementBySettleID(Convert.ToDecimal(uiLblSettleId.Text));
                    BankData.BankTransactionRow dr = Bank.GetBankTransactionByTransactionNo(int.Parse(TransactionNo));

                    if (selectedCheckBox.Checked)
                    {
                        bankTransactionValueList.Add(dr.TransactionNo + "|" + txtApprovalDesc.Text + "|" + DateTime.Now.ToString() + "|" + User.Identity.Name);
                    }
                }
                if (uiBLError.Items.Count == 0)
                {
                    //approve
                    Bank.ApproveRejectBankTransactionFromView(bankTransactionValueList, DateTime.Now, User.Identity.Name, "A");
                    FillBankDataGrid();
                }
               

            }
        }
        
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
            
        }
       

        return isValid;
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        List<string> bankTransactionValueList = new List<string>();
         MasterPage mp = (MasterPage)this.Master;
        if (mp.IsChecker)
        {
            if (IsValidEntry() == true)
            {
                foreach (GridViewRow GridRow in uiDgBankTransaction.Rows)
                {

                    CheckBox selectedCheckBox = new CheckBox();
                    selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                    TextBox txtApprovalDesc = new TextBox();
                    txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));
                    string TransactionNo = uiDgBankTransaction.DataKeys[GridRow.RowIndex].Values[0].ToString();
                    // Guard for editing proposed record

                    Label uiLblSettleId = (Label)GridRow.FindControl("uiLblSettleId");
                    //SettlementPriceData.SettlementPriceRow dr = SettlementPrice.SelectSettlementBySettleID(Convert.ToDecimal(uiLblSettleId.Text));
                    BankData.BankTransactionRow dr = Bank.GetBankTransactionByTransactionNo(int.Parse(TransactionNo));


                    if (selectedCheckBox.Checked)
                    {
                        bankTransactionValueList.Add(dr.TransactionNo + "|" + txtApprovalDesc.Text + "|" + DateTime.Now.ToString() + "|" + User.Identity.Name);
                    }
                }
                //reject
                if (uiBLError.Items.Count == 0)
                {
                    Bank.ApproveRejectBankTransactionFromView(bankTransactionValueList, DateTime.Now, User.Identity.Name, "R");
                    FillBankDataGrid();
                }
              
            }
        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewBankTransaction.aspx");
    }
}
