using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_New_ViewAccessPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiTxbPagename.Focus();
        if (!Page.IsPostBack)
        {
        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryAccessPage.aspx");
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        ApplicationPageData.ApplicationPageDataTable dt = new ApplicationPageData.ApplicationPageDataTable();
        dt = ApplicationPage.FillByPageName(uiTxbPagename.Text);

        uiDgApplicationPage.DataSource = dt;
        uiDgApplicationPage.DataBind();
    }

    protected void uiDgApplicationPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgApplicationPage.PageIndex = e.NewPageIndex;
        ApplicationPageData.ApplicationPageDataTable dt = new ApplicationPageData.ApplicationPageDataTable();
        dt = ApplicationPage.FillByPageName(uiTxbPagename.Text);

        uiDgApplicationPage.DataSource = dt;
        uiDgApplicationPage.DataBind();
        dt.Dispose();
        
    }
}
