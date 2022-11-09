using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ReportFee
/// </summary>
public class ReportFee
{
    public ReportFee()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static RptFee.report_fee_invoiceDataTable GetRptFeeSearch(string name, DateTime start, DateTime end, string pelaku, string status, string status2, string inv_number)
    {
        RptFeeTableAdapters.report_fee_invoiceTableAdapter ta = new RptFeeTableAdapters.report_fee_invoiceTableAdapter();
        
        try
        {
            return ta.GetDataBySearch(start, end, name ,  pelaku, status, status2, inv_number);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}