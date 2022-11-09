using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ClearingAndSettlement_MasterData_EntrySuretyBond : System.Web.UI.Page
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
           
            uiBLError.Visible = false;
            SetAccessPage();

            if (!Page.IsPostBack)
            {
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtEntryType.Enabled = false;
                    bindData();
                }
                else if (eType == "delete")
                {
                    uiTxtEntryType.Enabled = false;
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
                    SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";
                }

                SuretyBond.ProposeSuretyBond(uiTxtEntryType.Text, Convert.ToDecimal(CtlBondIssuer.LookupTextBoxID), uiTxtBondSN.Text,
                                             Convert.ToDecimal(CtlBankAccountLookup.LookupTextBoxID), Convert.ToDecimal(uiTxtAmount.Text),
                                             Convert.ToDecimal(uiTxtAmountHairCut.Text),
                                             DateTime.Parse(uiDtpExpiredDate.Text), DateTime.Parse(uiDtpExpDateHairCut.Text),
                                             uiDdlActiveStatus.SelectedValue, uiTxtNotes.Text, null, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                Response.Redirect("ViewManageSuretyBond.aspx");
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
        Response.Redirect("ViewManageSuretyBond.aspx");
    }
    
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                SuretyBond.ProposeSuretyBond(uiTxtEntryType.Text, Convert.ToDecimal(CtlBondIssuer.LookupTextBoxID), uiTxtBondSN.Text,
                                             Convert.ToDecimal(CtlBankAccountLookup.LookupTextBoxID), Convert.ToDecimal(uiTxtAmount.Text),
                                             Convert.ToDecimal(uiTxtAmountHairCut.Text),
                                             DateTime.Parse(uiDtpExpiredDate.Text), DateTime.Parse(uiDtpExpDateHairCut.Text),
                                             uiDdlActiveStatus.SelectedValue, uiTxtNotes.Text, null, "D", User.Identity.Name, Convert.ToDecimal(eID));

            }
            Response.Redirect("ViewManageSuretyBond.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    SuretyBond.ApproveSuretyBond(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text, uiTxtNotes.Text);
                }

                Response.Redirect("ViewManageSuretyBond.aspx");
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
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    SuretyBond.RejectProposedBondIssuer(Convert.ToDecimal(eID), User.Identity.Name);
                    //Product.RejectProposedProduct(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewManageSuretyBond.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }


    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;
            DateTime temp;
            string codeAcc = ""; 
            string fullCodeAcc = "";

            if (mp.IsMaker)
            {
                if (eType == "add" || eType == "edit")
                {
                    if (string.IsNullOrEmpty(uiTxtEntryType.Text))
                    { 
                        uiBLError.Items.Add("Entry Type is required.");
                    }
                    if (string.IsNullOrEmpty(CtlBondIssuer.LookupTextBox))
                    {
                        uiBLError.Items.Add("Bond Issuer is required.");
                    }
                    if (string.IsNullOrEmpty(uiTxtBondSN.Text))
                    {
                        uiBLError.Items.Add("Bond Serial Number is required.");
                    }

                    if (string.IsNullOrEmpty(CtlBankAccountLookup.LookupTextBox))
                    {
                        uiBLError.Items.Add("Account Code is required.");
                    }

                    if (string.IsNullOrEmpty(uiTxtAmount.Text))
                    {
                        uiBLError.Items.Add("Amount is required.");
                    }

                    if (string.IsNullOrEmpty(uiTxtAmountHairCut.Text))
                    {
                        uiBLError.Items.Add("Amount Haircut is required.");
                    }
                    if (eType == "add")
                    {
                        if (!string.IsNullOrEmpty(CtlBondIssuer.LookupTextBox) && !string.IsNullOrEmpty(uiTxtBondSN.Text))
                        {
                            bool isValidSurety = SuretyBond.ValidSuretyBondByIssuerAndSN(Convert.ToDecimal(CtlBondIssuer.LookupTextBoxID), uiTxtBondSN.Text);
                            if (isValidSurety == true)
                            {
                                uiBLError.Items.Add("Serial number " + uiTxtBondSN.Text + " and issuer " + CtlBondIssuer.LookupTextBox + " is already exists.");
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(CtlBondIssuer.LookupTextBox))
                    {
                        codeAcc = CtlBondIssuer.LookupTextBox.ToString().Substring(4, 2);
                        fullCodeAcc = CtlBondIssuer.LookupTextBox.ToString().Substring(0, 7);

                        if (codeAcc == "36" || codeAcc == "46")
                        {
                            uiBLError.Items.Add("Account Code " + fullCodeAcc + " is not allowed.");
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(uiDtpExpiredDate.Text) && !string.IsNullOrEmpty(uiDtpExpDateHairCut.Text))
                    {
                        DateTime dateExp = DateTime.Parse(uiDtpExpiredDate.Text);
                        DateTime dateExphairCut = DateTime.Parse(uiDtpExpDateHairCut.Text);

                        if (dateExp < dateExphairCut)
                        {
                            uiBLError.Items.Add("Expired Date cannot be less than Expired Date Haircut.");
                        }

                        if (DateTime.Now >= dateExp || DateTime.Now.Date >= dateExphairCut)
                        {
                            uiBLError.Items.Add("Expired Date or Expired Date Haircut cannot be less or equal than today.");
                        }
                    }
                    
                }

            }
             
            if (mp.IsChecker)
            {
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
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void bindData()
    {
        try
        {
            
            SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
            InvestorData.ViewLookupBankAccRow drInvestor = Investor.FillViewInvestorbyInvestorID(Convert.ToDecimal(dr.InvestorID));

            uiTxtEntryType.Text = dr.EntryType;
            CtlBondIssuer.SetExchangeValue(dr.BondIssuerID.ToString(), dr.IssuerName);
            uiTxtBondSN.Text = dr.BondSerialNo;
            CtlBankAccountLookup.SetBankAccountValue(dr.InvestorID.ToString(), drInvestor.Code + '-' + drInvestor.Name + '-' + drInvestor.CmName + '-' + drInvestor.CMID);
            uiTxtAmount.Text = dr.Amount.ToString("#,###.##");
            uiTxtAmountHairCut.Text = dr.HaircutPct; //dr.AmountHaircut.ToString("#,###.##");
            uiDtpExpiredDate.SetCalendarValue(dr.ExpiredDate.ToString("dd-MMM-yyyy"));
            uiDtpExpDateHairCut.SetCalendarValue(dr.ExpDateHaircut.ToString("dd-MMM-yyyy"));
            uiDdlActiveStatus.SelectedValue = dr.ActiveStatus;
            uiTxtNotes.Text = dr.Notes;

            //uiTxtProductName.Text = dr.ProductName;
            //uiDdlExchangeType.SelectedValue = dr.ExchangeType;

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
            if (!dr.IsDescriptionNull())
                uiTxtApprovalDesc.Text = dr.Description;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
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
                uiTxtEntryType.Enabled = mp.IsMaker;
                CtlBondIssuer.SetDisabledExchange(mp.IsMaker);
                uiTxtBondSN.Enabled = !mp.IsMaker;
                CtlBankAccountLookup.DisabledLookupButton = mp.IsMaker;
                uiTxtAmount.Enabled = mp.IsMaker;
                uiTxtAmountHairCut.Enabled = mp.IsMaker;
                uiDtpExpiredDate.DisabledCalendar = mp.IsMaker;
                uiDtpExpDateHairCut.DisabledCalendar = mp.IsMaker;
                uiDdlActiveStatus.Enabled = mp.IsMaker;
                uiTxtNotes.Enabled = mp.IsMaker;
                uiTxtAction.Enabled = mp.IsMaker;
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;


            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                // ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                uiTxtEntryType.Enabled = false;
                CtlBondIssuer.SetDisabledExchange(true);
                uiTxtBondSN.Enabled = false;
                CtlBankAccountLookup.DisabledLookupButton = true;
                uiTxtAmount.Enabled = false;
                uiTxtAmountHairCut.Enabled = false;
                uiDtpExpiredDate.DisabledCalendar = true;
                uiDtpExpDateHairCut.DisabledCalendar = true;
                uiDdlActiveStatus.Enabled = false;
                uiTxtNotes.Enabled = false;
                uiTxtAction.Enabled = false;
             }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion


}
