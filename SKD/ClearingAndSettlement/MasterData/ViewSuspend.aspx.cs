using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Microsoft.Reporting.WebForms;

public partial class WebUI_New_ViewSuspend : System.Web.UI.Page
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
            SetControlAccessByMakerChecker();
            //if (!IsPostBack)
            //{
            //    tblGrid.Visible = false;
            //    tblRpt.Visible = false;
            //}

        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntrySuspendStatus.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            tblGrid.Visible = true;
            //tblRpt.Visible = false;
            FillContractDataGrid();
            

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

    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        //tblGrid.Visible = false;
        tblRpt.Visible = true;

        FillSuspendStatus();
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
        uiBtnCreate.Visible = false;//!pageMaker;
        
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

    private void FillSuspendStatus()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SPPK_KBI_ReportEOD/RptSuspendStatus";

        List<ReportParameter> rp = new List<ReportParameter>();

        if (string.IsNullOrEmpty(CtlInvestorLookup1.LookupTextBox))
        {
            rp.Add(new ReportParameter("code", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("code", new string[] { CtlInvestorLookup1.LookupTextBox }));
        }
        if (string.IsNullOrEmpty(uiDdlApprovalStatus.Text))
        {
            rp.Add(new ReportParameter("approvalStatus", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("approvalStatus", new string[] { uiDdlApprovalStatus.SelectedValue }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }
    #endregion





}
