using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class AuditAndCompliance_ImportMBD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {

        ImportMBD imp = new ImportMBD(typeof(ClsImportTradeFeed),
                                    User.Identity.Name,
                                    Convert.ToDecimal(uiCtlCm.LookupTextBoxID),
                                    DateTime.Parse(uiDtpBussDate.Text));
        setMessage("Import Started at " + DateTime.Now.ToString());
        imp.Filename = uiUploadFile.PostedFile.FileName;
        imp.IsImportParse = false;
        imp.Import();
        uiTxbLog.Text = "";
        imp.RowError += new ImportHandler.RowErrorEventHandler(RowErrorHandler);
        setMessage("Import Finished at " + DateTime.Now.ToString());
    }

    private void RowErrorHandler(object sender, RowErrorEventArgs e)
    {
        setMessage(e.ErrMsg + " - " + e.Line);
    }
    private void setMessage(String msg)
    {
        String log = uiTxbLog.Text + "\n";

        StringBuilder sb = new StringBuilder();
        sb.Append(log + msg);

        uiTxbLog.Text = sb.ToString();
    }
}
