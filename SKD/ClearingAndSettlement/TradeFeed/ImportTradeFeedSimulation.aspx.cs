using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Microsoft.Reporting.WebForms;

public partial class ClearingAndSettlement_TradeFeed_ImportTradeFeedSimulation : System.Web.UI.Page
{
    bool dataValidated = false;
    DateTime bussDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dataValidated = false;
            uiBtnSimulate.Enabled = dataValidated;
            bussDate = DateTime.Parse(Session["BusinessDate"].ToString());
            bussDate = bussDate.AddDays(-1);
            uiDtBussDate.Text = bussDate.ToString("dd-MMM-yyyy");
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {

        if (IsValidUpload(false))
        {
            uiTxbLog.Text = "";

            try
            {
                uiUploadFile.SaveAs(string.Format("{0}{1}", 
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(), 
                    uiUploadFile.FileName));

                ImportTradeFeedSimulation imp = new ImportTradeFeedSimulation(typeof(ClsImportTradeFeed),
                                                    User.Identity.Name,
                                                    Convert.ToDecimal(uiDdlExchange.SelectedValue),
                                                    DateTime.Parse(uiDtBussDate.Text),
                                                    this.Session.SessionID);
                setMessage("Import Started at " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));

                imp.RowError += new ImportHandler.RowErrorEventHandler(RowErrorHandler);
                imp.Filename = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_UPLOAD].ToString(), uiUploadFile.FileName);
                imp.IsImportParse = false;
                imp.Import();

                //List<string> validationResult1 = TradefeedSimulation.ValidateContract();
                //List<string> validationResult2 = TradefeedSimulation.ValidateInvestor();

                ApplicationLog.Insert(DateTime.Now, "Import TradeFeed Simulation", "I", "Import Tradefeed Simulation", User.Identity.Name, Common.GetIPAddress(this.Request));
                //if (validationResult1.Count > 0)
                //{
                //    foreach (string s in validationResult1)
                //    {
                //        setMessage(s);
                //    }
                //}
                //if (validationResult2.Count > 0)
                //{
                //    foreach (string s in validationResult2)
                //    {
                //        setMessage(s);
                //    }
                //}
                //if (validationResult1.Count > 0 || validationResult2.Count > 0)
                //{
                //    dataValidated = false;
                //}
                //else
                //{
                //    dataValidated = true;
                //}

                //validate trade yang sudah di import jika false maka akan tampil laporan validate trade
                IsValidateTrade();
                setMessage("\nImport Finished at " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                uiRptViewer.Visible = false;
            }
            catch (Exception ex)
            {
                ApplicationLog.Insert(DateTime.Now, "Import TradeFeed Simulation", "E", "Import Tradefeed Simulation Failed", User.Identity.Name, Common.GetIPAddress(this.Request));
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

    private bool IsValidUpload(bool b)
    {
        bool isValid = true;
        uiBLError.Visible = false;  
        uiBLError.Items.Clear();

        if (b == true)
        {
            if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
            {
                uiBLError.Items.Add("Clearing Member can not be empty.");
            }
        }
            
        if (string.IsNullOrEmpty(uiDtBussDate.Text))
        {
            uiBLError.Items.Add("Business Date can not be empty.");
        }

        if (b == false)
        {
            if (!uiUploadFile.HasFile)
            {
                uiBLError.Items.Add("File not found.");
            }
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

    protected void uiBtnSimulate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidUpload(true))
            {
                uiRptViewer.Visible = true;
                ApplicationLog.Insert(DateTime.Now, "Report Tradefeed Simulation", "I", "Download", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
                GetReport();
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    private void IsValidateTrade()
    {
        string message = "";
        TradefeedSimulationData.SimulationValidateTradeDataTable dt = new TradefeedSimulationData.SimulationValidateTradeDataTable();
        dt = TradefeedSimulation.GetValidateTradeSimulation(Convert.ToDateTime(uiDtBussDate.Text));

        if (dt.Count > 0)
        {
            message = string.Format("\r \n{0} Errors found. \r", dt.Count.ToString());
            for (int i = 0; i < dt.Count; i++)
            {
                message = message + "Tradefeed ID : " + dt.Rows[i]["TradeFeedID"].ToString();
                message = message + " - Error Message : " + dt.Rows[i]["ErrorValidationResult"].ToString();
                message = message + "\r";             
            }
            uiTxbLog.Text += message;

            TradefeedSimulation.Delete(); //if error then delete all data
            uiBtnSimulate.Enabled = false;
        }
        else
        {
            uiBtnSimulate.Enabled = true;
            //TODO: 
        }
    }

    private void GetReport()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SKDReport/RptTradefeedSimulationResult";

        List<ReportParameter> rp = new List<ReportParameter>();

        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            rp.Add(new ReportParameter("cmid", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("cmid", new string[] { CtlClearingMemberLookup1.LookupTextBoxID }));
        }

        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBox))
        {
            rp.Add(new ReportParameter("cmCode", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("cmCode", new string[] { CtlClearingMemberLookup1.LookupTextBox }));
        }

        if (string.IsNullOrEmpty(uiDtBussDate.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { uiDtBussDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }

    protected void uiBtnClean_Click(object sender, EventArgs e)
    {
        TradefeedSimulation.Delete();
    }
}
