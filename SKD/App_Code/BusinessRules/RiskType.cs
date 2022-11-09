using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for RiskType
/// </summary>
public class RiskType
{

    public static RiskTypeData.RiskTypeDataTable SelectRiskTypeByRiskTypeCode(string riskTypeCode)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();
        RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
        try
        {
            ta.FillByRiskTypeCodeAndApprovalStatus(dt, riskTypeCode, null);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load risktype data");
        }
    }

    public static RiskTypeData.RiskTypeDataTable GetRiskTypeByRiskTypeCodeAndApprovalStatus(string riskTypeCode, string approvalStatus)
    {
        RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            ta.FillByRiskTypeCodeAndApprovalStatus(dt, riskTypeCode, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static RiskTypeData.RiskTypeRow SelectRiskTypeByRiskTypeID(decimal riskTypeId)
    {      
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();
        RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
        RiskTypeData.RiskTypeRow dr = null;
      
        try
        {
            ta.FillByRiskTypeID(dt, riskTypeId);

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

    public static void InsertRiskType(string riskTypeCode, string riskType, string approvalDesc,
        DateTime startDate, Nullable<DateTime> endDate,
        string description, string action, string createdBy, 
        DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate , decimal OriginalID)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            ta.Insert(riskTypeCode, "A", startDate, riskType, description, createdBy, 
                createdDate, lastUpdatedBy, lastUpdatedDate, endDate, null, action, OriginalID);
        }
        catch (Exception ex)
        {            
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateRiskType(string riskType, string description, string lastUpdatedBy, 
        DateTime lastUpdatedDate, string riskTypeCode, string approvalStatus, DateTime startDate)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            ta.UpdateRiskType(riskType, description, lastUpdatedBy, lastUpdatedDate, 
                riskTypeCode, approvalStatus, startDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex); 
        }
    }

    public static void DeleteRiskTypeByRiskTypeID(decimal riskTypeId)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            ta.DeleteRiskTypeByRiskTypeID(riskTypeId);
        }
        catch (Exception ex)
        {            
            throw new ApplicationException(ex.Message,ex);
        }
    }

    public static void ProposeRiskType(string riskTypeCode, string riskType, string approvalDesc,
                                          DateTime startDate, Nullable<DateTime> endDate,
                                          string description, string action, string userName, decimal OriginalID)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                    ta.Insert(riskTypeCode, "P", startDate, riskType, description, userName,
                            DateTime.Now, userName, DateTime.Now, endDate, approvalDesc, action, OriginalID);
                    string ActionFlagDesc = "";
                    switch (action)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Proposed Value: RiskTypeCode={0}|RiskType={1}|Description={2}|EffectiveStartDate={3}|EffectiveEndDate={4}",
                                            riskTypeCode.ToString(), riskType.ToString(),
                                            description.ToString(), startDate.ToString("dd-MM-yyyy"),
                                            Convert.ToDateTime(endDate).Date);
                    AuditTrail.AddAuditTrail("RiskType", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);
                
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist or in pending approval.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }

    }


    public static void RejectProposedRiskTypeID(decimal riskTypeID, string userName)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();
        RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByRiskTypeID(dt, riskTypeID);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    ActionFlagDesc = dt[0].ActionFlag;
                    logMessage = string.Format("Proposed Delete:  RiskTypeCode={0}|RiskType={1}|Description={2}|EffectiveStartDate={3}|EffectiveEndDate={4}",
                                                               dt[0].RiskTypeCode,
                                                               dt[0].RiskType,
                                                               dt[0].Description,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                }

                ta.DeleteRejectItem(riskTypeID);

                AuditTrail.AddAuditTrail("RiskType", AuditTrail.PROPOSE, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


    public static void ApproveRiskTypeID(decimal riskTypeID, string userName)
    {
        RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();
        RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();

        try
        {
            try
            {
                ta.FillByRiskTypeID(dt, riskTypeID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevDate(dt[0].EffectiveStartDate,
                                         dt[0].RiskTypeCode, dt[0].OriginalID));
                
                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(dt[0].RiskTypeCode,
                                        dt[0].EffectiveStartDate, dt[0].OriginalID));


                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";


                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByRiskTypeID(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }


                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                       if (dt[0].IsEffectiveEndDateNull())
                            {
                                ta.ApprovedProposedItem(dt[0].RiskTypeCode, "A", dt[0].EffectiveStartDate,
                                                    dt[0].RiskType, dt[0].Description, userName, DateTime.Now, userName, DateTime.Now, 
                                                    null, dt[0].ApprovalDesc, null, dt[0].RiskTypeID);
                            }
                       else
                            {
                                ta.ApprovedProposedItem(dt[0].RiskTypeCode, "A", dt[0].EffectiveStartDate,
                                                    dt[0].RiskType, dt[0].Description, userName, DateTime.Now, userName, DateTime.Now,
                                                    dt[0].EffectiveEndDate, dt[0].ApprovalDesc, null, dt[0].RiskTypeID);
                            }

                       logMessage = string.Format("Approved Insert: RiskTypeCode={0}|RiskType={1}|Description={2}|EffectiveStartDate={3}|EffectiveEndDate={4}",
                                                              dt[0].RiskTypeCode,
                                                              dt[0].RiskType,
                                                              dt[0].Description,
                                                              dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                              dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                      if (dt[0].IsEffectiveEndDateNull())
                        {
                                ta.ApprovedUpdateProposedItem(dt[0].RiskTypeCode, "A", dt[0].EffectiveStartDate,
                                                            dt[0].RiskType, dt[0].Description, userName, DateTime.Now, userName, DateTime.Now, 
                                                            null, dt[0].ApprovalDesc, null, dt[0].OriginalID, dt[0].RiskTypeID);
                        }
                        else
                        {
                                ta.ApprovedUpdateProposedItem(dt[0].RiskTypeCode, "A", dt[0].EffectiveStartDate,
                                                            dt[0].RiskType, dt[0].Description, userName, DateTime.Now, userName, DateTime.Now, 
                                                            dt[0].EffectiveEndDate, dt[0].ApprovalDesc, null, dt[0].OriginalID, dt[0].RiskTypeID);
                        }
                     
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].RiskTypeID);

                        logMessage = string.Format("Approved Update: RiskTypeCode={0}|RiskType={1}|Description={2}|EffectiveStartDate={3}|EffectiveEndDate={4}",
                                                              dt[0].RiskTypeCode,
                                                              dt[0].RiskType,
                                                              dt[0].Description,
                                                              dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                              dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                    
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].RiskTypeID);

                        logMessage = string.Format("Approved Delete: RiskTypeCode={0}|RiskType={1}|Description={2}|EffectiveStartDate={3}|EffectiveEndDate={4}",
                                                              dt[0].RiskTypeCode,
                                                              dt[0].RiskType,
                                                              dt[0].Description,
                                                              dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                              dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("RiskType", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                errorMessage = "Record is already exist or in pending approval";
            }
            throw new ApplicationException(errorMessage);
        }
    }


}
