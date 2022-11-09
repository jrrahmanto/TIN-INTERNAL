using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryInvestor : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    #region "   Use Case    "
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (currentID != null)
            {
                //ubl.Visible = false;
                bindData();
                // Set Control access by maker checker
               
            }
            SetControlAccessByMakerChecker();
        }

        uiTxbGroupProduct.Enabled = false;
        uiTxbInvStatus.Enabled = false;
        uiDdlAccountType.Enabled = false;
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewInvestor.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                string inHouseFlag = "N";
                if (uiChlInhouseAcc.Checked)
                {
                    inHouseFlag = "Y";
                }
                Investor.ProposedUpdate(uiTxbInvestorCode.Text, Convert.ToDecimal(CtlExchangeMemberLookup1.LookupTextBoxID),
                                        uiTxbInvestorCode.Text, inHouseFlag, User.Identity.Name,
                                        Convert.ToDecimal(currentID), uiTxbGroupProduct.Text, uiTxbInvStatus.Text, uiDdlAccountType.SelectedValue);

                ApplicationLog.Insert(DateTime.Now, "Investor", "I", "Propose update of investor", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                Response.Redirect("ViewInvestor.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
        else
        {
            try
            {
                string inHouseFlag = "N";
                if (uiChlInhouseAcc.Checked)
                {
                    inHouseFlag = "Y";
                }
                
                // Insert new record of investor to be approved
                Investor.ProposedInsert(uiTxbInvestorCode.Text, Convert.ToDecimal(CtlExchangeMemberLookup1.LookupTextBoxID),
                                        uiTxbInvestorCode.Text, inHouseFlag, User.Identity.Name, uiTxbGroupProduct.Text, uiTxbInvStatus.Text, uiDdlAccountType.SelectedValue);
                
                // Log to application log
                ApplicationLog.Insert(DateTime.Now, "Investor", "I", "Propose insert of investor", User.Identity.Name, Common.GetIPAddress(this.Request));

                // Redirect to summary page
                Response.Redirect("ViewInvestor.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                string inHouseFlag = "N";
                if (uiChlInhouseAcc.Checked)
                {
                    inHouseFlag = "Y";
                }
                Investor.ProposedDelete(uiTxbInvestorCode.Text, Convert.ToDecimal(CtlExchangeMemberLookup1.LookupTextBoxID),
                                        uiTxbInvestorCode.Text, inHouseFlag, User.Identity.Name,
                                        Convert.ToDecimal(currentID), uiTxbGroupProduct.Text, uiTxbInvStatus.Text, uiDdlAccountType.SelectedValue);

                // Log to application log
                ApplicationLog.Insert(DateTime.Now, "Investor", "I", "Propose delete of investor", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                Response.Redirect("ViewInvestor.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                Investor.Approve(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text,
                                 uiTxbAction.Text, User.Identity.Name);

                ApplicationLog.Insert(DateTime.Now, "Investor", "I", "Approve update of investor", User.Identity.Name, Common.GetIPAddress(this.Request));
                
                Response.Redirect("ViewInvestor.aspx");
            }
            catch (Exception ex)
            {
                // Display error message
                DisplayErrorMessage(ex);
            }
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                Investor.Reject(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text,
                                 uiTxbAction.Text, User.Identity.Name);

                ApplicationLog.Insert(DateTime.Now, "Investor", "I", "Reject update of investor", User.Identity.Name, Common.GetIPAddress(this.Request));

                Response.Redirect("ViewInvestor.aspx");
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
    private void bindData()
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        dt = Investor.FillByInvestorID(Convert.ToDecimal(currentID));
        if (dt.Count > 0)
        {
            uiTxbInvestorCode.Text = dt[0].Code;
            string emCode =  ExchangeMember.GetExchangeMemberCode(dt[0].EMID);
            CtlExchangeMemberLookup1.SetExchangeMemberValue(dt[0].EMID.ToString(), emCode);
            if (dt[0].InHouseFlag == "Y")
            {
                uiChlInhouseAcc.Checked = true;
            }
            else
            {
                uiChlInhouseAcc.Checked = false;
            }
            if (!dt[0].IsAccountTypeNull())
            {
                uiDdlAccountType.SelectedValue = dt[0].AccountType;
            }
            if (!dt[0].IsGroupProductNull())
            {
                uiTxbGroupProduct.Text = dt[0].GroupProduct;
            }
            if (!dt[0].IsInvestorStatusNull())
            {
                uiTxbInvStatus.Text = dt[0].InvestorStatus;
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

        if (uiTxbInvestorCode.Text == "")
        {
            uiBlError.Items.Add("Investor code is required.");
        }
        
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
        }
    }
    
    #endregion

}
