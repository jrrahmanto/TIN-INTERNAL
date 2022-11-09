using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class WebUI_New_EntyExchangeRate : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private string currentID
    {
        get 
        {
            return Request.QueryString["id"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiBlError.Visible = false;
        if (!IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
               //uiTxtExchangeCode.Enabled = false;
                bindData();
            }
            
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (!IsValidEntry())
        {
            return;
        }
        try
        {
            if (currentID != null)
            {

                ExchangeRate.ProposeExchangeRate(Convert.ToDecimal(uiDdlSourceCurrency.SelectedValue),
                                                Convert.ToDecimal(uiDdlDestinationCurrency.SelectedValue),
                                                Convert.ToDateTime(uiDtpStartDate.Text).Date,
                                                Convert.ToDateTime(uiDtpEndDate.Text).Date, User.Identity.Name,DateTime.Now,
                                                User.Identity.Name, DateTime.Now, uiDdlExchangeRate.SelectedValue, Convert.ToDecimal(uiTxbRate.Text),
                                                Convert.ToDecimal(currentID), null, "U",User.Identity.Name);
            }
            else
            {
                ExchangeRate.ProposeExchangeRate(Convert.ToDecimal(uiDdlSourceCurrency.SelectedValue),
                                                Convert.ToDecimal(uiDdlDestinationCurrency.SelectedValue),
                                                Convert.ToDateTime(uiDtpStartDate.Text).Date,
                                                Convert.ToDateTime(uiDtpEndDate.Text).Date, User.Identity.Name, DateTime.Now,
                                                User.Identity.Name, DateTime.Now, uiDdlExchangeRate.SelectedValue, Convert.ToDecimal(uiTxbRate.Text),
                                                Convert.ToDecimal(currentID),null, "I",User.Identity.Name);
            }
            Response.Redirect("ViewExchangeRate.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
                
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                if (currentID != "")
                {
                    // Guard for editing proposed record
                    ExchangeRateData.ExchangeRateRow dr = ExchangeRate.SelectExchangeRateByExchangeRateID(Convert.ToDecimal(currentID));
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("This record is not allowed to be deleted. Please wait for checker approval.");

                    ExchangeRate.ApproveExchangeRate(Convert.ToDecimal(currentID), User.Identity.Name, uiTxbApporvalDesc.Text);
                }
                Response.Redirect("ViewExchangeRate.aspx");
            }
            catch (Exception ex)
            {
                uiBlError.Items.Add(ex.Message);
                uiBlError.Visible = true;
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewExchangeRate.aspx");
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                if (currentID != "")
                {
                    // Guard for editing proposed record
                    ExchangeRateData.ExchangeRateRow dr = ExchangeRate.SelectExchangeRateByExchangeRateID(Convert.ToDecimal(currentID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be deleted. Please wait for checker approval.");

                    ExchangeRate.ProposeExchangeRate(Convert.ToDecimal(uiDdlSourceCurrency.SelectedValue),
                                                    Convert.ToDecimal(uiDdlDestinationCurrency.SelectedValue),
                                                    Convert.ToDateTime(uiDtpStartDate.Text).Date,
                                                    Convert.ToDateTime(uiDtpEndDate.Text).Date, User.Identity.Name,
                                                    DateTime.Now, User.Identity.Name, DateTime.Now,
                                                    uiDdlExchangeRate.SelectedValue, Convert.ToDecimal(uiTxbRate.Text),
                                                    Convert.ToDecimal(currentID), null, "D", User.Identity.Name);
                }
                Response.Redirect("ViewExchangeRate.aspx");
            }
            catch (Exception ex)
            {
                uiBlError.Items.Add(ex.Message);
                uiBlError.Visible = true;
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
                ExchangeRateData.ExchangeRateRow dr = ExchangeRate.SelectExchangeRateByExchangeRateID(Convert.ToDecimal(currentID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("This data is already approved. Please select other proposed data.");

                if (currentID != "")
                {
                    ExchangeRate.RejectProposedExchangeRate(Convert.ToDecimal(currentID), User.Identity.Name);
                }
                Response.Redirect("ViewExchangeRate.aspx");
            }
            catch (Exception ex)
            {
                uiBlError.Items.Add(ex.Message);
                uiBlError.Visible = true;

            }
        }
    }

    #region SupportingMethod

    private void bindData()
    {
        ExchangeRateData.ExchangeRateDataTable dt = new ExchangeRateData.ExchangeRateDataTable();
        dt = ExchangeRate.SelectExchangeRateByExchangeRateID(dt, Convert.ToDecimal(currentID));
        if (dt.Rows.Count > 0)
        {
            uiDdlSourceCurrency.SelectedValue = dt[0].SourceCurrID.ToString();
            uiDdlDestinationCurrency.SelectedValue = dt[0].DestinationCurrID.ToString();
            uiDdlExchangeRate.SelectedValue = dt[0].ExchRateType;
            uiDtpStartDate.SetCalendarValue(dt[0].ExchRateStartDate.ToString("dd-MMM-yyy"));
            uiDtpEndDate.SetCalendarValue(dt[0].ExchRateEndDate.ToString("dd-MMM-yyy"));
            uiTxbRate.Text = dt[0].Rate.ToString("#,##0.##");
            
            string actionDesc = "";
            if (!dt[0].IsActionFlagNull())
            {
                if (dt[0].ActionFlag == "I")
                {
                    actionDesc = "Insert";
                }
                else if (dt[0].ActionFlag == "U")
                {
                    actionDesc = "Update";
                }
                else if (dt[0].ActionFlag == "D")
                {
                    actionDesc = "Delete";
                }
            }
           
            uiTxbAction.Text = actionDesc;
        }
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBlError.Visible = false;
        uiBlError.Items.Clear();
        if (uiDtpStartDate.Text == "")
        {
            uiBlError.Items.Add("Start Date is required.");
        }
        if (uiDtpEndDate.Text == "")
        {
            uiBlError.Items.Add("End Date is required.");
        }
        if (uiDtpStartDate.Text != "" && uiDtpEndDate.Text != "")
        {
            if(Convert.ToDateTime(uiDtpStartDate.Text) > Convert.ToDateTime(uiDtpEndDate.Text))
            {
                uiBlError.Items.Add("Start Date should be earlier than end date.");
            }
        }

        if (uiTxbRate.Text == "")
        {
            uiBlError.Items.Add("Rate is required.");
        }

        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }
       
        return isValid;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;

        TRAction.Visible = mp.IsChecker || mp.IsViewer;
        TRApproval.Visible = mp.IsChecker || mp.IsViewer;

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
            ExchangeRateData.ExchangeRateRow dr = ExchangeRate.SelectExchangeRateByExchangeRateID(Convert.ToDecimal(currentID));

            uiDtpStartDate.SetDisabledCalendar(true);
            uiDtpEndDate.SetDisabledCalendar(true);
            uiTxbAction.Enabled = false;
            uiTxbAction.ReadOnly = true;
            uiTxbRate.Enabled = false;
            uiTxbRate.ReadOnly = true;
            uiDdlDestinationCurrency.Enabled = false;
            uiDdlExchangeRate.Enabled = false;
            uiDdlSourceCurrency.Enabled = false;
        }
    }

    #endregion
}
