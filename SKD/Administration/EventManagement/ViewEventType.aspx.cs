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

public partial class Administration_EventManagement_ViewEventType : System.Web.UI.Page
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
        uiTxtEventTypeCode.Focus();
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
            if (!Page.IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryEventType.aspx?eType=add");
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            uiDgEventType.DataSource = ObjectDataSourceEventType;
            uiDgEventType.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiDgEventType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    { 
        try
        {
            uiDgEventType.PageIndex = e.NewPageIndex;
            FillEventTypeDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgEventType_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
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

            FillEventTypeDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillEventTypeDataGrid()
    {
        try
        {
            uiDgEventType.DataSource = ObjectDataSourceEventType;
            IEnumerable dv = (IEnumerable)ObjectDataSourceEventType.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }
            uiDgEventType.DataSource = dve;
            uiDgEventType.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            uiDgEventType.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


}
