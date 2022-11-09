using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StagingMemberAccount
/// </summary>
public class StagingMemberAccount
{
    public static StagingData.MemberAccountDataTable SelectMemberAccountByBusinessDate(DateTime BusinessDate)
    {
        StagingDataTableAdapters.MemberAccountTableAdapter ta = new StagingDataTableAdapters.MemberAccountTableAdapter();
        StagingData.MemberAccountDataTable dt = new StagingData.MemberAccountDataTable();

        try
        {
            ta.Fill(dt, BusinessDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Member Account data");
        }
    }
}