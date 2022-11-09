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


public partial class Lookup_CTLMultiCLearingMemberLookup : System.Web.UI.UserControl
{
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
        //Tools.RegisterGenericScript(this.Page);

        uiTxtLookupIDClearingMember.Attributes.Add("readonly", "readonly");
        uiTxtLookupClearingMember.Attributes.Add("readonly", "readonly");
        //uiBtnCancelClearingMember.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupClearingMember.ClientID +
        //    ".value ='';window.document.forms[0]." + uiTxtLookupIDClearingMember.ClientID + ".value ='';return HideModalPopupClearingMember" + uiBtnLookup_ModalPopupExtenderClearingMember.ClientID + "();");
        uiBtnCancelClearingMember.Attributes.Add("onclick", "onLookupClosing();");
        
        //uiBtnSelect.Attributes.Add("onclick", "window.document.forms[0]." +
        //                     uiTxtLookupClearingMember.ClientID + ".value = '" + getCMIDText() +
        //                     "';window.document.forms[0]." + uiTxtLookupIDClearingMember.ClientID +
        //                     ".value = '" + getCMIDValue() + "';return HideModalPopupClearingMember" +
        //                     uiBtnLookup_ModalPopupExtenderClearingMember.ClientID + "();");
        //uiBtnSelect.Attributes.Add("onclick", "ManipulateGrid(" + uiTxtLookupClearingMember.ClientID.ToString() + "," + uiTxtLookupIDClearingMember.ClientID.ToString() + ");");
        uiBtnSelect.Attributes.Add("onclick", "ManipulateGrid();");
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
            SetDisabledClearingMember(DisabledLookupButton.Value);
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiTempIds.Value = "";
        uiTempNames.Value = "";
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
        //Set button attributes when button is click
        //HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        //if (btn != null)
        //{
        //    Label lblCMID = (Label)e.Row.FindControl("uiLblCMID");
        //    btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupClearingMember.ClientID + ".value = '" + e.Row.Cells[2].Text +
        //         "';window.document.forms[0]." + uiTxtLookupIDClearingMember.ClientID + ".value = '" + lblCMID.Text + "';return HideModalPopupClearingMember" + uiBtnLookup_ModalPopupExtenderClearingMember.ClientID + "();");
        //}
    }

    public void SetDisabledClearingMember(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupClearingMember.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupIDClearingMember.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupClearingMember.ClientID, b.ToString().ToLower()));
        //sb.AppendLine(string.Format("enable{0}('{1}', {2});", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiBtnLookupClearingMember.ClientID, b.ToString().ToLower()));

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

        //sb.AppendLine(string.Format("setValue{0}('{1}', '{2}');", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupIDClearingMember.ClientID, valueId));
        //sb.AppendLine(string.Format("setValue{0}('{1}', '{2}');", uiBtnLookup_ModalPopupExtenderClearingMember.ClientID, uiTxtLookupClearingMember.ClientID, value));

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

        foreach (GridViewRow row in uiDgClearingMember.Rows)
        {
            HtmlInputCheckBox uiChk = (HtmlInputCheckBox)row.FindControl("uiChbCMID");

            string cmid = dve[row.DataItemIndex]["CMID"].ToString();
            if (uiTempIds.Value == cmid ||
                uiTempIds.Value.StartsWith(cmid + ",") ||
                uiTempIds.Value.EndsWith("," + cmid) ||
                uiTempIds.Value.IndexOf("," + cmid + ",") >= 0)
            {
                uiChk.Checked = true;
            }
                
        }
    }

    private string getCMIDValue()
    {
        string cmidValue = "";
        string cmidText = "";
        foreach (GridViewRow dr in uiDgClearingMember.Rows)
        {
            
            HtmlInputCheckBox chkCmid = new HtmlInputCheckBox();
            chkCmid = ((HtmlInputCheckBox)dr.FindControl("uiChbCMID"));
            Label lblCMID = (Label)dr.FindControl("uiLblCMID");
            if (chkCmid.Checked)
            {
                if (cmidText != "")
                {
                    cmidText += "," + lblCMID.Text;
                }
                else
                {
                    cmidText += lblCMID.Text;
                }
                if (cmidValue != "")
                {
                    cmidValue += "," + dr.Cells[2].Text;
                }
                else
                {
                    cmidValue += dr.Cells[2].Text;
                }
            }
        }
        return cmidValue;
    }

    private string getCMIDText()
    {
        string cmidValue = "";
        string cmidText = "";
        foreach (GridViewRow dr in uiDgClearingMember.Rows)
        {
           
            HtmlInputCheckBox chkCmid = new HtmlInputCheckBox();
            chkCmid = ((HtmlInputCheckBox)dr.FindControl("uiChbCMID"));
            Label lblCMID = (Label)dr.FindControl("uiLblCMID");
            if (chkCmid.Checked)
            {
                if (cmidText != "")
                {
                    cmidText += "," + lblCMID.Text;
                }
                else
                {
                    cmidText += lblCMID.Text;
                }
                if (cmidValue != "")
                {
                    cmidValue += "," + dr.Cells[2].Text;
                }
                else
                {
                    cmidValue += dr.Cells[2].Text;
                }
            }
        }
        return cmidText;
    }
}
