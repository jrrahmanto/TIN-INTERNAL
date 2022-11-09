using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ClearingAndSettlement_MasterData_ViewManageBroadcast : System.Web.UI.Page
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
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgEventLog.DataSource = ObjectDataSourceEventLog;
        uiDgEventLog.DataBind();
    }

    protected void uiDgEventLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgEventLog.PageIndex = e.NewPageIndex;
        FillEventLogDataGrid();
    }

    protected void uiDgEventLog_Sorting(object sender, GridViewSortEventArgs e)
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

        FillEventLogDataGrid();
    }

    private void FillEventLogDataGrid()
    {
        uiDgEventLog.DataSource = ObjectDataSourceEventLog;
        IEnumerable dv = (IEnumerable)ObjectDataSourceEventLog.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgEventLog.DataSource = dve;
        uiDgEventLog.DataBind();
    }
}
