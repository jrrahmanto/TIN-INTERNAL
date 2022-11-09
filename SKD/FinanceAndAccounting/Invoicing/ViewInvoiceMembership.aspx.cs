using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Invoicing_ViewInvoice : System.Web.UI.Page
{
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

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryInvoiceMembership.aspx?eType=add");
    }


    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillBankAccountDataGrid()
    {
        try
        {
            uiDgInvoice.DataSource = ObjectDataSourceInvoice;
            IEnumerable dv = (IEnumerable)ObjectDataSourceInvoice.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgInvoice.DataSource = dve;
            uiDgInvoice.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

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

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    protected void uiDgInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgInvoice.PageIndex = e.NewPageIndex;
            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgInvoice_Sorting(object sender, GridViewSortEventArgs e)
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

            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}