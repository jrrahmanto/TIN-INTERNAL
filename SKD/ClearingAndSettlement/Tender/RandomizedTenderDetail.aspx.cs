using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_RandomizedTenderDetail : System.Web.UI.Page
{

    private int TenderID
    {
        get
        {
            return int.Parse(Request.QueryString["tenderId"]);
        }
    }

    private DateTime TenderDate
    {
        get { return DateTime.Parse(Request.QueryString["tenderDate"]);}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        if (!Page.IsPostBack)
        {

            FillTenderResultDataGrid();
        }
    }

    protected void uiBtnRerun_Click(object sender, EventArgs e)
    {
        try
        {
            Tender.ProcessTenderRandomized(TenderDate, TenderID);

            FillTenderResultDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }        
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("RandomizedTender.aspx");
    }

    protected void uiDgRequestTender_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label buyerInvId = (Label)e.Row.FindControl("uiLblBuyerInvId");
            buyerInvId.Text = Investor.GetNameInvestorByInvestorID(decimal.Parse(buyerInvId.Text));
        }
    }

    private void FillTenderResultDataGrid()
    {
        uiDgRequestTender.DataSource = ObjectDataSourceTenderRandomizedDetail;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTenderRandomizedDetail.Select();
        DataView dve = (DataView)dv;      

        uiDgRequestTender.DataSource = dve;
        uiDgRequestTender.DataBind();
    }
}
