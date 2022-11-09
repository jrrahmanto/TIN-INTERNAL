using System;
using System.Collections;
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
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;
////using ClosedXML.Excel;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
public partial class FinanceAndAccounting_Parameter_ViewManageKBIAccount : System.Web.UI.Page
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
        uiDdlAccountType.Focus();
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManageBankAccount.aspx?eType=add");
    }


    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        if (uiDgBankAccount.Rows.Count > 0)
        {
            DownloadCSV();
        }
        else
        {
            uiBLError.Items.Add("No records found to download.");
            uiBLError.Visible = true;
        }
    }

    
    protected void uiDgBankAccount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            uiDgBankAccount.PageIndex = e.NewPageIndex;
            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiDgBankAccount_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
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

            FillBankAccountDataGrid();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void FillBankAccountDataGrid()
    {
        try
        {
            uiDgBankAccount.DataSource = ObjectDataSourceBankAccount;
            IEnumerable dv = (IEnumerable)ObjectDataSourceBankAccount.Select();
            DataView dve = (DataView)dv;

            if (!string.IsNullOrEmpty(SortOrder))
            {
                dve.Sort = SortOrder;
            }

            uiDgBankAccount.DataSource = dve;
            uiDgBankAccount.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;
            uiBtnCreate.Visible = mp.IsMaker;
            uiDgBankAccount.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
     }

    #region Download data Bank Account to PDF and Excel
    //private void DownloadBankAccount(string approvalStatus, string accountType,
    //                                       Nullable<decimal> bankId, Nullable<decimal> cmId, Nullable<decimal> ccyId,
    //                                       string accStatus)
    //{
    //    try
    //    {
    //        //DataTable dt = new DataTable();
    //        //dt = BankAccount.SelectBankAccountForExport(approvalStatus, accountType, bankId, cmId, ccyId, accStatus);

    //        //Create a dummy GridView
    //        //GridView GridView1 = new GridView();
    //        //GridView1.AllowPaging = false;
    //        //GridView1.DataSource = dt;
    //        //GridView1.DataBind();

    //        uiDgBankAccount.AllowPaging = false;

    //        Response.Clear();
    //        Response.Buffer = true;
    //        Response.AddHeader("content-disposition",
    //         "attachment;filename=GridViewExport.xls");
    //        Response.Charset = "";
    //        Response.ContentType = "application/vnd.ms-excel";
    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);

    //        for (int i = 0; i < uiDgBankAccount.Rows.Count; i++)
    //        {
    //            //Apply text style to each Row
    //            uiDgBankAccount.Rows[i].Attributes.Add("class", "textmode");
    //        }
    //        uiDgBankAccount.RenderControl(hw);

    //        //style to format numbers to string
    //        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
    //        Response.Write(style);
    //        Response.Output.Write(sw.ToString());
    //        Response.Flush();
    //        Response.End();

    //        //using (XLWorkbook wb = new XLWorkbook())
    //        //{
    //        //    wb.Worksheets.Add(dt);

    //        //    Response.Clear();
    //        //    Response.Buffer = true;
    //        //    Response.Charset = "";
    //        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //        //    Response.AddHeader("content-disposition", "attachment;filename=BankAccount.xlsx");
    //        //    using (MemoryStream MyMemoryStream = new MemoryStream())
    //        //    {
    //        //        wb.SaveAs(MyMemoryStream);
    //        //        MyMemoryStream.WriteTo(Response.OutputStream);
    //        //        Response.Flush();
    //        //        Response.End();
    //        //    }
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }


    //}

    //private void ExportToPdf()
    //{
    //    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
    //    try
    //    {
    //        PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
    //        pdfDoc.Open();

    //        string imageFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Images/left.png");
    //        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
    //        //Resize image depend upon your need   
    //        jpg.ScaleToFit(280f, 100f);
    //        //Give space before image   
    //        jpg.SpacingBefore = 0f;
    //        //Give some space after the image   
    //        jpg.SpacingAfter = 1f;
    //        jpg.Alignment = Element.HEADER;
    //        pdfDoc.Add(jpg);


    //        Chunk c = new Chunk("Bank Account", FontFactory.GetFont("Verdana", 11));
    //        Paragraph p = new Paragraph();
    //        p.SpacingBefore = 0f;
    //        p.SpacingAfter = 15f; 
    //        p.Alignment = Element.ALIGN_CENTER;
    //        p.Add(c);
    //        pdfDoc.Add(p);


    //        Font font8 = FontFactory.GetFont("ARIAL", 7);

    //        if (uiDgBankAccount.Rows.Count > 0)
    //        {
    //            PdfPTable pdfTbl = new PdfPTable(uiDgBankAccount.Columns.Count);
    //             PdfPCell pdfCell = null;

    //            for (int i = 0; i < uiDgBankAccount.Columns.Count; i++)
    //            {
    //                string header = uiDgBankAccount.Columns[i].HeaderText;
    //                if (header =="Edit")
    //                {
    //                    header = "No";
    //                }
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(header, font8)));
    //                pdfTbl.AddCell(pdfCell);

    //            }

    //            for (int r = 0; r < uiDgBankAccount.Rows.Count; r++)
    //            {
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(Convert.ToString(r+1) , font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[1].Text, font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[2].Text, font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[3].Text, font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[4].Text, font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[5].Text, font8)));
    //                pdfTbl.AddCell(pdfCell);
    //                pdfCell = new PdfPCell(new Phrase(new Chunk(uiDgBankAccount.Rows[r].Cells[6].Text, font8)));
    //                pdfTbl.AddCell(pdfCell); 

    //            }

    //                pdfDoc.Add(pdfTbl); // add pdf table to the document   
    //        }

    //        pdfDoc.Close();
    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", "attachment; filename= SampleExport.pdf");
    //        System.Web.HttpContext.Current.Response.Write(pdfDoc);
    //        Response.Flush();
    //        Response.End();
    //        //HttpContext.Current.ApplicationInstance.CompleteRequest();  
    //    }
    //    catch (DocumentException de)
    //    {
    //        System.Web.HttpContext.Current.Response.Write(de.Message);
    //    }
    //    catch (IOException ioEx)
    //    {
    //        System.Web.HttpContext.Current.Response.Write(ioEx.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Web.HttpContext.Current.Response.Write(ex.Message);
    //    }
    //}

    //private void ExportExcel()
    //{
    //    try
    //    {
    //        //Dim ad As New results()
    //        //Dim dt As results.ResultsDataTable
    //        //dt = ad.Read()

    //        string attachment = "attachment; filename=USurvey.xls";
    //        Response.ClearContent();
    //        Response.AddHeader("content-disposition", attachment);
    //        Response.ContentType = "application/vnd.ms-excel";

    //        string tab = "";

    //        for (int i = 0; i < uiDgBankAccount.Columns.Count; i++)
    //        {
    //            string header = uiDgBankAccount.Columns[i].HeaderText;
    //            if (header == "Edit")
    //            {
    //                header = "No";
    //            }

    //            Response.Write(tab + header);
    //            tab = "\t";
    //        }
    //        Response.Write(tab);
    //        tab = "";

    //        for (int r = 0; r < uiDgBankAccount.Rows.Count; r++)
    //        {
    //            Response.Write(tab + Convert.ToString(r + 1));
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[1].Text);
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[2].Text);
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[3].Text);
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[4].Text);
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[5].Text);
    //            tab = "\t";
    //            Response.Write(tab + uiDgBankAccount.Rows[r].Cells[6].Text);
    //            tab = "\t";
    //            tab = "";


    //        }
    //        Response.Write(tab);
    //        Response.End(); 


    //    }
    //    catch (Exception ex)
    //    { }
    //}

    private void DownloadCSV()
    {
        try
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;
            string noAcc = string.Empty;
            string noVa = string.Empty;
            string rekVa = string.Empty;
            string partNm = string.Empty;
            for (int r = 0; r < uiDgBankAccount.Rows.Count; r++)
            {

                noAcc = uiDgBankAccount.Rows[r].Cells[4].Text;
                noVa = noAcc.Substring(0, 4);
                rekVa = noAcc.Substring(4);
                partNm = uiDgBankAccount.Rows[r].Cells[1].Text;

                csv += "'" + noVa + "," + "'" + rekVa + "," + partNm;

                //Add new line.
                csv += "\r\n";

            }

            //Download the CSV file.
            Response.Clear();
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AddHeader("content-disposition", "attachment;filename=BankAccount1.csv");
            //Response.Charset = "";
            Response.ContentType = "application/text";
            //Response.Output.Write(csv);
            Response.Write(csv);
            //Response.Flush();
            //Response.End();


        }
        catch (Exception ex)
        {
            System.Web.HttpContext.Current.Response.Write(ex.Message);
        }
        finally
        {
            try
            {
                //stop processing the script and return the current result
                HttpContext.Current.Response.End();
            }
            catch (Exception ex) {
                
            }
            finally
            {
                //Sends the response buffer
                HttpContext.Current.Response.Flush();
                // Prevents any other content from being sent to the browser
                HttpContext.Current.Response.SuppressContent = true;
                //Directs the thread to finish, bypassing additional processing
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Suspends the current thread
                Thread.Sleep(1);
            }
        }
    }

    #endregion
}
