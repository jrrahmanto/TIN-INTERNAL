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

public partial class Lookup_CtlContractCommodityLookup : System.Web.UI.UserControl
{
    public string LookupTextBox
    {
        get { return uiTxtLookupContractCommodity.Text; }
        set { uiTxtLookupContractCommodity.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDContractCommodity.Value; }
        set { uiTxtLookupIDContractCommodity.Value = value; }
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
        Tools.RegisterGenericScript(this.Page);

        uiTxtLookupIDContractCommodity.Attributes.Add("readonly", "readonly");
        uiTxtLookupContractCommodity.Attributes.Add("readonly", "readonly");
        uiBtnCancel.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupContractCommodity.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDContractCommodity.ClientID + ".value ='';return HideModalPopupContractCommodity" + uiBtnLookup_ModalPopupExtenderContractCommodity.ClientID + "();");

        if (DisabledLookupButton != null)
        {
            SetDisabledContractCommodity(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillContractCommodityDatagrid();
    }

    public void SetDisabledContractCommodity(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDContractCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupContractCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupContractCommodity.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("contractCommodityDisabled" + uiTxtLookupIDContractCommodity.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "contractCommodityDisabled" + uiTxtLookupIDContractCommodity.ClientID, sb.ToString(), true);
        }
    }

    public void SetContractCommodityValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDContractCommodity.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupContractCommodity.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("contractValue" + uiTxtLookupIDContractCommodity.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "contractValue" + uiTxtLookupIDContractCommodity.ClientID, sb.ToString(), true);
        }
    }

    private void FillContractCommodityDatagrid()
    {
        uiDgContractCommodity.DataSource = ObjectDataSourceContract;
        IEnumerable dv = (IEnumerable)ObjectDataSourceContract.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgContractCommodity.DataSource = dv;
        uiDgContractCommodity.DataBind();
    }

    protected void uiDgContractCommodity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgContractCommodity.PageIndex = e.NewPageIndex;
        FillContractCommodityDatagrid();
    }

    protected void uiDgContractCommodity_Sorting(object sender, GridViewSortEventArgs e)
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

        FillContractCommodityDatagrid();
    }

    protected void uiDgContractCommodity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label uiLblContractCommodityID = (Label)e.Row.FindControl("uiLblContractCommodityID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupContractCommodity.ClientID + ".value = '" + e.Row.Cells[3].Text + " " + e.Row.Cells[5].Text + e.Row.Cells[6].Text +
                "';window.document.forms[0]." + uiTxtLookupIDContractCommodity.ClientID + ".value = '" + uiLblContractCommodityID.Text + "';return HideModalPopupContractCommodity" + uiBtnLookup_ModalPopupExtenderContractCommodity.ClientID + "();");
        }
    }
}