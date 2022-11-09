using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClearingMemberStatus
/// </summary>
public class ClearingMemberStatus
{

    public static ClearingMemberStatusData.ClearingMemberSuspenseStatusDataTable GetLastClearingMemberStatus(DateTime bussDate, decimal clearingMemberId)
    {
        ClearingMemberStatusData ds = new ClearingMemberStatusData();
        ClearingMemberStatusDataTableAdapters.ClearingMemberSuspenseStatusTableAdapter ta = new ClearingMemberStatusDataTableAdapters.ClearingMemberSuspenseStatusTableAdapter();

        try
        {
            ta.FillLastStatusByCmAndBussDate(ds.ClearingMemberSuspenseStatus, bussDate, clearingMemberId);

            return ds.ClearingMemberSuspenseStatus;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Can not get last status of Clearing Member: " + ex.Message);
        }
    }

}
