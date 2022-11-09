using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_MasterData_EntryManageBroadcast : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
   
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (uiTxbBroadcastMessage.Text != "")
            {
                EventLog.SubmitBroadcast(uiTxbBroadcastMessage.Text, this.Request);
            }
            uiBlError.Items.Clear();
            uiBlError.Visible = false;
            uiLblSuccess.Text = "Message successfully broadcasted";
            uiLblSuccess.Visible = true;
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
            uiLblSuccess.Visible = false;
        }
    }
}
