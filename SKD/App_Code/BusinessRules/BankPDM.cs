using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;



/// <summary>
/// Summary description for BankPDM
/// </summary>
public class BankPDM
{

    //get all data from bank pdm by code and status
    public static BankData.BankDataTable SelectBankPDMByCode(string bankCode)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankDataTable dt = new BankData.BankDataTable();

        try
        {
            ta.FillByCode(dt, bankCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank pdm data");
        }

    }

    //get all data bank pdm from dataset to datagrid
    public static BankData.BankRow SelectBankPDMByBankID(decimal bankID)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankDataTable dt = new BankData.BankDataTable();
        BankData.BankRow dr = null;
        try
        {
            ta.FillByBankIdOnly(dt, bankID);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank PDM data");
        }

    }

    //get all data from bank pdm by code and status
    public static BankData.BankDataTable SelectBankByCodeAndStatus(string bankCode, string approvalStatus)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankDataTable dt = new BankData.BankDataTable();

        try
        {
            ta.FillByBankCodeAndStatus(dt, bankCode, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bank pdm data");
        }

    }


    public static void ProposeBankPDM(string bankCode, string biCode, string name,
                                      string branch, string division, string address,
                                      string province, string postCode, string country,
                                      string phoneNo, string fax, string contact1, string approvalDescription,
                                      string contact2, string city, string stamp, string description, string action,
                                      string userName, decimal originalID)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(bankCode, "P", biCode, name, branch, division, address, province, postCode,
                      country, phoneNo, fax, contact1, contact2, description, "", DateTime.Now,
                      "", DateTime.Now, city, stamp, approvalDescription, originalID, action);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}",
                                            bankCode, biCode, name, branch, division, address,
                                            province, postCode, country, phoneNo, fax, contact1,
                                            contact2, description, city, stamp, approvalDescription);
                AuditTrail.AddAuditTrail("BankPDM", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);

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


    public static void ApproveBankPDM(decimal bankId, string userName)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankDataTable dt = new BankData.BankDataTable();

        try
        {
           try
            {
                ta.FillByBankIdOnly(dt,  bankId);

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApproveProposedItem(dt[0].Code, "A", dt[0].BICode, dt[0].Name, dt[0].Branch,
                                               dt[0].Division, dt[0].Address, dt[0].Province, dt[0].PostCode,
                                               dt[0].Country, dt[0].PhoneNo, dt[0].Fax, dt[0].Contact1,
                                               dt[0].Contact2, dt[0].Description, userName, 
                                               DateTime.Now, dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc, null, null, dt[0].BankID);
                        logMessage = string.Format("Approved Insert: {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}",
                                                    dt[0].Code, dt[0].BICode, dt[0].Name, dt[0].Branch,
                                                    dt[0].Division, dt[0].Address, dt[0].Province,
                                                    dt[0].PostCode, dt[0].Country, dt[0].PhoneNo,
                                                    dt[0].Fax, dt[0].Contact1, dt[0].Contact2, dt[0].Description, 
                                                    dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].Code, "A", dt[0].BICode, dt[0].Name, dt[0].Branch,
                                                      dt[0].Division, dt[0].Address, dt[0].Province, dt[0].PostCode,
                                                      dt[0].Country, dt[0].PhoneNo, dt[0].Fax, dt[0].Contact1,
                                                      dt[0].Contact2, dt[0].Description, userName,
                                                      DateTime.Now, dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc, null, dt[0].OriginalID);
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].BankID);
                        logMessage = string.Format("Approved Update: {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}",
                                                    dt[0].Code, dt[0].BICode, dt[0].Name, dt[0].Branch,
                                                    dt[0].Division, dt[0].Address, dt[0].Province,
                                                    dt[0].PostCode, dt[0].Country, dt[0].PhoneNo,
                                                    dt[0].Fax, dt[0].Contact1, dt[0].Contact2, dt[0].Description,
                                                    dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].BankID);
                        logMessage = string.Format("Approved Delete: {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}",
                                                    dt[0].Code, dt[0].BICode, dt[0].Name, dt[0].Branch,
                                                    dt[0].Division, dt[0].Address, dt[0].Province,
                                                    dt[0].PostCode, dt[0].Country, dt[0].PhoneNo,
                                                    dt[0].Fax, dt[0].Contact1, dt[0].Contact2, dt[0].Description,
                                                    dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("BankPDM", AuditTrail.APPROVE, logMessage, userName, ActionFlagDesc);

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

    public static void RejectProposedBankPDM(decimal bankID, string userName)
    {
        BankDataTableAdapters.BankTableAdapter ta = new BankDataTableAdapters.BankTableAdapter();
        BankData.BankDataTable dt = new BankData.BankDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByBankIdOnly(dt, bankID);
                string ActionFlagDesc = "";
                
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject : {0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}",
                                               dt[0].Code, dt[0].BICode, dt[0].Name, dt[0].Branch,
                                                    dt[0].Division, dt[0].Address, dt[0].Province,
                                                    dt[0].PostCode, dt[0].Country, dt[0].PhoneNo,
                                                    dt[0].Fax, dt[0].Contact1, dt[0].Contact2, dt[0].Description,
                                                    dt[0].City, dt[0].Stamp, dt[0].ApprovalDesc);
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }
                ta.DeleteRejectItem(bankID);

                AuditTrail.AddAuditTrail("BankPDM", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

}
