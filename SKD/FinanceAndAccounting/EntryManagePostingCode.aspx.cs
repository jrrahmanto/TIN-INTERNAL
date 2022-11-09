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

public partial class WebUI_FinanceAndAccounting_EntryManagePostingCode : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtCode.Enabled = false;
                    uiTxtSeq.Enabled = false;
                    uiDdlLedgerType.Enabled = false;
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
        Response.Redirect("ViewManagePostingCode.aspx");
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
                    PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";
                }

                if (uiChkDisplay.Checked == true)
                {
                    Posting.ProposePostingCode(uiTxtCode.Text, uiDdlLedgerType.SelectedValue, uiDdlAccountType.SelectedValue,
                                             uiDdlBalance.SelectedValue, Convert.ToInt16(uiTxtSeq.Text), "Y", uiTxtDescription.Text,
                                             uiTxtApprovalDescription.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                }
                else
                {
                    Posting.ProposePostingCode(uiTxtCode.Text, uiDdlLedgerType.SelectedValue, uiDdlAccountType.SelectedValue,
                                             uiDdlBalance.SelectedValue, Convert.ToInt16(uiTxtSeq.Text), "N", uiTxtDescription.Text,
                                             uiTxtApprovalDescription.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                }
                Response.Redirect("ViewManagePostingCode.aspx");
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
            if (IsValidEntry() == true)
            {
                if (Convert.ToString(eID) != "")
                {
                    // Guard for editing proposed record
                    PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                    if (uiChkDisplay.Checked == true)
                    {
                        Posting.ProposePostingCode(uiTxtCode.Text, uiDdlLedgerType.SelectedValue,
                                                   uiDdlAccountType.SelectedValue, uiDdlBalance.SelectedValue,
                                                      Convert.ToInt16(uiTxtSeq.Text), "Y",
                                                       uiTxtDescription.Text, uiTxtDescription.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));
                    }
                    else
                    {
                        Posting.ProposePostingCode(uiTxtCode.Text, uiDdlLedgerType.SelectedValue,
                                                uiDdlAccountType.SelectedValue, uiDdlBalance.SelectedValue,
                                                   Convert.ToInt16(uiTxtSeq.Text), "N",
                                                    uiTxtDescription.Text, uiTxtDescription.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));
                    }
                }
                Response.Redirect("ViewManagePostingCode.aspx");
            }

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Posting.ApprovePostingCode(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDescription.Text);
                }
                Response.Redirect("ViewManagePostingCode.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");


                if (Convert.ToString(eID) != "")
                {
                    Posting.RejectProposedPostingCode(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDescription.Text);
                }
                Response.Redirect("ViewManagePostingCode.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
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

            if (eType == "add")
            {
                //cek apakah risktypecode sudah ada apa belum
                PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
                PostingData.AccountDataTable dtSeq = new PostingData.AccountDataTable();
                dt = Posting.GetPostingCodeByAccountCode(uiTxtCode.Text);
                dtSeq = Posting.GetPostingCodeBySetDisplayDFS(Convert.ToInt16(uiTxtSeq.Text));

                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("AccountCode is already exist.");
                }
                if (dtSeq.Count > 0)
                {
                    uiBLError.Items.Add("set display DFS is already exist");
                }
            }


            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDescription.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(uiTxtCode.Text))
            {
                uiBLError.Items.Add("Account code is required.");
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
            PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
            uiTxtCode.Text = dr.AccountCode;
            uiDdlLedgerType.SelectedValue = dr.LedgerType;
            uiDdlAccountType.SelectedValue = dr.AccountType;
            uiDdlBalance.SelectedValue = dr.BalanceType;
            if (Convert.ToUInt16(dr.DisplayInBS) == 1)
            {
                uiChkDisplay.Checked = true;
            }
            else
            {
                uiChkDisplay.Checked = false;
            }
            uiTxtSeq.Text = Convert.ToInt16(dr.SeqDisplayDFS).ToString();
            uiTxtDescription.Text = dr.Description;
              
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

            trAction.Visible = mp.IsChecker  || mp.IsViewer;
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            uiBtnDelete.Visible = mp.IsMaker;
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
             // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                PostingData.AccountRow dr = Posting.SelectPostingByAccountID(Convert.ToDecimal(eID));
                uiTxtCode.Enabled = false;
                uiDdlAccountType.Enabled = false;
                uiDdlBalance.Enabled = false;
                uiDdlLedgerType.Enabled = false;
                uiChkDisplay.Enabled = false;
                uiTxtSeq.Enabled = false;
                uiTxtDescription.Enabled = false;
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
