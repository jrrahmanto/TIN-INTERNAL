using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class FinanceAndAccounting_EntryMutasiDepositDFS: System.Web.UI.Page
{
    private int eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(Request.QueryString["eID"].ToString());
            }
        }
    }

    private string eType
    {
        get { return Request.QueryString["eType"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //uiBtnLookupCMID.Attributes.Add("onclick", "window.open('../Lookup/ClearingMemberLookup.aspx?txt=" + uiTxtCMID.ClientID + "',null,'height=500,width=500,scrollbars=yes,resizable=yes');");
        SetAccessPage();

        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                SetEnabledControl(false);
                SetDefaultValues();

                
            }
            else if (eType == "edit")
            {
                BindDataToForm();
                SetEnabledControl(false);
            }
        }

        //if (eType == "add")
        //{
        //    if (uiDdlTransactionType.SelectedValue == "D")
        //    {
        //        // uiDdlMutationType.SelectedValue = "C";
        //        uiTxtAccType.Text = "Settlement";
        //    }
        //    else if (uiDdlTransactionType.SelectedValue == "W")
        //    {
        //        //uiDdlMutationType.SelectedValue = "D";
        //        uiTxtAccType.Text = "Deposit";
        //    }
            
        //}

        //if (!String.IsNullOrEmpty(CtlBankAccountLookupSource.LookupTextBoxID.ToString()))
        //{
        //    BindDataCM(decimal.Parse(CtlBankAccountLookupSource.LookupTextBoxID));
        //}
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                SaveMutationDeposit();
                Response.Redirect("ViewMutasiDepositDFS.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewMutasiDepositDFS.aspx");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                MutationDeposit.UpdateApprovalStatusMutationDeposit("A", uiTxtApprovalDesc.Text,
                    User.Identity.Name, DateTime.Now, eID);

                Response.Redirect("ViewMutasiDepositDFS.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            MutationDeposit.UpdateApprovalStatusMutationDeposit("R", uiTxtApprovalDesc.Text,
                User.Identity.Name, DateTime.Now, eID);

            Response.Redirect("ViewMutasiDepositDFS.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

        private void BindDataToForm()
    {

        //IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        IEnumerable dv = ObjectDataSourceBankTransaction.Select();
        Object[] obj = (Object[])dv;
        MutationDepositDFS.MutationDepositDFSRow dr =(MutationDepositDFS.MutationDepositDFSRow)obj[0];

        CtlCalendarPickUpReceiveTime.SetCalendarValue(dr.EntryDate.ToString("dd-MMM-yyyy"));
        
        //uiTxtReceiveTime.Text = dr.ReceiveTime.ToString("HH:mm");
        if (!dr.IsBankAcctIDNull())
        {
            CtlBankAccountLookupSource.SetBankAccountValue(dr.BankAcctID.ToString(), BankAccount.GetAccountNoBankAccountByBankAccountID(dr.BankAcctID));
            //BindDataCM(dr.SourceAcctID);
        }
        
        uiTxtAmount.Text = dr.Amount.ToString("#,##0.00");
        
        if (!dr.IsApprovalDescNull())
        {
            uiTxtApprovalDesc.Text = dr.ApprovalDesc;
        }
        

    }


    private void SetEnabledControl(bool b)
    {
        if (eType == "edit")
        {
            CtlCalendarPickUpReceiveTime.DisabledCalendar = !b;
            uiTxtAmount.Enabled = !b;
            
        }

        MasterPage mp = (MasterPage)this.Master;
        if (mp.IsChecker)
        {
            CtlCalendarPickUpReceiveTime.DisabledCalendar =b;
            uiTxtAmount.Enabled = b;
            CtlBankAccountLookupSource.DisabledLookupButton = !b; 
        }

        //if (eType == "edit" || eType == "add")
        //{
        //    //uiDdlAccountType.Enabled = b;
        //    //CtlClearingMemberLookupSource.DisabledLookupButton = b;
        //    //CtlClearingMemberLookupDestination.DisabledLookupButton = !b;
        //    //CtlInvestorLookupSource.DisabledLookupButton = !b;
        //    //CtlInvestorLookupDestination.DisabledLookupButton = !b;
        //    //CtlBankLookupSource.DisabledLookupButton = !b;
        //    //uiTxtSourceAccount.Enabled = b;
        //    uiTxtBankReference.Enabled = !b;
        //}
    }


    private void SaveMutationDeposit()
    {
        //string mutation;

        if (IsValidEntry() == true)
        {
            try
            {
                
                string mutation="";

                string[] arrMutation = CtlBankAccountLookupSource.LookupTextBox.ToString().Split('-');
                for (int i = 0; i < arrMutation.Length; i++)
                {
                    mutation = arrMutation[i];
                    if (mutation == "Deposit")
                    {
                        mutation = "D";
                    }
                    else if (mutation == "Settlement")
                    {
                        mutation = "S";
                    }
                }

                MutationDeposit.InsertMutationDeposit("P", DateTime.Parse(CtlCalendarPickUpReceiveTime.Text), decimal.Parse(uiTxtAmount.Text),
                    User.Identity.Name, DateTime.Now, decimal.Parse(CtlBankAccountLookupSource.LookupTextBoxID), User.Identity.Name, DateTime.Now, null);
                
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
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
            if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }

        if (string.IsNullOrEmpty(CtlCalendarPickUpReceiveTime.Text))
        {
            uiBLError.Items.Add("Entry date is required.");
        }

        decimal amount;
        if (decimal.TryParse(uiTxtAmount.Text, out amount) == false)
        {
            uiBLError.Items.Add("Invalid amount.");
        }
        if (string.IsNullOrEmpty(uiTxtAmount.Text))
        {
            uiBLError.Items.Add("Amount is required.");
        }

        if (string.IsNullOrEmpty(CtlBankAccountLookupSource.LookupTextBoxID.ToString()))
        {
            uiBLError.Items.Add("Bank account is required.");
        }


        if (eType == "edit" && mp.IsMaker)
        {
            MutationDepositDFS.MutationDepositDFSRow dr = MutationDeposit.GetMutationDepositbyMutationNo(eID);
            if (dr.ApprovalStatus == "P")
            {
                uiBLError.Items.Add("This record not has been approved.");
            }
            else if (dr.ApprovalStatus == "R")
            {
                uiBLError.Items.Add("This record has been rejected.");
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
        trApprovalDesc.Visible = mp.IsChecker;
        uiBtnSave.Visible = mp.IsMaker;
        uiBtnAddAndSave.Visible = mp.IsMaker;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
    }

    private void SetDefaultValues()
    {
        CtlCalendarPickUpReceiveTime.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        //uiTxtReceiveTime.Text = DateTime.Now.ToString("HH:mm");

    }

    protected void uiBtnAddAndSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                SaveMutationDeposit();
                Response.Redirect("EntryMutasiDepositDFS.aspx?eType=add");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }
}
