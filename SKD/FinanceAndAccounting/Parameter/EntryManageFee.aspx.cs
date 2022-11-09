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

public partial class FinanceAndAccounting_Parameter_EntryManageFee : System.Web.UI.Page
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
                    CtlCalendarEndDate.SetCalendarValue(null);
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    CtlCommodityLookup1.SetDisabledCommodity(true);
                    CtlCalendarStartDate.SetDisabledCalendar(true);
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
                    FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    //guard for number record
                    FeeDataTableAdapters.FeeTableAdapter ta = new FeeDataTableAdapters.FeeTableAdapter();
                    FeeData.FeeDataTable dt = new FeeData.FeeDataTable();
                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(decimal.Parse(CtlCommodityLookup1.LookupTextBoxID), DateTime.Parse(CtlCalendarStartDate.Text), eID));
                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");

                    actionFlag = "U";

                }

                Nullable<DateTime> EndDate = null;
                Nullable<decimal> TNClearing = null;
                Nullable<decimal> TNExchange = null;
                Nullable<decimal> TNCompensation = null;
                Nullable<decimal> TNThirdPartyFee = null;
                Nullable<decimal> ClearingFund = null;


                if (!string.IsNullOrEmpty(uiTxtTNClearing.Text))
                    TNClearing = decimal.Parse(uiTxtTNClearing.Text);
                if (!string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEndDate.Text);
                if (!string.IsNullOrEmpty(uiTxtTNExchange.Text)) 
                    TNExchange = decimal.Parse(uiTxtTNExchange.Text);
                if (!string.IsNullOrEmpty(uiTxtTNCompensationFund.Text))
                    TNCompensation = decimal.Parse(uiTxtTNCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtTNThirdPartyFee.Text))
                    TNThirdPartyFee = decimal.Parse(uiTxtTNThirdPartyFee.Text);
               
                if (!string.IsNullOrEmpty(uiTxtClearingFund.Text))
                    ClearingFund = decimal.Parse(uiTxtClearingFund.Text);
                

                Fee.ProposeFee(decimal.Parse(CtlCommodityLookup1.LookupTextBoxID), TNClearing, TNExchange,
                               TNCompensation, Convert.ToDateTime(CtlCalendarStartDate.Text),
                               EndDate, TNThirdPartyFee,
                               ClearingFund, uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));

                Response.Redirect("ViewManageFee.aspx");
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
                FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                Nullable<DateTime> EndDate = null;
                Nullable<decimal> TNClearing = null;
                Nullable<decimal> TNExchange = null;
                Nullable<decimal> TNCompensation = null;
                Nullable<decimal> TNThirdPartyFee = null;
                Nullable<decimal> ClearingFund = null;


                if (!string.IsNullOrEmpty(uiTxtTNClearing.Text))
                    TNClearing = decimal.Parse(uiTxtTNClearing.Text);
                if (!string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                    EndDate = DateTime.Parse(CtlCalendarEndDate.Text);
                if (!string.IsNullOrEmpty(uiTxtTNExchange.Text))
                    TNExchange = decimal.Parse(uiTxtTNExchange.Text);
                if (!string.IsNullOrEmpty(uiTxtTNCompensationFund.Text))
                    TNCompensation = decimal.Parse(uiTxtTNCompensationFund.Text);
                if (!string.IsNullOrEmpty(uiTxtTNThirdPartyFee.Text))
                    TNThirdPartyFee = decimal.Parse(uiTxtTNThirdPartyFee.Text);
                if (!string.IsNullOrEmpty(uiTxtClearingFund.Text))
                    ClearingFund = decimal.Parse(uiTxtClearingFund.Text);

                
                Fee.ProposeFee(decimal.Parse(CtlCommodityLookup1.LookupTextBoxID), TNClearing, TNExchange,
                                TNCompensation, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                EndDate, TNThirdPartyFee,
                                ClearingFund, uiTxtApprovalDesc.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));
                

            }

            Response.Redirect("ViewManageFee.aspx");

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
                FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Fee.ApproveFee(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageFee.aspx");
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
                FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Fee.RejectProposedFee(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }
                Response.Redirect("ViewManageFee.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }
    
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageFee.aspx");
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

            if (string.IsNullOrEmpty(CtlCalendarStartDate.Text))
            {
                uiBLError.Items.Add("Start Date is required.");
            }

            if (!string.IsNullOrEmpty(CtlCalendarStartDate.Text) && !string.IsNullOrEmpty(CtlCalendarEndDate.Text))
            {
                if (DateTime.Parse(CtlCalendarStartDate.Text) >= DateTime.Parse(CtlCalendarEndDate.Text))
                    uiBLError.Items.Add("End date should be greater than start date.");
            }

            if (string.IsNullOrEmpty(uiTxtTNClearing.Text))
            {
                uiBLError.Items.Add("Tender Clearing Fee is required.");
            }

            if (string.IsNullOrEmpty(uiTxtTNExchange.Text))
            {
                uiBLError.Items.Add("Tender Exchange Fee is required.");
            }

            if (string.IsNullOrEmpty(uiTxtTNCompensationFund.Text))
            {
                uiBLError.Items.Add("Tender Compensation Fund is required.");
            }

            if (string.IsNullOrEmpty(uiTxtTNThirdPartyFee.Text))
            {
                uiBLError.Items.Add("Tender Third Party Fee is required.");
            }

            if (string.IsNullOrEmpty(uiTxtClearingFund.Text))
            {
                uiBLError.Items.Add("Clearing Fund is required.");
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
            FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));
            CtlCommodityLookup1.SetCommodityValue(dr.CommodityID.ToString(), dr.CommodityCode.ToString());
            CtlCalendarStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (dr.IsEffectiveEndDateNull())
            {
                //disable end date
                CtlCalendarEndDate.SetCalendarValue(null);
                //CtlCalendarEndDate.SetDisabledCalendar(true);
            }
            else
            {
                CtlCalendarEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
            }

            if (!dr.IsTNClearingFeeNull())
            { uiTxtTNClearing.Text = Convert.ToDecimal(dr.TNClearingFee).ToString("#,##0.###"); }
            else { uiTxtTNClearing.Text = ""; }

            if (!dr.IsTNExchangeFeeNull())
            { uiTxtTNExchange.Text = Convert.ToDecimal(dr.TNExchangeFee).ToString("#,##0.###"); }
            else { uiTxtTNExchange.Text = ""; }

            if (!dr.IsTNCompensationFundNull())
            { uiTxtTNCompensationFund.Text = Convert.ToDecimal(dr.TNCompensationFund).ToString("#,##0.###"); }
            else { uiTxtTNCompensationFund.Text = ""; }

            if (!dr.IsTNThirdPartyFeeNull())
            { uiTxtTNThirdPartyFee.Text = Convert.ToDecimal(dr.TNThirdPartyFee).ToString("#,##0.###"); }
            else { uiTxtTNThirdPartyFee.Text = ""; }

          
            if (!dr.IsClearingFundNull())
            { uiTxtClearingFund.Text = Convert.ToDecimal(dr.ClearingFund).ToString("#,##0.###"); }
            else { uiTxtClearingFund.Text = ""; }

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
                FeeData.FeeRow dr = Fee.SelectFeeByFeeID(Convert.ToDecimal(eID));

                uiTxtTNClearing.Enabled = false;
                uiTxtTNCompensationFund.Enabled = false;
                uiTxtTNExchange.Enabled = false;
                uiTxtTNThirdPartyFee.Enabled = false;
                uiTxtClearingFund.Enabled = false;
                CtlCommodityLookup1.SetDisabledCommodity(true);
                uiTxtAction.Enabled = false;
                CtlCalendarStartDate.SetDisabledCalendar(true);
                CtlCalendarStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
                if (dr.IsEffectiveEndDateNull())
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
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
