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
using AjaxControlToolkit;

public partial class Lookup_CtlUserMemberLookup : System.Web.UI.UserControl
{
  
    public string LookupTextBox
    {
        get { return uiTxtLookupUser.Text; }
        set { uiTxtLookupUser.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDUser.Value; }
        set { uiTxtLookupIDUser.Value = value; }
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
        uiTxtLookupIDUser.Attributes.Add("readonly", "readonly");
        uiTxtLookupUser.Attributes.Add("readonly", "readonly");
        uiBtnCancelUser.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupUser.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDUser.ClientID + ".value ='';return HideModalPopupUser" + uiBtnLookup_ModalPopupExtenderUser.ClientID + "();");

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillUserDatagrid();
    }

    private void FillUserDatagrid()
    {
        uiDgUser.DataSource = ObjectDataSourceUser;
        IEnumerable dv = (IEnumerable)ObjectDataSourceUser.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgUser.DataSource = dv;
        uiDgUser.DataBind();
    }

    protected void uiDgUser_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
    {
        uiDgUser.PageIndex = e.NewPageIndex;
        FillUserDatagrid();
    }

    protected void uiDgUser_Sorting(object sender, GridViewSortEventArgs e)
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

        FillUserDatagrid();
    }

    public void SetDisabledExchangeMember(bool b)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDUser.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupUser.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupUser.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("userDisabled" + uiTxtLookupIDUser.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "userDisabled" + uiTxtLookupIDUser.ClientID, sb.ToString(), true);
        }
    }

    public void SetClearingMemberValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDUser.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupUser.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("userValue" + uiTxtLookupIDUser.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "userValue" + uiTxtLookupIDUser.ClientID, sb.ToString(), true);
        }
    }


    protected void uiDgUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblUserID = (Label)e.Row.FindControl("uiLblUserID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupUser.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDUser.ClientID + ".value = '" + lblUserID.Text + "';return HideModalPopupUser" + uiBtnLookup_ModalPopupExtenderUser.ClientID + "();");
        }
    }

    protected void uiDgUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgUser.PageIndex = e.NewPageIndex;
        FillUserDatagrid();
    }
}

