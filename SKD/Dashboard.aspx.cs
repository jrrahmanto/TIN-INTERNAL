using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Collections;
using System.Data;

public partial class Dashboard : System.Web.UI.Page
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


            LoadDashboard();
        }
    }

    protected void uiDdlRpt1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt1.SelectedValue))
        {
            SetReport1(uiDdlRpt1.SelectedValue);

            ProcessDashboard();
        }
    }

    protected void uiDdlRpt2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt2.SelectedValue))
        {
            SetReport2(uiDdlRpt2.SelectedValue);

            ProcessDashboard();
        }
    }

    protected void uiDdlRpt3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt3.SelectedValue))
        {
            SetReport3(uiDdlRpt3.SelectedValue);

            ProcessDashboard();
        }
    }

    protected void uiDdlRpt4_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(uiDdlRpt4.SelectedValue))
        {
            SetReport4(uiDdlRpt4.SelectedValue);

            ProcessDashboard();
        }
    }

    private void FillReportDropDown(DropDownList ddl)
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceRpt1.Select();
        DataView dve = (DataView)dv;

        dve.RowFilter = "Type = 'Dashboard'";
        dve.Sort = "Name Asc";
        ddl.DataSource = dve;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "Path";
        ddl.DataBind();
    }

    private void ProcessDashboard()
    {
        DashboardHandle.ProcessDashboard(Session["UserName"].ToString(), uiDdlRpt1.SelectedValue, 
            uiDdlRpt2.SelectedValue, uiDdlRpt3.SelectedValue, uiDdlRpt4.SelectedValue);
    }

    private void LoadDashboard()
    {
        DashboardData.DashboardRow dr =  
            DashboardHandle.GetDashboardByUserName(Session["UserName"].ToString());

        if (dr != null)
        {
            if (!string.IsNullOrEmpty(dr.Report1))
            {
                uiDdlRpt1.SelectedValue = dr.Report1;

                SetReport1(dr.Report1);
            }
            if (!string.IsNullOrEmpty(dr.Report2))
            {
                uiDdlRpt2.SelectedValue = dr.Report2;

                SetReport2(dr.Report2);
            }
            if (!string.IsNullOrEmpty(dr.Report3))
            {
                uiDdlRpt3.SelectedValue = dr.Report3;

                SetReport3(dr.Report3);

            }
            if (!string.IsNullOrEmpty(dr.Report4))
            {
                uiDdlRpt4.SelectedValue = dr.Report4;

                SetReport4(dr.Report4);
            }
        }
    }

    private void SetReport1(string report1)
    {
        uiRptViewer1.Visible = true;
        uiRptViewer1.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer1.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer1.ServerReport.ReportPath = report1;

        ReportParameterInfoCollection rpiCol = uiRptViewer1.ServerReport.GetParameters();
        List<ReportParameter> rp = new List<ReportParameter>();

        foreach (ReportParameterInfo rpi in rpiCol)
        {
            if (rpi.Name == "businessDate")
            {
                rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "startBusinessDate")
            {
                rp.Add(new ReportParameter("startBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "endBusinessDate")
            {
                rp.Add(new ReportParameter("endBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "topSelect")
            {
                rp.Add(new ReportParameter("topSelect", new string[] { "10" }));
            }
        }

        uiRptViewer1.ServerReport.SetParameters(rp);

        uiRptViewer1.ServerReport.Refresh();
    }

    private void SetReport2(string report2)
    {
        uiRptViewer2.Visible = true;
        uiRptViewer2.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer2.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer2.ServerReport.ReportPath = report2;

        ReportParameterInfoCollection rpiCol = uiRptViewer2.ServerReport.GetParameters();
        List<ReportParameter> rp = new List<ReportParameter>();

        foreach (ReportParameterInfo rpi in rpiCol)
        {
            if (rpi.Name == "businessDate")
            {
                rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "startBusinessDate")
            {
                rp.Add(new ReportParameter("startBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "endBusinessDate")
            {
                rp.Add(new ReportParameter("endBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "topSelect")
            {
                rp.Add(new ReportParameter("topSelect", new string[] { "10" }));
            }
        }

        uiRptViewer2.ServerReport.SetParameters(rp);

        uiRptViewer2.ServerReport.Refresh();
    }

    private void SetReport3(string report3)
    {
        uiRptViewer3.Visible = true;
        uiRptViewer3.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer3.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer1.ServerReport.ReportPath = report3;

        ReportParameterInfoCollection rpiCol = uiRptViewer3.ServerReport.GetParameters();
        List<ReportParameter> rp = new List<ReportParameter>();

        foreach (ReportParameterInfo rpi in rpiCol)
        {
            if (rpi.Name == "businessDate")
            {
                rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "startBusinessDate")
            {
                rp.Add(new ReportParameter("startBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "endBusinessDate")
            {
                rp.Add(new ReportParameter("endBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "topSelect")
            {
                rp.Add(new ReportParameter("topSelect", new string[] { "10" }));
            }
        }

        uiRptViewer3.ServerReport.SetParameters(rp);

        uiRptViewer3.ServerReport.Refresh();
    }

    private void SetReport4(string report4)
    {
        uiRptViewer4.Visible = true;
        uiRptViewer4.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer4.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        uiRptViewer4.ServerReport.ReportPath = report4;

        ReportParameterInfoCollection rpiCol = uiRptViewer4.ServerReport.GetParameters();
        List<ReportParameter> rp = new List<ReportParameter>();

        foreach (ReportParameterInfo rpi in rpiCol)
        {
            if (rpi.Name == "businessDate")
            {
                rp.Add(new ReportParameter("businessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "startBusinessDate")
            {
                rp.Add(new ReportParameter("startBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "endBusinessDate")
            {
                rp.Add(new ReportParameter("endBusinessDate", new string[] { DateTime.Parse(Session["BusinessDate"].ToString()).ToString("yyyy-MM-dd") }));
            }
            if (rpi.Name == "topSelect")
            {
                rp.Add(new ReportParameter("topSelect", new string[] { "10" }));
            }
        }

        uiRptViewer4.ServerReport.SetParameters(rp);

        uiRptViewer4.ServerReport.Refresh();
    }
}
