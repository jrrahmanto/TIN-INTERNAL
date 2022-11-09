using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Parameter_EntryManageTransactionFeePrimer : System.Web.UI.Page
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
                    //disable end date
                    CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    CtlCommodityLookup1.SetDisabledCommodity(true);
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
        Response.Redirect("ViewManageTransactionFeePrimer.aspx");
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
                    TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    //guard for number record
                    TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter ta = new TransactionFeeDataTableAdapters.TransactionFeePrimerTableAdapter();
                    TransactionFeeData.TransactionFeePrimerDataTable dt = new TransactionFeeData.TransactionFeePrimerDataTable();
                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(decimal.Parse(CtlCommodityLookup1.LookupTextBoxID), uiDdlCmType.SelectedValue,
                                                                                            DateTime.Parse(CtlCalendarEffectiveStartDate.Text), eID));
                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");

                    actionFlag = "U";

                }

                Nullable<DateTime> EndDate = null;
               

                if (!string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEffectiveEndDate.Text);
                

                TransactionFee.ProposeTransactionFeePrimer(decimal.Parse(uiDdlExchange.SelectedValue.ToString()), decimal.Parse(CtlCommodityLookup1.LookupTextBoxID), 
                                                         uiDdlCmType.SelectedValue,
                                                         Convert.ToDecimal(uiTxtUpperLimit.Text),
                                                         Convert.ToDecimal(uiTxtClearingFee.Text), 
                                                         Convert.ToDecimal(uiTxtExchangeFee.Text),
                                                         Convert.ToDecimal(uiTxtCompensationFund.Text),
                                                         Convert.ToDecimal(uiTxtThirdPartyFee.Text),
                                                         Convert.ToDecimal(uiTxtCSClearingFee.Text),
                                                         Convert.ToDecimal(uiTxtCSExchangeFee.Text),
                                                         Convert.ToDecimal(uiTxtCSCompensationFund.Text),
                                                         Convert.ToDecimal(uiTxtCSThirdPartyFee.Text),
                                                         Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text),
                                                         EndDate,uiTxtApprovalDesc.Text, actionFlag, 
                                                         User.Identity.Name, Convert.ToDecimal(eID));

                Response.Redirect("ViewManageTransactionFeePrimer.aspx");
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
                TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                Nullable<DateTime> EndDate = null;
             
                if (!string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEffectiveEndDate.Text);


                TransactionFee.ProposeTransactionFeePrimer(decimal.Parse(uiDdlExchange.SelectedValue.ToString()), decimal.Parse(CtlCommodityLookup1.LookupTextBoxID),
                                                        uiDdlCmType.SelectedValue, Convert.ToDecimal(uiTxtUpperLimit.Text),
                                                         Convert.ToDecimal(uiTxtClearingFee.Text),
                                                         Convert.ToDecimal(uiTxtExchangeFee.Text),
                                                         Convert.ToDecimal(uiTxtCompensationFund.Text),
                                                         Convert.ToDecimal(uiTxtThirdPartyFee.Text),
                                                         Convert.ToDecimal(uiTxtCSClearingFee.Text),
                                                         Convert.ToDecimal(uiTxtCSExchangeFee.Text),
                                                         Convert.ToDecimal(uiTxtCSCompensationFund.Text),
                                                         Convert.ToDecimal(uiTxtCSThirdPartyFee.Text), Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text),
                                                         EndDate, uiTxtApprovalDesc.Text, "D", 
                                                         User.Identity.Name, Convert.ToDecimal(eID));


            }
            Response.Redirect("ViewManageTransactionFeePrimer.aspx");

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
                TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    TransactionFee.ApproveTransactionFeePrimer(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageTransactionFeePrimer.aspx");
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
                TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    TransactionFee.RejectProposedTransactionFeePrimer(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageTransactionFeePrimer.aspx");
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

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(CtlCommodityLookup1.LookupTextBoxID))
            {
                uiBLError.Items.Add("Commodity is required.");
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
            if (string.IsNullOrEmpty(uiTxtClearingFee.Text))
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
            TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));
            CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();
            dt = Commodity.FillByCommodityID2(dr.CommodityID);
            CtlCommodityLookup1.SetCommodityValue(dr.CommodityID.ToString(), dt[0].CommodityCode);

            uiDdlExchange.SelectedValue = dr.ExchangeID.ToString();
            uiDdlCmType.SelectedValue = dr.CMType;
            uiTxtUpperLimit.Text = dr.UpperLimit.ToString("#,##0.###"); 
            uiTxtClearingFee.Text = Convert.ToDecimal(dr.TransClearingFee).ToString("#,##0.###");
            uiTxtExchangeFee.Text = dr.TransExchangeFee.ToString("#,##0.###"); 
            uiTxtCompensationFund.Text = dr.TransCompensationFund.ToString("#,##0.###"); 
            uiTxtThirdPartyFee.Text = dr.TransThirdPartyFee.ToString("#,##0.###");
            uiTxtCSClearingFee.Text = Convert.ToDecimal(dr.CSClearingFee).ToString("#,##0.###");
            uiTxtCSExchangeFee.Text = dr.CSExchangeFee.ToString("#,##0.###");
            uiTxtCSCompensationFund.Text = dr.CSCompensationFund.ToString("#,##0.###");
            uiTxtCSThirdPartyFee.Text = dr.CSThirdPartyFee.ToString("#,##0.###"); 
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
                //TransactionFeeData.TransactionFeePrimerRow
                TransactionFeeData.TransactionFeePrimerRow dr = TransactionFee.SelectTransactionFeePrimerByTransactionFeePrimerID(Convert.ToDecimal(eID));

                CtlCommodityLookup1.SetDisabledCommodity(true);
                uiDdlCmType.Enabled = false;
                uiTxtClearingFee.Enabled = false;
                uiTxtExchangeFee.Enabled = false;
                uiTxtCompensationFund.Enabled = false;
                uiTxtThirdPartyFee.Enabled = false;
                uiTxtCSClearingFee.Enabled = false;
                uiTxtCSCompensationFund.Enabled = false;
                uiTxtCSExchangeFee.Enabled = false;
                uiTxtCSThirdPartyFee.Enabled = false;
                uiTxtUpperLimit.Enabled = false;
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
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion

}
