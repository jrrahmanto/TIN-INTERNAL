using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;
using System.Text;

public partial class Lookup_CtlBankLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupBank.Text; }
        set { uiTxtLookupBank.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDBank.Value; }
        set { uiTxtLookupIDBank.Value = value; }
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
        uiTxtLookupIDBank.Attributes.Add("readonly", "readonly");
        uiTxtLookupBank.Attributes.Add("readonly", "readonly");
        uiBtnCancelBank.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupBank.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDBank.ClientID + ".value ='';return HideModalPopupBank" + uiBtnLookup_ModalPopupExtenderBank.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledBank(DisabledLookupButton.Value);
        }
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillBankDatagrid();
    }
    protected void uiDgBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgBank.PageIndex = e.NewPageIndex;
        FillBankDatagrid();
    }
    protected void uiDgBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblBank = (Label)e.Row.FindControl("uiLblBankID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupBank.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDBank.ClientID + ".value = '" + lblBank.Text + "';return HideModalPopupBank" + uiBtnLookup_ModalPopupExtenderBank.ClientID + "();");
        }       
    }
    protected void uiDgBank_Sorting(object sender, GridViewSortEventArgs e)
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

        FillBankDatagrid();
    }

    public void SetDisabledBank(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupBank.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("bankDisabled" + uiTxtLookupIDBank.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "bankDisabled" + uiTxtLookupIDBank.ClientID, sb.ToString(), true);
        }
    }

    public void SetBankValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDBank.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupBank.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("bankValue" + uiTxtLookupIDBank.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "bankValue" + uiTxtLookupIDBank.ClientID, sb.ToString(), true);
        }
    }

    private void FillBankDatagrid()
    {
        uiDgBank.DataSource = ObjectDataSourceBank;
        IEnumerable dv = (IEnumerable)ObjectDataSourceBank.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgBank.DataSource = dv;
        uiDgBank.DataBind();
    }
}
