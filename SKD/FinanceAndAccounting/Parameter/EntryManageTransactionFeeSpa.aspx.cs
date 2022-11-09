using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Parameter_EntryManageTransactionFeeSpa : System.Web.UI.Page
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
                SetDisableForModeFee(false);
                if (eType == "add")
                {
                    //disable end date
                    CtlCalendarEffectiveStartDate.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
                    CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                    uiDdlCmType.Enabled = false;
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

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageTransactionFeeSPA.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";

        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                long contractRefKurs = 0;
                if (uiCtlContractRefKurs.LookupTextBoxID != "")
                {
                    contractRefKurs = long.Parse(uiCtlContractRefKurs.LookupTextBoxID);
                }
                // Case Update/Revision
                if (eID != 0)
                {

                    // Guard for editing proposed record
                    TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    //guard for number record
                    TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeeSPATableAdapter();
                    TransactionFeeData.TransactionFeeSPADataTable dt = new TransactionFeeData.TransactionFeeSPADataTable();
                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(uiDdlCmType.SelectedValue,decimal.Parse(uiTxtUpperLimit.Text),
                                                                                            DateTime.Parse(CtlCalendarEffectiveStartDate.Text), eID));
                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");

                    actionFlag = "U";

                }

                Nullable<DateTime> EndDate = null;
                decimal pctClearingFee = 0;
                decimal pctExchangeFee = 0;
                decimal pctCSFee = 0;
                decimal pctThirdParty = 0;
                decimal clearingFee = 0;
                decimal exchangeFee = 0;
                decimal compensationFund = 0;
                decimal thirdPartyFee = 0;
                decimal csClearingFee = 0;
                decimal csExchangeFee = 0;
                decimal csCompensationFund = 0;
                decimal csThirdParty = 0;

                if (!string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEffectiveEndDate.Text);
                if (!string.IsNullOrEmpty(uiTxtExchangeFee.Text))

                /*Add by Zainab -24072014-*/
                if (!string.IsNullOrEmpty(uiTxtPctClearingFee.Text))
                    pctClearingFee = Convert.ToDecimal(uiTxtPctClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctExchangeFee.Text))
                    pctExchangeFee = Convert.ToDecimal(uiTxtPctExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctCompensationFee.Text))
                    pctCSFee = Convert.ToDecimal(uiTxtPctCompensationFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctThirdPartyFee.Text))
                    pctThirdParty = Convert.ToDecimal(uiTxtPctThirdPartyFee.Text);

                if (!string.IsNullOrEmpty(uiTxtClearingFee.Text))
                    clearingFee = Convert.ToDecimal(uiTxtClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtExchangeFee.Text))
                    exchangeFee = Convert.ToDecimal(uiTxtExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCompensationFund.Text))
                    compensationFund = Convert.ToDecimal(uiTxtCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtThirdPartyFee.Text))
                    thirdPartyFee = Convert.ToDecimal(uiTxtThirdPartyFee.Text);

                if (!string.IsNullOrEmpty(uiTxtCSClearingFee.Text))
                    csClearingFee = Convert.ToDecimal(uiTxtCSClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCSExchangeFee.Text))
                    csExchangeFee = Convert.ToDecimal(uiTxtCSExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCSCompensationFund.Text))
                    csCompensationFund = Convert.ToDecimal(uiTxtCSCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtCSThirdPartyFee.Text))
                    csThirdParty = Convert.ToDecimal(uiTxtCSThirdPartyFee.Text);

                TransactionFee.ProposeTransactionFeeSPA(decimal.Parse(uiDdlExchange.SelectedValue.ToString()), uiDdlModeFee.SelectedValue, uiDdlCmType.SelectedValue, 
                                                    Convert.ToDecimal(uiTxtUpperLimit.Text),
                                                    uiDdlModeKurs.SelectedValue,
                                                    Convert.ToDecimal(uiTxtFixedKurs.Text), contractRefKurs,
                                                     clearingFee,
                                                     exchangeFee,
                                                      compensationFund,
                                                     thirdPartyFee,
                                                     pctClearingFee,
                                                     pctExchangeFee,
                                                     pctCSFee,
                                                     pctThirdParty,
                                                     csClearingFee,
                                                     csExchangeFee,
                                                     csCompensationFund,
                                                     csThirdParty,
                                                     Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text),
                                                     EndDate, uiTxtApprovalDesc.Text, actionFlag, 
                                                     User.Identity.Name, Convert.ToDecimal(eID));

                Response.Redirect("ViewManageTransactionFeeSPA.aspx");
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
                long contractRefKurs = 0;
                if (uiCtlContractRefKurs.LookupTextBoxID != "")
                {
                    contractRefKurs = long.Parse(uiCtlContractRefKurs.LookupTextBoxID);
                }
                // Guard for editing proposed record
                TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                Nullable<DateTime> EndDate = null;
                decimal pctClearingFee = 0;
                decimal pctExchangeFee = 0;
                decimal pctCSFee = 0;
                decimal pctThirdParty = 0;
                decimal clearingFee = 0;
                decimal exchangeFee = 0;
                decimal compensationFund = 0;
                decimal thirdPartyFee = 0;
                decimal csClearingFee = 0;
                decimal csExchangeFee = 0;
                decimal csCompensationFund = 0;
                decimal csThirdParty = 0;

                if (!string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEffectiveEndDate.Text);
                if (!string.IsNullOrEmpty(uiTxtExchangeFee.Text))

                    /*Add by Zainab -24072014-*/
                    if (!string.IsNullOrEmpty(uiTxtPctClearingFee.Text))
                        pctClearingFee = Convert.ToDecimal(uiTxtPctClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctExchangeFee.Text))
                    pctExchangeFee = Convert.ToDecimal(uiTxtPctExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctCompensationFee.Text))
                    pctCSFee = Convert.ToDecimal(uiTxtPctCompensationFee.Text);
                if (!string.IsNullOrEmpty(uiTxtPctThirdPartyFee.Text))
                    pctThirdParty = Convert.ToDecimal(uiTxtPctThirdPartyFee.Text);

                if (!string.IsNullOrEmpty(uiTxtClearingFee.Text))
                    clearingFee = Convert.ToDecimal(uiTxtClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtExchangeFee.Text))
                    exchangeFee = Convert.ToDecimal(uiTxtExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCompensationFund.Text))
                    compensationFund = Convert.ToDecimal(uiTxtCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtThirdPartyFee.Text))
                    thirdPartyFee = Convert.ToDecimal(uiTxtThirdPartyFee.Text);

                if (!string.IsNullOrEmpty(uiTxtCSClearingFee.Text))
                    csClearingFee = Convert.ToDecimal(uiTxtCSClearingFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCSExchangeFee.Text))
                    csExchangeFee = Convert.ToDecimal(uiTxtCSExchangeFee.Text);
                if (!string.IsNullOrEmpty(uiTxtCSCompensationFund.Text))
                    csCompensationFund = Convert.ToDecimal(uiTxtCSCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtCSThirdPartyFee.Text))
                    csThirdParty = Convert.ToDecimal(uiTxtCSThirdPartyFee.Text);
                   

                TransactionFee.ProposeTransactionFeeSPA(decimal.Parse(uiDdlExchange.SelectedValue.ToString()), uiDdlModeFee.SelectedValue ,uiDdlCmType.SelectedValue, Convert.ToDecimal(uiTxtUpperLimit.Text),
                                                   uiDdlModeKurs.SelectedValue, Convert.ToDecimal(uiTxtFixedKurs.Text),
                                                   contractRefKurs,
                                                     clearingFee,
                                                     exchangeFee,
                                                     compensationFund,
                                                     thirdPartyFee,
                                                     pctClearingFee,
                                                     pctExchangeFee,
                                                     pctCSFee,
                                                     pctThirdParty,
                                                     csClearingFee,
                                                     csExchangeFee,
                                                     csCompensationFund,
                                                     csThirdParty ,
                                                     Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text),
                                                      EndDate,uiTxtApprovalDesc.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));


            }
            Response.Redirect("ViewManageTransactionFeeSPA.aspx");

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
                TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    TransactionFee.ApproveTransactionFeeSPA(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageTransactionFeeSPA.aspx");
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
                TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    TransactionFee.RejectProposedTransactionFeeSPA(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageTransactionFeeSPA.aspx");
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
            /*Add by Zainab -24072014-*/
            decimal pctClearingFee = 0;
            decimal pctExchangeFee = 0;
            decimal pctCSFee = 0;
            decimal pctThirdParty = 0;
            decimal clearingFee = 0;
            decimal exchangeFee = 0;
            decimal compensationFund = 0;
            decimal thirdPartyFee = 0;
            decimal csClearingFee = 0;
            decimal csExchangeFee = 0;
            decimal csCompensationFund = 0;
            decimal csThirdParty = 0;

            if (!string.IsNullOrEmpty(uiTxtPctClearingFee.Text))
                pctClearingFee = Convert.ToDecimal(uiTxtPctClearingFee.Text);
            if (!string.IsNullOrEmpty(uiTxtPctExchangeFee.Text))
                pctExchangeFee = Convert.ToDecimal(uiTxtPctExchangeFee.Text);
            if (!string.IsNullOrEmpty(uiTxtPctCompensationFee.Text))
                pctCSFee = Convert.ToDecimal(uiTxtPctCompensationFee.Text);
            if (!string.IsNullOrEmpty(uiTxtPctThirdPartyFee.Text))
                pctThirdParty = Convert.ToDecimal(uiTxtPctThirdPartyFee.Text);

            if (!string.IsNullOrEmpty(uiTxtClearingFee.Text))
                clearingFee = Convert.ToDecimal(uiTxtClearingFee.Text);
            if (!string.IsNullOrEmpty(uiTxtExchangeFee.Text))
                exchangeFee = Convert.ToDecimal(uiTxtExchangeFee.Text);
            if (!string.IsNullOrEmpty(uiTxtCompensationFund.Text))
                compensationFund = Convert.ToDecimal(uiTxtCompensationFund.Text);
            if (!string.IsNullOrEmpty(uiTxtThirdPartyFee.Text))
                thirdPartyFee = Convert.ToDecimal(uiTxtThirdPartyFee.Text);

            if (!string.IsNullOrEmpty(uiTxtCSClearingFee.Text))
                csClearingFee = Convert.ToDecimal(uiTxtCSClearingFee.Text);
            if (!string.IsNullOrEmpty(uiTxtCSExchangeFee.Text))
                csExchangeFee = Convert.ToDecimal(uiTxtCSExchangeFee.Text);
            if (!string.IsNullOrEmpty(uiTxtCSCompensationFund.Text))
                csCompensationFund = Convert.ToDecimal(uiTxtCSCompensationFund.Text);
            if (!string.IsNullOrEmpty(uiTxtCSThirdPartyFee.Text))
                csThirdParty = Convert.ToDecimal(uiTxtCSThirdPartyFee.Text);

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

            if (string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text))
            {
                uiBLError.Items.Add("Start date is required.");
            }


            if (!string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text) && !string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
            {
                if (DateTime.Parse(CtlCalendarEffectiveStartDate.Text) >= DateTime.Parse(CtlCalendarEffectiveEndDate.Text))
                    uiBLError.Items.Add("End date should be greater than start date.");
            }

            if (string.IsNullOrEmpty(uiTxtUpperLimit.Text))
            {
                uiBLError.Items.Add("Upper limit is required.");
            }
            /*if (string.IsNullOrEmpty(uiTxtClearingFee.Text))
            {
                uiBLError.Items.Add("Transaction Clearing fee is required.");
            }
            if (string.IsNullOrEmpty(uiTxtExchangeFee.Text))
            {
                uiBLError.Items.Add("Transaction Exchange fee is required.");
            }
            if (string.IsNullOrEmpty(uiTxtCompensationFund.Text))
            {
                uiBLError.Items.Add("Transaction Compensation fund is required.");
            }
            if (string.IsNullOrEmpty(uiTxtThirdPartyFee.Text))
            {
                uiBLError.Items.Add("Transaction third party fee is required.");
            }
            if (string.IsNullOrEmpty(uiTxtCSClearingFee.Text))
            {
                uiBLError.Items.Add("Cash Settlement Clearing fee is required.");
            }
            if (string.IsNullOrEmpty(uiTxtCSExchangeFee.Text))
            {
                uiBLError.Items.Add("Cash Settlement Exchange fee is required.");
            }
            if (string.IsNullOrEmpty(uiTxtCSCompensationFund.Text))
            {
                uiBLError.Items.Add("Cash Settlement Compensation Fund is required.");
            }
            if (string.IsNullOrEmpty(uiTxtCSThirdPartyFee.Text))
            {
                uiBLError.Items.Add("Cash Settlement third party fee is required.");
            }*/

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
            TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));
            uiDdlCmType.SelectedValue = dr.CMType;
            uiDdlModeFee.SelectedValue = dr.ModeFee;
            uiDdlExchange.SelectedValue = dr.ExchangeID.ToString();
            uiTxtUpperLimit.Text = Convert.ToDecimal(dr.UpperLimit).ToString("#,##0.###");
            uiDdlModeKurs.SelectedValue = dr.ModeKurs;
            uiTxtFixedKurs.Text = Convert.ToDecimal(dr.FixedKurs).ToString("#,##0.###");
            if (!dr.IsContractRefKursNull())
            {
                uiCtlContractRefKurs.LookupTextBoxID = dr.ContractRefKurs.ToString();
            }
            uiTxtClearingFee.Text = Convert.ToDecimal(dr.TransClearingFee).ToString("#,##0.###");
            uiTxtExchangeFee.Text = dr.TransExchangeFee.ToString("#,##0.###");
            uiTxtCompensationFund.Text = dr.TransCompensationFund .ToString("#,##0.###"); 
            uiTxtThirdPartyFee.Text = dr.TransThirdPartyFee.ToString("#,##0.###");
            if (!dr.IsPctClearingFeeNull())
            {
                uiTxtPctClearingFee.Text = Convert.ToDecimal(dr.PctClearingFee).ToString("#,##0.###");
            }
            if (!dr.IsPctExchangeFeeNull())
            {
                uiTxtPctExchangeFee.Text = Convert.ToDecimal(dr.PctExchangeFee).ToString("#,##0.###");
            }
            if (!dr.IsPctCompensationFundNull())
            {
                uiTxtPctCompensationFee.Text = Convert.ToDecimal(dr.PctCompensationFund).ToString("#,##0.###");
            }
            if (!dr.IsPctThirdPartyFeeNull())
            {
                uiTxtPctThirdPartyFee.Text = Convert.ToDecimal(dr.PctThirdPartyFee).ToString("#,##0.###");
            }
            uiTxtCSClearingFee.Text = Convert.ToDecimal(dr.CSClearingFee).ToString("#,##0.###");
            uiTxtCSExchangeFee.Text = dr.CSExchangeFee.ToString("#,##0.###");
            uiTxtCSCompensationFund.Text = dr.CSCompensationFund.ToString("#,##0.###");
            uiTxtCSThirdPartyFee.Text = dr.CSThirdPartyFee.ToString("#,##0.###");

            if (!dr.IsContractRefKursNull() && dr.ContractRefKurs != 0)
            {
                TransactionFeeData.ContractLookupRow cr = TransactionFee.SelectContractLookupByContractID(Convert.ToDecimal(dr.ContractRefKurs));
                uiCtlContractRefKurs.LookupTextBox = cr.CommodityCode + " " + cr.ContractYear + cr.ContractMonth.ToString("00");
            }
            else
            {
                uiCtlContractRefKurs.LookupTextBox = "";
            }

            CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (dr.IsEffectiveEndDateNull())
            {
                //disable end date
                CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
            }
            else
            {
                CtlCalendarEffectiveEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
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

            //cek approval description
            if (!dr.IsApprovalDescNull())
            {
                uiTxtApprovalDesc.Text = dr.ApprovalDesc;
            }

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
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;


            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                 TransactionFeeData.TransactionFeeSPARow dr = TransactionFee.SelectTransactionFeeSPAByTransactionFeeSPAID(Convert.ToDecimal(eID));

                uiDdlCmType.Enabled = false;
                uiTxtUpperLimit.Enabled = false;
                uiTxtClearingFee.Enabled = false;
                uiTxtExchangeFee.Enabled = false;
                uiTxtCompensationFund.Enabled = false;
                uiTxtThirdPartyFee.Enabled = false;
                uiTxtAction.Enabled = false;
                CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
                if (dr.IsEffectiveEndDateNull())
                {
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    CtlCalendarEffectiveEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
                }
            }
            SetDisableForModeFee(false);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion

    protected void uiDdlModeFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetDisableForModeFee(false);
        }
        catch (Exception ex)
        {
            
            throw new ApplicationException(ex.Message);
        }
    }

    private void SetDisableForModeFee(bool enable)
    {
        uiTxtPctClearingFee.Enabled = true;
        uiTxtPctCompensationFee.Enabled = true;
        uiTxtPctExchangeFee.Enabled = true;
        uiTxtPctThirdPartyFee.Enabled = true;
        uiTxtClearingFee.Enabled = true;
        uiTxtCompensationFund.Enabled = true;
        uiTxtExchangeFee.Enabled = true;
        uiTxtThirdPartyFee.Enabled = true;
        uiTxtCSClearingFee.Enabled = true;
        uiTxtCSCompensationFund.Enabled = true;
        uiTxtCSExchangeFee.Enabled = true;
        uiTxtCSThirdPartyFee.Enabled = true;

        if (uiDdlModeFee.SelectedValue == "LOT")
        {
            uiTxtPctClearingFee.Enabled = enable;
            uiTxtPctCompensationFee.Enabled = enable;
            uiTxtPctExchangeFee.Enabled = enable;
            uiTxtPctThirdPartyFee.Enabled = enable;
        }
        else if (uiDdlModeFee.SelectedValue == "PER")
        {
            uiTxtClearingFee.Enabled = enable;
            uiTxtCompensationFund.Enabled = enable;
            uiTxtExchangeFee.Enabled = enable;
            uiTxtThirdPartyFee.Enabled = enable;
        }
        else if (uiDdlModeFee.SelectedValue == "MAX")
        {
            uiTxtCSClearingFee.Enabled = enable;
            uiTxtCSCompensationFund.Enabled = enable;
            uiTxtCSExchangeFee.Enabled = enable;
            uiTxtCSThirdPartyFee.Enabled = enable;
        }
    }
}
