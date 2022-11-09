using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

public partial class ClearingAndSettlement_Tender_EntryTenderApproval : System.Web.UI.Page
{
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
    }

    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
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

    private TenderData.TenderRequestDataTable dtTenderRequest
    {
        get { return (TenderData.TenderRequestDataTable)ViewState["dtTenderRequest"]; }
        set { ViewState["dtTenderRequest"] = value; }
    }

    private StringCollection scTenderID
    {
        get { return (StringCollection)ViewState["scTenderID"]; }
        set { ViewState["scTenderID"] = value; }
    }

    private decimal TenderNo
    {
        get { return (decimal)ViewState["TenderNo"]; }
        set { ViewState["TenderNo"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();

        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {

        }

        if (eType == "edit")
        {
            BindDataToForm();

            FillTenderRequestDataGrid();

            SetEnabledControls(false);
            SetVisibleControls(false);
        }
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillContractPositionDataGrid();
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("approve") == true)
        {
            try
            {
                ContractData.ContractRow drContract = Contract.GetContractByContractID(decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID));

                Tender.ProcessTenderRequest(eID, TenderNo, decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID),
                    decimal.Parse(CtlInvestorLookupSeller.LookupTextBoxID), DateTime.Now.Date,
                    uiTxtDeliveryLocation.Text, drContract.TenderReqType, "A", uiTxtApprovalDesc.Text, User.Identity.Name,
                    DateTime.Now, User.Identity.Name, DateTime.Now, dtTenderRequest, DateTime.Parse(Session["BusinessDate"].ToString()));

                if (eID != null)
                {
                    TenderData.TenderRow drTender = Tender.GetTenderByTenderId(eID);
                    if (drTender != null)
                    {
                        foreach (TenderData.TenderRequestRow dr in dtTenderRequest)
                        {
                            EventLog.AddEventQueue("TenderNotification", this.Request, "%CMSender%=" + 
                                          ClearingMember.GetCMSender(decimal.Parse(CtlInvestorLookupSeller.LookupTextBoxID)).ToString() + ";"
                                          + "%Contract%=" + CtlContractCommodityLookup1.LookupTextBox + ";"
                                          + "%Quantity%=" + dr.Quantity.ToString() + ";"
                                          + "%Price%=" + dr.Price.ToString() + ";"
                                          + "%TradePosition%=" + dr.TradePosition.ToString() + ";"
                                          + "%BusinessDate%=" + drTender.BusinessDate.ToString("dd-MMM-yyyy") + ";"
                                          + "%TenderDate%=" + drTender.TenderDate.ToString("dd-MMM-yyyy") + ";");
                        }
                    }
                }
               
                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTenderApproval.aspx");
                }
                else
                {
                    Response.Redirect("ViewTenderApproval.aspx?menu=hide");
                }                
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
                ContractData.ContractRow drContract = Contract.GetContractByContractID(decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID));

                Tender.ProcessTenderRequest(eID, TenderNo, decimal.Parse(CtlContractCommodityLookup1.LookupTextBoxID),
                    decimal.Parse(CtlInvestorLookupSeller.LookupTextBoxID), DateTime.Now.Date,
                    uiTxtDeliveryLocation.Text, drContract.TenderReqType, "R", null, User.Identity.Name,
                    DateTime.Now, User.Identity.Name, DateTime.Now, dtTenderRequest, DateTime.Parse(Session["BusinessDate"].ToString()));

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTenderApproval.aspx");
                }
                else
                {
                    Response.Redirect("ViewTenderApproval.aspx?menu=hide");
                }      
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
        if (string.IsNullOrEmpty(Menu))
        {
            Response.Redirect("ViewTenderApproval.aspx");
        }
        else
        {
            Response.Redirect("ViewTenderApproval.aspx?menu=hide");
        }      
    }

    protected void uiDgContractPosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgContractPosition.PageIndex = e.NewPageIndex;
        FillContractPositionDataGrid();
    }

    protected void uiDgContractPosition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label investor = (Label)e.Row.FindControl("uiLblInvestor");
            Label commodity = (Label)e.Row.FindControl("uiLblCommodity");
            //Change Investor ID to Investor Code
            investor.Text = Investor.GetNameInvestorByInvestorID(int.Parse(investor.Text));

            //Change Commodity ID to Commodity Name
            commodity.Text = Commodity.GetCommodityNameByCommID(int.Parse(commodity.Text));
        }
    }

    protected void uiDgContractPosition_Sorting(object sender, GridViewSortEventArgs e)
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

        FillContractPositionDataGrid();
    }

    protected void uiDgContractPosition_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        if (IsValidAdd(e.NewSelectedIndex) == true)
        {
            AddRow(e.NewSelectedIndex);
        }
    }

    protected void uiDgTenderRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void uiDgTenderRequest_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        dtTenderRequest.Rows.RemoveAt(e.NewSelectedIndex);

        BindViewStateToDataGrid();
    }

    private void AddRow(int rowIndex)
    {
        //Clear constraint to set default value    
        TenderData.TenderRequestRow dr = dtTenderRequest.NewTenderRequestRow();
        dtTenderRequest.Constraints.Clear();
        dtTenderRequest.TenderIDColumn.AllowDBNull = true;

        TextBox positionInput = (TextBox)uiDgContractPosition.Rows[rowIndex].FindControl("uiTxtPosition");
        Label investor = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblInvestorID");
        Label commodity = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblCommodityID");
        Label contract = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblContractID");
        Label tradePosition = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblTradePosition");
        Label position = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblPosition");
        Label price = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblPrice");

        decimal priceOut = 0;
        decimal.TryParse(price.Text, out priceOut);
        dr.Price = priceOut;
        dr.TradePosition = tradePosition.Text;
        dr.Quantity = int.Parse(positionInput.Text);

        dtTenderRequest.AddTenderRequestRow(dr);

        scTenderID.Add(string.Format("{0}_{1}",
            tradePosition.Text, price.Text));

        //Reset position textbox
        positionInput.Text = "";

        BindViewStateToDataGrid();
    }

    private void FillContractPositionDataGrid()
    {
        uiDgContractPosition.DataSource = ObjectDataSourceTransferContractPosition;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTransferContractPosition.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgContractPosition.DataSource = dve;
        uiDgContractPosition.DataBind();
    }

    private bool IsValidEntry(string command)
    {
        bool isValid = true;
        uiBLError.Items.Clear();
        uiBLError.Visible = false;

        IEnumerable dv = (IEnumerable)ObjectDataSourceTender.Select();
        Object[] obj = (Object[])dv;
        TenderData.TenderRow drTender = (TenderData.TenderRow)obj[0];
        if (command == "approve")
        {
            if (drTender.ApprovalStatus == "A")
            {
                uiBLError.Items.Add("This record has been approved.");
            }
            if (drTender.ApprovalStatus == "R")
            {
                uiBLError.Items.Add("This record has been rejected.");
            }else if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }
        else if (command == "reject")
        {
            if (drTender.ApprovalStatus == "R")
            {
                uiBLError.Items.Add("This record has been rejected.");
            }
            if (drTender.ApprovalStatus == "A")
            {
                uiBLError.Items.Add("This record has been approved.");
            }
        }
        if (string.IsNullOrEmpty(CtlContractCommodityLookup1.LookupTextBoxID))
        {
            uiBLError.Items.Add("Contract is required.");
        }
        if (string.IsNullOrEmpty(CtlInvestorLookupSeller.LookupTextBoxID))
        {
            uiBLError.Items.Add("Investor seller is required.");
        }
        if (dtTenderRequest.Rows.Count == 0)
        {
            uiBLError.Items.Add("Tender request is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private bool IsValidAdd(int rowIndex)
    {
        bool isValid = true;
        uiBLError.Items.Clear();
        uiBLError.Visible = false;

        TextBox positionInput = (TextBox)uiDgContractPosition.Rows[rowIndex].FindControl("uiTxtPosition");
        if (!string.IsNullOrEmpty(positionInput.Text))
        {
            int positionResult;
            if (int.TryParse(positionInput.Text, out positionResult) == false)
            {
                uiBLError.Items.Add("Invalid numeric.");
            }
            else
            {
                Label investor = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblInvestorID");
                Label contract = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblContractID");
                Label tradePosition = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblTradePosition");
                Label position = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblPosition");
                Label price = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblPrice");
                if (scTenderID.Contains(string.Format("{0}_{1}",
                    tradePosition.Text, price.Text)))
                {
                    uiBLError.Items.Add("This record already exist.");
                }

                Label openPosition = (Label)uiDgContractPosition.Rows[rowIndex].FindControl("uiLblOpenPosition");
                if (positionResult > int.Parse(openPosition.Text))
                {
                    uiBLError.Items.Add("Position exceed open position.");
                }
            }
        }
        else
        {
            uiBLError.Items.Add("Input position is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void FillTenderRequestDataGrid()
    {
        uiDgTenderRequest.DataSource = ObjectDataSourceTenderRequest;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTenderRequest.Select();
        DataView dve = (DataView)dv;

        dtTenderRequest = (TenderData.TenderRequestDataTable)dve.Table;

        BindViewStateToDataGrid();
    }

    private void BindViewStateToDataGrid()
    {
        uiDgTenderRequest.DataSource = dtTenderRequest;
        uiDgTenderRequest.DataBind();
    }

    private void BindDataToForm()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceTender.Select();
        Object[] obj = (Object[])dv;
        TenderData.TenderRow drTender = (TenderData.TenderRow)obj[0];
        ContractDataTableAdapters.ContractCommodityTableAdapter taContract = 
            new ContractDataTableAdapters.ContractCommodityTableAdapter();
        ContractData.ContractCommodityDataTable dtContract = 
            new ContractData.ContractCommodityDataTable();
        taContract.FillByContractID(dtContract, drTender.ContractID);
        CtlContractCommodityLookup1.SetContractCommodityValue(drTender.ContractID.ToString(), dtContract[0].CommodityCode + ' ' + dtContract[0].ContractYear + ' ' + dtContract[0].ContractMonth);
        CtlInvestorLookupSeller.SetInvestorValue(drTender.SellerInvID.ToString(), Investor.GetNameInvestorByInvestorID(drTender.SellerInvID));
        TenderNo = drTender.TenderNo;
        if (!drTender.IsDeliveryLocationNull())
        {
            uiTxtDeliveryLocation.Text = drTender.DeliveryLocation;
        }
        if (!drTender.IsApprovalDescNull())
        {
            uiTxtApprovalDesc.Text = drTender.ApprovalDesc;
        }

        if (drTender.ApprovalStatus == "Y")
        {
            uiTxtApprovalDesc.Enabled = false;
        }
         
       
    }

    private void SetEnabledControls(bool b)
    {
        //CtlContractCommodityLookup1.DisabledLookupButton = !b;
        CtlInvestorLookupSeller.SetDisabledInvestor(!b);
        uiTxtDeliveryLocation.Enabled = b;

    }

    private void SetVisibleControls(bool b)
    {
        tblSearch.Visible = b;
        uiDgTenderRequest.Columns[0].Visible = b;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
    }
}
