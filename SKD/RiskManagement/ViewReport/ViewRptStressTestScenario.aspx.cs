using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class WebUI_FinanceAndAccounting_ViewManagePostingCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name");

            dt.Rows.Add("Scenario 1");
            dt.Rows.Add("Scenario 2");
            dt.Rows.Add("Scenario 3");
            dt.Rows.Add("Scenario 4");

            uiDgManageExchange.DataSource = dt;
            uiDgManageExchange.DataBind();

        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RiskManagement/EntryStressTestScenario.aspx");
    }
}
