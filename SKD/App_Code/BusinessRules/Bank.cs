using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Bank
/// </summary>
public class Bank
{

    # region "-- Bank --"
    public static BankData.BankDataTable GetBank()
    {
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();

        try
        {
            ta.FillByBankOnly(dt);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static BankData.BankDataTable GetBankDataByCodeAndApprovalStatus(string code, string approvalStatus)
    {
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();

        try
        {
            ta.FillByBankCodeAndStatus(dt, code, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetBankDataByCodeParameter()
    {
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankRow dr = null;
        decimal bankID = 0;

        try
        {
            ta.FillByParameterCode(dt);

            if (dt.Count > 0)
            {
                dr = dt[0];

                bankID = dr.BankID;
            }

            return bankID;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetBankIDByBICode(string biCode)
    {
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankData.BankRow dr = null;
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        decimal bankID = 0;

        try
        {
            ta.FillByBICode(dt, biCode);
            if (dt.Count > 0)
            {
                dr = dt[0];

                bankID = dr.BankID;
            }

            return bankID;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetBankCodeByBankID(int bankID)
    {
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankData.BankRow dr = null;
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        string bankCode = "";

        try
        {
            ta.FillByBankID(dt, bankID);
            if (dt.Count > 0)
            {
                dr = dt[0];

                bankCode = dr.BICode;
            }

            return bankCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }        
    #endregion

    #region "-- BankTransaction --"
    public static void ProcessBankTransaction(DateTime transDateExc,int transSeqExc, 
        int bankIdExc, string approvalStatusExc, string approvalStatusInputExc, 
        string approvalDescInputExc, 
        string lastUpdatedByExc, DateTime lastUpdatedDateExc,
        string approvalStatus, DateTime transactionTime,
        DateTime receiveTime, string accountType, Nullable<decimal> sourceAccID, Nullable<decimal> destAccID, Nullable<decimal> sourceCMID,
        Nullable<decimal> destCMID, Nullable<decimal> sourceInvID, Nullable<decimal> destInvID, decimal sourceBank,
        string sourceAccount, decimal amount, string mutationType, string transactionType,
        string news, string transactionDescription, string createdBy, DateTime createdDate,
        string lastUpdatedBy, DateTime lastUpdatedDate, string approvalDesc, string referenceType)
    {
        BankDataTableAdapters.BankTransactionExceptionTableAdapter dtExc = new BankDataTableAdapters.BankTransactionExceptionTableAdapter();
        BankDataTableAdapters.BankTransactionExceptionTableAdapter taExc = new BankDataTableAdapters.BankTransactionExceptionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Update transaction exception
                taExc.UpdateTransactionException(approvalStatusInputExc, lastUpdatedByExc, lastUpdatedDateExc, 
                    approvalDescInputExc, transDateExc, transSeqExc, bankIdExc, approvalStatusExc);

                //Insert to Bank Transaction
                InsertBankTransaction(approvalStatus, transactionTime, receiveTime,
                    accountType, sourceAccID, destAccID, sourceCMID, destCMID, sourceInvID, destInvID,
                    sourceBank, sourceAccount, amount, mutationType, transactionType,
                    news, transactionDescription, createdBy, createdDate, lastUpdatedBy, lastUpdatedDate, approvalDesc, referenceType);

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void InsertBankTrans(string approvalStatus, DateTime transactionTime,
        DateTime receiveTime, decimal amount, string mutationType, string transactionType,
        string bankRef, string transactionDescription, string createdBy, DateTime createdDate,
        string lastUpdatedBy, DateTime lastUpdatedDate, string approvalDesc, decimal bankAccount, string referenceType)
    {
        BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                decimal transactionNo;
                transactionNo = (decimal)ta.InsertBankTrans(approvalStatus, transactionTime, receiveTime, amount,
                    mutationType, transactionType, bankRef,
                   transactionDescription, createdBy, createdDate, lastUpdatedBy,
                   lastUpdatedDate, approvalDesc,bankAccount, referenceType);

                string log = string.Format("TransactionNo:{0}|TransactionTime:{1}|ReceiveTime:{2}|" +
                    "Amount:{3}|" +
                    "MutationType:{4}|TransactionType:{5}|BankReference:{6}|Transactio3nDescritpion:{7}",
                    transactionNo, transactionTime, receiveTime, amount, mutationType, transactionType, 
                    bankRef, transactionDescription);

                AuditTrail.AddAuditTrail("BankTransaction", "Insert", log, createdBy, "Insert");

                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static void InsertBankTransaction(string approvalStatus, DateTime transactionTime, 
        DateTime receiveTime, string accountType, Nullable<decimal> sourceAccID, Nullable<decimal> destAccID, Nullable<decimal> sourceCMID, 
        Nullable<decimal> destCMID, Nullable<decimal> sourceInvID, Nullable<decimal> destInvID,Nullable<decimal> sourceBank, 
        string sourceAccount, decimal amount, string mutationType, string transactionType,
        string news, string transactionDescription, string createdBy, DateTime createdDate, 
        string lastUpdatedBy, DateTime lastUpdatedDate, string approvalDesc, string referenceType)
    {
        BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                decimal transactionNo;
                transactionNo = (decimal)ta.InsertBankTransaction(approvalStatus, transactionTime, receiveTime, accountType, sourceAccID,
                   destAccID, sourceCMID, destCMID, sourceInvID, destInvID, sourceBank,
                   sourceAccount, amount, mutationType, transactionType, news,
                   transactionDescription, createdBy, createdDate, lastUpdatedBy,
                   lastUpdatedDate, approvalDesc, referenceType);

                string log = string.Format("TransactionNo:{0}|TransactionTime:{1}|ReceiveTime:{2}|" +
                    "AccountType:{3}|SourceAcctID:{4}|DesctAcctID:{5}|SourceCMID:{6}|DestCMID:{7}|" + 
                    "SourceInvID:{8}|DestInvID:{9}|SourceBank:{10}|SourceAccount:{11}|Amount:{12}|" + 
                    "MutationType:{13}|TransactionType:{14}|News:{15}|TransactionDescritpion:{16}", 
                    transactionNo, transactionTime, receiveTime,accountType, sourceAccID, destAccID, sourceCMID, destCMID,
                    sourceInvID, destInvID, sourceBank,sourceAccount, amount, mutationType, transactionType, news, transactionDescription);

                AuditTrail.AddAuditTrail("BankTransaction", "Insert", log, createdBy, "Insert");

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
        Nullable<DateTime> receiveDate;

        if (receiveTime == null)
        {
            receiveDate = null;
        }
        else
        {
            receiveDate = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", receiveTime));
        }
         
        try
        {
            ta.FillByCriteria(dt, receiveDate,  approvalStatus, clearingMemberId);

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

    public static int GetMaxTransactionNo()
    {
        BankDataTableAdapters.BankTransactionTableAdapter dt = new BankDataTableAdapters.BankTransactionTableAdapter();
        int maxNo;

        maxNo = (int)dt.GetMaxTransactionNo();

        return maxNo;
    }

    public static void ImportBankTransaction(BankData.BankTransactionDataTable dt)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                BankDataTableAdapters.BankTransactionTableAdapter ta = new BankDataTableAdapters.BankTransactionTableAdapter();
                ta.DeleteByBusinessDate(dt[0].ReceiveTime);
                ta.Update(dt);

                string logMessage = string.Format("Import Bank Transaction", "");
                AuditTrail.AddAuditTrail("BankTransaction", "Insert", logMessage, dt[0].CreatedBy, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    # region "-- RawBankTransaction --"
    public static BankData.RawBankTransactionRow 
        GetRawBankTransactionByTransDateSeqBankID(DateTime transactionDate, 
        int transactionSeq, int bankID)
    {
        BankData.RawBankTransactionDataTable dt = new BankData.RawBankTransactionDataTable();
        BankDataTableAdapters.RawBankTransactionTableAdapter ta = new BankDataTableAdapters.RawBankTransactionTableAdapter();
        BankData.RawBankTransactionRow dr = null;

        try
        {
            ta.FillByTransDateSeqBankID(dt, transactionDate, transactionSeq, bankID);
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


    #endregion

    # region "-- BankTransactionException --"
    public static BankData.BankTransactionExceptionDataTable
        GetBankTransactionExceptionByTransDateBankIDApprovalStatus(Nullable<DateTime> transDate,
        Nullable<int> bankId, string approvalStatus)
    {
        BankData.BankTransactionExceptionDataTable dt = new BankData.BankTransactionExceptionDataTable();
        BankDataTableAdapters.BankTransactionExceptionTableAdapter ta = new BankDataTableAdapters.BankTransactionExceptionTableAdapter();

        try
        {
            ta.FillByTransDateBankIDApprovalStatus(dt, transDate, bankId, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }        
    }
    #endregion
    
}
