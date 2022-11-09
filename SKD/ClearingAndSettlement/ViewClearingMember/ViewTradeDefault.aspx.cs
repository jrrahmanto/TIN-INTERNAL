using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using System.Drawing;


public partial class ClearingAndSettlement_ViewClearingMember_ViewTradeDefault : System.Web.UI.Page
{
    string currentParticipant =string.Empty;
    decimal subTotal = 0;
    decimal total = 0;
    int subTotalRowIndex = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            // load contract type

            TradeDefaultData.DdlContractTypeDataTable dtContractType = TradeDefault.GetContractType();

            foreach (TradeDefaultData.DdlContractTypeRow item in dtContractType)
            {
                uiDdlContractType.Items.Add(new ListItem(item.ContractType, item.ContractType));
            }
        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsValidGet())
            //{
            FillTradeDefault();

            //}
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
    }

   
   

   
    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for start date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            uiBLError.Items.Add("Start Date is required.");
        }

                // Display if only error exist       
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
    private void FillTradeDefault()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/SPPK_KBI_ReportEOD/RptTradeDefault";

        List<ReportParameter> rp = new List<ReportParameter>();

        //if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        //{
        //    rp.Add(new ReportParameter("businessDate", new string[] { null }));
        //}
        //else
        //{
        rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpStartDate.Text }));
        //}
        if (string.IsNullOrEmpty(uiDdlContractType.Text))
        {
            rp.Add(new ReportParameter("contractType", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("contractType", new string[] { uiDdlContractType.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }


}
