using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
/// <summary>
/// Summary description for BankAccount
/// </summary>
public class TermPayment
{
    //get all data from TermPayment by StartPaymentDate

    public static TermPaymentData.TermPaymentDataTable SelectTermPayment(DateTime startPaymentDate)
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
        try
        {
            ta.FillByStartDate(dt, startPaymentDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load term payment data");
        }
    }

    public static TermPaymentData.TermPaymentDataTable SelectTermPaymentByDateAppStatus(DateTime startDate, string approvalStatus)
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
        try
        {
            ta.FillByDateNAppStatus(dt,startDate,approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load term payment data");
        }
    }

    //get all data from TermPayment 
    public static TermPaymentData.TermPaymentDataTable SelectAllTermPayment()
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load term payment data");
        }
    }
    //get all data from TermPayment by TermID
    public static TermPaymentData.TermPaymentDataTable SelectTermPaymentByID(decimal termId)
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
        try
        {
            ta.FillByTermId(dt, termId);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load term payment data");
        }
    }

    //validate Start payment Date
    public static  TermPaymentData.NewStartPaymentDateDataTable ValidStartPaymentDate()
    {
        TermPaymentData.NewStartPaymentDateDataTable dt = new TermPaymentData.NewStartPaymentDateDataTable();
        TermPaymentDataTableAdapters.NewStartPaymentDateTableAdapter ta = new  TermPaymentDataTableAdapters.NewStartPaymentDateTableAdapter();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load term payment data");
        }
    }

    public static TermPaymentData.TermPaymentRow GetTermPaymentByID(decimal termId)
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentData.TermPaymentRow dr = null;
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
        try
        {
            ta.FillByTermId(dt, termId);
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
    
    public static void ProposeTermofPayment(DateTime startDate, DateTime endDate, string notes, string approvalDesc,
                                          string action, string userName, decimal originalID)
    {
        
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();
         
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
                DateTime dueDate = endDate.AddDays(1);
                if ((dueDate.DayOfWeek == DayOfWeek.Saturday) || dueDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dueDate = endDate.AddDays(3);
                }
                ta.Insert("P", startDate, endDate, dueDate, notes, approvalDesc,action, userName, DateTime.Now, userName, DateTime.Now,originalID);

                logMessage = string.Format("Proposed Value: StartPaymentDate={0}|EndPaymentDate={1}|DueDate={2}|",
                                           Convert.ToString(startDate) , Convert.ToString(endDate),
                                           Convert.ToString(dueDate));
                AuditTrail.AddAuditTrail("TermPayment", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

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


    public static void ApproveTermPayment(decimal termID, string userName, string approveDesc)
    {
        
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();

        string logMessage = "";
        try
        {
            //ta.FillByBankAccountID(dt, bankAccountID);
            ta.FillByTermId(dt, termID);
                               
                
            using (TransactionScope scope = new TransactionScope())
            {
                if (dt.Rows.Count > 0)
                {
                    //Approve new records
                    if (dt[0].ActionFlag == "I")
                    {
                        //update approve status to "A"
                        ta.ApproveInsert("A", approveDesc, null, userName, DateTime.Now, termID);
                        // Create audit trail message
                        logMessage = string.Format("Approve Insert:  StartpaymentDate:{0} | EndPaymentDate:{1} | DueDate:{2} |",
                                                            dt[0].StartPaymentDate, dt[0].EndPaymentDate,
                                                            dt[0].DueDateKBIPKJ);
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApproveUpdate("A", dt[0].StartPaymentDate, dt[0].EndPaymentDate, dt[0].DueDateKBIPKJ, dt[0].Notes,
                                         approveDesc, null, userName, DateTime.Now, null, dt[0].OriginalID);
                        logMessage = string.Format("Approve Update:  StartpaymentDate:{0} | EndPaymentDate:{1} | DueDate:{2} |",
                                                            dt[0].StartPaymentDate, dt[0].EndPaymentDate,
                                                            dt[0].DueDateKBIPKJ);
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteTermPayment(dt[0].OriginalID);
                        ta.DeleteTermPayment(termID);
                        logMessage = string.Format("Approve Delete:  StartpaymentDate:{0} | EndPaymentDate:{1} | DueDate:{2} |",
                                                            dt[0].StartPaymentDate, dt[0].EndPaymentDate,
                                                            dt[0].DueDateKBIPKJ);
                    }


                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("TermofPayment", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);


            }

            scope.Complete();
            

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

   }
       
    public static void RejectProposedTermOfPaymentID(decimal termID, string userName)
    {
        TermPaymentData.TermPaymentDataTable dt = new TermPaymentData.TermPaymentDataTable();
        TermPaymentDataTableAdapters.TermPaymentTableAdapter ta = new TermPaymentDataTableAdapters.TermPaymentTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByTermId(dt, termID);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject:  StartPaymenDate={0}|EndPaymentDate={1}",
                                                               dt[0].StartPaymentDate,
                                                               dt[0].EndPaymentDate);
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.DeleteTermPayment(termID);

                AuditTrail.AddAuditTrail("TermOfPayment", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    


}
