using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_EntryBankTransactionException : System.Web.UI.Page
{
    private DateTime TransDate
    {
        get { return DateTime.Parse(Request.QueryString["transDate"].ToString()); }
    }

    private int TransSeq
    {
        get { return int.Parse(Request.QueryString["transSeq"].ToString()); }
    }

    private int BankID
    {
        get { return int.Parse(Request.QueryString["bankId"].ToString()); }
    }

    private string ApprovalStatus
    {
        get { return Request.QueryString["approvalStatus"]; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //uiBtnLookupCMID.Attributes.Add("onclick", "window.open('../Lookup/ClearingMemberLookup.aspx?txt=" + uiTxtCMID.ClientID + "',null,'height=500,width=500,scrollbars=yes,resizable=yes');");
        SetAccessPage();

        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {

        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                BankData.RawBankTransactionRow dr = Bank.GetRawBankTransactionByTransDateSeqBankID(TransDate, TransSeq, BankID);
                decimal bankID = Bank.GetBankIDByBICode(dr.SourceBank);

                Bank.ProcessBankTransaction(TransDate, TransSeq, BankID, ApprovalStatus, "A", uiTxtApprovalDesc.Text, User.Identity.Name, DateTime.Now, 
                    "P", dr.TransactionTime, dr.ReceiveTime,
                    uiDdlAccountType.SelectedValue, null, null, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), null, null, null,
                    bankID, dr.SourceAccount, dr.Amount, dr.MutationType, dr.TransactionType,
                    dr.News, "", User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, "", "");

                //Bank.InsertBankTransaction("P", dr.TransactionTime, dr.ReceiveTime,
                //    uiDdlAccountType.SelectedValue, null, null, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), null, null, null,
                //    bankID, dr.SourceAccount, dr.Amount, dr.MutationType, dr.TransactionType,
                //    dr.News, "", User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, "");
                
                Response.Redirect("ViewBankTransactionException.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewBankTransactionException.aspx");
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        if (ApprovalStatus == "A")
        {
            uiBLError.Items.Add("This record has been approved.");
        }
        else if (ApprovalStatus == "R")
        {
            uiBLError.Items.Add("This record has been rejected.");
        }
        else
        {
            if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
            {
                uiBLError.Items.Add("Clearing Member ID is required.");
            }
            if (string.IsNullOrEmpty(uiDdlAccountType.SelectedValue))
            {
                uiBLError.Items.Add("AccountType is required.");
            }
            if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }        

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnSave.Visible = mp.IsMaker;
    }
}
