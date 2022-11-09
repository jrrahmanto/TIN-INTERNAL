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

public partial class WebUI_ClearingAndSettlement_ViewManageExchange : System.Web.UI.Page
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
        uiTxtExchangeCode.Focus();
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
        Response.Redirect("EntryManageExchange.aspx?eType=add");
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        ////approved
        //List<string> approvedList = getCheckedRows();
        //try
        //{
        //    Exchange.ApproveExchange(approvedList, User.Identity.Name);
        //}
        //catch (Exception ex)
        //{
        //    uiBLError.Items.Add(ex.Message);
        //    uiBLError.Visible = true;
        //}

    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        List<string> rejectedList = getCheckedRows();
        try
        {
            Exchange.RejectExchange(rejectedList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        //deleted
        List<string> deletedList = getCheckedRows();
        try
        {
            Exchange.ProposedDeletedExchange(deletedList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private List<string> getCheckedRows()
    {
        //get dataset to datagrid
        List<string> checkedList = new List<string>();

        foreach (GridViewRow GridRow in uiDgManageExchange.Rows)
        {
            CheckBox selectedCheckBox = new CheckBox();
            selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
            if (selectedCheckBox.Checked)
            {
                DataKey dk = uiDgManageExchange.DataKeys[GridRow.RowIndex];
                decimal value = Convert.ToDecimal(dk.Value);
                checkedList.Add(value.ToString());
            }
        }
        return checkedList;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgManageExchange.DataSource = ObjDtSrcExchange;
        uiDgManageExchange.DataBind();
    }

    protected void uiDgManageExchange_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgManageExchange.PageIndex = e.NewPageIndex;
        FillExchangeDataGrid();
    }

    protected void uiDgManageExchange_Sorting(object sender, GridViewSortEventArgs e)
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

        FillExchangeDataGrid();
    }

    private void FillExchangeDataGrid()
    {
        uiDgManageExchange.DataSource = ObjDtSrcExchange;
        IEnumerable dv = (IEnumerable)ObjDtSrcExchange.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgManageExchange.DataSource = dve;
        uiDgManageExchange.DataBind();
    }

    private ExchangeData.ExchangeDataTable dtManageExchange
    {
        get { return (ExchangeData.ExchangeDataTable)ViewState["dtManageExchange"]; }
        set { ViewState["dtManageExchange"] = value; }
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
        uiDgManageExchange.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

}
