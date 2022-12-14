using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_New_ViewIjinUsaha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("kode");
            for (int ii = 1; ii < 7; ii++)
            {
                dt.Rows.Add(ii);
            }

            uiDgIjinUsaha.DataSource = dt;
            uiDgIjinUsaha.DataBind();

        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryIjinUsaha.aspx");
    }
    protected void uiDgIjinUsaha_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Response.Redirect("EntryIjinUsaha.aspx");
        }
    }
}
