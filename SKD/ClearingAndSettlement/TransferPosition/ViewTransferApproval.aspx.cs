using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class WebUI_New_ViewTransferApproval : System.Web.UI.Page
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

    private string TransferType
    {
        get { return Request.QueryString["transferType"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiDdlTransferType.Focus();
    }

  
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillTransferPositionDataGrid();
    }

    protected void uiDgTransferPosition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTransferPosition.PageIndex = e.NewPageIndex;
        FillTransferPositionDataGrid();
    }

    protected void uiDgTransferPosition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Change Transfer Type
            Label transferType = (Label)e.Row.FindControl("uiLblTransferType");
            switch (transferType.Text)
            {
                case "MM":
                    transferType.Text = "Member to Member";
                    break;
                case "AA":
                    transferType.Text = "Account to Account";
                    break;
                case "BT":
                    transferType.Text = "Bulk Transfer";
                    break;
                default:
                    transferType.Text = transferType.Text;
                    break;
            }

            //Change Destination Clearing Member ID to Code
            Label destClearingMember = (Label)e.Row.FindControl("uiLblCMID");
            destClearingMember.Text = ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(destClearingMember.Text));

            //Change apprroval to description
            Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
            approvalStatus.Text = Common.GetApprovalDescription(approvalStatus.Text);

        }
    }

    protected void uiDgTransferPosition_Sorting(object sender, GridViewSortEventArgs e)
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

        FillTransferPositionDataGrid();
    }

    private void FillTransferPositionDataGrid()
    {
        uiDgTransferPosition.DataSource = ObjectDataSourceTransferPosition;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTransferPosition.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgTransferPosition.DataSource = dve;
        uiDgTransferPosition.DataBind();
    }
}
