using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

public partial class MainPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["ListReportFile"] = Server.MapPath("~/App_Data/ListReports.xml");

            FillReportDropDown(uiDdlRpt1);
            FillReportDropDown(uiDdlRpt2);
            FillReportDropDown(uiDdlRpt3);
            FillReportDropDown(uiDdlRpt4);
        }

    }
    
    protected void uiDdlRpt1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt1.SelectedValue))
        {
            uiRptViewer1.Visible = true;
            uiRptViewer1.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer1.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            uiRptViewer1.ServerReport.ReportPath = uiDdlRpt1.SelectedValue;

            ReportParameterInfoCollection rpiCol = uiRptViewer1.ServerReport.GetParameters();
            List<ReportParameter> rp = new List<ReportParameter>();

            foreach (ReportParameterInfo rpi in rpiCol)
            {
                if (rpi.Name == "businessDate")
                {
                    rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Now.ToString("dd/MM/yyyy") }));
                    break;
                }
            }

            uiRptViewer1.ServerReport.SetParameters(rp);

            uiRptViewer1.ServerReport.Refresh();
        }        
    }

    protected void uiDdlRpt2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt2.SelectedValue))
        {
            uiRptViewer2.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer2.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            uiRptViewer2.ServerReport.ReportPath = uiDdlRpt2.SelectedValue;

            ReportParameterInfoCollection rpiCol = uiRptViewer2.ServerReport.GetParameters();
            List<ReportParameter> rp = new List<ReportParameter>();

            foreach (ReportParameterInfo rpi in rpiCol)
            {
                if (rpi.Name == "businessDate")
                {
                    rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Now.ToString("dd/MM/yyyy") }));
                    break;
                }
            }

            uiRptViewer2.ServerReport.SetParameters(rp);

            uiRptViewer2.ServerReport.Refresh();
        }   
    }

    protected void uiDdlRpt3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt3.SelectedValue))
        {
            uiRptViewer3.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer3.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            uiRptViewer3.ServerReport.ReportPath = uiDdlRpt3.SelectedValue;

            ReportParameterInfoCollection rpiCol = uiRptViewer3.ServerReport.GetParameters();
            List<ReportParameter> rp = new List<ReportParameter>();

            foreach (ReportParameterInfo rpi in rpiCol)
            {
                if (rpi.Name == "businessDate")
                {
                    rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Now.ToString("dd/MM/yyyy") }));
                    break;
                }
            }

            uiRptViewer3.ServerReport.SetParameters(rp);

            uiRptViewer3.ServerReport.Refresh();
        }   
    }

    protected void uiDdlRpt4_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt4.SelectedValue))
        {
            uiRptViewer4.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer4.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            uiRptViewer4.ServerReport.ReportPath = uiDdlRpt4.SelectedValue;

            ReportParameterInfoCollection rpiCol = uiRptViewer4.ServerReport.GetParameters();
            List<ReportParameter> rp = new List<ReportParameter>();

            foreach (ReportParameterInfo rpi in rpiCol)
            {
                if (rpi.Name == "businessDate")
                {
                    rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Now.ToString("dd/MM/yyyy") }));
                    break;
                }
            }

            uiRptViewer4.ServerReport.SetParameters(rp);

            uiRptViewer4.ServerReport.Refresh();
        }   
    }

    private void FillReportDropDown(DropDownList ddl)
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceRpt1.Select();
        DataView dve = (DataView)dv;

        dve.RowFilter = "Type = 'Dashboard'";

        ddl.DataSource = dve;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "Path";
        ddl.DataBind();
    }
    
}
