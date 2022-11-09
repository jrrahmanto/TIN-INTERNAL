using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;

public partial class WebUI_frmImportCtl : System.Web.UI.UserControl
{
    public event EventHandler Import;
    public event EventHandler Cancel;

    public TextBox LogTextBox
    {
        get { return uiTxtLog; }
    }

    public FileUpload FileUpload
    {
        get { return uiFU; }
    }

    public string Filename
    {
        get { return uiFU.FileName; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Thread.Sleep(3000);

        Cancel(this, EventArgs.Empty);
    }

    protected void uiBtnImport_Click(object sender, EventArgs e)
    {
        uiTxtLog.Text = "";

        if (uiFU.HasFile == false)
        { 
        
        }

        Thread.Sleep(3000);
       
        Import(this, EventArgs.Empty);
    }

    public void SetMessage(String msg)
    {
        String log = uiTxtLog.Text + "\n";

        StringBuilder sb = new StringBuilder();
        sb.Append(log + msg);

        uiTxtLog.Text = sb.ToString();
    }
}
