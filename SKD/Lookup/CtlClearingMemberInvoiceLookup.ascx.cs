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

public partial class Lookup_CtlClearingMemberInvoiceLookup : System.Web.UI.UserControl
{
    private string _exchangeMemberLookupID;
    public string ExchangeMemberCodeLookupID
    {
        get
        {
            return _exchangeMemberLookupID;
        }
        set
        {
            _exchangeMemberLookupID = value;
        }
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

    public string LookupTextBox
    {
        get { return uiTxtLookupClearingMember.Text; }
        set { uiTxtLookupClearingMember.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDClearingMember.Value; }
        set { uiTxtLookupIDClearingMember.Value = value; }
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
        string script;
        script = string.Format("window.document.forms[0].{0}.value =''; " + 
                            "window.document.forms[0].{1}.value =''; ",
                        uiTxtLookupClearingMember.ClientID, 
                        uiTxtLookupIDClearingMember.ClientID);

        if (ExchangeMemberCodeLookupID != null)
        {
            script += string.Format("window.document.forms[0].{0}.value =''; ", 
                        ExchangeMemberCodeLookupID);
        }

        script += string.Format("return HideModalPopupClearingMember{0}();",
                        uiBtnLookup_ModalPopupExtenderClearingMember.ClientID);

        uiTxtLookupIDClearingMember.Attributes.Add("readonly", "readonly");
        uiTxtLookupClearingMember.Attributes.Add("readonly", "readonly");
        uiBtnCancelClearingMember.Attributes.Add("onclick", script);

        if (DisabledLookupButton != null)
        {
            SetDisabledClearingMember(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillClearingMemberDatagrid();
    }

    protected void uiDgClearingMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgClearingMember.PageIndex = e.NewPageIndex;
        FillClearingMemberDatagrid();
    }

    protected void uiDgClearingMember_Sorting(object sender, GridViewSortEventArgs e)
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

        FillClearingMemberDatagrid();
    }

    protected void uiDgClearingMember_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblCMID = (Label)e.Row.FindControl("uiLblCMID");
            string script;
            if (ExchangeMemberCodeLookupID != null)
            {
                script = string.Format("window.document.forms[0].{0}.value = '{1}'; " +
                            "window.document.forms[0].{2}.value = '{3}'; " +
                            "window.document.forms[0].{4}.value = '{5}'; " +
                            "window.document.forms[0].{4}.readOnly = 'true'; " +
                            "return HideModalPopupClearingMember{6}();",
                            uiTxtLookupClearingMember.ClientID, e.Row.Cells[2].Text,
                            uiTxtLookupIDClearingMember.ClientID, lblCMID.Text,
                            ExchangeMemberCodeLookupID, e.Row.Cells[2].Text,
                            uiBtnLookup_ModalPopupExtenderClearingMember.ClientID);

            }
            else
            {
                
                script = string.Format("window.document.forms[0].{0}.value = '{1}'; " + 
                            "window.document.forms[0].{2}.value = '{3}'; " + 
                            "return HideModalPopupClearingMember{4}();",
                            uiTxtLookupClearingMember.ClientID, e.Row.Cells[2].Text,
                            uiTxtLookupIDClearingMember.ClientID, lblCMID.Text,
                            uiBtnLookup_ModalPopupExtenderClearingMember.ClientID);
            }

            Session["CMCode"] = e.Row.Cells[2].Text;
            btn.Attributes.Add("onclick", script);
            Session["CMID"] = lblCMID.Text;  
        }
    }

    public void SetDisabledClearingMember(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("clearingMemberDisabled" + uiTxtLookupIDClearingMember.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clearingMemberDisabled" + uiTxtLookupIDClearingMember.ClientID, sb.ToString(), true);
        }
    }

    public void SetClearingMemberValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDClearingMember.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupClearingMember.ClientID + ".value='" + value + "';");

        if (ExchangeMemberCodeLookupID != null)
        {
            sb.AppendLine(string.Format("getElementById('{0}').value='{1}';",
                ExchangeMemberCodeLookupID, value));
        }

        if (!Page.ClientScript.IsStartupScriptRegistered("clearingMemberValue" + uiTxtLookupIDClearingMember.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clearingMemberValue" + uiTxtLookupIDClearingMember.ClientID, sb.ToString(), true);
        }
    }

    private void FillClearingMemberDatagrid()
    {
        uiDgClearingMember.DataSource = ObjectDataSourceClearingMember;
        IEnumerable dv = (IEnumerable)ObjectDataSourceClearingMember.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgClearingMember.DataSource = dv;
        uiDgClearingMember.DataBind();
    }
}
