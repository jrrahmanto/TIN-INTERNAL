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

public partial class Lookup_CtlBankAccountLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupBankAccount.Text; }
        set { uiTxtLookupBankAccount.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDBankAccount.Value; }
        set { uiTxtLookupIDBankAccount.Value = value; }
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
        uiTxtLookupIDBankAccount.Attributes.Add("readonly", "readonly");
        uiTxtLookupBankAccount.Attributes.Add("readonly", "readonly");
        uiBtnCancelBankAccount.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupBankAccount.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDBankAccount.ClientID + ".value ='';return HideModalPopupBankAccount" + uiBtnLookup_ModalPopupExtenderBankAccount.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledBankAccount(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillBankAccountDatagrid();
    }

    protected void uiDgBankAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgBankAccount.PageIndex = e.NewPageIndex;
        FillBankAccountDatagrid();
    }

    protected void uiDgBankAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Set button attributes when button is click
            HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
            string accType;


            if (btn != null)
            {
                if (e.Row.Cells[2].Text == "RD")
                {
                    accType = "Deposito";
                }
                else if (e.Row.Cells[2].Text == "RS")
                {
                    accType = "Settlement";
                }

                    Label lblBankAccount = (Label)e.Row.FindControl("uiLblBankAccountID");
                //btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupBankAccount.ClientID + ".value = '" + e.Row.Cells[3].Text + ' ' + e.Row.Cells[2].Text + ' ' + (e.Row.Cells[5].Text=="&nbsp;" ? "" : e.Row.Cells[5].Text) + ' ' + e.Row.Cells[7].Text +
                //    "';window.document.forms[0]." + uiTxtLookupIDBankAccount.ClientID + ".value = '" + lblBankAccount.Text + "';return HideModalPopupBankAccount" + uiBtnLookup_ModalPopupExtenderBankAccount.ClientID + "();");

                btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupBankAccount.ClientID + ".value = '" + e.Row.Cells[2].Text + '-' + e.Row.Cells[3].Text + '-' + e.Row.Cells[8].Text + '-' + e.Row.Cells[5].Text + '-' + (e.Row.Cells[6].Text == "&nbsp;" ? "" : e.Row.Cells[6].Text) +
                    "';window.document.forms[0]." + uiTxtLookupIDBankAccount.ClientID + ".value = '" + lblBankAccount.Text + "';return HideModalPopupBankAccount" + uiBtnLookup_ModalPopupExtenderBankAccount.ClientID + "();");

            }
        }        
    }

    protected void uiDgBankAccount_Sorting(object sender, GridViewSortEventArgs e)
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

        FillBankAccountDatagrid();
    }

    private void SetDisabledBankAccount(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupBankAccount.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("bankAccountDisabled" + uiTxtLookupIDBankAccount.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "bankAccountDisabled" + uiTxtLookupIDBankAccount.ClientID, sb.ToString(), true);
        }
    }

    public void SetBankAccountValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDBankAccount.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupBankAccount.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("bankAccountValue" + uiTxtLookupIDBankAccount.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "bankAccountValue" + uiTxtLookupIDBankAccount.ClientID, sb.ToString(), true);
        }
    }

    private void FillBankAccountDatagrid()
    {
        uiDgBankAccount.DataSource = odsBankAccount;
        IEnumerable dv = (IEnumerable)odsBankAccount.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort =  SortOrder;
        }

        uiDgBankAccount.DataSource = dv;
        uiDgBankAccount.DataBind();
    }
}
