using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_ShippingInstruction : System.Web.UI.Page
{
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

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    private void Search()
    {
        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

        try
        {
            DateTime bussDate = DateTime.Now;
            Nullable<decimal> sellerCMID = null;
            Nullable<decimal> contractID = null;

            if (CtlCalendarPickUp1.Text != "")
            {
                bussDate = DateTime.Parse(CtlCalendarPickUp1.Text);
            }

            dt = Tradefeed.FillShippingInstruction(bussDate);

            DataView dv = new DataView(dt);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgTradeFeed.DataSource = dv;
            uiDgTradeFeed.DataBind();
        }
        catch (Exception ex)
        {
            uiBlError.Visible = true;
            uiBlError.Items.Add(ex.Message);
        }
        finally
        {
            dt.Dispose();
        }
    }

    protected void uiDgTradeFeed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTradeFeed.PageIndex = e.NewPageIndex;
        Search();
    }

    protected void uiDgTradeFeed_Sorting(object sender, GridViewSortEventArgs e)
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

        Search();
    }

    protected void uiDgTradeFeed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label price = (Label)e.Row.FindControl("uiLblPrice");
            Label contractId = (Label)e.Row.FindControl("uiLblContractId");

            switch (Tradefeed.GetCurrencyCodeByContractID(decimal.Parse(contractId.Text)))
            {
                case "IDR":
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                case "USD":
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                default:
                    price.Text = price.Text;
                    break;
            }
        }
    }
}