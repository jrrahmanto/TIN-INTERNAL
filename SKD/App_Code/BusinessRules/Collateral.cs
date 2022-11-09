using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for Collateral
/// </summary>
public class Collateral
{
    # region "-- Collateral --"

    public static CollateralData.CollateralDataTable SelectCollateralByLodgementNo(string lodgementNo)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();
        CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();
        try
        {
            ta.FillByLodgementNo(dt,lodgementNo);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load collateral data");
        }
    }


    public static CollateralData.CollateralRow SelectCollateralByCollateralID(decimal collateralID)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();
        CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();
        CollateralData.CollateralRow dr = null;
        try
        {
            ta.FillByCollateralByCollateralID(dt, collateralID);

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

    public static CollateralData.CollateralDataTable SelectCollateralByCollateralNoAndStatus(string collateralNo,
                                                        string approvalStatus,Nullable<decimal> clearingMember)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();
        CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();

        try
        {
            ta.FillByLodgementNoCMIDAndStatus(dt, collateralNo, approvalStatus, clearingMember);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank account data");
        }

    }

    public static void ProposeCollateral(string lodgementNo, decimal cmID, DateTime lodgementDate,
                                         string collateralType, string lodgementType,
                                         string issuer, DateTime issuerDate, decimal ccyCode,
                                         DateTime maturityDate, decimal nominal, decimal haircut, decimal effectiveNominal,
                                         string approvalDesc, string action, string userName, decimal OriginalID, string issuerType)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(lodgementNo, "P", cmID, lodgementDate, collateralType,lodgementType,  
                          issuer, issuerDate, maturityDate, nominal, haircut, effectiveNominal,userName, DateTime.Now, userName,
                        DateTime.Now, ccyCode, action, OriginalID, approvalDesc, issuerType);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: lodgementNo={0}|clearing member={1}|lodgement date={2}|collateral type={3}" +
                                           "|lodgement type={4}|issuer={5}|issuerdate={6}|currency={7}|maturitydate={8}|nominal={9}|haircut={10}|effectivenominal={11}|" +
                                           "issuertype={12}" ,
                                        lodgementNo, cmID,lodgementDate.ToString("dd-MM-yyyy"), collateralType,
                                       lodgementType,issuer,issuerDate.ToString("dd-MM-yyyy"),ccyCode,
                                        maturityDate.ToString("dd-MM-yyyy"), nominal, haircut, effectiveNominal, issuerType);
                AuditTrail.AddAuditTrail("Collateral", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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


    public static void ApproveCollateral(decimal collateralID, string userName)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();
        CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();

        try
        {
            try
            {
                ta.FillByCollateralByCollateralID(dt, collateralID);

               using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                       
                            ta.ApprovedProposedItem(dt[0].LodgementNo, "A",dt[0].CMID,dt[0].LodgementDate,
                                                   dt[0].CollateralType, dt[0].LodgementType, dt[0].Issuer, dt[0].IssuerDate,
                                                   dt[0].MaturityDate, dt[0].Nominal, dt[0].Haircut, 
                                                   dt[0].EffectiveNominal, userName, DateTime.Now,
                                                   dt[0].CurrencyID, null, null, dt[0].ApprovalDesc, dt[0].IssuerType, dt[0].LodgementID);

                            logMessage = string.Format("Approved Insert: lodgementNo={0}|clearing member={1}|lodgement date={2}|collateral type={3}" +
                                               "|lodgement type={4}|issuer={5}|issuerdate={6}|currency={7}|maturitydate={8}|nominal={9}|haircut={10}" +
                                               "|effectivenominal={11}|issuertype={12}",
                                                          dt[0].LodgementNo,
                                                         dt[0].CMCode,
                                                         dt[0].LodgementDate,
                                                         dt[0].CollateralType,
                                                         dt[0].LodgementType,
                                                         dt[0].Issuer, dt[0].IssuerDate,
                                                         dt[0].CurrencyID,
                                                         dt[0].MaturityDate,
                                                         dt[0].Nominal, dt[0].Haircut, dt[0].EffectiveNominal, dt[0].IssuerType);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].LodgementNo, "A", dt[0].CMID, dt[0].LodgementDate,
                                                    dt[0].CollateralType, dt[0].LodgementType, dt[0].Issuer, dt[0].IssuerDate,
                                                    dt[0].MaturityDate, dt[0].Nominal, dt[0].Haircut,
                                                    dt[0].EffectiveNominal, userName, DateTime.Now,
                                                    dt[0].CurrencyID, null, dt[0].ApprovalDesc, dt[0].IssuerType, dt[0].OriginalID);
                      
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].LodgementID);
                        logMessage = string.Format("Approved Update: lodgementNo={0}|clearing member={1}|lodgement date={2}|collateral type={3}" +
                                        "|lodgement type={4}|issuer={5}|issuerdate={6}|currency={7}|maturitydate={8}|nominal={9}|haircut={10}" +
                                        "|effectivenominal={11}|issuertype={12}",
                                                   dt[0].LodgementNo,
                                                  dt[0].CMCode,
                                                  dt[0].LodgementDate,
                                                  dt[0].CollateralType,
                                                  dt[0].LodgementType,
                                                  dt[0].Issuer, dt[0].IssuerDate,
                                                  dt[0].CurrencyID,
                                                  dt[0].MaturityDate,
                                                  dt[0].Nominal, dt[0].Haircut, dt[0].EffectiveNominal, dt[0].IssuerType);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].LodgementID);
                        logMessage = string.Format("Approved Delete: lodgementNo={0}|clearing member={1}|lodgement date={2}|collateral type={3}" +
                                               "|lodgement type={4}|issuer={5}|issuerdate={6}|currency={7}|maturitydate={8}|nominal={9}|haircut={10}" +
                                               "|effectivenominal={11}|issuertype={12}",
                                                          dt[0].LodgementNo,
                                                         dt[0].CMCode,
                                                         dt[0].LodgementDate,
                                                         dt[0].CollateralType,
                                                         dt[0].LodgementType,
                                                         dt[0].Issuer, dt[0].IssuerDate,
                                                         dt[0].CurrencyID,
                                                         dt[0].MaturityDate,
                                                         dt[0].Nominal, dt[0].Haircut, dt[0].EffectiveNominal, dt[0].IssuerType);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Collateral", AuditTrail.APPROVE, logMessage, userName,"Approve " + ActionFlagDesc);

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

    public static void RejectProposedCollateral(decimal collateralId, string userName)
    {
        CollateralDataTableAdapters.CollateralTableAdapter ta = new CollateralDataTableAdapters.CollateralTableAdapter();
        CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByCollateralByCollateralID(dt, collateralId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : llodgementNo={0}|clearing member={1}|lodgement date={2}|collateral type={3}" +
                                               "|lodgement type={4}|issuer={5}|issuerdate={6}|currency={7}|maturitydate={8}|nominal={9}|haircut={10}" +
                                               "|effectivenominal={11}|issuertype={12}",
                                                          dt[0].LodgementNo,
                                                         dt[0].CMCode,
                                                         dt[0].LodgementDate,
                                                         dt[0].CollateralType,
                                                         dt[0].LodgementType,
                                                         dt[0].Issuer, dt[0].IssuerDate,
                                                         dt[0].CurrencyID,
                                                         dt[0].MaturityDate,
                                                         dt[0].Nominal, dt[0].Haircut, dt[0].EffectiveNominal, dt[0].IssuerType);
                }
                ta.DeleteRejectItem(collateralId);

                AuditTrail.AddAuditTrail("Collateral", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


    #endregion
}
