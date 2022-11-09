using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Withdrawal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            Search();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
        
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        uiDgUser.Attributes.Add("style", "display:none");
        uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
        uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

        uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptWithdrawal";

        List<ReportParameter> rp = new List<ReportParameter>();

        var exchangeReff = "0";
        var businessDate = "-";
        var seller = " ";
        if (uiExchReff.Text != "")
        {
            exchangeReff = uiExchReff.Text;
        }
        if (uiSeller.Text != "")
        {
            seller = uiSeller.Text;
        }
        if (CtlCalendarPickUpBusinessDate.Text != "")
        {
            businessDate = CtlCalendarPickUpBusinessDate.Text;
        }
        // Set parameter reportName
        rp.Add(new ReportParameter("BusinessDate", new string[] { businessDate }));
        rp.Add(new ReportParameter("exchangeReff", new string[] { exchangeReff }));
        rp.Add(new ReportParameter("seller", new string[] { seller }));

        uiRptViewer.ServerReport.SetParameters(rp);

        //byte[] result = null;
        //string mimeType = "";
        //string encoding = "";
        //string filenameExtension = "";
        //string[] streamids = null;
        //Warning[] warnings = null;

        //result = uiRptViewer.ServerReport.Render("WORD", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

        //string fileName = "Report Withdrawal " + (DateTime.Now).ToString("dd MMM yyyy") + ".doc";

        //System.IO.FileStream file = System.IO.File.Create(HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
        //file.Write(result, 0, result.Length);
        //file.Close();
        


        //string path = HttpContext.Current.Server.MapPath("~\\Report\\{0}");

        //string filePath = string.Format(path, fileName);


        //WebClient req = new WebClient();
        //HttpResponse response = HttpContext.Current.Response;
        //response.Clear();
        //Response.ContentType = "application/vnd.ms-word";
        //response.ClearContent();
        //response.ClearHeaders();
        //response.Buffer = true;
        //response.AddHeader("Content-Disposition", "attachment;filename="+ fileName);
        //byte[] data = File.ReadAllBytes(filePath);
        //response.BinaryWrite(data);
        //response.End();
        uiRptViewer.ServerReport.Refresh();

    }
    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {
        if (IsValidUpload())
        {
            try
            {
                List<data_csv> dataCsv = new List<data_csv>();
                CultureInfo culture = new CultureInfo("en-US");
                if (Path.GetExtension(uiUploadFile.PostedFile.FileName).Substring(1) == "csv")
                {
                    HttpPostedFile upload = uiUploadFile.PostedFile;
                    //int i = 0;
                    using (StreamReader csvReader = new StreamReader(upload.InputStream))
                    {
                        while (!csvReader.EndOfStream)
                        {
                            //var line = csvReader.ReadLine();
                            //var values = line.Split(';');
                            //if (i >= 1)
                            //{
                            //    dataCsv.Add(new data_csv
                            //    {
                            //        AccountNo = (values[0].ToString()),
                            //        Date = (values[2].ToString().Remove(values[2].Length - 9)),
                            //        ValDate = (values[2].ToString().Remove(values[2].Length - 9)),
                            //        //TransactionCode = (rows[4].ToString()),
                            //        Description1 = (values[3].ToString()),
                            //        Description2 = (values[4].ToString()),
                            //        //ReferenceNo = (rows[7].ToString()),
                            //        Debit = (values[6].ToString()),
                            //        Credit = (values[5].ToString()),
                            //    });
                            //}

                            //i++;

                            MatchCollection matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(csvReader.ReadLine());
                            string[] rows = new string[matches.Count + 1];
                            int i = 1;

                            foreach (var item in matches)
                            {
                                rows[i] = item.ToString();
                                i++;
                            }
                            i = 1;


                            dataCsv.Add(new data_csv
                            {
                                AccountNo = (rows[1].ToString()),
                                Date = (rows[2].ToString()),
                                ValDate = (rows[3].ToString()),
                                TransactionCode = (rows[4].ToString()),
                                Description1 = (rows[5].ToString()),
                                Description2 = (rows[6].ToString()),
                                ReferenceNo = (rows[7].ToString()),
                                Debit = (rows[8].ToString()),
                                Credit = (rows[9].ToString()),
                            });
                        }
                    }
                    var dt = new BankTransferTableAdapters.Bank_TransferTableAdapter();
                    int j = 0;
                    foreach (var item in dataCsv)
                    {
                        if (j > 0)
                        {
                            Decimal number_credit = Convert.ToDecimal(item.Credit.Replace(",", string.Empty));

                            if (number_credit > 0)
                            {
                                DateTime date = new DateTime();
                                var date1 = DateTime.Now.ToString("dd/MM/yy");
                                date = DateTime.ParseExact(item.Date, "dd/MM/yy", CultureInfo.InvariantCulture);
                                var dr = dt.GetDataByFilter(date.ToString("yyyy-MMM-dd"), item.Description2, number_credit);
                                if (dr.Count == 0)
                                {
                                    dt.Insert(date, item.Description1, item.Description2, number_credit, DateTime.Now, "");
                                }
                            }
                        }
                        j++;
                    }
                    j = 0;
                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }
                else
                {
                    uiBLError.Items.Add("Format not csv file");
                    uiBLError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
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
    private void Search()
    {
        try
        {
            var dr = new BankTransfer.Bank_TransferDataTable();
            var dt = new BankTransferTableAdapters.Bank_TransferTableAdapter();
            dr = dt.GetDataBy();

            DataView dv = new DataView(dr);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgUser.DataSource = dv;
            uiDgUser.DataBind();

            dr.Dispose();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiDgRptFee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label amount = (Label)e.Row.FindControl("amount");
            amount.Text = (decimal.Parse(amount.Text)).ToString("#,##0.00");
        }
    }
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
    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
    }
    public class data_csv
    {
        public string AccountNo { get; set; }
        public string Date { get; set; }
        public string ValDate { get; set; }
        public string TransactionCode { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string ReferenceNo { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }

    }

}