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
using System.Collections.Generic;

public partial class WebUI_FinanceAndAccounting_EntryManagePostingGroup : System.Web.UI.Page
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

    private string RowType
    {
        get { return ViewState["RowType"].ToString(); }
        set { ViewState["RowType"] = value; }
    }


    private string TempPostingGroup
    {
        get { return ViewState["TempPostingGroup"].ToString(); }
        set { ViewState["TempPostingGroup"] = value; }
    }

    private string TempPostingGroupDr
    {
        get { return ViewState["TempPostingGroupDr"].ToString(); }
        set { ViewState["TempPostingGroupDr"] = value; }
    }

    private PostingData.PostingGroupAccountDataTable dtPostingGroupAccount
    {
        get { return (PostingData.PostingGroupAccountDataTable)ViewState["dtPostingGroupAccount"]; }
        set { ViewState["dtPostingGroupAccount"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
            

            if (!Page.IsPostBack)
            {
                dtPostingGroupAccount = new PostingData.PostingGroupAccountDataTable();
                FillPostingGroupAccountDataGrid();

            }

            if (eType == "add")
            {
                //disable end date
                CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                uiTxtGroupCode.Enabled = false;
                uiDdlLedgerType.Enabled = false;
                CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                bindData();
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
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Posting.RejectProposedPostingGroupID(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewManagePostingGroup.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                UpdatePostingGroup(eID, "U", "A");

                Response.Redirect("ViewManagePostingGroup.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                UpdatePostingGroup(0, "D", "P");

                Response.Redirect("ViewManagePostingGroup.aspx");
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
        Response.Redirect("ViewManagePostingGroup.aspx");
    }

    private void UpdatePostingGroup(decimal postingGroupId, string actionFlag, 
        string approvalStatus)
    {
        try
        {
            // Case Update/Revision
            if (string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
            {
                Posting.UpdatePostingGroup(postingGroupId, uiTxtGroupCode.Text, uiDdlLedgerType.SelectedValue, approvalStatus,
                    uiTxtApprovalDesc.Text, DateTime.Parse(CtlCalendarEffectiveStartDate.Text),
                    null, uiTxtDescription.Text,
                    User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now,
                    actionFlag, eID, dtPostingGroupAccount);
            }
            else
            {
                Posting.UpdatePostingGroup(postingGroupId, uiTxtGroupCode.Text, uiDdlLedgerType.SelectedValue, approvalStatus,
                    uiTxtApprovalDesc.Text, DateTime.Parse(CtlCalendarEffectiveStartDate.Text),
                    DateTime.Parse(CtlCalendarEffectiveEndDate.Text), uiTxtDescription.Text,
                    User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now,
                    actionFlag, eID, dtPostingGroupAccount);
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
        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                if (eType == "add")
                {
                    UpdatePostingGroup(0, "I", "P");
                }
                else if (eType == "edit")
                {
                    PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus == "A")
                    {
                        UpdatePostingGroup(0, "U", "P");
                    }
                    else
                    {
                        UpdatePostingGroup(eID, "U", "P");
                    }
                    
                }
               
                Response.Redirect("ViewManagePostingGroup.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    #region " -- Datagrid Posting Group Account -- "

    private void DisableDataGridRowEnabled(int rowIndex)
    {
        try
        {
            for (int ii = 0; ii < dtPostingGroupAccount.Count; ii++)
            {
                if (ii != rowIndex)
                {
                    uiDgPostingGroupAccount.Rows[ii].Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgPostingGroupAccount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            if (RowType == "add")
            {
                dtPostingGroupAccount.Rows[e.RowIndex].RejectChanges();
            }

            uiDgPostingGroupAccount.EditIndex = -1;

            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgPostingGroupAccount_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            RowType = "edit";
            PostingData.PostingGroupAccountRow dr = null;
            uiDgPostingGroupAccount.EditIndex = e.NewEditIndex;

            Label drCr = (Label)uiDgPostingGroupAccount.Rows[e.NewEditIndex].FindControl("uiLblDrCr");
            Label postingCodeId = (Label)uiDgPostingGroupAccount.Rows[e.NewEditIndex].FindControl("uiLblPostingCodeID");
            
            dr = dtPostingGroupAccount[e.NewEditIndex];
            TempPostingGroupDr = dr.DrCrType.ToString();
            TempPostingGroup = dr.AccountID.ToString();
           

            BindViewStateToDataGrid();

            Lookup_CtlPostingCodeLookup account = (Lookup_CtlPostingCodeLookup)uiDgPostingGroupAccount.Rows[e.NewEditIndex].FindControl("CtlPostingCodeLookup");
            account.SetPostingCodeValue(postingCodeId.Text, Posting.GetPostingAccountCodeByAccountID(decimal.Parse(postingCodeId.Text)));
     
            DisableDataGridRowEnabled(e.NewEditIndex);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgPostingGroupAccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (IsValidDetailEntry(e.RowIndex) == true)
            {
                PostingData.PostingGroupAccountRow dr = null;

                TextBox postingGroupId = (TextBox)uiDgPostingGroupAccount.Rows[e.RowIndex].FindControl("uiTxtPostingGroup");
                DropDownList DrCrType = (DropDownList)uiDgPostingGroupAccount.Rows[e.RowIndex].FindControl("uiDdlDrCr");
                Lookup_CtlPostingCodeLookup accountId = (Lookup_CtlPostingCodeLookup)uiDgPostingGroupAccount.Rows[e.RowIndex].FindControl("CtlPostingCodeLookup");

                dr = dtPostingGroupAccount[e.RowIndex];
                dr.PostingGroupID = 0;
                dr.DrCrType = DrCrType.SelectedValue;
                dr.AccountID = decimal.Parse(accountId.LookupTextBoxID);

                uiDgPostingGroupAccount.EditIndex = -1;

                BindViewStateToDataGrid();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgRiskProfile_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtPostingGroupAccount.Rows[e.RowIndex].Delete();
            dtPostingGroupAccount.AcceptChanges();
            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private bool IsValidDetailEntry(int rowIndex)
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();

            TextBox postingGroupId = (TextBox)uiDgPostingGroupAccount.Rows[rowIndex].FindControl("uiTxtPostingGroup");
            DropDownList DrCrType = (DropDownList)uiDgPostingGroupAccount.Rows[rowIndex].FindControl("uiDdlDrCr");
            Lookup_CtlPostingCodeLookup accountId = (Lookup_CtlPostingCodeLookup)uiDgPostingGroupAccount.Rows[rowIndex].FindControl("CtlPostingCodeLookup");

            if (!string.IsNullOrEmpty(accountId.LookupTextBoxID))
            {
                if (accountId.LookupTextBoxID == "")
                {
                    isValid = false;
                    uiBLError.Items.Add(string.Format("Row {0} : Posting group is required.", rowIndex + 1));
                }
                else
                {
                    if (dtPostingGroupAccount.Count > 0)
                    {
                        foreach (PostingData.PostingGroupAccountRow dr in dtPostingGroupAccount)
                        {
                            if (DrCrType.SelectedValue.ToString() != TempPostingGroupDr || accountId.LookupTextBoxID != TempPostingGroup)
                            {
                                if (dr.DrCrType == DrCrType.SelectedValue.ToString() && dr.AccountID.ToString() == accountId.LookupTextBoxID)
                                {
                                    isValid = false;
                                    uiBLError.Items.Add(string.Format("Row {0} : Posting group already exist.", rowIndex + 1));
                                    break;
                                }
                            }                            
                        }
                    }
                    
                }
            }
            if (DrCrType != null)
            {
                if (DrCrType.Text == "0")
                {
                    isValid = false;
                    uiBLError.Items.Add(string.Format("Row {0} : DrCrType is required.", rowIndex + 1));
                }
            }
            if (string.IsNullOrEmpty(accountId.LookupTextBoxID))
            {
                if (accountId.LookupTextBoxID == "")
                {
                    isValid = false;
                    uiBLError.Items.Add(string.Format("Row {0} : Account is required.", rowIndex + 1));
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

    protected void uiBtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtPostingGroupAccount.Count == 0)
            {
                AddRow();
            }
            else
            {
                if (uiDgPostingGroupAccount.EditIndex == -1)
                {
                    AddRow();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillPostingGroupAccountDataGrid()
    {
        try
        {
            uiDgPostingGroupAccount.DataSource = ObjectDataSourcePostingGroupAccount;
            IEnumerable dv = (IEnumerable)ObjectDataSourcePostingGroupAccount.Select();
            DataView dve = (DataView)dv;
            dtPostingGroupAccount = (PostingData.PostingGroupAccountDataTable)dve.Table;
            dtPostingGroupAccount.Columns.Add("DrCrTypeDesc", typeof(string), "iif(DrCrType = 'D', 'Debet', 'Credit')");
            BindViewStateToDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void AddRow()
    {
        try
        {
            RowType = "add";
            //Reserve data
            //Temporary clear constraint
            dtPostingGroupAccount.Constraints.Clear();
            PostingData.PostingGroupAccountRow dr = dtPostingGroupAccount.NewPostingGroupAccountRow();
            dtPostingGroupAccount.PostingGroupIDColumn.AllowDBNull = true;
            dtPostingGroupAccount.AccountIDColumn.AllowDBNull = true;
            dtPostingGroupAccount.DrCrTypeColumn.AllowDBNull = true;
            dr.PostingGroupID = 0;
            dr.AccountID = 0;
            dr.DrCrType = "0";
            dtPostingGroupAccount.AddPostingGroupAccountRow(dr);

            TempPostingGroup = "";
            TempPostingGroupDr = "";

            int newEditIndex = uiDgPostingGroupAccount.Rows.Count;
            uiDgPostingGroupAccount.EditIndex = newEditIndex;

            BindViewStateToDataGrid();
            DisableDataGridRowEnabled(newEditIndex);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void BindViewStateToDataGrid()
    {
        try
        {
            uiDgPostingGroupAccount.DataSource = dtPostingGroupAccount;
            uiDgPostingGroupAccount.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    #endregion

    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

             PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();
             PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();

             if (eType == "add")
             {
                 //cek apakah risktypecode sudah ada apa belum
                 dt = Posting.GetPostingGroupByGroupCode(uiTxtGroupCode.Text);

                 if (dt.Count > 0)
                 {
                     uiBLError.Items.Add("Posting group is already exist.");
                 }

                 if (dtPostingGroupAccount.Count == 0)
                 {
                     isValid = false;
                     uiBLError.Items.Add("Posting group account is required.");
                 }
                 else
                 {
                     for (int ii = 0; ii < dtPostingGroupAccount.Count; ii++)
                     {
                         //isValid = IsValidDetailEntry(ii);
                     }
                 }

             }

             //guard for number record
             decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(uiTxtGroupCode.Text, uiDdlLedgerType.SelectedValue,
                                                 DateTime.Parse(CtlCalendarEffectiveStartDate.Text), eID));

             if (NumberRecord > 0)
             {
                 uiBLError.Items.Add("Can not set start date less than other approved records.");
             }

            if (eType == "edit" && mp.IsChecker)
            {

                PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus == "A")
                {
                    isValid = false;
                    uiBLError.Items.Add("Record already approved.");
                }
                else if (dr.ApprovalStatus == "P")
                {
                    if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                    {
                        isValid = false;
                        uiBLError.Items.Add("Approval description is required.");
                    }
                }
            }
            else if (eType == "edit" && mp.IsMaker)
            {
                PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A")
                {
                    isValid = false;
                    uiBLError.Items.Add("Can not edit pending approval record.");
                }

                if (string.IsNullOrEmpty(uiTxtGroupCode.Text))
                {
                    isValid = false;
                    uiBLError.Items.Add("Group code is required.");
                }
                if (string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text))
                {
                    isValid = false;
                    uiBLError.Items.Add("Effective start date is required.");
                }
            }

            if (string.IsNullOrEmpty(uiTxtGroupCode.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Group code is required.");
            }
            if (string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Effective start date is required.");
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
            PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));
            uiTxtGroupCode.Text = dr.PostingGroupCode;
            uiDdlLedgerType.SelectedValue = dr.LedgerType;
            CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
            if (dr.IsDescriptionNull())
            {
                uiTxtDescription.Text = null;
            }
            else 
            {
                uiTxtDescription.Text = dr.Description;
            }
            
            if (dr.IsEffectiveEndDateNull())
            {
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

            uiDgPostingGroupAccount.Columns[0].Visible = mp.IsMaker;
            uiBtnAdd.Visible = mp.IsMaker;

            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                PostingData.PostingGroupRow dr = Posting.SelectPostingGroupByPostingGroupID(Convert.ToDecimal(eID));

                uiTxtGroupCode.Enabled = false;
                uiDdlLedgerType.Enabled = false;
                CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
                uiTxtDescription.Enabled = false;
                uiTxtAction.Enabled = false;
              
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
