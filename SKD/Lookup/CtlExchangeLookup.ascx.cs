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

public partial class Lookup_CtlExchangeLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupExchange.Text; }
        set { uiTxtLookupExchange.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDExchange.Value; }
        set { uiTxtLookupIDExchange.Value = value; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxtLookupIDExchange.Attributes.Add("readonly", "readonly");
        uiTxtLookupExchange.Attributes.Add("readonly", "readonly");
        uiBtnCancelExchange.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupExchange.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDExchange.ClientID + ".value ='';return HideModalPopupExchange" + uiBtnLookup_ModalPopupExtenderExchange.ClientID + "();");

        
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
            Label lblExchange = (Label)e.Row.FindControl("uiLblExchangeID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupExchange.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDExchange.ClientID + ".value = '" + lblExchange.Text + "';return HideModalPopupExchange" + uiBtnLookup_ModalPopupExtenderExchange.ClientID + "();");
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
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDExchange.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupExchange.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupExchange.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeDisabled" + uiTxtLookupIDExchange.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeDisabled" + uiTxtLookupIDExchange.ClientID, sb.ToString(), true);
        }
    }

    public void SetExchangeValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDExchange.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupExchange.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeValue" + uiTxtLookupIDExchange.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeValue" + uiTxtLookupIDExchange.ClientID, sb.ToString(), true);
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
