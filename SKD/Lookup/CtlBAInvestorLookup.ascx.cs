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
using AjaxControlToolkit;

public partial class Lookup_CtlInvestorLookup : System.Web.UI.UserControl
{
    private string _clearingMemberLookupID;
    public string ClearingMemberCodeLookupID
    {
        get
        {
            return _clearingMemberLookupID;
        }
        set
        {
            _clearingMemberLookupID = value;
        }
    }
    public string LookupTextBox
    {
        get { return uiTxtLookupInvestor.Text; }
        set { uiTxtLookupInvestor.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDInvestor.Value; }
        set { uiTxtLookupIDInvestor.Value = value; }
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
        trInvestorName.Visible = false;
        uiTxtLookupIDInvestor.Attributes.Add("readonly", "readonly");
        uiTxtLookupInvestor.Attributes.Add("readonly", "readonly");
        //uiTxtInvestorCode.Text = "899";//ClearingMemberCodeLookupID;
        //Session["CMCode"]= uiTxtInvestorCode.Text ; 
       // uiTxtInvestorCode.Text = (string)Session["CMCode"];
        uiBtnCancelInvestor.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupInvestor.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDInvestor.ClientID + ".value ='';return HideModalPopupInvestor" + uiBtnLookup_ModalPopupExtenderInvestor.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledInvestor(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillInvestorDatagrid();
    }

    private void FillInvestorDatagrid()
    {
        uiDgInvestor.DataSource = ObjectDataSourceInvestor;
        IEnumerable dv = (IEnumerable)ObjectDataSourceInvestor.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgInvestor.DataSource = dv;
        uiDgInvestor.DataBind();
    }

    public void SetDisabledInvestor(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDInvestor.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupInvestor.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupInvestor.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("investorDisabled" + uiTxtLookupIDInvestor.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "investorDisabled" + uiTxtLookupIDInvestor.ClientID, sb.ToString(), true);
        }
    }

    public void SetInvestorValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDInvestor.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupInvestor.ClientID + ".value='" + value + "';");
        
        if (!Page.ClientScript.IsStartupScriptRegistered("investorValue" + uiTxtLookupIDInvestor.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "investorValue" + uiTxtLookupIDInvestor.ClientID, sb.ToString(), true);
        }
    }

    protected void uiDgInvestor_Sorting(object sender, GridViewSortEventArgs e)
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

        FillInvestorDatagrid();
    }

    protected void uiDgInvestor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label investorId = (Label)e.Row.FindControl("uiLblInvestorId");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupInvestor.ClientID + ".value = '" + e.Row.Cells[5].Text +
                "';window.document.forms[0]." + uiTxtLookupIDInvestor.ClientID + ".value = '" + investorId.Text + "';return HideModalPopupInvestor" + uiBtnLookup_ModalPopupExtenderInvestor.ClientID + "();");
        }
    }

    protected void uiDgInvestor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgInvestor.PageIndex = e.NewPageIndex;
        FillInvestorDatagrid();
    }
}