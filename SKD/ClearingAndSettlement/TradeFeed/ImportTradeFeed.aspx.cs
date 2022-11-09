using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class ClearingAndSettlement_TradeFeed_ImportTradeFeed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewTradeFeed.aspx");
    }

    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {

        if (IsValidUpload())
        {
            uiTxbLog.Text = "";

            try
            {
                uiUploadFile.SaveAs(string.Format("{0}{1}", 
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(), 
                    uiUploadFile.FileName));

                ImportTradeFeed imp = new ImportTradeFeed(typeof(ClsImportTradeFeed),
                                        User.Identity.Name,
                                        Convert.ToDecimal(uiDdlExchange.SelectedValue),
                                        DateTime.Parse(uiDtpBussDate.Text));
                setMessage("Import Started at " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));

                imp.RowError += new ImportHandler.RowErrorEventHandler(RowErrorHandler);
                imp.Filename = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(), uiUploadFile.FileName);
                imp.IsImportParse = false;
                imp.Import();

                ApplicationLog.Insert(DateTime.Now, "Import TradeFeed", "I", "Import Tradefeed", User.Identity.Name, Common.GetIPAddress(this.Request));
                setMessage("Import Finished at " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
            }
            catch (Exception ex)
            {
                ApplicationLog.Insert(DateTime.Now, "Import TradeFeed", "E", "Import Tradefeed Failed", User.Identity.Name, Common.GetIPAddress(this.Request));
                uiTxbLog.Text = ex.Message;
            }

            try
            {
                File.Delete(string.Format("{0}{1}",
                   System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(),
                   uiUploadFile.FileName));
            }
            catch (Exception ex)
            {	
                throw new ApplicationException("Failed to delete temporary file.", ex);
            }
        }
       
    }

    private void RowErrorHandler(object sender, RowErrorEventArgs e)
    {
        setMessage(e.ErrMsg + " - " + e.Line);
    }

    private bool IsValidUpload()
    {
        bool isValid = true;
        uiBLError.Visible = false;  
        uiBLError.Items.Clear();

        if (!uiUploadFile.HasFile)
        {
            uiBLError.Items.Add("File not found.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void setMessage(String msg)
    {
        String log = uiTxbLog.Text + "\n";

        StringBuilder sb = new StringBuilder();
        sb.Append(log + msg);

        uiTxbLog.Text = sb.ToString();
    }
}
