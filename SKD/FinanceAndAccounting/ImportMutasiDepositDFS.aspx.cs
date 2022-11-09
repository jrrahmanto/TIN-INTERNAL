using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_ImportMutasiDepositDFS : System.Web.UI.Page
{
    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uiBLError.Visible = false;
            uiBLError.Items.Clear();

            if (!String.IsNullOrEmpty(Session["BusinessDate"] as string))
            {
                string currentBusinessDate = Session["BusinessDate"] as string;
                CtlCalendarBusinessDate.SetCalendarValue(DateTime.Parse(currentBusinessDate).ToString("dd-MMM-yyyy"));
            }
        }
        catch (Exception ex)
        {
            setMessage(ex.Message);
            setMessage(ex.StackTrace);
        }
    }
    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {
        if (IsValidUpload())
        {
            try
            {
                uiTxbLog.Text = "";

                uiUploadFile.SaveAs(string.Format("{0}{1}",
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(),
                    uiUploadFile.FileName));

                ImportMutationDepositDFS  imp = new ImportMutationDepositDFS(typeof(ClsImportMutationDeposit), User.Identity.Name, DateTime.Parse(CtlCalendarBusinessDate.Text), MutationDeposit.GetMaxMutationNo());
                setMessage("Import Started at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"));

                imp.RowError += new ImportHandler.RowErrorEventHandler(RowErrorHandler);
                imp.Filename = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(),
                    uiUploadFile.FileName);
                imp.IsImportParse = false;
                imp.Import();

                File.Delete(string.Format("{0}{1}",
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(),
                    uiUploadFile.FileName));

                setMessage("Import Finished at " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    setMessage("Records are already exist. Please import new record.");
                }
                else
                {
                    setMessage(ex.Message);
                    setMessage(ex.StackTrace);
                }
            }
        }
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
        else
        {
            FileInfo fi = new FileInfo(uiUploadFile.FileName);
            if (fi.Extension.ToLower() != ".xls")
            {
                uiBLError.Items.Add("Only excel file is allowed for import");
            }
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (Menu == "hide")
        {
            Response.Redirect("ViewMutasiDepositDFS.aspx?menu=hide");
        }
        else
        {
            Response.Redirect("ViewMutasiDepositDFS.aspx");
        } 
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