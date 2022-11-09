using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


public partial class WebUI_ClearingAndSettlement_ViewManageHoliday : System.Web.UI.Page
{
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
        uiDdlHolidayType.Focus();
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
   
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgHoliday.DataSource = ObjectDataSourceHoliday;
        uiDgHoliday.DataBind();
    }

    protected void uiDgHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgHoliday.PageIndex = e.NewPageIndex;
        FillHolidayDataGrid();
    }

    protected void uiDgHoliday_Sorting(object sender, GridViewSortEventArgs e)
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

        FillHolidayDataGrid();
    }

    private void FillHolidayDataGrid()
    {
        uiDgHoliday.DataSource = ObjectDataSourceHoliday;
        IEnumerable dv = (IEnumerable)ObjectDataSourceHoliday.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgHoliday.DataSource = dve;
        uiDgHoliday.DataBind();
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManageHoliday.aspx?eType=add");
    }
}
