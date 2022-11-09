using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.Configuration;
using System.Data.SqlClient;

public partial class ClearingAndSettlement_TradeFeed_NoticeOfShipment : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {

        }
    }
    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        Download();
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void uiBtnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/RptNoticeOfShipment";

            List<ReportParameter> rp = new List<ReportParameter>();
            LinkButton btn = (LinkButton)sender;

            string plaintext = btn.CommandArgument.ToString() + "-" + CtlCalendarPickUp1.Text;

            //BarcodeHelper.CreateQR(btn.CommandArgument.ToString(), plaintext);

            // Set parameter reportName             
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                rp.Add(new ReportParameter("NoSI", new string[] { null }));
            }
            else
            {
                rp.Add(new ReportParameter("NoSI", btn.CommandArgument.ToString()));
                rp.Add(new ReportParameter("FilePath", "D:\\QR\\" + btn.CommandArgument.ToString() + ".png"));
            }

            uiRptViewer.ServerReport.SetParameters(rp);
            uiRptViewer.ServerReport.Refresh();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    protected void uiBtnGenerateAll_Click(object sender, EventArgs e)
    {
        List<string> nosList = new List<string>();
        foreach (TradefeedData.TradeFeedRow tfr in Tradefeed.LoopNoticeOfShipment(DateTime.Parse(CtlCalendarPickUp1.Text), NoSI.Text))
        {
            string exchangeRef = tfr.ExchangeRef;
            string nosi = tfr.NoSI;

            ReportViewer rv = new ReportViewer();

            rv.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            rv.ServerReport.ReportServerCredentials = new ReportServerCredentials();

            rv.ServerReport.ReportPath = "/TIN_EOD_Report/RptNoticeOfShipment";

            List<ReportParameter> rp = new List<ReportParameter>();

            string plaintext = exchangeRef + "-" + CtlCalendarPickUp1.Text;
            ApplicationLog.Insert(DateTime.Now, "Create QR", "I", "CreateQR", User.Identity.Name, Common.GetIPAddress(this.Request));
            //BarcodeHelper.CreateQR(exchangeRef, plaintext);
            ApplicationLog.Insert(DateTime.Now, "Create QR", "I", "CreateQR2", User.Identity.Name, Common.GetIPAddress(this.Request));
            // Set parameter reportName
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                rp.Add(new ReportParameter("NoSI", new string[] { null }));
            }
            else
            {
                rp.Add(new ReportParameter("NoSI", nosi));
                rp.Add(new ReportParameter("FilePath", "D:\\QR\\QRCode\\" + exchangeRef + ".png"));
            }

            rv.ServerReport.SetParameters(rp);

            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, extension;
            byte[] bytes = rv.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            string pdfPath = @"F:\KBI\NoticeOfShipment\" + nosi.Replace("/", "_").Replace(".", "_").Replace(":", "_") + "." + extension;

            System.IO.FileStream pdfFile = new System.IO.FileStream(pdfPath, System.IO.FileMode.Create);
            pdfFile.Write(bytes, 0, bytes.Length);
            pdfFile.Close();

            if (!nosList.Contains(pdfPath))
            {
                nosList.Add(pdfPath);

                ClearingMemberData.ClearingMemberRow cmbRow = ClearingMember.getEmailBuyerWithExchange(exchangeRef);
                ClearingMemberData.ClearingMemberRow cmsRow = ClearingMember.getEmailSellerWithExchange(exchangeRef);

                SMTPHelper.SendInformasiNoticeOfShipmentForBuyerSeller(cmbRow.Email, pdfPath);
                SMTPHelper.SendInformasiNoticeOfShipmentForBuyerSeller(cmsRow.Email, pdfPath);
            }
        }

        TradefeedData.TradeFeedRow tfrw = Tradefeed.FillByNoticeOfShipment(NoSI.Text);

        if (tfrw != null)
        {
            ParameterData.ParameterRow param = Parameter.GetParameterByCodeAndApprovalStatus("EmailTKS", "A");
            SMTPHelper.SendInformasiNoticeOfShipment(param.StringValue, nosList);
        }
        else
        {
            ParameterData.ParameterRow param = Parameter.GetParameterByCodeAndApprovalStatus("EmailBGR", "A");
            SMTPHelper.SendInformasiNoticeOfShipment(param.StringValue, nosList);
        }


    }
    private void Download()
    {
        try
        {
            List<String> filePathAll = new List<string>();
            List<parameter_invoice_dalam_negeri> parameter_invoice_dalam_negeri = new List<parameter_invoice_dalam_negeri>();

            var con = ConfigurationManager.ConnectionStrings["SKDConnectionString"].ToString();

            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string oString = "SELECT DISTINCT a.BuyerId,a.SellerId,b.CMID as buyerCMID, c.CMID as sellerCMID, b.Name as namebuyer, b.StatusDomisiliFlag as flagbuyer, c.Name as nameseller, c.StatusDomisiliFlag as flagseller FROM SKD.EODTradeProgress a JOIN SKD.CMProfile b ON SUBSTRING(a.BuyerId, 0, 5) = b.Code JOIN SKD.CMProfile c ON SUBSTRING(a.SellerId, 0, 5) = c.Code WHERE BusinessDate = '" + CtlCalendarPickUp1.Text + "'";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var data = new parameter_invoice_dalam_negeri
                        {
                            sellerId = (oReader["SellerId"].ToString()),
                            buyerId = (oReader["BuyerId"].ToString()),
                            flagdomisilibuyer = (oReader["flagbuyer"].ToString()),
                            flagdomisiliseller = (oReader["flagseller"].ToString()),
                            nameBuyer = (oReader["namebuyer"].ToString()),
                            nameSeller = (oReader["nameseller"].ToString()),
                            buyerCMID = (oReader["buyerCMID"].ToString()),
                            sellerCMID = (oReader["sellerCMID"].ToString()),
                        };
                        parameter_invoice_dalam_negeri.Add(data);
                    }
                    myConnection.Close();
                }
            }
            foreach (var item in parameter_invoice_dalam_negeri)
            {
                if (item.flagdomisiliseller == "D" && item.flagdomisilibuyer == "D")
                {
                    uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
                    uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();
                    //get rdlc for seller
                    uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Invoice_DN";

                    List<ReportParameter> rp = new List<ReportParameter>();

                    // Set parameter reportName
                    if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
                    {
                        rp.Add(new ReportParameter("PeriodStart", new string[] { null }));
                        rp.Add(new ReportParameter("SellerId", new string[] { null }));
                        rp.Add(new ReportParameter("BuyerId", new string[] { null }));
                    }
                    else
                    {
                        rp.Add(new ReportParameter("PeriodStart", new string[] { CtlCalendarPickUp1.Text }));
                        rp.Add(new ReportParameter("SellerId", new string[] { item.sellerId }));
                        rp.Add(new ReportParameter("BuyerId", new string[] { item.buyerId }));
                    }
                    uiRptViewer.ServerReport.SetParameters(rp);

                    byte[] result = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;

                    result = uiRptViewer.ServerReport.Render("WORD", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    string fileName = "Invoice Seller Dalam Negeri " + item.nameSeller+" " + Convert.ToDateTime(CtlCalendarPickUp1.Text).ToString("dd MMM yyyy") + ".doc";
                    string path = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                    if (File.Exists(path))
                    {

                    }
                    else
                    {
                        System.IO.FileStream file = System.IO.File.Create(HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                        file.Write(result, 0, result.Length);
                        file.Close();
                    }
                    string filePath = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                    filePathAll.Add(filePath);
                }
                else
                {
                    //INVOICE for SELLER
                    String SellerCMID = item.sellerCMID;
                    String sellerName = item.nameSeller;

                    uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
                    uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();
                    //get rdlc for seller
                    uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Invoice Seller";

                    List<ReportParameter> rp = new List<ReportParameter>();
                    List<ReportParameter> rp1 = new List<ReportParameter>();

                    // Set parameter reportName
                    if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
                    {
                        rp.Add(new ReportParameter("BusinessDate", new string[] { null }));
                        rp1.Add(new ReportParameter("SellerCMID", new string[] { null }));
                    }
                    else
                    {
                        rp.Add(new ReportParameter("BusinessDate", new string[] { CtlCalendarPickUp1.Text }));
                        rp1.Add(new ReportParameter("SellerCMID", new string[] { SellerCMID }));
                    }
                    uiRptViewer.ServerReport.SetParameters(rp);
                    uiRptViewer.ServerReport.SetParameters(rp1);

                    byte[] result = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;

                    result = uiRptViewer.ServerReport.Render("WORD", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    string fileName = "Invoice Seller " + sellerName + " " + Convert.ToDateTime(CtlCalendarPickUp1.Text).ToString("dd MMM yyyy") + ".doc";
                    string path = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                    if (File.Exists(path))
                    {
                        
                    }
                    else
                    {
                        System.IO.FileStream file = System.IO.File.Create(HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                        file.Write(result, 0, result.Length);
                        file.Close();
                        string filePath = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                        filePathAll.Add(filePath);
                    }

                    //FOR BUYER
                    String BuyerCMID = item.buyerCMID;
                    String BuyerName = item.nameBuyer;

                    uiRptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
                    uiRptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials();
                    //get rdlc for seller
                    uiRptViewer.ServerReport.ReportPath = "/TIN_EOD_Report/Invoice Buyer";

                    List<ReportParameter> rp_buyer = new List<ReportParameter>();

                    // Set parameter reportName
                    if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
                    {
                        rp_buyer.Add(new ReportParameter("BusinessDate", new string[] { null }));
                        rp_buyer.Add(new ReportParameter("BuyerCMID", new string[] { null }));
                    }
                    else
                    {
                        rp_buyer.Add(new ReportParameter("BusinessDate", new string[] { CtlCalendarPickUp1.Text }));
                        rp_buyer.Add(new ReportParameter("BuyerCMID", new string[] { BuyerCMID }));
                    }
                    uiRptViewer.ServerReport.SetParameters(rp_buyer);

                    result = null;
                    mimeType = "";
                    encoding = "";
                    filenameExtension = "";
                    streamids = null;
                    warnings = null;

                    result = uiRptViewer.ServerReport.Render("WORD", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    fileName = "Invoice Buyer " + BuyerName + " " + Convert.ToDateTime(CtlCalendarPickUp1.Text).ToString("dd MMM yyyy") + ".doc";

                    path = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                    if (File.Exists(path))
                    {

                    }
                    else
                    {
                        System.IO.FileStream file = System.IO.File.Create(HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                        file.Write(result, 0, result.Length);
                        file.Close();
                        string filePath = (HttpContext.Current.Server.MapPath("~\\Report\\" + fileName + ""));
                        filePathAll.Add(filePath);
                    }
                }
            }

            var ta_buyer = new GetExchangeReffTableAdapters.TradeFeed1TableAdapter();
            var dt_buyer = new GetExchangeReff.TradeFeed1DataTable();
            dt_buyer = ta_buyer.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));

            var ta = new GetExchangeReffTableAdapters.TradeFeedTableAdapter();
            var dt = new GetExchangeReff.TradeFeedDataTable();
            dt = ta.GetData(Convert.ToDateTime(CtlCalendarPickUp1.Text));

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (var item in filePathAll.Select(a=>a).Distinct())
                {
                    zip.AddFile(item, "Files");
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
            foreach (var item in filePathAll)
            {
                File.Delete(item);
            }
        }
        catch (Exception ex)
        {
            uiBlError.Visible = true;
            uiBlError.Items.Add(ex.Message);
        }
    }
    private void Search()
    {
        TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

        try
        {
            DateTime bussDate = DateTime.Now;

            if (CtlCalendarPickUp1.Text != "")
            {
                bussDate = DateTime.Parse(CtlCalendarPickUp1.Text);
            }

            dt = Tradefeed.FillNoticeOfShipment(decimal.Parse(uiDdlExchange.SelectedValue), bussDate, NoSI.Text);

            DataView dv = new DataView(dt);
            if (!string.IsNullOrEmpty(SortOrder))
            {
                dv.Sort = SortOrder;
            }

            uiDgTradeFeed.DataSource = dv;
            uiDgTradeFeed.DataBind();
        }
        catch (Exception ex)
        {
            uiBlError.Visible = true;
            uiBlError.Items.Add(ex.Message);
        }
        finally
        {
            dt.Dispose();
        }
    }
    protected void uiDgTradeFeed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTradeFeed.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void uiDgTradeFeed_Sorting(object sender, GridViewSortEventArgs e)
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

        Search();
    }
    protected void uiDgTradeFeed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label price = (Label)e.Row.FindControl("uiLblPrice");
            Label contractId = (Label)e.Row.FindControl("uiLblContractId");

            switch (Tradefeed.GetCurrencyCodeByContractID(decimal.Parse(contractId.Text)))
            {
                case "IDR":
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                case "USD":
                    price.Text = decimal.Parse(price.Text).ToString("#,##0.0000");
                    break;
                default:
                    price.Text = price.Text;
                    break;
            }
        }
    }
    private string ExRef
    {
        get { return Request.QueryString["exRef"]; }
    }
    public class parameter_invoice_dalam_negeri
    {
        public string sellerId { get; set; }
        public string buyerId { get; set; }
        public string flagdomisiliseller { get; set; }
        public string flagdomisilibuyer { get; set; }
        public string nameSeller { get; set; }
        public string nameBuyer { get; set; }
        public string sellerCMID { get; set; }
        public string buyerCMID { get; set; }
    }
}