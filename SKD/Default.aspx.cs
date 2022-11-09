using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NotificationDataTableAdapters.QueriesTableAdapter queryTa = new NotificationDataTableAdapters.QueriesTableAdapter();

        uiLblCounterCM.Text = "" + queryTa.CalcCounterCM();
        uiLblCounterBA.Text = "" + queryTa.CalcCounterBA();
        uiLblCounterBT.Text = "" + queryTa.CalcCounterBT();
        uiLblCounterTF.Text = "" + queryTa.CalcCounterTF();
    }
}
