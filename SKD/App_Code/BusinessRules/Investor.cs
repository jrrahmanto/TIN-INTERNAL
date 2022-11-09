using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Investor
/// </summary>
public class Investor
{
    public static string GetNameInvestorByInvestorID(decimal investorId)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorData.InvestorRow dr = null;
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        string name = "";
        try
        {
            ta.FillByInvestorID(dt, investorId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                name = dr.Name;
            }

            return name;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static InvestorData.InvestorDataTable 
        GetInvestorByCodeNameApprovalStatus(string code, string name, string approvalStatus)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();

        try
        {
            ta.FillByCodeNameApprovalStatus(dt, code, name, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static InvestorData.InvestorDataTable FillBySearchCriteria(Nullable<decimal> cmid,
                                                Nullable<decimal> emid, string approvalStatus)
    {
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        try
        {
            ta.FillBySearchCriteria(dt, approvalStatus, cmid, emid);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static InvestorData.InvestorDataTable FillByInvestorID(decimal investorID)
    {
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        try
        {
            return ta.GetDataByInvestorID(investorID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static InvestorData.ViewLookupBankAccRow FillViewInvestorbyInvestorID(decimal investorID)
    {
        InvestorData.ViewLookupBankAccDataTable dt = new InvestorData.ViewLookupBankAccDataTable();
        InvestorDataTableAdapters.ViewLookupBankAccTableAdapter ta = new InvestorDataTableAdapters.ViewLookupBankAccTableAdapter();
        InvestorData.ViewLookupBankAccRow dr = null;

        try
        {
            ta.FillByInvestorID(dt, investorID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }
            return dr;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static InvestorData.InvestorRow FillInvestorbyInvestorId(decimal investorID)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        InvestorData.InvestorRow dr = null;
         
        try
        {
            ta.FillByInvestorID(dt, investorID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }
            return dr;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool FillAccStatusbyInvestorId(decimal investorID)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        InvestorData.InvestorRow dr = null;
        bool isValid = false;

        try
        {
            ta.FillByInvestorID(dt, investorID);

            if (dt.Count > 0)
            {
                dr = dt[0];
                if (dr.ApprovalStatus == "A" && dr.AccountStatus == "S")
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static InvestorData.ViewLookupBankAccDataTable GetAccountCodeByNameApprovalStatusAccType(string code, string cmName)
    {
        InvestorData.ViewLookupBankAccDataTable dt = new InvestorData.ViewLookupBankAccDataTable();
        InvestorDataTableAdapters.ViewLookupBankAccTableAdapter ta = new InvestorDataTableAdapters.ViewLookupBankAccTableAdapter();
        try
        {
            ta.FillByNameAndApprovalStatusAccType(dt,code,cmName);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProposedInsert(string code, decimal emid, string name, string inHouseFlag, string userName, 
                                         string groupProduct, string invStatus, string accType)
    {
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(code, emid, "P", name, inHouseFlag, userName, DateTime.Now,
                          userName, DateTime.Now, null, "I", null, groupProduct, invStatus, accType,null,null,null);
                string logMessage = string.Format("Proposed Insert, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | InHouseFlag:{3}",
                                                  code,emid,name,inHouseFlag);
                AuditTrail.AddAuditTrail("Investor", AuditTrail.PROPOSE, logMessage, userName, "Insert");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
    public static void ProposedUpdate(string code, decimal emid, string name,
                                      string inHouseFlag, string userName, decimal originalInvestorID,
                                      string groupProduct, string invStatus, string accType)
    {
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(code, emid, "P", name, inHouseFlag, userName, DateTime.Now,
                          userName, DateTime.Now, null, "U", originalInvestorID, groupProduct, invStatus, accType,null,null,null);
                string logMessage = string.Format("Proposed Update, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | InHouseFlag:{3}",
                                                  code, emid, name, inHouseFlag);
                AuditTrail.AddAuditTrail("Investor", AuditTrail.PROPOSE, logMessage, userName,"Update");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static void ProposedDelete(string code, decimal emid, string name,
                                      string inHouseFlag, string userName, decimal originalInvestorID,
                                      string groupProduct, string invStatus, string accType)
    {
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(code, emid, "P", name, inHouseFlag, userName, DateTime.Now,
                          userName, DateTime.Now, null, "D",originalInvestorID, groupProduct, invStatus, accType,null,null,null);
                string logMessage = string.Format("Proposed Delete, Code:{0} | " +
                                                  "EMID:{1} | Name:{2} | InHouseFlag:{3}",
                                                  code, emid, name, inHouseFlag);
                AuditTrail.AddAuditTrail("Investor", AuditTrail.PROPOSE, logMessage, userName, "Delete");
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Approve(decimal investorID, string approvalDesc, string action, string userName)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByInvestorID(dt, investorID);

                if (dt.Count > 0)
                {
                    if (action == "Insert")
                    {
                        ta.ApproveInsert("A", null, null, investorID);
                    }
                    else if (action == "Update")
                    {
                        ta.ApproveUpdate(dt[0].Code, dt[0].EMID, "A", dt[0].Name, dt[0].InHouseFlag,
                                         userName, DateTime.Now, approvalDesc, null, null, dt[0].OriginalInvestorID);
                        ta.DeleteInvestor(investorID);
                    }
                    else if (action == "Delete")
                    {
                        ta.DeleteInvestor(investorID);
                    }
                    string logMessage = string.Format("Approve " + action + ", Code:{0} | " +
                                                      "EMID:{1} | Name:{2} | InHouseFlag:{3}",
                                                      dt[0].Code, dt[0].EMID, dt[0].Name, dt[0].InHouseFlag);
                    AuditTrail.AddAuditTrail("Investor", AuditTrail.APPROVE, logMessage, userName, "Approve " + action);
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
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByInvestorID(dt, investorID);
                if (dt.Count > 0)
                {
                    ta.DeleteInvestor(investorID);
                    string logMessage = string.Format("Delete " + action + ", Code:{0} | " +
                                                      "EMID:{1} | Name:{2} | InHouseFlag:{3}",
                                                      dt[0].Code, dt[0].EMID, dt[0].Name, dt[0].InHouseFlag);
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

    public static InvestorData.InvestorDataTable GetCMExchangeInvestor(string approvalStatus, string cmCode, string investorCode)
    {
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();

        try
        {
            ta.FillByCMCodeInvestor(dt, investorCode, approvalStatus, cmCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
}
