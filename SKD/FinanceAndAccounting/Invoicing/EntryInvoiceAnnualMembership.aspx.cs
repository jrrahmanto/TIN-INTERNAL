using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Invoicing_EntryInvoiceAnnualMembership : System.Web.UI.Page
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

            //CtlClearingMemberLookup1.InvestorCodeLookupID = CtlInvestorLookup1.ClearingMemberCodeLookupID;
            
            if (!Page.IsPostBack)
            {

                if (eType == "add")
                {
                    //if (uiDdlAccountType.SelectedValue == "RD" || uiDdlAccountType.SelectedValue == "RS")
                    //{
                    //trCM.Visible = true;
                    //trInvestor.Visible = false;
                    //}

                    //disable end date
                    //CtlCalendarPickUpEndDate.SetCalendarValue(null);
                    //CtlCalendarPickUpEndDate.SetDisabledCalendar(true);

                }
                else if (eType == "edit")
                {
                    //uiTxtAccountNo.Enabled = true;
                    //uiDdlBankCode.Enabled = true;
                    //CtlCalendarPickUpStartDate.SetDisabledCalendar(true);
                    //uiDdlCurrency.Enabled = true;

                    //bindData();
                }
            }
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

            //trAction.Visible = mp.IsChecker || mp.IsViewer;
            //trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            if (eType == "edit")
            {
                //uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = true;

            if (mp.IsChecker)
            {
                //uiTxtAccountNo.Enabled = false;
                //uiDdlAccountType.Enabled = false;
                //uiDdlCurrency.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                string pathInvoice = string.Format("{0}\\{3}\\{1}_{2}_Invoice.pdf", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_ANNUAL].ToString(), DateTime.Parse(CtlCalendarPickUp2.Text).ToString("yyyyMMdd"), CtlClearingMemberLookup1.LookupTextBox, CtlClearingMemberLookup1.LookupTextBox);
                string pathFakturPajak = string.Format("{0}\\{3}\\{1}_{2}_FakturPajak.pdf", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_ANNUAL].ToString(), DateTime.Parse(CtlCalendarPickUp2.Text).ToString("yyyyMMdd"), CtlClearingMemberLookup1.LookupTextBox, CtlClearingMemberLookup1.LookupTextBox);

                if (!Directory.Exists(string.Format("{0}\\{1}\\", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_ANNUAL].ToString(), CtlClearingMemberLookup1.LookupTextBox)))
                {
                    Directory.CreateDirectory(string.Format("{0}\\{1}\\", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_ANNUAL].ToString(), CtlClearingMemberLookup1.LookupTextBox));
                }

                uiUfInvoice.SaveAs(pathInvoice);
                uiUfFakturPajak.SaveAs(pathFakturPajak);

                InvoiceDataTableAdapters.InvoiceTableAdapter inta = new InvoiceDataTableAdapters.InvoiceTableAdapter();
                inta.Insert(
                    decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), "", "AM", DateTime.Parse(CtlCalendarPickUp2.Text), 0, 0,
                    DateTime.Now, null, null, pathInvoice, pathFakturPajak);

                ClearingMemberData.ClearingMemberDataTable dtCM = new ClearingMemberData.ClearingMemberDataTable();
                ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

                ta.FillByCode(dtCM, CtlClearingMemberLookup1.LookupTextBox);

                SMTPHelper.SendInvoice(dtCM[0].Email, pathInvoice, pathFakturPajak);

                Response.Redirect("ViewInvoiceAnnualMembership.aspx");
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
        Response.Redirect("ViewInvoiceAnnualMembership.aspx");
    }
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
                //if (string.IsNullOrEmpty(uiTxtApporvalDesc.Text))
                //{
                //    uiBLError.Items.Add("Approval description is required.");
                //}
            }

            //if (string.IsNullOrEmpty(uiTxtAccountNo.Text))
            //{
            //    uiBLError.Items.Add("Account No is required.");
            //}

            //validAccType = BankAccount.SelectAcctType(uiTxtAccountNo.Text);
            if (mp.IsMaker)
            {
                //if (validAccType == true)
                //{
                //    uiBLError.Items.Add("This account " + uiTxtAccountNo.Text + " has been recorded");
                //}
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

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(eID) != "")
            {
                BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                //if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
                //{
                //    if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
                //    {
                //        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                //                         "", uiDdlAccountType.SelectedValue,
                //                              Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                //                               Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null,
                //                                null,
                //                               Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID), uiDdlAccountStatus.SelectedValue);

                //    }
                //    else if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
                //    {
                //        BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                //                       "", uiDdlAccountType.SelectedValue,
                //                            Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                //                             Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null, null,
                //                             Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID), uiDdlAccountStatus.SelectedValue);
                //    }
                //}
                //else
                //{
                //    BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
                //                          "", uiDdlAccountType.SelectedValue,
                //                               Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
                //                                Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), Convert.ToDateTime(CtlCalendarPickUpEndDate.Text),
                //                                decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                //                                Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID), uiDdlAccountStatus.SelectedValue);
                //}

            }
            Response.Redirect("ViewInvoiceAnnualMembership.aspx");
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
                    //BankAccount.ApproveBankAccountID(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApporvalDesc.Text);
                }
                Response.Redirect("ViewInvoiceAnnualMembership.aspx");
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
                Response.Redirect("ViewInvoiceAnnualMembership.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
}