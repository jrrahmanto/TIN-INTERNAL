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

public partial class FinanceAndAccounting_Parameter_ViewManageFee : System.Web.UI.Page
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
        uiDdlApprovalStatus.Focus();
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
    
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManageFee.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillFeeDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgFee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgFee.PageIndex = e.NewPageIndex;
            FillFeeDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgFee_Sorting(object sender, GridViewSortEventArgs e)
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

            FillFeeDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillFeeDataGrid()
    {
        try
        {
            uiDgFee.DataSource = ObjectDataSourceFee;
            IEnumerable dv = (IEnumerable)ObjectDataSourceFee.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgFee.DataSource = dve;
            uiDgFee.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            uiDgFee.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
