using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
/// <summary>
/// Summary description for BankAccount
/// </summary>
public class BankAccount
{
    //get all data from bank account by code and status
    public static BankData.BankAccountDataTable SelectBankAccountByAccountNo(string accountNo)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {
            ta.FillByAccountNo(dt, accountNo);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account data");
        }

    }


    public static string GetAccountNoBankAccountByBankAccountID(decimal bankAccountId)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        string accountNo = "";
        string accountType = "";

        try
        {
            ta.FillByBankAccountID(dt, bankAccountId);

            if (dt.Count > 0)
            {
                dr = dt[0];

                if (dr.AccountType == "RD")
                {
                    accountType = "Deposit";
                }
                else if (dr.AccountType == "RS")
                {
                    accountType = "Settlement";
                }
                accountNo = string.Format("{0} {1} {2} {3}", dr.BankCode + "-", accountType + "-", (dr.IsCMIDNull() ? "" : ClearingMember.GetClearingMemberNameByClearingMemberID(dr.CMID)) +"-", dr.AccountNo);                 
            }

            return accountNo;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetBankAccountIDByInvestorID(decimal investorID)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        decimal bankAccID = 0;

        try
        {
            ta.FillByInvestorID(dt,investorID);

            if (dt.Count > 0)
            {
                dr = dt[0];
                bankAccID = dr.BankAccountID;
            }

            return bankAccID;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetBankAccountIDByAccNo(string AccNo)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        decimal bankAccID = 0;

        try
        {
            ta.FillByAccNo(dt, AccNo);

            if (dt.Count > 0)
            {
                dr = dt[0];
                bankAccID = dr.BankAccountID;
            }

            return bankAccID;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetCMIDByAccNo(string AccNo)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        decimal bankAccID = 0;

        try
        {
            ta.FillByAccNo(dt, AccNo);

            if (dt.Count > 0)
            {
                dr = dt[0];
                bankAccID = dr.CMID;
            }

            return bankAccID;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetAccountType(decimal bankAccountID)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        return ta.GetAccountType(bankAccountID);
    }

    public static bool ValidateRTPAccount(decimal InvestorID, decimal CMID, string RTPType)
    {
        bool validAccount = false;
        InvestorDataTableAdapters.InvestorTableAdapter ta = new InvestorDataTableAdapters.InvestorTableAdapter();
        InvestorData.InvestorDataTable dt = new InvestorData.InvestorDataTable();

        // Validate InvestorID
        if (InvestorID == 0)
            throw new ApplicationException("InvestorID has not been set");

        try
        {
            // Get investor & CM data
            ta.FillCMByInvestorID(dt, InvestorID);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to fill Investor from database due to " + ex.Message);
        }

        // Check if investor and ClearingMember exist and related
        if (dt.Rows.Count == 0)
            throw new ApplicationException("Can not locate investor");

        // Validate relationship between investor & CM
        if (Convert.ToDecimal(dt[0].CMID) != CMID)
            throw new ApplicationException("Investor is not registered to specified Clearing Member");

        // RTPU account should be linked only to InHouse Account
        if ((RTPType == "RTPU") && (dt[0].InHouseFlag != "Y"))
            throw new ApplicationException("RTPU Account should be registered to InHouse Account");

        // RTPS account should be linked only to non-InHouse Account
        if ((RTPType == "RTPS") && (dt[0].InHouseFlag != "N"))
            throw new ApplicationException("RTPS Account should be registered to non InHouse Account");

        validAccount = true;

        return validAccount;
    }

    public static BankData.BankAccountDataTable SelectBankAccountbyAccountNoAndBankID(decimal bankAccountID, string bankAccountNo, DateTime businessDate)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {
            ta.FillByAccountNoBankID(dt, bankAccountNo, bankAccountID, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }

    public static BankData.BankAccountDataTable SelectBankAccountbyBankIDAndEffectiveEndDate(string bankAccountNo, DateTime businessDate)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {
            ta.FillByBankIdEndDate(dt, bankAccountNo, businessDate);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }
    public static BankData.BankAccountRow GetBankAccountByBankAccId(decimal bankAccId)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();

        try
        {
            ta.FillByBankAccountID(dt, bankAccId);
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

    public static BankData.BankAccountRow GetBankAccountIDByAccountType(string accountType)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();

        try
        {
            ta.FillByAccountType(dt, accountType);
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

    public static BankData.BankAccountDataTable GetBankAccountByAccountNoAndApprovalStatus(string accountNo, string approvalStatus)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();

        try
        {
            ta.FillByAccountNoAndApprovalStatus(dt, accountNo, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool SelectAcctType(string accNo)
    {
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        bool isValid = false;
        try
        {
            ta.FillByValidAcc(dt,accNo);
            if (dt.Count > 0)
            {
                isValid = true;
            }
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load clearing member bank account (FillBankBySearchCriteria)", ex);
        }
        return isValid;
    }

    public static BankData.BankAccountSearchRow SelectCMByBankAccountID(decimal bankAccId)
    {
        BankDataTableAdapters.BankAccountSearchTableAdapter ta = new BankDataTableAdapters.BankAccountSearchTableAdapter();
        BankData.BankAccountSearchDataTable dt = new BankData.BankAccountSearchDataTable();
        BankData.BankAccountSearchRow dr = null;

        try
        {
            ta.FillByBankAccID(dt, bankAccId);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }
            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load clearing member bank account (FillBankBySearchCriteria)", ex);
        }
    }
    //get all data bank pdm from dataset to datagrid
    public static BankData.BankAccountRow SelectBankAccountByBankAccountID(decimal bankAccountID)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankData.BankAccountRow dr = null;
        try
        {
            ta.FillByBankAccountID(dt, bankAccountID);

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

    //get all data from bank pdm by code and status
    //public static BankData.BankAccountDataTable SelectBankAccountByAccountNoAndStatus(string approvalStatus, string accountType, 
    //                                                    Nullable<decimal> bankId, Nullable<decimal> cmId, Nullable<decimal> 
    //                                                    investorId, Nullable<decimal> ccyId)
    public static BankData.BankAccountDataTable SelectBankAccountByAccountNoAndStatus(string approvalStatus, string accountType,
                                                    Nullable<decimal> bankId, Nullable<decimal> cmId, Nullable<decimal> ccyId, string accStatus)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {

            //ta.FillByBankAccountCriteria(dt, approvalStatus, accountType, bankId, cmId, investorId, ccyId);
            ta.FillByBankAccCriteria(dt, approvalStatus, accountType, bankId, cmId, ccyId,accStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account data");
        }

    }

    //export data Bank Account to Excel
    public static BankData.BankAccountDataTable SelectBankAccountForExport(string approvalStatus, string accountType,
                                                    Nullable<decimal> bankId, Nullable<decimal> cmId, Nullable<decimal> ccyId, string accStatus)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {

            //ta.FillByBankAccountCriteria(dt, approvalStatus, accountType, bankId, cmId, investorId, ccyId);
            ta.FillByExportBankAccount(dt, approvalStatus, accountType, bankId, cmId, ccyId, accStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account data");
        }

    }

    //get all data from bank account by effective start date
    public static BankData.BankAccountDataTable SelectBankAccountByEffectiveStartDate(DateTime effectiveStartDate)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {
            ta.FillByEffectiveStartDate(dt, effectiveStartDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account data");
        }

    }

    
    public static BankData.BankAccountSearchDataTable SelectBankAccountBySearchCriteria(string CMName, string AccountType, decimal BankID, string AccountNo)
    {
        BankDataTableAdapters.BankAccountSearchTableAdapter ta = new BankDataTableAdapters.BankAccountSearchTableAdapter();
        BankData.BankAccountSearchDataTable dt = new BankData.BankAccountSearchDataTable();

        try
        {
            ta.FillBankBySearchCriteria(dt, CMName, AccountType, BankID, AccountNo);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account (FillBankBySearchCriteria)", ex);
        }
    }

    public static void ProposeBankAccount(string accountNo, decimal bankID, string approvalDesc,
                                          string accountType, Nullable<decimal> investorID,
                                          DateTime startDate, Nullable<DateTime> endDate,
                                          Nullable<decimal> cmID, Nullable<decimal> ccyID,
                                          string action, string userName, decimal OriginalID, string accStatus)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();

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
                

                if (accountType == "RD" || accountType == "RS")
                {
                    //if (ValidateRTPAccount(Convert.ToDecimal(investorID), Convert.ToDecimal(cmID), accountType))
                    //{

                    ta.Insert(accountNo, bankID, accountType, userName, DateTime.Now, userName,
                            DateTime.Now, "P", startDate, endDate, approvalDesc, investorID, cmID, ccyID, action, OriginalID,accStatus);

                    logMessage = string.Format("Proposed Value: AccountNo={0}|BankCode={1}|AccountType={2}|EffectiveStartDate={3}|EffectiveEndDate={4}|Clearing Member={5}|Currency={6}",
                                           accountNo.ToString(), Convert.ToDecimal(bankID),
                                           accountType.ToString(), startDate.ToString("dd-MM-yyyy"),
                                           Convert.ToDateTime(endDate).Date,
                                           Convert.ToDecimal(cmID), Convert.ToDecimal(ccyID));
                    AuditTrail.AddAuditTrail("BankAccount", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

                    //}
                }
               
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


    public static void ApproveBankAccountID(decimal bankAccountID, string userName, string approveDesc)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

        try
        {
            try
            {
                ta.FillByBankAccountID(dt, bankAccountID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevStartDate(dt[0].EffectiveStartDate,
                                Convert.ToDecimal(dt[0].BankID), dt[0].AccountNo,dt[0].OriginalID));
            

                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(Convert.ToDecimal(dt[0].BankID),
                   dt[0].AccountNo, dt[0].EffectiveStartDate, dt[0].OriginalID));


                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    Nullable<decimal> invId;

                    if (dt[0].IsInvestorIDNull())
                    {
                        invId = null;
                    }
                    else
                    {
                        invId = dt[0].InvestorID;
                    }
                   
                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                       
                        if (dt[0].AccountType == "RD" || dt[0].AccountType == "RS")
                        {
                            if (dt[0].IsEffectiveEndDateNull())
                            {
                                ta.ApprovedProposedItem(dt[0].AccountNo, dt[0].BankID, dt[0].AccountType,
                                                    userName, DateTime.Now, "A", dt[0].EffectiveStartDate,
                                                    null, approveDesc,
                                                   invId, dt[0].CMID, dt[0].CurrencyID, null, dt[0].BankAccountID);
                            }
                            else
                            {
                                ta.ApprovedProposedItem(dt[0].AccountNo, dt[0].BankID, dt[0].AccountType,
                                                    userName, DateTime.Now, "A", dt[0].EffectiveStartDate,
                                                    dt[0].EffectiveEndDate, approveDesc,
                                                   invId, dt[0].CMID, dt[0].CurrencyID, null, dt[0].BankAccountID);
                            }

                        }
                        
                        logMessage = string.Format("Approved Insert: AccountNo={0}|BankCode={1}|AccountType={2}|EffectiveStartDate={3}|EffectiveEndDate={4}|Currency={5}|Clearing Member={6}|Investor={7}",
                                                               dt[0].AccountNo,
                                                               dt[0].BankCode,
                                                               dt[0].AccountType,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                               dt[0].CurrencyCode,
                                                               dt[0].IsCMIDNull() ? "" : dt[0].CMCode,
                                                               dt[0].IsInvestorIDNull() ? "" : dt[0].InvestorCode);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {

                       

                        if (dt[0].AccountType == "RD" || dt[0].AccountType == "RS")
                        {
                            if (dt[0].IsEffectiveEndDateNull())
                            {
                                ta.ApprovedUpdateProposedItem(dt[0].AccountNo, dt[0].BankID, dt[0].AccountType,
                                                         userName, DateTime.Now, "A", dt[0].EffectiveStartDate, null,
                                                         approveDesc, invId, dt[0].CMID, dt[0].CurrencyID, null, null, dt[0].AccountStatus, dt[0].OriginalID);
                            }
                            else
                            {
                                ta.ApprovedUpdateProposedItem(dt[0].AccountNo, dt[0].BankID, dt[0].AccountType,
                                                         userName, DateTime.Now, "A", dt[0].EffectiveStartDate, dt[0].EffectiveEndDate,
                                                         approveDesc, invId, dt[0].CMID, dt[0].CurrencyID, null, null, dt[0].AccountStatus, dt[0].OriginalID);
                            }
                        }
                       

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].BankAccountID);

                        logMessage = string.Format("Approved Update: AccountNo={0}|BankCode={1}|AccountType={2}|EffectiveStartDate={3}|EffectiveEndDate={4}|Currency={5}|Clearing Member={6}|Investor={7}",
                                                               dt[0].AccountNo,
                                                               dt[0].BankCode,
                                                               dt[0].AccountType,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                               dt[0].CurrencyCode,
                                                               dt[0].IsCMIDNull() ? "" : dt[0].CMCode,
                                                               dt[0].IsInvestorIDNull() ? "" : dt[0].InvestorCode);
                    }

                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].BankAccountID);

                        logMessage = string.Format("Approved Delete: AccountNo={0}|BankCode={1}|AccountType={2}|EffectiveStartDate={3}|EffectiveEndDate={4}|Currency={5}|Clearing Member={6}|Investor={7}",
                                                               dt[0].AccountNo,
                                                               dt[0].BankCode,
                                                               dt[0].AccountType,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                               dt[0].CurrencyCode,
                                                               dt[0].IsCMIDNull() ? "" : dt[0].CMCode,
                                                               dt[0].IsInvestorIDNull() ? "" : dt[0].InvestorCode);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("BankAccount", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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


    public static void RejectProposedBankAccountID(decimal bankAccountID, string userName)
    {
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByBankAccountID(dt, bankAccountID);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject:  AccountNo={0}|BankCode={1}|AccountType={2}|EffectiveStartDate={3}|EffectiveEndDate={4}|Currency={5}|Clearing Member={6}|Investor={7}",
                                                               dt[0].AccountNo,
                                                               dt[0].BankCode,
                                                               dt[0].AccountType,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                               dt[0].CurrencyCode,
                                                               dt[0].IsCMIDNull() ? "" : dt[0].CMCode,
                                                               dt[0].IsInvestorIDNull() ? "" : dt[0].InvestorCode);
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.DeleteRejectItem(bankAccountID);

                AuditTrail.AddAuditTrail("BankAccount", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static decimal GetCurrencyIdOfBankAccount(decimal bankAccountId)
    {
        decimal id = 0;

        BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();
        BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();

        try
        {
            ta.FillByBankAccountID(dt, bankAccountId);

            if (dt != null && dt.Rows.Count>0)
            {
                // Take first record and it should be only one 
                BankData.BankAccountRow dr = (BankData.BankAccountRow)dt.Rows[0];
                id = dr.CurrencyID;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(String.Format("Cannot get Bank Account data: {0}", ex.Message));
        }

        return id;
    }

    //public static BankData.BankAccountDataTable SelectBankAccountbyAccountNoAndBankID(decimal bankAccountID, string bankAccountNo, DateTime businessDate)
    //{
    //    BankDataTableAdapters.BankAccountTableAdapter ta = new BankDataTableAdapters.BankAccountTableAdapter();
    //    BankData.BankAccountDataTable dt = new BankData.BankAccountDataTable();

    //    try
    //    {
    //        ta.FillByAccountNoBankID(dt, bankAccountNo, bankAccountID, businessDate);

    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException(ex.Message, ex);
    //    }

    //}


}
