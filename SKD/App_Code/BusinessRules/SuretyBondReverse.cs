using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for SuretyBondReverse
/// </summary>
public class SuretyBondReverse
{
    public SuretyBondReverse()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static SuretyBondReverseData.ViewSuretyBondReverseDataTable GetSuretyBondReverse(Nullable<DateTime> busDate,
                                                                                            string exchangeRef, string bondSN, 
                                                                                            string accCode, string approvalStatus)
    {
        SuretyBondReverseData.ViewSuretyBondReverseDataTable dt = new SuretyBondReverseData.ViewSuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter ta = new SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter();

        try
        {
            ta.FillByCriteria(dt,busDate,exchangeRef,accCode,bondSN,approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SuretyBondReverseData.HistoricalSuretyBondReverseDataTable GetHistoricalSuretyBondReverse(Nullable<DateTime> busDate,
                                                                                           string exchangeRef, string bondSN,
                                                                                           string accCode, string approvalStatus)
    {
        SuretyBondReverseData.HistoricalSuretyBondReverseDataTable dt = new SuretyBondReverseData.HistoricalSuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.HistoricalSuretyBondReverseTableAdapter ta = new SuretyBondReverseDataTableAdapters.HistoricalSuretyBondReverseTableAdapter();

        try
        {
            ta.FillByCriteria(dt,approvalStatus, busDate, exchangeRef, accCode, bondSN);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static SuretyBondReverseData.ViewSuretyBondReverseRow SelectSuretyBondByID(decimal bondID)
    {
        SuretyBondReverseData.ViewSuretyBondReverseDataTable dt = new SuretyBondReverseData.ViewSuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter ta = new SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter();
        SuretyBondReverseData.ViewSuretyBondReverseRow dr = null;
        try
        {
            ta.FillBySuretyBondId(dt, bondID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load surety bond data");
        }
    }

    public static SuretyBondReverseData.SuretyBondReverseRow GetSuretyReverseBySuretyBondID(decimal bondID)
    {
        SuretyBondReverseData.SuretyBondReverseDataTable dt = new SuretyBondReverseData.SuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter ta = new SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter();
        SuretyBondReverseData.SuretyBondReverseRow dr = null;
        try
        {
            ta.FillBySuretyBondID(dt, bondID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load surety bond data");
        }
    }

    public static SuretyBondReverseData.EODSuretyBondUsageRow GetSuretyBondUsageBySuretyBondID(decimal bondID)
    {
        SuretyBondReverseData.EODSuretyBondUsageDataTable dt = new SuretyBondReverseData.EODSuretyBondUsageDataTable();
        SuretyBondReverseDataTableAdapters.EODSuretyBondUsageTableAdapter ta = new SuretyBondReverseDataTableAdapters.EODSuretyBondUsageTableAdapter();
        SuretyBondReverseData.EODSuretyBondUsageRow dr = null;
        try
        {
            ta.FillBySuretyBondId(dt, bondID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load surety bond data");
        }
    }
    public static void ProposedSuretyBondReverse(decimal suretyBondID, string action,decimal bondRemainAmount,DateTime buyerDefault,
                                                  string notes, string approvalStatus,string userID)
    {
        SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter ta = new SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter();
        SuretyBondReverseData.ViewSuretyBondReverseRow dr = SuretyBondReverse.SelectSuretyBondByID(suretyBondID);
        SuretyBondReverseData.EODSuretyBondUsageRow drSBU = SuretyBondReverse.GetSuretyBondUsageBySuretyBondID(suretyBondID);
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

                ta.Insert(dr.TransactionDate, dr.ExchangeRef, dr.InvestorId, dr.CMID, dr.SuretyBondId, dr.BondIssuerID, dr.UsageAmount,
                          bondRemainAmount, DateTime.Now, dr.EODSuretyBondUsageId, buyerDefault, notes, approvalStatus,
                          null, null, action, userID, DateTime.Now, userID, DateTime.Now,dr.ContraInvestorId,dr.ContraCMID);
                
                logMessage = string.Format("Proposed Value: SuretyBondId={0}|BondIssuerId={1}|UsageAmount={2}|InvestorID={3}|" +
                                            "bondRemainAmount={4}",
                                             dr.SuretyBondId, dr.BondIssuerID, dr.UsageAmount, dr.InvestorId, bondRemainAmount);
                AuditTrail.AddAuditTrail("SuretyBondReverse", AuditTrail.PROPOSE, logMessage, userID, ActionFlagDesc);

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

    public static void ApprovedSuretyBondReverse(decimal suretyBondID, string userName, string approvalDesc)
    {
        SuretyBondReverseData.EODJournalLineDataTable dtEODJL = new SuretyBondReverseData.EODJournalLineDataTable();
        SuretyBondReverseDataTableAdapters.EODJournalLineTableAdapter taEODJL = new  SuretyBondReverseDataTableAdapters.EODJournalLineTableAdapter();

        SuretyBondReverseData.EODSuretyBondUsageDataTable dtBondUsage = new SuretyBondReverseData.EODSuretyBondUsageDataTable();
        SuretyBondReverseDataTableAdapters.EODSuretyBondUsageTableAdapter taBondUsage = new SuretyBondReverseDataTableAdapters.EODSuretyBondUsageTableAdapter();

        SuretyBondReverseData.SuretyBondDataTable dtSB = new SuretyBondReverseData.SuretyBondDataTable();
        SuretyBondReverseDataTableAdapters.SuretyBondTableAdapter taSB = new SuretyBondReverseDataTableAdapters.SuretyBondTableAdapter();

        SuretyBondReverseData.SuretyBondReverseDataTable dtBondReverse = new SuretyBondReverseData.SuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter taBondReverse = new SuretyBondReverseDataTableAdapters.SuretyBondReverseTableAdapter();

        SuretyBondReverseData.ViewSuretyBondReverseDataTable dtViewReverse = new SuretyBondReverseData.ViewSuretyBondReverseDataTable();
        SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter taViewReverse = new SuretyBondReverseDataTableAdapters.ViewSuretyBondReverseTableAdapter();
        SuretyBondReverseData.ViewSuretyBondReverseRow dr = SuretyBondReverse.SelectSuretyBondByID(suretyBondID);

        ParameterData.ParameterRow drParameter = Parameter.GetParameterByCodeAndApprovalStatus("BusinessDate", "A");
        int revision = EOD.GetMaxRevisionInvContractPositionEOD(drParameter.DateValue);
        //int revision = 1;
        BankData.BankAccountRow drBankAccount = BankAccount.GetBankAccountIDByAccountType("RR");
        decimal bankAccountID;
        decimal reverseAmount;

        try
        {
            try
            {
                taBondReverse.FillBySuretyBondID(dtBondReverse, suretyBondID);
                taViewReverse.FillBySuretyBondId(dtViewReverse, suretyBondID);
                bankAccountID = BankAccount.GetBankAccountIDByInvestorID(dtBondReverse[0].InvestorID);
                //amountBankTrx = dt[0].Amount - ((dt[0].AmountHaircut / 100) * dt[0].Amount);


                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    //update record
                    if (dtBondReverse[0].ActionFlag == "I")
                    {
                        //update Approval Status SuretyBondReverse
                        taBondReverse.ApprovedProposed("A", approvalDesc, null, userName, DateTime.Now, dr.SuretyBondReverseID);

                        //update tbl suretybond
                        reverseAmount = dtBondReverse[0].UsageAmount + dtBondReverse[0].BondRemainAmount;
                        taSB.UpdateBusDateReverseByID(reverseAmount, userName, DateTime.Now,dtBondReverse[0].SuretyBondID);

                        //update EODSuretyBondUsage
                        taBondUsage.UpdateEodSuretyBondUsageByID(dtBondReverse[0].BusinessDateReverse,userName, DateTime.Now, dtBondReverse[0].EODSuretyBondUsageID);


                        //Insert Journal Line for Debit
                        taEODJL.InsertJournalLine(drParameter.DateValue, revision, drParameter.DateValue, "N", 80044410, drBankAccount.BankAccountID, "D",
                                                 dtBondReverse[0].UsageAmount, "Reverse Collateral", dtBondReverse[0].ExchangeRef, null, null, userName,
                                                 DateTime.Now, userName, DateTime.Now);

                        //Insert Journal Line for Credit
                        taEODJL.InsertJournalLine(drParameter.DateValue, revision, drParameter.DateValue, "N", 80044420, bankAccountID, "C",
                                                 dtBondReverse[0].UsageAmount, "Reverse Collateral", dtBondReverse[0].ExchangeRef, dtBondReverse[0].InvestorID,
                                                 dtBondReverse[0].CMID, userName,
                                                 DateTime.Now, userName, DateTime.Now);


                        logMessage = string.Format("Approved Insert: SuretyBondReverseID={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                                                   "amount={4}|BusinessDateReverse={5}",
                                                   dtBondReverse[0].SuretyBondReverseID, dtBondReverse[0].BondIssuerID, 
                                                   dtViewReverse[0].BondSerialNo, dtBondReverse[0].InvestorID,
                                                   dtBondReverse[0].BondRemainAmount, dtBondReverse[0].BusinessDateReverse);

                    }
                    //else if (dt[0].ActionFlag == "U")
                    //{


                    //    ta.ApprovedProposed(dt[0].BondIssuerID, dt[0].EntryType, dt[0].BondSerialNo, Convert.ToDecimal(dt[0].InvestorID1), dt[0].Amount,
                    //                       dt[0].ExpireDate, dt[0].ExpDateHaircut, dt[0].ActiveStatus,
                    //                       dt[0].Notes, "A", approveDesc, userName, DateTime.Now, null, dt[0].Haircut, dt[0].RemainAmount, dt[0].BankAccountID, suretyId);


                    //    //delete proposed record
                    //    ta.DeleteSuretyBond(suretyId);

                    //    logMessage = string.Format("Approved Update: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                    //                               "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                    //                               dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID1,
                    //                               dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);
                    //}

                    //else if (dt[0].ActionFlag == "D")
                    //{
                    //    ta.DeleteSuretyBond(dt[0].OriginalId);
                    //    ta.DeleteSuretyBond(dt[0].BondIssuerID);

                    //    logMessage = string.Format("Approved Delete: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                    //                               "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                    //                               dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID1,
                    //                               dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);
                    //}
                    string ActionFlagDesc = "";
                    switch (dtBondReverse[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("SuretyBondReverse", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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
}