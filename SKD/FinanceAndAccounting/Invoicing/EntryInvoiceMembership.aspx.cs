using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinanceAndAccounting_Invoicing_EntryInvoice : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private decimal eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(Request.QueryString["eID"]);
            }
        }
        set { ViewState["eID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            uiBLError.Visible = false;

            if (!Page.IsPostBack)
            {

                if (eType == "add")
                {

                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            uiBtnSave.Visible = true;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                string pathInvoice = string.Format("{0}\\{3}\\{1}_{2}_Invoice.pdf", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_MEMBERSHIP].ToString(), DateTime.Parse(CtlCalendarPickUp2.Text).ToString("yyyyMMdd"), CtlClearingMemberLookup1.LookupTextBox, CtlClearingMemberLookup1.LookupTextBox);
                string pathFakturPajak = string.Format("{0}\\{3}\\{1}_{2}_FakturPajak.pdf", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_MEMBERSHIP].ToString(), DateTime.Parse(CtlCalendarPickUp2.Text).ToString("yyyyMMdd"), CtlClearingMemberLookup1.LookupTextBox, CtlClearingMemberLookup1.LookupTextBox);

                if (!Directory.Exists(string.Format("{0}\\{1}\\", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_MEMBERSHIP].ToString(), CtlClearingMemberLookup1.LookupTextBox)))
                {
                    Directory.CreateDirectory(string.Format("{0}\\{1}\\", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_INVOICE_MEMBERSHIP].ToString(), CtlClearingMemberLookup1.LookupTextBox));
                }

                uiUfInvoice.SaveAs(pathInvoice);
                uiUfFakturPajak.SaveAs(pathFakturPajak);

                InvoiceDataTableAdapters.InvoiceTableAdapter inta = new InvoiceDataTableAdapters.InvoiceTableAdapter();
                inta.Insert(
                    decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), "", "IE", DateTime.Parse(CtlCalendarPickUp2.Text), 0, 0,
                    DateTime.Now, null, null, pathInvoice, pathFakturPajak);
                
                ClearingMemberData.ClearingMemberDataTable dtCM = new ClearingMemberData.ClearingMemberDataTable();
                ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

                ta.FillByCode(dtCM, CtlClearingMemberLookup1.LookupTextBox);

                SMTPHelper.SendInvoice(dtCM[0].Email, pathInvoice, pathFakturPajak);

                Response.Redirect("ViewInvoiceMembership.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewInvoiceMembership.aspx");
    }

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage) this.Master;

            if (!uiUfInvoice.HasFile)
            {
                uiBLError.Items.Add("File Invoice not found.");
            }

            if (!uiUfFakturPajak.HasFile)
            {
                uiBLError.Items.Add("File Faktur Pajak not found.");
            }

            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }
            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}