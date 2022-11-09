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


public partial class ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodity : System.Web.UI.Page
{
    string currentParticipant =string.Empty;
    decimal subTotal = 0;
    decimal total = 0;
    int subTotalRowIndex = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsValidGet())
            //{
            FillWithdrawalGrid();

            //}
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
    }

    protected void uiDgvWithDrawal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgvWithDrawal.PageIndex = e.NewPageIndex;
        FillWithdrawalGrid();
    }


    protected void uiDgvWithDrawal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            subTotal = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dt = (e.Row.DataItem as DataRowView).DataView.Table;
                string participant = Convert.ToString(dt.Rows[e.Row.RowIndex]["Participant"]);
                
                //Count Total
                //if (dt.Rows[e.Row.RowIndex]["Amount"] == DBNull.Value)
                //{
                //    total += 0;
                //}
                //else
                //{
                //    total += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["Amount"]);
                //}
                
                if (participant != currentParticipant)
                {
                    if (e.Row.RowIndex > 0)
                    {
                        for (int i = subTotalRowIndex; i < e.Row.RowIndex; i++)
                        {
                            //string a = uiDgvWithDrawal.Rows[i].Cells[7].Text;
                            if (uiDgvWithDrawal.Rows[i].Cells[7].Text== "&nbsp;")
                            {
                                subTotal += 0;
                            }
                            else
                            {
                                subTotal += Convert.ToDecimal(uiDgvWithDrawal.Rows[i].Cells[7].Text);
                            }
                            
                        }
                        this.AddTotalRow("Total", subTotal.ToString("N0"));
                        subTotalRowIndex = e.Row.RowIndex;
                    }
                    currentParticipant = participant;
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }

    protected void uiDgvWithDrawal_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = subTotalRowIndex; i < uiDgvWithDrawal.Rows.Count; i++)
            {
                subTotal += Convert.ToDecimal(uiDgvWithDrawal.Rows[i].Cells[7].Text);
            }
            this.AddTotalRow("Total", subTotal.ToString("N0"));
            //this.AddTotalRow("Total", total.ToString("N2"));
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }


    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        ViewRptWithDrawal();
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
    private void FillWithdrawalGrid()
    {
        uiDgvWithDrawal.DataSource = odsWithdrawal;
        //IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        IEnumerable dv = odsWithdrawal.Select();
        DataView dve = (DataView)dv;

        //if (!string.IsNullOrEmpty(SortOrder))
        //{
        //    dve.Sort = SortOrder;
        //}

        uiDgvWithDrawal.DataSource = dve;
        uiDgvWithDrawal.DataBind();
    }

    private void AddTotalRow(string labelText, string value)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
        row.Cells.AddRange(new TableCell[8] {   new TableCell (), new TableCell (),new TableCell (), new TableCell (), new TableCell (),new TableCell (), //Empty Cell
                                        new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right},
                                        new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Center} });

        uiDgvWithDrawal.Controls[0].Controls.Add(row);
    }

    private void ViewRptWithDrawal()
    {
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials =
                new ReportServerCredentials();

        
        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptWithDrawalSeller";

        List<ReportParameter> rp = new List<ReportParameter>();

        
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            rp.Add(new ReportParameter("businessDate", new string[] { null }));
        }
        else
        {
            rp.Add(new ReportParameter("businessDate", new string[] { CtlCalendarPickUpStartDate.Text }));
        }

        uiRptViewer.ServerReport.SetParameters(rp);

        uiRptViewer.ServerReport.Refresh();
    }




}
