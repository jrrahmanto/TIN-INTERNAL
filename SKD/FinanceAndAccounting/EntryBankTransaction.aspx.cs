using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class FinanceAndAccounting_EntryBankTransaction : System.Web.UI.Page
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
                SaveBankTransaction();
                Response.Redirect("ViewBankTransaction.aspx");
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
        Response.Redirect("ViewBankTransaction.aspx");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                Bank.UpdateApprovalStatusBankTransaction("A", uiTxtApprovalDesc.Text,
                    User.Identity.Name, DateTime.Now, eID);

                Response.Redirect("ViewBankTransaction.aspx");
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
            Bank.UpdateApprovalStatusBankTransaction("R", uiTxtApprovalDesc.Text,
                User.Identity.Name, DateTime.Now, eID);

            Response.Redirect("ViewBankTransaction.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    //private void BindDataCM(decimal sourceBankAccID)
    //{
    //    try
    //    {
    //        BankData.BankAccountSearchRow drSource = BankAccount.SelectCMByBankAccountID(sourceBankAccID);
    //        uiTxtParticipant.Text = drSource.CMID.ToString() +"-"+ drSource.CMName;
    //        if (drSource.AccountType == "RD")
    //        {
    //            uiTxtAccType.Text = "Deposit";
    //        }
    //        else if (drSource.AccountType == "RS")
    //        {
    //            uiTxtAccType.Text = "Settlement";
    //        }


    //        //accountType = drDest.AccountType;
    //    }
    //    catch (Exception ex)
    //    {
    //        uiBLError.Items.Add(ex.Message);
    //        uiBLError.Visible = true;
    //    }   
    //}
    private void BindDataToForm()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        Object[] obj = (Object[])dv;
        BankData.BankTransactionRow dr = (BankData.BankTransactionRow)obj[0];

        CtlCalendarPickUpTransactionTime.SetCalendarValue(dr.TransactionTime.ToString("dd-MMM-yyyy"));
        uiTxtTransTime.Text = dr.TransactionTime.ToString("HH:mm");
        CtlCalendarPickUpReceiveTime.SetCalendarValue(dr.ReceiveTime.ToString("dd-MMM-yyyy"));
        //uiTxtReceiveTime.Text = dr.ReceiveTime.ToString("HH:mm");
        
        if (!dr.IsSourceAcctIDNull())
        {
            CtlBankAccountLookupSource.SetBankAccountValue(dr.SourceAcctID.ToString(), BankAccount.GetAccountNoBankAccountByBankAccountID(dr.SourceAcctID));
            //BindDataCM(dr.SourceAcctID);
        }
        
        uiTxtAmount.Text = dr.Amount.ToString("#,##0.00");
        
        uiDdlTransactionType.SelectedValue = dr.TransactionType;
        
        if (!dr.IsTransactionDescriptionNull())
        {
            uiTxtTransactionDescription.Text = dr.TransactionDescription;
        }
        if (!dr.IsApprovalDescNull())
        {
            uiTxtApprovalDesc.Text = dr.ApprovalDesc;
        }
        if (!dr.IsBankReferenceNull())
        {
            uiTxtBankReference.Text = dr.BankReference;
        }

    }


    private void SetEnabledControl(bool b)
    {
        if (eType == "edit")
        {
            CtlCalendarPickUpTransactionTime.DisabledCalendar = !b;
            uiTxtTransTime.Enabled = b;
            CtlCalendarPickUpReceiveTime.DisabledCalendar = !b;
            //uiTxtReceiveTime.Enabled = b;
            uiTxtAmount.Enabled = !b;
            //uiDdlMutationType.Enabled = b;
            uiDdlTransactionType.Enabled = b;
            //uiTxtNews.Enabled = b;
            uiTxtTransactionDescription.Enabled = b;
            uiTxtBankReference.Enabled = !b;
            CtlCalendarPickUpReceiveTime.DisabledCalendar = !b;
            uiTxtTransactionDescription.Enabled = !b;

        }
        if (eType == "edit" || eType == "add")
        {
            uiTxtBankReference.Enabled = !b;
            CtlCalendarPickUpReceiveTime.DisabledCalendar = !b;
            uiTxtTransactionDescription.Enabled = !b;
            uiTxtAmount.Enabled = !b;
            if (uiDdlTransactionType.SelectedValue == "O")
            {
                uiRbtDebit.Enabled = !b;
                uiRbtCredit.Enabled = !b;
            }
        }
    }


    private void SaveBankTransaction()
    {
        //string mutation;

        if (IsValidEntry() == true)
        {
            try
            {
                
                TimeSpan tsTransTime = TimeSpan.Parse(uiTxtTransTime.Text);
                
                //string mutation="";
                string tipeMutasi = "";
                //string trxType = "";

                //string[] arrMutation = CtlBankAccountLookupSource.LookupTextBox.ToString().Split('-');
                //for (int i = 0; i < arrMutation.Length; i++)
                //{
                //    mutation = arrMutation[i];
                //    if (mutation.Trim() == "Deposit")
                //    {
                //        trxType = "RD";
                //    }
                //    else if (mutation.Trim() == "Settlement")
                //    {
                //        trxType = "RS";
                //    }
                //}

                if (uiDdlTransactionType.SelectedValue == "D" || uiRbtCredit.Checked == true)
                {
                    tipeMutasi = "C";
                }
                else if (uiDdlTransactionType.SelectedValue == "W" || uiRbtDebit.Checked==true)
                {
                    tipeMutasi = "D";
                }


                Bank.InsertBankTrans("P", DateTime.Parse(CtlCalendarPickUpTransactionTime.Text).Date.Add(tsTransTime),
                DateTime.Parse(CtlCalendarPickUpReceiveTime.Text),
                decimal.Parse(uiTxtAmount.Text), tipeMutasi,
                uiDdlTransactionType.SelectedValue, uiTxtBankReference.Text, uiTxtTransactionDescription.Text,
                User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, null, decimal.Parse(CtlBankAccountLookupSource.LookupTextBoxID), ddlReferenceType.SelectedValue);



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
        bool isAccInv = true;
        decimal invID;
        InvestorData.InvestorRow drInvestor = null;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        if (eType == "add")
        {
            if (string.IsNullOrEmpty(CtlCalendarPickUpTransactionTime.Text))
            {
                uiBLError.Items.Add("Transaction date is required.");
            }
            if (string.IsNullOrEmpty(uiTxtTransTime.Text))
            {
                uiBLError.Items.Add("Transaction time is required.");
            }
            else
            {
                string[] strTime = uiTxtTransTime.Text.Split(":".ToCharArray());
                if (int.Parse(strTime[0]) < 0 || int.Parse(strTime[0]) > 23)
                {
                    uiBLError.Items.Add("Invalid transaction hour.");
                }
                if (int.Parse(strTime[1]) < 0 || int.Parse(strTime[1]) > 59)
                {
                    uiBLError.Items.Add("Invalid transaction minute.");
                }
            }
            if (string.IsNullOrEmpty(CtlCalendarPickUpReceiveTime.Text))
            {
                uiBLError.Items.Add("Receive date is required.");
            }
            
            if (!string.IsNullOrEmpty(uiTxtTransTime.Text) &&
                !string.IsNullOrEmpty(CtlCalendarPickUpTransactionTime.Text) &&
                !string.IsNullOrEmpty(CtlCalendarPickUpReceiveTime.Text))
            {
                TimeSpan tsTransTime = TimeSpan.Parse(uiTxtTransTime.Text);
                
            }
            
            decimal amount;
            if (decimal.TryParse(uiTxtAmount.Text, out amount) == false)
            {
                uiBLError.Items.Add("Invalid amount.");
            }
            
            if (string.IsNullOrEmpty(uiDdlTransactionType.SelectedValue))
            {
                uiBLError.Items.Add("Transaction type is required.");
            }

            if ((uiDdlTransactionType.SelectedValue == "W") || (uiDdlTransactionType.SelectedValue == "O" && uiRbtDebit.Checked == true))
            {
                string[] invParts = CtlBankAccountLookupSource.LookupTextBox.Split('-');
                for (int i = 0; i < invParts.Length; i++)
                {
                    if (i == 4)
                    {
                        invID = Convert.ToDecimal(invParts[i]);
                        isAccInv = Investor.FillAccStatusbyInvestorId(invID);
                        drInvestor = Investor.FillInvestorbyInvestorId(invID);
                       // break;
                    }
                        

                }
                if (isAccInv == true)
                {
                    uiBLError.Items.Add("Transaction is not allowed, AccountCode " + drInvestor.Code + " was suspended by " + drInvestor.SuspendedBy);
                }

            }
        }
        else if (eType == "edit" )
        {
            MasterPage mp = (MasterPage)this.Master;
            BankData.BankTransactionRow dr = Bank.GetBankTransactionByTransactionNo(eID);
            //if (dr.ApprovalStatus == "A")
            //{
            //    uiBLError.Items.Add("This record has been approved.");
            //}
            

            if (mp.IsMaker)
            {
                if (dr.ApprovalStatus == "P")
                {
                    uiBLError.Items.Add("This record not has been approved.");
                }
                if (dr.ApprovalStatus == "R")
                {
                    uiBLError.Items.Add("This record has been rejected.");
                }
            }
            
            if (mp.IsChecker)
            { 
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
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
        if (uiDdlTransactionType.SelectedValue == "O")
        {
            uiRbtCredit.Enabled = true;
            uiRbtDebit.Enabled = true;
        }
        else
        {
            uiRbtCredit.Enabled = false;
            uiRbtDebit.Enabled = false;
        }

    }

    private void SetDefaultValues()
    {
        CtlCalendarPickUpTransactionTime.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        uiTxtTransTime.Text = DateTime.Now.ToString("HH:mm");
        CtlCalendarPickUpReceiveTime.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        //uiTxtReceiveTime.Text = DateTime.Now.ToString("HH:mm");
       // uiRbtCredit.Enabled = false;
        //uiRbtDebit.Enabled = false; 
    }

    protected void uiBtnAddAndSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                SaveBankTransaction();
                Response.Redirect("EntryBankTransaction.aspx?eType=add");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    protected void uiDdlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (uiDdlTransactionType.SelectedValue == "O")
        {
            uiRbtCredit.Enabled = true;
            uiRbtDebit.Enabled = true;
        }
        else
        {
            uiRbtCredit.Enabled = false;
            uiRbtDebit.Enabled = false;
            uiRbtCredit.Checked = false;
            uiRbtDebit.Checked = false;
        }
    }


}
