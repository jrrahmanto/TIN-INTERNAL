using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_DetailWithdrawal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            Search();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    private void Search()
    {
        try
        {
            var dr = new BankTransfer.Rpt_WithdrawalDataTable();
            var dt = new BankTransferTableAdapters.Rpt_WithdrawalTableAdapter();
            dr = dt.GetDataByIdTf(Convert.ToInt32(currentID));

            DataView dv = new DataView(dr);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgBankAccount.DataSource = dv;
            uiDgBankAccount.DataBind();

            dr.Dispose();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
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
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    FillTransactionFeeDataGrid();
        //}
        //catch (Exception ex)
        //{
        //    uiBLError.Items.Add(ex.Message);
        //    uiBLError.Visible = true;
        //}
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            var dr = new BankTransferTableAdapters.Bank_TransferTableAdapter();
            var drw = new BankTransferTableAdapters.Rpt_WithdrawalTableAdapter();
            var drCM = new BankTransferTableAdapters.DataTable2TableAdapter();
            var dr_tg = new BankTransferTableAdapters.EODTradeProgressTableAdapter();
            var dr_validasi = new BankTransferTableAdapters.DataTable3TableAdapter();
            var dr_CP = new BankTransferTableAdapters.CMProfileTableAdapter();

            var dt = dr.GetDataById(Convert.ToInt32(currentID));
            var dt_tg = dr_tg.GetDataById(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID));
            var dtCM = drCM.GetDataId(dt_tg[0].SellerId);

            var dt_validasi = dr_validasi.GetData(Convert.ToInt32(currentID));
            var dt_validasi2 = dr.GetDataById(Convert.ToInt32(currentID));
            if ((dt_validasi[0].amount + Convert.ToDecimal(uiAmount.Text)) <= dt_validasi2[0].amount)
            {
                var dt_secfund = dr_tg.GetDataByExchangeReff(dt_tg[0].ExchangeRef, dt_tg[0].BuyerId, dt_tg[0].SellerId);
                var dt_validasi_secfun = dr_validasi.GetDataByExc(dt_tg[0].ExchangeRef, dt_tg[0].BuyerId, dt_tg[0].SellerId);
                var dt_CP = dr_CP.GetDataByCode(dtCM[0].CMCode);
                Decimal secfun = 0;
                //if (dt_secfund[0].Amount == dt_validasi_secfun[0].amount+ Convert.ToDecimal(uiAmount.Text))
                //{
                //    secfun = dt_tg[0].SecFundValueBond;
                //}
                if (cbSecfund.Checked)
                {
                    secfun = dt_tg[0].SecFundValueBond;
                }
                drw.Insert(dt[0].date, dtCM[0].CMCode, dtCM[0].seller, dt_CP[0].CMBankName, dt_CP[0].CMAccountNo, dt_tg[0].ExchangeRef, dt_tg[0].Amount, secfun, Convert.ToDecimal(uiAmount.Text), Convert.ToInt32(currentID), dt_tg[0].BusinessDate, 0, dt_tg[0].BuyerId, dt_tg[0].SellerId);
                Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            else
            {
                uiBLError.Items.Add("Total withdrawal melebihi jumlah transfer");
                uiBLError.Visible = true;
            }

        }
        catch (Exception x)
        {
            uiBLError.Items.Add(x.Message);
            uiBLError.Visible = true;
        }
    }
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Withdrawal.aspx");
    }
    protected void uiDgTransactionFee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgBankAccount.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void uiDgTransactionFee_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void uiDgBankAccount_Sorting(object sender, GridViewSortEventArgs e)
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

            Search();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    protected void uiDgWithdrawal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label amount = (Label)e.Row.FindControl("amount");
            amount.Text = (decimal.Parse(amount.Text)).ToString("#,##0.00");

            Label secfund = (Label)e.Row.FindControl("secfund");
            secfund.Text = (decimal.Parse(secfund.Text)).ToString("#,##0.00");

            Label transfer = (Label)e.Row.FindControl("transfer");
            transfer.Text = ((decimal.Parse(secfund.Text))+(decimal.Parse(amount.Text))).ToString("#,##0.00");
        }
    }

    protected void uiDgBankAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //try
        //{
        //    uiDgBankAccount.PageIndex = e.NewPageIndex;
        //    FillBankAccountDataGrid();
        //}
        //catch (Exception ex)
        //{
        //    uiBLError.Items.Add(ex.Message);
        //    uiBLError.Visible = true;
        //}
    }
}