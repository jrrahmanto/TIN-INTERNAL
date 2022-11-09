using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for ClearingMember
/// </summary>
public class ClearingMember
{

    #region "   Supporting Method   "
    static decimal idCM;
    public static ClearingMemberData.ClearingMemberDataTable GetClearingMember()
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.Fill(dt);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }        

        return dt;
    }

    public static void UpdateDocument(
            string noAktaPendiri, string noAktaPerusahaan, string domisiliPerusahaan, string npwp, string identitasKepabean, string eksportirTerdaftarTimah, string perizinanInstansiEksportir,
            string siup, string nib, string identitasDiriPengurus, string laporanKeuangan, string suratRefBankNegeri, string companyProfile, int cmid)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter cmpta = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();

        cmta.DeleteDocumentCM(
            noAktaPendiri, noAktaPerusahaan, domisiliPerusahaan, npwp, identitasKepabean, eksportirTerdaftarTimah, perizinanInstansiEksportir, siup, nib,
            identitasDiriPengurus, laporanKeuangan, suratRefBankNegeri, companyProfile, cmid);

        cmpta.DeleteDocumentCMProfile(
            noAktaPendiri, noAktaPerusahaan, domisiliPerusahaan, npwp, identitasKepabean, eksportirTerdaftarTimah, perizinanInstansiEksportir, siup, nib,
            identitasDiriPengurus, laporanKeuangan, suratRefBankNegeri, companyProfile, cmid);
    }

    public static ClearingMemberData.ClearingMemberDataTable GetActiveClearingMember(DateTime businessDate)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillByActiveClearingMember(dt, businessDate);
            int i = dt.Rows.Count;
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ClearingMemberData.ClearingMemberDataTable GetClearingMemberByCodeAndName(string code, string name)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillByCodeAndName(dt,  code, name);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ClearingMemberData.ClearingMemberDataTable GetClearingMemberInvoiceByCodeAndName(string code, string name)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillInvoiceByCodeAndName(dt, code, name);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetClearingMemberCodeByClearingMemberID(decimal clearingMemberID)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberData.ClearingMemberRow dr = null;
        string clearingMemberCode = "";
        try
        {
            ta.FillByCMID(dt, clearingMemberID);

            if (dt.Count > 0)
            {
                dr = dt[0];
                clearingMemberCode = dr.Code;
            }

            return clearingMemberCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetClearingMemberNameByClearingMemberID(decimal clearingMemberID)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberData.ClearingMemberRow dr = null;
        string clearingMemberName = "";

        try
        {
            ta.FillByCMID(dt, clearingMemberID);

            if (dt.Count > 0)
            {
                dr = dt[0];
                clearingMemberName = dr.Name;
            }

            return clearingMemberName;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetCMSender(decimal invID)
    {
        //ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        string cmSender = "";

        cmSender =Convert.ToString(ta.GetCMSender(invID)); //ta.GetCMSender(investorID);

        return cmSender;
    }
    #endregion

    #region "   Search Method   "

    public static ClearingMemberData.ClearingMemberViewDataTable GetClearingMemberBySearchCriteria(string code, 
                                                             string cmType, string exchCode, string status)
    {
        ClearingMemberData.ClearingMemberViewDataTable dt = new ClearingMemberData.ClearingMemberViewDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberViewTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberViewTableAdapter();

        try
        {
            ta.FillCriteriaCM(dt, code, cmType,exchCode,status);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }
    
    public static ClearingMemberData.ClearingMemberDataTable GetClearingMemberByCMID(string CMID)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillByCMID(dt, Convert.ToDecimal(CMID)); 
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static ClearingMemberData.ClearingMemberRow SelectClearingMemberByCMID(string CMID)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberData.ClearingMemberRow dr = null;

        try
        {
            ta.FillByCMID(dt, Convert.ToDecimal(CMID));
            if (dt.Count > 0)
            {
                dr = dt[0];
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dr;
    }
    public static ClearingMemberData.CMProfileDataTable GetClearingMemberByCMProfileID(string CMProfileID)
    {
        ClearingMemberData.CMProfileDataTable dt = new ClearingMemberData.CMProfileDataTable();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter ta = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();

        try
        {
            ta.FillByCMProfileID(dt, Convert.ToDecimal(CMProfileID));
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    public static ClearingMemberData.CMProfileViewDataTable GetCMProfileBySearchCriteria(string code,
                                                                                          string cmType,
                                                                                          string exchangeCode,
                                                                                          string approvalStatus)
    {
        ClearingMemberData.CMProfileViewDataTable dt = new ClearingMemberData.CMProfileViewDataTable();
        ClearingMemberDataTableAdapters.CMProfileViewTableAdapter ta= new ClearingMemberDataTableAdapters.CMProfileViewTableAdapter();

        try
        {
            ta.Fill(dt, code, cmType, exchangeCode, approvalStatus);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return dt;
    }

    #endregion

    public static void ValidateRecord(decimal EditedCMID, string ApprovalStatus)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();

        ta.FillByCMID(dt, EditedCMID);

        if (dt.Count > 0)
        {
            if (dt[0].ApprovalStatus == "P" && ApprovalStatus == "P")
            {
                throw new ApplicationException("This record is not allowed to be edited / deleted. Please wait for checker approval.");
            }

            if (dt[0].ApprovalStatus == "A" && (ApprovalStatus == "A" || ApprovalStatus == "R"))
            {
                throw new ApplicationException("Approved row is not allowed to be approved or rejected.");
            }
        }

    }

    public static void ValidateProfileRecord(decimal EditedCMProfileID, string ApprovalStatus)
    {
        ClearingMemberDataTableAdapters.CMProfileTableAdapter ta = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();
        ClearingMemberData.CMProfileDataTable dt = new ClearingMemberData.CMProfileDataTable();

        ta.FillByCMProfileID(dt, EditedCMProfileID);

        if (dt.Count > 0)
        {
            if (dt[0].ApprovalStatus == "P" && ApprovalStatus == "P")
            {
                throw new ApplicationException("This record is not allowed to be edited / deleted. Please wait for checker approval.");
            }

            if (dt[0].ApprovalStatus == "A" && (ApprovalStatus == "A" || ApprovalStatus == "R"))
            {
                throw new ApplicationException("Approved row is not allowed to be approved or rejected.");
            }
        }
    }

    public static DateTime GetStartDate(decimal CMProfileID)
    {
        ClearingMemberDataTableAdapters.CMProfileTableAdapter ta = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();
        
        return DateTime.Parse(ta.GetStartDate(CMProfileID).ToString());
    }

    
    #region "   Use Case   "

    // Create a new record for insert / update
    public static void ProposeInsertUpdate(string cmCode, string cmName , DateTime effectiveDate, string PPKP,
                                     string webSite, string CMStatus, string AgreementNo,
                                     Nullable<DateTime> AgreementDate, String exchangeStatus, string certNo,
                                     Nullable<DateTime> certDate, string spaTraderNo, Nullable<DateTime> spaTraderDate,
                                     string spaBrokerNo, Nullable<DateTime> spaBrokerDate, string PALNLicense,
                                     Nullable<DateTime> companyAniversary, byte[] imageLogo, string agreementType,
                                     string userName, string actionFlag, Nullable<decimal> originalID,
                                     string proposedFlag, bool isLogoUpdated, Nullable<DateTime> endDate,
                                     decimal initialMarginMultiplier, decimal minReqInitialMarginIDR,
                                     decimal minReqInitialMarginUSD,decimal CMID, string address, string email, decimal regionId,
                                     string phoneNumber, string contactPerson, string contactPhone, Nullable<DateTime> regDate, 
                                     string cmAccNo, string cmBankName, string cmAccName,string region)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.ImageTableAdapter imgTa = new ClearingMemberDataTableAdapters.ImageTableAdapter();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter cmProfileTa = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter emTa = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();

        decimal imageID = 0;
        string ActionFlagDesc = "";
        
        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
            {
                // Validate record if the same record is pending for approval by checker 
                if (originalID.HasValue)
                    ValidateProfileRecord(Convert.ToDecimal(originalID), "P");

                // Image Company update
                if (isLogoUpdated)
                {
                    imgTa.Insert("P", imageLogo, userName, DateTime.Now, userName, DateTime.Now, null);
                    imageID = Convert.ToDecimal(imgTa.getMaxImageID());
                }
                else
                {
                    //get image id
                    if (actionFlag == "U")
                    {
                        imageID = Convert.ToDecimal(cmTa.getImageID(Convert.ToDecimal(originalID)));
                    }
                }

                // Creating a new version
                if (actionFlag == "U")
                {
                    if (effectiveDate != GetStartDate(originalID.Value))
                    {
                        actionFlag = "V";
                    }
                }

                // Create a new record in ClearingMember table to get CMID as reference for CMProfile
                if (actionFlag == "I")
                {
                    CMID = (decimal)cmTa.InsertCM(cmCode, "P", effectiveDate, cmName, PPKP, webSite, CMStatus, AgreementNo,
                                   AgreementDate, exchangeStatus, certNo, certDate, spaTraderNo,
                                   spaTraderDate, spaBrokerNo, spaBrokerDate, PALNLicense, companyAniversary,
                                   imageID, agreementType, userName, DateTime.Now, userName,
                                   DateTime.Now, endDate, null, null, actionFlag, initialMarginMultiplier,
                                   minReqInitialMarginIDR, minReqInitialMarginUSD,address,email,regionId,phoneNumber,contactPerson,contactPhone,regDate,cmAccNo,cmBankName,cmAccName);

                }
                if (actionFlag == "U")
                {

                }

                // Create a new Clearing Member profile for history
                //cmProfileTa.Insert("P", effectiveDate, cmName, cmCode, PPKP, webSite, CMStatus, AgreementNo
                //            , AgreementDate, exchangeStatus, certNo, certDate, spaTraderNo, spaTraderDate, spaBrokerNo, spaBrokerDate
                //            , PALNLicense, companyAniversary, imageID, agreementType, userName, DateTime.Now, userName
                //            , DateTime.Now, endDate, null, originalID, actionFlag, initialMarginMultiplier,
                //            minReqInitialMarginIDR, minReqInitialMarginUSD, address, email, phoneNumber, contactPerson, contactPhone, regDate, cmAccNo, cmBankName, cmAccName, null, CMID
                //            ,null, null, null, null, null, null, null, null, null//, null, null);
                //            , null, null, null, null, null, null, null, null
                //            );


                //emTa.Insert(cmCode, 1, "P", DateTime.Now, CMID, cmName, "N", userName,
                //    DateTime.Now, userName, DateTime.Now, null, null, "P", null, "I", null);

                // Create audit trail message
                string logMessage = string.Format(proposedFlag + ", Code:{0} | Name:{1} | PPKP:{2} |" +
                                                  " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                                  " AgreementDate {6} | ExchangeStatus:{7} | certNo:{8} |" +
                                                  " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                                  " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                                  " CompanyAnniversary {15} | ImageID:{16} | agreemenType{17}",
                                                  cmCode, cmName, PPKP, webSite, CMStatus, AgreementNo,
                                                  AgreementDate.ToString(), exchangeStatus, certNo,
                                                  certDate.ToString(), spaBrokerNo,
                                                  spaTraderDate.ToString(), spaBrokerNo, spaBrokerDate.ToString(),
                                                  PALNLicense, companyAniversary, imageID, agreementType);
                switch (actionFlag)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "V": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                // Log audit trail
                AuditTrail.AddAuditTrail("ClearingMember", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

                // Commit transaction
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist. Please input new record.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    // Delete the existing record
    public static void ProposeDelete(decimal CMProfileID, string userName)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter cmProfileTa = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();

        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberData.CMProfileDataTable dtProfile = new ClearingMemberData.CMProfileDataTable();

        Nullable<DateTime> AgreementDate = null;
        Nullable<DateTime> CertificateDate = null;
        Nullable<DateTime> SPATraderDate = null;
        Nullable<DateTime> SPABrokerDate = null;
        Nullable<DateTime> AnniversaryDate = null;
        Nullable<DateTime> EndDate = null;

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                cmProfileTa.FillByCMProfileID(dtProfile, CMProfileID);
                //cmTa.FillByCMID(dt, CMProfileID);
                if (dtProfile.Count > 0)
                {
                    // Validation for null value
                    if (!dtProfile[0].IsAgreementDateNull())
                        AgreementDate = dtProfile[0].AgreementDate;
                    if (!dtProfile[0].IsCertDateNull())
                        CertificateDate = dtProfile[0].CertDate;
                    if (!dtProfile[0].IsSPATraderDateNull())
                        SPATraderDate = dtProfile[0].SPATraderDate;
                    if (!dtProfile[0].IsSPABrokerDateNull())
                        SPABrokerDate = dtProfile[0].SPABrokerDate;
                    if (!dtProfile[0].IsCompAnniversaryNull())
                        AnniversaryDate = dtProfile[0].CompAnniversary;
                    if (!dtProfile[0].IsEffectiveEndDateNull())
                        EndDate = dtProfile[0].EffectiveEndDate;


                    cmProfileTa.Insert("P", dtProfile[0].EffectiveStartDate, dtProfile[0].Name, dtProfile[0].Code, (dtProfile[0].IsPPKPNull()) ? null : dtProfile[0].PPKP, (dtProfile[0].IsWebsiteNull()) ? null : dtProfile[0].Website, dtProfile[0].CMStatus
                                , (dtProfile[0].IsAgreementNoNull()) ? null : dtProfile[0].AgreementNo, AgreementDate, dtProfile[0].ExchangeStatus, dtProfile[0].CertNo, CertificateDate
                                , dtProfile[0].SPATraderNo, SPATraderDate, dtProfile[0].SPABrokerNo, SPABrokerDate
                                , dtProfile[0].PALNLicense, AnniversaryDate, dtProfile[0].CompImageID, dtProfile[0].AgreementType, userName, DateTime.Now, userName
                                , DateTime.Now, EndDate, null, CMProfileID, "D", dtProfile[0].InitialMarginMultiplier, dtProfile[0].MinReqInitialMarginIDR,
                                dtProfile[0].MinReqInitialMarginUSD, dt[0].Address, dt[0].Email, dt[0].PhoneNumber
                                , dt[0].ContactPersonName, dt[0].ContactPersonPhone, dt[0].RegistrationDate, dt[0].CMAccountNo, dt[0].CMBankName, dt[0].CMAccountName, null, dtProfile[0].CMID
                                ,null, null, null, null, null, null, null, null, null//, null, null);
                                , null, null, null, null, null, null, null, null
                                );

                    string logMessage = string.Format("Propose Delete, Code:{0} | Name:{1} | PPKP:{2} |" +
                                                 " Website:{3} | CMStatus:{4} | AgreementNo:{5} |" +
                                                 " AgreementDate:{6} | ExchangeStatus:{7} | CertNo:{8} |" +
                                                 " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                                 " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                                 " CompanyAnniversary:{15} | ImageID:{16} | AgreementType:{17}",
                                                 dtProfile[0].Code, dtProfile[0].Name, (dtProfile[0].IsPPKPNull()) ? null : dtProfile[0].PPKP, (dtProfile[0].IsWebsiteNull()) ? null : dtProfile[0].Website, dtProfile[0].CMStatus,
                                                 (dtProfile[0].IsAgreementNoNull()) ? null : dtProfile[0].AgreementNo, AgreementDate.ToString(), dtProfile[0].ExchangeStatus,
                                                 dtProfile[0].CertNo, CertificateDate.ToString(), dtProfile[0].SPATraderNo,
                                                 SPATraderDate.ToString(), dtProfile[0].SPABrokerNo, SPATraderDate.ToString(),
                                                 dtProfile[0].PALNLicense, AnniversaryDate.ToString(), dtProfile[0].CompImageID, dtProfile[0].AgreementType);

                    AuditTrail.AddAuditTrail("ClearingMember", AuditTrail.PROPOSE, logMessage, userName, "Propose Delete");
                    scope.Complete();
                }
                else
                    throw new ApplicationException("Unable to get data from Clearing Member Profile");
            }
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    // Approve for the existing record (insert,update,delete)
    public static void Approve(decimal CMID, string userName, string approvalDesc, decimal cmProfileID, string address, string email,
                                     string phoneNumber, string contactPerson, string contactPhone, Nullable<DateTime> regDate)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter cmProfileTA = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();
        BankDataTableAdapters.BankAccountTableAdapter baTa = new BankDataTableAdapters.BankAccountTableAdapter();
        InvestorDataTableAdapters.InvestorTableAdapter iTa = new InvestorDataTableAdapters.InvestorTableAdapter();
        SuspendAccStatusDataTableAdapters.TFMemberInfoTableAdapter miTa = new SuspendAccStatusDataTableAdapters.TFMemberInfoTableAdapter();
        SuspendAccStatusDataTableAdapters.TradefeedMemberInfoTableAdapter infoTa = new SuspendAccStatusDataTableAdapters.TradefeedMemberInfoTableAdapter();
        ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter emTa = new ExchangeMemberDataTableAdapters.ExchangeMemberTableAdapter();
        ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter cmeTa = new ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter();
        FinancialInfoDataTableAdapters.FinancialInfoTableAdapter fita = new FinancialInfoDataTableAdapters.FinancialInfoTableAdapter();

        ClearingMemberData.CMProfileDataTable dtProfile = new ClearingMemberData.CMProfileDataTable();
        ClearingMemberExchangeData.ClearingMemberExchangeDataTable dtCME = new ClearingMemberExchangeData.ClearingMemberExchangeDataTable();
        ExchangeMemberData.ExchangeMemberDataTable dtEM = new ExchangeMemberData.ExchangeMemberDataTable();
        InvestorData.InvestorDataTable dtInvestor = new InvestorData.InvestorDataTable();
        
        string approvedFlag = "";
        string ActionFlagDesc = "";
        Nullable<DateTime> AgreementDate = null;
        Nullable<DateTime> CertificateDate = null;
        Nullable<DateTime> SPATraderDate = null;
        Nullable<DateTime> SPABrokerDate = null;
        Nullable<DateTime> AnniversaryDate = null;
        Nullable<DateTime> EndDate = null;

        try 
	    {
            using (TransactionScope scope = new TransactionScope())
            {
                // Validate record if the same record is pending for approval by checker 
                ValidateProfileRecord(cmProfileID, "A");

                // Fill data table
                cmProfileTA.FillByCMProfileID(dtProfile, cmProfileID);
                cmeTa.FillByCMID(dtCME, dtProfile[0].CMID);
                
                if (dtProfile.Count > 0)
                {
                    // Validation for null value
                    if (!dtProfile[0].IsAgreementDateNull())
                        AgreementDate = dtProfile[0].AgreementDate;
                    if (!dtProfile[0].IsCertDateNull())
                        CertificateDate = dtProfile[0].CertDate;
                    if (!dtProfile[0].IsSPATraderDateNull())
                        SPATraderDate = dtProfile[0].SPATraderDate;
                    if (!dtProfile[0].IsSPABrokerDateNull())
                        SPABrokerDate = dtProfile[0].SPABrokerDate;
                    if (!dtProfile[0].IsCompAnniversaryNull())
                        AnniversaryDate = dtProfile[0].CompAnniversary;
                    if (!dtProfile[0].IsEffectiveEndDateNull())
                        EndDate = dtProfile[0].EffectiveEndDate;

                    // Approve new record
                     if (dtProfile[0].ActionFlag == "I")
                     {
                        approvedFlag = "Approved Insert";

                         // Update record status of ClearingMember to 'A'
                        cmTa.ApproveInsert("A", approvalDesc, null, null, userName, DateTime.Now, dtProfile[0].CMID);

                         // Update record status of CMProfile  to 'A'
                        cmProfileTA.ApproveInsert("A", approvalDesc, null, null, userName, DateTime.Now, cmProfileID);

                        cmeTa.ApproveInsert("A",approvalDesc,null, userName, DateTime.Now, dtCME[0].CMExchangeID);

                        emTa.Insert(dtProfile[0].Code, 1, "A", DateTime.Now, CMID, dtProfile[0].Name, "N", userName,
                             DateTime.Now, userName, DateTime.Now, null, null, "A", null, null, null);

                        DateTime businessdate = DateTime.Now.Date;
                        
                        int? sequenceInfo = infoTa.MaxMemberSeq(businessdate);
                        int? sequenceFinancial = fita.MaxNewFinancialSeq(businessdate);

                        sequenceInfo = sequenceInfo == null ? 0 : sequenceInfo;
                        sequenceFinancial = sequenceFinancial == null ? 0 : sequenceFinancial;

                        //force insert MemberInfo Record
                        if (dtCME[0].CMType == "B")
                        {
                            miTa.InsertApproveOnRegister(DateTime.Now.Date, ++sequenceInfo, "I", DateTime.Now, "889", dtProfile[0].Code, dtProfile[0].Code + "0577"
                                , dtCME[0].CMType, "ALL", null, dtCME[0].CMType, dtProfile[0].Name
                                , dtProfile[0].Email, userName, DateTime.Now, userName, DateTime.Now, "A", "N");

                            miTa.InsertApproveOnRegister(DateTime.Now.Date, ++sequenceInfo, "I", DateTime.Now, "889", dtProfile[0].Code, dtProfile[0].Code + "3677"
                                , dtCME[0].CMType, "ALL", null, dtCME[0].CMType, dtProfile[0].Name
                                , dtProfile[0].Email, userName, DateTime.Now, userName, DateTime.Now, "A", "Y");

                            fita.Insert(DateTime.Now.Date, "F", "A", ++sequenceFinancial, DateTime.Now, "899", dtProfile[0].Code + "0577", "USD", 0, userName, DateTime.Now, userName
                                , DateTime.Now, "", null, "", null, null);

                            fita.Insert(DateTime.Now.Date, "F", "A", ++sequenceFinancial, DateTime.Now, "899", dtProfile[0].Code + "3677", "USD", 0, userName, DateTime.Now, userName
                                , DateTime.Now, "", null, "", null, null);
                        }
                        else
                        {
                            miTa.InsertApproveOnRegister(DateTime.Now.Date, ++sequenceInfo, "I", DateTime.Now, "889", dtProfile[0].Code, dtProfile[0].Code + "4577"
                                , dtCME[0].CMType, "ALL", null, dtCME[0].CMType, dtProfile[0].Name
                                , dtProfile[0].Email, userName, DateTime.Now, userName, DateTime.Now, "A", "N");

                            fita.Insert(DateTime.Now.Date, "F", "A", ++sequenceFinancial, DateTime.Now, "899", dtProfile[0].Code + "4577", "USD", 0, userName, DateTime.Now, userName
                                , DateTime.Now, "", null, "", null, null);
                        }
                    }

                     // Approve new version of ClearingMember
                     else if (dtProfile[0].ActionFlag == "V")
                     {
                         approvedFlag = "Approved Update";

                         // Update ClearingMember table with the newly inserted from CMProfile
                         cmTa.ApproveUpdate(dtProfile[0].Code, "A", dtProfile[0].EffectiveStartDate, dtProfile[0].Name, (dtProfile[0].IsPPKPNull()) ? null : dtProfile[0].PPKP, (dtProfile[0].IsWebsiteNull()) ? null : dtProfile[0].Website,
                             dtProfile[0].CMStatus, (dtProfile[0].IsAgreementNoNull()) ? null : dtProfile[0].AgreementNo, AgreementDate, dtProfile[0].ExchangeStatus, dtProfile[0].CertNo,
                             CertificateDate, dtProfile[0].SPATraderNo, SPATraderDate, dtProfile[0].SPABrokerNo,
                             SPABrokerDate, dtProfile[0].PALNLicense, AnniversaryDate, dtProfile[0].CompImageID,
                             dtProfile[0].AgreementType, userName, DateTime.Now, EndDate,
                             approvalDesc, null, null, dtProfile[0].InitialMarginMultiplier,
                             dtProfile[0].MinReqInitialMarginIDR, dtProfile[0].MinReqInitialMarginUSD, dtProfile[0].Address, dtProfile[0].Email, dtProfile[0].RegionID, dtProfile[0].PhoneNumber
                            , dtProfile[0].ContactPersonName, dtProfile[0].ContactPersonPhone, dtProfile[0].RegistrationDate, dtProfile[0].CMID);

                         // Update record status of CMProfile to 'A'
                         cmProfileTA.ApproveInsert("A", approvalDesc, null, null, userName, DateTime.Now, cmProfileID);
                     }

                     // Approve update of ClearingMember
                     else if (dtProfile[0].ActionFlag == "U")
                     {
                         approvedFlag = "Approved Update";

                         // Update the existing record of CMProfile with the newly updated record
                         cmProfileTA.ApproveUpdate(dtProfile[0].Code, "A", dtProfile[0].EffectiveStartDate,
                                                 dtProfile[0].Name, (dtProfile[0].IsPPKPNull()) ? null : dtProfile[0].PPKP,
                                                 (dtProfile[0].IsWebsiteNull()) ? null : dtProfile[0].Website,
                                                 dtProfile[0].CMStatus, (dtProfile[0].IsAgreementNoNull()) ? null : dtProfile[0].AgreementNo, AgreementDate
                                                  , dtProfile[0].ExchangeStatus, dtProfile[0].CertNo, CertificateDate
                                                  , dtProfile[0].SPATraderNo, SPATraderDate
                                                  , dtProfile[0].SPABrokerNo, SPABrokerDate
                                                  , dtProfile[0].PALNLicense, AnniversaryDate
                                                  , dtProfile[0].CompImageID, dtProfile[0].AgreementType, userName
                                                  , DateTime.Now, EndDate, approvalDesc, null, null, dtProfile[0].MinReqInitialMarginUSD, dtProfile[0].MinReqInitialMarginIDR,
                                                  dtProfile[0].InitialMarginMultiplier,dtProfile[0].Address, dtProfile[0].Email, dtProfile[0].PhoneNumber
                                                , dtProfile[0].ContactPersonName, dtProfile[0].ContactPersonPhone, dtProfile[0].RegistrationDate,dtProfile [0].RegionID, dtProfile[0].OriginalID);
                         // Delete the proposed record
                         cmProfileTA.DeleteByCMProfileID(cmProfileID);

                         // Update record in ClearingMember to reflect changes (to be used by other table)
                         cmTa.ApproveUpdate(dtProfile[0].Code, "A", dtProfile[0].EffectiveStartDate, dtProfile[0].Name, (dtProfile[0].IsPPKPNull()) ? null : dtProfile[0].PPKP, (dtProfile[0].IsWebsiteNull()) ? null : dtProfile[0].Website,
                             dtProfile[0].CMStatus, (dtProfile[0].IsAgreementNoNull()) ? null : dtProfile[0].AgreementNo, AgreementDate, dtProfile[0].ExchangeStatus, dtProfile[0].CertNo,
                             CertificateDate, dtProfile[0].SPATraderNo, SPATraderDate, dtProfile[0].SPABrokerNo,
                             SPABrokerDate, dtProfile[0].PALNLicense, AnniversaryDate, dtProfile[0].CompImageID,
                             dtProfile[0].AgreementType, userName, DateTime.Now, EndDate,
                             approvalDesc, null, null, dtProfile[0].InitialMarginMultiplier,
                             dtProfile[0].MinReqInitialMarginIDR, dtProfile[0].MinReqInitialMarginUSD, dtProfile[0].Address, dtProfile[0].Email, dtProfile[0].RegionID, dtProfile[0].PhoneNumber
                            , dtProfile[0].ContactPersonName, dtProfile[0].ContactPersonPhone, dtProfile[0].RegistrationDate, dtProfile[0].CMID);
                         
                     }

                     // Approve deleting record
                     else if (dtProfile[0].ActionFlag == "D")
                     {
                         approvedFlag = "Approved Delete";

                         // Delete from CMProfile
                         cmProfileTA.DeleteByCMProfileID(cmProfileID);
                         cmProfileTA.DeleteByCMProfileID(dtProfile[0].OriginalID);

                         // Delete from ClearingMember
                         cmTa.DeleteByCMID(dtProfile[0].CMID);
                     }

                    // Create audit trail message
                     string logMessage = string.Format(approvedFlag + ", Code:{0} | Name:{1} | PPKP:{2} |" +
                                               " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                               " AgreementDate {6} | ExchangeStatus:{7} | certNo:{8} |" +
                                               " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                               " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                               " CompanyAnniversary {15} | ImageID:{16} | AgreementType:{17} | ApprovalDesc:{18}",
                                               dtProfile[0].Code, dtProfile[0].Name, dtProfile[0].PPKP, dtProfile[0].Website, dtProfile[0].CMStatus,
                                               dtProfile[0].AgreementNo, AgreementDate.ToString(), dtProfile[0].ExchangeStatus,
                                               dtProfile[0].CertNo, CertificateDate.ToString(), dtProfile[0].SPATraderNo,
                                               SPATraderDate.ToString(), dtProfile[0].SPABrokerNo, SPABrokerDate.ToString(),
                                               dtProfile[0].PALNLicense, AnniversaryDate, dtProfile[0].CompImageID, dtProfile[0].AgreementType, approvalDesc);

                    switch (dtProfile[0].ActionFlag)
                     {
                         case "I": ActionFlagDesc = "Insert"; break;
                         case "U": ActionFlagDesc = "Update"; break;
                         case "D": ActionFlagDesc = "Delete"; break;
                     }

                    // Log action to audit trail
                    AuditTrail.AddAuditTrail("CMProfile", approvedFlag, logMessage, userName, "Approve " + ActionFlagDesc);

                    SMTPHelper.SendApprove(dtProfile[0].Email, approvalDesc);

                    // Commit transaction
                    scope.Complete();
                }
                else
                    throw new ApplicationException("Unable to get data from Clearing Member Profile");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                if (dtProfile[0].ActionFlag == "I")
                {
                    emTa.FillByCodeCMID(dtEM, null, "A", dtProfile[0].CMID);

                    //force insert Investor Record
                    if (dtCME[0].CMType == "B")
                    {
                        //account 36
                        iTa.Insert(dtProfile[0].Code + "3677", dtEM[0].EMID, "A", dtProfile[0].Name, "N", userName, DateTime.Now,
                            userName, DateTime.Now, null, null, null, "ALL", "", "R", "A", null, null);
                   
                        //account 05
                        iTa.Insert(dtProfile[0].Code + "0577", dtEM[0].EMID, "A", dtProfile[0].Name, "N", userName, DateTime.Now,
                            userName, DateTime.Now, null, null, null, "ALL", "", "R", "A", null, null);
                    }
                    else
                    {
                        //account 45
                        iTa.Insert(dtProfile[0].Code + "4577", dtEM[0].EMID, "A", dtProfile[0].Name, "N", userName, DateTime.Now,
                            userName, DateTime.Now, null, null, null, "ALL", "", "R", "A", null, null);
                    }

                    scope.Complete();
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                if (dtProfile[0].ActionFlag == "I")
                {
                    iTa.FillByCMCodeInvestor(dtInvestor, null, "A", dtProfile[0].Code);

                    for(int i=0; i<dtInvestor.Rows.Count; i++)
                    {
                        if (dtInvestor[i].Code.Contains("3677"))
                        {
                            if (dtCME[0].CMType == "B")
                            {
                                baTa.Insert("000" + i, 2, "RS", userName, DateTime.Now, userName,
                                DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");

                                //baTa.Insert("001" + i, 2, "RD", userName, DateTime.Now, userName,
                                //DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");

                                baTa.Insert("002" + i, 2, "RC", userName, DateTime.Now, userName,
                                DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");
                            }
                        }
                        else if (dtInvestor[i].Code.Contains("0577"))
                        {
                            if (dtCME[0].CMType == "B")
                            {
                                baTa.Insert("003" + i, 2, "RD", userName, DateTime.Now, userName,
                                DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");

                                baTa.Insert("004" + i, 2, "RS", userName, DateTime.Now, userName,
                                DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");
                            }
                        }
                        else if (dtInvestor[i].Code.Contains("4577"))
                        {
                            if (dtCME[0].CMType == "S")
                            {
                                baTa.Insert("000" + i, 2, "RS", userName, DateTime.Now, userName,
                                DateTime.Now, "A", DateTime.Now, null, approvalDesc, dtInvestor[i].InvestorID, dtProfile[0].CMID, 5, null, null, "R");
                            }
                        }
                    }

                    scope.Complete();
                }
            }
        }
	    catch (Exception ex)
	    {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist. Please input new record.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
	    }
    }

    // Reject the proposed record
    public static void Reject(decimal CMProfileID, string UserName, string ApprovalDesc)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.CMProfileTableAdapter cmProfileTa = new ClearingMemberDataTableAdapters.CMProfileTableAdapter();
        ClearingMemberDataTableAdapters.ImageTableAdapter imgTa = new ClearingMemberDataTableAdapters.ImageTableAdapter();

        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberData.CMProfileDataTable dtProfile = new ClearingMemberData.CMProfileDataTable();
        string ActionFlagDesc = "";
        string logMessage = "";

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                // Validate record if the same record is pending for approval by checker 
                ValidateProfileRecord(CMProfileID, "R");

                // Fill data table
                cmProfileTa.FillByCMProfileID(dtProfile, CMProfileID);

                if (dtProfile.Count > 0)
                {
                    // Get Clearing Member data
                    cmTa.FillByCMID(dt, dtProfile[0].CMID);

                    // Create audit trail message
                    switch (dtProfile[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Reject Insert"; break;
                        case "U": ActionFlagDesc = "Reject Update"; break;
                        case "D": ActionFlagDesc = "Reject Delete"; break;
                    }

                    logMessage = string.Format(ActionFlagDesc +", Code:{0} | Name:{1} | PPKP:{2} |" +
                                                     " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                                     " ExchangeStatus:{6} | certNo:{7} |" +
                                                     " SpaTraderNo:{8} | SpaBrokerNo:{9} | PALNLicense:{10} |" +
                                                     " ImageID:{11} | agreemenType{12}",
                                                     dtProfile[0].Code, dtProfile[0].Name, dtProfile[0].PPKP, dtProfile[0].Website, dtProfile[0].CMStatus,
                                                     dtProfile[0].AgreementNo, dtProfile[0].ExchangeStatus, dtProfile[0].CertNo, dtProfile[0].SPATraderNo,
                                                     dtProfile[0].SPABrokerNo, dtProfile[0].PALNLicense, dtProfile[0].CompImageID, dtProfile[0].AgreementType);

                   
                    // Delete from ClearingMember table if rejecting the proposed new record
                    if (dtProfile[0].ActionFlag == "I")
                    {
                        // Delete from ClearingMember table
                        //cmTa.DeleteByCMID(dt[0].CMID);

                        // Delete from Image table
                        //decimal imageID = Convert.ToDecimal(cmTa.getImageID(dt[0].CMID));
                        //imgTa.DeleteByImageID(imageID, "P");

                        //update status clearingmember
                        cmTa.RejectApprovalClearingMember(ApprovalDesc, dt[0].CMID);
                        cmProfileTa.RejectApprovalCMProfile(ApprovalDesc, dt[0].CMID);
                    }

                    // Delete from CMProfile table
                    //cmProfileTa.DeleteByCMProfileID(CMProfileID);
                    SMTPHelper.SendReject(dt[0].Email, ApprovalDesc);

                    // Log to audit trail
                    AuditTrail.AddAuditTrail("ClearingMember", ActionFlagDesc, logMessage, UserName, ActionFlagDesc);

                    // Commit transaction
                    scope.Complete();

                }
                else
                    throw new ApplicationException("Unable to get data from Clearing Member Profile");

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region "   Unused   "
    public static void Approve(List<string> approvedList, string userName, string approvalDesc)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string cmid in approvedList)
                {
                    cmTa.FillByCMID(dt, Convert.ToDecimal(cmid));
                    if (dt.Count > 0)
                    {
                        string approvedFlag = "";

                        if (dt[0].ActionFlag == "I")
                        {
                            approvedFlag = "Approved Insert";
                            cmTa.ApproveInsert("A", approvalDesc, null, null, userName, DateTime.Now, dt[0].CMID);
                        }
                        else if (dt[0].ActionFlag == "U")
                        {
                            cmTa.ApproveUpdate(dt[0].Code, "P", dt[0].EffectiveStartDate,
                                                     dt[0].Name, dt[0].PPKP, dt[0].Website, dt[0].CMStatus
                                                     , dt[0].AgreementNo, dt[0].AgreementDate
                                                     , dt[0].ExchangeStatus, dt[0].CertNo, dt[0].CertDate
                                                     , dt[0].SPATraderNo, dt[0].SPATraderDate
                                                     , dt[0].SPABrokerNo, dt[0].SPATraderDate
                                                     , dt[0].PALNLicense, dt[0].CompAnniversary
                                                     , dt[0].CompImageID, dt[0].AgreementType, userName
                                                     , DateTime.Now, null, "", null, null, dt[0].InitialMarginMultiplier, dt[0].MinReqInitialMarginIDR,
                                                     dt[0].MinReqInitialMarginUSD,dt[0].Address,dt[0].Email,dt[0].RegionID,
                                                     dt[0].PhoneNumber,dt[0].ContactPersonName,dt[0].ContactPersonPhone,
                                                     dt[0].RegistrationDate, dt[0].OriginalID);
                            approvedFlag = "Approved Update";
                        }
                        else if (dt[0].ActionFlag == "D")
                        {
                            approvedFlag = "Approved Delete";
                            cmTa.ApproveDelete("T", "A", dt[0].Code, dt[0].ApprovalStatus, dt[0].EffectiveStartDate);
                        }

                        string logMessage = string.Format(approvedFlag + ", Code:{0} | Name:{1} | PPKP:{2} |" +
                                                  " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                                  " AgreementDate {6} | ExchangeStatus:{7} | certNo:{8} |" +
                                                  " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                                  " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                                  " CompanyAnniversary {15} | ImageID:{16} | agreemenType{17}",
                                                  dt[0].Code, dt[0].Name, dt[0].PPKP, dt[0].Website, dt[0].CMStatus,
                                                  dt[0].AgreementNo, dt[0].AgreementDate.ToShortDateString(), dt[0].ExchangeStatus,
                                                  dt[0].CertNo, dt[0].CertDate.ToShortDateString(), dt[0].SPATraderNo,
                                                  dt[0].SPATraderDate.ToShortDateString(), dt[0].SPABrokerNo, dt[0].SPATraderDate.ToShortDateString(),
                                                  dt[0].PALNLicense, dt[0].CompAnniversary, dt[0].CompImageID, dt[0].AgreementType);
                        string ActionFlagDesc = "";
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                        AuditTrail.AddAuditTrail("ClearingMember", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);
                    }
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void Reject(List<string> rejectedList, string userName)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.ImageTableAdapter imgTa = new ClearingMemberDataTableAdapters.ImageTableAdapter();
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string cmid in rejectedList)
                {
                    //delete cm
                    //get image id
                    //delete image
                    string logMessage = "";
                    cmTa.FillByCMID(dt, Convert.ToDecimal(cmid));
                    string ActionFlagDesc = "";
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Rejected, Code:{0} | Name:{1} | PPKP:{2} |" +
                                                        " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                                        " AgreementDate {6} | ExchangeStatus:{7} | certNo:{8} |" +
                                                        " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                                        " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                                        " CompanyAnniversary {15} | ImageID:{16} | agreemenType{17}",
                                                        dt[0].Code, dt[0].Name, dt[0].PPKP, dt[0].Website, dt[0].CMStatus,
                                                        dt[0].AgreementNo, dt[0].AgreementDate.ToShortDateString(), dt[0].ExchangeStatus,
                                                        dt[0].CertNo, dt[0].CertDate.ToShortDateString(), dt[0].SPATraderNo,
                                                        dt[0].SPATraderDate.ToShortDateString(), dt[0].SPABrokerNo, dt[0].SPATraderDate.ToShortDateString(),
                                                        dt[0].PALNLicense, dt[0].CompAnniversary, dt[0].CompImageID, dt[0].AgreementType);
                       
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                    }
                    cmTa.DeleteByCMID(Convert.ToDecimal(cmid));
                    decimal imageID = Convert.ToDecimal(cmTa.getImageID(Convert.ToDecimal(cmid)));
                    imgTa.DeleteByImageID(imageID,"P");
                    AuditTrail.AddAuditTrail("ClearingMember", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ProposeDelete(List<string> deletedList, string userName)
    {
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter cmTa = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();
        ClearingMemberDataTableAdapters.ImageTableAdapter imgTa = new ClearingMemberDataTableAdapters.ImageTableAdapter();
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberData.ImageDataTable imgDt = new ClearingMemberData.ImageDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string cmid in deletedList)
                {
                    cmTa.FillByCMID(dt, Convert.ToDecimal(cmid));
                    if (dt.Count > 0)
                    {
                        cmTa.Insert(dt[0].Code, "P", dt[0].EffectiveStartDate, dt[0].Name, dt[0].PPKP, dt[0].Website, dt[0].CMStatus, dt[0].AgreementNo
                            , dt[0].AgreementDate, dt[0].ExchangeStatus, dt[0].CertNo, dt[0].CertDate, dt[0].SPATraderNo, dt[0].SPATraderDate, dt[0].SPABrokerNo, dt[0].SPATraderDate, dt[0].PALNLicense, dt[0].CompAnniversary, dt[0].CompImageID
                            , dt[0].AgreementType, userName, DateTime.Now, userName, DateTime.Now, null, null, Convert.ToDecimal(cmid), "D", dt[0].InitialMarginMultiplier
                            , dt[0].MinReqInitialMarginIDR, dt[0].MinReqInitialMarginUSD,dt[0].Address,dt[0].Email,dt[0].PhoneNumber, dt[0].ContactPersonName,dt[0].ContactPersonPhone ,dt[0].RegistrationDate,dt[0].CMAccountNo,dt[0].CMBankName
                            ,dt[0].CMAccountName, null, null, null, null, null, null, null, null, null, "", null
                            , null, null, null, null, null, null, null, null, null);

                        string logMessage = string.Format("Proposed Delete, Code:{0} | Name:{1} | PPKP:{2} |" +
                                                     " Website:{3} | CMStatus: {4} | AgreementNo:{5} |" +
                                                     " AgreementDate {6} | ExchangeStatus:{7} | certNo:{8} |" +
                                                     " CertDate:{9} | SpaTraderNo:{10} | SpaTraderDate:{11} |" +
                                                     " SpaBrokerNo:{12} | SpaBrokerDate:{13} | PALNLicense:{14} |" +
                                                     " CompanyAnniversary {15} | ImageID:{16} | agreemenType{17}",
                                                     dt[0].Code, dt[0].Name, dt[0].PPKP, dt[0].Website, dt[0].CMStatus,
                                                     dt[0].AgreementNo, dt[0].AgreementDate.ToShortDateString(), dt[0].ExchangeStatus,
                                                     dt[0].CertNo, dt[0].CertDate.ToShortDateString(), dt[0].SPATraderNo,
                                                     dt[0].SPATraderDate.ToShortDateString(), dt[0].SPABrokerNo, dt[0].SPATraderDate.ToShortDateString(),
                                                     dt[0].PALNLicense, dt[0].CompAnniversary, dt[0].CompImageID, dt[0].AgreementType);

                        AuditTrail.AddAuditTrail("ClearingMember", AuditTrail.PROPOSE, logMessage, userName, "Propose Delete");
                    }
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static ClearingMemberData.ClearingMemberRow getEmailBuyerWithExchange(string exchangeRef)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberData.ClearingMemberRow dr = null;
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillEmailBuyerByExchangeRef(dt, exchangeRef);
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

    public static ClearingMemberData.ClearingMemberRow getEmailSellerWithExchange(string exchangeRef)
    {
        ClearingMemberData.ClearingMemberDataTable dt = new ClearingMemberData.ClearingMemberDataTable();
        ClearingMemberData.ClearingMemberRow dr = null;
        ClearingMemberDataTableAdapters.ClearingMemberTableAdapter ta = new ClearingMemberDataTableAdapters.ClearingMemberTableAdapter();

        try
        {
            ta.FillEmailSellerByExchangeRef(dt, exchangeRef);
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
}
