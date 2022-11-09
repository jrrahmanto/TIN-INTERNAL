using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_RandomizedTender : System.Web.UI.Page
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
        uiBLError.Items.Clear();

        if (!Page.IsPostBack)
        {
           

        }
    }

    protected void uiBtnRerun_Click(object sender, EventArgs e)
    {
        if (IsValidRandom())
        {
            Tender.ProcessTenderRandomized(DateTime.Parse(CtlCalendarPickUp1.Text), null);
        }
    }

    private bool IsValidRandom()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
        {
            uiBLError.Items.Add("Business date is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
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

        dve.RowFilter = "TenderReqType='R'";        

        uiDgTender.DataSource = dve;
        uiDgTender.DataBind();
    }
}
