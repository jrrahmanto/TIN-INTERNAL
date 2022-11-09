using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ClearingAndSettlement_TradeFeed_ViewTradeFeed : System.Web.UI.Page
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
        uiDdlExchange.Focus();
        uiBlError.Items.Clear();
        uiBlError.Visible = false;
    }

    protected void uiBtnImport_Click(object sender, EventArgs e)
    {
        Response.Redirect("ImportTradeFeed.aspx");
    }

    protected void uiDgTradeFeed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label price = (Label)e.Row.FindControl("uiLblPrice");
            Label contractId = (Label)e.Row.FindControl("uiLblContractId");

            switch (Tradefeed.GetCurrencyCodeByContractID(decimal.Parse(contractId.Text)))
            { 
                case "IDR" :
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                case "USD" :
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                default :
                    price.Text = price.Text;
                    break;
            }

            //Label sellerCm = (Label)e.Row.FindControl("uiLblSellerCmCode");
            //Label sellerId = (Label)e.Row.FindControl("uiLblSellerCmId");
            //sellerCm.Text = ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(sellerId.Text));

            //Label buyerCm = (Label)e.Row.FindControl("uiLblBuyerCmCode");
            //Label buyerId = (Label)e.Row.FindControl("uiLblBuyerCmId");
            //buyerCm.Text = ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(buyerCm.Text));
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

    private void Search()
    {
        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

        try
        {
            Nullable<DateTime> bussDate = null;
            Nullable<decimal> sellerCMID = null;
            Nullable<decimal> buyerCMID = null;
            Nullable<decimal> tradefeedID = null;
            Nullable<decimal> contractID = null;

            if (CtlCalendarPickUp1.Text != "")
            {
                bussDate = DateTime.Parse(CtlCalendarPickUp1.Text);
            }
            if (CtlClearingMemberLookup1.LookupTextBoxID != "")
            {
                sellerCMID = decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID);
            }
            if (!string.IsNullOrEmpty(uiTxbTradeFeedID.Text))
            {
                decimal tradeFeedIdOut;
                if (decimal.TryParse(uiTxbTradeFeedID.Text, out tradeFeedIdOut) == false)
                {
                    throw new ApplicationException("Invalid numeric.");
                }
                else
                {
                    tradefeedID = tradeFeedIdOut;
                }

                
            }
            if (CtlContractLookup1.LookupTextBoxID != "")
            {
                contractID = decimal.Parse(CtlContractLookup1.LookupTextBoxID);
            }

            dt = Tradefeed.FillBySearchCrit(decimal.Parse(uiDdlExchange.SelectedValue),
                                                bussDate,
                                                sellerCMID,
                                                tradefeedID,
                                                contractID);

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
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }


    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryTradeFeed.aspx?eType=add");
    }
}
