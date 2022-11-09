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

public partial class WebUI_RiskManagement_ViewStressTestScenario : System.Web.UI.Page
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
        uiTxtScenarioName.Focus();
        try
        {
            SetAccessPage();

            uiBLError.Visible = false;

            if (!Page.IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgStressTestScenario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgStressTestScenario.PageIndex = e.NewPageIndex;
            FillStressTestScenarioDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgStressTestScenario_Sorting(object sender, GridViewSortEventArgs e)
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

            FillStressTestScenarioDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void FillStressTestScenarioDataGrid()
    {
        try
        {
            uiDgStressTestScenario.DataSource = ObjectDataSourceStressTestScenario;
            IEnumerable dv = (IEnumerable)ObjectDataSourceStressTestScenario.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgStressTestScenario.DataSource = dve;
            uiDgStressTestScenario.DataBind();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryStressTestScenario.aspx?eType=add");
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            uiDgStressTestScenario.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillStressTestScenarioDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
