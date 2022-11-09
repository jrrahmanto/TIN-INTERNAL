using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

public partial class WebUI_New_EntryIRCA : System.Web.UI.Page
{

    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private DateTime startDate
    {
        get
        {
            if (Request.QueryString["startDate"] == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.Parse(Request.QueryString["startDate"]);
            }
        }
        set { ViewState["startDate"] = value; }
    }

    private IRCAData.IRCADataTable dtIRCA
    {
        get { return (IRCAData.IRCADataTable)ViewState["dtIRCA"]; }
        set { ViewState["dtIRCA"] = value; }
    }

    private IRCAData.CommodityDataTable dtCommodity
    {
        get { return (IRCAData.CommodityDataTable)ViewState["dtCommodity"]; }
        set { ViewState["dtCommodity"] = value; }
    }

    private IRCAData.IRCADataTable dtIRCAUpdate
    {
        get { return (IRCAData.IRCADataTable)ViewState["dtIRCAUpdate"]; }
        set { ViewState["dtIRCAUpdate"] = value; }
    }

    private IRCAData.IRCACreateEditDataTable dtCommodity1
    {
        get { return (IRCAData.IRCACreateEditDataTable)ViewState["dtCommodity1"]; }
        set { ViewState["dtCommodity1"] = value; }
    }

    private void BindViewStateCommodityToDataGrid()
    {
        uiDgCommodity.DataSource = dtCommodity;
        uiDgCommodity.DataBind();
    }

