using System;
using System.Collections.Generic;
using System.Transactions;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for IRCA
/// </summary>
public class IRCA
{
    public static void ProcessIRCA(DateTime effectiveStart, IRCAData.IRCADataTable dtNewIRCA, string userName)
    {
        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();
        IRCAData.IRCADataTable dtIRCA = new IRCAData.IRCADataTable();
        IRCAData.IRCARow drNewIRCA = null;
        ta.FillByEffectiveStartDate(dtIRCA, effectiveStart);

        try
        {
            foreach (IRCAData.IRCARow dr in dtNewIRCA)
            {

                if (dr.ActionFlag == "I")
                {
                    drNewIRCA = dtIRCA.FindByCommodityIDEffectiveStartDateApprovalStatus(dr.CommodityID, dr.EffectiveStartDate, dr.ApprovalStatus);

                    if (drNewIRCA == null)
                    {
                        drNewIRCA = dtIRCA.NewIRCARow();
                    }

                    drNewIRCA.CommodityID = dr.CommodityID;
                    drNewIRCA.EffectiveStartDate = effectiveStart;
                    drNewIRCA.ApprovalStatus = dr.ApprovalStatus;
                    drNewIRCA.IRCAValue = dr.IRCAValue;
                    drNewIRCA.CreatedBy = dr.CreatedBy;
                    drNewIRCA.CreatedDate = dr.CreatedDate;
                    drNewIRCA.LastUpdatedBy = dr.LastUpdatedBy;
                    drNewIRCA.LastUpdatedDate = dr.LastUpdatedDate;
                    if (!dr.IsEffectiveEndDateNull())
                    {
                        drNewIRCA.EffectiveEndDate = dr.EffectiveEndDate;
                    }
                    if (!dr.IsApprovalDescNull())
                    {
                        drNewIRCA.ApprovalDesc = dr.ApprovalDesc;
                    }
                    if (!dr.IsOriginalIDNull())
                    {
                        drNewIRCA.OriginalID = dr.OriginalID;
                    }
                    else
                    {
                        drNewIRCA.OriginalID = 0;
                    }

                    if (!dr.IsActionFlagNull())
                    {
                        drNewIRCA.ActionFlag = dr.ActionFlag;
                    }

                    if (drNewIRCA.RowState == System.Data.DataRowState.Detached)
                    {
                        dtIRCA.AddIRCARow(drNewIRCA);
                    }

                    ta.Update(dtIRCA);

                    string logMessage = string.Format("Proposed Insert, CommodityID:{0} | " +
                                                       "EffectiveStartDate:{1} | " +
                                                           "IRCAValue:{2}",
                                                           dr.CommodityID,
                                                           effectiveStart.ToShortDateString(),
                                                           dr.IRCAValue);
                    AuditTrail.AddAuditTrail("IRCA", AuditTrail.PROPOSE, logMessage, userName,"Insert");
                }
                else if (dr.ActionFlag == "U")
                {
                    drNewIRCA = dtIRCA.FindByCommodityIDEffectiveStartDateApprovalStatus(dr.CommodityID, dr.EffectiveStartDate, dr.ApprovalStatus);

                    if (drNewIRCA == null)
                    {
                        drNewIRCA = dtIRCA.NewIRCARow();
                    }

                    drNewIRCA.CommodityID = dr.CommodityID;
                    drNewIRCA.EffectiveStartDate = effectiveStart;
                    drNewIRCA.ApprovalStatus = dr.ApprovalStatus;
                    drNewIRCA.IRCAValue = dr.IRCAValue;
                    drNewIRCA.CreatedBy = dr.CreatedBy;
                    drNewIRCA.CreatedDate = dr.CreatedDate;
                    drNewIRCA.LastUpdatedBy = dr.LastUpdatedBy;
                    drNewIRCA.LastUpdatedDate = dr.LastUpdatedDate;
                    if (!dr.IsEffectiveEndDateNull())
                    {
                        drNewIRCA.EffectiveEndDate = dr.EffectiveEndDate;
                    }
                    if (!dr.IsApprovalDescNull())
                    {
                        drNewIRCA.ApprovalDesc = dr.ApprovalDesc;
                    }
                    if (!dr.IsOriginalIDNull())
                    {
                        drNewIRCA.OriginalID = dr.OriginalID;
                    }
                    else
                    {
                        drNewIRCA.OriginalID = 0;
                    }

                    if (!dr.IsActionFlagNull())
                    {
                        drNewIRCA.ActionFlag = dr.ActionFlag;
                    }

                    if (drNewIRCA.RowState == System.Data.DataRowState.Detached)
                    {
                        dtIRCA.AddIRCARow(drNewIRCA);
                    }

                    ta.Update(dtIRCA);

                    string logMessage = string.Format("Proposed Update, CommodityID:{0} | " +
                                                       "EffectiveStartDate:{1} | " +
                                                           "IRCAValue:{2}",
                                                           dr.CommodityID,
                                                           effectiveStart.ToShortDateString(),
                                                           dr.IRCAValue);
                    string ActionFlagDesc = "";
                    switch (dr.ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("IRCA", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                }
            }
            
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                throw new ApplicationException("Record Already Exist.");
            }
            else
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }

    public static IRCAData.CommodityDataTable GetCommodityByEffectiveStartDate(DateTime effectiveStart)
    {
        IRCAData.CommodityDataTable dt = new IRCAData.CommodityDataTable();
        IRCADataTableAdapters.CommodityTableAdapter ta = new IRCADataTableAdapters.CommodityTableAdapter();

        try
        {
            ta.FillByStartDate(dt, effectiveStart);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static IRCAData.IRCADataTable GetIRCAByCriteria(DateTime startDate,decimal commodityID)
    {
        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();
        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();

        try
        {
            ta.FillByAllCriteria(dt, startDate, commodityID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static IRCAData.IRCADataTable GetIRCAByEffectiveStartDate(DateTime effectiveStart)
    {
        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();
        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();
     
        try
        {
            ta.FillByEffectiveStartDate(dt, effectiveStart);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static IRCAData.IRCACreateEditDataTable FillForCreate(DateTime startDate)
    {
        IRCADataTableAdapters.IRCACreateEditTableAdapter ta = new IRCADataTableAdapters.IRCACreateEditTableAdapter();
        try
        {
            return ta.GetDataCreate(startDate);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static string GetCommodityNameByCommID(decimal commID)
    {
        IRCADataTableAdapters.CommodityTableAdapter ta = new IRCADataTableAdapters.CommodityTableAdapter();
        try
        {
            return ta.GetCommNameByCommID(commID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public static IRCAData.IRCADataTable GetIRCAByTransaction(DateTime effectiveStart)
    {
        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();
        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();

        try
        {
            ta.FillByEffectiveStartDateApproval(dt, effectiveStart);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    public static IRCAData.IRCARow SelectIRCAByIRCAID(decimal IRCAID)
    {
        IRCADataTableAdapters.IRCATableAdapter ta = new IRCADataTableAdapters.IRCATableAdapter();
        IRCAData.IRCADataTable dt = new IRCAData.IRCADataTable();
        IRCAData.IRCARow dr = null;
        try
        {
            ta.FillByIrcaID(dt, IRCAID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }

    public static IRCAData.IRCACreateEditDataTable GetIRCAByEffectiveStartDateAndApprovalStatus(Nullable<DateTime> effectiveStartDate, string approvalStatus)
    {
        IRCAData.IRCACreateEditDataTable dt = new IRCAData.IRCACreateEditDataTable();
        IRCADataTableAdapters.IRCACreateEditTableAdapter ta = new IRCADataTableAdapters.IRCACreateEditTableAdapter();

        try
        {
            ta.FillEffectiveStartDateAndApprovalStatus(dt, approvalStatus,effectiveStartDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool isValidDecimal(string ircaValue, string CommName)
    {
        try
        {
            string strRegex = @"^(?=.*[1-9].*$)^(-)?\d+(\.\d\d)*\d{0,18}(?:\.\d{0,6})?$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(ircaValue))
            {
                return true;
            }
            else
            {
                throw new ApplicationException(string.Format("IRCA value for {0} is not valid.", CommName));
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(string.Format("IRCA value for {0} is not valid.", CommName), ex);
        }
    }
   


    public static void Reject(List<string> IRCAValueList, DateTime effectiveStartDate, string userName)
    {
        IRCADataTableAdapters.IRCATableAdapter taIRCA = new IRCADataTableAdapters.IRCATableAdapter();
        IRCAData.IRCADataTable dtIRCA = new IRCAData.IRCADataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string[] ircaArr;
                foreach (string item in IRCAValueList)
                {
                    string logMessage = "";
                    ircaArr = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    taIRCA.FillByIrcaID(dtIRCA, Convert.ToDecimal(ircaArr[6]));
                    string ActionFlagDesc = "";
                    if (dtIRCA.Count > 0)
                    {
                        logMessage = string.Format("Rejected: {0}|{1}|{2}",
                                                          dtIRCA[0].EffectiveStartDate,
                                                           dtIRCA[0].IRCAValue,
                                                            dtIRCA[0].IsEffectiveEndDateNull() ? "" : dtIRCA[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                        
                        switch (dtIRCA[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                    }

                    taIRCA.DeleteRejectItem(Convert.ToDecimal(ircaArr[6]));
                    AuditTrail.AddAuditTrail("IRCA", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }



    public static void Approve(List<string> IRCAValueList,
                           DateTime effectiveStartDate, string userName)
    {
        IRCADataTableAdapters.IRCATableAdapter taIRCA = new IRCADataTableAdapters.IRCATableAdapter();
        IRCAData.IRCADataTable dtIRCA = new IRCAData.IRCADataTable();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string[] ircaArr;
                foreach (string item in IRCAValueList)
                {
                    ircaArr = item.Split(new string[] { "|" }, StringSplitOptions.None);

                    taIRCA.FillByIrcaID(dtIRCA, Convert.ToDecimal(ircaArr[6]));

                    decimal prevOriginalID = Convert.ToDecimal(taIRCA.GetOriginalIDPrevDate(dtIRCA[0].EffectiveStartDate,
                                       Convert.ToDecimal(dtIRCA[0].CommodityID), dtIRCA[0].OriginalID));
                 
                    DateTime? nextStartDate = Convert.ToDateTime(taIRCA.GetNextStartDate(Convert.ToDecimal(dtIRCA[0].CommodityID),
                       dtIRCA[0].EffectiveStartDate, dtIRCA[0].OriginalID));


                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        taIRCA.UpdateEndDateByIRCAID(dtIRCA[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dtIRCA[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    
                    IRCAData.IRCARow dr = IRCA.SelectIRCAByIRCAID(Convert.ToDecimal(ircaArr[6]));

                    if (dr.ActionFlag == "I")
                    {
                        taIRCA.ApproveInsert("A", ircaArr[5], null, dr.IRCAValue, userName, DateTime.Now, Convert.ToDecimal(ircaArr[6]));
                        string logMessage = string.Format("Approve Insert, CommodityID:{0} | " +
                                                      "EffectiveStartDate:{1} | " +
                                                      "IRCAValue:{2}",
                                                      dr.CommodityID,
                                                      dr.EffectiveStartDate,
                                                      dr.IRCAValue);
                        AuditTrail.AddAuditTrail("IRCA", AuditTrail.APPROVE, logMessage, userName, "Approve Insert");
                    }
                    else if (dr.ActionFlag == "U")
                    {
                        taIRCA.ApproveUpdate("A", dr.IRCAValue, userName, DateTime.Now, ircaArr[5],
                                             dr.OriginalID);
                        
                        taIRCA.DeleteByIrcaID(Convert.ToDecimal(dr.IRCAID));
                        string logMessage = string.Format("Approve Update, CommodityID:{0} | " +
                                                      "EffectiveStartDate:{1} | " +
                                                      "IRCAValue:{2}",
                                                      dr.CommodityID,
                                                      dr.EffectiveStartDate,
                                                      dr.IRCAValue);
                        AuditTrail.AddAuditTrail("IRCA", AuditTrail.APPROVE, logMessage, userName, "Approve Update");
                    }
                }
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                throw new ApplicationException("Record Already Exist. Please input new record.");
            }
            else
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }

    public static void ImportIrcaAutoApprv(IRCAData.IRCADataTable dt, string userName)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                IRCADataTableAdapters.IRCATableAdapter ta =
                    new IRCADataTableAdapters.IRCATableAdapter();

                ta.Update(dt);
                ta.uspIrcaCloseLatestCommodity(userName);

                string logMessage = string.Format("Import IRCA Auto-Approve", "");
                AuditTrail.AddAuditTrail("IRCA", "Insert", logMessage, dt[0].CreatedBy, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }


}
