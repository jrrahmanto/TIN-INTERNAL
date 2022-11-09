using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class ClearingAndSettlement_MasterData_EntryContract : System.Web.UI.Page
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
                // bind data from database
                if (currentID != null)
                {
                    uiBtnRefresh.Enabled = false;
                    uiCtlCommodity.SetDisabledCommodity(true);
                    //uiDtpEffStartDate.SetDisabledCalendar(true);
                    BindData();
                }

                // Set Control access by maker checker
                SetControlAccessByMakerChecker();

                uiDtpStartDate.SetCalendarValue("");
                uiDtpStartSpot.SetCalendarValue("");
                uiDtpEndSpot.SetCalendarValue("");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    // Handler for save button
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidEntry())
            {
                string newcontract = "";
                string VMIRCA = null;
                string SettlementFactor = null;
                string CrossCurr = "NA"; //Default Karena Sudah tidak digunakan lagi
                string CrossCurrProduct = null;
                string TenderReqType = null;
                Nullable<decimal> PEG = 0;//Default Karena Sudah tidak digunakan lagi
                Nullable<int> dayref = null;
                Nullable<decimal> divisor = 0; //Default Karena Sudah tidak digunakan lagi
                Nullable<decimal> marginTender = null;
                Nullable<decimal> CalSpreadRemoteMargin = null;
                
               //---Add By Ramadhan 2014-07-01---\\
                // Penambahan 10 variable \\
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
                //-----End-----------------\\
                string quality = null;
                decimal regionID = 0;
                string commodityType = null;

                if (uiChkKIE.Checked)
                {
                    newcontract = "Y";
                }
                else
                {
                    newcontract = "N";
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
                //if (uiDtpEffEndDate.Text != "")
                //{
                //    EffEndDate = DateTime.Parse(uiDtpEffEndDate.Text);
                //}
                if (uiDdlVMIRCACal.SelectedValue != "")
                {
                    VMIRCA = uiDdlVMIRCACal.SelectedValue;
                }
                if (uiDdlSettlementFactor.SelectedValue != "")
                {
                    SettlementFactor = uiDdlSettlementFactor.SelectedValue;
                }
                //if (uiddlCrossCurrency.SelectedValue != "")
                //{
                //    CrossCurr = uiddlCrossCurrency.SelectedValue;
                //}
                if (uiDdlCrossCurrencyProduct.SelectedValue != "")
                {
                    CrossCurrProduct = uiDdlCrossCurrencyProduct.SelectedValue;
                }
                if (uiDdlTenderReqType.SelectedValue != "")
                {
                    TenderReqType = uiDdlTenderReqType.SelectedValue;
                }

                //---Add by Ramadhan 2014 - 06 - 16---\\
                // -- penambahan kondisi -- \\\\
                if (uiDdlbModeK1.SelectedValue != "")
                {
                    modeK1 = uiDdlbModeK1.SelectedValue;
                }
                if (uiDdlbModeK2.SelectedValue != "")
                {
                    modeK2 = uiDdlbModeK2.SelectedValue;
                }

                if (uiTxbValueK1.Text != "")
                {
                    valueK1 = Convert.ToDecimal(uiTxbValueK1.Text);
                }
                if (uiTxbValueK2.Text != "")
                {
                    valueK2 = Convert.ToDecimal(uiTxbValueK2.Text);
                }

                if (uiCtlContractRefK1.LookupTextBoxID != "")
                {
                    contractRefK1 = long.Parse(uiCtlContractRefK1.LookupTextBoxID);
                }
                if (uiCtlContractRefK2.LookupTextBoxID != "")
                {
                    contractRefK2 = long.Parse(uiCtlContractRefK2.LookupTextBoxID);
                }

                //if (uiTxbContractRefK.Text != "")
                //{
                //    contractRefK = long.Parse(uiTxbContractRefK.Text);
                //}

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

                //if (uiTxbContractRefD.Text != "")
                //{
                //    contractRefD = long.Parse(uiTxbContractRefD.Text);
                //}

                if (uiDdlbModeIM.SelectedValue != "")
                {
                    modeIM = uiDdlbModeIM.SelectedValue;
                }
                if (uiTxbPercentageSpotIM.Text != "")
                {
                    //fixedIM = long.Parse(uiTxbPercentageSpotIM.Text);
                    percentageSpotIM = Convert.ToDecimal(uiTxbPercentageSpotIM.Text);
                }

                if (uiTxbPercentageRemoteIM.Text != "")
                {
                    percentageRemoteIM = Convert.ToDecimal(uiTxbPercentageRemoteIM.Text);
                    //percentageIM = long.Parse((uiTxbPercentageRemoteIM.Text).ToString());
                }
                if (uiDdlbModeFee.SelectedValue != "")
                {
                    modeFee = uiDdlbModeFee.SelectedValue;
                }

                //------End-------\\

                quality = uiTxbQuality.Text;
                regionID = decimal.Parse(uiDdlRegional.SelectedValue);
                commodityType = uiTxbCommType.Text;

                //decimal commid = decimal.Parse(uiCtlCommodity.LookupTextBoxID);
                //int month = int.Parse(uiDtpMonthYear.Month);
                //int year = int.Parse(uiDtpMonthYear.Year);
                //DateTime efstart = DateTime.Parse(uiDtpEffStartDate.Text);
                //int conSize = int.Parse(uiTxbContractSize.Text);
                //DateTime startDate = DateTime.Parse(uiDtpStartDate.Text);
                //DateTime startSpot = DateTime.Parse(uiDtpStartSpot.Text);
                //DateTime endSpot = DateTime.Parse(uiDtpEndSpot.Text);
                //decimal marginSpot = decimal.Parse(uiTxbMarginSpot.Text);
                //decimal marRemote = decimal.Parse(uiTxbMarginRemote.Text);
                //decimal homCur = Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue);
                //decimal curID = decimal.Parse(currentID);



                if (currentID == null)
                {
                    Contract.Proposed(Convert.ToDecimal(uiCtlCommodity.LookupTextBoxID),
                                  int.Parse(uiDtpMonthYear.Month), int.Parse(uiDtpMonthYear.Year),
                                  DateTime.Parse(uiDtpStartDate.Text), decimal.Parse(uiTxbContractSize.Text.Replace(",", "")),
                                  uiTXbDescription.Text, uiTxbUnit.Text,
                                  DateTime.Parse(uiDtpStartDate.Text), DateTime.Parse(uiDtpEndSpot.Text),
                                  DateTime.Parse(uiDtpStartSpot.Text), PEG, VMIRCA,
                                  CrossCurr, SettlementFactor, dayref, divisor, marginTender,
                                  Convert.ToDecimal(uiTxbMarginSpot.Text), Convert.ToDecimal(uiTxbMarginRemote.Text),
                                  CalSpreadRemoteMargin, CrossCurrProduct,
                                  Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), DateTime.Parse(uiDtpEndSpot.Text),
                                  newcontract, uiDdlSettlementType.SelectedValue,
                                  User.Identity.Name, "I", null, TenderReqType, Convert.ToDecimal(uiCtlSubCategory.LookupTextBoxID),
                                  modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM, 
                                  percentageRemoteIM, modeFee, quality, regionID, commodityType);
                }
                else
                {
                    int asd = int.Parse(uiDtpMonthYear.Month);
                    int asdasd = int.Parse(uiDtpMonthYear.Year);
                    Contract.Proposed(decimal.Parse(uiCtlCommodity.LookupTextBoxID),
                                  int.Parse(uiDtpMonthYear.Month), int.Parse(uiDtpMonthYear.Year),
                                  DateTime.Parse(uiDtpStartDate.Text), decimal.Parse(uiTxbContractSize.Text.Replace(".00", "").Replace(",", "")),
                                  uiTXbDescription.Text, uiTxbUnit.Text,
                                  DateTime.Parse(uiDtpStartDate.Text), DateTime.Parse(uiDtpEndSpot.Text),
                                  DateTime.Parse(uiDtpStartSpot.Text), PEG, VMIRCA,
                                  CrossCurr, SettlementFactor, dayref, divisor, marginTender,
                                  decimal.Parse(uiTxbMarginSpot.Text), decimal.Parse(uiTxbMarginRemote.Text),
                                  CalSpreadRemoteMargin, CrossCurrProduct,
                                  Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), DateTime.Parse(uiDtpEndSpot.Text),
                                  newcontract, uiDdlSettlementType.SelectedValue,
                                  User.Identity.Name, "U", decimal.Parse(currentID), TenderReqType, decimal.Parse(uiCtlSubCategory.LookupTextBoxID),
                                  modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM, 
                                  percentageRemoteIM, modeFee, quality, regionID, commodityType);
                }
                
                // Redirect to summary page
                Response.Redirect("ViewContract.aspx");
            }
        }
        catch (Exception ex)
        {
            // Display error message
            DisplayErrorMessage(ex);
        }
    }

    // Handler for delete button
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string newcontract = "";
            string VMIRCA = null;
            string SettlementFactor = null;
            string CrossCurr = "NA"; //Default Karena Sudah tidak digunakan lagi
            string CrossCurrProduct = null;
            string TenderReqType = null;
            Nullable<decimal> PEG = 0; //Default Karena Sudah tidak digunakan lagi
            Nullable<int> dayref = null;
            Nullable<decimal> divisor = 0;//Default Karena Sudah tidak digunakan lagi
            Nullable<decimal> marginTender = null;
            Nullable<decimal> CalSpreadRemoteMargin = null;
            
           //---Add By Ramadhan 2014-06-16---\\
           //--Penambahan 10 variable--\\
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
            //-----End-----------------\\
            string quality = null;
            decimal regionID = 0;
            string commodityType = null;

            if (currentID != null)
            {
                if (uiChkKIE.Checked)
                {
                    newcontract = "Y";
                }
                else
                {
                    newcontract = "N";
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
                //if (uiDtpEffEndDate.Text != "")
                //{
                //    EffEndDate = DateTime.Parse(uiDtpEffEndDate.Text);
                //}
                if (uiDdlVMIRCACal.SelectedValue != "")
                {
                    VMIRCA = uiDdlVMIRCACal.SelectedValue;
                }
                if (uiDdlSettlementFactor.SelectedValue != "")
                {
                    SettlementFactor = uiDdlSettlementFactor.SelectedValue;
                }
                //if (uiddlCrossCurrency.SelectedValue != "")
                //{
                //    CrossCurr = uiddlCrossCurrency.SelectedValue;
                //}
                if (uiDdlCrossCurrencyProduct.SelectedValue != "")
                {
                    CrossCurrProduct = uiDdlCrossCurrencyProduct.SelectedValue;
                }
                if (uiDdlTenderReqType.SelectedValue != "")
                {
                    TenderReqType = uiDdlTenderReqType.SelectedValue;
                }
                // Add by Ramadhan 2014-07-02/
                //-- Penambahan Kondisi--\\
                if (uiDdlbModeK1.SelectedValue != "")
                {
                    modeK1 = uiDdlbModeK1.SelectedValue;
                }
                if (uiDdlbModeK2.SelectedValue != "")
                {
                    modeK2 = uiDdlbModeK2.SelectedValue;
                }

                if (uiTxbValueK1.Text != "")
                {
                    valueK1 = Convert.ToDecimal(uiTxbValueK1.Text);
                }
                if (uiTxbValueK2.Text != "")
                {
                    valueK2 = Convert.ToDecimal(uiTxbValueK2.Text);
                }

                if (uiCtlContractRefK1.LookupTextBoxID !="")
                {
                    contractRefK1 = long.Parse(uiCtlContractRefK1.LookupTextBoxID);
                }
                if (uiCtlContractRefK2.LookupTextBoxID != "")
                {
                    contractRefK2 = long.Parse(uiCtlContractRefK2.LookupTextBoxID);
                }
                //if (uiTxbContractRefK.Text != "")
                //{
                //    contractRefK = long.Parse(uiTxbContractRefK.Text);
                //}

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
                if (!string.IsNullOrEmpty(uiTxbQuality.Text))
                {
                    quality = uiTxbQuality.Text;
                }
                if (decimal.Parse(uiDdlRegional.SelectedValue) > 0)
                {
                    regionID = decimal.Parse(uiDdlRegional.SelectedValue);
                }
                if (!string.IsNullOrEmpty(uiTxbCommType.Text))
                {
                    commodityType = uiTxbQuality.Text;
                }

                Contract.Proposed(Convert.ToDecimal(uiCtlCommodity.LookupTextBoxID),
                                  int.Parse(uiDtpMonthYear.Month), int.Parse(uiDtpMonthYear.Year),
                                  DateTime.Parse(uiDtpStartDate.Text), decimal.Parse(uiTxbContractSize.Text),
                                  uiTXbDescription.Text, uiTxbUnit.Text,
                                  DateTime.Parse(uiDtpStartDate.Text), DateTime.Parse(uiDtpEndSpot.Text),
                                  DateTime.Parse(uiDtpStartSpot.Text), PEG, VMIRCA,
                                  CrossCurr, SettlementFactor,
                                  dayref, divisor, marginTender,
                                  Convert.ToDecimal(uiTxbMarginSpot.Text), Convert.ToDecimal(uiTxbMarginRemote.Text),
                                  CalSpreadRemoteMargin, CrossCurrProduct,
                                  Convert.ToDecimal(uiDdlHomeCurrency.SelectedValue), DateTime.Parse(uiDtpEndSpot.Text),
                                  newcontract, uiDdlSettlementType.SelectedValue,
                                  User.Identity.Name, "D", Convert.ToDecimal(currentID), TenderReqType, decimal.Parse(uiCtlSubCategory.LookupTextBoxID),
                                  modeK1, valueK1, contractRefK1, modeK2, valueK2, contractRefK2, modeD, valueD, contractRefD, modeIM, percentageSpotIM, 
                                  percentageRemoteIM, modeFee, quality, regionID, commodityType);
            }

            // Redirect to summary page
            Response.Redirect("ViewContract.aspx");
        }
        catch (Exception ex)
        {
            // Display error message
            DisplayErrorMessage(ex);
        }
    }

    // Handler for cancel button
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        // Redirect to summary page
        Response.Redirect("ViewContract.aspx");
    }

    // Handler for approve button
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                // Approve record
                Contract.Approve(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text, uiTxbAction.Text,
                                  User.Identity.Name);

                // Redirect to summary page
                Response.Redirect("ViewContract.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    // Handler for reject button
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                // Reject record
                Contract.Reject(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text, uiTxbAction.Text,
                                  User.Identity.Name);

                // Redirect to summary page
                Response.Redirect("ViewContract.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    #endregion

    #region "   Supporting Method   "

    // BindData
    // Purpose      : Binding data based on database
    // Parameter    : -
    // Return       : -
    private void BindData()
    {
        try
        {
            ContractData.ContractCommodityDataTable dt = new ContractData.ContractCommodityDataTable();
            dt = Contract.FillByContractID2(Convert.ToDecimal(currentID));

            if (dt.Count > 0)
            {
                uiTxbUnit.Text = dt[0].Unit;
                uiDtpMonthYear.SetMonthYear(dt[0].ContractMonth, dt[0].ContractYear);
                //uiDtpMonthYear.SetDisabledMonthYear(true);
                uiDtpStartDate.SetCalendarValue(dt[0].StartDate.ToString("dd-MMM-yyyy"));
                uiDtpStartSpot.SetCalendarValue(dt[0].StartSpot.ToString("dd-MMM-yyyy"));
                uiDtpEndSpot.SetCalendarValue(dt[0].EndSpot.ToString("dd-MMM-yyyy"));
                uiCtlCommodity.SetCommodityValue(dt[0].CommodityID.ToString(), dt[0].CommodityCode);
                if (!dt[0].IsDescriptionNull())
                {
                    uiTXbDescription.Text = dt[0].Description;
                }
                //if (!dt[0].IsPEGNull())
                //{
                //    uiTxbPEG.Text = dt[0].PEG.ToString("#,##0.00000");
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
                //    uiTxbDivisor.Text = dt[0].Divisor.ToString("#,##0.00000");
                //}
                if (!dt[0].IsMarginTenderNull())
                {
                    uiTxbMarginTender.Text = dt[0].MarginTender.ToString("#,##0.###");
                }
                uiTxbMarginSpot.Text = dt[0].MarginSpot.ToString("#,##0.###");
                uiTxbMarginRemote.Text = dt[0].MarginRemote.ToString("#,##0.###");
                if (!dt[0].IsCalSpreadRemoteMarginNull())
                {
                    uiTxbCalSpreadMargin.Text = dt[0].CalSpreadRemoteMargin.ToString("#,##0.###");
                }
                if (!dt[0].IsCrossCurrProductNull())
                {
                    uiDdlCrossCurrencyProduct.SelectedValue = dt[0].CrossCurrProduct;
                }
                uiDdlHomeCurrency.SelectedValue = dt[0].HomeCurrencyID.ToString();
                if (dt[0].NewContract == "Y")
                {
                    uiChkKIE.Checked = true;
                }
                else
                {
                    uiChkKIE.Checked = false;
                }
                uiDdlSettlementType.SelectedValue = dt[0].SettlementType;
                uiTxbContractSize.Text = dt[0].ContractSize.ToString("#,##0.###");

                if (!dt[0].IsTenderReqTypeNull())
                {
                    uiDdlTenderReqType.SelectedValue = dt[0].TenderReqType;
                }
                //---Add By Ramadhan 2014 - 07 - 02---\\\
                //-- Penambahan kondisi jika null--\\
                if (!dt[0].IsModeK1Null())
                {
                    uiDdlbModeK1.SelectedValue = dt[0].ModeK1.ToString();
                }
                if (!dt[0].IsModeK2Null())
                {
                    uiDdlbModeK2.SelectedValue = dt[0].ModeK2.ToString();
                }
                if (!dt[0].IsValueK1Null())
                {
                    uiTxbValueK1.Text = dt[0].ValueK1.ToString();
                }
                if (!dt[0].IsValueK2Null())
                {
                    uiTxbValueK2.Text = dt[0].ValueK2.ToString();
                }
                if (!dt[0].IsContractRefK2Null() && dt[0].ContractRefK2 != 0)
                {
                    //uiCtlContractRefK2.LookupTextBoxID = dt[0].ContractRefK2.ToString();
                    ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefK2);
                    uiCtlContractRefK2.SetContractCommodityValue(dt[0].ContractRefK2.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
              
                }
                if (!dt[0].IsContractRefK1Null() && dt[0].ContractRefK1 != 0)
                {
                    //uiCtlContractRefK1.LookupTextBoxID = dt[0].ContractRefK1.ToString();
                    ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefK1);
                    uiCtlContractRefK1.SetContractCommodityValue(dt[0].ContractRefK1.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
              
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
                   // uiCtlContractRefD.LookupTextBoxID = dt[0].ContractRefD.ToString();
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
                
                //-----End-----\\\\

                //new field sub category
                if (!dt[0].IsSubcategoryIDNull())
                {
                    uiCtlSubCategory.SetSubCategoryValue(dt[0].SubcategoryID.ToString(), SubCategory.GetCodeBySubCategoryID(dt[0].SubcategoryID));
                }

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

                //uiDtpEffStartDate.SetCalendarValue(dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
                //if (!dt[0].IsEffectiveEndDateNull())
                //{
                //    uiDtpEffEndDate.SetCalendarValue(dt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
                //}
                //else
                //{
                //    uiDtpEffEndDate.SetCalendarValue(null);
                //    uiDtpEffEndDate.SetDisabledCalendar(true);
                //}
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
            throw new ApplicationException(ex.Message);
        }
    }
    
    // DisplayErrorMessage
    // Purpose      : Validate entry
    // Parameter    : -
    // Return       : Boolean whether the entry is valid or not
    private bool IsValidEntry()
    {
        bool isError = false;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();

        // ---------- Validate required field ----------
        if (uiCtlCommodity.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Commodity is required.");
        }

        if (uiCtlSubCategory.LookupTextBoxID == "")
        {
            uiBlError.Items.Add("Sub Category is required.");
        }
        //if (uiDtpEffStartDate.Text == "")
        //{
        //    uiBlError.Items.Add("Effective Start Date is required.");
        //}
        if (uiDtpStartDate.Text == "")
        {
            uiBlError.Items.Add("Start Date is required.");
        }
        if (uiDtpStartSpot.Text == "")
        {
            uiBlError.Items.Add("Start Spot is required.");
        }
        if (uiDtpEndSpot.Text == "")
        {
            uiBlError.Items.Add("End Spot is required.");
        }
        if (uiTxbUnit.Text == "")
        {
            uiBlError.Items.Add("Unit is required.");
        }
        if (uiTxbMarginSpot.Text == "")
        {
            uiBlError.Items.Add("Margin Spot is required.");
        }
        if (uiTxbMarginRemote.Text == "")
        {
            uiBlError.Items.Add("Margin Remote is required.");
        }
        if (uiTxbContractSize.Text == "")
        {
            uiBlError.Items.Add("Contract Size is required.");
        }
        if (uiDtpMonthYear.Month == "")
        {
            uiBlError.Items.Add("Contract month is required.");
        }
        if (uiDtpMonthYear.Year == "")
        {
            uiBlError.Items.Add("Contract year is required.");
        }

        //if (uiddlCrossCurrency.Text == "")
        //{
        //    uiBlError.Items.Add("Cross currency is required.");
        //}

        // ------------ Validate date range -----------
        if (DateTime.Parse(uiDtpStartDate.Text) > DateTime.Parse(uiDtpStartSpot.Text))
        {
            uiBlError.Items.Add("Start Date should be earlier than Start Spot.");
        }
        if (DateTime.Parse(uiDtpStartDate.Text) > DateTime.Parse(uiDtpEndSpot.Text))
        {
            uiBlError.Items.Add("Start Date should be earlier than End Spot.");
        }
        if (DateTime.Parse(uiDtpStartSpot.Text) > DateTime.Parse(uiDtpEndSpot.Text))
        {
            uiBlError.Items.Add("Start Spot should be earlier than End Spot.");
        }

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

        // Show visibility of error message
        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isError = true;
        }

        return isError;
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
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
        uiCtlSubCategory.SetDisabledSubCategory(!pageMaker);
        uiCtlContractRefK1.SetDisabledContractCommodity(!pageMaker);
        uiCtlContractRefK2.SetDisabledContractCommodity(!pageMaker);
        uiCtlContractRefD.SetDisabledContractCommodity(!pageMaker);
       // uiCtlContractRefK1.SetDisabledSubCategory(!pageMaker);
        

        // Special for approval desc is enabled for checker
        uiTxbApprovalDesc.Enabled = pageChecker;
    }

    // SetEnabledControl
    // Purpose      : Recursively set enable of each control (textbox, checkbox, drop down, calendar)
    // Parameter    : Page as Control container
    //                enabledControl as parameter to set
    // Return       : -
    private void SetEnabledControl(Control Page, bool enabledControl)
    {
        uiTxbCalSpreadMargin.Enabled = enabledControl;
        uiTxbContractSize.Enabled = enabledControl;
        uiTxbDayReference.Enabled = enabledControl;
        //uiTxbDivisor.Enabled = enabledControl;
        uiTxbMarginRemote.Enabled = enabledControl;
        uiTxbMarginSpot.Enabled = enabledControl;
        //uiTxbPEG.Enabled = enabledControl;
        uiTxbUnit.Enabled = enabledControl;
        uiCtlCommodity.SetDisabledCommodity(!enabledControl);
        //uiddlCrossCurrency.Enabled = enabledControl;
        uiDdlCrossCurrencyProduct.Enabled = enabledControl;
        uiDdlHomeCurrency.Enabled = enabledControl;
        uiDdlSettlementFactor.Enabled = enabledControl;
        uiDdlSettlementType.Enabled = enabledControl;
        uiDdlTenderReqType.Enabled = enabledControl;
        uiDdlVMIRCACal.Enabled = enabledControl;
        //uiDtpEffEndDate.SetDisabledCalendar(!enabledControl);
        //uiDtpEffStartDate.SetDisabledCalendar(!enabledControl);
        uiTxbAction.Enabled = enabledControl;
        uiTxbMarginTender.Enabled = enabledControl;
        uiChkKIE.Enabled = enabledControl;
        uiDtpEndSpot.SetDisabledCalendar(!enabledControl);
        uiDtpMonthYear.SetDisabledMonthYear(!enabledControl);
        uiDtpStartDate.SetDisabledCalendar(!enabledControl);
        uiDtpStartSpot.SetDisabledCalendar(!enabledControl);
        uiTXbDescription.Enabled = enabledControl;

        //foreach (Control ctrl in Page.Controls)
        //{
        //    if (ctrl.Controls.Count > 0)
        //    {
        //        SetEnabledControl(ctrl, enabledControl);
        //    }
        //    else if (ctrl.ID == "uiTxbContractSize")
        //    {
        //        // skip
        //        ((EcCustomControls.EcTextBox.FilteredTextBox)ctrl).Enabled = enabledControl;
        //    }
        //    else if (ctrl is TextBox)
        //    {
        //        ((TextBox)ctrl).Enabled = enabledControl;
        //    }
        //    else if (ctrl is EcCustomControls.EcTextBox.FilteredTextBox)
        //    {
        //        ((EcCustomControls.EcTextBox.FilteredTextBox)ctrl).Enabled = enabledControl;
        //    }
        //    else if (ctrl is DropDownList)
        //    {
        //        ((DropDownList)ctrl).Enabled = enabledControl;
        //    }
        //    else if (ctrl is CheckBox)
        //    {
        //        ((CheckBox)ctrl).Enabled = enabledControl;
        //    }
        //    else if (ctrl is Controls_CtlCalendarPickUp)
        //    {
        //        ((Controls_CtlCalendarPickUp)ctrl).SetDisabledCalendar(!enabledControl);
        //    }
        //}
    }

    protected void uiBtnRefresh_Click(object sender, EventArgs e)
    {
        if (uiCtlCommodity.LookupTextBoxID != "")
        {
            //get data by commodityID
            CommodityData.CommProductExchDataTable dt = new CommodityData.CommProductExchDataTable();
            dt = Commodity.FillByCommodityID(decimal.Parse(uiCtlCommodity.LookupTextBoxID));

            uiDdlHomeCurrency.SelectedValue = dt[0].HomeCurrencyID.ToString();
            uiDdlSettlementType.SelectedValue = dt[0].SettlementType;
            //Add By Ramadhan 2014 - 07 - 08 \\\
            //--Penambahan saat tombol refresh di pilih--\\
            uiDdlbModeK1.SelectedValue = dt[0].ModeK1.ToString();
            uiDdlbModeK2.SelectedValue = dt[0].ModeK2.ToString();
            uiTxbValueK1.Text = dt[0].ValueK1.ToString();
            uiTxbValueK2.Text = dt[0].ValueK2.ToString();
            uiDdlbModeD.SelectedValue = dt[0].ModeD.ToString();
            uiTxbValueD.Text = dt[0].ValueD.ToString();
            uiDdlbModeIM.SelectedValue = dt[0].ModeIM.ToString();
            
            if (!dt[0].IsPercentageSpotIMNull())
            {
                uiTxbPercentageSpotIM.Text = dt[0].PercentageSpotIM.ToString();
            }
            else
            {
                uiTxbPercentageSpotIM.Text = "";
            }
            if (!dt[0].IsPercentageRemoteIMNull())
            {
                uiTxbPercentageRemoteIM.Text = dt[0].PercentageRemoteIM.ToString();
            }
            else
            {
                uiTxbPercentageRemoteIM.Text = "";
            }
            //uiTxbPercentageSpotIM.Text = dt[0].PercentageSpotIM.ToString();
            //uiTxbPercentageRemoteIM.Text = dt[0].PercentageRemoteIM.ToString();
            uiDdlbModeFee.SelectedValue = dt[0].ModeFee.ToString();

            if (!dt[0].IsContractRefDNull() && dt[0].ContractRefD != 0)
            {
                ContractData.ContractCommodityRow drCc = Contract.GetContractByContractID2(dt[0].ContractRefD);
                uiCtlContractRefD.SetContractCommodityValue(dt[0].ContractRefD.ToString(), drCc.CommodityCode + " " + drCc.ContractYear + drCc.ContractMonth);
            }

            //---END---\\\\

            if (!dt[0].IsUnitNull())
            {
                uiTxbUnit.Text = dt[0].Unit;
            }
            else
            {
                uiTxbUnit.Text = "";
            }
            
            //uiDtpEffStartDate.SetCalendarValue(dt[0].EffectiveStartDate.Date.ToString("dd-MMM-yyyy"));

            //if (!dt[0].IsEffectiveEndDateNull())
            //{
            //    uiDtpEffEndDate.SetCalendarValue(dt[0].EffectiveStartDate.Date.ToString("dd-MMM-yyyy"));
            //}

            if (!dt[0].IsCrossCurrProductNull())
            {
                uiDdlCrossCurrencyProduct.SelectedValue = dt[0].CrossCurrProduct;
            }
            else
            {
                uiDdlCrossCurrencyProduct.SelectedValue = "";
            }
           
            //uiddlCrossCurrency.SelectedValue = dt[0].CrossCurr.Trim();
           
            if (!dt[0].IsSettlementFactorNull())
            {
                uiDdlSettlementFactor.SelectedValue = dt[0].SettlementFactor;
            }
            else
            {
                uiDdlSettlementFactor.SelectedValue = "";
            }
            if (!dt[0].IsVMIRCACalTypeNull())
            {
                uiDdlVMIRCACal.SelectedValue = dt[0].VMIRCACalType;
            }
            else
            {
                uiDdlVMIRCACal.SelectedValue = "";
            }
            //if (!dt[0].IsPEGNull())
            //{
            //    uiTxbPEG.Text = dt[0].PEG.ToString("#,##0.00000");
            //}
            //else
            //{
            //    uiTxbPEG.Text = "";
            //}
            if (!dt[0].IsDayRefNull())
            {
                uiTxbDayReference.Text = dt[0].DayRef.ToString("#,##0.###");
            }
            else
            {
                uiTxbDayReference.Text = "";
            }
            uiTxbContractSize.Text = dt[0].ContractSize.ToString("#,##0.###");
            uiTxbMarginSpot.Text = dt[0].MarginSpot.ToString("#,##0.###");
            uiTxbMarginRemote.Text = dt[0].MarginRemote.ToString("#,##0.###");
            if (!dt[0].IsMarginTenderNull())
            {
                uiTxbMarginTender.Text = dt[0].MarginTender.ToString("#,##0.###");
            }
            else
            {
                uiTxbMarginTender.Text = "";
            }
            if (!dt[0].IsCalSpreadRemoteMarginNull())
            {
                uiTxbCalSpreadMargin.Text = dt[0].CalSpreadRemoteMargin.ToString("#,##0.###");
            }
            else
            {
                uiTxbCalSpreadMargin.Text = "";
            }
            //if (!dt[0].IsDivisorNull())
            //{
            //    uiTxbDivisor.Text = dt[0].Divisor.ToString("#,##0.00000");
            //}
            //else
            //{
            //    uiTxbDivisor.Text = "";
            //}
            //uiChkKIE.Checked = (dt[0].IsKIE == "1") ? true : false;
            //if (!dt[0].IsTenderReqTypeNull())
            //{
            //    uiDdlTenderReqType.SelectedValue = dt[0].TenderReqType;
            //}
            //else
            //{
            //    uiDdlTenderReqType.SelectedValue = "";
            //}

            if (!dt[0].IsQualityNull())
            {
                uiTxbQuality.Text = dt[0].Quality.ToString();
            }
            if (!dt[0].IsRegionIDNull())
            {
                uiDdlRegional.SelectedValue = dt[0].RegionID.ToString();
            }
            if (!dt[0].IsCommodityTypeNull())
            {
                uiTxbCommType.Text = dt[0].CommodityType.ToString();
            }            
        }
        else
        {
            //kosongkan semua field.
            uiTxbUnit.Text = "";
            //uiDtpEffStartDate.SetCalendarValue("");
            //uiDtpEffEndDate.SetCalendarValue("");
            uiDdlCrossCurrencyProduct.SelectedValue = "";
            //uiddlCrossCurrency.SelectedValue = "";
            uiDdlSettlementFactor.SelectedValue = "";
            uiDdlVMIRCACal.SelectedValue = "";
            //uiTxbPEG.Text = "";
            uiTxbContractSize.Text = "";
            uiTxbDayReference.Text = "";
            uiTxbMarginSpot.Text = "";
            uiTxbMarginRemote.Text = "";
            uiTxbMarginTender.Text = "";
            uiTxbCalSpreadMargin.Text = "";
            //uiTxbDivisor.Text = "";
            uiChkKIE.Checked = false;
            uiDdlTenderReqType.SelectedValue = "";
        }
    }

    #endregion

   
}

