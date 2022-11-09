using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using 

public partial class Home_ViewInbox : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
            Session["EventRecipientListID"] = EventReceipientList.GetRecipientListID(User.Identity.Name);
            CtlCalendarPickUp1.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
            uiFLDEventRecipientID.Value = EventReceipientList.GetRecipientListID(User.Identity.Name).ToString();
            //BindingDataGrid();
        }
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
        //Session.Remove("EventRecipientListID");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        BindingDataGrid();
    }

    protected void odsEventLog_DataBinding(object sender, EventArgs e)
    {
        
    }

    protected void BindingDataGrid()
    {
        uiDgInbox.DataSource = odsEventLog;
        uiDgInbox.DataBind();
    }

    protected void uiDgInbox_Sorting(object sender, GridViewSortEventArgs e)
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

        BindingDataGrid();
    }
    protected void uiDgInbox_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgInbox.PageIndex = e.NewPageIndex;
        BindingDataGrid();
    }
}
