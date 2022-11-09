using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Invoice
/// </summary>
public class Invoice
{
    public static InvoiceData.InvoiceViewDataTable SelectInvoiceByFilterMM(Nullable<DateTime> invoicedate, Nullable<decimal> cmid)
    {
        InvoiceDataTableAdapters.InvoiceViewTableAdapter ta = new InvoiceDataTableAdapters.InvoiceViewTableAdapter();
        InvoiceData.InvoiceViewDataTable dt = new InvoiceData.InvoiceViewDataTable();

        try
        {
            ta.Fill(dt, cmid, invoicedate, "IM");
        }
        catch
        {
            
        }

        return dt;
    }

    public static InvoiceData.InvoiceViewDataTable SelectInvoiceByFilterAM(Nullable<DateTime> invoicedate, Nullable<decimal> cmid)
    {
        InvoiceDataTableAdapters.InvoiceViewTableAdapter ta = new InvoiceDataTableAdapters.InvoiceViewTableAdapter();
        InvoiceData.InvoiceViewDataTable dt = new InvoiceData.InvoiceViewDataTable();

        try
        {
            ta.Fill(dt, cmid, invoicedate, "AM");
        }
        catch
        {

        }

        return dt;
    }

    public static InvoiceData.InvoiceViewDataTable SelectInvoiceByFilterFC(Nullable<DateTime> invoicedate, Nullable<decimal> cmid)
    {
        InvoiceDataTableAdapters.InvoiceViewTableAdapter ta = new InvoiceDataTableAdapters.InvoiceViewTableAdapter();
        InvoiceData.InvoiceViewDataTable dt = new InvoiceData.InvoiceViewDataTable();

        try
        {
            ta.Fill(dt, cmid, invoicedate, "FC");
        }
        catch
        {

        }

        return dt;
    }
}