using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

public partial class WebUI_New_EntrySettlementPrice : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private int RowIndex
    {
        get { return int.Parse(ViewState["RowIndex"].ToString()); }
        set { ViewState["RowIndex"] = value; }
    }

    private DateTime businessDate
    {
        get
        {
            if (Request.QueryString["businessDate"] == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(Request.QueryString["businessDate"]);
            }
        }
        set { ViewState["businessDate"] = value; }
    }

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiBLError.Visible = false;
        
        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                RowIndex = 0;
                uiDgSettlementPrice.Visible = false;
                trAllButton.Visible = false;
                dtContract = new SettlementPriceData.ContractDataTable();
                dtContract1 = new SettlementPriceData.SettlementPriceCreateEditDataTable();
                dtSettlementUpdate = new SettlementPriceData.SettlementPriceDataTable();
               
                if (businessDate != null)
                {
                    trAllButton.Visible = true;
                    dtContract1.Clear();
                    dtContract1.AcceptChanges();
                    dtContract1 = SettlementPrice.FillForCreate(businessDate,uiDdlSettlePriceType.SelectedValue);
                    CtlCalendarBusinessDate.SetCalendarValue(businessDate.ToString("dd-MMM-yyyy"));
                    uiDgContract.DataSource = dtContract1;
                    uiDgContract.DataBind();
                    uiBtnAdd.Visible = true;
                }
                
            }
            else if (eType == "create")
            {
                CtlCalendarBusinessDate.SetCalendarValue(businessDate.ToString("dd-MMM-yyyy"));
            }
            else if (eType == "edit")
            {
                uiDgContract.Visible = false;
                //bind data from table IRCA status 'A'
                dtSettlementUpdate = new SettlementPriceData.SettlementPriceDataTable();
                bindData();
                uiBtnAdd.Visible = false;
                uiBtnImport.Visible = false;
                //CtlCalendarBusinessDate.SetCalendarValue(businessDate.ToString("dd-MMM-yyyy"));
                trSettlementPriceType.Visible = false;
                trBusinessDate.Visible = false;
            }
            else if (eType == "transaction")
            {
                uiDgContract.Visible = false;
                //bind data from table IRCA status 'A' and 'P'
                bindTrxData();

                //validasi buat readonly textbox settlement price
                MasterPage mp = (MasterPage)this.Master;
                if (mp.IsChecker)
                {
                    foreach (GridViewRow gvr in uiDgSettlementPrice.Rows)
                    {
                        TextBox txtSettlementPrice = new TextBox();
                        txtSettlementPrice = ((TextBox)gvr.FindControl("uiTxbSettlePriceEdit"));
                        txtSettlementPrice.ReadOnly = true;
                    }
                }
            }
        }
    }

    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    protected void uiDgContract_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgContract.PageIndex = e.NewPageIndex;
            int rowIndex = e.NewPageIndex;
            if (e.NewPageIndex > 0)
            {
                RowIndex = (uiDgContract.PageSize * rowIndex);
            }
            else
            {
                RowIndex = 0;
            }
            

            if (eType == "edit")
            {
                FillContractDataGrid();
            }
            else if (eType == "add")
            {
                SettlementPriceData.SettlementPriceCreateEditDataTable dtContract1 = new SettlementPriceData.SettlementPriceCreateEditDataTable();
                dtContract1 = SettlementPrice.FillForCreate(businessDate,uiDdlSettlePriceType.SelectedValue);

                uiDgContract.DataSource = dtContract1;
                uiDgContract.DataBind();
            }
            
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgContract_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(SortOrder))
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrder = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrder = e.SortExpression + " " + "ASC";
                }
            }

            FillContractDataGrid();
            if (eType == "add")
            {
                dtContract1 = new SettlementPriceData.SettlementPriceCreateEditDataTable();
                dtSettlementUpdate = new SettlementPriceData.SettlementPriceDataTable();
                if (businessDate != null)
                {
                    trAllButton.Visible = true;
                    dtContract1.Clear();
                    dtContract1.AcceptChanges();
                    dtContract1 = SettlementPrice.FillForCreate(businessDate, uiDdlSettlePriceType.SelectedValue);
                    CtlCalendarBusinessDate.SetCalendarValue(businessDate.ToString("dd-MMM-yyyy"));
                    uiDgContract.DataSource = dtContract1;
                    uiDgContract.DataBind();
                    uiBtnAdd.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void bindTrxData()
    {
        uiDgSettlementPrice.DataSource = ObjectDataSourceTrxSettlementPrice;
        IEnumerable dvi = (IEnumerable)ObjectDataSourceTrxSettlementPrice.Select();
        DataView dve = (DataView)dvi;

        dtSettlement = (SettlementPriceData.SettlementPriceDataTable)dve.Table;
        uiDgSettlementPrice.DataSource = dtSettlement;
        uiDgSettlementPrice.DataBind();
    }

    private void bindData()
    {

        uiDgSettlementPrice.DataSource = ObjectDataSourceSettlementPrice;

        IEnumerable dvi = (IEnumerable)ObjectDataSourceSettlementPrice.Select();
        DataView dve = (DataView)dvi;

        dtSettlement = (SettlementPriceData.SettlementPriceDataTable)dve.Table;
        uiDgSettlementPrice.DataSource = dtSettlement;
        uiDgSettlementPrice.DataBind();

    }

    private SettlementPriceData.SettlementPriceDataTable dtSettlement
    {
        get { return (SettlementPriceData.SettlementPriceDataTable)ViewState["dtSettlement"]; }
        set { ViewState["dtSettlement"] = value; }
    }

    private SettlementPriceData.SettlementPriceDataTable dtSettlementUpdate
    {
        get { return (SettlementPriceData.SettlementPriceDataTable)ViewState["dtSettlementUpdate"]; }
        set { ViewState["dtSettlementUpdate"] = value; }
    }

    private SettlementPriceData.ContractDataTable dtContract
    {
        get { return (SettlementPriceData.ContractDataTable)ViewState["dtContract"]; }
        set { ViewState["dtContract"] = value; }
    }

    private SettlementPriceData.SettlementPriceCreateEditDataTable dtContract1
    {
        get { return (SettlementPriceData.SettlementPriceCreateEditDataTable)ViewState["dtContract1"]; }
        set { ViewState["dtContract1"] = value; }
    }

    private void BindViewStateContractToDataGrid()
    {
        uiDgContract.DataSource = dtContract;
        uiDgContract.DataBind();
    }

    private void BindViewStateSettlementPriceToDataGrid()
    {
        uiDgSettlementPrice.DataSource = dtSettlement;
        uiDgSettlementPrice.DataBind();
    }


    protected void uiDgSettlementPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgSettlementPrice.PageIndex = e.NewPageIndex;
            FillSettlementPriceDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgSettlementPrice_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(SortOrder))
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else
            {
                string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
                if (arrSortOrder[1] == "ASC")
                {
                    SortOrder = e.SortExpression + " " + "DESC";
                }
                else if (arrSortOrder[1] == "DESC")
                {
                    SortOrder = e.SortExpression + " " + "ASC";
                }
            }

            FillSettlementPriceDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillSettlementPriceDataGrid()
    {
        try
        {
            uiDgSettlementPrice.DataSource = ObjectDataSourceContract;

            IEnumerable dv = (IEnumerable)ObjectDataSourceSettlementPrice.Select();
            DataView dve = (DataView)dv;

            dtSettlement = (SettlementPriceData.SettlementPriceDataTable)dve.Table;
            TextBox TxtSettlementPrice = (TextBox)uiDgContract.FindControl("uiTxtSettlementPriceValueContract");

            uiDgSettlementPrice.DataSource = dtSettlement;
            uiDgSettlementPrice.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
   
    protected void uiBtnImport_Click1(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            Response.Redirect("ImportSettlementPrice.aspx?menu=hide");
        }
        else
        {

            Response.Redirect("ImportSettlementPrice.aspx");
        }
        
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            Response.Redirect("ViewSettlementPrice.aspx?menu=hide");
        }
        else
        {

            Response.Redirect("ViewSettlementPrice.aspx");
        }
        
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {

                //check di settlemen price business date tanggal skrg sudah ada atau blm dan status klo propose tidak
                //boleh insert lg

                if (eType == "add")
                {
                    //add with datagrid contract
                    SettlementPriceData.SettlementPriceCreateEditRow dr = null;
                    SettlementPriceData.SettlementPriceRow drUpdate = null;
                    foreach (GridViewRow gvr in uiDgContract.Rows)
                    {
                        TextBox settelmentPriceValue = (TextBox)gvr.FindControl("uiTxtSettlementPriceValueContract");

                        //dr = (SettlementPriceData.SettlementPriceCreateEditRow)dtContract1.Rows[gvr.RowIndex];
                        dr = (SettlementPriceData.SettlementPriceCreateEditRow)dtContract1.Rows[RowIndex];

                        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
                        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();

                        if (!string.IsNullOrEmpty(settelmentPriceValue.Text))
                        {
                            // Guard for editing proposed record
                            SettlementPriceData.SettlementPriceDataTable dtBusiness = SettlementPrice.GetSettlementByCriteria(DateTime.Parse(CtlCalendarBusinessDate.Text), 
                                                                                      dr.ContractID,uiDdlSettlePriceType.SelectedValue);
                            if (dtBusiness.Count > 0) throw new ApplicationException("Data already exist.");
                            drUpdate = dtSettlementUpdate.NewSettlementPriceRow();
                            drUpdate.BusinessDate = DateTime.Parse(CtlCalendarBusinessDate.Text);
                            drUpdate.ContractID = dr.ContractID;
                            drUpdate.SettlementPriceType = uiDdlSettlePriceType.SelectedValue;
                            drUpdate.ApprovalStatus = "P";
                            drUpdate.SettlementPrice = decimal.Parse(settelmentPriceValue.Text);
                            drUpdate.CreatedBy = User.Identity.Name;
                            drUpdate.CreatedDate = DateTime.Now;
                            drUpdate.LastUpdatedBy = User.Identity.Name;
                            drUpdate.LastUpdatedDate = DateTime.Now;
                            drUpdate.SetApprovalDescNull();
                            drUpdate.SetOriginalSettleIDNull();
                            drUpdate.ActionFlag = "I";
                            drUpdate.SettleID = (decimal)ta.GetMaxSettleID() + 1;

                            dtSettlementUpdate.AddSettlementPriceRow(drUpdate);
                        }

                        RowIndex++;
                    }
                    
                    SettlementPrice.ProcessSettlementPrice(DateTime.Parse(CtlCalendarBusinessDate.Text), dtSettlementUpdate, User.Identity.Name);

                    //redirect after insert
                    if (Menu == "hide")
                    {
                        Response.Redirect("ViewSettlementPrice.aspx?menu=hide");
                    }
                    else
                    {

                        Response.Redirect("ViewSettlementPrice.aspx");
                    }                    
                }
                else if (eType == "edit")
                {

                    //edit with datagrid settlementprice
                    SettlementPriceData.SettlementPriceRow dr = null;
                    SettlementPriceData.SettlementPriceRow drUpdate = null;
                    
                    CtlCalendarBusinessDate.SetCalendarValue(businessDate.ToString("dd-MMM-yyyy"));
                    bool isCheckboxChecked = false;
                    foreach (GridViewRow gvr in uiDgSettlementPrice.Rows)
                    {
                        CheckBox selectedCheckBox = new CheckBox();
                        selectedCheckBox = ((CheckBox)gvr.FindControl("uiChkList"));
                        TextBox SettlementPriceValue = (TextBox)gvr.FindControl("uiTxbSettlePriceEdit");
                        
                        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
                        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();

                        decimal settleId = 0;
                        decimal contractId = 0;
                        string createdBy = "";
                        DateTime createdDate = DateTime.MinValue;
                        Label createdbyLbl = (Label)gvr.FindControl("uiLblCreatedBy");
                        Label settleLbl = (Label)gvr.FindControl("uiLblSettleId");
                        Label contractLbl = (Label)gvr.FindControl("uiLblContractID");
                        Label createdDateLbl = (Label)gvr.FindControl("uiLblCreatedDate");
                        createdBy = createdbyLbl.Text;
                        createdDate = DateTime.Parse(createdDateLbl.Text);
                        settleId = decimal.Parse(settleLbl.Text);
                        contractId = decimal.Parse(contractLbl.Text);
                        string settlePriceType = gvr.Cells[6].Text;
                        if (settlePriceType == "Normal")
                        {
                            settlePriceType = "N";
                        }
                        else if (settlePriceType == "Urgent")
                        {
                            settlePriceType = "U";
                        }
                        if (selectedCheckBox.Checked == true)
                        {
                            isCheckboxChecked = true;
                            SettlementPriceData.SettlementPriceDataTable dtBusiness = SettlementPrice.GetSettlementByCriteria(businessDate.Date, contractId, settlePriceType);
                            if (dtBusiness.Count > 0)
                            {
                                if (dtBusiness[0].ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                            }
                            if (!string.IsNullOrEmpty(SettlementPriceValue.Text))
                            {

                                drUpdate = dtSettlementUpdate.NewSettlementPriceRow();
                                drUpdate.BusinessDate = businessDate.Date;//DateTime.Parse(CtlCalendarBusinessDate.Text);
                                drUpdate.ContractID = contractId;
                                drUpdate.SettlementPriceType = settlePriceType;
                                drUpdate.ApprovalStatus = "P";
                                drUpdate.SettlementPrice = decimal.Parse(SettlementPriceValue.Text);
                                drUpdate.CreatedBy = createdBy;
                                drUpdate.CreatedDate = createdDate;
                                drUpdate.LastUpdatedBy = User.Identity.Name;
                                drUpdate.LastUpdatedDate = DateTime.Now;
                                drUpdate.SetApprovalDescNull();
                                drUpdate.OriginalSettleID = settleId;
                                drUpdate.ActionFlag = "U";
                                drUpdate.SettleID = (decimal)ta.GetMaxSettleID() + 1;
                            }

                            dtSettlementUpdate.AddSettlementPriceRow(drUpdate);
                        }
                       
                    }

                    SettlementPrice.ProcessSettlementPrice(businessDate.Date, dtSettlementUpdate, User.Identity.Name);

                    //redirect after insert
                    if (isCheckboxChecked)
                    {
                        if (Menu == "hide")
                        {
                            Response.Redirect("ViewSettlementPrice.aspx?menu=hide");
                        }
                        else
                        {

                            Response.Redirect("ViewSettlementPrice.aspx");
                        }        
                    }
                   
                }
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
            List<string> settlementValueList = new List<string>();

            if (IsValidEntry() == true)
            {
                foreach (GridViewRow GridRow in uiDgSettlementPrice.Rows)
                {

                    CheckBox selectedCheckBox = new CheckBox();
                    selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                    TextBox txtSettlementVAlue = new TextBox();
                    txtSettlementVAlue = ((TextBox)GridRow.FindControl("uiTxbSettlePriceEdit"));
                    TextBox txtApprovalDesc = new TextBox();
                    txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));

                    // Guard for editing proposed record
                    Label uiLblSettleId = (Label)GridRow.FindControl("uiLblSettleId");
                    SettlementPriceData.SettlementPriceRow dr = SettlementPrice.SelectSettlementBySettleID(Convert.ToDecimal(uiLblSettleId.Text));
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                    if (selectedCheckBox.Checked)
                    {
                        settlementValueList.Add(dr.ContractID + "|" + dr.BusinessDate.Date +
                                        "|" + txtSettlementVAlue.Text + "|" + dr.SettlementPriceType + "|"
                                        + dr.ActionFlag + "|" + dr.ApprovalStatus
                                        + "|" + txtApprovalDesc.Text + "|" + dr.SettleID.ToString());
                    }
                }
                //approve to table irca
                SettlementPrice.Approve(settlementValueList, businessDate.Date, User.Identity.Name);

                //redirect after insert
                if (Menu == "hide")
                {
                    Response.Redirect("ViewSettlementPrice.aspx?menu=hide");
                }
                else
                {

                    Response.Redirect("ViewSettlementPrice.aspx");
                }        

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
            List<string> settlementValueList = new List<string>();

            if (IsValidEntry() == true)
            {
                foreach (GridViewRow GridRow in uiDgSettlementPrice.Rows)
                {

                    CheckBox selectedCheckBox = new CheckBox();
                    selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                    TextBox txtSettlementVAlue = new TextBox();
                    txtSettlementVAlue = ((TextBox)GridRow.FindControl("uiTxbSettlePriceEdit"));
                    TextBox txtApprovalDesc = new TextBox();
                    txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));

                    // Guard for editing proposed record
                    Label uiLblSettleId = (Label)GridRow.FindControl("uiLblSettleId");
                   
                    if (selectedCheckBox.Checked)
                    {
                        SettlementPriceData.SettlementPriceRow dr = SettlementPrice.SelectSettlementBySettleID(Convert.ToDecimal(uiLblSettleId.Text));
                        if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                        settlementValueList.Add(dr.ContractID + "|" + dr.BusinessDate.Date +
                                        "|" + txtSettlementVAlue.Text + "|" + dr.SettlementPriceType + "|"
                                        + dr.ActionFlag + "|" + dr.ApprovalStatus
                                        + "|" + txtApprovalDesc.Text + "|" + dr.SettleID.ToString());
                    }
                }
                //approve to table irca
                SettlementPrice.Reject(settlementValueList, businessDate.Date, User.Identity.Name);

                //redirect after insert
                if (Menu == "hide")
                {
                    Response.Redirect("ViewSettlementPrice.aspxmenu=hide");
                }
                else
                {

                    Response.Redirect("ViewSettlementPrice.aspx");
                }        


            }

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnAdd_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                trAllButton.Visible = true;

                uiDgSettlementPrice.Visible = false;
                //trAllButton.Visible = false;
                dtContract = new SettlementPriceData.ContractDataTable();
                dtSettlementUpdate = new SettlementPriceData.SettlementPriceDataTable();


                dtContract1 = new SettlementPriceData.SettlementPriceCreateEditDataTable();
                dtContract1 = SettlementPrice.FillForCreate(DateTime.Parse(CtlCalendarBusinessDate.Text), uiDdlSettlePriceType.SelectedValue);

                uiDgContract.DataSource = dtContract1;
                uiDgContract.DataBind();

            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }

    private void FillContractDataGrid()
    {
        try
        {
            uiDgContract.DataSource = ObjectDataSourceContract;

            IEnumerable dv = (IEnumerable)ObjectDataSourceContract.Select();
            DataView dve = (DataView)dv;

            dtContract = (SettlementPriceData.ContractDataTable)dve.Table;
            TextBox TxtSettlementPrice = (TextBox)uiDgContract.FindControl("uiTxtSettlementPriceValueContract");

            uiDgContract.DataSource = dtContract;
            uiDgContract.DataBind();
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
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (mp.IsMaker)
        {
            if (eType != "edit")
            {
                if (CtlCalendarBusinessDate.Text == "")
                {
                    uiBLError.Items.Add("Please fill business date field.");
                }
                uiBLError.Visible = true;
            }
        }

        if (mp.IsChecker)
        {
            foreach (GridViewRow GridRow in uiDgSettlementPrice.Rows)
            {
                TextBox txtApprovalDesc = new TextBox();
                txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));
                CheckBox selectedCheckBox = new CheckBox();
                selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));

                if (selectedCheckBox.Checked == true)
                {
                    if (txtApprovalDesc.Text == "")
                    {
                        txtApprovalDesc.BorderColor = System.Drawing.Color.Red;
                        txtApprovalDesc.ToolTip = "Please fill approval desctiption.";
                        uiBLError.Items.Add("Approval description is required.");
                    }
                }
            }
            uiBLError.Visible = true;
        }
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;

        uiDgSettlementPrice.Columns[1].Visible = false;
        uiDgSettlementPrice.Columns[3].Visible = false;
        //uiDgSettlementPrice.Columns[4].Visible = true;
        //uiDgSettlementPrice.Columns[5].Visible = true;
        //uiDgSettlementPrice.Columns[6].Visible = mp.IsMaker;
        uiDgSettlementPrice.Columns[8].Visible = mp.IsChecker;
        uiDgSettlementPrice.Columns[9].Visible = false;
        uiDgSettlementPrice.Columns[10].Visible = false;
        uiDgSettlementPrice.Columns[11].Visible = mp.IsChecker;
        

        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
        if (eType == "add")
        {
            uiBtnImport.Visible = mp.IsMaker;
        }
       
        ////// set disabled for other controls other than approval description, for checker
        if (mp.IsChecker)
        {
            uiBtnSave.Visible = mp.IsMaker;
            trBusinessDate.Visible = mp.IsMaker;
            trButtonAdd.Visible = mp.IsMaker;
            trSettlementPriceType.Visible = mp.IsMaker;
        }
    }

    #endregion

    protected void uiDgSettlementPrice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label actionDesc = (Label)e.Row.FindControl("uiLblActionDesc");
            switch (actionDesc.Text)
            { 
                case "I" :
                    actionDesc.Text = "Proposed Insert";
                    break;
                case "U" :
                    actionDesc.Text = "Proposed Update";
                    break;
                default :
                    actionDesc.Text = "Approved";
                    break;

            }
        }
    }


    protected void uiDgSettlementPrice_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
