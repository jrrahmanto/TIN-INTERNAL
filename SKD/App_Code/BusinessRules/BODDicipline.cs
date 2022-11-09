using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions; 

/// <summary>
/// Summary description for BODDicipline
/// </summary>
public class BODDicipline
{
    public static BODDiciplineData.BODDiciplineDataTable FillBySearchCriteria(Nullable<decimal> CMID, string name,
                                                                              string appStatus)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();
        try
        {
            return ta.GetDataBySearchCriteria(CMID, name, appStatus);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static BODDiciplineData.BODDiciplineDataTable FillByBODDiciplineID(decimal BODDiciplineID)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();
        try
        {
            return ta.GetDataByBODDisciplineID(BODDiciplineID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ProposedInsert(string diciplineNO, string desc, string userName, decimal BODID)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(diciplineNO, "P", desc, userName, DateTime.Now, userName, 
                          DateTime.Now, null, BODID,null, "I");
                string logMessage = string.Format("Proposed Insert, DiciplineNo:{0} | Description:{1} | " +
                                                  " BODID:{3}", diciplineNO, desc, BODID);
                AuditTrail.AddAuditTrail("BODDicipline", AuditTrail.PROPOSE, logMessage, userName, "Insert");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    public static void ProposedUpdate(string diciplineNO, string desc, string userName,
                                      decimal BODID, decimal originalDiciplineID)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(diciplineNO, "P", desc, userName, DateTime.Now, userName,
                          DateTime.Now, null, BODID, originalDiciplineID, "U");
                string logMessage = string.Format("Proposed Update, DiciplineNo:{0} | Description:{1} | " +
                                                  " BODID:{3}", diciplineNO,desc,BODID);
                AuditTrail.AddAuditTrail("BODDicipline", AuditTrail.PROPOSE, logMessage, userName, "Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public static void ProposedDelete(string diciplineNO, string desc, string userName, decimal BODID,
                                      decimal originalDiciplineID)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(diciplineNO, "P", desc, userName, DateTime.Now, userName,
                          DateTime.Now, null, originalDiciplineID, null, "U");
                string logMessage = string.Format("Proposed Update, DiciplineNo:{0} | Description:{1} | " +
                                                  " BODID:{3}", diciplineNO, desc, BODID);
                AuditTrail.AddAuditTrail("BODDicipline", AuditTrail.PROPOSE, logMessage, userName, "Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public static void Approved(decimal DiciplineID, string appDesc, string action, string userName)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();
        BODDiciplineData.BODDiciplineDataTable dt = new BODDiciplineData.BODDiciplineDataTable();
        try
        {
            ta.FillByDiciplineID(dt, DiciplineID);
            using (TransactionScope scope = new TransactionScope())
            {
                if (dt.Count > 0)
                {
                    if (action == "Insert")
                    {
                        ta.ApproveInsert("A", appDesc, null, null, DiciplineID);
                    }
                    else if (action == "Update")
                    {
                        ta.DeleteBODDiciplin(DiciplineID);
                        ta.ApproveUpdate(dt[0].BODDiciplineNo,"A",dt[0].Description,userName,DateTime.Now,
                                         appDesc,dt[0].BODID,null,null,dt[0].OriginalBODDiciplineID);
                    }
                    else if (action == "Delete")
                    {
                        ta.DeleteBODDiciplin(DiciplineID);
                        ta.DeleteBODDiciplin(dt[0].OriginalBODDiciplineID);
                    }

                    string logMessage = string.Format("Proposed " + action + ", DiciplineNo:{0} | Description:{1} | " +
                                                 " BODID:{2}", dt[0].BODDiciplineNo, dt[0].Description, dt[0].BODID);
                    AuditTrail.AddAuditTrail("BODDicipline", AuditTrail.PROPOSE, logMessage, userName,action);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dt.Dispose();
        }

    }

    public static void Reject(decimal DiciplineID, string appDesc, string action, string userName)
    {
        BODDiciplineDataTableAdapters.BODDiciplineTableAdapter ta = new BODDiciplineDataTableAdapters.BODDiciplineTableAdapter();
        BODDiciplineData.BODDiciplineDataTable dt = new BODDiciplineData.BODDiciplineDataTable();
        try
        {
            ta.FillByDiciplineID(dt, DiciplineID);
            using (TransactionScope scope = new TransactionScope())
            {
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    ta.DeleteBODDiciplin(DiciplineID);

                    switch (dt[0].Action)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    string logMessage = string.Format("Proposed " + action + ", DiciplineNo:{0} | Description:{1} | " +
                                                 " BODID:{2}", dt[0].BODDiciplineNo, dt[0].Description, dt[0].BODID);
                    AuditTrail.AddAuditTrail("BODDicipline", AuditTrail.REJECT, logMessage, userName, ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dt.Dispose();
        }

    }
}