    private void BindViewStateIRCAToDataGrid()
    {
        uiDgIRCA.DataSource = dtIRCA;
        uiDgIRCA.DataBind();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                uiDgIRCA.Visible = false;
                trAllButton.Visible = false;
                trButtonAdd.Visible = false;
                dtCommodity = new IRCAData.CommodityDataTable();
                dtCommodity1 = new IRCAData.IRCACreateEditDataTable();
                dtIRCAUpdate = new IRCAData.IRCADataTable();

                if (startDate != null)
                {
                    trAllButton.Visible = true;
                    dtCommodity1.Clear();
                    dtCommodity1.AcceptChanges();
                    dtCommodity1 = IRCA.FillForCreate(startDate);
                    CtlCalendarEffectiveStartDate.SetCalendarValue(startDate.ToString("dd-MMM-yyyy"));
                    uiDgCommodity.DataSource = dtCommodity1;
                    uiDgCommodity.DataBind();
                    uiBtnAdd.Visible = true;
                }
            }
            else if (eType == "edit")
            {
                uiDgCommodity.Visible = false;
                //bind data from table IRCA status 'A'
                trButtonAdd.Visible = false;
                trEffectiveStartDate.Visible = false;
                dtIRCAUpdate = new IRCAData.IRCADataTable();
                bindData();
            }
            else if (eType == "transaction")
            {
                uiDgCommodity.Visible = false;
                //bind data from table IRCA status 'A' and 'P'
                bindTrxData();

                //validasi buat readonly textbox irca
                MasterPage mp = (MasterPage)this.Master;
                if (mp.IsChecker)
                {
                    foreach (GridViewRow gvr in uiDgIRCA.Rows)
                    {
                        TextBox txtIRCA = new TextBox();
                        txtIRCA = ((TextBox)gvr.FindControl("uiTxtIRCAValue"));
                        txtIRCA.ReadOnly = true;
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


    private void bindTrxData()
    {
        uiDgIRCA.DataSource = ObjectDataSourceTrxIRCA;
        IEnumerable dvi = (IEnumerable)ObjectDataSourceTrxIRCA.Select();
        DataView dve = (DataView)dvi;

        dtIRCA = (IRCAData.IRCADataTable)dve.Table;
        uiDgIRCA.DataSource = dtIRCA;
        uiDgIRCA.DataBind();
    }

    private void bindData()
    {
        
        uiDgIRCA.DataSource = ObjectDataSourceIRCA;

        IEnumerable dvi = (IEnumerable)ObjectDataSourceIRCA.Select();
        DataView dve = (DataView)dvi;

        dtIRCA = (IRCAData.IRCADataTable)dve.Table;

        uiDgIRCA.DataSource = dtIRCA;
        uiDgIRCA.DataBind();

    }

    protected void uiDgCommodity_Sorting(object sender, GridViewSortEventArgs e)
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

            FillCommodityDataGrid();
            if (eType == "add")
            {
                dtCommodity1 = new IRCAData.IRCACreateEditDataTable();
                dtIRCAUpdate = new IRCAData.IRCADataTable();
                if (startDate != null)
                {
                    trAllButton.Visible = true;
                    dtCommodity1.Clear();
                    dtCommodity1.AcceptChanges();
                    dtCommodity1 = IRCA.FillForCreate(startDate);
                    CtlCalendarEffectiveStartDate.SetCalendarValue(startDate.ToString("dd-MMM-yyyy"));
                    uiDgCommodity.DataSource = dtCommodity1;
                    uiDgCommodity.DataBind();
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


    protected void uiDgCommodity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgCommodity.PageIndex = e.NewPageIndex;
            if (eType == "edit")
            {
                FillCommodityDataGrid();
            }
            else if (eType == "add")
            {
                IRCAData.IRCACreateEditDataTable dtCommodity1 = new IRCAData.IRCACreateEditDataTable();
                dtCommodity1 = IRCA.FillForCreate(startDate);

                uiDgCommodity.DataSource = dtCommodity1;
                uiDgCommodity.DataBind();
            }

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    private void FillCommodityDataGrid()
    {
        try
        {
            uiDgCommodity.DataSource = ObjectDataSourceCommodity;

            IEnumerable dv = (IEnumerable)ObjectDataSourceCommodity.Select();
            DataView dve = (DataView)dv;

            dtCommodity = (IRCAData.CommodityDataTable)dve.Table;
            TextBox TxtIRCA = (TextBox)uiDgCommodity.FindControl("uiTxtIRCAValueCommodity");

            uiDgCommodity.DataSource = dtCommodity;
            uiDgCommodity.DataBind();
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
                uiBtnSave.Visible = true;
                FillCommodityDataGrid();
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
        Response.Redirect("ViewIRCA.aspx");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> ircaValueList = new List<string>();

            if (IsValidEntry() == true)
            {
            foreach (GridViewRow GridRow in uiDgIRCA.Rows)
            {
               
                CheckBox selectedCheckBox = new CheckBox();
                selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                TextBox txtIRCAVAlue = new TextBox();
                txtIRCAVAlue = ((TextBox)GridRow.FindControl("uiTxtIRCAValue"));
                TextBox txtApprovalDesc = new TextBox();
                txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));

                    // Guard for editing proposed record
                    Label lblIRCAID = (Label)GridRow.FindControl("uiLblIRCAID");
                    IRCAData.IRCARow dr = IRCA.SelectIRCAByIRCAID(Convert.ToDecimal(lblIRCAID.Text));
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                    if (selectedCheckBox.Checked)
                    {
                            ircaValueList.Add(dr.CommodityID + "|" + dr.EffectiveStartDate.Date +
                                            "|" + txtIRCAVAlue.Text + "|" 
                                            + dr.ActionFlag + "|" + dr.ApprovalStatus
                                            + "|" + txtApprovalDesc.Text + "|" + dr.IRCAID.ToString());
                     }
                }
            //approve to table irca
            IRCA.Approve(ircaValueList, startDate.Date, User.Identity.Name);

            //redirect after insert
            Response.Redirect("ViewIRCA.aspx");
            
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
            List<string> ircaValueList = new List<string>();

            if (IsValidEntry() == true)
            {
            foreach (GridViewRow GridRow in uiDgIRCA.Rows)
            {

                CheckBox selectedCheckBox = new CheckBox();
                selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
                TextBox txtIRCAVAlue = new TextBox();
                txtIRCAVAlue = ((TextBox)GridRow.FindControl("uiTxtIRCAValue"));
                TextBox txtApprovalDesc = new TextBox();
                txtApprovalDesc = ((TextBox)GridRow.FindControl("uiTxtApprovalDesc"));

                
                    // Guard for editing proposed record
                    Label lblIRCAID = (Label)GridRow.FindControl("uiLblIRCAID");
                    IRCAData.IRCARow dr = IRCA.SelectIRCAByIRCAID(Convert.ToDecimal(lblIRCAID.Text));
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                    if (selectedCheckBox.Checked)
                    {
                        ircaValueList.Add(dr.CommodityID + "|" + dr.EffectiveStartDate.Date +
                                        "|" + txtIRCAVAlue.Text + "|"
                                        + dr.ActionFlag + "|" + dr.ApprovalStatus
                                        + "|" + txtApprovalDesc.Text + "|" + dr.IRCAID.ToString());
                    }
                }
            //approve to table irca
            IRCA.Reject(ircaValueList, startDate.Date, User.Identity.Name);

            //redirect after insert
            Response.Redirect("ViewIRCA.aspx");
        

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
        try
        {
            if (IsValidEntry() == true)
            {

                if (eType == "add")
                {
                    //add with datagrid commodity
                    IRCAData.IRCACreateEditRow dr = null;
                    IRCAData.IRCARow drUpdate = null;
                    foreach (GridViewRow gvr in uiDgCommodity.Rows)
                    {
                        TextBox ircaValue = (TextBox)gvr.FindControl("uiTxtIRCAValueCommodity");
                        Label commodityName = (Label)gvr.FindControl("uiLblCommdityName");
                        Label commodityId = (Label)gvr.FindControl("uiLblCommodityId");

                        dr = (IRCAData.IRCACreateEditRow)dtCommodity1.Rows[gvr.RowIndex];

                        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();
                        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();

                        if (!string.IsNullOrEmpty(ircaValue.Text) && IRCA.isValidDecimal(ircaValue.Text, commodityName.Text))
                        {
                          
                            // Guard for editing proposed record
                            IRCAData.IRCADataTable dtStartDate = IRCA.GetIRCAByCriteria(DateTime.Parse(CtlCalendarEffectiveStartDate.Text),
                                                                                      decimal.Parse(commodityId.Text));
                            if (dtStartDate.Count > 0) throw new ApplicationException("Data already exist.");

                            CommodityDataTableAdapters.CommodityTableAdapter taCommodity = new CommodityDataTableAdapters.CommodityTableAdapter();
                            CommodityData.CommodityDataTable dtCommodity = new CommodityData.CommodityDataTable();
                            taCommodity.FillByCommodityId(dtCommodity, decimal.Parse(commodityId.Text));

                            drUpdate = dtIRCAUpdate.NewIRCARow();
                            drUpdate.CommodityID = decimal.Parse(commodityId.Text);
                            drUpdate.EffectiveStartDate = DateTime.Parse(CtlCalendarEffectiveStartDate.Text);
                            drUpdate.ApprovalStatus = "P";
                            drUpdate.IRCAValue = decimal.Parse(ircaValue.Text);
                            drUpdate.CreatedBy = User.Identity.Name;
                            drUpdate.CreatedDate = DateTime.Now;
                            drUpdate.LastUpdatedBy = User.Identity.Name;
                            drUpdate.LastUpdatedDate = DateTime.Now;
                            drUpdate.SetEffectiveEndDateNull();
                            drUpdate.SetApprovalDescNull();
                            drUpdate.SetOriginalIDNull();
                            drUpdate.ActionFlag = actionFlag;
                            drUpdate.IRCAID = (decimal)ta.GetMaxIRCAID() + 1;

                            if (!string.IsNullOrEmpty(drUpdate.IRCAID.ToString()))
                            {
                                //guard for number record
                                decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordsBeforeStartDate(Convert.ToDecimal(dr.CommodityID), DateTime.Parse(CtlCalendarEffectiveStartDate.Text), Convert.ToDecimal(drUpdate.IRCAID)));
                                if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");
                            }

                            dtIRCAUpdate.AddIRCARow(drUpdate);
                          
                        }
                       
                    }

                    IRCA.ProcessIRCA(DateTime.Parse(CtlCalendarEffectiveStartDate.Text), dtIRCAUpdate, User.Identity.Name);

                    //redirect after insert
                    Response.Redirect("ViewIRCA.aspx");
                }
                else if (eType == "edit")
                {

                    //edit with datagrid IRCA
                    IRCAData.IRCARow dr = null;
                    IRCAData.IRCARow drUpdate = null;

                    CtlCalendarEffectiveStartDate.SetCalendarValue(startDate.ToString("dd-MMM-yyyy"));

                    foreach (GridViewRow gvr in uiDgIRCA.Rows)
                    {
                        CheckBox selectedCheckBox = new CheckBox();
                        selectedCheckBox = ((CheckBox)gvr.FindControl("uiChkList"));
                        TextBox ircaValue = (TextBox)gvr.FindControl("uiTxtIRCAValue");

                                               
                        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();
                        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();

                        decimal ircaId = 0;
                        decimal commodityId = 0;
                        string createdBy = "";
                        DateTime createdDate = DateTime.MinValue;
                        Label createdbyLbl = (Label)gvr.FindControl("uiLblCreatedBy");
                        Label ircaLbl = (Label)gvr.FindControl("uiLblIRCAId");
                        Label commodity = (Label)gvr.FindControl("uiLblCommodity");
                        Label createdDateLbl = (Label)gvr.FindControl("uiLblCreatedDate");
                        createdBy = createdbyLbl.Text;
                        createdDate = DateTime.Parse(createdDateLbl.Text);
                        ircaId = decimal.Parse(ircaLbl.Text);
                        commodityId = decimal.Parse(commodity.Text);

                        if (selectedCheckBox.Checked == true)
                        {   
                           if (Convert.ToDecimal(ircaId) != 0)
                           {
                                   // Guard for editing proposed record
                                   IRCAData.IRCADataTable dtStartDate = IRCA.GetIRCAByCriteria(startDate.Date, commodityId);
                                   if (dtStartDate.Count > 0)
                                   {
                                       if (dtStartDate[0].ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                                   }
                        
                                    //guard for number record
                                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordsBeforeStartDate(Convert.ToDecimal(commodityId), startDate.Date, Convert.ToDecimal(ircaId)));
                                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");
                                    actionFlag = "U";
                           }

                          if (!string.IsNullOrEmpty(ircaValue.Text))
                           {

                               CommodityDataTableAdapters.CommodityTableAdapter taCommodity = new CommodityDataTableAdapters.CommodityTableAdapter();
                               CommodityData.CommodityDataTable dtCommodityEdit = new CommodityData.CommodityDataTable();
                               taCommodity.FillByCommodityId(dtCommodityEdit, commodityId);

                               IRCA.isValidDecimal(ircaValue.Text, dtCommodityEdit[0].CommName);

                                drUpdate = dtIRCAUpdate.NewIRCARow();
                                drUpdate.CommodityID = commodityId;
                                drUpdate.EffectiveStartDate = startDate.Date;//DateTime.Parse(CtlCalendarEffectiveStartDate.Text);
                                drUpdate.ApprovalStatus = "P";
                                drUpdate.IRCAValue = decimal.Parse(ircaValue.Text);
                                drUpdate.CreatedBy = createdBy;
                                drUpdate.CreatedDate = createdDate;
                                drUpdate.LastUpdatedBy = User.Identity.Name;
                                drUpdate.LastUpdatedDate = DateTime.Now;
                                drUpdate.SetEffectiveEndDateNull();
                                drUpdate.SetApprovalDescNull();
                                drUpdate.OriginalID = ircaId;
                                drUpdate.ActionFlag = actionFlag;
                                drUpdate.IRCAID = (decimal)ta.GetMaxIRCAID() + 1;
                            }

                          dtIRCAUpdate.AddIRCARow(drUpdate);
                       }
                        
                    }

                    IRCA.ProcessIRCA(startDate.Date, dtIRCAUpdate, User.Identity.Name);

                    //redirect after insert
                    Response.Redirect("ViewIRCA.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            dtIRCAUpdate.Clear();
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
                if (CtlCalendarEffectiveStartDate.Text == "")
                {
                    uiBLError.Items.Add("Please fill effective start date field.");
                }
                uiBLError.Visible = true;
            }
        }

        if (mp.IsChecker)
        {
            foreach (GridViewRow GridRow in uiDgIRCA.Rows)
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

        uiDgIRCA.Columns[1].Visible = mp.IsMaker || mp.IsChecker;
        uiDgIRCA.Columns[2].Visible = false;
        uiDgIRCA.Columns[3].Visible = mp.IsMaker || mp.IsChecker;
        uiDgIRCA.Columns[6].Visible = mp.IsChecker;
        uiDgIRCA.Columns[4].Visible = mp.IsMaker || mp.IsChecker;
        uiDgIRCA.Columns[5].Visible = false;
        uiDgIRCA.Columns[7].Visible = false;
        uiDgIRCA.Columns[8].Visible = false;
        uiDgIRCA.Columns[9].Visible = mp.IsChecker;

        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;

        ////// set disabled for other controls other than approval description, for checker
        if (mp.IsChecker)
        {
            uiBtnSave.Visible = mp.IsMaker;
            trEffectiveStartDate.Visible = mp.IsMaker;
            trButtonAdd.Visible = mp.IsMaker;
        }
    }

    #endregion
    //protected void uiDgIRCA_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label commodity = (Label)e.Row.FindControl("uiLblCommodityId");
    //        commodity.Text = Commodity.GetCommodityNameByCommID(decimal.Parse(commodity.Text));
    //    }
    //}

}
