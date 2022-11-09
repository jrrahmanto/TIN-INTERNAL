using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_MasterData_ViewContract : System.Web.UI.Page
{
    #region "   Use Case   "

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

    // Handler for page load
    protected void Page_Load(object sender, EventArgs e)
    {
        uiDdlApprovalStatus.Focus();
        try
        {
            SetControlAccessByMakerChecker();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    // Handler for search button
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Nullable<decimal> commID = null;
            Nullable<int> month = null;
            Nullable<int> year = null;
            string approval = null;
            if (uiDtpCommID.LookupTextBoxID != "")
            {
                commID = Convert.ToDecimal(uiDtpCommID.LookupTextBoxID);
            }
            if (uiDtpMonthYear.Month != "")
            {
                month = int.Parse(uiDtpMonthYear.Month);
            }
            if (uiDtpMonthYear.Year != "")
            {
                year = int.Parse(uiDtpMonthYear.Year);
            }
            if (uiDdlApprovalStatus.Text != "")
            {
                approval = uiDdlApprovalStatus.SelectedValue;
            }
            ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
            dt = Contract.FillBySearchCriteria(commID, month, year, approval);

            uiDgContract.DataSource = dt;
            uiDgContract.DataBind();

            dt.Dispose();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    // Handler for create button
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryContract.aspx");
    }

    // Handler for index change
    protected void uiDgContract_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgContract.PageIndex = e.NewPageIndex;
        FillContractDataGrid();
    }

    // Handler for sorting
    protected void uiDgContract_Sorting(object sender, GridViewSortEventArgs e)
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
        //Nullable<decimal> commID = null;
        //Nullable<int> month = null;
        //Nullable<int> year = null;
        //string approval = null;

        //if (uiDtpCommID.LookupTextBoxID != "")
        //{
        //    commID = Convert.ToDecimal(uiDtpCommID.LookupTextBoxID);
        //}
        //if (uiDtpMonthYear.Month != "")
        //{
        //    month = int.Parse(uiDtpMonthYear.Month);
        //}
        //if (uiDtpMonthYear.Year != "")
        //{
        //    year = int.Parse(uiDtpMonthYear.Year);
        //}
        //if (uiDdlApprovalStatus.Text != "")
        //{
        //    approval = uiDdlApprovalStatus.SelectedValue;
        //}

        //ContractData.ContractDataTable dt = new ContractData.ContractDataTable();
        //dt = Contract.FillBySearchCriteria(commID, month, year, approval);

        IEnumerable dv = (IEnumerable)odsContract.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgContract.DataSource = dve;
        uiDgContract.DataBind();
    }

    #endregion
}
