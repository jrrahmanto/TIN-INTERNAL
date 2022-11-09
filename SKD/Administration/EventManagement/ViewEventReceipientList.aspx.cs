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

public partial class Administration_EventManagement_ViewEventReceipientList : System.Web.UI.Page
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
        uiTxtEventReceipientListName.Focus();
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch(Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryEventReceipientList.aspx?eType=add");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            uiDgEventReceipientList.DataSource = ObjectDataSourceEventReceipientList;
            uiDgEventReceipientList.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    protected void uiDgEventReceipientList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgEventReceipientList.PageIndex = e.NewPageIndex;
            FillEventRecipientListDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgEventReceipientList_Sorting(object sender, GridViewSortEventArgs e)
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

            FillEventRecipientListDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    private void FillEventRecipientListDataGrid()
    {
        try
        {
            uiDgEventReceipientList.DataSource = ObjectDataSourceEventReceipientList;
            IEnumerable dv = (IEnumerable)ObjectDataSourceEventReceipientList.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgEventReceipientList.DataSource = dve;
            uiDgEventReceipientList.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message,ex);
        }   
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            uiDgEventReceipientList.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }  
     }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        MasterPage mp = (MasterPage)this.Master;

        if (string.IsNullOrEmpty(uiTxtEventReceipientListName.Text))
        {
            uiBLError.Items.Add("Event receipient listname is required.");
        }


        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

   
}
