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
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;
////using ClosedXML.Excel;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
public partial class ClearingAndSettlement_MasterData_ViewTermofPayment : System.Web.UI.Page
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
        
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryTermofPayment.aspx?eType=add");
    }


    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillTermOfPaymentDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

   

    protected void uiDgTermOfPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgTermOfPayment.PageIndex = e.NewPageIndex;
            FillTermOfPaymentDataGrid();
        }
        catch (Exception ex)
        {

            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
        
    }

    protected void uiDgTermOfPayment_Sorting(object sender, GridViewSortEventArgs e)
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

            FillTermOfPaymentDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
   
    private void FillTermOfPaymentDataGrid()
    {
        try
        {
            uiDgTermOfPayment.DataSource = ObjectDataSourceTermOfPayment;
            IEnumerable dv = (IEnumerable)ObjectDataSourceTermOfPayment.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgTermOfPayment.DataSource = dve;
            uiDgTermOfPayment.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
     }



   
}
