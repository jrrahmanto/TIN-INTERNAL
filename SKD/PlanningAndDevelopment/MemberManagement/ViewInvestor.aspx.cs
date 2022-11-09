using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_ViewInvestor : System.Web.UI.Page
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

    #region "    Use Case    "
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CtlClearingMemberLookup1.ExchangeMemberCodeLookupID = CtlExchangeMemberLookup1.ExchangeCodeControlID;
            SetControlAccessByMakerChecker();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryInvestor.aspx");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Nullable<decimal> cmid = null;
            Nullable<decimal> emid = null;
            string approval = null;
            if (CtlClearingMemberLookup1.LookupTextBoxID != "")
            {
                cmid = Convert.ToDecimal(CtlClearingMemberLookup1.LookupTextBoxID);
            }
            if (CtlExchangeMemberLookup1.LookupTextBoxID != "")
            {
                emid = Convert.ToDecimal(CtlExchangeMemberLookup1.LookupTextBoxID);
            }
            if (uiDdlApprovalStatus.Text != "")
            {
                approval = uiDdlApprovalStatus.SelectedValue;
            }
            InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
            dt = Investor.FillBySearchCriteria(cmid, emid, approval);
            uiDgInvestor.DataSource = dt;
            uiDgInvestor.DataBind();

            dt.Dispose();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiDgInvestor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgInvestor.PageIndex = e.NewPageIndex;
        FillContractDataGrid();
    }

    protected void uiDgInvestor_Sorting(object sender, GridViewSortEventArgs e)
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

        FillContractDataGrid();
    }
    #endregion
    
    #region "   Supporting Method   "

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }

    // SetControlAccessByMakerChecker
    // Purpose      : Set Control visibility / enabled based on maker checker privilege
    // Parameter    : -
    // Return       : -
    private void SetControlAccessByMakerChecker()
    {
        MasterPage mp = (MasterPage)this.Master;

        bool pageMaker = mp.IsMaker;

        // Set control visibility
        uiBtnCreate.Visible = pageMaker;
        
    }

    private void FillContractDataGrid()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceInvestor.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgInvestor.DataSource = dve;
        uiDgInvestor.DataBind();
    }

    #endregion


   
}
