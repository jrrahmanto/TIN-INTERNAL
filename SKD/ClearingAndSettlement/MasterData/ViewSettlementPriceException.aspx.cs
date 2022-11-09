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

public partial class ClearingAndSettlement_MasterData_ViewSettlementPriceException : System.Web.UI.Page
{
    #region "   Member Variables   "
    // object handler
    Exchange _exchangeHandler = new Exchange();
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
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (IsValidSearch())
        {
            FillSettlementPriceExceptionDataGrid();
        }
    }

    protected void uiDgSettlementPriceException_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgSettlementPriceException.PageIndex = e.NewPageIndex;
        FillSettlementPriceExceptionDataGrid();
    }

    protected void uiDgSettlementPriceException_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            approvalStatus.Text = Common.GetApprovalDescription(approvalStatus.Text);

            Label exchange = (Label)e.Row.FindControl("uiLblExchangeId");
            exchange.Text = Exchange.GetExchangeNameByExchangeId(decimal.Parse(exchange.Text));
        }
    }

    protected void uiDgSettlementPriceException_Sorting(object sender, GridViewSortEventArgs e)
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

        FillSettlementPriceExceptionDataGrid();
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

    private void FillSettlementPriceExceptionDataGrid()
    {
        uiDgSettlementPriceException.DataSource = ObjectDataSourceSettlementPriceException;
        IEnumerable dv = (IEnumerable)ObjectDataSourceSettlementPriceException.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgSettlementPriceException.DataSource = dve;
        uiDgSettlementPriceException.DataBind();
    }
}
