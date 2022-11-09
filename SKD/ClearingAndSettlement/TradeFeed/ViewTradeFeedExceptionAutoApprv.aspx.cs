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
using System.Data;
using System.IO;

public partial class ClearingAndSettlement_TradeFeed_ViewTradeFeedExceptionAutoApprv : System.Web.UI.Page
{
    #region "   Member Variables   "
    // object handler
    Exchange _exchangeHandler = new Exchange();
    TradefeedException _tfExceptionHandler = new TradefeedException();
    Tradefeed _tfRawHandler = new Tradefeed();
    #endregion

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

    #region "   Use Case: Summary View   "

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        if (!Page.IsPostBack)
        {
            // load exchange
            ExchangeData.ExchangeDataTable dtExchange = _exchangeHandler.GetExchanges();
            foreach (ExchangeData.ExchangeRow item in dtExchange)
            {
                ddlExchange.Items.Add(new ListItem(item.ExchangeName, item.ExchangeId.ToString()));
            }

            // hide button Approve All
            btnApproveAll.Visible = false;
            uiLblApprovalDesc.Visible = false;
            uiTxtApprovalDesc.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // display data based on specified criteria
        if (IsValidSearch())
        {
            FillTradeFeedExceptionDataGrid();
        }

        //TradefeedData.TradefeedExceptionDataTable dtException =
        //    _tfExceptionHandler.GetData(decimal.Parse(ddlExchange.SelectedValue),
        //                       DateTime.Parse(CtlCalendarPickUp1.Text),
        //                       ddlApprovalStatus.SelectedValue);

        //Session["SelectedExchangeID"] = ddlExchange.SelectedValue;
        //Session["SelectedBusinessDate"] = DateTime.Parse(CtlCalendarPickUp1.Text);

        //grvException.DataSource = dtException;
        //grvException.DataBind();
    }
    #endregion

    

    protected void uiDgTradeFeedException_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTradeFeedException.PageIndex = e.NewPageIndex;
        FillTradeFeedExceptionDataGrid();
    }

    protected void uiDgTradeFeedException_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            approvalStatus.Text = Common.GetApprovalDescription(approvalStatus.Text);

            Label exchange = (Label)e.Row.FindControl("uiLblExchangeId");
            exchange.Text = Exchange.GetExchangeNameByExchangeId(decimal.Parse(exchange.Text));
        }
    }

    protected void uiDgTradeFeedException_Sorting(object sender, GridViewSortEventArgs e)
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

        FillTradeFeedExceptionDataGrid();
    }

    private bool IsValidSearch()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
        {
            uiBLError.Items.Add("Business date is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void FillTradeFeedExceptionDataGrid()
    {
        uiDgTradeFeedException.DataSource = ObjectDataSourceTradeFeedException;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTradeFeedException.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgTradeFeedException.DataSource = dve;
        uiDgTradeFeedException.DataBind();

        btnApproveAll.Visible = (dve.Count > 0) ? true : false ;
        uiLblApprovalDesc.Visible = (dve.Count > 0) ? true : false;
        uiTxtApprovalDesc.Visible = (dve.Count > 0) ? true : false;
    }

    protected void btnApproveAll_Click(object sender, EventArgs e)
    {
        int? exchangeId = null;
        if (ddlExchange.SelectedValue.ToString().Trim() != "") exchangeId = int.Parse(ddlExchange.SelectedValue.ToString());
        DateTime? bussDate = null;
        bussDate = DateTime.Parse(CtlCalendarPickUp1.Text);

        TradefeedException.ApproveAll(exchangeId, bussDate, uiTxtApprovalDesc.Text, User.Identity.Name);
        FillTradeFeedExceptionDataGrid();
    }
}
