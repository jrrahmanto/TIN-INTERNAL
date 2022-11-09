using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Mutation Deposit DFS
/// </summary>
public class MutationDeposit
{


    #region "-- MutationDepositDFS --"

    public static MutationDepositDFS.ViewMutationDepositDataTable
        GetMutationDepositEntryDateApprovalCM(Nullable<DateTime> entryDate, Nullable<decimal> clearingMemberId,
        string approvalStatus)
    {
        MutationDepositDFS.ViewMutationDepositDataTable dt = new MutationDepositDFS.ViewMutationDepositDataTable();
        MutationDepositDFSTableAdapters.ViewMutationDepositTableAdapter ta = new MutationDepositDFSTableAdapters.ViewMutationDepositTableAdapter();
        Nullable<DateTime> receiveDate;

        if (entryDate == null)
        {
            receiveDate = null;
        }
        else
        {
            receiveDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", entryDate));
        }
        try
        {
            ta.FillByEntryDateCMIDApproval(dt, receiveDate, clearingMemberId, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static MutationDepositDFS.MutationDepositDFSRow GetMutationDepositbyMutationNo(int mutationNo)
    {
        
        MutationDepositDFS.MutationDepositDFSDataTable dt = new MutationDepositDFS.MutationDepositDFSDataTable();
        MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter ta = new MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter();
        MutationDepositDFS.MutationDepositDFSRow dr = null;
   
        try
        {
            ta.FillByMutationNo(dt, mutationNo);
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

    public static void InsertMutationDeposit(string approvalStatus, DateTime entryDate,
        decimal amount, string createdBy, DateTime createdDate, decimal bankAcctId,
        string lastUpdatedBy, DateTime lastUpdatedDate, string approvalDesc)
    {
        
        MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter ta = new MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                decimal mutationNo;
                mutationNo = (decimal)ta.InsertMutationDeposit(approvalStatus, entryDate,bankAcctId, amount,
                    createdBy, createdDate, lastUpdatedBy,
                   lastUpdatedDate, approvalDesc);

                string log = string.Format("mutationNo:{0}|EntryDate:{1}|BankAccount:{2}|" +
                    "Amount:{3}|",
                    mutationNo, entryDate, bankAcctId, amount);

                AuditTrail.AddAuditTrail("MutationDepositDFS", "Insert", log, createdBy, "Insert");

                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateApprovalStatusMutationDeposit(string approvalStatus,
        string approvalDesc, string lastUpdatedBy, DateTime lastUpdatedDate,
        int mutationNo)
    {
        MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter ta = new MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.UpdateStatusApprovalMutationDeposit(approvalStatus, lastUpdatedBy,
                    lastUpdatedDate, approvalDesc, mutationNo);

                string log = string.Format("MutationNo:{0}", mutationNo);
                if (approvalStatus == "A")
                {
                    //todo :
                    //after approve run sp to insert into header and journal line
                    //BankDataTableAdapters.QueriesTableAdapter taQuery = new BankDataTableAdapters.QueriesTableAdapter();


                    AuditTrail.AddAuditTrail("MutationDepositDFS", AuditTrail.APPROVE, log, lastUpdatedBy, "Approve Update");

                }
                else if (approvalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("MutationDepositDFS", AuditTrail.REJECT, log, lastUpdatedBy, "Reject Update");
                }

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static BankData.BankTransactionRow GetBankTransactionByTransactionNo(int transactionNo)
    {
        BankData.BankTransactionDataTable dt = new BankData.BankTransactionDataTable();
        BankData.BankTransactionRow dr = null;
        BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            ta.FillByTransactionNo(dt, transactionNo);
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

    public static void ApproveRejectBankTransactionFromView(List<string> approvalList,DateTime lastUpdatedDate,
                                                            string userName, string status)
    {
         BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            foreach (var item in approvalList)
	        {
                string[] bankTrans = item.Split("|".ToCharArray());
                UpdateApprovalStatusBankTransaction(status, bankTrans[1], userName, lastUpdatedDate, int.Parse(bankTrans[0]));
	        }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static BankData.VTransBankDataTable
        GetBankTransactionTimeAppStatusCM(Nullable<DateTime> receiveTime, Nullable<decimal> clearingMemberId,
        string approvalStatus)
    {
        BankData.VTransBankDataTable dt = new BankData.VTransBankDataTable();
        BankDataTableAdapters.VTransBankTableAdapter ta = new BankDataTableAdapters.VTransBankTableAdapter();

        try
        {
            ta.FillByCriteria(dt,receiveTime,  approvalStatus, clearingMemberId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static BankData.ViewBankTransactionDataTable
        GetBankTransactionByTransTimeApprovalStatusClearingMember(
        Nullable<DateTime> transactionTime, decimal? clearingMemberId,
        string approvalStatus)
    {
        BankData.ViewBankTransactionDataTable dt = new BankData.ViewBankTransactionDataTable();
        BankDataTableAdapters.ViewBankTransactionTableAdapter ta = new BankDataTableAdapters.ViewBankTransactionTableAdapter();

        try
        {
            ta.FillTransTimeApprovalStatusClearingMember(dt, transactionTime, approvalStatus, clearingMemberId);

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    
    public static BankData.BankTransactionDataTable 
        GetBankTransactionByTransactionTimeAndApprovalStatus(Nullable<DateTime> transactionTime, 
        string approvalStatus)
    {
        BankData.BankTransactionDataTable dt = new BankData.BankTransactionDataTable();
        BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            ta.FillByTransactionTimeAndApprovalStatus(dt, transactionTime, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateApprovalStatusBankTransaction(string approvalStatus, 
        string approvalDesc, string lastUpdatedBy, DateTime lastUpdatedDate, 
        int transactionNo)
    {
        BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {                
                ta.UpdateApprovalStatusBankTransaction(approvalStatus, lastUpdatedBy,
                    lastUpdatedDate, approvalDesc, transactionNo);

                string log = string.Format("TransactionNo:{0}", transactionNo);                
                if (approvalStatus == "A")
                {
                    //todo :
                    //after approve run sp to insert into header and journal line
                    BankDataTableAdapters.QueriesTableAdapter taQuery = new BankDataTableAdapters.QueriesTableAdapter();
                    

                    AuditTrail.AddAuditTrail("BankTransaction", AuditTrail.APPROVE, log, lastUpdatedBy,"Approve Update");
                    
                }
                else if (approvalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("BankTransaction", AuditTrail.REJECT, log, lastUpdatedBy,"Reject Update");
                }

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static int GetMaxMutationNo()
    {
        
        MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter dt = new MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter();
        int maxNo;

        maxNo = (int)dt.GetMaxMutationNo();

        return maxNo;
    }

    public static void ImportMutationDepositDFS(MutationDepositDFS.MutationDepositDFSDataTable dt)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                
                MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter ta = new MutationDepositDFSTableAdapters.MutationDepositDFSTableAdapter();
                ta.DeleteByEntryDate(dt[0].EntryDate);
                ta.Update(dt);

                string logMessage = string.Format("Import Mutation Deposit DFS", "");
                AuditTrail.AddAuditTrail("MutationDepositDFS", "Insert", logMessage, dt[0].CreatedBy, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    
    
}
