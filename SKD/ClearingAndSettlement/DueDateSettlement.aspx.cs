using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_DueDateSettlement : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
            Search();
        }
    }

    protected void uiDgDueDate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgDueDate.PageIndex = e.NewPageIndex;
        Search();
    }

    protected void uiDgDueDate_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void uiDgDueDate_Sorting(object sender, GridViewSortEventArgs e)
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

        Search();
    }

    private void Search()
    {
        EODTradeProgressData.ViewDueDateSettlementDataTable dt = new EODTradeProgressData.ViewDueDateSettlementDataTable();

        try
        {
            DateTime bussDate =DateTime.Parse (Convert.ToDateTime(Session["Busdate"]).AddDays(-1).ToString("yyyy-MM-dd"));
            //DateTime bussDate = DateTime.Parse(DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
            //DateTime bussDate = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
            //dt = EODTradeProgress.GetDueDateSettlement(bussDate);
            dt = EODTradeProgress.GetDueDateSettlement(bussDate);

            DataView dv = new DataView(dt);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgDueDate.DataSource = dv;
            uiDgDueDate.DataBind();
        }
        catch (Exception ex)
        {
            uiBlError.Visible = true;
            uiBlError.Items.Add(ex.Message);
        }
        finally
        {
            dt.Dispose();
        }
    }
}