using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class AuditAndCompliance_ViewApplicationLog : System.Web.UI.Page
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
        ddlApplModule.Focus();
        string pageName = Request.Url.AbsolutePath.Replace(".aspx", "");
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        if (!IsValidEntry())
            return;
        FillApplicationLogDataGrid();
    }

    protected void uiDgApplicationLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgApplicationLog.PageIndex = e.NewPageIndex;
        FillApplicationLogDataGrid();
    }

    protected void uiDgApplicationLog_Sorting(object sender, GridViewSortEventArgs e)
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

        FillApplicationLogDataGrid();
    }

    private void FillApplicationLogDataGrid()
    {
        uiDgApplicationLog.DataSource = ObjectDataSourceApplicationLog;
        IEnumerable dv = (IEnumerable)ObjectDataSourceApplicationLog.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        //dtApplicationLog = (ApplicationLogData.ApplicationLogDataTable)dve.Table;

        uiDgApplicationLog.DataSource = dve;
        uiDgApplicationLog.DataBind();
    }

    //private ApplicationLogData.ApplicationLogDataTable dtApplicationLog
    //{
    //    get { return (ApplicationLogData.ApplicationLogDataTable)ViewState["dtApplicationLog"]; }
    //    set { ViewState["dtApplicationLog"] = value; }
    //}

    private bool IsValidEntry()
    {
        bool isValid = true;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();


        if (CtlCalendarPickUp1.Text == "")
        {
            uiBlError.Items.Add("Log Time is required.");
        }
        
        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }
}
