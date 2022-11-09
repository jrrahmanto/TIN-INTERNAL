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

public partial class ClearingAndSettlement_MasterData_EntrySuretyBondReverse : System.Web.UI.Page
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
                    //uiTxtEntryType.Enabled = false;
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
                    SuretyBondReverseData.ViewSuretyBondReverseRow dr = SuretyBondReverse.SelectSuretyBondByID(Convert.ToDecimal(eID));
                    if (!dr.IsApprovalStatusNull())
                    {
                        if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                        actionFlag = "U";
                    }
                    
                }
                
                SuretyBondReverse.ProposedSuretyBondReverse(Convert.ToDecimal(eID), actionFlag, Convert.ToDecimal(ftbBondRemainAmount.Text),
                                                            DateTime.Parse(uiDtpBuyerDefault.Text),uiTxtNotes.Text, "P", User.Identity.Name);
                Response.Redirect("ViewManageSuretyBondReverse.aspx");
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
        Response.Redirect("ViewManageSuretyBondReverse.aspx");
    }
    
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    if (Convert.ToString(eID) != "")
        //    {
        //        // Guard for editing proposed record
        //        SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
        //        if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

        //        SuretyBond.ProposeSuretyBond(uiTxtEntryType.Text, Convert.ToDecimal(CtlBondIssuer.LookupTextBoxID), uiTxtBondSN.Text,
        //                                     Convert.ToDecimal(CtlBankAccountLookup.LookupTextBoxID), Convert.ToDecimal(uiTxtAmount.Text),
        //                                     Convert.ToDecimal(uiTxtAmountHairCut.Text),
        //                                     DateTime.Parse(uiDtpExpiredDate.Text), DateTime.Parse(uiDtpExpDateHairCut.Text),
        //                                     uiDdlActiveStatus.SelectedValue, uiTxtNotes.Text, null, "D", User.Identity.Name, Convert.ToDecimal(eID));

        //    }
        //    Response.Redirect("ViewManageSuretyBond.aspx");
        //}
        //catch (Exception ex)
        //{
        //    uiBLError.Items.Add(ex.Message);
        //    uiBLError.Visible = true;
        //}
    }
    
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                SuretyBondReverseData.ViewSuretyBondReverseRow dr = SuretyBondReverse.SelectSuretyBondByID(Convert.ToDecimal(eID));
                if (!dr.IsApprovalStatusNull())
                {
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");
                    
                }
               

                if (Convert.ToString(eID) != "")
                {
                    SuretyBondReverse.ApprovedSuretyBondReverse(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }

                Response.Redirect("ViewManageSuretyBondReverse.aspx");
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
        //if (IsValidEntry() == true)
        //{
        //    try
        //    {
        //        // Guard for editing proposed record
        //        SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
        //        if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

        //        if (Convert.ToString(eID) != "")
        //        {
        //            SuretyBond.RejectProposedBondIssuer(Convert.ToDecimal(eID), User.Identity.Name);
        //            //Product.RejectProposedProduct(Convert.ToDecimal(eID), User.Identity.Name);
        //        }
        //        Response.Redirect("ViewManageSuretyBond.aspx");
        //    }
        //    catch (Exception ex)
        //    {
        //        uiBLError.Items.Add(ex.Message);
        //        uiBLError.Visible = true;

        //    }
        //}
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
            //DateTime temp;
            //string codeAcc = "";
            //string fullCodeAcc = "";

            if (mp.IsMaker)
            {
                if (eType == "add" || eType == "edit")
                {
                    if (string.IsNullOrEmpty(ftbBondRemainAmount .Text))
                    {
                        uiBLError.Items.Add("Bond Remain Amount is required.");
                    }
                    if (string.IsNullOrEmpty(uiDtpBuyerDefault.Text ))
                    {
                        uiBLError.Items.Add("Default Date is required.");
                    }
                    

                    if (string.IsNullOrEmpty(uiDdlReverseAmount.Text))
                    {
                        uiBLError.Items.Add("Reverse Ammount is required.");
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

            SuretyBondReverseData.ViewSuretyBondReverseRow dr = SuretyBondReverse.SelectSuretyBondByID(Convert.ToDecimal(eID));
            SuretyBondReverseData.SuretyBondReverseRow drReverse = SuretyBondReverse.GetSuretyReverseBySuretyBondID(Convert.ToDecimal(eID));
            //InvestorData.ViewLookupBankAccRow drInvestor = Investor.FillViewInvestorbyInvestorID(Convert.ToDecimal(dr.InvestorID));

            uiTxtAccCode.Text = dr.AccountCode;
            if (!dr.IsActionFlagNull())
            {
                uiTxtAction.Text = dr.ActionFlag;
            }
            uiTxtBondIssuerName.Text = dr.BondIssuerName;
            uiTxtBondSN.Text = dr.BondSerialNo;
            uiTxtBusDate.Text = dr.TransactionDate.ToString("dd-MMM-yyyy");
            if (!dr.IsDefaultDateNull())
            {
                uiDtpBuyerDefault.SetCalendarValue(dr.DefaultDate.ToString("dd-MMM-yyyy"));

            }
            else
            {
                //uiDtpBuyerDefault.SetCalendarValue(drReverse.BuyerDefault.ToString("dd-MMM-yyyy"));
            }
            
            uiTxtExchangeRef.Text = dr.ExchangeRef;

            uiTxtParticipantName.Text = dr.ParticipantName;
            
            if (!dr.IsRemainAmountNull())
            {
                ftbBondRemainAmount.Text = dr.RemainAmount.ToString("#,###.##");
                
            }

            ftbUsageAmount.Text = dr.UsageAmount.ToString("#,###.##");

            if (!dr.IsNotesNull())
                uiTxtNotes.Text = dr.Notes;

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
            if (!dr.IsApprovalDescNull())
                uiTxtApprovalDesc.Text = dr.ApprovalDesc;
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
            uiDtpBuyerDefault.DisabledCalendar = true;
            uiDtpBuyerDefault.SetDisabledCalendar(true);

            if (eType == "edit")
            {
                uiTxtAccCode.Enabled = false;
                uiTxtAction.Enabled = false; 
                uiTxtBondIssuerName.Enabled = false;
                uiTxtBondSN.Enabled = false;
                uiTxtBusDate.Enabled = false;
               
                
                uiTxtExchangeRef.Enabled = false;
                
                uiTxtParticipantName.Enabled = false;
                ftbBondRemainAmount.Enabled = false;
                ftbUsageAmount.Enabled = false;
                if (mp.IsChecker)
                {
                    
                    uiTxtNotes.Enabled = false;
                    uiTxtAction.Enabled = false;
                }
                else if (mp.IsMaker)
                {
                    
                    uiTxtNotes.Enabled = true;
                }
                               
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;


            // set disabled for other controls other than approval description, for checker
            //if (mp.IsChecker)
            //{
            //    // ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
            //    SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
            //    uiTxtEntryType.Enabled = false;
            //    CtlBondIssuer.SetDisabledExchange(true);
            //    uiTxtBondSN.Enabled = false;
            //    CtlBankAccountLookup.DisabledLookupButton = true;
            //    uiTxtAmount.Enabled = false;
            //    uiTxtAmountHairCut.Enabled = false;
            //    uiDtpExpiredDate.DisabledCalendar = true;
            //    uiDtpExpDateHairCut.DisabledCalendar = true;
            //    uiDdlActiveStatus.Enabled = false;
            //    uiTxtNotes.Enabled = false;
            //    uiTxtAction.Enabled = false;
            // }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion


}
