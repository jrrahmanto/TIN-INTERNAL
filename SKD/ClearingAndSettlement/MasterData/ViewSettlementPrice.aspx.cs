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

public partial class WebUI_New_ViewSettlementPrice : System.Web.UI.Page
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

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        if (!Page.IsPostBack)
        {
            uiBLError.Visible = false;
        }
    }
   
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                FillSettlementPriceDataGrid();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgSettlementPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgSettlementPrice.PageIndex = e.NewPageIndex;
        FillSettlementPriceDataGrid();
    }

    protected void uiDgSettlementPrice_Sorting(object sender, GridViewSortEventArgs e)
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

        FillSettlementPriceDataGrid();
    }

    private void FillSettlementPriceDataGrid()
    {
        uiDgSettlementPrice.DataSource = ObjectDataSourceSettlementPrice;
        IEnumerable dv = (IEnumerable)ObjectDataSourceSettlementPrice.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgSettlementPrice.DataSource = dve;
        uiDgSettlementPrice.DataBind();
    }

    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            if (Menu == "hide")
            {
                Response.Redirect("EntrySettlementPrice.aspx?menu=hide&eType=add&businessDate=" + CtlCalendarBusinessDate.Text + "");
            }
            else
            {
                Response.Redirect("EntrySettlementPrice.aspx?eType=add&businessDate=" + CtlCalendarBusinessDate.Text + "");
            }
            
        }
        
    }
    
    protected void uiBtnEdit_Click(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            Response.Redirect("EntrySettlementPrice.aspx?menu=hide&eType=edit&businessDate=" + CtlCalendarBusinessDate.Text + "");
        }
        else
        {

            Response.Redirect("EntrySettlementPrice.aspx?eType=edit&businessDate=" + CtlCalendarBusinessDate.Text + "");
        }
    }

    protected void uiBtnViewTransaction_Click(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            Response.Redirect("EntrySettlementPrice.aspx?menu=hide&eType=transaction&businessDate=" + CtlCalendarBusinessDate.Text + "&approval=P");
        }
        else
        {

            Response.Redirect("EntrySettlementPrice.aspx?eType=transaction&businessDate=" + CtlCalendarBusinessDate.Text + "&approval=P");
        }
        
    }

    #region "----- Supporting methode -----"

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCreate.Visible = mp.IsMaker;
        uiBtnEdit.Visible = mp.IsMaker;
        uiBtnViewTransaction.Visible = mp.IsChecker;
        uiDgSettlementPrice.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
    }

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (string.IsNullOrEmpty(CtlCalendarBusinessDate.Text))
            {
                uiBLError.Items.Add("Business date is required.");
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

    protected void uiBtnPrint_Click(object sender, EventArgs e)
    {
        GridView gv = uiDgSettlementPrice;
        if (gv.Rows.Count > 0)
        {
            string fileName = "SP" + DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fileName));
            Response.Charset = "";

            Response.ContentType = "application/vnd.csv";
            System.IO.StringWriter sWriter;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sWriter = new System.IO.StringWriter(sb);

            // First we will write the headers.
            IEnumerable dv = (IEnumerable)ObjectDataSourceSettlementPrice.Select();
            DataView dve = (DataView)dv;
            DataTable dt = dve.Table;
            int iColCount = dt.Columns.Count;

            sWriter.Write("Business Date,Code,Contract Month,Settlement Price,Settlement Price Type Desc," +
                          "ActionDesc");

            sWriter.Write(sWriter.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dt.Rows)
            {
                string strValue = "";
                if (!Convert.IsDBNull(dr["BusinessDate"]))
                {
                    strValue += (dr["BusinessDate"].ToString()) + ",";
                }
                if (!Convert.IsDBNull(dr["CommodityCode"]))
                {
                    strValue += (dr["CommodityCode"].ToString()) + ",";
                }
                if (!Convert.IsDBNull(dr["ContractYearMonth"]))
                {
                    strValue += (dr["ContractYearMonth"].ToString()) + ",";
                }
                if (!Convert.IsDBNull(dr["SettlementPrice"]))
                {
                    strValue += (dr["SettlementPrice"].ToString()) + ",";
                }
                if (!Convert.IsDBNull(dr["SettlementPriceTypeDesc"]))
                {
                    strValue += (dr["SettlementPriceTypeDesc"].ToString()) + ",";
                }
                if (!Convert.IsDBNull(dr["ActionDesc"]))
                {
                    strValue += (dr["ActionDesc"].ToString()) + ",";
                }
                sWriter.Write(strValue);
                sWriter.Write(sWriter.NewLine);
            }
            sWriter.Close();


            Response.Write(sb.ToString());
            Response.End();
        }
       
    }
}
