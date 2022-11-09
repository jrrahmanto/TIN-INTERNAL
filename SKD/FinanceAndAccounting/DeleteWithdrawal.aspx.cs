using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_DeleteWithdrawal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var dr = new BankTransferTableAdapters.Rpt_WithdrawalTableAdapter();
        var dt = dr.GetDataById(Convert.ToInt32(currentID));
        dt[0].deleted = 1;
        dr.Update(dt);
        Page.Response.Redirect("/FinanceAndAccounting/DetailWithdrawal.aspx?id=" + dt[0].id_Bank_Transfer + "");
    }
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }
}