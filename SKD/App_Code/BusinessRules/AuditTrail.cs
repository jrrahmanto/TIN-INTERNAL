using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
/// <summary>
/// Summary description for AuditTrail
/// </summary>
public class AuditTrail
{
	public AuditTrail()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public const string PROPOSE = "Propose";
    public const string APPROVE = "Approve";
    public const string REJECT = "Reject";

    public static void AddAuditTrail(string applicationTable, string userAction, string logMessage, 
                                     string userName, string action)
    {
        AuditTrailDataTableAdapters.AuditTrailTableAdapter ta = new AuditTrailDataTableAdapters.AuditTrailTableAdapter();

        try
        {
            ta.Insert(DateTime.Now, applicationTable, userAction, logMessage, userName);
           
            ApplicationLog.Insert(DateTime.Now, applicationTable, "I", action, userName,  HttpContext.Current.Session["ClientIPAddress"].ToString());
        }
        catch (Exception ex)
        {
            
            throw new ApplicationException(ex.Message);
        }
    }

    public static AuditTrailData.ApplicationTableDDLDataTable GetApplicationTableDDL()
    {
        AuditTrailDataTableAdapters.ApplicationTableDDLTableAdapter ta = new AuditTrailDataTableAdapters.ApplicationTableDDLTableAdapter();

        try
        {
            return ta.GetData();
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message);
        }
    }

    public static AuditTrailData.UserActionDDLDataTable GetUserActionDDL()
    {
        AuditTrailDataTableAdapters.UserActionDDLTableAdapter ta = new AuditTrailDataTableAdapters.UserActionDDLTableAdapter();

        try
        {
            return ta.GetData();
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message);
        }
    }

    public static AuditTrailData.AuditTrailDataTable SearchAuditTrail(string applicationTable, string userAction, 
                                                                      DateTime logTimeFrom, DateTime LogTimeTo, string userName)
    {
        AuditTrailDataTableAdapters.AuditTrailTableAdapter ta = new AuditTrailDataTableAdapters.AuditTrailTableAdapter();

        try
        {
            return ta.GetDataBySearchCriteria(logTimeFrom,LogTimeTo, applicationTable, userAction, userName);
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message);
        }
    }
    public static AuditTrailData.AuditTrailDataTable SearchAuditTrailByID(decimal auditTrailID)
    {
        AuditTrailDataTableAdapters.AuditTrailTableAdapter ta = new AuditTrailDataTableAdapters.AuditTrailTableAdapter();

        try
        {
            return ta.GetDataByAuditTrailID(auditTrailID);
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message);
        }
    }

}
