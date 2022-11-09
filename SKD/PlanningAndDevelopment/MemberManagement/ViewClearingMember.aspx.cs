using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class WebUI_New_ViewClearingMember : System.Web.UI.Page
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

    #region "   Use Case   "

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiTxtCMCode.Focus();
        if (!Page.IsPostBack)
        {
            uiBlError.Visible = false;

            try
            {
                ExchangeData.ExchangeDdlDataTable dt = new ExchangeData.ExchangeDdlDataTable();
                dt = Exchange.SelectExchange();

                uiDdlExchange.DataSource = dt;
                uiDdlExchange.DataBind();
                uiDdlExchange.DataTextField = "ExchangeCode";
                uiDdlExchange.DataValueField = "ExchangeCode";
                uiDdlExchange.Items.Insert(0, new ListItem("", String.Empty));

            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex);
            }

            //ClearingMemberExchangeData.ClearingMemberDDLDataTable cmdt = new ClearingMemberExchangeData.ClearingMemberDDLDataTable();
            //cmdt = ClearingMemberExchange.fillDDl();

            //uiDdlMembership.DataSource = cmdt;
            //uiDdlMembership.DataBind();
            //uiDdlMembership.DataTextField = "CMType";
            //uiDdlMembership.DataValueField = "CMType";
            //uiDdlMembership.Items.Insert(0, new ListItem("", String.Empty));
        } 
        
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryClearingMember.aspx");
    }

    protected void uiDgClearingMember_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Response.Redirect("EntryClearingMember.aspx");
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillCelaringMemberDataGrid();
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        List<string> approveList = new List<string>();

        try
        {
            approveList = getCheckedRows();
            ClearingMember.Approve(approveList, User.Identity.Name, "Batch Approve");
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        List<string> rejectList = new List<string>();

        try
        {
            rejectList = getCheckedRows();
            ClearingMember.Reject(rejectList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        List<string> deleteList = new List<string>();

        try
        {
            deleteList = getCheckedRows();
            ClearingMember.ProposeDelete(deleteList, User.Identity.Name);
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    protected void uiDgClearingMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgClearingMember.PageIndex = e.NewPageIndex;
        FillCelaringMemberDataGrid();
    }

    private void FillCelaringMemberDataGrid()
    {
        try
        {
            uiDgClearingMember.DataSource = ObjectDataSource3;
            IEnumerable dv = (IEnumerable)ObjectDataSource3.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgClearingMember.DataSource = dve;
            uiDgClearingMember.DataBind();
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
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

    private List<string> getCheckedRows()
    {
        List<string> checkedList = new List<string>();

        foreach (GridViewRow GridRow in uiDgClearingMember.Rows)
        {
            CheckBox selectedCheckBox = new CheckBox();
            selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
            if (selectedCheckBox.Checked)
            {
                DataKey dk = uiDgClearingMember.DataKeys[GridRow.RowIndex];
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
    }

    #endregion

    protected void uiDgClearingMember_Sorting(object sender, GridViewSortEventArgs e)
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

        FillCelaringMemberDataGrid();
    }
}
