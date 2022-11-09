using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryIjinUsaha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void uiBtnAktePerubahanAnggaranDasar_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryAktePerubahanAnggaranDasar.aspx");
    }
    protected void uiBtnManageLaporanAkuntanPublik_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryLaporanAkuntanPublik.aspx");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryRekening.aspx");
    }
}
