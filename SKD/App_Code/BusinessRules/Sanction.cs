using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Sanction
/// </summary>
public class Sanction
{
    public static CMSanctionListData.CMSanctionListDataTable FillBySearchCriteria(string approvalStatus,
                                                        string sanctionSource, Nullable<decimal> CMID)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        try
        {
            return ta.GetDataByCriteria(approvalStatus,null, null);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static CMSanctionListData.CMSanctionListDataTable FillBySanctionNoAndApproval(string sanctionNo, string approvalStatus)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        try
        {
            return ta.GetDataBySanctionNoAndStatus(sanctionNo, approvalStatus);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static CMSanctionListData.CMSanctionListDataTable FillBySanctionNo(string sanctionNo)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        try
        {
            return ta.GetDataBySanctionNo(sanctionNo);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //get all data from dataset to datagrid
    public static CMSanctionListData.CMSanctionListDataTable SelectSanctionBySanctionNo(string sanctionNo, string approval)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();
        try
        {
            ta.FillBySanctionNoAndStatus(dt, sanctionNo, approval);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load sanction data");
        }
    }


    public static void ProposedInsert(string sanctionNo, decimal CMID, string description, DateTime startDate,
                                      string sanctionSource, string penaltyType, string sanctionType,
                                      DateTime sanctionDate, string createdBy, DateTime createdDate, string lastUpdateBy, DateTime lastUpdateDate,
                                      decimal fineNominal, string sanctionStatus, Nullable<DateTime> endDate, 
                                      string userName)
    {
       CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
       
       try
       {
           using (TransactionScope scope = new TransactionScope())
           {
               ta.Insert(sanctionNo, CMID, description, startDate, sanctionSource, "P", sanctionDate, penaltyType,
                    sanctionType, fineNominal, sanctionStatus, endDate, createdDate, createdBy, lastUpdateDate,
                    lastUpdateBy, null, null, "I");

               string logMessage = string.Format("Proposed Insert, SanctionNo:{0} | CMID:{1} | " +
                                                 "Description: {2} | StartDate :{3} | " +
                                                 "SanctionSource:{4} | PenaltyType:{5} | " +
                                                 "SanctionType:{6} | fineNominal:{7} | sanctionStatus:{8}" +
                                                 "SanctionDate: {9} | EndDate :{10} | " , 
                                                 sanctionNo,  CMID, description,  startDate,
                                                 sanctionSource, penaltyType, sanctionType, 
                                                 fineNominal,  sanctionStatus, sanctionDate, endDate);
               AuditTrail.AddAuditTrail("CMSanctionList", AuditTrail.PROPOSE, logMessage, userName,"Insert");
               scope.Complete();
           }    
          
       }
       catch (Exception ex)
       {

           throw new ApplicationException(ex.Message, ex);
       }
    }

    public static void ProposedUpdate(string sanctionNo, decimal CMID, string desc, DateTime startDate,
                                     string sanctionSource, string penaltyType, string sanctionType,
                                     decimal fineNominal, string sanctionStatus, Nullable<DateTime> endDate,
                                     string originalSanctionNo,  string userName)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(sanctionNo, CMID, desc, startDate, sanctionSource, "P", DateTime.Now.Date, penaltyType,
                     sanctionType, fineNominal, sanctionStatus, endDate, DateTime.Now, userName, DateTime.Now,
                     userName, null, originalSanctionNo, "U");
                string logMessage = string.Format("Proposed Update, SanctionNo:{0} | CMID:{1} | " +
                                                    "Description: {2} | StartDate :{3} | " +
                                                    "SanctionSource:{4} | PenaltyType:{5} | " +
                                                    "SanctionType:{6} | fineNominal:{7} | " +
                                                    "sanctionStatus:{8}",
                                                    sanctionNo, CMID, desc, startDate,
                                                    sanctionSource, penaltyType, sanctionType,
                                                    fineNominal, sanctionStatus);
                AuditTrail.AddAuditTrail("CMSanctionList", AuditTrail.PROPOSE, logMessage, userName,"Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProposedDelete(string sanctionNo, decimal CMID, string desc, DateTime startDate,
                                    string sanctionSource, string penaltyType, string sanctionType,
                                    decimal fineNominal, string sanctionStatus, DateTime endDate,
                                    string originalSanctionNo, string userName)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(sanctionNo, CMID, desc, startDate, sanctionSource, "P", DateTime.Now.Date, penaltyType,
                     sanctionType, fineNominal, sanctionStatus, endDate, DateTime.Now, userName, DateTime.Now,
                     userName, null, originalSanctionNo, "D");

                string logMessage = string.Format("Proposed Delete, SanctionNo:{0} | CMID:{1} | " +
                                                   "Description: {2} | StartDate :{3} | " +
                                                   "SanctionSource:{4} | PenaltyType:{5} | " +
                                                   "SanctionType:{6} | fineNominal:{7} | " +
                                                   "sanctionStatus:{8}",
                                                   sanctionNo, CMID, desc, startDate,
                                                   sanctionSource, penaltyType, sanctionType,
                                                   fineNominal, sanctionStatus);

                AuditTrail.AddAuditTrail("CMSanctionList", AuditTrail.PROPOSE, logMessage, userName,"Delete");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void Approve(string sanctionNo, string approvalDesc, string action, 
                               string userName, string approvalStatus)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter taCM = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillBySanctionNoAndStatus(dt,sanctionNo, approvalStatus);

                if (dt.Count > 0)
                {
                    if (action == "New Record")
                    {

                        ta.ApproveInsert("A", approvalDesc, sanctionNo, null);
                        if (dt[0].SanctionStatus == "S" || dt[0].SanctionStatus == "T" || dt[0].SanctionStatus == "F")
                        {
                            taCM.UpdateCMStatus(dt[0].SanctionStatus, dt[0].CMID);
                        }
                    }
                    else if (action == "Revision")
                    {
                        ta.DeleteCMSanction(sanctionNo,dt[0].ApprovalStatus);
                        ta.ApproveUpdate(dt[0].SanctionNo, dt[0].CMID, dt[0].Description, dt[0].StartDate,
                                         dt[0].SanctionSource, "A", dt[0].SanctionDate, dt[0].PenaltyType,
                                         dt[0].SanctionType, dt[0].FineNominal, dt[0].SanctionStatus, dt[0].EndDate,
                                         DateTime.Now, userName, approvalDesc, null, null, dt[0].OriginalSanctionNo);
                        if (dt[0].SanctionType == "S" || dt[0].SanctionType == "T" || dt[0].SanctionType == "F")
                        {
                            taCM.UpdateCMStatus(dt[0].SanctionType, dt[0].CMID);
                        }
                    }
                    else if (action == "Delete")
                    {
                        ta.DeleteCMSanction(sanctionNo,"A");
                        ta.DeleteCMSanction(dt[0].OriginalSanctionNo, dt[0].ApprovalStatus);
                    }
                    string logMessage = string.Format("Approve " + action + " , SanctionNo:{0} | CMID:{1} | " +
                                                "Description: {2} | StartDate :{3} | " +
                                                "SanctionSource:{4} | PenaltyType:{5} | " +
                                                "SanctionType:{6} | fineNominal:{7} | " +
                                                "sanctionStatus:{8}",
                                                sanctionNo, dt[0].CMID, dt[0].Description, dt[0].StartDate,
                                                dt[0].SanctionSource, dt[0].PenaltyType, dt[0].SanctionType,
                                                dt[0].FineNominal, dt[0].SanctionStatus);
                    AuditTrail.AddAuditTrail("CMSanctionList", AuditTrail.APPROVE, logMessage, userName,action);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void Reject(string sanctionNo, string approvalDesc, string action, 
                              string userName,string approvalStatus)
    {
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillBySanctionNoAndStatus(dt, sanctionNo, approvalStatus);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    ta.DeleteCMSanction(sanctionNo,"P");
                    string logMessage = string.Format("Reject " + action + " , SanctionNo:{0} | CMID:{1} | " +
                                                "Description: {2} | StartDate :{3} | " +
                                                "SanctionSource:{4} | PenaltyType:{5} | " +
                                                "SanctionType:{6} | fineNominal:{7} | " +
                                                "sanctionStatus:{8}",
                                                sanctionNo, dt[0].CMID, dt[0].Description, dt[0].StartDate,
                                                dt[0].SanctionSource, dt[0].PenaltyType, dt[0].SanctionType,
                                                dt[0].FineNominal, dt[0].SanctionStatus);
                    switch (action)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("CMSanctionList", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static CMSanctionListData.CMSanctionListDataTable FillBySearchCriteriaAll(string approvalStatus,
                                                       string sanctionSource, Nullable<decimal> CMID)
    {
        CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();
        CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
        try
        {
            ta.FillBySearchCriteriaAll(dt, CMID,sanctionSource, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

}
