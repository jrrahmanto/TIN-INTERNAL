using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data;
using System.Text;

public partial class Lookup_CtlExchangeMemberLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupExchangeMember.Text; }
        set { uiTxtLookupExchangeMember.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDExchangeMember.Value; }
        set { uiTxtLookupIDExchangeMember.Value = value; }
    }

    public string ExchangeCodeControlID
    {
        get { return uiTxtCMCode.ClientID; }
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
        if (!Page.IsPostBack)
        { 
          uiTxtLookupIDExchangeMember.Attributes.Add("readonly", "readonly");
                uiTxtLookupExchangeMember.Attributes.Add("readonly", "readonly");
                uiBtnCancelExchangeMember.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupExchangeMember.ClientID +
                    ".value ='';window.document.forms[0]." + uiTxtLookupIDExchangeMember.ClientID + ".value ='';return HideModalPopupExchangeMember" + uiBtnLookup_ModalPopupExtenderExchangeMember.ClientID + "();");


        }
      
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillExchangeMemberDatagrid();
    }

    protected void uiDgExchangeMember_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label uLblEMID = (Label)e.Row.FindControl("uiLblEMID");
            Label uiLblExchangeMemberCode = (Label)e.Row.FindControl("uiLblExchangeMemberCode");
            string script;

            script = string.Format("window.document.forms[0].{0}.value = '{1}'; " +
                "window.document.forms[0].{2}.value = '{3}'; " +
                "return HideModalPopupExchangeMember{4}();",
                uiTxtLookupIDExchangeMember.ClientID, uLblEMID.Text,
                uiTxtLookupExchangeMember.ClientID, uiLblExchangeMemberCode.Text,
                uiBtnLookup_ModalPopupExtenderExchangeMember.ClientID);

            btn.Attributes.Add("onclick", script);
        }
    }

    protected void uiDgExchangeMember_Sorting(object sender, GridViewSortEventArgs e)
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

        FillExchangeMemberDatagrid();
    }

    protected void uiDgExchangeMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgExchangeMember.PageIndex = e.NewPageIndex;
        FillExchangeMemberDatagrid();
    }

    public void SetDisabledExchangeMember(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDExchangeMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupExchangeMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupExchangeMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeMemberDisabled" + uiTxtLookupIDExchangeMember.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeMemberDisabled" + uiTxtLookupIDExchangeMember.ClientID, sb.ToString(), true);
        }
    }

    public void SetExchangeMemberValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDExchangeMember.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupExchangeMember.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("exchangeMemberValue" + uiTxtLookupIDExchangeMember.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "exchangeMemberValue" + uiTxtLookupIDExchangeMember.ClientID, sb.ToString(), true);
        }
    }

    private void FillExchangeMemberDatagrid()
    {
        uiDgExchangeMember.DataSource = ObjectDataSourceExchangeMember;
        IEnumerable dv = (IEnumerable)ObjectDataSourceExchangeMember.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgExchangeMember.DataSource = dv;
        uiDgExchangeMember.DataBind();
    }


}
