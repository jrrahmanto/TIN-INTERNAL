using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;
using AjaxControlToolkit;

public partial class Lookup_CtlCMExchangeLookup : System.Web.UI.UserControl
{
    private string _lookupTextBoxValue;
    public string LookupTextBoxValue
    {
        get { return _lookupTextBoxValue; }
        set { _lookupTextBoxValue = value; }
    }

    private string _lookupTextBoxIDValue;
    public string LookupTextBoxIDValue
    {
        get { return _lookupTextBoxIDValue; }
        set { _lookupTextBoxIDValue = value; }
    }

    public string LookupTextBox
    {
        get { return uiTxtLookupCMExchange.Text; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDCMExchange.Value; }
    }

    private bool _disableLookUpButton = false;
    public bool DisableLookUpButton
    {
        get { return _disableLookUpButton; }
        set { _disableLookUpButton = value; }
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
        uiTxtLookupIDCMExchange.Attributes.Add("readonly", "readonly");
        uiTxtLookupCMExchange.Attributes.Add("readonly", "readonly");
        uiBtnCancel.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupCMExchange.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDCMExchange.ClientID + ".value ='';return HideModalCMExchangePopup" + uiBtnLookup_ModalPopupExtenderCMExchange.ClientID + "();");

        if (!string.IsNullOrEmpty(LookupTextBoxValue) ||
            !string.IsNullOrEmpty(LookupTextBoxIDValue))
        {
            uiTxtLookupCMExchange.Text = LookupTextBoxValue;
            uiTxtLookupIDCMExchange.Value = LookupTextBoxIDValue;
        }


        if (DisableLookUpButton == true)
        {
            uiBtnLookupCMExchange.Enabled = false;
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillClearingMemberDatagrid();
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
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblCMExchange = (Label)e.Row.FindControl("uiLblCMExchange");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupCMExchange.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDCMExchange.ClientID + ".value = '" + lblCMExchange.Text + "';return HideModalCMExchangePopup" + uiBtnLookup_ModalPopupExtenderCMExchange.ClientID + "();");
        }
    }
}
