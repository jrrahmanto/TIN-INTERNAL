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
using System.Text;

public partial class Lookup_CtlContractLookup : System.Web.UI.UserControl
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
        get { return uiTxtLookupContract.Text; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDContract.Value; }
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
        Tools.RegisterGenericScript(this.Page);

        uiTxtLookupIDContract.Attributes.Add("readonly", "readonly");
        uiTxtLookupContract.Attributes.Add("readonly", "readonly");
        uiBtnCancel.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupContract.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDContract.ClientID + ".value ='';return HideModalPopup"+ uiBtnLookup_ModalPopupExtenderContract.ClientID  +"();");

        
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillClearingMemberDatagrid();
    }

    private void FillClearingMemberDatagrid()
    {
        uiDgContract.DataSource = ObjectDataSourceContract;
        IEnumerable dv = (IEnumerable)ObjectDataSourceContract.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgContract.DataSource = dv;
        uiDgContract.DataBind();
    }

    protected void uiDgClearingMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgContract.PageIndex = e.NewPageIndex;
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
            Label uiLblContractID = (Label)e.Row.FindControl("uiLblContractID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupContract.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDContract.ClientID + ".value = '" + uiLblContractID.Text + "';return HideModalContractPopup" + uiBtnLookup_ModalPopupExtenderContract.ClientID + "();");
        }
    }

    public void SetDisabledContract(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDContract.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupContract.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine(string.Format("enable('{0}', {1});", uiBtnLookupContract.ClientID, b.ToString().ToLower()));
        sb.AppendLine(string.Format("enable('{0}', {1});", uiTxtCommodityCode.ClientID, b.ToString().ToLower()));

        if (!Page.ClientScript.IsStartupScriptRegistered("contractDisabled" + uiTxtLookupIDContract.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "contractDisabled" + uiTxtLookupIDContract.ClientID, sb.ToString(), true);
        }
    }

    public void SetContractValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Format("setValue('{0}', '{1}');", uiTxtLookupIDContract.ClientID, valueId));
        sb.AppendLine(string.Format("setValue('{0}', '{1}');", uiTxtLookupContract.ClientID, value));

        if (!Page.ClientScript.IsStartupScriptRegistered("contractValue" + uiTxtLookupIDContract.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "contractValue" + uiTxtLookupIDContract.ClientID, sb.ToString(), true);
        }
    }

}
