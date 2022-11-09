using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Collections;

public partial class WebUI_New_ViewUser : System.Web.UI.Page
{

    #region "    Use Case    "
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
        uiTxbUserName.Focus();
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryUser.aspx");
    }
    protected void uiDgUser_Sorting(object sender, GridViewSortEventArgs e)
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

        FillUserDataGrid();
    }
    protected void uiDgUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgUser.PageIndex = e.NewPageIndex;
        FillUserDataGrid();
    }
    #endregion
    
    #region "    Supporting Method    "
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

    private void FillUserDataGrid()
    {

        IEnumerable dv = (IEnumerable)odsUser.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgUser.DataSource = dve;
        uiDgUser.DataBind();
    }
    #endregion

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            UserData.aspnet_UsersDataTable dt = new UserData.aspnet_UsersDataTable();
            dt = SKDUser.GetUserByUserNameLike(uiTxbUserName.Text);

            uiDgUser.DataSource = dt;
            uiDgUser.DataBind();

            dt.Dispose();
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }
}
