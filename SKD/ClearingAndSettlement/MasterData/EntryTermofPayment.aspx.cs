using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClearingAndSettlement_MasterData_EntryTermofPayment : System.Web.UI.Page
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


            if (!Page.IsPostBack)
            {
                
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                    
                }
                else if (eType == "edit")
                {
                    
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
        //try
        //{
        //    if (Convert.ToString(eID) != "")
        //    {
        //        // Guard for editing proposed record
        //        BankData.BankAccountRow dr = BankAccount.SelectBankAccountByBankAccountID(Convert.ToDecimal(eID));
        //        if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

        //        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        //        {
        //            //if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID) || string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
        //            if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        //            {
        //                BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
        //                                 "", uiDdlAccountType.SelectedValue,
        //                                      Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
        //                                       Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null,
        //                                        null,
        //                                       Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);

        //            }
        //            else if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        //            {
        //                BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
        //                               "", uiDdlAccountType.SelectedValue,
        //                                    Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
        //                                     Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null, null,
        //                                     Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
        //            }
        //            //else if (string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBoxID))
        //            //{
        //            //    BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
        //            //                   "", uiDdlAccountType.SelectedValue,
        //            //                        null,
        //            //                         Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), null, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
        //            //                         Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID));
        //            //}
        //         }
        //        else
        //        {
        //            BankAccount.ProposeBankAccount(uiTxtAccountNo.Text, Convert.ToDecimal(uiDdlBankCode.SelectedValue),
        //                                  "", uiDdlAccountType.SelectedValue,
        //                                       Convert.ToDecimal(CtlInvestorLookup1.LookupTextBoxID),
        //                                        Convert.ToDateTime(CtlCalendarPickUpStartDate.Text), Convert.ToDateTime(CtlCalendarPickUpEndDate.Text),
        //                                        decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
        //                                        Convert.ToDecimal(uiDdlCurrency.SelectedValue), "D", User.Identity.Name, Convert.ToDecimal(eID),uiDdlAccountStatus.SelectedValue);
        //        }
                
        //    }
        //    Response.Redirect("ViewManageBankAccount.aspx");
        //}
        //catch (Exception ex)
        //{
        //    uiBLError.Items.Add(ex.Message);
        //    uiBLError.Visible = true;
        //}
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                TermPaymentData.TermPaymentRow dr = TermPayment.GetTermPaymentByID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    TermPayment.ApproveTermPayment(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApporvalDesc.Text);
                }
                Response.Redirect("ViewTermOfPayment.aspx");
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
                    TermPaymentData.TermPaymentRow dr = TermPayment.GetTermPaymentByID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    actionFlag = "U";
                }
                
                TermPayment.ProposeTermofPayment(Convert.ToDateTime(CtlCalendarPickUpStartDate.Text).Date,
                                                  Convert.ToDateTime(CtlCalendarPickUpEndDate.Text).Date,uiTxtNotes.Text,
                                                  null, actionFlag,User.Identity.Name, Convert.ToDecimal(eID)); 
                

                Response.Redirect("ViewTermofPayment.aspx");
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
        Response.Redirect("ViewTermofPayment.aspx");
    }
    

    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            //bool validStartDate;
            bool isValid = true;
            // DataTable dt = new DataTable();
            
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

            if (mp.IsMaker)
            {
                
                //valid start payment date
                isValid= Holiday.ValidHoliday(Convert.ToDateTime(CtlCalendarPickUpStartDate.Text));

                if (isValid == false)
                {

                    uiBLError.Items.Add("Start Payment Date is holiday");
                }

                //valid end payment date
                isValid = Holiday.ValidHoliday(Convert.ToDateTime(CtlCalendarPickUpEndDate.Text));

                if (isValid == false)
                {

                    uiBLError.Items.Add("End Payment Date is holiday");
                }

                //valid due payment date
                
                isValid = Holiday.ValidHoliday(Convert.ToDateTime(CtlCalendarPickUpEndDate.Text).AddDays(1));

                if (isValid == false)
                {

                    uiBLError.Items.Add("Due Date is holiday");
                }

                TermPaymentData.NewStartPaymentDateDataTable dt = TermPayment.ValidStartPaymentDate();

                if (dt.Rows.Count > 0)
                {
                    
                    if (CtlCalendarPickUpStartDate.Text != dt[0].DueDateKBIPKJ.ToString("dd-MMM-yyyy"))
                    {
                        isValid = false;
                        //uiBLError.Items.Add("Start Payment Date must higher than Due Date( " + dt[0].DueDateKBIPKJ.ToString("dd-MMM-yyyy") + " )");
                        uiBLError.Items.Add("Start Payment Date begin at " + dt[0].DueDateKBIPKJ.ToString("dd-MMM-yyyy"));
                    }

                    //if (Convert.ToDateTime(CtlCalendarPickUpStartDate.Text) != Convert.ToDateTime(dt[0].DueDateKBIPKJ.ToString("dd-MMM-yyyy")))
                    //{
                    //    isValid = false;
                    //    uiBLError.Items.Add("Start Payment Date begin at " + dt[0].DueDateKBIPKJ.ToString("dd-MMM-yyyy"));
                    //}

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

            TermPaymentData.TermPaymentRow dr = TermPayment.GetTermPaymentByID(eID);
              
            CtlCalendarPickUpStartDate.SetCalendarValue(dr.StartPaymentDate.ToString("dd-MMM-yyyy"));
            CtlCalendarPickUpEndDate.SetCalendarValue(dr.EndPaymentDate.ToString("dd-MMM-yyyy"));
            uiTxtNotes.Text = dr.Notes;
            if (dr.IsApprovalDescNull())
            {
                uiTxtApporvalDesc.Text = "";
            }
            else
            {
                uiTxtApporvalDesc.Text = dr.ApprovalDesc;
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

            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
            //trAction.Visible = mp.IsChecker || mp.IsViewer;
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            if (eType == "edit")
            {
                uiBtnDelete.Visible = mp.IsMaker;
            }
           
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    
}
