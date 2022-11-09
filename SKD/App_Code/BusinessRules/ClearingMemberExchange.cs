using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for ClearingMemberExchange
/// </summary>
public class ClearingMemberExchange
{
    public static ClearingMemberExchangeData.ClearingMemberExchangeDataTable fill()
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ClearingMemberExchangeData.ClearingMemberExchangeDataTable fill(string ApprovalStatus,
                                                                            string CMID)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        try
        {
            ta.FillByCMIDApproval(dt,Convert.ToDecimal(CMID),ApprovalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ClearingMemberExchangeData.ClearingMemberExchangeDataTable fillByCMID(string CMID)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        try
        {
            ta.FillByCMID(dt, Convert.ToDecimal(CMID));
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ClearingMemberExchangeData.ClearingMemberExchangeDataTable fillByCMIDAndExchangeID(decimal CMID,
                                                                                        decimal exchangeID)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        try
        {
            ta.FillByCMIDandExchangeID(dt, CMID,exchangeID);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static ClearingMemberExchangeData.ClearingMemberDDLDataTable fillDDl()
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberDDLTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberDDLTableAdapter();
        ClearingMemberExchangeData.ClearingMemberDDLDataTable dt = new ClearingMemberExchangeData.ClearingMemberDDLDataTable();
        
        try
        {
            dt.AddClearingMemberDDLRow("");
            dt.AcceptChanges();
            ta.FillCMtypeForDDL(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ValidateRecord(decimal EditedCMExchangeID, string ApprovalStatus)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();

        ta.FillByCMExchangeID(dt, EditedCMExchangeID);

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

    public static void ProposedCMExchange(decimal CMID, decimal exchangeID, DateTime startDate, Nullable<DateTime> endDate,
                                string exchangeCode, string cmType, string userName, string ActionFlag, 
                                Nullable<decimal> OriginalCMExchangeID, string exchangeLicenseNo,
                                Nullable<DateTime> exchangeLicenseDate)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(CMID, exchangeID, "P", startDate.Date, exchangeCode, cmType, userName, DateTime.Now,
                      userName, DateTime.Now, endDate, null, ActionFlag, OriginalCMExchangeID,exchangeLicenseNo,
                      exchangeLicenseDate);
                
                string ActionFlagDesc = "";
                switch (ActionFlag)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                string logMessage = string.Format("Proposed Update, CMID:{0} | ExchangeID:{1} | ApprovalStatus:{2} |" +
                                                 " effectiveStartDate:{3} | CMExchangeCode: {4} | CMType:{5} | EndDate : {6} " ,
                                                 CMID, exchangeID, "P", startDate.ToShortDateString(), 
                                                 exchangeCode, (cmType == "B") ? "Broker" : "Trader",
                                                 endDate.ToString());
               
                AuditTrail.AddAuditTrail("ClearingMemberExchange", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist. Please input new record.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
        
    }

    public static void Approve(decimal CMExchangeID, string UserName, string ApprovalDesc)
    {
        try 
	    {	        
    		ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
            ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
            string ActionFlagDesc = "";

            ValidateRecord(CMExchangeID, "A");

            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByCMExchangeID(dt, CMExchangeID);

                string logMessage = "";

                Nullable<DateTime> effectiveEndDate = null;

                if (dt.Count > 0)
                {
                    if (!dt[0].IsEffectiveEndDateNull())
                    {
                        effectiveEndDate = dt[0].EffectiveEndDate;
                    }

                    if (dt[0].ActionFlag == "I")
                    {
                        ActionFlagDesc = "Approved Insert";
                        // Approve new record
                        ta.ApproveInsert("A", ApprovalDesc, null, UserName, DateTime.Now, CMExchangeID);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ActionFlagDesc = "Approved Update";
                        // Update target record
                       
                        ta.ApproveUpdate(dt[0].CMID, dt[0].ExchangeId, "A", dt[0].EffectiveStartDate, dt[0].CMExchangeCode, dt[0].CMType,
                                UserName, DateTime.Now, effectiveEndDate, ApprovalDesc, null, null, dt[0].OriginalCMExchangeID);
                        
                        // Delete propose / temporary record
                        ta.DeleteByCMExchangeID(CMExchangeID);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ActionFlagDesc = "Approved Delete";
                        // Delete proposed record
                        ta.DeleteByCMExchangeID(dt[0].CMExchangeID);

                        // Delete target record
                        ta.DeleteByCMExchangeID(dt[0].OriginalCMExchangeID);
                    }

                    logMessage = string.Format(ActionFlagDesc + " , CMID:{0} | ExchangeID:{1} | ApprovalStatus:{2} |" +
                                                    " effectiveStartDate:{3} | CMExchangeCode: {4} | CMType:{5} | EndDate : {6} ",
                                                    dt[0].CMID, dt[0].ExchangeId, "A", dt[0].EffectiveStartDate, 
                                                    dt[0].CMExchangeCode, 
                                                    (dt[0].CMType == "B") ? "Broker" : "Trader",
                                                    effectiveEndDate.ToString());

                    AuditTrail.AddAuditTrail("ClearingMemberExchange", ActionFlagDesc, logMessage, UserName, ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
	    catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist. Please input new record.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
        
    }

    public static void Reject(decimal CMExchangeID, string UserName, string ApprovalDesc)
    {
        try 
	    {	        
    		ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
            ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
            string logMessage = "";
            string proposedFlag = "";

            ValidateRecord(CMExchangeID, "R");

            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByCMExchangeID(dt, CMExchangeID);

                if (dt.Count > 0)
                {
                    switch (dt[0].ActionFlag)
                    {
                        case "I": proposedFlag = "Reject Insert"; break;
                        case "U": proposedFlag = "Reject Update"; break;
                        case "D": proposedFlag = "Reject Delete"; break;
                    }

                    logMessage = string.Format(proposedFlag + " , CMID:{0} | ExchangeID:{1} | ApprovalDesc:{2} |" +
                                                    " effectiveStartDate:{3} | CMExchangeCode: {4} | CMType:{5} |",
                                                    dt[0].CMID, dt[0].ExchangeId, ApprovalDesc, dt[0].EffectiveStartDate, dt[0].CMExchangeCode, (dt[0].CMType=="B") ? "Broker" : "Trader");

                    ta.DeleteByCMExchangeID(CMExchangeID);
                    AuditTrail.AddAuditTrail("ClearingMemberExchange", AuditTrail.REJECT, logMessage, UserName, proposedFlag);
                    
                }
                scope.Complete();
            }
        }
	    catch (Exception ex)
	    {
    		throw ex;
	    }
        
    }
 
    public static ClearingMemberExchangeData.ClearingMemberExchangeDataTable fillLookup(decimal CMID,
                                                           string CMExchangeCode, string CMType)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        try
        {
            return ta.GetDataByCMExchangeLookup(CMExchangeCode, CMType);
        }
        catch (Exception ex)
        {
            throw ex;
            throw;
        }
    }

    public static String GetCMType(decimal CMID)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
        string exchangeType = "";
        string membershipType = "";
        try
        {
            dt = ta.GetDataByCMID(CMID);
            foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in dt)
            {
                exchangeType = Exchange.GetExchangeType(dr.ExchangeId);
                if (exchangeType == "S")
                {
                    membershipType = dr.CMType;
                }
            }
            return membershipType;
        }
        catch (Exception ex)
        {
            throw ex;
            throw;
        }

    }

    public static String GetExchangeCode(decimal CMID)
    {
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter ta = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dt = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
        string exchangeType = "";
        string ExchangeCode = "";
        try
        {
            dt = ta.GetDataByCMID(CMID);
            foreach (ClearingMemberExchangeData.ClearingMemberExchangeRow dr in dt)
            {
                exchangeType = Exchange.GetExchangeType(dr.ExchangeId);
                if (exchangeType == "S")
                {
                    ExchangeCode = Exchange.GetExchangeCode(dr.ExchangeId);
                }
            }
            return ExchangeCode;
        }
        catch (Exception ex)
        {
            throw ex;
            throw;
        }
    }
    
}
