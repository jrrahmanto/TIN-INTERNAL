using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;


/// <summary>
/// Summary description for Posting
/// </summary>
public class Posting
{
    #region "-- PostingCode --"

    public static PostingData.AccountDataTable GetPostingCodeByAccountCode(string accountCode)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();

        try
        {
            ta.FillByAccountCode(dt, accountCode);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetPostingDescriptionByAccountId(decimal accountId)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingData.AccountRow dr = null;
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();
        string description = "";
        try
        {
            ta.FillByAccountID(dt, accountId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                if (!dr.IsDescriptionNull())
                {
                    description = dr.Description;
                }
            }

            return description;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }


    public static PostingData.AccountDataTable GetPostingCodeByAccountCodeAndApprovalStatus(string accountCode, string approvalStatus)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();

        try
        {
            ta.FillByAccountCodeAndApprovalStatus(dt, accountCode, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static PostingData.AccountDataTable GetPostingCodeByAccountCodeAndLedgerType(string accountCode, string ledgerType)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();

        try
        {
            ta.FillByAccountCodeAndLedgerType(dt, accountCode, ledgerType);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    public static string GetPostingAccountCodeByAccountID(decimal accountId)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingData.AccountRow dr = null;
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();
        string accountCode = "";

        try
        {
            ta.FillByAccountID(dt, accountId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                accountCode = dr.AccountCode;
            }

            return accountCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    //get all data from dataset to datagrid
    public static PostingData.AccountRow SelectPostingByAccountID(decimal AccountID)
    {
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingData.AccountRow dr = null;
        try
        {
            ta.FillByAccountID(dt, AccountID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load posting code data");
        }
    }

    //Proposed Save Transaction
    public static void ProposePostingCode(string accountCode, string ledgerType, 
                                      string accountType, string balanceType, Int16 seqDisplay,
                                      string displayInBS, string description, string approvalDescription,
                                      string action, string userName, decimal originalID)
    {
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(accountCode, ledgerType, "P", accountType, balanceType, Convert.ToInt16(seqDisplay), displayInBS, description, userName,
                      DateTime.Now, userName, DateTime.Now, approvalDescription, action, originalID);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: accountCode={0}|ledgerType={1}|accountType={2}|balanceType={3}|seqDisplay={4}|displayInBS={5}|description={6}|approvalDescription={7}",
                                            accountCode, ledgerType, accountType, balanceType, 
                                            seqDisplay, displayInBS, description, approvalDescription);
                AuditTrail.AddAuditTrail("PostingCode", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);

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


    public static void ApprovePostingCode(decimal accountID, string userName, string approvalDesc)
    {
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();

        try
        {
          try
            {
                ta.FillByAccountID(dt, accountID);

               using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";
                    
                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (dt[0].DisplayInBS == "1")
                        {
                            ta.ApprovedProposedItem(dt[0].AccountCode, dt[0].LedgerType, "A", dt[0].AccountType,
                                                    dt[0].BalanceType, dt[0].SeqDisplayDFS, "Y", dt[0].Description, userName, DateTime.Now, approvalDesc, null, null, dt[0].AccountID);
                        }
                        else
                        {
                            ta.ApprovedProposedItem(dt[0].AccountCode, dt[0].LedgerType, "A", dt[0].AccountType,
                                                   dt[0].BalanceType, dt[0].SeqDisplayDFS, "N", dt[0].Description,
                                                   userName, DateTime.Now, approvalDesc, null, null, dt[0].AccountID);
              
                        }

                        logMessage = string.Format("Approved Insert: accountCode={0}|ledgerType={1}|accountType={2}|balanceType={3}|seqDisplay={4}|displayInBS={5}|description={6}|approvalDescription={7}",
                                                              dt[0].AccountCode,
                                                              dt[0].LedgerType,
                                                              dt[0].AccountType,
                                                              dt[0].BalanceType,
                                                              dt[0].SeqDisplayDFS,
                                                              dt[0].DisplayInBS,
                                                              dt[0].Description,
                                                              approvalDesc);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (dt[0].DisplayInBS == "1")
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].AccountCode, dt[0].LedgerType, "A", dt[0].AccountType,
                                                    dt[0].BalanceType, dt[0].SeqDisplayDFS, "Y", dt[0].Description,
                                                    userName, DateTime.Now, approvalDesc, null, dt[0].OriginalID);
                        }
                        else
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].AccountCode, dt[0].LedgerType, "A", dt[0].AccountType,
                                             dt[0].BalanceType, dt[0].SeqDisplayDFS, "N", dt[0].Description,
                                             userName, DateTime.Now, approvalDesc, null, dt[0].OriginalID);
                        }
               
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].AccountID);
                        logMessage = string.Format("Approved Update: accountCode={0}|ledgerType={1}|accountType={2}|balanceType={3}|seqDisplay={4}|displayInBS={5}|description={6}|approvalDescription={7}",
                                                              dt[0].AccountCode,
                                                              dt[0].LedgerType,
                                                              dt[0].AccountType,
                                                              dt[0].BalanceType,
                                                              dt[0].SeqDisplayDFS,
                                                              dt[0].DisplayInBS,
                                                              dt[0].Description,
                                                              approvalDesc);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].AccountID);

                        logMessage = string.Format("Approved Delete: accountCode={0}|ledgerType={1}|accountType={2}|balanceType={3}|seqDisplay={4}|displayInBS={5}|description={6}|approvalDescription={7}",
                                                              dt[0].AccountCode,
                                                              dt[0].LedgerType,
                                                              dt[0].AccountType,
                                                              dt[0].BalanceType,
                                                              dt[0].SeqDisplayDFS,
                                                              dt[0].DisplayInBS,
                                                              dt[0].Description,
                                                              approvalDesc);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("PostingCode", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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

    public static void RejectProposedPostingCode(decimal accountID, string userName, string approvalDesc)
    {
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByAccountID(dt, accountID);

                if (dt.Count > 0)
                {
                    logMessage = string.Format("Approved Insert: accountCode={0}|ledgerType={1}|accountType={2}|balanceType={3}|seqDisplay={4}|displayInBS={5}|description={6}|approvalDescription={7}",
                                                              dt[0].AccountCode,
                                                              dt[0].LedgerType,
                                                              dt[0].AccountType,
                                                              dt[0].BalanceType,
                                                              dt[0].SeqDisplayDFS,
                                                              dt[0].DisplayInBS,
                                                              dt[0].Description,
                                                              approvalDesc);
                }
                ta.DeleteRejectItem(accountID);
                string ActionFlagDesc = "";
                switch (dt[0].ActionFlag)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                AuditTrail.AddAuditTrail("PostingCode", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    #endregion

    #region "-- Posting Group --"

    public static PostingData.PostingGroupDataTable GetPostingGroupByGroupCode(string groupCode)
    {
        PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
        PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();

        try
        {
            ta.FillByPostingGroupCode(dt, groupCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static PostingData.PostingGroupDataTable GetPostingGroupByGroupCodeAndApprovalStatus(string groupCode, string approvalStatus)
    {
        PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
        PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();

        try
        {
            ta.FillByGroupCodeAndStatus(dt, groupCode, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static PostingData.PostingGroupRow SelectPostingGroupByPostingGroupID(decimal postingGroupID)
    {
        PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();
        PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
        PostingData.PostingGroupRow dr = null;
        try
        {
            ta.FillByPostingGroupID(dt, postingGroupID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load posting group data");
        }

    }

    public static void RejectProposedPostingGroupID(decimal postingGroupID, string userName)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                PostingDataTableAdapters.PostingGroupTableAdapter taPostingGroup = new PostingDataTableAdapters.PostingGroupTableAdapter();
                PostingDataTableAdapters.PostingGroupAccountTableAdapter taGroupAccountDetail =
                    new PostingDataTableAdapters.PostingGroupAccountTableAdapter();
                PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
                PostingData.PostingGroupRow dr = null;
                taPostingGroup.FillByPostingGroupID(dt, postingGroupID);
                dr = dt[0];
                string logMessage = "";
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject:  PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                                               dt[0].PostingGroupCode,
                                                               dt[0].LedgerType,
                                                               dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                               dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                   
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                taGroupAccountDetail.DeletePostingGroupAccountByPostingGroupID(postingGroupID);
                taPostingGroup.DeletePostingGroupByPostingGroupID(postingGroupID);

                AuditTrail.AddAuditTrail("PostingGroup", AuditTrail.REJECT, logMessage, userName,"Reject " + ActionFlagDesc);
                scope.Complete();
                return;
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void UpdatePostingGroup(decimal postingGroupID, string postingGroupCode, 
            string ledgerType, string approvalStatus, string approvalDesc,
           DateTime startDate, Nullable<DateTime> endDate,
           string description, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate,
            string action, decimal OriginalID,
        PostingData.PostingGroupAccountDataTable dtNewGroupAccount)
    {

        try
        {
            string logMessage = "";
            using (TransactionScope scope = new TransactionScope())
            {
                PostingDataTableAdapters.PostingGroupTableAdapter taPostingGroup = new PostingDataTableAdapters.PostingGroupTableAdapter();
                PostingDataTableAdapters.PostingGroupAccountTableAdapter taGroupAccountDetail =
                    new PostingDataTableAdapters.PostingGroupAccountTableAdapter();

                if (postingGroupID == 0)
                {
                        postingGroupID = (decimal)taPostingGroup.InsertPostingGroup(postingGroupCode, ledgerType, startDate, approvalStatus,
                            description, createdBy, createdDate, lastUpdatedBy, lastUpdatedDate, endDate,
                            approvalDesc, action, OriginalID);

                        //Posting Group Account
                        PostingData.PostingGroupAccountRow drNewGroupAcountDetail = null;
                        PostingData.PostingGroupAccountDataTable dtGroupAccountDetail =
                            new PostingData.PostingGroupAccountDataTable();

                        taGroupAccountDetail.FillByPostingGroupAccountByPostingGroupID(dtGroupAccountDetail, postingGroupID);
                        foreach (PostingData.PostingGroupAccountRow drGroupAccount in dtNewGroupAccount)
                        {
                            drNewGroupAcountDetail = dtGroupAccountDetail.FindByDrCrTypePostingGroupIDAccountID(drGroupAccount.DrCrType, postingGroupID, drGroupAccount.AccountID);

                            if (drNewGroupAcountDetail == null)
                            {
                                drNewGroupAcountDetail = dtGroupAccountDetail.NewPostingGroupAccountRow();
                            }

                            drNewGroupAcountDetail.AccountID = drGroupAccount.AccountID;
                            drNewGroupAcountDetail.DrCrType = drGroupAccount.DrCrType;
                            drNewGroupAcountDetail.PostingGroupID = postingGroupID;
                            drNewGroupAcountDetail.OriginalID = postingGroupID;
                            drNewGroupAcountDetail.ActionFlag = action;

                            if (drNewGroupAcountDetail.RowState == System.Data.DataRowState.Detached)
                            {
                                dtGroupAccountDetail.AddPostingGroupAccountRow(drNewGroupAcountDetail);
                            }

                        }
                        foreach (PostingData.PostingGroupAccountRow drGroupAccount in dtGroupAccountDetail)
                        {
                            if (drGroupAccount.RowState == System.Data.DataRowState.Unchanged)
                            {
                                drGroupAccount.Delete();
                            }
                        }

                        taGroupAccountDetail.Update(dtGroupAccountDetail);

                   

                    logMessage = string.Format("Proposed Value: PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                         postingGroupCode, ledgerType, startDate.ToString("dd-MM-yyyy"), Convert.ToDateTime(endDate).Date);
                    AuditTrail.AddAuditTrail("PostingGroup", AuditTrail.PROPOSE, logMessage, lastUpdatedBy, "Propose Update");

                    scope.Complete();
                        
                }
                else
                {
                    PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
                    PostingData.PostingGroupRow dr = null;
                    taPostingGroup.FillByPostingGroupID(dt, postingGroupID);
                    dr = dt[0];
     
                    if (dr.IsOriginalIDNull())
                    {
                        taPostingGroup.UpdatePostingGroup(ledgerType, startDate, approvalStatus, description,
                          lastUpdatedBy, lastUpdatedDate, approvalDesc, null, null, postingGroupID);

                    }
                    else
                    {
                        if (dr.IsActionFlagNull() || dr.ActionFlag == "U")
                        {
                            taPostingGroup.UpdatePostingGroup(ledgerType, startDate, approvalStatus, description,
                               lastUpdatedBy, lastUpdatedDate, approvalDesc, null, null, dr.OriginalID);


                            taGroupAccountDetail.DeletePostingGroupAccountByPostingGroupID(dr.OriginalID);

                            //Posting Group Account
                            PostingData.PostingGroupAccountRow drUpdateGroupAcountDetail = null;
                            PostingData.PostingGroupAccountDataTable dtGroupAccountDetail =
                                new PostingData.PostingGroupAccountDataTable();

                            taGroupAccountDetail.FillByPostingGroupAccountByPostingGroupID(dtGroupAccountDetail, postingGroupID);
                            foreach (PostingData.PostingGroupAccountRow drGroupAccount in dtNewGroupAccount)
                            {
                                drUpdateGroupAcountDetail = dtGroupAccountDetail.FindByDrCrTypePostingGroupIDAccountID(drGroupAccount.DrCrType, postingGroupID, drGroupAccount.AccountID);

                                drUpdateGroupAcountDetail.AccountID = drUpdateGroupAcountDetail.AccountID;
                                drUpdateGroupAcountDetail.DrCrType = drUpdateGroupAcountDetail.DrCrType;
                                drUpdateGroupAcountDetail.PostingGroupID = dr.OriginalID;
                                drUpdateGroupAcountDetail.OriginalID = 0;
                                drUpdateGroupAcountDetail.ActionFlag = null;

                                if (drUpdateGroupAcountDetail.RowState == System.Data.DataRowState.Detached)
                                {
                                    dtGroupAccountDetail.AddPostingGroupAccountRow(drUpdateGroupAcountDetail);
                                }

                            }

                            taGroupAccountDetail.Update(dtGroupAccountDetail);

                            //delete posting group 
                            taPostingGroup.DeletePostingGroupByPostingGroupID(postingGroupID);

                            logMessage = string.Format("Approved Update: PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                                    dt[0].PostingGroupCode,
                                                  dt[0].LedgerType,
                                                  dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                  dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));

                            AuditTrail.AddAuditTrail("PostingGroup", AuditTrail.APPROVE, logMessage, lastUpdatedBy,"Approve Update");

                            if (dr.IsActionFlagNull())
                            {
                                scope.Complete();

                                return;
                            }

                            scope.Complete();
                            return;
                        }
                        else if (dr.ActionFlag == "I")
                        {
                            taPostingGroup.UpdatePostingGroup(ledgerType, startDate, approvalStatus, description,
                                  lastUpdatedBy, lastUpdatedDate, approvalDesc, null, null, postingGroupID);

                            //Posting Group Account
                            PostingData.PostingGroupAccountRow drUpdateGroupAcountDetail = null;
                            PostingData.PostingGroupAccountDataTable dtGroupAccountDetail =
                                new PostingData.PostingGroupAccountDataTable();

                            taGroupAccountDetail.FillByPostingGroupAccountByPostingGroupID(dtGroupAccountDetail, postingGroupID);
                            foreach (PostingData.PostingGroupAccountRow drGroupAccount in dtNewGroupAccount)
                            {
                                drUpdateGroupAcountDetail = dtGroupAccountDetail.FindByDrCrTypePostingGroupIDAccountID(drGroupAccount.DrCrType, postingGroupID, drGroupAccount.AccountID);

                                drUpdateGroupAcountDetail.AccountID = drGroupAccount.AccountID;
                                drUpdateGroupAcountDetail.DrCrType = drGroupAccount.DrCrType;
                                drUpdateGroupAcountDetail.PostingGroupID = postingGroupID;
                                drUpdateGroupAcountDetail.OriginalID = 0;
                                drUpdateGroupAcountDetail.ActionFlag = null;

                                if (drUpdateGroupAcountDetail.RowState == System.Data.DataRowState.Detached)
                                {
                                    dtGroupAccountDetail.AddPostingGroupAccountRow(drUpdateGroupAcountDetail);
                                }

                            }
                            
                            taGroupAccountDetail.Update(dtGroupAccountDetail);

                            logMessage = string.Format("Approved Insert:  PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                                         dt[0].PostingGroupCode,
                                                       dt[0].LedgerType,
                                                       dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                       dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));
                            scope.Complete();
                            return;
                        
                        }
                        else if (dr.ActionFlag == "D")
                        {
                            taGroupAccountDetail.DeletePostingGroupAccountByPostingGroupID(dr.OriginalID);
                            taPostingGroup.DeletePostingGroupByPostingGroupID(dr.OriginalID);

                            taGroupAccountDetail.DeletePostingGroupAccountByPostingGroupID(postingGroupID);
                            taPostingGroup.DeletePostingGroupByPostingGroupID(postingGroupID);

                            logMessage = string.Format("Approved Delete: PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                                   dt[0].PostingGroupCode,
                                                 dt[0].LedgerType,
                                                 dt[0].EffectiveStartDate.ToString("dd-MM-yyyy"),
                                                 dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"));

                            AuditTrail.AddAuditTrail("PostingGroup", AuditTrail.APPROVE, logMessage, lastUpdatedBy,"Approve Delete");

                            scope.Complete();

                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }        
    }

    public static void ProposePostingGroup(string postingGroupCode, string ledgerType, string approvalDesc,
                                           DateTime startDate, Nullable<DateTime> endDate,
                                           string description, string action, string userName, decimal OriginalID)
    {
        PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(postingGroupCode, ledgerType, startDate, "P", description, userName, DateTime.Now, 
                         userName, DateTime.Now, endDate, approvalDesc, action, OriginalID);

                logMessage = string.Format("Proposed Value: PostingGroupCode={0}|LedgerType={1}|EffectiveStartDate={2}|EffectiveEndDate={3}",
                                           postingGroupCode,ledgerType, startDate.ToString("dd-MM-yyyy"), Convert.ToDateTime(endDate).Date);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                AuditTrail.AddAuditTrail("PostingGroup", AuditTrail.PROPOSE, logMessage, userName, "Propose " + ActionFlagDesc);
      
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


    public static string GetPostingGroupByPostingGroupID(decimal groupId)
    {
        PostingData.PostingGroupDataTable dt = new PostingData.PostingGroupDataTable();
        PostingData.PostingGroupRow dr = null;
        PostingDataTableAdapters.PostingGroupTableAdapter ta = new PostingDataTableAdapters.PostingGroupTableAdapter();
        string groupCode = "";

        try
        {
            ta.FillByPostingGroupID(dt, groupId);
            if (dt.Count > 0)
            {
                dr = dt[0];
                groupCode = dr.PostingGroupCode;
            }

            return groupCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    #endregion

    #region "-- Posting Group Account --"


    public static PostingData.PostingGroupAccountDataTable GetDrCr()
    {
        PostingData.PostingGroupAccountDataTable dt = new PostingData.PostingGroupAccountDataTable();
        PostingDataTableAdapters.PostingGroupAccountTableAdapter ta = new PostingDataTableAdapters.PostingGroupAccountTableAdapter();

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


    public static PostingData.PostingGroupAccountDataTable GetPostingGroupAccountByPostingGroupId(decimal postingGroupID)
    {
        PostingData.PostingGroupAccountDataTable dt = new PostingData.PostingGroupAccountDataTable();
        PostingDataTableAdapters.PostingGroupAccountTableAdapter ta = new PostingDataTableAdapters.PostingGroupAccountTableAdapter();
        try
        {
            ta.FillByPostingGroupAccountByPostingGroupID(dt, postingGroupID);
                       
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

    }


    #endregion


    #region supporting method

    public static PostingData.AccountDataTable GetPostingCodeBySetDisplayDFS(int seqDisplayDFS)
    {
        PostingData.AccountDataTable dt = new PostingData.AccountDataTable();
        PostingDataTableAdapters.AccountTableAdapter ta = new PostingDataTableAdapters.AccountTableAdapter();

        try
        {
            ta.FillBySeqDisplayDFS(dt, Convert.ToInt16(seqDisplayDFS));
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load set dislpay DFS data");
        }
    }

    #endregion

}
