using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for BondIssuer
/// </summary>
public class BondIssuer
{
    public BondIssuer()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static BondIssuerData.BondIssuerDataTable GetBondIssuer()
    {
        BondIssuerData.BondIssuerDataTable dt  = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();

        try
        {
            ta.Fill(dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static BondIssuerData.BondIssuerDataTable GetActiveBondIssuer()
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();

        try
        {
            ta.FillByApprovalStatus(dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static string GetBondIssuerById(decimal bondId)
    {

        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        BondIssuerData.BondIssuerRow dr;
        string issuerNm = "";

        try
        {
            ta.FillByID(dt, bondId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                issuerNm = dr.BondIssuerName;
            }

            return issuerNm;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static BondIssuerData.BondIssuerDataTable SelectBondByIssuerNm(string issuerNm)
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        try
        {
            ta.FillByLikeIssuerNm(dt, issuerNm);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Bond Issuer data");
        }
    }

    public static BondIssuerData.BondIssuerDataTable GetDataBondIssuerAndApprovalStatus(string issuerNm, string approvalStatus)
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        try
        {
            ta.FillByIssuerNmApprovalStatus(dt, issuerNm,approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Bond Issuer data");
        }
    }

    public static void ProposeBondIssuer(string issuerNm, string approvalDesc,
                                         string action, string userName, decimal OriginalID,string notes)
    {
        
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                

                ta.Insert(issuerNm, "P",approvalDesc,userName,DateTime.Now, userName,
                            DateTime.Now,action,OriginalID,notes);

                logMessage = string.Format("Proposed Value: IssuerName={0}|",
                                       issuerNm);
                AuditTrail.AddAuditTrail("BondIssuer", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

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
    public static BondIssuerData.BondIssuerRow SelectBondIssuerByID(decimal bondID)
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        BondIssuerData.BondIssuerRow dr = null;
        try
        {
            ta.FillByID(dt, bondID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bond issuer data");
        }
    }
    public static void ApproveBondIssuer(decimal bondId,string issuerNm, string userName, string approveDesc,string notes)
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();

        try
        {
            try
            {
                ta.FillByID(dt, bondId);

              
                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                     //update record
                    if (dt[0].ActionFlag == "I")
                    {

                        ta.ApproveProposedItem(issuerNm, "A", approveDesc,notes, userName, DateTime.Now, null, dt[0].BondIssuerID);
                        logMessage = string.Format("Approved Insert: IssuerName={0}",
                                                               dt[0].BondIssuerName);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {


                        ta.ApproveProposedItem(issuerNm, "A", approveDesc,notes, userName, DateTime.Now, null, dt[0].OriginalId);

                        //delete proposed record
                        ta.Delete(dt[0].BondIssuerID);

                        logMessage = string.Format("Approved Update: IssuerName={0}",
                                                               dt[0].BondIssuerName);
                    }

                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.Delete(dt[0].OriginalId);
                        ta.Delete(dt[0].BondIssuerID);

                        logMessage = string.Format("Approved Delete: IssuerName={0}",
                                                               dt[0].BondIssuerName);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("BondIssuer", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw ex;
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


    public static void RejectProposedBondIssuer(decimal bondId, string userName)
    {
        BondIssuerData.BondIssuerDataTable dt = new BondIssuerData.BondIssuerDataTable();
        BondIssuerDataTableAdapters.BondIssuerTableAdapter ta = new BondIssuerDataTableAdapters.BondIssuerTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByID(dt, bondId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject:  IssuerName={0}",
                                                               dt[0].BondIssuerName);

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.Delete(bondId);

                AuditTrail.AddAuditTrail("BondIssuer", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}