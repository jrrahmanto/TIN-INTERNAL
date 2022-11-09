using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for ExchangeMember
/// </summary>
public class ExchangeMember
{
    public static ExchangeMemberData.ExchangeMemberDataTable 
        GetExchangeMemberByCodeApprovalStatus(string code, string approvalStatus)
    {
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();

        try
        {
            ta.FillByCodeApprovalStatus(dt, code, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ExchangeMemberData.ExchangeMemberCompleteDataTable
        GetExchangeMemberCompleteByExCmEmCode(string exchangeCode, string cmCode, string emCode)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberCompleteTableAdapter ta = 
            new ExchangeMemberDataTableAdapters.ExchangeMemberCompleteTableAdapter();

        return ta.GetDataByExCmEmCode(exchangeCode, cmCode, emCode);
    }

    public static string GetExchangeMemberNameByExchangeMemberId(decimal exchangeMemberId)
    {
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();
        ExchangeMemberData.ExchangeMemberRow dr = null;
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        string exchangeMemberName = "";

        try
        {
            ta.FillByEMID(dt, exchangeMemberId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                exchangeMemberName = dr.Name;
            }

            return exchangeMemberName;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ExchangeMemberData.ExchangeMemberDataTable FillByApproval(string approvalStatus, decimal CMID)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        try
        {
            return ta.GetDataByApprovalStatus(approvalStatus, CMID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ExchangeMemberData.ExchangeMemberDataTable FillByEMID(decimal EMID)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        try
        {
            return ta.GetDataByEMID(EMID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ExchangeMemberData.ExchangeMemberDdlDataTable FillDDL()
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberDdlTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberDdlTableAdapter();
        try
        {
            return ta.GetData();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ValidateRecord(decimal EditedEMID, string ApprovalStatus)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();

        ta.FillByEMID(dt, EditedEMID);

        if (dt.Count > 0)
        {
            if (dt[0].ApprovalStatus == "P" && ApprovalStatus == "P")
            {
                throw new ApplicationException("This record is not allowed to be edited / deleted. Please wait for checker approval.");
            }

            if (dt[0].ApprovalStatus == "A" && (ApprovalStatus == "A" || ApprovalStatus == "R"))
            {
                throw new ApplicationException("Approved row is not allowed to be approved or rejected.");
            }
        }

    }

    public static void ProposedEM(string Code, decimal ExchangeID, DateTime StartDate, Nullable<DateTime> EndDate, 
                                        decimal CMID, string Name, string CMRep, string Status, string UserName, string ActionFlag, 
                                        Nullable<decimal> OriginalID, string MiniLotStatus)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        string ActionFlagDesc="";

        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                if (OriginalID.HasValue)
                    ValidateRecord(Convert.ToDecimal(OriginalID), "P");

                switch (ActionFlag)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                string logMessage = string.Format(ActionFlagDesc + " , Code:{0} | ExchangeID:{1} |" +
                                                  " StartDate:{2} | RegisteredCMID:{3} | " +
                                                  " Name:{4} | Status:{5} | EndDate:{6} | CMRep: {7}",
                                                  Code, ExchangeID.ToString(), StartDate.ToShortDateString(),
                                                  CMID.ToString(), Name, Status, EndDate.ToString(), CMRep);

                ta.Insert(Code, ExchangeID, "P", StartDate, CMID, Name, CMRep, UserName,
                    DateTime.Now, UserName, DateTime.Now, EndDate, null, Status, OriginalID, ActionFlag, MiniLotStatus);

                AuditTrail.AddAuditTrail("ExchangeMember", AuditTrail.PROPOSE, logMessage, UserName, ActionFlagDesc);
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Approve(decimal EMID, string ApprovalDesc, string UserName)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();
        string ActionFlagDesc = "";

        try
        {
            TableAdapterHelper.SetAllCommandTimeouts(ta, 300);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                ValidateRecord(EMID, "A");

                ta.FillByEMID(dt, EMID);
                if (dt.Count > 0)
                {

                    // TODO: Guard for conflict effectiveness date
                    //if (dt[0].ActionFlag != "D")
                    //{
                    //    decimal nextID = Convert.ToDecimal(ta.GetNextDateEMID(dt[0].Code, dt[0].ExchangeId, "A",
                    //                dt[0].EffectiveStartDate));
                    //    decimal prefID = Convert.ToDecimal(ta.GetPrevDateEMID(dt[0].Code, dt[0].ExchangeId, "A",
                    //                     dt[0].EffectiveStartDate));
                    //    if (nextID != null)
                    //    {
                    //        ta.UpdateStartDate(dt[0].EffectiveStartDate.AddDays(-1), EMID);
                    //    }
                    //    if (prefID != null)
                    //    {
                    //        ta.UpdateEndDate(dt[0].EffectiveEndDate.AddDays(-1), EMID);
                    //    }
                    //}

                    Nullable<DateTime> EndDate = null;
                    string CMRep = null;

                    if (!dt[0].IsEffectiveEndDateNull())
                        EndDate = dt[0].EffectiveEndDate;
                    if (!dt[0].IsCMRepNull())
                        CMRep = dt[0].CMRep;

                    string MiniLotFlag = null;
                    if (!dt[0].IsMiniLotNull())
                        MiniLotFlag = dt[0].MiniLot;


                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApproveInsert("A", ApprovalDesc, null, null, UserName, DateTime.Now, EMID);

                        ActionFlagDesc = "Approved Insert";
                    }
                    else if (dt[0].ActionFlag == "U")
                    {

                        ta.ApproveUpdate(dt[0].Code, dt[0].ExchangeId, "A", dt[0].EffectiveStartDate, 
                            dt[0].RegisteredCMID, dt[0].Name, CMRep, UserName, DateTime.Now,
                            EndDate, ApprovalDesc, dt[0].Status,
                            null, null, MiniLotFlag, dt[0].OriginalID);
                        //ta.ApproveUpdate(dt[0].Code, dt[0].ExchangeId, "A", dt[0].EffectiveStartDate,
                        //    dt[0].RegisteredCMID, dt[0].Name, CMRep, UserName, DateTime.Now,
                        //    EndDate, ApprovalDesc, dt[0].Status,
                        //    null, null, dt[0].OriginalID);
                        ta.DeleteEM(EMID);
                        //(dt[0].CMRep == "Y") ? true : false
                        ActionFlagDesc = "Approved Update";
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteEM(dt[0].OriginalID);
                        ta.DeleteEM(EMID);

                        ActionFlagDesc = "Approved Delete";
                    }

                    string logMessage = string.Format(ActionFlagDesc + " , Code:{0} | ExchangeID:{1} |" +
                                                  " StartDate:{2} | RegisteredCMID:{3} | " +
                                                  " Name:{4} | Status:{5} | EndDate:{6} | CMRep:{7}",
                                                  dt[0].Code, dt[0].ExchangeId.ToString(),
                                                  dt[0].EffectiveStartDate.ToShortDateString(),
                                                  dt[0].RegisteredCMID.ToString(),
                                                  dt[0].Name, dt[0].Status,
                                                  EndDate.ToString(), CMRep);
                    AuditTrail.AddAuditTrail("ExchangeMember", ActionFlagDesc, logMessage, UserName,ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    public static string GetExchangeMemberCode(decimal exchangeMemberId)
    {
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();
        ExchangeMemberData.ExchangeMemberRow dr = null;
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        string exchangeMemberCode = "";

        try
        {
            ta.FillByEMID(dt, exchangeMemberId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                exchangeMemberCode = dr.Code;
            }

            return exchangeMemberCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void Reject(decimal EMID, string ApprovalDesc, string UserName)
    {
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter ta = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        ExchangeMemberData.ExchangeMemberDataTable dt = new ExchangeMemberData.ExchangeMemberDataTable();
        string ActionFlagDesc = "";

        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                ValidateRecord(EMID, "R");

                ta.FillByEMID(dt, EMID);
                if (dt.Count > 0)
                {
                    Nullable<DateTime> EndDate = null;
                    string CMRep = null;

                    if (!dt[0].IsEffectiveEndDateNull())
                        EndDate = dt[0].EffectiveEndDate;
                    if (!dt[0].IsCMRepNull())
                        CMRep = dt[0].CMRep;

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Reject Insert"; break;
                        case "U": ActionFlagDesc = "Reject Update"; break;
                        case "D": ActionFlagDesc = "Reject Delete"; break;
                    }

                    ta.DeleteEM(EMID);
                    string logMessage = string.Format(ActionFlagDesc + " , Code:{0} | ExchangeID:{1} |" +
                                                  " StartDate:{2} | RegisteredCMID:{3} | " +
                                                  " Name:{4} | Status:{5} | EndDate:{6} | CMRep:{7}",
                                                  dt[0].Code, dt[0].ExchangeId.ToString(),
                                                  dt[0].EffectiveStartDate.ToShortDateString(),
                                                  dt[0].RegisteredCMID.ToString(),
                                                  dt[0].Name, dt[0].Status,
                                                  EndDate.ToString(), CMRep);
                    AuditTrail.AddAuditTrail("ExchangeMember", ActionFlagDesc, logMessage, UserName, ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}
