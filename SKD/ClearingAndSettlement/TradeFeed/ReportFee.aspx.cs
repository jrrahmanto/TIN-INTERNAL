using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_ReportFee : System.Web.UI.Page
{
    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void uiDgUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgUser.PageIndex = e.NewPageIndex;
        FillUserDataGrid();
    }
    private void FillUserDataGrid()
    {
        Search();
    }

    protected void uiDgUser_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (string.IsNullOrEmpty(SortOrder))
        {
            SortOrder = e.SortExpression + " " + "DESC";
        }
        else
        {
            string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
            if (arrSortOrder[1] == "ASC")
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else if (arrSortOrder[1] == "DESC")
            {
                SortOrder = e.SortExpression + " " + "ASC";
            }
        }

        FillUserDataGrid();
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }
    protected void uiBtnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            var name = uiName.Text;
            if (uiName.Text == "")
            {
                name = " ";
            }
            var status = uiDropdown2.Value;
            var status2 = uiDropdown2.Value;
            if (status == "2")
            {
                status = "1";
                status2 = "0";
            }
            var inv = uiInv.Text;
            if (uiInv.Text == "")
            {
                inv = "/";
            }

            if (uiDropdown.Value != "e")
            {
                uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Rpt Fee By Seller Buyer";

                List<ReportParameter> rp = new List<ReportParameter>();

                rp.Add(new ReportParameter("start", uiStart.Text));
                rp.Add(new ReportParameter("end", uiEnd.Text));
                rp.Add(new ReportParameter("nama", name));
                rp.Add(new ReportParameter("status", status));
                rp.Add(new ReportParameter("pelaku", uiDropdown.Value));
                rp.Add(new ReportParameter("status2", status2));
                rp.Add(new ReportParameter("no_inv", inv));

                uiRptViewer.ServerReport.SetParameters(rp);
                uiRptViewer.ServerReport.Refresh();
            }
            else
            {
                uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Rpt Fee";

                List<ReportParameter> rp = new List<ReportParameter>();

                rp.Add(new ReportParameter("start", uiStart.Text));
                rp.Add(new ReportParameter("end", uiEnd.Text));
                rp.Add(new ReportParameter("nama", name));
                rp.Add(new ReportParameter("status", status));
                rp.Add(new ReportParameter("status2", status2));
                rp.Add(new ReportParameter("no_inv", inv));

                uiRptViewer.ServerReport.SetParameters(rp);
                uiRptViewer.ServerReport.Refresh();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }
    private void Search()
    {
        try
        {
            RptFee.report_fee_invoiceDataTable dt = new RptFee.report_fee_invoiceDataTable();
            var pelaku = uiDropdown.Value;
            if (uiDropdown.Value == "All")
            {
                pelaku = "e";
            }
            var status = uiDropdown2.Value;
            var status2 = uiDropdown2.Value;
            if (status == "2")
            {
                status = "1";
                status2 = "0";
            }
            dt = ReportFee.GetRptFeeSearch(uiName.Text.ToString(), DateTime.Parse(uiStart.Text), DateTime.Parse(uiEnd.Text), pelaku, status, status2, uiInv.Text);

            DataView dv = new DataView(dt);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgUser.DataSource = dv;
            uiDgUser.DataBind();

            dt.Dispose();
        }
        catch (Exception ex)
        {
            // DisplayErrorMessage(ex);
        }
    }
    protected void uiDgRptFee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        RptFee.report_fee_invoiceDataTable dt = new RptFee.report_fee_invoiceDataTable();
        RptFeeTableAdapters.report_fee_invoiceTableAdapter ta = new RptFeeTableAdapters.report_fee_invoiceTableAdapter();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label uiID = (Label)e.Row.FindControl("uiID");
            dt = ta.GetDataById(Convert.ToInt32(uiID.Text));

            Label price = (Label)e.Row.FindControl("uiLblPrice");
            price.Text = decimal.Parse(price.Text).ToString("#,##0.00");

            Label uiTrxFeeBeforePpn = (Label)e.Row.FindControl("uiTrxFeeBeforePpn");
            uiTrxFeeBeforePpn.Text = Math.Round(decimal.Parse(uiTrxFeeBeforePpn.Text)).ToString("#,##0.00");

            Label uiTRXFEEDOLLARBeforePpn = (Label)e.Row.FindControl("uiTRXFEEDOLLARBeforePpn");
            uiTRXFEEDOLLARBeforePpn.Text =  decimal.Parse(uiTRXFEEDOLLARBeforePpn.Text).ToString("#,##0.00");

            Label uippn = (Label)e.Row.FindControl("uippn");
            if (Math.Round(decimal.Parse(uiTrxFeeBeforePpn.Text)) == 0)
            {
                uippn.Text = decimal.Parse(uippn.Text).ToString("#,##0.00");
            }
            else
            {
                uippn.Text = Math.Round(decimal.Parse(uippn.Text)).ToString("#,##0.00");
            }
            

            Label uiTrxFee = (Label)e.Row.FindControl("uiTrxFee");
            if (decimal.Parse(uiTrxFee.Text) == 0)
            {
                uiTrxFee.Text = Math.Round((decimal.Parse(uiTrxFee.Text))).ToString("#,##0.00");
            }
            else
            {
                uiTrxFee.Text = (Math.Round(decimal.Parse(uiTrxFee.Text)) + Math.Round(decimal.Parse(uippn.Text))).ToString("#,##0.00");
            }


            Label uiTRXFEEDOLLAR = (Label)e.Row.FindControl("uiTRXFEEDOLLAR");
            if (decimal.Parse(uiTRXFEEDOLLAR.Text) == 0)
            {
                uiTRXFEEDOLLAR.Text = (decimal.Parse(uiTRXFEEDOLLAR.Text)).ToString("#,##0.00");

            }
            else
            {
                uiTRXFEEDOLLAR.Text = (decimal.Parse(uiTRXFEEDOLLAR.Text) + decimal.Parse(uippn.Text)).ToString("#,##0.00");
            }

            CheckBox cb = (CheckBox)e.Row.FindControl("cbSelect");
            if (dt[0].status == "0")
            {
                cb.Checked = true;
            }

            TextBox tb = (TextBox)e.Row.FindControl("tgl_pembayaran");
            if (dt[0].tgl_pembayaran != Convert.ToDateTime("1900-01-01"))
            {
                tb.Text = (dt[0].tgl_pembayaran).ToString("MM-dd-yyyy");
            }
        }
    }
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        RptFee.report_fee_invoiceDataTable dt = new RptFee.report_fee_invoiceDataTable();
        RptFeeTableAdapters.report_fee_invoiceTableAdapter ta = new RptFeeTableAdapters.report_fee_invoiceTableAdapter();

        try
        {
            foreach (GridViewRow row in uiDgUser.Rows)
            {
                string id = (row.FindControl("uiID") as Label).Text;
                string tgl_pembayaran = (row.FindControl("tgl_pembayaran") as TextBox).Text;
                CheckBox cb = (CheckBox)row.FindControl("cbSelect");
                string status = "1";
                if (cb.Checked == true)
                {
                    status = "0";
                }

                dt = ta.GetDataById(Convert.ToInt32(id));
                if (tgl_pembayaran != "")
                {
                    dt[0].tgl_pembayaran = Convert.ToDateTime(tgl_pembayaran);
                }
                else
                {
                    dt[0].tgl_pembayaran = Convert.ToDateTime("1900-01-01");
                }
                dt[0].status = status;
                ta.Update(dt);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error '" + ex.Message + ")", true);
        }

    }
    protected void uiBtnJournal_Click(object sender, EventArgs e)
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();
        //get rdlc for seller
        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Journal Rpt Fee";

        List<ReportParameter> rp = new List<ReportParameter>();

        // Set parameter reportName
        var name = uiName.Text;
        if (uiName.Text == "")
        {
            name = " ";
        }
        var status = uiDropdown2.Value;
        var status2 = uiDropdown2.Value;
        if (status == "2")
        {
            status = "1";
            status2 = "0";
        }
        var inv = uiInv.Text;
        if (uiInv.Text == "")
        {
            inv = "/";
        }

        rp.Add(new ReportParameter("Tanggal", uiStart.Text));
        rp.Add(new ReportParameter("end", uiEnd.Text));
        rp.Add(new ReportParameter("nama_var", name));
        rp.Add(new ReportParameter("inv_number", inv));
        uiRptViewer.ServerReport.SetParameters(rp);
        // Variables
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;
        byte[] bytes = uiRptViewer.ServerReport.Render("EXCEL", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

        // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
        Response.Buffer = true;
        Response.Clear();
        Response.ContentType = mimeType;
        Response.AddHeader("content-disposition", "attachment; filename=Journal Pembayaran." + extension);
        Response.BinaryWrite(bytes); // create the file
        Response.Flush(); // send it to the client to download
    }
}