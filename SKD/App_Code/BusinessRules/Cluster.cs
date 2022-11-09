using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Cluster
/// </summary>
public class Cluster
{
    public static ClusterData.ClusterDataTable FillBySearchCriteria(decimal CMID, string approvalStatus)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();

        try
        {
            return ta.GetDataBySearchCriteria(CMID, approvalStatus);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ValidateRecord(decimal EditedClusterID, string ApprovalStatus)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        ClusterData.ClusterDataTable dt = new ClusterData.ClusterDataTable();

        ta.FillByClusterID(dt, EditedClusterID);

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

    public static void ProposedCluster(decimal TraderCMID, decimal BrokerCMID, decimal CommodityID,
                                        DateTime StartDate, Nullable<DateTime> EndDate, string UserName, 
                                        string ActionFlag, Nullable<decimal> OriginalClusterID)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        string ActionFlagDesc = "";

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (OriginalClusterID.HasValue)
                    ValidateRecord(Convert.ToDecimal(OriginalClusterID), "P");

                switch (ActionFlag)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                ta.Insert1(TraderCMID, BrokerCMID, CommodityID, StartDate, "P",
                        UserName, DateTime.Now, UserName, DateTime.Now, EndDate, null, OriginalClusterID, ActionFlag);

                
                string logMessage = string.Format(ActionFlagDesc + " , TraderCMID:{0}| BrokerCMID:{1}|" +
                                           " ContractID:{2}| StartDate:{3}| EndDate:{4}| OriginalID:{5}",
                                            TraderCMID.ToString(), BrokerCMID.ToString(),
                                            CommodityID.ToString(), StartDate.ToString("dd-MMM-yyyy"),
                                            EndDate.ToString(), OriginalClusterID);

                AuditTrail.AddAuditTrail("Cluster", AuditTrail.PROPOSE, logMessage, UserName, ActionFlagDesc);
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

    public static void Approve(decimal ClusterID, string UserName, string ApprovalDesc)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        ClusterData.ClusterDataTable dt = new ClusterData.ClusterDataTable();
        ClusterData.ClusterDataTable dtDate = new ClusterData.ClusterDataTable();
        string ActionFlagDesc = "";
        Nullable<DateTime> EndDate = null;

        ValidateRecord(ClusterID, "A");

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByClusterID(dt, ClusterID);

                if (dt.Count > 0)
                {
                    // TODO: Guard for conflict effectiveness date
                    //ta.FillNextDate(dtDate, dt[0].TraderCMID, dt[0].BrokerCMID, dt[0].ContractID, dt[0].EffectiveStartDate);
                    //if (dtDate.Count > 0)
                    //{
                    //    ta.UpdateStartDate(dt[0].EffectiveStartDate.AddDays(-1), ClusterID);
                    //    //ta.UpdateStartDate(dt[0].EffectiveStartDate.AddDays(-1), traderCMID,
                    //    //                   brokerCMID, contractID, startDate, "A");
                    //    dtDate.Clear();
                    //    dtDate.AcceptChanges();
                    //}
                    //ta.FillPrevDate(dtDate, dt[0].TraderCMID, dt[0].BrokerCMID, dt[0].ContractID, dt[0].EffectiveStartDate);
                    //if (dtDate.Count > 0)
                    //{
                    //    ta.UpdateEndDate(dt[0].EffectiveEndDate.AddDays(-1), ClusterID);
                    //    //ta.UpdateEndDate(dt[0].EffectiveEndDate.AddDays(-1), traderCMID,
                    //    //                  brokerCMID, contractID, startDate, "A");
                    //    dtDate.Clear();
                    //    dtDate.AcceptChanges();
                    //}

                    if (!dt[0].IsActionFlagNull())
                    {
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Approve Insert"; break;
                            case "U": ActionFlagDesc = "Approve Update"; break;
                            case "D": ActionFlagDesc = "Approve Delete"; break;
                        }
                    }
                    if (!dt[0].IsEffectiveEndDateNull())
                        EndDate = dt[0].EffectiveEndDate;

                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApproveInsert("A", ApprovalDesc, null, null, UserName, DateTime.Now, ClusterID);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApproveUpdate(dt[0].TraderCMID, dt[0].BrokerCMID, dt[0].CommodityID, dt[0].EffectiveStartDate, "A",
                                UserName, DateTime.Now, EndDate, ApprovalDesc, null, null, dt[0].OriginalID);
                        ta.DeleteByClusterID(ClusterID);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteByClusterID(dt[0].OriginalID);
                        ta.DeleteByClusterID(ClusterID);
                    }

                    string logMessage = string.Format(ActionFlagDesc +
                                            " , TraderCMID:{0}| BrokerCMID:{1}" +
                                            "| ContractID: {2}| StartDate: {3}| EndDate: {4}",
                                            dt[0].TraderCMID, dt[0].BrokerCMID, dt[0].CommodityID, dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"),
                                            EndDate.ToString());
                    AuditTrail.AddAuditTrail("Cluster", ActionFlagDesc, logMessage, UserName, ActionFlagDesc);
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

    public static void Reject(decimal ClusterID, string UserName, string ApprovalDesc)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        ClusterData.ClusterDataTable dt = new ClusterData.ClusterDataTable();
        string ActionFlagDesc = "";

        ValidateRecord(ClusterID, "R");

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByClusterID(dt, ClusterID);
                if (dt.Count > 0)
                {
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Reject Insert"; break;
                        case "U": ActionFlagDesc = "Reject Update"; break;
                        case "D": ActionFlagDesc = "Reject Delete"; break;
                    }

                    ta.DeleteByClusterID(ClusterID);

                    string logMessage = string.Format(ActionFlagDesc +
                                            " , TraderCMID:{0}| BrokerCMID:{1}" +
                                            "| ContractID: {2}| StartDate: {3}| EndDate: {4}",
                                            dt[0].TraderCMID.ToString(), dt[0].BrokerCMID.ToString(),
                                            dt[0].CommodityID.ToString(), dt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"),
                                            (dt[0].IsEffectiveEndDateNull()) ? "" : dt[0].EffectiveEndDate.ToString());
                    AuditTrail.AddAuditTrail("Cluster", ActionFlagDesc, logMessage, UserName, ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int CountBrokerContract(decimal BrokerCMID, decimal contractID, DateTime startDate)
    {
        ClusterDataTableAdapters.ClusterTableAdapter ta = new ClusterDataTableAdapters.ClusterTableAdapter();
        return int.Parse(ta.CountBrokerContract(BrokerCMID,contractID, startDate).ToString());
    }

    public static int CheckOpenPosition(decimal traderCMID, decimal commodityID, DateTime startDate)
    {
        ClusterDataTableAdapters.CheckOpenPositionTableAdapter ta = new ClusterDataTableAdapters.CheckOpenPositionTableAdapter();
        ClusterData.CheckOpenPositionDataTable dt = new ClusterData.CheckOpenPositionDataTable();

        try
        {
            ta.Fill(dt, traderCMID, commodityID, startDate);

            return dt.Count;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

}
