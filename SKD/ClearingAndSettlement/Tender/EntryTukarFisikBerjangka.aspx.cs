using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_Tender_EntryTukarFisikBerjangka : System.Web.UI.Page
{
    

    #region "    Use Case    "
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!IsValidEntry())
                return;

            try
            {
                Tender.SaveTukarFisik(DateTime.Parse(uiDtpTenderDate.Text),
                                      decimal.Parse(uiCTLContract.LookupTextBoxID),
                                      decimal.Parse(uiCTLSellerInvestor.LookupTextBoxID),
                                      decimal.Parse(uiTxbSellerPrice.Text),
                                      int.Parse(uiTxbSellerQuantity.Text),
                                      uiDdlSellerTradePosition.SelectedValue,
                                      decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID),
                                      decimal.Parse(uiTxbBuyerPrice.Text),
                                      int.Parse(uiTxbBuyerQuantity.Text),
                                      uiDdlBuyerPosition.SelectedValue,
                                      uiTxbDeliveryLocation.Text,
                                      User.Identity.Name,
                                      DateTime.Parse(Session["BusinessDate"].ToString()));
                ClearControl();     

            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex);
            }
        }

    #endregion

    #region "   Supporting Method   "

    // IsValidEntry
    // Purpose      : To validate entry of the newly or modified record
    // Parameter    : -
    // Return       : Boolean whether the entry is valid based on required field
    private bool IsValidEntry()
    {
        bool isValid = true;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();


        if (uiDtpTenderDate.Text == "")
        {
            uiBlError.Items.Add("Tender date is required.");
        }
        if (uiCTLContract.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Contract is required.");
        }
        if (uiCTLSellerInvestor.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Seller investor is required.");
        }
        if (uiTxbSellerPrice.Text == "")
        {
            uiBlError.Items.Add("Seller price is required.");
        }
        if (uiTxbSellerQuantity.Text == "")
        {
            uiBlError.Items.Add("Seller quantity is required.");
        }
        if (uiCtlBuyerInvestor.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Buyer investor is required.");
        }
        if (uiTxbBuyerPrice.Text == "")
        {
            uiBlError.Items.Add("Buyer price is required.");
        }
        if (uiTxbBuyerQuantity.Text == "")
        {
            uiBlError.Items.Add("Buyer quantity is required.");
        }
        if (uiTxbDeliveryLocation.Text == "")
        {
            uiBlError.Items.Add("Delivery location is required.");
        }
        if (CtlClearingMemberLookup1.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Seller Clearing Member is required.");
        }
        if (CtlClearingMemberLookup2.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Buyer Clearing Member location is required.");
        }

        if (uiCTLContract.LookupTextBoxID != "" 
            && uiCTLSellerInvestor.LookupTextBoxID != ""
            && CtlClearingMemberLookup1.LookupTextBoxID != ""
            && CtlClearingMemberLookup2.LookupTextBoxID != ""
            && uiCtlBuyerInvestor.LookupTextBoxID != "")
        {
            DateTime endSpot = Contract.GetEndSpot(decimal.Parse(uiCTLContract.LookupTextBoxID));
            bool isExceed = false;
            if (DateTime.Parse(uiDtpTenderDate.Text) > endSpot)
            {
                uiBlError.Items.Add("Tender date is greater than end spot date of selected contract.");
            }

            if (Session["BusinessDate"] != null)
            {
                decimal sellerOpenPosition = Tender.SellerOpenPosition(uiDdlSellerTradePosition.SelectedValue,
                                        DateTime.Parse(Session["BusinessDate"].ToString()),
                                        decimal.Parse(uiCTLSellerInvestor.LookupTextBoxID),
                                        decimal.Parse(uiCTLContract.LookupTextBoxID),
                                        decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID));

                decimal buyerOpenPosition = Tender.BuyerOpenPosition(uiDdlBuyerPosition.SelectedValue,
                                             DateTime.Parse(Session["BusinessDate"].ToString()),
                                             decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID),
                                             decimal.Parse(uiCTLContract.LookupTextBoxID),
                                             decimal.Parse(CtlClearingMemberLookup2.LookupTextBoxID));

                if (sellerOpenPosition == 0)
                {
                    isExceed = true;
                    //uiBlError.Items.Add("Total requested position exceeds total available open position.");
                }
                else
                {
                    int availPosition = int.Parse(sellerOpenPosition.ToString()) - Tender.CountTenderRequsetQty(decimal.Parse(uiCTLContract.LookupTextBoxID),
                                         decimal.Parse(uiCTLSellerInvestor.LookupTextBoxID),
                                         uiDdlSellerTradePosition.SelectedValue,
                                         DateTime.Parse(Session["BusinessDate"].ToString()));

                    if (!string.IsNullOrEmpty(uiTxbSellerQuantity.Text))
                    {
                        if (int.Parse(uiTxbSellerQuantity.Text) > availPosition)
                        {
                            isExceed = true;
                            //uiBlError.Items.Add("Total requested position exceeds total available open position.");
                        }
                    }
                }

                if (buyerOpenPosition == 0)
                {
                    isExceed = true;
                    //uiBlError.Items.Add("Total requested position exceeds total available open position.");
                }
                else
                {
                    int availPosition = int.Parse(buyerOpenPosition.ToString()) -
                                        Tender.CountTenderResultReqQty(decimal.Parse(uiCTLContract.LookupTextBoxID),
                                        decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID),
                                        uiDdlBuyerPosition.SelectedValue,
                                        DateTime.Parse(Session["BusinessDate"].ToString()));

                    if (!string.IsNullOrEmpty(uiTxbBuyerQuantity.Text))
                    {
                        if (int.Parse(uiTxbBuyerQuantity.Text) > availPosition)
                        {
                            isExceed = true;
                            //uiBlError.Items.Add("Total requested position exceeds total available open position.");
                        }
                    }
                }
            }
            else
            {
                uiBlError.Items.Add("Start of day has not been executed yet.");
            }
            if (isExceed)
            {
                uiBlError.Items.Add("Total requested position exceeds total available open position.");
            }

            if (uiDdlSellerTradePosition.SelectedValue == "NT")
            { 
            
            }
            else if (uiDdlSellerTradePosition.SelectedValue == "BF")
            { 
            
            }
        }

        if (Session["BusinessDate"] != null)
        {
            if (uiDdlSellerTradePosition.SelectedValue == "NT")
            {
                if (!string.IsNullOrEmpty(uiCTLContract.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiCTLSellerInvestor.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiTxbSellerPrice.Text))
                {
                    bool isValidSeller = Tender.IsValidNewTradeSellerTukarFisikBerjangka(decimal.Parse(uiCTLContract.LookupTextBoxID),
                        decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), decimal.Parse(uiCTLSellerInvestor.LookupTextBoxID),
                        decimal.Parse(uiTxbSellerPrice.Text), DateTime.Parse(Session["BusinessDate"].ToString()));
                    if (!isValidSeller)
                    {
                        uiBlError.Items.Add("You are not allowed to entry TFB seller position.");
                    }
                }

            }
            else if (uiDdlSellerTradePosition.SelectedValue == "BF")
            {
                if (!string.IsNullOrEmpty(uiCTLContract.LookupTextBoxID) &&
                   !string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID) &&
                   !string.IsNullOrEmpty(uiCTLSellerInvestor.LookupTextBoxID) &&
                   !string.IsNullOrEmpty(uiTxbSellerPrice.Text))
                {
                    bool isValidSeller = Tender.IsValidBroughtForwardSellerTukarFisikBerjangka(decimal.Parse(uiCTLContract.LookupTextBoxID),
                        decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), decimal.Parse(uiCTLSellerInvestor.LookupTextBoxID),
                        decimal.Parse(uiTxbSellerPrice.Text), DateTime.Parse(Session["BusinessDate"].ToString()).AddDays(-1));
                    if (!isValidSeller)
                    {
                        uiBlError.Items.Add("You are not allowed to entry TFB seller position.");
                    }
                }
            }

            if (uiDdlBuyerPosition.SelectedValue == "NT")
            {
                if (!string.IsNullOrEmpty(uiCTLContract.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(CtlClearingMemberLookup2.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiCtlBuyerInvestor.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiTxbBuyerPrice.Text))
                {
                    bool isValidBuyer = Tender.IsValidNewTradeBuyerTukarFisikBerjangka(decimal.Parse(uiCTLContract.LookupTextBoxID),
                        decimal.Parse(CtlClearingMemberLookup2.LookupTextBoxID), decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID),
                        decimal.Parse(uiTxbBuyerPrice.Text), DateTime.Parse(Session["BusinessDate"].ToString()));
                    if (!isValidBuyer)
                    {
                        uiBlError.Items.Add("You are not allowed to entry TFB buyer position.");
                    }
                }
            }
            else if (uiDdlBuyerPosition.SelectedValue == "BF")
            {
                if (!string.IsNullOrEmpty(uiCTLContract.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(CtlClearingMemberLookup2.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiCtlBuyerInvestor.LookupTextBoxID) &&
                    !string.IsNullOrEmpty(uiTxbBuyerPrice.Text))
                {
                    bool isValidBuyer = Tender.IsValidBroughtForwardBuyerTukarFisikBerjangka(decimal.Parse(uiCTLContract.LookupTextBoxID),
                      decimal.Parse(CtlClearingMemberLookup2.LookupTextBoxID), decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID),
                      decimal.Parse(uiTxbBuyerPrice.Text), DateTime.Parse(Session["BusinessDate"].ToString()).AddDays(-1));
                    if (!isValidBuyer)
                    {
                        uiBlError.Items.Add("You are not allowed to entry TFB buyer position.");
                    }
                }
            }
        }
        
        
        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }

    // SetControlAccessByMakerChecker
    // Purpose      : Set Control visibility / enabled based on maker checker privilege
    // Parameter    : -
    // Return       : -
    private void SetControlAccessByMakerChecker()
    {
        MasterPage mp = (MasterPage)this.Master;

        bool pageMaker = mp.IsMaker;
        bool pageChecker = mp.IsChecker;
        bool pageViewer = mp.IsViewer;


        // Set control visibility
        uiBtnSave.Visible = pageMaker;
      
        // Set enabled for all textbox and dropdown list control only for maker
        if (pageChecker || pageViewer)
        {
            SetEnabledControl(Page, false);
        }
        else if (pageMaker)
        {
            SetEnabledControl(Page, true);
        }
    }

    // SetEnabledControl
    // Purpose      : Recursively set enable of each control (textbox, checkbox, drop down, calendar)
    // Parameter    : Page as Control container
    //                enabledControl as parameter to set
    // Return       : -
    private void SetEnabledControl(Control Page, bool enabledControl)
    {
        foreach (Control ctrl in Page.Controls)
        {
            if (ctrl.Controls.Count > 0)
            {
                SetEnabledControl(ctrl, enabledControl);
            }
            else if (ctrl is TextBox)
            {
                ((TextBox)ctrl).Enabled = enabledControl;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = enabledControl;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = enabledControl;
            }
            else if (ctrl is Controls_CtlCalendarPickUp)
            {
                ((Controls_CtlCalendarPickUp)ctrl).SetDisabledCalendar(!enabledControl);
            }
        }
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception that has been raised
    // Parameter    : -
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }

    private void ClearControl()
    {
        uiDtpTenderDate.SetCalendarValue("");
        uiCtlBuyerInvestor.SetInvestorValue("", "");
        uiCTLContract.SetContractCommodityValue("", "");
        uiCTLSellerInvestor.SetInvestorValue("", "");
        CtlClearingMemberLookup1.SetClearingMemberValue("", "");
        CtlClearingMemberLookup2.SetClearingMemberValue("", "");
        uiTxbBuyerPrice.Text = "";
        uiTxbBuyerQuantity.Text = "";
        uiTxbDeliveryLocation.Text = "";
        uiTxbSellerPrice.Text = "";
        uiTxbSellerQuantity.Text = "";
    }

    #endregion

    
}
