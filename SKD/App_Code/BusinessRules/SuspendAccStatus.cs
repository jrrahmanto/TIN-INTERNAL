using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Text;
using System.Collections;


/// <summary>
/// Summary description for SuspendAccStatus
/// </summary>
public class SuspendAccStatus
{
    public SuspendAccStatus()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    public static void ProposedInsertSuspend(string code, decimal emid, string name, string userName,
                                         decimal oriInvID,string accStatus, string accReasonStatus,string suspendBy, string action)
    {
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.InsertSuspendStatus(code, emid, "P", name, userName, DateTime.Now,
                          userName, DateTime.Now, null, action, oriInvID,accStatus, accReasonStatus, suspendBy);
                string logMessage = string.Format("Proposed Insert Suspend Acc Status, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | AccountStatus:{3}",
                                                  code, emid, name, accStatus);
                AuditTrail.AddAuditTrail("SuspendAccountStatus", AuditTrail.PROPOSE, logMessage, userName, "Insert");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static void ProposedUpdateSuspend(string code, decimal emid, string name, string userName,
                                         decimal oriInvID, string accStatus, string accReasonStatus, string suspendBy, string action)
    {
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.InsertSuspendStatus(code, emid, "P", name, userName, DateTime.Now,
                          userName, DateTime.Now, null, action, oriInvID, accStatus, accReasonStatus, "KBI");
                string logMessage = string.Format("Proposed Update Suspend Acc Status, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | AccountStatus:{3}",
                                                  code, emid, name, accStatus);
                AuditTrail.AddAuditTrail("SuspendAccountStatus", AuditTrail.PROPOSE, logMessage, userName, "Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static void ProposedDeleteSuspend(string code, decimal emid, string name, string userName,
                                         decimal oriInvID, string accStatus, string accReasonStatus, string suspendBy, string action)
    {
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.InsertSuspendStatus(code, emid, "P", name, userName, DateTime.Now,
                          userName, DateTime.Now, null, action, oriInvID, accStatus, accReasonStatus, "KBI");
                string logMessage = string.Format("Proposed Update Suspend Acc Status, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | AccountStatus:{3}",
                                                  code, emid, name, accStatus);
                AuditTrail.AddAuditTrail("SuspendAccountStatus", AuditTrail.PROPOSE, logMessage, userName, "Delete");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ApproveSuspend(decimal investorID, string approvalDesc, string action, string userName
                                        ,string suspendBy, decimal originalID)
    {
        SuspendAccStatusData.InvestorDataTable dt = new SuspendAccStatusData.InvestorDataTable();
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();
        string insBond = "";
        string flagInsBond = "N";

       

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByInvestorID(dt, investorID);

                insBond = dt[0].Code.ToString().Substring(4, 2);
                if (insBond == "36" || insBond == "46")
                    flagInsBond = "Y";

                if (dt.Count > 0)
                {
                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApprovedSuspendStatus(dt[0].Code,dt[0].EMID,"A",dt[0].Name, userName,DateTime.Now,
                                                 approvalDesc, null,dt[0].AccountStatus,dt[0].AccountStatusReason,suspendBy,dt[0].OriginalInvestorID);
                        InsertTFMemberInfo(dt[0].OriginalInvestorID, userName, dt[0].AccountStatus, flagInsBond);
                        ta.DeleteSuspend(investorID);
                        
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedSuspendStatus(dt[0].Code, dt[0].EMID, "A", dt[0].Name, userName, DateTime.Now,
                                                 approvalDesc, null, dt[0].AccountStatus, dt[0].AccountStatusReason, suspendBy, dt[0].OriginalInvestorID);
                        InsertTFMemberInfo(dt[0].OriginalInvestorID, userName, dt[0].AccountStatus, flagInsBond);
                        ta.DeleteSuspend(investorID);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteSuspend(investorID);
                        
                    }
                    string logMessage = string.Format("Approve " + action + ", Code:{0} | " +
                                                      "EMID:{1} | Name:{2} | AccountStatus:{3}",
                                                      dt[0].Code, dt[0].EMID, dt[0].Name, dt[0].AccountStatus);
                    AuditTrail.AddAuditTrail("SuspendAccountStatus", AuditTrail.APPROVE, logMessage, userName, "Approve " + action);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Reject(decimal investorID, string approvalDesc, string action, string userName)
    {
        SuspendAccStatusData.InvestorDataTable dt = new SuspendAccStatusData.InvestorDataTable();
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByInvestorID(dt, investorID);
                if (dt.Count > 0)
                {
                    ta.DeleteSuspend(investorID);
                    string logMessage = string.Format("Delete " + action + ", Code:{0} | " +
                                                      "EMID:{1} | Name:{2} | AccountStatus:{3}",
                                                      dt[0].Code, dt[0].EMID, dt[0].Name, dt[0].AccountStatus);
                    string ActionFlagDesc = "";
                    switch (action)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Investor", "Delete " + action, logMessage, userName, "Reject " + ActionFlagDesc);
                    scope.Complete();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static SuspendAccStatusData.InvestorDataTable FillBySearchCriteria(string code, string approvalStatus)
    {
        SuspendAccStatusData.InvestorDataTable dt = new SuspendAccStatusData.InvestorDataTable();
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();
        try
        {
            ta.FillBySearchCriteria(dt,code, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static SuspendAccStatusData.InvestorRow FillByID(decimal investorID)
    {
        SuspendAccStatusData.InvestorDataTable dt = new SuspendAccStatusData.InvestorDataTable();
        SuspendAccStatusDataTableAdapters.InvestorTableAdapter ta = new SuspendAccStatusDataTableAdapters.InvestorTableAdapter();
        SuspendAccStatusData.InvestorRow dr = null;
        try
        {
            ta.FillByInvestorID(dt,investorID);
            if (dt.Count > 0)
            {
                dr = dt[0];
                //issuerNm = dr.BondIssuerName;
            }
            return dr;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void InsertTFMemberInfo(decimal investorID, string userNm, string accStatus, string suretyBond)
    {
        try
        {
           

            using (TransactionScope scope = new TransactionScope())
            {
                SuspendAccStatusData.TFMemberInfoDataTable dt = new SuspendAccStatusData.TFMemberInfoDataTable();
                SuspendAccStatusDataTableAdapters.TFMemberInfoTableAdapter ta = new SuspendAccStatusDataTableAdapters.TFMemberInfoTableAdapter();
                SuspendAccStatusData.TradefeedMemberInfoDataTable dtTF = new SuspendAccStatusData.TradefeedMemberInfoDataTable();
                SuspendAccStatusDataTableAdapters.TradefeedMemberInfoTableAdapter taTF = new SuspendAccStatusDataTableAdapters.TradefeedMemberInfoTableAdapter();
                string regCode = "";
                //Busdate
                ParameterData.ParameterRow drBusDate = Parameter.GetParameterByCodeAndApprovalStatus("BusinessDate", "A");
                DateTime busDate =drBusDate.DateValue;
                //Member Seq
                int memberSeq = Convert.ToInt32( taTF.MaxMemberSeq(drBusDate.DateValue));
               
                //Fixed Participant
                ParameterData.ParameterRow drFixParticipant = Parameter.GetParameterByCodeAndApprovalStatus("PKJParticipantCode", "A");
                string fixParticipant = drFixParticipant.StringValue;

                ta.Fill(dt, investorID);
                if (dt[0].IsRegionCodeNull())
                {
                    regCode = "";
                }
                else
                {
                    regCode = dt[0].RegionCode;
                }

               
                if (dt.Rows.Count > 0)
                {
                   
                    taTF.Insert(busDate, memberSeq + 1, "I", dt[0].RegistrationDate, fixParticipant, dt[0].Code, dt[0].AccountCode
                                , dt[0].CMType, dt[0].GroupProduct, regCode, dt[0].AccountType, dt[0].Name
                                , dt[0].Email, userNm, DateTime.Now, userNm, DateTime.Now, accStatus, suretyBond);
                    string logMessage = string.Format("Proposed Insertradefeed Member Info T, Busdate:{0} | " +
                                                 "MemberSeq:{1} | Code:{2} | AccountCode:{3}|Name:{4}",
                                                 busDate, memberSeq + 1, dt[0].Code, dt[0].AccountCode, dt[0].Name);

                    AuditTrail.AddAuditTrail("TradeFeedMemberInfo", AuditTrail.PROPOSE, logMessage, userNm, "Insert");
                }
                scope.Complete();
            }
                


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}