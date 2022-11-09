using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for SuretyBond
/// </summary>
public class SuretyBond
{
    public SuretyBond()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static SuretyBondData.VSuretyBondDataTable GetSuretyBond()
    {
        SuretyBondData.VSuretyBondDataTable dt = new SuretyBondData.VSuretyBondDataTable();
        SuretyBondDataTableAdapters.VSuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.VSuretyBondTableAdapter();

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

    public static SuretyBondData.VSuretyBondRow SelectSuretyBondByID(decimal bondID)
    {
        SuretyBondData.VSuretyBondDataTable dt = new SuretyBondData.VSuretyBondDataTable();
        SuretyBondDataTableAdapters.VSuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.VSuretyBondTableAdapter();
        SuretyBondData.VSuretyBondRow dr = null;
        try
        {
            ta.FillBySuretyID(dt, bondID);

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

    public static bool ValidSuretyBondByIssuerAndSN(decimal issuerID, string bondSN)
    {
        SuretyBondData.SuretyBondDataTable dt = new SuretyBondData.SuretyBondDataTable();
        SuretyBondDataTableAdapters.SuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.SuretyBondTableAdapter();
        bool isValid = false;
        try
        {
            ta.FillByIssuerIDNBondSN(dt, issuerID,bondSN);

            if (dt.Count > 0)
            {
                isValid = true;
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load surety bond data");
        }
    }
    public static SuretyBondData.VSuretyBondDataTable GetDataSuretyBondAndApprovalStatus(string entryType, string approvalStatus, Nullable<decimal> bondIssuer, string bondSN)
    {
        SuretyBondData.VSuretyBondDataTable dt = new SuretyBondData.VSuretyBondDataTable();
        SuretyBondDataTableAdapters.VSuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.VSuretyBondTableAdapter();

        try
        {
            ta.FillByEntryTypeAndApprovalStatus(dt, entryType, approvalStatus,bondIssuer,bondSN);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Surety Bond data");
        }
    }

    public static SuretyBondData.SuretyBondDataTable GetDataSuretyBondByEntryType(string entryType)
    {
        SuretyBondData.SuretyBondDataTable dt = new SuretyBondData.SuretyBondDataTable();
        SuretyBondDataTableAdapters.SuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.SuretyBondTableAdapter();

        try
        {
            ta.FillByEntryType(dt, entryType);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Surety Bond data");
        }
    }

    public static SuretyBondData.LookUpSuretyBondDataTable GetDataForLookUpSuretyBond(string investorName, string accCode,string approvalStatus, string ActiveStatus)
    {
        SuretyBondData.LookUpSuretyBondDataTable dt = new SuretyBondData.LookUpSuretyBondDataTable();
        SuretyBondDataTableAdapters.LookUpSuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.LookUpSuretyBondTableAdapter();

        try
        {
            ta.Fill(dt,ActiveStatus,approvalStatus, investorName, accCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Surety Bond data");
        }
    }
    public static void ProposeSuretyBond(string entryType,decimal idBond,string bondSN, decimal invID,
                                         decimal amount, decimal amountHaircut, DateTime expiredDate, DateTime expDateHaircut,
                                         string activeStatus, string notes, string approvalDesc,
                                        string action, string userName, Nullable<decimal> OriginalID)
    {

        SuretyBondDataTableAdapters.SuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.SuretyBondTableAdapter();
        try
        {
            string logMessage;
            decimal remainAmount = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                string ActionFlagDesc = "";
                remainAmount = amount - (amount * amountHaircut / 100);
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                ta.Insert(idBond, entryType, bondSN, invID, amount, amountHaircut,remainAmount, expiredDate, expDateHaircut,
                            activeStatus, notes, "P", null, userName, DateTime.Now, userName, DateTime.Now, action, OriginalID,null);
                //ta.Insert(issuerNm, "P", approvalDesc, userName, DateTime.Now, userName,
                //            DateTime.Now, action, OriginalID, notes);

                logMessage = string.Format("Proposed Value: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" + 
                                            "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                                             entryType,idBond,bondSN,invID,amount,amountHaircut,expiredDate,expDateHaircut);
                AuditTrail.AddAuditTrail("SuretyBond", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

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
    public static void ApproveSuretyBond(decimal suretyId, string userName, string approveDesc, string notes)
    {
        SuretyBondData.SuretyBondDataTable dt = new SuretyBondData.SuretyBondDataTable();
        SuretyBondDataTableAdapters.SuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.SuretyBondTableAdapter();
        SuretyBondData.VInvestorDataTable dtTF = new  SuretyBondData.VInvestorDataTable();
        SuretyBondDataTableAdapters.VInvestorTableAdapter tatf = new  SuretyBondDataTableAdapters.VInvestorTableAdapter();
        BankDataTableAdapters.BankAccountTableAdapter tabank = new BankDataTableAdapters.BankAccountTableAdapter();

        string accNoBankAcc = "";
        decimal bankID;
        decimal cmID;
        decimal amountBankTrx= 0;
        decimal bankAccID = 0;
        try
        {
            try
            {
                ta.FillBySuretyId(dt, suretyId);
                tatf.Fill(dtTF, Convert.ToDecimal(dt[0].InvestorID));
                bankID = Bank.GetBankDataByCodeParameter();
                cmID = Convert.ToDecimal(dtTF[0].CMID);
                accNoBankAcc = dtTF[0].AccountCode + "C" + dt[0].BondIssuerID + suretyId;
                //amountBankTrx = dt[0].Amount - ((dt[0].AmountHaircut / 100) * dt[0].Amount);

                Console.WriteLine(dt[0].OriginalId);
                Console.WriteLine(dt[0].SuretyBondID);

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";
                   
                    //update record
                    if (dt[0].ActionFlag == "I")
                    {

                        ta.ApprovedProposed(dt[0].BondIssuerID, dt[0].EntryType, dt[0].BondSerialNo, Convert.ToDecimal(dt[0].InvestorID), dt[0].Amount,
                                           dt[0].ExpireDate, dt[0].ExpDateHaircut, dt[0].ActiveStatus,
                                           dt[0].Notes, "A", approveDesc, userName, DateTime.Now, null,dt[0].Haircut,dt[0].RemainAmount,null, suretyId);

                        //insert Bank Account
                        //tabank.Insert(accNoBankAcc, bankID, "RC", userName, DateTime.Now, userName, DateTime.Now, "A", DateTime.Now,
                        //             null, null, Convert.ToDecimal(dt[0].InvestorID1), cmID, 1, null, null, "A");

                       

                        logMessage = string.Format("Approved Insert: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                                                   "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                                                   dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID,
                                                   dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {


                        ta.ApprovedProposed(dt[0].BondIssuerID, dt[0].EntryType, dt[0].BondSerialNo, Convert.ToDecimal(dt[0].InvestorID), dt[0].Amount,
                                           dt[0].ExpireDate, dt[0].ExpDateHaircut, dt[0].ActiveStatus,
                                           dt[0].Notes, "A", approveDesc, userName, DateTime.Now, null, dt[0].Haircut, dt[0].RemainAmount, null, dt[0].OriginalId);


                        //delete proposed record
                        ta.DeleteSuretyBond(suretyId);

                        logMessage = string.Format("Approved Update: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                                                   "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                                                   dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID,
                                                   dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);
                    }

                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteSuretyBond(dt[0].OriginalId);
                        ta.DeleteSuretyBond(dt[0].SuretyBondID);

                        logMessage = string.Format("Approved Delete: EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                                                   "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                                                   dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID,
                                                   dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("SuretyBond", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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
        SuretyBondData.SuretyBondDataTable dt = new SuretyBondData.SuretyBondDataTable();
        SuretyBondDataTableAdapters.SuretyBondTableAdapter ta = new SuretyBondDataTableAdapters.SuretyBondTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillBySuretyId(dt, bondId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject:  EntryType={0}|BondId={1}|BondSN={2}|InvestorID={3}|" +
                                                   "amount={4}|AmountHaircut={5}|ExpiredDate={6}|ExpiredDateHairCut={7}",
                                                   dt[0].EntryType, dt[0].BondIssuerID, dt[0].BondSerialNo, dt[0].InvestorID,
                                                   dt[0].Amount, dt[0].Haircut, dt[0].ExpireDate, dt[0].ExpDateHaircut);

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.DeleteSuretyBond(bondId);

                AuditTrail.AddAuditTrail("SuretyBond", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}