using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_EntryRptFee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                BindData();
                SetControlAccessByMakerChecker();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage(ex);
            }
        }
        MasterPage master = (MasterPage)this.Master;

        bool isMaker = master.IsMaker;

        string[] roles = Roles.GetRolesForUser();
    }
    private void SetControlAccessByMakerChecker()
    {


    }
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }
    private void DisplayErrorMessage(Exception ex)
    {
        //uiBlError.Items.Clear();
        //uiBlError.Items.Add(ex.Message);
        //uiBlError.Visible = true;
    }
    private void BindData()
    {
        RptFee.report_fee_invoiceDataTable dt = new RptFee.report_fee_invoiceDataTable();
        RptFeeTableAdapters.report_fee_invoiceTableAdapter ta = new RptFeeTableAdapters.report_fee_invoiceTableAdapter();

        dt = ta.GetDataById(Convert.ToInt32(currentID));
        
        uiDate.SetCalendarValue(dt[0].tgl_pembayaran.ToString("dd-MMM-yyyy"));

    }
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        RptFee.report_fee_invoiceDataTable dt = new RptFee.report_fee_invoiceDataTable();
        RptFeeTableAdapters.report_fee_invoiceTableAdapter ta = new RptFeeTableAdapters.report_fee_invoiceTableAdapter();

        dt = ta.GetDataById(Convert.ToInt32(currentID));
        dt[0].tgl_pembayaran = Convert.ToDateTime(uiDate.Text);
        dt[0].status = "0";
        ta.Update(dt);
        Response.Redirect("ReportFee.aspx");
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReportFee.aspx");
    }
}