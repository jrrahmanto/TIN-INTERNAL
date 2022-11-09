using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_ViewTenderApproval : System.Web.UI.Page
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

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiDdlStatus.Focus();
        if (!Page.IsPostBack)
        {
           

        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillTenderDataGrid();
    }

    protected void uiDgTender_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTender.PageIndex = e.NewPageIndex;
        FillTenderDataGrid();
    }

    protected void uiDgTender_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Change Investor ID to Investor Name
            Label sellerInv = (Label)e.Row.FindControl("uiLblSellerInvId");
            sellerInv.Text = Investor.GetNameInvestorByInvestorID(decimal.Parse(sellerInv.Text));

            //Change Approval Status to description
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            switch (approvalStatus.Text)
            {
                case "P":
                    approvalStatus.Text = "Proposed";
                    break;
                case "A":
                    approvalStatus.Text = "Approved";
                    break;
                case "R":
                    approvalStatus.Text = "Rejected";
                    break;
                default:
                    approvalStatus.Text = approvalStatus.Text;
                    break;
            }
        }
    }
    protected void uiDgTender_Sorting(object sender, GridViewSortEventArgs e)
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

        FillTenderDataGrid();
    }

    private void FillTenderDataGrid()
    {
        uiDgTender.DataSource = ObjectDataSourceTender;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTender.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgTender.DataSource = dve;
        uiDgTender.DataBind();
    }
}
