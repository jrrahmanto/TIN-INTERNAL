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

public partial class WebUI_New_ViewIRCA : System.Web.UI.Page
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
        uiDdlAction.Focus();
        SetAccessPage();
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            CtlCalendarEffectiveStartDate.SetCalendarValue(DateTime.Now.ToString("dd-MMM-yyyy"));
        }
    }


    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillIRCADataGrid();
    }

    protected void uiDgIRCA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgIRCA.PageIndex = e.NewPageIndex;
        FillIRCADataGrid();
    }

    protected void uiDgIRCA_Sorting(object sender, GridViewSortEventArgs e)
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

        FillIRCADataGrid();
    }

    private void FillIRCADataGrid()
    {
        uiDgIRCA.DataSource = ObjectDataSourceIRCA;
        IEnumerable dv = (IEnumerable)ObjectDataSourceIRCA.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgIRCA.DataSource = dve;
        uiDgIRCA.DataBind();
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            Response.Redirect("EntryIRCA.aspx?eType=add&startDate=" + CtlCalendarEffectiveStartDate.Text + "");
        }
    }
    protected void uiBtnEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryIRCA.aspx?eType=edit&startDate=" + CtlCalendarEffectiveStartDate.Text + "");
    }
    protected void uiBtnViewTransaction_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryIRCA.aspx?eType=transaction&startDate=" + CtlCalendarEffectiveStartDate.Text + "");
    }

    #region "-----Supporting Method-----"
    
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
        uiBtnEdit.Visible = mp.IsMaker;
        uiBtnViewTransaction.Visible = mp.IsChecker;
        uiDgIRCA.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text))
            {
                uiBLError.Items.Add("Effective start date is required.");
            }

            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }

            return isValid;

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    
    #endregion


}
