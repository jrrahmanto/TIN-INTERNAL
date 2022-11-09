using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;


public partial class WebUI_New_EntryManualPosting : System.Web.UI.Page
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

    private JournalData.JournalLineDataTable dtJournalLine
    {
        get { return (JournalData.JournalLineDataTable)ViewState["dtJournalLine"]; }
        set { ViewState["dtJournalLine"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();       

        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                uiDdlLedgerType.SelectedValue = "M";
                uiDdlLedgerType.Enabled = false;
                dtJournalLine = new JournalData.JournalLineDataTable();
                SetDefaultValues();
                eID = 0;
            }
            else if (eType == "edit")
            {
                BindDataToForm();

                FillJournalDataGrid();

                SetEnabledControl(false);
                SetVisibleControls(false);
            }
        }
    }

    protected void uiBtnAdd_Click(object sender, EventArgs e)
    {
        if (dtJournalLine.Count == 0)
        {
            AddRow();
        }
        else
        {
            if (uiDgJournal.EditIndex == -1)
            {
                AddRow();
            }
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("") == true)
        {
            try
            {
                SaveManualPosting();
                Response.Redirect("ViewManualPosting.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManualPosting.aspx");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("approve") == true)
        {
            try
            {
                Journal.UpdateJournal(eID, 0, DateTime.Parse(CtlCalendarPickUpPostingDate.Text),
                    uiDdlLedgerType.SelectedValue, "A", decimal.Parse(uiDdlCurrency.SelectedValue),
                    uiDdlJournalType.SelectedValue, uiTxtDescription.Text,
                    User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, uiTxtApprovalDesription.Text, dtJournalLine);

                Response.Redirect("ViewManualPosting.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("reject") == true)
        { 
            try
            {
                Journal.UpdateJournal(eID, 0, DateTime.Parse(CtlCalendarPickUpPostingDate.Text),
                    uiDdlLedgerType.SelectedValue, "R", decimal.Parse(uiDdlCurrency.SelectedValue),
                    uiDdlJournalType.SelectedValue, uiTxtDescription.Text,
                    User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, uiTxtApprovalDesription.Text, dtJournalLine);

                Response.Redirect("ViewManualPosting.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }        
    }

    protected void uiDgJournal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }

    protected void uiDgJournal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        if (RowType == "add")
        {
            dtJournalLine.Rows[e.RowIndex].RejectChanges();
        }        

        uiDgJournal.EditIndex = -1;

        BindViewStateToDataGrid();

    }

    protected void uiDgJournal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        dtJournalLine.Rows[e.RowIndex].Delete();

        BindViewStateToDataGrid();
    }

    protected void uiDgJournal_RowEditing1(object sender, GridViewEditEventArgs e)
    {
        RowType = "edit";

        uiDgJournal.EditIndex = e.NewEditIndex;

        Label postingCodeId = (Label)uiDgJournal.Rows[e.NewEditIndex].FindControl("uiLblPostingCodeID");
        Label clearingMemberId = (Label)uiDgJournal.Rows[e.NewEditIndex].FindControl("uiLblClearingMemberID");

        BindViewStateToDataGrid();

        Lookup_CtlPostingCodeLookup account = (Lookup_CtlPostingCodeLookup)uiDgJournal.Rows[e.NewEditIndex].FindControl("CtlPostingCodeLookup");
        account.SetPostingCodeValue(postingCodeId.Text, Posting.GetPostingAccountCodeByAccountID(decimal.Parse(postingCodeId.Text)));
        Lookup_CtlClearingMemberLookup attCode1 = (Lookup_CtlClearingMemberLookup)uiDgJournal.Rows[e.NewEditIndex].FindControl("CtlClearingMemberLookup");
        if (!string.IsNullOrEmpty(clearingMemberId.Text))
        {
            attCode1.SetClearingMemberValue(clearingMemberId.Text, ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(clearingMemberId.Text)));
        }        
        
        DisableDataGridRowEnabled(e.NewEditIndex);
    }

    protected void uiDgJournal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (IsValidDetailEntry(e.RowIndex) == true)
        {
            JournalData.JournalLineRow dr = null;

            Lookup_CtlPostingCodeLookup account = (Lookup_CtlPostingCodeLookup)uiDgJournal.Rows[e.RowIndex].FindControl("CtlPostingCodeLookup");
            TextBox drAmount = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtDrAmount");
            TextBox crAmount = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtCrAmount");
            TextBox linDesc = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtLineDescription");
            Lookup_CtlClearingMemberLookup attCode1 = (Lookup_CtlClearingMemberLookup)uiDgJournal.Rows[e.RowIndex].FindControl("CtlClearingMemberLookup");
            TextBox attCode2 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt2Code");
            TextBox attCode3 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt3Code");
            TextBox attCode4 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt4Code");
            TextBox attCode5 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt5Code");
            TextBox attDesc1 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt1Desc");
            TextBox attDesc2 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt2Desc");
            TextBox attDesc3 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt3Desc");
            TextBox attDesc4 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt4Desc");
            TextBox attDesc5 = (TextBox)uiDgJournal.Rows[e.RowIndex].FindControl("uiTxtAtt5Desc");

            dr = dtJournalLine[e.RowIndex];

            dr.AccountID = decimal.Parse(account.LookupTextBoxID);

            if (!string.IsNullOrEmpty(drAmount.Text))
            {
                dr.DrAmount = decimal.Parse(drAmount.Text);
            }
            else
            {
                dr.SetDrAmountNull();
            }
            if (!string.IsNullOrEmpty(crAmount.Text))
            {
                dr.CrAmount = decimal.Parse(crAmount.Text);
            }
            else
            {
                dr.SetCrAmountNull();
            }
            dr.LineDescription = linDesc.Text;            
            dr.Attribute1Code = attCode1.LookupTextBoxID;
            dr.Attribute2Code = attCode2.Text;
            dr.Attribute3Code = attCode3.Text;
            dr.Attribute4Code = attCode4.Text;
            dr.Attribute5Code = attCode5.Text;
            dr.Attribute1Description = attDesc1.Text;
            dr.Attribute2Description = attDesc2.Text;
            dr.Attribute3Description = attDesc3.Text;
            dr.Attribute4Description = attDesc4.Text;
            dr.Attribute5Description = attDesc5.Text;

            uiDgJournal.EditIndex = -1;

            BindViewStateToDataGrid();
        }
    }

    private void AddRow()
    {
        RowType = "add";
        //Reserve data
        //Temporary clear constraint
        dtJournalLine.Constraints.Clear();       
        JournalData.JournalLineRow dr = dtJournalLine.NewJournalLineRow();
        dtJournalLine.JournalLineIDColumn.AllowDBNull = true;
        dtJournalLine.JournalHeaderIDColumn.AllowDBNull = true;
        dtJournalLine.AccountIDColumn.AllowDBNull = true;
        dtJournalLine.CurrencyIDColumn.AllowDBNull = true;
        dr.JournalLineID = 0;
        dr.JournalHeaderID = 0;
        dr.SetCrAmountNull();
        dr.SetDrAmountNull();
        dr.CurrencyID = 0; 
        dr.eType = "add";        
        dtJournalLine.AddJournalLineRow(dr);

        int newEditIndex = uiDgJournal.Rows.Count;
        uiDgJournal.EditIndex = newEditIndex;

        BindViewStateToDataGrid();
    }

    private void VisibleCommandButton()
    {
        for (int ii = 0; ii < dtJournalLine.Count; ii++)
        {
            if (string.IsNullOrEmpty(dtJournalLine[ii].eType))
            {
                LinkButton btnUpdate = (LinkButton)uiDgJournal.Rows[ii].Cells[0].Controls[0];
                btnUpdate.Visible = false;
                LinkButton btnCancel = (LinkButton)uiDgJournal.Rows[ii].Cells[0].Controls[2];
                btnCancel.Visible = false;
            }
        }
    }

    private void DisableDataGridRowEnabled(int rowIndex)
    {
        for (int ii = 0; ii < dtJournalLine.Count; ii++)
        {
            if (ii != rowIndex)
            {
                uiDgJournal.Rows[ii].Enabled = false;
            }
        }
    }

    private void FillJournalDataGrid()
    {
        uiDgJournal.DataSource = ObjectDataSourceJournal;
        IEnumerable dv = (IEnumerable)ObjectDataSourceJournal.Select();
        DataView dve = (DataView)dv;

        dtJournalLine = (JournalData.JournalLineDataTable)dve.Table;

        BindViewStateToDataGrid();
    }

    private void BindViewStateToDataGrid()
    {
        uiDgJournal.DataSource = dtJournalLine;
        uiDgJournal.DataBind();

        VisibleCommandButton();
    }

    private void BindDataToForm()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceJournalHeader.Select();
        Object[] obj = (Object[])dv;        
        JournalData.JournalHeaderRow dr = (JournalData.JournalHeaderRow)obj[0];
        uiTxtPostingNo.Text = dr.JournalNo.ToString();
        CtlCalendarPickUpPostingDate.SetCalendarValue(dr.TransactionDate.ToString("dd-MMM-yyyy"));
        if (!dr.IsCurrencyIDNull())
        {
            uiDdlCurrency.SelectedValue = dr.CurrencyID.ToString();
        }
        uiDdlLedgerType.SelectedValue = dr.LedgerType;
        uiDdlJournalType.SelectedValue = dr.JournalType;
        uiTxtDescription.Text = dr.HeaderDescription;
        if (!dr.IsApprovalDescNull())
        {
            uiTxtApprovalDesription.Text = dr.ApprovalDesc;
        }
    }

    private bool IsValidEntry(string command)
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        MasterPage mp = (MasterPage)this.Master;

        IEnumerable dv = (IEnumerable)ObjectDataSourceJournalHeader.Select();
        Object[] obj = (Object[])dv;
        if (obj != null)
        {
            JournalData.JournalHeaderRow dr = (JournalData.JournalHeaderRow)obj[0];
            if (command == "approve")
            {
                if (dr.ApprovalStatus == "A")
                {
                    uiBLError.Items.Add("This record has been approved.");
                }
                else if (dr.ApprovalStatus == "R")
                {
                    uiBLError.Items.Add("This record has been rejected.");
                }
                if (string.IsNullOrEmpty(uiTxtApprovalDesription.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }
            else if (command == "reject")
            {
                if (dr.ApprovalStatus == "A")
                {
                    uiBLError.Items.Add("This record has been approved.");
                }
                else if (dr.ApprovalStatus == "R")
                {
                    uiBLError.Items.Add("This record has been rejected.");
                }
            }
        }        

        if (string.IsNullOrEmpty(CtlCalendarPickUpPostingDate.Text))
        {
            uiBLError.Items.Add("Transaction date is required.");
        }
        
        if (eType == "add")
        {
            if (mp.IsMaker)
            {
                if (string.IsNullOrEmpty(uiDdlCurrency.SelectedValue))
                {
                    uiBLError.Items.Add("Currency is required.");
                }
                if (string.IsNullOrEmpty(uiDdlLedgerType.SelectedValue))
                {
                    uiBLError.Items.Add("Ledger type is required.");
                }
                if (string.IsNullOrEmpty(uiDdlJournalType.SelectedValue))
                {
                    uiBLError.Items.Add("Journal type is required.");
                }

                if (dtJournalLine.Count == 0)
                {
                    uiBLError.Items.Add("Journal is required.");
                }
                else
                {
                    decimal drAmount = 0;
                    decimal crAmount = 0;
                    foreach (JournalData.JournalLineRow drJournalLine in dtJournalLine)
                    {
                        if (!drJournalLine.IsDrAmountNull())
                        {
                            drAmount += drJournalLine.DrAmount;
                        }
                        if (!drJournalLine.IsCrAmountNull())
                        {
                            crAmount += drJournalLine.CrAmount;
                        }
                    }

                    if (drAmount != crAmount)
                    {
                        uiBLError.Items.Add("Journal is not balance.");
                    }
                }
            }
        }

       
        //if (mp.IsChecker)
        //{
        //    if (string.IsNullOrEmpty(uiTxtApprovalDesription.Text))
        //    {
        //        uiBLError.Items.Add("Approval description is required.");
        //    }
        //}    
        

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private bool IsValidDetailEntry(int rowIndex)
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        Lookup_CtlPostingCodeLookup account = (Lookup_CtlPostingCodeLookup)uiDgJournal.Rows[rowIndex].FindControl("CtlPostingCodeLookup");
        TextBox drAmount = (TextBox)uiDgJournal.Rows[rowIndex].FindControl("uiTxtDrAmount");
        TextBox crAmount = (TextBox)uiDgJournal.Rows[rowIndex].FindControl("uiTxtCrAmount");
        TextBox linDesc = (TextBox)uiDgJournal.Rows[rowIndex].FindControl("uiTxtLineDescription");
        Lookup_CtlClearingMemberLookup attCode1 = (Lookup_CtlClearingMemberLookup)uiDgJournal.Rows[rowIndex].FindControl("CtlClearingMemberLookup");

        if (string.IsNullOrEmpty(account.LookupTextBoxID))
        {
            uiBLError.Items.Add(string.Format("Row {0} : Account is required.", rowIndex + 1));
        }

        if ((string.IsNullOrEmpty(drAmount.Text)) && string.IsNullOrEmpty(crAmount.Text))
        {
            uiBLError.Items.Add(string.Format("Row {0} : DrAmount or CrAmount is required.", rowIndex + 1));
        }

        //if (string.IsNullOrEmpty(attCode1.LookupTextBoxID))
        //{
        //    uiBLError.Items.Add(string.Format("Row {0} : Attribute 1 Code is required.", rowIndex + 1));
        //}

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void SetDefaultValues()
    {
        CtlCalendarPickUpPostingDate.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        uiDdlJournalType.Enabled = false;
        uiDdlJournalType.SelectedIndex = 2;
    }

    private void SetVisibleControls(bool b)
    {
        MasterPage mp = (MasterPage)this.Master;
        if (mp.IsMaker)
        {
            uiBtnAdd.Visible = b;
            uiBtnSave.Visible = b;
        }
    }

    private void SetEnabledControl(bool b)
    {
        uiTxtPostingNo.Enabled = b;
        CtlCalendarPickUpPostingDate.DisabledCalendar = !b;
        uiDdlCurrency.Enabled = b;
        uiDdlLedgerType.Enabled = b;
        uiDdlJournalType.Enabled = b;
        uiTxtDescription.Enabled = b;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        trApprovalDes.Visible = mp.IsChecker;
        uiBtnAdd.Visible = mp.IsMaker;
        uiBtnSave.Visible = mp.IsMaker;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
    }

    private void SaveManualPosting()
    {
        if (IsValidEntry("") == true)
        {
            try
            {
                Journal.UpdateJournal(eID, 0, DateTime.Parse(CtlCalendarPickUpPostingDate.Text),
                    uiDdlLedgerType.SelectedValue, "P", decimal.Parse(uiDdlCurrency.SelectedValue),
                    uiDdlJournalType.SelectedValue, uiTxtDescription.Text,
                    User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, uiTxtApprovalDesription.Text, dtJournalLine);

                Response.Redirect("ViewManualPosting.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    protected void uiBtnAddAndSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("") == true)
        {
            try
            {
                SaveManualPosting();
                Response.Redirect("EntryManualPosting.aspx?eType=add");
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }
}
