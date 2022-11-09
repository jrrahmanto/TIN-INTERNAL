using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_MasterData_EntryCommodity : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    #region "   Use Case   "

    // Handler for page load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {            
            try
            {
                if (currentID != null)
                {
                    BindData();
                }

                SetControlAccessByMakerChecker();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex);
            }
        }

    }

    // Handler for Save button
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (!IsValidEntry())
            return;

        try
        {
            string incentive = "";
            string iskie = "";
            string SettlementFactor = null;
            string VMIRCA = null;
            string CrossCurr = "NA"; /* Default because it didn't used anymore in entry commodity */
            string CrossCurrProduct = null;
            Nullable<decimal> refComID = null;
            Nullable<decimal> PEG = 1; /* Default because it didn't used anymore in entry commodity */
            Nullable<int> dayref = null;
            Nullable<decimal> divisor = 1; /* Default because it didn't used anymore in entry commodity */
            Nullable<decimal> marginTender = null;
            Nullable<decimal> CalSpreadRemoteMargin = null;
            Nullable<DateTime> EffEndDate = null;

            /* Added by Zainab*/
            string modeK1 = null;
            decimal valueK1 = 0;
            long contractRefK1 = 0;
            string modeK2 = null;
            decimal valueK2 = 0;
            long contractRefK2 = 0;
            string modeD = null;
            decimal valueD = 0;
            long contractRefD = 0;
            string modeIM = null;
            decimal percentageSpotIM = 0;
            decimal percentageRemoteIM = 0;
            string modeFee = null;

            string quality = null;
            decimal regionID = 0;
            string commodityType = null;

            if (uiCtlCommodity.LookupTextBoxID != "")
            {
                refComID = Convert.ToDecimal(uiCtlCommodity.LookupTextBoxID);
            }
            if (uiChkIncencitive.Checked)
            {
                incentive = "Y";
            }
            else
            {
                incentive = "N";
            }
            if (uiChkKIE.Checked)
            {
                iskie = "Y";
            }
            else
            {
                iskie = "N";
            }
            //if (uiTxbPEG.Text != "")
            //{
            //    PEG = Convert.ToDecimal(uiTxbPEG.Text);
            //}
            if (uiTxbDayReference.Text != "")
            {
                dayref = int.Parse(uiTxbDayReference.Text);
            }
            //if (uiTxbDivisor.Text != "")
            //{
            //    divisor = Convert.ToDecimal(uiTxbDivisor.Text);
            //}
            if (uiTxbMarginTender.Text != "")
            {
                marginTender = Convert.ToDecimal(uiTxbMarginTender.Text);
            }
            if (uiTxbCalSpreadMargin.Text != "")
            {
                CalSpreadRemoteMargin = Convert.ToDecimal(uiTxbCalSpreadMargin.Text);
            }
            if (uiDdlSettlementFactor.SelectedValue != "")
            {
                SettlementFactor = uiDdlSettlementFactor.SelectedValue;
            }
            if (uiDdlVMIRCACal.SelectedValue != "")
            {
                VMIRCA = uiDdlVMIRCACal.SelectedValue;
            }
            if (uiDtpEndDate.Text != "")
            {
                EffEndDate = DateTime.Parse(uiDtpEndDate.Text);
            }
            //if (uiddlCrossCurrency.SelectedValue != "")
            //{
            //    CrossCurr = uiddlCrossCurrency.SelectedValue;
            //}
            if (uiDdlCrossCurrencyProduct.SelectedValue != "")
            {
                CrossCurrProduct = uiDdlCrossCurrencyProduct.SelectedValue;
            }

            /* Added by Zainab*/
            if (uiDdlbModeK1.SelectedValue != "")
            {
                modeK1 = uiDdlbModeK1.SelectedValue;
            }
            if (uiTxbValueK1.Text != "")
            {
                valueK1 = Convert.ToDecimal(uiTxbValueK1.Text);
            }
            if (uiCtlContractRefK1.LookupTextBoxID != "")
            {
                contractRefK1 = long.Parse(uiCtlContractRefK1.LookupTextBoxID);
            }
            if (uiDdlbModeK2.SelectedValue != "")
            {
                modeK2 = uiDdlbModeK2.SelectedValue;
            }
            if (uiTxbValueK2.Text != "")
            {
                valueK2 = Convert.ToDecimal(uiTxbValueK2.Text);
            }
            if (uiCtlContractRefK2.LookupTextBoxID != "")
            {
                contractRefK2 = long.Parse(uiCtlContractRefK2.LookupTextBoxID);
            }
            if (uiDdlbModeD.SelectedValue != "")
            {
                modeD = uiDdlbModeD.SelectedValue;
            }
            
            if (uiTxbValueD.Text != "")
            {
                valueD = Convert.ToDecimal(uiTxbValueD.Text);
            }
            if (uiCtlContractRefD.LookupTextBoxID != "")
            {
                contractRefD = long.Parse(uiCtlContractRefD.LookupTextBoxID);
            }

            if (uiDdlbModeIM.SelectedValue != "")
            {
                modeIM = uiDdlbModeIM.SelectedValue;
            }

            if (uiTxbPercentageSpotIM.Text != "")
            {
                percentageSpotIM = Convert.ToDecimal(uiTxbPercentageSpotIM.Text);
            }
            if (uiTxbPercentageRemoteIM.Text != "")
            {
                percentageRemoteIM = Convert.ToDecimal(uiTxbPercentageRemoteIM.Text);
            }
            if (uiDdlbModeFee.SelectedValue != "")
            {
                modeFee = uiDdlbModeFee.SelectedValue;
            }

            /*-----End--------*/

            quality = uiTxbQuality.Text;
            regionID = decimal.Parse(uiDdlRegional.SelectedValue);
            commodityType = uiTxbCommType.Text;

            if (currentID == null)
            {
                // Insert new record
                Commodity.Proposed(Convert.ToDecimal(uiCtlProduct.LookupTextBoxID), Convert.ToDecimal(uiCtlExchange.LookupTextBoxID),
                              DateTime.Parse(uiDtpStartDate.Text), uiTxbCommodityCode.Text,
                              refComID, uiTxbUnit.Text,PEG, VMIRCA,CrossCurr, SettlementFactor,
                              dayref, divisor,uiTxbCommodityName.Text, marginTender,
                              Convert.ToDecimal(uiTxbMarginSpot.Text), Convert.ToDecimal(uiTxbMarginRemote.Text),
                              CalSpreadRemoteMargin, CrossCurrProduct,
                              Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), EffEndDate,
                              iskie, uiDdlSettlementType.SelectedValue,
                              int.Parse(uiTxbContractSize.Text.Replace(",","")), incentive,
                              User.Identity.Name, "I", null, uiDdlTenderReqType.SelectedValue,
                              modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM,
                              percentageRemoteIM, modeFee, quality, regionID, commodityType);
            }
            else
            {
                // Propose changes
                Commodity.Proposed(Convert.ToDecimal(uiCtlProduct.LookupTextBoxID), Convert.ToDecimal(uiCtlExchange.LookupTextBoxID),
                              DateTime.Parse(uiDtpStartDate.Text), uiTxbCommodityCode.Text,
                              refComID, uiTxbUnit.Text,PEG, VMIRCA,CrossCurr, SettlementFactor,
                              dayref, divisor, uiTxbCommodityName.Text, marginTender,
                              Convert.ToDecimal(uiTxbMarginSpot.Text), Convert.ToDecimal(uiTxbMarginRemote.Text),
                              CalSpreadRemoteMargin, CrossCurrProduct,
                              Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), EffEndDate,
                              iskie, uiDdlSettlementType.SelectedValue,
                              int.Parse(uiTxbContractSize.Text.Replace(",", "")), incentive,
                              User.Identity.Name, "U", Convert.ToDecimal(currentID), uiDdlTenderReqType.SelectedValue,
                              modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM,
                              percentageRemoteIM, modeFee, quality, regionID, commodityType);
            }

            // Redirect to summary page
            Response.Redirect("ViewComodity.aspx");
        }
        catch (Exception ex)
        {
            // Display error message
            DisplayErrorMessage(ex);
        }
    }

    // Handler for Delete button
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                string incentive = "";
                string iskie = "";
                string SettlementFactor = null;
                string VMIRCA = null;
                string CrossCurr = "NA"; /* Default because it didn't used anymore in entry commodity */
                string CrossCurrProduct = null;
                Nullable<decimal> refComID = null;
                Nullable<decimal> PEG = 1; /* Default because it didn't used anymore in entry commodity */
                Nullable<int> dayref = null;
                Nullable<decimal> divisor = 1; /* Default because it didn't used anymore in entry commodity */
                Nullable<decimal> marginTender = null;
                Nullable<decimal> CalSpreadRemoteMargin = null;
                Nullable<DateTime> endDate = null;

                /* Added by Zainab*/
                string modeK1 = null;
                decimal valueK1 = 0;
                long contractRefK1 = 0;
                string modeK2 = null;
                decimal valueK2 = 0;
                long contractRefK2 = 0;
                string modeD = null;
                decimal valueD = 0;
                long contractRefD = 0;
                string modeIM = null;
                decimal percentageSpotIM = 0;
                decimal percentageRemoteIM = 0;
                string modeFee = null;

                string quality = null;
                decimal regionID = 0;
                string commodityType = null;

                if (uiCtlCommodity.LookupTextBoxID != "")
                {
                    refComID = Convert.ToDecimal(uiCtlCommodity.LookupTextBoxID);
                }
                
                incentive = uiChkIncencitive.Checked ? "Y" : "N";

                iskie = uiChkKIE.Checked ? "Y" : "N";

                //if (uiTxbPEG.Text != "")
                //{
                //    PEG = Convert.ToDecimal(uiTxbPEG.Text);
                //}
                if (uiTxbDayReference.Text != "")
                {
                    dayref = int.Parse(uiTxbDayReference.Text);
                }
                //if (uiTxbDivisor.Text != "")
                //{
                //    divisor = Convert.ToDecimal(uiTxbDivisor.Text);
                //}
                if (uiTxbMarginTender.Text != "")
                {
                    marginTender = Convert.ToDecimal(uiTxbMarginTender.Text);
                }
                if (uiTxbCalSpreadMargin.Text != "")
                {
                    CalSpreadRemoteMargin = Convert.ToDecimal(uiTxbCalSpreadMargin.Text);
                }
                if (uiDtpEndDate.Text != "")
                {
                    endDate = DateTime.Parse(uiDtpEndDate.Text);
                }
                if (uiDdlSettlementFactor.SelectedValue != "")
                {
                    SettlementFactor = uiDdlSettlementFactor.SelectedValue;
                }
                if (uiDdlVMIRCACal.SelectedValue != "")
                {
                    VMIRCA = uiDdlVMIRCACal.SelectedValue;
                }
                //if (uiddlCrossCurrency.SelectedValue != "")
                //{
                //    CrossCurr = uiddlCrossCurrency.SelectedValue;
                //}
                if (uiDdlCrossCurrencyProduct.SelectedValue != "")
                {
                    CrossCurrProduct = uiDdlCrossCurrencyProduct.SelectedValue;
                }
                /* Added by Zainab*/
                if (uiDdlbModeK1.SelectedValue != "")
                {
                    modeK1 = uiDdlbModeK1.SelectedValue;
                }

                if (uiTxbValueK1.Text != "")
                {
                    valueK1 = Convert.ToDecimal(uiTxbValueK1.Text);
                }

                if (uiDdlbModeK2.SelectedValue != "")
                {
                    modeK2 = uiDdlbModeK2.SelectedValue;
                }

                if (uiTxbValueK1.Text != "")
                {
                    valueK1 = Convert.ToDecimal(uiTxbValueK1.Text);
                }

                if (uiDdlbModeD.SelectedValue != "")
                {
                    modeD = uiDdlbModeD.SelectedValue;
                }

                if (uiTxbValueD.Text != "")
                {
                    valueD = Convert.ToDecimal(uiTxbValueD.Text);
                }

                if (uiDdlbModeIM.SelectedValue != "")
                {
                    modeIM = uiDdlbModeIM.SelectedValue;
                }

                
                if (uiTxbPercentageSpotIM.Text != "")
                {
                     percentageSpotIM = Convert.ToDecimal(uiTxbPercentageSpotIM.Text);
                }
                if (uiTxbPercentageRemoteIM.Text != "")
                {
                    percentageRemoteIM = Convert.ToDecimal(uiTxbPercentageRemoteIM.Text);
                }
                if (uiDdlbModeFee.SelectedValue != "")
                {
                    modeFee = uiDdlbModeFee.SelectedValue;
                }

                /*-----End--------*/

                quality = uiTxbQuality.Text;
                regionID = decimal.Parse(uiDdlRegional.SelectedValue);
                commodityType = uiTxbCommType.Text;

                // Propose delete changes
                Commodity.Proposed(Convert.ToDecimal(uiCtlProduct.LookupTextBoxID), Convert.ToDecimal(uiCtlExchange.LookupTextBoxID),
                               DateTime.Parse(uiDtpStartDate.Text), uiTxbCommodityCode.Text,
                               refComID, uiTxbUnit.Text, PEG, VMIRCA, CrossCurr, SettlementFactor,
                               dayref, divisor, uiTxbCommodityName.Text, marginTender,
                               Convert.ToDecimal(uiTxbMarginSpot.Text), Convert.ToDecimal(uiTxbMarginRemote.Text),
                               CalSpreadRemoteMargin, CrossCurrProduct,
                               Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), endDate,
                               iskie, uiDdlSettlementType.SelectedValue,
                               int.Parse(uiTxbContractSize.Text.Replace(",", "")), incentive,
                               User.Identity.Name, "D", Convert.ToDecimal(currentID), uiDdlTenderReqType.SelectedValue,
                               modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM,
                               percentageRemoteIM, modeFee, quality, regionID, commodityType);

                // Redirect to summary page
                Response.Redirect("ViewComodity.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    // Handler for Approve button
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                // Validate for required field
                if (uiTxbApprovalDesc.Text == "")
                {
                    throw new ApplicationException("Approval description is required");
                }

                // Approve record
                Commodity.Approve(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text, uiTxbAction.Text,
                                  User.Identity.Name);

                // Redirect to summary page
                Response.Redirect("ViewComodity.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }
     
    // Handler for Reject button
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                // Reject record
                Commodity.Reject(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text, uiTxbAction.Text,
                                  User.Identity.Name);
                
                // Redirect to summary page
                Response.Redirect("ViewComodity.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    // Handler for Cancel button
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        // Redirect to summary page
        Response.Redirect("ViewComodity.aspx");
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


        if (uiCtlProduct.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Product is required.");
        }
        if (uiCtlExchange.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Exchange is required.");
        }
        if (uiDtpStartDate.Text == "")
        {
            uiBlError.Items.Add("Effective Start Date is required.");
        }
        if (uiTxbCommodityCode.Text == "")
        {
            uiBlError.Items.Add("Comodity Code is required.");
        }
        if (uiTxbCommodityName.Text == "")
        {
            uiBlError.Items.Add("Comodity Name is required.");
        }
        if (uiTxbMarginSpot.Text == "")
        {
            uiBlError.Items.Add("Spot margin is required.");
        }
        if (uiTxbMarginRemote.Text == "")
        {
            uiBlError.Items.Add("Remote margin is required.");
        }
        if (uiTxbContractSize.Text == "")
        {
            uiBlError.Items.Add("Contract Size is required.");
        }
        //if (uiddlCrossCurrency.SelectedValue == "PEG")
        //{
        //    if (uiTxbPEG.Text == "")
        //    {
        //        uiBlError.Items.Add("PEG is required.");
        //    }
        //}

        //------------- Validate Tender Request type---------
        if (uiDdlTenderReqType.Text != "")
        {
            if (uiTxbMarginTender.Text == "" || uiTxbMarginTender.Text == "0")
            {
                uiBlError.Items.Add("Margin tender is required");
            }
        }

        //------------- Validate Settlement type---------
        if (uiDdlSettlementType.SelectedValue == "G")
        {
            if (uiTxbDayReference.Text == "" || uiTxbDayReference.Text == "0")
            {
                uiBlError.Items.Add("Day reference is required");
            }
            if (uiCtlCommodity.LookupTextBox == "")
            {
                uiBlError.Items.Add("Reference product is required");
            }
        }

        if (string.IsNullOrEmpty(uiTxbCommType.Text))
        {
            uiBlError.Items.Add("Commodity Type is required");
        }
        if (string.IsNullOrEmpty(uiTxbQuality.Text))
        {
            uiBlError.Items.Add("Quality is required");
        }
        //------------- Validate Cross Currency---------
        //if (uiddlCrossCurrency.Text == "PEG")
        //{
        //    if (uiTxbPEG.Text == "" || uiTxbPEG.Text == "0")
        //    {
        //        uiBlError.Items.Add("PEG is required");
        //    }
        //}

        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }

    // BindData
    // Purpose      : Binding relevant field from database to UI control
    // Parameter    : -
    // Return       : -
    private void BindData()
    {
        try
        {
            CommodityData.CommProductExchDataTable dt = new CommodityData.CommProductExchDataTable();
            dt = Commodity.FillByCommodityID(Convert.ToDecimal(currentID));

            if (dt.Count > 0)
            {
                uiTxbCommodityCode.Text = dt[0].CommodityCode;
                uiCtlProduct.SetProductValue(dt[0].ProductID.ToString(), dt[0].ProductCode);
                uiCtlExchange.SetExchangeValue(dt[0].ExchangeId.ToString(), dt[0].ExchDesc);
                if (!dt[0].IsReffCommIDNull())
                {
                    uiCtlCommodity.SetCommodityValue(dt[0].ReffCommID.ToString(), Commodity.GetCommodityNameByCommID(dt[0].ReffCommID));
                }
                if (!dt[0].IsUnitNull())
                {
                    uiTxbUnit.Text = dt[0].Unit;
                }
                //if (!dt[0].IsPEGNull())
                //{
                //    uiTxbPEG.Text = dt[0].PEG.ToString("#,###.####");
                //}
                if (!dt[0].IsVMIRCACalTypeNull())
                {
                    uiDdlVMIRCACal.SelectedValue = dt[0].VMIRCACalType;
                }
                
                //uiddlCrossCurrency.SelectedValue = dt[0].CrossCurr.Trim();
                
                if (!dt[0].IsSettlementFactorNull())
                {
                    uiDdlSettlementFactor.SelectedValue = dt[0].SettlementFactor;
                }
                if (!dt[0].IsDayRefNull())
                {
                    uiTxbDayReference.Text = dt[0].DayRef.ToString();
                }
                //if (!dt[0].IsDivisorNull())
                //{
                //    uiTxbDivisor.Text = dt[0].Divisor.ToString("#,###.####");
                //}
                uiTxbCommodityName.Text = dt[0].CommName;
                if (!dt[0].IsMarginTenderNull())
                {
                    uiTxbMarginTender.Text = dt[0].MarginTender.ToString("#,###.##");
                }
                uiTxbMarginSpot.Text = dt[0].MarginSpot.ToString("#,###.##");
                uiTxbMarginRemote.Text = dt[0].MarginRemote.ToString("#,###.##");
                if (!dt[0].IsCalSpreadRemoteMarginNull())
                {
                    uiTxbCalSpreadMargin.Text = dt[0].CalSpreadRemoteMargin.ToString("#,###.##");
                }
                if (!dt[0].IsCrossCurrProductNull())
                {
                    uiDdlCrossCurrencyProduct.SelectedValue = dt[0].CrossCurrProduct;
                }
                uiDdlHomeCurrency.SelectedValue = dt[0].HomeCurrencyID.ToString();
                if (dt[0].IsKIE == "Y")
                {
                    uiChkKIE.Checked = true;
                }
                else
                {
                    uiChkIncencitive.Checked = false;
                }
                uiDdlSettlementType.SelectedValue = dt[0].SettlementType;
                uiTxbContractSize.Text = dt[0].ContractSize.ToString("#,###");
                if (!dt[0].IsIsIncentiveNull())
                {
                    if (dt[0].IsIncentive == "Y")
                    {
                        uiChkIncencitive.Checked = true;
                    }
                    else
                    {
                        uiChkIncencitive.Checked = false;
                    }
                }
                if (!dt[0].IsTenderReqTypeNull())
                {
                    uiDdlTenderReqType.SelectedValue = dt[0].TenderReqType;
                }

                uiDtpStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
                if (!dt[0].IsEffectiveEndDateNull())
                {
                    uiDtpEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
                }
                /* Add by Zainab */
                if (!dt[0].IsModeK1Null())
                {
                    uiDdlbModeK1.SelectedValue = dt[0].ModeK1.ToString();
                }                
                if (!dt[0].IsValueK1Null())
                {
                    uiTxbValueK1.Text = dt[0].ValueK1.ToString();
                }
                /*ini*/
                if (!dt[0].IsContractRefK1Null() && dt[0].ContractRefK1!= 0)
                {
                    ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefK1);
                    uiCtlContractRefK1.SetContractCommodityValue(dt[0].ContractRefK1.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
                }
                if (!dt[0].IsModeK2Null())
                {
                    uiDdlbModeK2.SelectedValue = dt[0].ModeK2.ToString();
                }
                if (!dt[0].IsValueK2Null())
                {
                    uiTxbValueK2.Text = dt[0].ValueK2.ToString();
                }
                if (!dt[0].IsContractRefK2Null() && dt[0].ContractRefK2 != 0)
                {
                    ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefK2);
                    uiCtlContractRefK2.SetContractCommodityValue(dt[0].ContractRefK2.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
              
                }
                if (!dt[0].IsModeDNull())
                {
                    uiDdlbModeD.SelectedValue = dt[0].ModeD.ToString();
                }
                if (!dt[0].IsValueDNull())
                {
                    uiTxbValueD.Text = dt[0].ValueD.ToString();
                }
                if (!dt[0].IsContractRefDNull() && dt[0].ContractRefD != 0)
                {
                    ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefD);
                    uiCtlContractRefD.SetContractCommodityValue(dt[0].ContractRefD.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
                }
                if (!dt[0].IsModeIMNull())
                {
                    uiDdlbModeIM.SelectedValue = dt[0].ModeIM.ToString();
                }
                if (!dt[0].IsPercentageSpotIMNull())
                {
                    uiTxbPercentageSpotIM.Text = dt[0].PercentageSpotIM.ToString();
                }
                if (!dt[0].IsPercentageRemoteIMNull())
                {
                    uiTxbPercentageRemoteIM.Text = dt[0].PercentageRemoteIM.ToString();
                }
                
                if (!dt[0].IsModeFeeNull())
                {
                    uiDdlbModeFee.SelectedValue = dt[0].ModeFee.ToString();
                }
                /*-----end-----*/

                if (!dt[0].IsQualityNull())
                {
                    uiTxbQuality.Text = dt[0].Quality;
                }
                if (!dt[0].IsRegionIDNull())
                {
                    uiDdlRegional.SelectedValue = dt[0].RegionID.ToString();
                }
                if (!dt[0].IsCommodityTypeNull())
                {
                    uiTxbCommType.Text = dt[0].CommodityType;
                }

                if (!dt[0].IsActionFlagNull())
                {
                    if (dt[0].ActionFlag == "I")
                    {
                        uiTxbAction.Text = "Insert";
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        uiTxbAction.Text = "Update";
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        uiTxbAction.Text = "Delete";
                    }
                }
            }

            dt.Dispose();

        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
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
        uiBtnDelete.Visible = pageMaker;
        uiBtnApprove.Visible = pageChecker;
        uiBtnReject.Visible = pageChecker;
        TRAction.Visible = pageChecker;
        TRApproval.Visible = pageChecker;

        // Set enabled for all textbox and dropdown list control only for maker
        if (pageChecker || pageViewer)
        {
            SetEnabledControl(Page, false);
        }
        else if (pageMaker)
        {
            SetEnabledControl(Page, true);
        }
        // Always set enabled as false
        uiTxbAction.Enabled = false;

        // Disable user defined control for pageMaker
        uiCtlCommodity.SetDisabledCommodity(!pageMaker);
        uiCtlExchange.SetDisabledExchange(!pageMaker);
        uiCtlProduct.SetDisabledProduct(!pageMaker);
        uiDtpEndDate.SetCalendarValue(null);
        uiDtpEndDate.SetDisabledCalendar(true);
        uiCtlContractRefK1.SetDisabledContractCommodity(!pageMaker);
        uiCtlContractRefK2.SetDisabledContractCommodity(!pageMaker);
        uiCtlContractRefD.SetDisabledContractCommodity(!pageMaker);
        //uiCtlContractRefK1.SetDisabledContractRefK1(!pageMaker); /*Zainab*/

        // Special for approval desc is enabled for checker
        uiTxbApprovalDesc.Enabled = pageChecker;
        uiDtpStartDate.SetCalendarValue("");
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

    #endregion

}
