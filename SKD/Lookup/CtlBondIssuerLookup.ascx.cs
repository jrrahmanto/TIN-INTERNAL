using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class Lookup_CtlBondIssuerLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupIssuer.Text; }
        set { uiTxtLookupIssuer.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDIssuer.Value; }
        set { uiTxtLookupIDIssuer.Value = value; }
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
    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxtLookupIDIssuer.Attributes.Add("readonly", "readonly");
        uiTxtLookupIssuer.Attributes.Add("readonly", "readonly");
        uiBtnCancelExchange.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupIssuer.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDIssuer.ClientID + ".value ='';return HideModalPopupExchange" + uiBtnLookup_ModalPopupExtenderExchange.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledExchange(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillExchangeDatagrid();
    }

    protected void uiDgExchange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblIssuer = (Label)e.Row.FindControl("uiLblBondID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupIssuer.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDIssuer.ClientID + ".value = '" + lblIssuer.Text + "';return HideModalPopupExchange" + uiBtnLookup_ModalPopupExtenderExchange.ClientID + "();");
        }
    }

    protected void uiDgExchange_Sorting(object sender, GridViewSortEventArgs e)
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

        FillExchangeDatagrid();
    }

    protected void uiDgExchange_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgExchange.PageIndex = e.NewPageIndex;
        FillExchangeDatagrid();
    }

    public void SetDisabledExchange(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDIssuer.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIssuer.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupExchange.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeDisabled" + uiTxtLookupIDIssuer.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeDisabled" + uiTxtLookupIDIssuer.ClientID, sb.ToString(), true);
        }
    }

    public void SetExchangeValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDIssuer.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIssuer.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeValue" + uiTxtLookupIDIssuer.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeValue" + uiTxtLookupIDIssuer.ClientID, sb.ToString(), true);
        }
    }

    private void FillExchangeDatagrid()
    {
        uiDgExchange.DataSource = ObjectDataSourceExchange;
        IEnumerable dv = (IEnumerable)ObjectDataSourceExchange.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgExchange.DataSource = dv;
        uiDgExchange.DataBind();
    }
}
