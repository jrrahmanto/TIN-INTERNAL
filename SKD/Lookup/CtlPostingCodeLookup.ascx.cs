using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class Lookup_CtlPostingCodeLookup : System.Web.UI.UserControl
{
    //private string _lookupTextBoxValue;
    //public string LookupTextBoxValue
    //{
    //    get { return _lookupTextBoxValue; }
    //    set { _lookupTextBoxValue = value; }
    //}

    //private string _lookupTextBoxIDValue;
    //public string LookupTextBoxIDValue
    //{
    //    get { return _lookupTextBoxIDValue; }
    //    set { _lookupTextBoxIDValue = value; }
    //}

    //public string LookupTextBoxValue
    //{
    //    get
    //    {
    //        if (Session["PostingCodeLookup"] != null)
    //        {
    //            return Session["PostingCodeLookup"].ToString();
    //        }
    //        else
    //        {
    //            return "";
    //        }
    //    }
    //}

    //public string LookupTextBoxIDValue
    //{
    //    get
    //    {
    //        if (Session["PostingCodeIDLookup"] != null)
    //        {
    //            return Session["PostingCodeIDLookup"].ToString();
    //        }
    //        else
    //        {
    //            return "";
    //        }
    //    }
    //}

    public string LookupTextBox
    {
        get { return uiTxtLookupPostingCode.Text; }
        set { uiTxtLookupPostingCode.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDPostingCode.Value; }
        set { uiTxtLookupIDPostingCode.Value = value; }
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
        uiTxtLookupIDPostingCode.Attributes.Add("readonly", "readonly");
        uiTxtLookupPostingCode.Attributes.Add("readonly", "readonly");
        uiBtnCancelPostingCode.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupPostingCode.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDPostingCode.ClientID + ".value ='';return HideModalPopupPostingCode" + uiBtnLookup_ModalPopupExtenderPostingCode.ClientID + "();");

        //if (!string.IsNullOrEmpty(LookupTextBoxValue) ||
        //    !string.IsNullOrEmpty(LookupTextBoxIDValue))
        //{
        //    uiTxtLookupPostingCode.Text = LookupTextBoxValue;
        //    uiTxtLookupIDPostingCode.Value = LookupTextBoxIDValue;
        //}


        //if (DisableLookUpButton == true)
        //{
        //    uiBtnLookupPostingCode.Enabled = false;
        //}        
    }
    
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillPostingCodeDataGrid();
    }

    protected void uiDgPostingCode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblAccountID = (Label)e.Row.FindControl("uiLblAccountID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupPostingCode.ClientID + ".value = '" + e.Row.Cells[5].Text +
                "';window.document.forms[0]." + uiTxtLookupIDPostingCode.ClientID + ".value = '" + lblAccountID.Text + "';return HideModalPopupPostingCode" + uiBtnLookup_ModalPopupExtenderPostingCode.ClientID + "();");
        }
    }

    protected void uiDgPostingCode_Sorting(object sender, GridViewSortEventArgs e)
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

        FillPostingCodeDataGrid();
    }

    protected void uiDgPostingCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgPostingCode.PageIndex = e.NewPageIndex;
        FillPostingCodeDataGrid();
    }

    public void SetDisabledPostingCode(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDPostingCode.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupPostingCode.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("clearingMemberDisabled" + uiTxtLookupIDPostingCode.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clearingMemberDisabled" + uiTxtLookupIDPostingCode.ClientID, sb.ToString(), true);
        }
    }

    public void SetPostingCodeValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDPostingCode.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupPostingCode.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("clearingMemberValue" + uiTxtLookupIDPostingCode.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "calendarValue" + uiTxtLookupIDPostingCode.ClientID, sb.ToString(), true);
        }
    }

    private void FillPostingCodeDataGrid()
    {
        uiDgPostingCode.DataSource = ObjectDataSourcePostingCode;
        IEnumerable dv = (IEnumerable)ObjectDataSourcePostingCode.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgPostingCode.DataSource = dv;
        uiDgPostingCode.DataBind();
    }
}
