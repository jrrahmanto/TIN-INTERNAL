using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for BOD
/// </summary>
public class BOD
{
    public static BODData.BODDataTable Fill(decimal CMID, string name, string boardPosition, string approvalStatus)
    {
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
        BODData.BODDataTable dt = new BODData.BODDataTable();
        try
        {
            ta.FillBySearchCriteria(dt, CMID, name, boardPosition,approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
    }

    public static BODData.BODDdlDataTable FillDdl()
    {
        BODDataTableAdapters.BODDdlTableAdapter ta = new BODDataTableAdapters.BODDdlTableAdapter(); 

        try
        {
            return ta.GetData();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public static void ValidateRecord(decimal EditedBODID, string ApprovalStatus)
    {
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
        BODData.BODDataTable dt = new BODData.BODDataTable();

        ta.FillByBODID(dt, EditedBODID);

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

    public static void ProposeBOD(string BODNo, string name, DateTime DOB, string IDNO,
                                      string education, string WorkExperience, string boardPosition,
                                      string mobilePhoneNo, string PhoneNo, string Email, string certNO,
                                      DateTime CertDate, bool SignatureAuthor, DateTime startDate, Nullable<DateTime> endDate,
                                      decimal CMID, string address, string province, string city, string POB,
                                      string postalCode, byte[] photo, byte[] signature,string userName, 
                                      string ActionFlag, Nullable<decimal> OriginalBODID, bool isSignatureUpdated,
                                      bool isPhotoUpdated)
    {
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
        ClearingMemberDataTableAdapters.ImageTableAdapter imgTa = new ClearingMemberDataTableAdapters.ImageTableAdapter();
        string ActionFlagDesc = "";

        if (OriginalBODID.HasValue)
        {
            // Validate for propose/approve/reject record
            ValidateRecord(Convert.ToDecimal(OriginalBODID), "P");
        }
            
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Nullable<decimal> photoID = null;
                Nullable<decimal> signatureID = null;
                BODData.BODDataTable boddt = new BODData.BODDataTable();
               
                if (isPhotoUpdated)
                {
                    imgTa.Insert("P", photo, userName, DateTime.Now, userName, DateTime.Now, null);
                    photoID = Convert.ToDecimal(imgTa.getMaxImageID());
                }
                else
                {
                    if (ActionFlag == "U")
                    {
                        ta.FillByBODID(boddt, OriginalBODID.Value);
                        if (!boddt[0].IsSignatureImageIDNull())
                        {
                            signatureID = boddt[0].SignatureImageID;
                            signature = boddt[0].SignatureImage;
                        }
                    }
                }
                
                if (isSignatureUpdated)
                {
                    imgTa.Insert("P", signature, userName, DateTime.Now, userName, DateTime.Now, null);
                    signatureID = Convert.ToDecimal(imgTa.getMaxImageID());
                }
                else
                {
                    if (ActionFlag == "U")
                    {
                        boddt.Clear();
                        boddt.AcceptChanges();

                        ta.FillByBODID(boddt, OriginalBODID.Value);
                        if (!boddt[0].IsPhotoImageIDNull())
                        {
                            photoID = boddt[0].PhotoImageID;
                            photo = boddt[0].PhotoImage;
                        }
                    }
                }

                switch (ActionFlag) 
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }

                string logMessage = string.Format(ActionFlagDesc + " , BODNo:{0} | Name:{1} | DOB:{2} |" +
                                               " IDNO:{3} | Education: {4} | WorkExp:{5} | BoardPodition:{6} |" +
                                               " MobilePhoneNo:{7} | PhoneNo:{8} | Email:{9} | EffStartDate:{10} |" +
                                               " CertNo:{11} | CertDate:{12} | SignAuthor:{13} | CMID:{14} |" +
                                               " Address:{15} | Province:{16} | City:{17} | PostalCode:{18} |" +
                                               " SignImageID:{19} | POB:{20} | PhotoImageID:{21}",
                                               BODNo, name,DOB.ToShortDateString(),IDNO,education,WorkExperience,boardPosition,
                                               mobilePhoneNo,PhoneNo,Email,startDate.ToShortDateString(), certNO,CertDate.ToShortDateString(),"", CMID.ToString(),
                                               address,province, city,postalCode,signatureID.ToString(),
                                               POB, photoID.ToString());

                ta.Insert("P", name, DOB, IDNO, education, WorkExperience, boardPosition,
                          mobilePhoneNo, PhoneNo, Email, userName, DateTime.Now, userName,
                          DateTime.Now, startDate, certNO, CertDate, SignatureAuthor, endDate,
                          CMID, address, province, city, postalCode, null, signatureID, POB, photoID,
                          ActionFlag, OriginalBODID, BODNo);

                AuditTrail.AddAuditTrail("BOD", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

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

    public static void Approve(decimal BODID, string UserName, string ApprovalDesc)
    {
        try
        {
            BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
            BODData.BODDataTable dt = new BODData.BODDataTable();
            string actionFlag = "";
            string approveFlag = "";

            ValidateRecord(BODID, "A");

            using (TransactionScope scope = new TransactionScope())
            {
                ta.FillByBODID(dt, BODID);
                if (dt.Count > 0)
                {
                    // TODO: guard for conflict effectiveness date
                    //decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevStartDate(dt[0].EffectiveStartDate,
                    //                                          dt[0].BODNo, dt[0].OriginalBODID));

                    //DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextDate(dt[0].BODNo,
                    //                          dt[0].EffectiveStartDate, dt[0].BODID));

                        // Update end date of previous record
                        //if (prevOriginalID != 0)
                        //{
                        //    ta.UpdateEndDateByBODID(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                        //}

                        //// Update end date of current record
                        //if (nextStartDate > DateTime.MinValue)
                        //{
                        //    dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                        //}

                        actionFlag = dt[0].ActionFlag;

                        if (actionFlag == "I")
                        {
                            approveFlag = "Approve Insert";

                            ta.ApproveInsert("A", ApprovalDesc, UserName, DateTime.Now, dt[0].EffectiveStartDate, null, null, dt[0].BODID);
                        }
                        else if (actionFlag == "U")
                        {
                            approveFlag = "Approve Update";

                            //ta.DeleteBOD(dt[0].OriginalBOD, dt[0].OriginalAppStatus);
                            Nullable<DateTime> EndDate = null;
                            Nullable<decimal> SignatureImageID = null;
                            Nullable<decimal> PhotoImageID = null;

                            if (!dt[0].IsEffectiveEndDateNull())
                            {
                                EndDate = dt[0].EffectiveEndDate;
                            }
                            if (!dt[0].IsSignatureImageIDNull())
                            {
                                SignatureImageID = dt[0].SignatureImageID;
                            }
                            if (!dt[0].IsPhotoImageIDNull())
                            {
                                PhotoImageID = dt[0].PhotoImageID;
                            }

                            // Update the targeted record
                            ta.ApproveUpdate(dt[0].BODNo, "A", dt[0].Name, dt[0].IDNO,
                                dt[0].IsEducationNull() ? null : dt[0].Education, dt[0].IsWorkExpNull() ? null : dt[0].WorkExp, dt[0].BoardPosition,
                                dt[0].IsMobilePhoneNoNull() ? null : dt[0].MobilePhoneNo, dt[0].IsPhoneNoNull() ? null : dt[0].PhoneNo,
                                dt[0].IsEmailNull() ? null : dt[0].Email, UserName, DateTime.Now, dt[0].EffectiveStartDate, EndDate,
                                dt[0].CMID, dt[0].IsAddressNull() ? null : dt[0].Address, dt[0].IsProvinceNull() ? null : dt[0].Province,
                                dt[0].IsCityNull() ? null : dt[0].City, dt[0].IsPostCodeNull() ? null : dt[0].PostCode, ApprovalDesc,
                                SignatureImageID, dt[0].IsPOBNull() ? null : dt[0].POB, PhotoImageID, null,
                                null, dt[0].OriginalBODID);

                            // Delete proposed item
                            ta.DeleteBOD(dt[0].BODID);
                        }
                        else if (actionFlag == "D")
                        {
                            approveFlag = "Approve Delete";
                            ta.DeleteBOD(dt[0].BODID);
                            ta.DeleteBOD(dt[0].OriginalBODID);
                        }
                        string address = "";
                        string IDNO = "";
                        string mobilePhone = "";
                        string phoneNo = "";
                        string email = "";
                        string education = "";
                        string workExp = "";
                        string signCheck = "";
                        string certNo = "";
                        string photoID = "";
                        string signID = "";
                        string POB = "";
                        string province = "";
                        string city = "";
                        string postalCode = "";
                        if (!dt[0].IsAddressNull())
                        {
                            address = dt[0].Address;
                        }
                        if (!dt[0].IsIDNONull())
                        {
                            IDNO = dt[0].IDNO;
                        }
                        if (!dt[0].IsMobilePhoneNoNull())
                        {
                            mobilePhone = dt[0].MobilePhoneNo;
                        }
                        if (!dt[0].IsPhoneNoNull())
                        {
                            phoneNo = dt[0].PhoneNo;
                        }
                        if (!dt[0].IsEmailNull())
                        {
                            email = dt[0].Email;
                        }
                        if (!dt[0].IsEducationNull())
                        {
                            education = dt[0].Education;
                        }
                        if (!dt[0].IsWorkExpNull())
                        {
                            workExp = dt[0].WorkExp;
                        }
                        if (!dt[0].IsSignatureAuthorNull())
                        {
                            if (dt[0].SignatureAuthor)
                            {
                                signCheck = "True";
                            }
                            else
                            {
                                signCheck = "False";
                            }
                        }
                        if (!dt[0].IsCertNoNull())
                        {
                            certNo = dt[0].CertNo;
                        }

                        if (!dt[0].IsPhotoImageIDNull())
                        {
                            photoID = dt[0].PhotoImageID.ToString();
                        }
                        if (!dt[0].IsSignatureImageIDNull())
                        {
                            signID = dt[0].SignatureImageID.ToString();
                        }
                        if (!dt[0].IsPOBNull())
                        {
                            POB = dt[0].POB;
                        }
                        if (!dt[0].IsProvinceNull())
                        {
                            province = dt[0].Province;
                        }
                        if (!dt[0].IsCityNull())
                        {
                            city = dt[0].City;
                        }
                        if (!dt[0].IsPostCodeNull())
                        {
                            postalCode = dt[0].PostCode;
                        }

                        string logMessage = string.Format(approveFlag + " , BODNo:{0} | Name:{1} | DOB:{2} |" +
                                                       " IDNO:{3} | Education: {4} | WorkExp:{5} | BoardPodition:{6} |" +
                                                       " MobilePhoneNo:{7} | PhoneNo:{8} | Email:{9} | EffStartDate:{10} |" +
                                                       " CertNo:{11} | CertDate:{12} | SignAuthor:{13} | CMID:{14} |" +
                                                       " Address:{15} | Province:{16} | City:{17} | PostalCode:{18} |" +
                                                       " SignImageID:{19} | POB:{20} | PhotoImageID:{21} | ApprovalDesc:{22}",
                                                       dt[0].BODNo, dt[0].Name, dt[0].DOB.ToShortDateString(),
                                                       IDNO, education, workExp, dt[0].BoardPosition,
                                                       mobilePhone, phoneNo, email, dt[0].EffectiveStartDate.ToShortDateString(),
                                                       certNo, dt[0].CertDate.ToShortDateString(),
                                                       signCheck, dt[0].CMID.ToString(),
                                                       address, province, city,
                                                       postalCode, signID,
                                                       POB, photoID, ApprovalDesc);
                        AuditTrail.AddAuditTrail("BOD", approveFlag, logMessage, UserName, approveFlag);
                    
                    }
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

    public static void Reject(decimal BODID, string UserName, string ApprovalDesc)
    {
        BODDataTableAdapters.BODTableAdapter ta = new BODDataTableAdapters.BODTableAdapter();
        BODData.BODDataTable dt = new BODData.BODDataTable();
        string proposedFlag = "";

        ValidateRecord(BODID, "R");

        ta.FillByBODID(dt, BODID);
        if (dt.Count > 0)
        {
            try
            {
                string rejectFlag = "";
                using (TransactionScope scope = new TransactionScope())
                {
                    switch (dt[0].ActionFlag)
                    {
                        case "I": proposedFlag = "Reject Insert"; break;
                        case "U": proposedFlag = "Reject Update"; break;
                        case "D": proposedFlag = "Reject Delete"; break;
                    }                    

                    string address = "";
                    string IDNO = "";
                    string mobilePhone = "";
                    string phoneNo = "";
                    string email = "";
                    string education = "";
                    string workExp = "";
                    string signCheck = "";
                    string certNo = "";
                    string photoID = "";
                    string signID = "";
                    string POB = "";
                    string province = "";
                    string city = "";
                    string postalCode = "";
                    if (!dt[0].IsAddressNull())
                    {
                        address = dt[0].Address;
                    }
                    if (!dt[0].IsIDNONull())
                    {
                        IDNO = dt[0].IDNO;
                    }
                    if (!dt[0].IsMobilePhoneNoNull())
                    {
                        mobilePhone = dt[0].MobilePhoneNo;
                    }
                    if (!dt[0].IsPhoneNoNull())
                    {
                        phoneNo = dt[0].PhoneNo;
                    }
                    if (!dt[0].IsEmailNull())
                    {
                        email = dt[0].Email;
                    }
                    if (!dt[0].IsEducationNull())
                    {
                        education = dt[0].Education;
                    }
                    if (!dt[0].IsWorkExpNull())
                    {
                        workExp = dt[0].WorkExp;
                    }
                    if (!dt[0].IsSignatureAuthorNull())
                    {
                        if (dt[0].SignatureAuthor)
                        {
                            signCheck = "True";
                        }
                        else
                        {
                            signCheck = "False";
                        }
                    }
                    if (!dt[0].IsCertNoNull())
                    {
                        certNo = dt[0].CertNo;
                    }

                    if (!dt[0].IsPhotoImageIDNull())
                    {
                        photoID = dt[0].PhotoImageID.ToString();
                    }
                    if (!dt[0].IsSignatureImageIDNull())
                    {
                        signID = dt[0].SignatureImageID.ToString();
                    }
                    if (!dt[0].IsPOBNull())
                    {
                        POB = dt[0].POB;
                    }
                    if (!dt[0].IsProvinceNull())
                    {
                        province = dt[0].Province;
                    }
                    if (!dt[0].IsCityNull())
                    {
                        city = dt[0].City;
                    }
                    if (!dt[0].IsPostCodeNull())
                    {
                        postalCode = dt[0].PostCode;
                    }
                    string logMessage = string.Format(rejectFlag + " , BODNo:{0} | Name:{1} | DOB:{2} |" +
                                                   " IDNO:{3} | Education: {4} | WorkExp:{5} | BoardPodition:{6} |" +
                                                   " MobilePhoneNo:{7} | PhoneNo:{8} | Email:{9} | EffStartDate:{10} |" +
                                                   " CertNo:{11} | CertDate:{12} | SignAuthor:{13} | CMID:{14} |" +
                                                   " Address:{15} | Province:{16} | City:{17} | PostalCode:{18} |" +
                                                   " SignImageID:{19} | POB:{20} | PhotoImageID:{21}",
                                                    dt[0].BODNo, dt[0].Name, dt[0].DOB.ToShortDateString(),
                                                   IDNO, education, workExp, dt[0].BoardPosition,
                                                   mobilePhone, phoneNo, email, dt[0].EffectiveStartDate.ToShortDateString(),
                                                   certNo, dt[0].CertDate.ToShortDateString(),
                                                   signCheck, dt[0].CMID.ToString(),
                                                   address, province, city,
                                                   postalCode, signID,
                                                   POB, photoID);

                    ta.DeleteBOD(dt[0].BODID);
                    AuditTrail.AddAuditTrail("BOD", AuditTrail.REJECT, logMessage, UserName,rejectFlag);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

