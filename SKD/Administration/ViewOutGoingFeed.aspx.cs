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

public partial class Administration_ViewOutGoingFeed : System.Web.UI.Page
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
        uiDdlFeedType.Focus();
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            uiDgOutGoingFeed.DataSource = ObjectDataSourceOutGoingFeed;
            uiDgOutGoingFeed.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    protected void uiDgOutGoingFeed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        MasterPage mp = (MasterPage)this.Master;

        Label uiDdlFeedType = (Label)e.Row.FindControl("uiLblFeedType");
        Label uiDdlSubmittedStatus = (Label)e.Row.FindControl("uiLblSubmittedStatus");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!string.IsNullOrEmpty(uiDdlFeedType.Text))
            {
                switch (uiDdlFeedType.Text)
                {
                    case "B":
                        uiDdlFeedType.Text = "Bank Transaction";
                        break;
                    case "A":
                        uiDdlFeedType.Text = "Opening Balance";
                        break;
                    default:
                        uiDdlFeedType.Text = uiDdlFeedType.Text;
                        break;                    
                }
            }
            
            if (!string.IsNullOrEmpty(uiDdlSubmittedStatus.Text))
            {
                switch (uiDdlSubmittedStatus.Text)
                {
                    case "N":
                        uiDdlSubmittedStatus.Text = "Queue";
                        break;
                    case "Y":
                        uiDdlSubmittedStatus.Text = "Submitted";
                        break;
                    default:
                        uiDdlSubmittedStatus.Text = uiDdlSubmittedStatus.Text;
                        break;
                }
            }
        }
    }

    protected void uiDgOutGoingFeed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgOutGoingFeed.PageIndex = e.NewPageIndex;
            FillOutGoingFeedDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    protected void uiDgOutGoingFeed_Sorting(object sender, GridViewSortEventArgs e)
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

            FillOutGoingFeedDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    private void FillOutGoingFeedDataGrid()
    {
        try
        {
            uiDgOutGoingFeed.DataSource = ObjectDataSourceOutGoingFeed;
            IEnumerable dv = (IEnumerable)ObjectDataSourceOutGoingFeed.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgOutGoingFeed.DataSource = dve;
            uiDgOutGoingFeed.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }
}
