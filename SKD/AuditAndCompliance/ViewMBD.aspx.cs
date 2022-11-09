using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class AuditAndCompliance_ViewMBD : System.Web.UI.Page
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
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        MBDData.MBDDataTable dt = new MBDData.MBDDataTable();
        try
        {
            Nullable<decimal> cmid = null;
            if (uiCtlCM.LookupTextBoxID != "")
            {
                cmid = decimal.Parse(uiCtlCM.LookupTextBoxID.ToString());
            }

            dt = MBD.FillBySearchCriteria(cmid, uiDdlApprovalStatus.SelectedValue);
            uiDgMBD.DataSource = odsMBD;
            uiDgMBD.DataBind();
        }
        catch (Exception ex)
        {

            DisplayErrorMessage(ex);
        }
    }

    protected void uiDgMBD_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgMBD.PageIndex = e.NewPageIndex;
        FillMBDDataGrid();
    }

    protected void uiDgMBD_Sorting(object sender, GridViewSortEventArgs e)
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

        FillMBDDataGrid();
    }

    private void FillMBDDataGrid()
    {
        uiDgMBD.DataSource = odsMBD;
        IEnumerable dv = (IEnumerable)odsMBD.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgMBD.DataSource = dve;
        uiDgMBD.DataBind();
    }


    protected void uiBtnImport_Click(object sender, EventArgs e)
    {
        Response.Redirect("ImportMBD.aspx");
    }

    private void DisplayErrorMessage(Exception ex)
    {
        uiBLError.Items.Clear();
        uiBLError.Items.Add(ex.Message);
        uiBLError.Visible = true;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiDgMBD.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

}
