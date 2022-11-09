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


public partial class WebUI_New_ViewExchangeRate : System.Web.UI.Page
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

    protected void uiDgExchangeRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgExchangeRate.PageIndex = e.NewPageIndex;
        FillExchangeRateDataGrid();
    }

    protected void uiDgExchangeRate_Sorting(object sender, GridViewSortEventArgs e)
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

        FillExchangeRateDataGrid();
    }

    private void FillExchangeRateDataGrid()
    {
        uiDgExchangeRate.DataSource = ObjectDataSource1;
        IEnumerable dv = (IEnumerable)ObjectDataSource1.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgExchangeRate.DataSource = dve;
        uiDgExchangeRate.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiDdlExchType.Focus();
        SetAccessPage();
        uiBLError.Visible = false;   
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgExchangeRate.DataSource = ObjectDataSource1;
        uiDgExchangeRate.DataBind();
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryExchangeRate.aspx?eType=add");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        List<string> approvedList = getCheckedRows();
        try
        {
            ExchangeRate.ApproveExchangeRate(approvedList,User.Identity.Name);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
       
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        List<string> rejectedList = getCheckedRows();
        try
        {
            ExchangeRate.RejectExchangeRate(rejectedList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        List<string> deletedList = getCheckedRows();
        try
        {
            ExchangeRate.ProposedDeletedExchangeRate(deletedList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private  List<string> getCheckedRows()
    {
        List<string> checkedList = new List<string>();

        foreach (GridViewRow GridRow in uiDgExchangeRate.Rows)
        {
            CheckBox selectedCheckBox = new CheckBox();
            selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
            if (selectedCheckBox.Checked)
            {
                DataKey dk = uiDgExchangeRate.DataKeys[GridRow.RowIndex];
                decimal value = Convert.ToDecimal(dk.Value);
                checkedList.Add(value.ToString());
            }
        }
        return checkedList;
    }


    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
        uiDgExchangeRate.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

  
}
