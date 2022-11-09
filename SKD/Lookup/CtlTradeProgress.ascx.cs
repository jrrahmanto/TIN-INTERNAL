using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Lookup_CltTradeRegister : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxtLookupIDCommodity.Attributes.Add("readonly", "readonly");
        uiTxtLookupCommodity.Attributes.Add("readonly", "readonly");
        uiBtnCancelCommodity.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupCommodity.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDCommodity.ClientID + ".value ='';return HideModalPopupCommodity" + uiBtnLookup_ModalPopupExtenderCommodity.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledCommodity(DisabledLookupButton.Value);
        }
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillCommodityDatagrid();
    }
    public string LookupTextBox
    {
        get { return uiTxtLookupCommodity.Text; }
        set { uiTxtLookupCommodity.Text = value; }
    }
    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDCommodity.Value; }
        set { uiTxtLookupIDCommodity.Value = value; }
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
    public Nullable<bool> DisabledLookupButton
    {
        get
        {
            if (ViewState["DisabledLookupButton"] != null)
            {
                return bool.Parse(ViewState["DisabledLookupButton"].ToString());
            }
            else
            {
                return null;
            }
        }
        set { ViewState["DisabledLookupButton"] = value; }
    }
    public void SetDisabledCommodity(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("commodityDisabled" + uiTxtLookupIDCommodity.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "commodityDisabled" + uiTxtLookupIDCommodity.ClientID, sb.ToString(), true);
        }
    }
    public void SetCommodityValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDCommodity.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupCommodity.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("commodityValue" + uiTxtLookupIDCommodity.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "commodityValue" + uiTxtLookupIDCommodity.ClientID, sb.ToString(), true);
        }
    }
    protected void uiDgCommodity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblCommodityID = (Label)e.Row.FindControl("uiLblCommodityID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupCommodity.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDCommodity.ClientID + ".value = '" + lblCommodityID.Text + "';return HideModalPopupCommodity" + uiBtnLookup_ModalPopupExtenderCommodity.ClientID + "();");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label amount = (Label)e.Row.FindControl("amount");
            amount.Text = (decimal.Parse(amount.Text)).ToString("#,##0.00");
        }
    }
    protected void uiDgCommodity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgCommodity.PageIndex = e.NewPageIndex;
        FillCommodityDatagrid();
    }
    private void FillCommodityDatagrid()
    {
        var dr = new BankTransfer.DataTable1DataTable();
        var dt = new BankTransferTableAdapters.DataTable1TableAdapter();
        dr = dt.GetData(uiSellerName.Text, uiExchangeRef.Text);

        DataView dv = new DataView(dr);

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dv.Sort = SortOrder;
        }

        uiDgCommodity.DataSource = dv;
        uiDgCommodity.DataBind();
    }
    protected void uiDgCommodity_Sorting(object sender, GridViewSortEventArgs e)
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

        FillCommodityDatagrid();
    }
}