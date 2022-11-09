using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for EventReceipientList
/// </summary>
public class EventReceipientList
{

    public static EventReceipientListData.EventRecipientListDataTable 
        SelectEventReceipientListByEventReceipientListID(EventReceipientListData.EventRecipientListDataTable dt, 
        decimal EventReceipientListID)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        try
        {
            ta.FillByEventRecipientListID(dt, EventReceipientListID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load event recipient list data by ID", ex);
        }

    }

    //get all data eventtype from EventReceipientList by eventrecipientName and name
    public static EventReceipientListData.EventRecipientListDataTable SelectEventReceipientListByEventReceipientListNameAndUserName(string eventReceipientName, string eventUserName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();

        try
        {
            ta.FillByEventRecipientNameAndUserName(dt, eventReceipientName, eventUserName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Event Recipient List data by Name and Username", ex);
        }

    }


    public static void AddEventReceipientList(string eventReceipientListName, string phoneNumber, 
                                              string emailAddress, Nullable<Guid> userId, string userName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(eventReceipientListName, phoneNumber, emailAddress, userId);

                logMessage = string.Format("Insert Value: {0}|{1}|{2}|{3}",
                                            eventReceipientListName, 
                                            phoneNumber, emailAddress, 
                                            userId);
                AuditTrail.AddAuditTrail("EventRecipientList", "Insert", logMessage, userName, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }


    public static void UpdateEventReceipientList(string eventReceipientListName, string phoneNumber, 
                                                string emailAddress, Nullable<Guid> userId, string userName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(phoneNumber, emailAddress, userId, eventReceipientListName);

                logMessage = string.Format("Update Value: {0}|{1}|{2}|{3}",
                                            eventReceipientListName,
                                            phoneNumber, emailAddress,
                                            userId);
                AuditTrail.AddAuditTrail("EventRecipientList", "Update", logMessage, userName,"Update");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static void DeleteEventReceipientList(decimal eventReceipientListID, string userName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByEventRecipientListID(dt, eventReceipientListID);

                if (dt.Count > 0)
                {
                    logMessage = string.Format("Delete Value: {0}|{1}|{2}|{3}",
                                                          dt[0].EventRecipientName,
                                                          dt[0].IsPhoneNumberNull() ? "" : dt[0].PhoneNumber,
                                                          dt[0].IsEmailAddressNull() ? "" : dt[0].EmailAddress,
                                                          dt[0].IsUserNameNull() ? "" : dt[0].UserName);
                }
                ta.DeleteEventReceipientName(eventReceipientListID);

                AuditTrail.AddAuditTrail("EventRecipientList", "Delete", logMessage, userName,"Delete");
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                exMessage = "This record can not be deleted due to referential integrity checking on table.";
            }
            else
            {
                exMessage = ex.Message;
            }
            throw new ApplicationException(exMessage);
        }
    }

    public static void DeleteEventReceipientListByList(List<string> deleteList, string userName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string item in deleteList)
                {
                    string logMessage = "";
                    ta.FillByEventRecipientListID(dt, Convert.ToDecimal(item));
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Delete Value: {0}|{1}|{2}|{3}",
                                                          dt[0].EventRecipientName,
                                                          dt[0].IsPhoneNumberNull() ? "" : dt[0].PhoneNumber,
                                                          dt[0].IsEmailAddressNull() ? "" : dt[0].EmailAddress,
                                                          dt[0].IsUserNameNull() ? "" : dt[0].UserName);
                    }

                    ta.DeleteEventReceipientName(Convert.ToDecimal(item));
                    AuditTrail.AddAuditTrail("EventRecipientList", "Deleted", logMessage, userName,"Delete");
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void isValidPhoneNumber(string phoneNumber)
    {
        try
        {
            // Check for phone number
            string strRegex = "^[0-9]*";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(phoneNumber))
            {
                return;
            }
            else
            {
                throw new ApplicationException("Phone Number is not valid.");
            }
        }
        catch
        {
            throw new ApplicationException("Phone Number is not valid.");
        }
    }

    public static void isValidEmail(string emailAddress)
    {
        try
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(emailAddress))
            {
                return;
            }
            else
            {
                throw new ApplicationException("Email is not valid.");
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Email is not valid.");
        }
        
         
    }

    public static decimal GetRecipientListID(string UserName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        decimal? RecipientListID;

        try
        {
            RecipientListID = Convert.ToDecimal(ta.GetEventRecipientListIDByUserName(UserName));
            return RecipientListID.Value;
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to get Recipient List ID by Username", ex);
        }

    }
}
