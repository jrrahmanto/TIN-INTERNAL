using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_Fajar_StressTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("contract");
            dt2.Columns.Add("type");
            dt2.Columns.Add("base");
            dt2.Columns.Add("low");
            dt2.Columns.Add("high");
            dt2.Rows.Add("OLE 2009011", "Percentage", "10000", "-5% (9500)", "+5% (10500)");
            dt2.Rows.Add("OLE 2009012", "Value", "-", "9000", "11000");

            uiDgManageExchange0.DataSource = dt2;
            uiDgManageExchange0.DataBind();

            DataTable dt = new DataTable();
            dt.Columns.Add("kontrak");
            dt.Columns.Add("member");
            dt.Columns.Add("account");
            dt.Rows.Add("OLE 200911", "PT AAA", "0001");
            dt.Rows.Add("OLE 200911", "PT AAA", "0002");
            dt.Rows.Add("OLE 200911", "PT BBB", "0001");
            dt.Rows.Add("OLE 200911", "PT BBB", "0002");
            dt.Rows.Add("OLE 200911", "PT BBB", "0003");
            dt.Rows.Add("OLE 200912", "PT AAA", "0002");
            dt.Rows.Add("OLE 200912", "PT AAA", "0003");
            dt.Rows.Add("OLE 200912", "PT CCC", "0001");
            dt.Rows.Add("OLE 200912", "PT CCC", "0003");

            uiDgManageExchange.DataSource = dt;
            uiDgManageExchange.DataBind();

        }

    }
}
