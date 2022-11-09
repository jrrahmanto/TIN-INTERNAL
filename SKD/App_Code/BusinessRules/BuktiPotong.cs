using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for BuktiPotong
/// </summary>
public class BuktiPotong
{
    public static BuktiPotongData.WithholdingTaxDataTable SelectBuktiPotongByCriteria(Nullable<decimal> CMID, string period, string approvalStatus)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();
        BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();

        try
        {
            string yearMonth = null;
            int month = 0;
            int year = 0;
            if (period == null)
            {
                yearMonth = period;
            }
            else
            {
                if (period.Length == 6)
                {
                    year = int.Parse(period.Substring(0, 4));
                    month = int.Parse(period.Substring(4, 2));
                    yearMonth = string.Format("{0:0000}{1:00}", year, month);
                }
                else if (period.Length == 5)
                {
                    year = int.Parse(period.Substring(0, 4));
                    month = int.Parse(period.Substring(4, 1));
                    yearMonth = string.Format("{0:0000}{1:00}", year, month);
                }
                else if (period.Length == 4)
                {
                    yearMonth = period;
                }
                else if (period.Length == 1)
                {
                    month = int.Parse(period);
                    yearMonth = string.Format("{0:00}", month);
                }
                else if (period.Length == 2)
                {
                    month = int.Parse(period);
                    yearMonth = string.Format("{0:00}", month);
                }
            }
           
           
            ta.FillByCriteria(dt, CMID, yearMonth, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bukti potong data");
        }

    }

    public static BuktiPotongData.WithholdingTaxRow SelectBuktiPotongByTaxID(decimal taxID)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();
        BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();
        BuktiPotongData.WithholdingTaxRow dr = null;
        try
        {
            ta.FillByTaxID(dt, taxID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bukti potong data");
        }

    }

    public static BuktiPotongData.WithholdingTaxDataTable SelectBuktiPotongByPK(decimal CMID, DateTime businessDate)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();
        BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();

        try
        {
            ta.FillByPK(dt, businessDate, CMID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load bukti potong data");
        }

    }


    public static void ProposeBuktiPotong(DateTime businessDate, decimal cmID, string taxNo,
                                         decimal tax, string period,
                                         string approvalDesc, string action, string userName, decimal OriginalID)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(businessDate, cmID, "P", taxNo, tax, period, userName,
                          DateTime.Now, userName, DateTime.Now, approvalDesc, action, OriginalID);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: business date={0}|clearing member={1}|tax number={2}|tax={3}" +
                                           "|period={4}",
                                        businessDate.ToString("dd-MMM-yyyy"), cmID, taxNo, tax,
                                       period);
                AuditTrail.AddAuditTrail("Bukti Potong", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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


    public static void ApproveBuktiPotong(decimal taxID, string userName)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();
        BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();

        try
        {
            try
            {
                ta.FillByTaxID(dt, taxID);

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {

                        ta.ApprovedProposedItem(dt[0].BusinessDate, dt[0].CMID, dt[0].TaxNo,
                                               dt[0].Tax, dt[0].Period, userName, DateTime.Now,
                                               "A",  dt[0].ApprovalDesc,null,null, dt[0].TaxID);

                        logMessage = string.Format("Approved Insert: business date={0}|clearing member={1}|tax number={2}|tax={3}" +
                                                    "|period={4}",
                                                     dt[0].BusinessDate,
                                                     dt[0].CMCode,
                                                     dt[0].TaxNo,
                                                     dt[0].Tax,
                                                     dt[0].Period);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].BusinessDate, dt[0].CMID, dt[0].TaxNo,
                                                    dt[0].Tax, dt[0].Period, userName, DateTime.Now,
                                                    "A", dt[0].ApprovalDesc, null, dt[0].OriginalID);

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].TaxID);
                        logMessage = string.Format("Approved Update: business date={0}|clearing member={1}|tax number={2}|tax={3}" +
                                                    "|period={4}",
                                                     dt[0].BusinessDate,
                                                     dt[0].CMCode,
                                                     dt[0].TaxNo,
                                                     dt[0].Tax,
                                                     dt[0].Period);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].TaxID);
                        logMessage = string.Format("Approved Delete: business date={0}|clearing member={1}|tax number={2}|tax={3}" +
                                                    "|period={4}",
                                                     dt[0].BusinessDate,
                                                     dt[0].CMCode,
                                                     dt[0].TaxNo,
                                                     dt[0].Tax,
                                                     dt[0].Period);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Bukti Potong", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);

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

    public static void RejectProposedBuktiPotong(decimal taxID, string userName)
    {
        BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter ta = new BuktiPotongDataTableAdapters.WithholdingTaxTableAdapter();
        BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByTaxID(dt, taxID);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {

                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    logMessage = string.Format("Reject : business date={0}|clearing member={1}|tax number={2}|tax={3}" +
                                                    "|period={4}",
                                                     dt[0].BusinessDate,
                                                     dt[0].CMCode,
                                                     dt[0].TaxNo,
                                                     dt[0].Tax,
                                                     dt[0].Period);
                }
                ta.DeleteRejectItem(taxID);

                AuditTrail.AddAuditTrail("Bukti Potong", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}
