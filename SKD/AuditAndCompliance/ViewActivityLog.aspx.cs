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

public partial class AuditAndCompliance_ViewActivityLog : System.Web.UI.Page
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
        uiLblWarning.Visible = false;

        string pageName = Request.Url.AbsolutePath.Replace(".aspx", "");

    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgActivityLog.DataSource = ObjectDataSourceActivityLog;
        uiDgActivityLog.DataBind();
    }

    protected void uiDgActivityLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgActivityLog.PageIndex = e.NewPageIndex;
        FillActivityLogDataGrid();
    }

    protected void uiDgActivityLog_Sorting(object sender, GridViewSortEventArgs e)
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

        FillActivityLogDataGrid();
    }

    private void FillActivityLogDataGrid()
    {
        uiDgActivityLog.DataSource = ObjectDataSourceActivityLog;
        IEnumerable dv = (IEnumerable)ObjectDataSourceActivityLog.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        dtActivityLog = (ActivityLogData.ActivityLogDataTable)dve.Table;

        uiDgActivityLog.DataSource = dve;
        uiDgActivityLog.DataBind();
    }

    private ActivityLogData.ActivityLogDataTable dtActivityLog
    {
        get { return (ActivityLogData.ActivityLogDataTable)ViewState["dtActivityLog"]; }
        set { ViewState["dtActivityLog"] = value; }
    }
}
