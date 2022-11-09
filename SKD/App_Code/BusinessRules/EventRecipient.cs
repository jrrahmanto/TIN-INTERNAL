using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;


/// <summary>
/// Summary description for EventRecipient
/// </summary>
public class EventRecipient
{

    //get all data eventrecipient from dataset to datagrid

    public static EventRecipientData.EventTypeRecipientDataTable SelectEventRecipientByEventTypeID(EventRecipientData.EventTypeRecipientDataTable dt, decimal eventTypeID)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();
        try
        {
            ta.FillByEventTypeID(dt, eventTypeID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load event recipient data");
        }

    }


    public static EventRecipientData.EventTypeRecipientDataTable SelectEventRecipientByEventTypeIDAndEventRecipientListID(EventRecipientData.EventTypeRecipientDataTable dt, decimal eventTypeID, decimal eventRecipientListID)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();
        try
        {
            ta.FillByEventTypeIDAndEventRecipientListID(dt, eventTypeID, eventRecipientListID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load event recipient data");
        }

    }


    public static EventRecipientData.EventTypeRecipientDataTable SelectEventRecipientByEventTypeCodeAndEventRecipientName(string eventTypeCode, string eventRecipientName)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();
        EventRecipientData.EventTypeRecipientDataTable dt = new EventRecipientData.EventTypeRecipientDataTable();

        try
        {
            ta.FillByEventTypeCodeAndEventRecipientName(dt, eventTypeCode, eventRecipientName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load event recipient data");
        }
    }

    public static void AddRecipient(decimal eventTypeID, List<string> eventRecipientListID, string username)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();
        
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
            string logMessage = "";
            {
              
               foreach (string item in eventRecipientListID)
                    {
                       
                       
                            ta.Insert(eventTypeID, Convert.ToDecimal(item));

                            logMessage = string.Format("Insert Value: {0}|{1}",
                                                        eventTypeID,
                                                        eventRecipientListID);
                        
                    }
                    AuditTrail.AddAuditTrail("EventRecipient", "Insert", logMessage, username,"Insert");
                }
            
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

    public static void AddSingleRecipient(Nullable<decimal> eventTypeID, Nullable<decimal> eventRecipientListID, string username)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                {
                    ta.Insert(eventTypeID.Value, eventRecipientListID.Value);
                    logMessage = string.Format("Insert Value: {0}|{1}",
                                                eventTypeID,
                                                eventRecipientListID.ToString());
                    AuditTrail.AddAuditTrail("EventRecipient", "Insert", logMessage, username, "Insert");
                }

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

    public static void UpdateRecipient(decimal eventTypeID, decimal eventRecipientListID, string username)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(eventTypeID, eventRecipientListID);

                logMessage = string.Format("Update Value: {0}|{1}",
                                            eventTypeID,
                                            eventRecipientListID);
                AuditTrail.AddAuditTrail("EventRecipient", "Update", logMessage, username, "Update");

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

    public static void DeleteRecipient(decimal eventTypeID, List<decimal> eventRecipientListID, string userName)
    {
        EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeRecipientTableAdapter();
        EventRecipientData.EventTypeRecipientDataTable dt = new EventRecipientData.EventTypeRecipientDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                //ta.FillByEventTypeIDAndEventRecipientListID(dt, eventTypeID, eventRecipientListID);

           
                foreach (decimal item in eventRecipientListID)
                {
                   
                    ta.DeleteEventTypeIDAndRecipientListID(eventTypeID, Convert.ToDecimal(item));
                   
                    logMessage = string.Format("Delete Value: {0}|{1}", eventTypeID, item);
                   
                }

                AuditTrail.AddAuditTrail("EventRecipient", "Delete", logMessage, userName,"Delete");
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static Nullable<decimal> GetEventRecipientListID(string eventRecipientListName)
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        return ta.GetIDByName(eventRecipientListName);
    }
    public static Nullable<decimal> GetEventTypeID(string eventTypeCode)
    {
        EventRecipientDataTableAdapters.EventTypeTableAdapter ta = new EventRecipientDataTableAdapters.EventTypeTableAdapter();

        return ta.GetIDByEventTypeCode(eventTypeCode);
    }
    #region supporting method

    public static EventTypeData.EventTypeDataTable getEventTypeCode()
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();
        EventTypeData.EventTypeDataTable dt = new EventTypeData.EventTypeDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load event type code data");
        }
    }

    public static EventReceipientListData.EventRecipientListDataTable getEventRecipientListName()
    {
        EventReceipientListDataTableAdapters.EventRecipientListTableAdapter ta = new EventReceipientListDataTableAdapters.EventRecipientListTableAdapter();
        EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load event recipient name data");
        }
    }

    #endregion

}
