using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Report
/// </summary>
public class Report
{
    public static ReportData.uspCashSettlementDateDataTable GetCashSettlementDate(int monthDate, int yearDate)
    {
        ReportData.uspCashSettlementDateDataTable dt = new ReportData.uspCashSettlementDateDataTable();
        ReportDataTableAdapters.uspCashSettlementDateTableAdapter ta = new ReportDataTableAdapters.uspCashSettlementDateTableAdapter();

        try
        {
            ta.FillCashSettlementData(dt, monthDate, yearDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to retrieve cash settlement info", ex);
        }
    }

}
