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

public partial class Lookup_CtlSubCategory : System.Web.UI.UserControl
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
        get { return uiTxtLookupSubCategory.Text; }
        set { uiTxtLookupSubCategory.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDSubCategory.Value; }
        set { uiTxtLookupIDSubCategory.Value = value; }
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
        //Tools.RegisterGenericScript(this.Page);

        string script;
        script = string.Format("window.document.forms[0].{0}.value =''; " +
                            "window.document.forms[0].{1}.value =''; ",
                        uiTxtLookupSubCategory.ClientID,
                        uiTxtLookupIDSubCategory.ClientID);

        if (ExchangeMemberCodeLookupID != null)
        {
            script += string.Format("window.document.forms[0].{0}.value =''; ",
                        ExchangeMemberCodeLookupID);
        }

        script += string.Format("return HideModalPopupSubCategory{0}();",
                        uiBtnLookup_ModalPopupExtenderSubCategory.ClientID);

        uiTxtLookupIDSubCategory.Attributes.Add("readonly", "readonly");
        uiTxtLookupSubCategory.Attributes.Add("readonly", "readonly");
        uiBtnCancelSubCategory.Attributes.Add("onclick", script);

        //if (!string.IsNullOrEmpty(LookupTextBoxValue) ||
        //    !string.IsNullOrEmpty(LookupTextBoxIDValue))
        //{
        //    uiTxtLookupClearingMember.Text = LookupTextBoxValue;
        //    uiTxtLookupIDClearingMember.Value = LookupTextBoxIDValue;
        //}


        //if (DisableLookUpButton == true)
        //{
        //    uiBtnLookupClearingMember.Enabled = false;
        //}
        if (DisabledLookupButton != null)
        {
            SetDisabledSubCategory(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillSubCategoryDatagrid();
    }

    protected void uiDgSubCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgSubCategory.PageIndex = e.NewPageIndex;
        FillSubCategoryDatagrid();
    }

    protected void uiDgSubCategory_Sorting(object sender, GridViewSortEventArgs e)
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

        FillSubCategoryDatagrid();
    }

    protected void uiDgSubCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
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
                            "return HideModalPopupSubCategory{6}();",
                            uiTxtLookupSubCategory.ClientID, e.Row.Cells[2].Text,
                            uiTxtLookupIDSubCategory.ClientID, lblCMID.Text,
                            ExchangeMemberCodeLookupID, e.Row.Cells[2].Text,
                            uiBtnLookup_ModalPopupExtenderSubCategory.ClientID);
            }
            else
            {
                script = string.Format("window.document.forms[0].{0}.value = '{1}'; " +
                            "window.document.forms[0].{2}.value = '{3}'; " +
                            "return HideModalPopupSubCategory{4}();",
                            uiTxtLookupSubCategory.ClientID, e.Row.Cells[2].Text,
                            uiTxtLookupIDSubCategory.ClientID, lblCMID.Text,
                            uiBtnLookup_ModalPopupExtenderSubCategory.ClientID);
            }
            btn.Attributes.Add("onclick", script);
        }
    }

    public void SetDisabledSubCategory(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupSubCategory.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupIDClearingMember.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupClearingMember.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiBtnLookupClearingMember.ClientID, b.ToString().ToLower()));

        if (!Page.ClientScript.IsStartupScriptRegistered("SubCategoryDisabled" + uiTxtLookupIDSubCategory.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SubCategoryDisabled" + uiTxtLookupIDSubCategory.ClientID, sb.ToString(), true);
        }
    }

    public void SetSubCategoryValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDSubCategory.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupSubCategory.ClientID + ".value='" + value + "';");

        if (ExchangeMemberCodeLookupID != null)
        {
            //sb.AppendLine(string.Format("window.document.forms[0].{0}.value='{1}';", 
            //    ExchangeMemberLookupID, value));
            sb.AppendLine(string.Format("getElementById('{0}').value='{1}';",
                ExchangeMemberCodeLookupID, value));
        }

        //sb.AppendLine(string.Format("setValue{0}('{1}', '{2}');", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupIDClearingMember.ClientID, valueId));
        //sb.AppendLine(string.Format("setValue{0}('{1}', '{2}');", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupClearingMember.ClientID, value));

        if (!Page.ClientScript.IsStartupScriptRegistered("SubCategoryValue" + uiTxtLookupIDSubCategory.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SubCategoryValue" + uiTxtLookupIDSubCategory.ClientID, sb.ToString(), true);
        }
    }

    private void FillSubCategoryDatagrid()
    {
        uiDgSubCategory.DataSource = ObjectDataSourceSubCategory;
        IEnumerable dv = (IEnumerable)ObjectDataSourceSubCategory.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgSubCategory.DataSource = dv;
        uiDgSubCategory.DataBind();
    }
}
