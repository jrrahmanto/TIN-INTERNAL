using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Parameter_EntryManageKBIAccount : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private decimal eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(Request.QueryString["eID"]);
            }
        }
        set { ViewState["eID"] = value; }
    }

   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            SetAccessPage();
            uiBLError.Visible = false;

            CtlClearingMemberLookup1.InvestorCodeLookupID = CtlInvestorLookup1.ClearingMemberCodeLookupID;
            

            if (!Page.IsPostBack)
            {
                
                if (eType == "add")
                {
                    if (uiDdlAccountType.SelectedValue == "RD" || uiDdlAccountType.SelectedValue == "RS")
                    {
                        trCM.Visible = true;
                        //trInvestor.Visible = false;
                    }
                    
                    //disable end date
                    CtlCalendarPickUpEndDate.SetCalendarValue(null);
                    CtlCalendarPickUpEndDate.SetDisabledCalendar(true);

                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtAccountNo.Enabled = true;
                    uiDdlBankCode.Enabled = true;
                    CtlCalendarPickUpStartDate.SetDisabledCalendar(true);
                    uiDdlCurrency.Enabled = true;
                    
                    bindData();
                }  
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
                {
                    //if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID) || string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
                    if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
                    {
                        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                                         "", uiDdlAccountType.SelectedValue,
                                              Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                                               Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null,
                                                null,
                                               Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);

                    }
                    else if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
                    {
                        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                                       "", uiDdlAccountType.SelectedValue,
                                            Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                                             Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null, null,
                                             Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
                    }
                    //else if (string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
                    //{
                    //    BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                    //                   "", uiDdlAccountType.SelectedValue,
                    //                        null,
                    //                         Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                    //                         Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID));
                    //}
                 }
                else
                {
                    BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                                          "", uiDdlAccountType.SelectedValue,
                                               Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                                                Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), Convert.ToDateTime(CtlCalendarPickUpEndDate.Text),
                                                decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                                                Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
                }
                
            }
            Response.Redirect("ViewManageBankAccount.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    BankAccount.ApproveBankAccountID(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApporvalDesc.Text );
                }

                BankDataTableAdapters.BankAccountTableAdapter bAccount = new BankDataTableAdapters.BankAccountTableAdapter();
                int counter = (int) bAccount.CounterBankAccountFullApproval(dr.CMID);

                if(counter < 2)
                {
                    BankDataTableAdapters.BankAccountTableAdapter baTa = new BankDataTableAdapters.BankAccountTableAdapter();

                    ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

                    ClearingMemberData.ClearingMemberDataTable cmdt = cmta.GetDataByCMID(dr.CMID);
                    BankData.BankAccountDataTable dtAll = baTa.GetBankAccountByCMID(dr.CMID);

                    SMTPHelper.SendInformasiVirtualAccount(cmdt[0].Email, dtAll);
                }

                Response.Redirect("ViewManageBankAccount.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {

                    BankAccount.RejectProposedBankAccountID(Convert.ToDecimal(eID), User.Identity.Name);

                }
                Response.Redirect("ViewManageBankAccount.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";

        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                // Case Update/Revision
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    //guard for number record
                    BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
                    BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(Convert.ToDecimal(uiDdlBankCode.SelectedValue), uiTxtAccountNo.Text, DateTime.Parse(CtlCalendarPickUpStartDate.Text), eID));
                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");

                    actionFlag = "U";
                }
                else
                {
                    //skip
                }

                if (uiDdlAccountType.SelectedValue == "RD"|| uiDdlAccountType.SelectedValue == "RS")
                {
                    if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
                    {
                        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                                     uiTxtApporvalDesc.Text, uiDdlAccountType.SelectedValue, 
                                           Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                                           Convert.ToDateTime(CtlCalendarPickUpStartDate.Text).Date, null,
                                           Convert.ToDecimal(CtlClearingMemberLookup1.LookupTextBoxID),
                                           Convert.ToDecimal(uiDdlCurrency.SelectedValue), actionFlag, User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
                    }
                    else
                    {
                        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                                     uiTxtApporvalDesc.Text, uiDdlAccountType.SelectedValue,
                                           Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                                           Convert.ToDateTime(CtlCalendarPickUpStartDate.Text).Date, Convert.ToDateTime(CtlCalendarPickUpEndDate.Text),
                                           Convert.ToDecimal(CtlClearingMemberLookup1.LookupTextBoxID),
                                           Convert.ToDecimal(uiDdlCurrency.SelectedValue), actionFlag, User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
                    }
                }
                
                Response.Redirect("ViewManageBankAccount.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }

    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageBankAccount.aspx");
    }

    protected void uiDdlAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (uiDdlAccountType.SelectedValue == "RTLK" || uiDdlAccountType.SelectedValue == "RTA")
            //{
            //    trCM.Visible = false;
            //    trInvestor.Visible = false;
            //}
            //else if (uiDdlAccountType.SelectedValue == "RTPS" || uiDdlAccountType.SelectedValue == "RTPU")
            if (uiDdlAccountType.SelectedValue == "RD" || uiDdlAccountType.SelectedValue == "RS")
            {
                //trInvestor.Visible = false;
                trCM.Visible = true;
            }
            //else if (uiDdlAccountType.SelectedValue == "RTI" || uiDdlAccountType.SelectedValue == "RNI")
            //{
            //    trInvestor.Visible = true;
            //    trCM.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }      
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool validAccType;
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApporvalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(uiTxtAccountNo.Text))
            {
                uiBLError.Items.Add("Account No is required.");
            }

            validAccType = BankAccount.SelectAcctType(uiTxtAccountNo.Text);
            if (mp.IsMaker)
            {
                if (validAccType == true)
                {
                    //isValid = false;
                    uiBLError.Items.Add("This account " + uiTxtAccountNo.Text + " has been recorded");
                }
            }

            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }

            

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }



    private void bindData()
    {
     
     try
     {
  
        BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
        ClearingMemberData.ClearingMemberRow drCM = null;
        InvestorData.InvestorRow drInvestor = null;  
        uiDdlBankCode.SelectedValue = dr.BankID.ToString();
        uiTxtAccountNo.Text = dr.AccountNo;
        uiDdlAccountType.SelectedValue = dr.AccountType.ToString();
        uiDdlCurrency.SelectedValue = dr.CurrencyID.ToString();
        CtlCalendarPickUpStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
        drCM = ClearingMember.SelectClearingMemberByCMID(dr.CMID.ToString());
        

            //cek accountStatus null
            if (!dr.IsAccountStatusNull())
            {
                if (dr.AccountStatus  == "R")
                {
                    uiDdlAccountStatus.SelectedValue = "R";
                }
                else if (dr.AccountStatus  == "A")
                {
                    uiDdlAccountStatus.SelectedValue="A" ;
                }
                
            }

            if (dr.IsEffectiveEndDateNull())
        {
            //disable end date
            CtlCalendarPickUpEndDate.SetCalendarValue(null);
            CtlCalendarPickUpEndDate.SetDisabledCalendar(true);

             if (uiDdlAccountType.SelectedValue == "RD" || uiDdlAccountType.SelectedValue == "RS")
            {

                CtlClearingMemberLookup1.SetDisabledClearingMember(false);
                CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString() +"-" + drCM.Name.ToString());
                //CtlInvestorLookup1.SetDisabledInvestor(true);
                //CtlInvestorLookup1.SetInvestorValue(dr.InvestorID.ToString(), dr.InvestorCode.ToString());
                //trInvestor.Visible = true;
            }

        }
        else
        {
            CtlCalendarPickUpEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
        }

         if (uiDdlAccountType.SelectedValue == "RD" || uiDdlAccountType.SelectedValue == "RS")
        {
            CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString() + "-" + drCM.Name.ToString());
            CtlClearingMemberLookup1.SetDisabledClearingMember(false);
            
           
            }

         if (!dr.IsInvestorIDNull ())
        { 
            drInvestor = Investor.FillInvestorbyInvestorId(dr.InvestorID);
            CtlInvestorLookup1.SetDisabledInvestor(false);
            CtlInvestorLookup1.SetInvestorValue(dr.InvestorID.ToString(), drInvestor.Code.ToString() + "-" + drInvestor.Name);
        }
            string actionDesc = "";
        //cek actionflag null
        if (!dr.IsActionFlagNull())
        {
            if (dr.ActionFlag == "I")
            {
                actionDesc = "New Record";
            }
            else if (dr.ActionFlag == "U")
            {
                actionDesc = "Revision";
            }
            else if (dr.ActionFlag == "D")
            {
                actionDesc = "Delete";
            }
        }
        uiTxtAction.Text = actionDesc;
     }
     catch (Exception ex)
     {
         uiBLError.Items.Add(ex.Message);
         uiBLError.Visible = true;
     }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            trAction.Visible = mp.IsChecker || mp.IsViewer;
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            if (eType == "edit")
            {
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;

            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                //BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));

                uiDdlBankCode.Enabled = false;
                uiTxtAccountNo.Enabled = false;
                uiDdlAccountType.Enabled = false;
                uiDdlCurrency.Enabled = false;
                uiTxtAction.Enabled = false;
                CtlCalendarPickUpStartDate.SetDisabledCalendar(true);
                //CtlCalendarPickUpStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
                CtlCalendarPickUpEndDate.SetDisabledCalendar(true);
                
                
                if (uiDdlAccountType.SelectedValue == "RD"|| uiDdlAccountType.SelectedValue == "RS")
                {
                    CtlClearingMemberLookup1.SetDisabledClearingMember(false);
                    //CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString());
                }
                

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    
}
