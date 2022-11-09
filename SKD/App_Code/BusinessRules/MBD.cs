using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for MBD
/// </summary>
public class MBD
{
    public static MBDData.MBDDataTable FillByMBDID(decimal MBDID)
    {
        try
        {
            MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();
            return ta.GetDataByMBDID(MBDID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static MBDData.MBDViewDataTable FillByMBDID2(decimal MBDID)
    {
        try
        {
            MBDDataTableAdapters.MBDViewTableAdapter ta = new MBDDataTableAdapters.MBDViewTableAdapter();
            return ta.GetDataByMBDID(MBDID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static MBDData.MBDDataTable FillBySearchCriteria(Nullable<decimal> CMID, string approvalStatus)
    {
        try
        {
            MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();
            if (approvalStatus == "")
            {
                return ta.GetDataBySearchCriteria(CMID, null);
            }
            else
            {
                return ta.GetDataBySearchCriteria(CMID, approvalStatus);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //get all data from dataset to datagrid
    public static MBDData.MBDRow SelectMBDByMBDID(decimal mbdID)
    {
        MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();
        MBDData.MBDDataTable dt = new MBDData.MBDDataTable();
        MBDData.MBDRow dr = null;
        try
        {
            ta.FillByMBDID(dt, mbdID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange data");
        }
    }

    public static void Proposed(decimal CMID, DateTime startDate, decimal mbdValue, decimal mbdFund,
                                      string action, string userName, Nullable<decimal> originalMBDID)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();

                ta.Insert(CMID, DateTime.Now,startDate,"P",mbdValue,mbdFund,userName,DateTime.Now,
                          null, null, originalMBDID, action);
               
                if (action == "I")
                {
                    action = "Insert";
                }
                else if (action == "U")
                {
                    action = "Update";
                }
                else if (action == "D")
                {
                    action = "Delete";
                }

                string logMessage = string.Format("Proposed " + action + ", CMID:{0} | " +
                                                 "Effective Start Date:{1} | " +
                                                 "MBDValue:{2} | " +
                                                 "MBDFund:{3}", CMID.ToString(),
                                                 startDate.ToString("dd-MM-yyyy"),
                                                 mbdValue.ToString(), mbdFund.ToString());

                AuditTrail.AddAuditTrail("MBD", AuditTrail.PROPOSE, logMessage, userName, action);
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Approve(decimal MBDID, string approvalDesc, string action, string userName)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();
                MBDData.MBDDataTable dt = new MBDData.MBDDataTable();

                dt = ta.GetDataByMBDID(MBDID);

                if (dt.Count > 0)
                {
                    if (action == "I")
                    {
                        ta.ApproveInsert("A", approvalDesc, null, null, MBDID);
                    }
                    else if (action == "U")
                    {
                        ta.ApproveUpdate(dt[0].CMID, dt[0].UploadDate, dt[0].EffectiveStartDate, "A",
                                         dt[0].MBDValue, dt[0].MBDFund, dt[0].EffectiveEndDate, 
                                         approvalDesc,
                                         null, null,dt[0].OriginalMBDID);
                    }
                    if (action == "I")
                    {
                        action = "Insert";
                    }
                    else
                    {
                        action = "Update";
                    }

                    string logMessage = string.Format("Approved "+ action + ", CMID:{0} | " +
                                                  "Effective Start Date:{1} | " +
                                                  "MBDValue:{2} | " +
                                                  "MBDFund:{3}", dt[0].CMID,
                                                  dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                  dt[0].MBDValue.ToString(), 
                                                  dt[0].MBDFund.ToString());
                    

                    AuditTrail.AddAuditTrail("MBD", AuditTrail.APPROVE, logMessage, userName,action);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Reject(decimal MBDID, string action, string userName)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                MBDDataTableAdapters.MBDTableAdapter ta = new MBDDataTableAdapters.MBDTableAdapter();
                MBDData.MBDDataTable dt = new MBDData.MBDDataTable();

                dt = ta.GetDataByMBDID(MBDID);

                if (dt.Count > 0)
                {
                    ta.DeleteByMBDID(MBDID);
                    ta.DeleteByMBDID(dt[0].OriginalMBDID);

                    string logMessage = string.Format("Reject, CMID:{0} | " +
                                                  "Effective Start Date:{1} | " +
                                                  "MBDValue:{2} | " +
                                                  "MBDFund:{3}", dt[0].CMID,
                                                  dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                  dt[0].MBDValue.ToString(),
                                                  dt[0].MBDFund.ToString());
                    if (action == "I")
                    {
                        action = "Insert";
                    }
                    else if (action == "U")
                    {
                        action = "Update";
                    }
                     else if (action == "D")
                    {
                        action = "Delete";
                    }
                    AuditTrail.AddAuditTrail("MBD", AuditTrail.REJECT, logMessage, userName, "Reject " +action);
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
