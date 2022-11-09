using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections;

public partial class Lookup_ClearingMemberLookup : System.Web.UI.Page
{
    private string TextControl
    {
        get { return ViewState["TextControl"].ToString(); }
        set { ViewState["TextControl"] = value; }
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
            //Get Text ID Control from query string
            TextControl = Request.QueryString["txt"];

            
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillClearingMemberDatagrid();
    }

    private void FillClearingMemberDatagrid()
    {
        uiDgClearingMember.DataSource = ObjectDataSourceClearingMember;
        //IEnumerable<ObjectDataSourceClearingMember>
        IEnumerable dv = (IEnumerable)ObjectDataSourceClearingMember.Select();
        
        //DataTable dt = (DataTable)dve;
        DataView dve = (DataView)dv;
        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }
        
        uiDgClearingMember.DataSource = dv;
        uiDgClearingMember.DataBind();
    }

    protected void uiDgClearingMember_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Set button attributes when button is click
        HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("uiBtnSelect");
        if (btn != null)
        {
            //btn.Attributes.Add("onclick", "." + TextControl + ".value;window.close();");
            //btn.Attributes.Add("onclick", "alert(opener.document.form1.pf1.value);");
            //btn.Attributes.Add("onclick", "alert(opener.document.forms[0].pf1.value);");
            btn.Attributes.Add("onclick", "opener.document.forms[0]." + TextControl + ".value=" + e.Row.Cells[1].Text +";window.close();");
        }
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
}
