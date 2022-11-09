using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Collections;

public partial class Lookup_CtlProductLookup : System.Web.UI.UserControl
{

    #region "   Property   "

    public string LookupTextBox
    {
        get { return uiTxtLookupProduct.Text; }
        set { uiTxtLookupProduct.Text = value; }
    }

    public string LookupTextBoxID
    {
        get { return uiTxtLookupIDProduct.Value; }
        set { uiTxtLookupIDProduct.Value = value; }
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

    #endregion

    #region "   Use Case   "

    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxtLookupIDProduct.Attributes.Add("readonly", "readonly");
        uiTxtLookupProduct.Attributes.Add("readonly", "readonly");
        uiBtnCancelProduct.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupProduct.ClientID +
            ".value ='';window.document.forms[0]." + uiTxtLookupIDProduct.ClientID + ".value ='';return HideModalPopupProduct" + uiBtnLookup_ModalPopupExtenderProduct.ClientID + "();");
                
    }    

    protected void uiDgProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgProduct.PageIndex = e.NewPageIndex;
        FillProductDatagrid();
    }
        
    protected void uiDgProduct_Sorting(object sender, GridViewSortEventArgs e)
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

        FillProductDatagrid();
    }

    protected void uiDgProduct_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            Label lblProductID = (Label)e.Row.FindControl("uiLblProductID");
            btn.Attributes.Add("onclick", "window.document.forms[0]." + uiTxtLookupProduct.ClientID + ".value = '" + e.Row.Cells[2].Text +
                "';window.document.forms[0]." + uiTxtLookupIDProduct.ClientID + ".value = '" + lblProductID.Text + "';return HideModalPopupProduct" + uiBtnLookup_ModalPopupExtenderProduct.ClientID + "();");
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillProductDatagrid();
    }
    
    #endregion

    #region "   Supporting Method   "

    public void SetDisabledProduct(bool b)
    {
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupIDProduct.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiTxtLookupProduct.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        //sb.AppendLine("window.document.forms[0]." +
        //    uiBtnCancelProduct.ClientID + ".disabled=" + b.ToString().ToLower() + ";");
        sb.AppendLine("window.document.forms[0]." +
            uiBtnLookupProduct.ClientID + ".disabled=" + b.ToString().ToLower() + ";");

        if (!Page.ClientScript.IsStartupScriptRegistered("productDisabled" + uiTxtLookupIDProduct.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "productDisabled" + uiTxtLookupIDProduct.ClientID, sb.ToString(), true);
        }
    }

    public void SetProductValue(string valueId, string value)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupIDProduct.ClientID + ".value='" + valueId + "';");
        sb.AppendLine("window.document.forms[0]." +
            uiTxtLookupProduct.ClientID + ".value='" + value + "';");

        if (!Page.ClientScript.IsStartupScriptRegistered("productValue" + uiTxtLookupIDProduct.ClientID))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "productValue" + uiTxtLookupIDProduct.ClientID, sb.ToString(), true);
        }
    }

    private void FillProductDatagrid()
    {
        //uiDgProduct.DataSource = ObjectDataSourceProduct;
        IEnumerable dv = (IEnumerable)ObjectDataSourceProduct.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgProduct.DataSource = dv;
        uiDgProduct.DataBind();
    }

    #endregion

}
