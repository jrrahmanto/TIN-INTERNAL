using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for EventLog
/// </summary>
public class EventLog
{
    //get all data eventtype from dataset to datagrid
    public static EventLogData.EventLogDataTable SelectEventTypeByEventTypeID(EventLogData.EventLogDataTable dt, decimal eventTypeID, decimal eventRecipientID, DateTime eventTime)
    {
        EventLogDataTableAdapters.EventLogTableAdapter ta = new EventLogDataTableAdapters.EventLogTableAdapter();
        try
        {
            ta.FillByEventTypeIDEventTimeAndRecipientName(dt, eventTypeID, eventRecipientID, eventTime);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Event Log data by EventTypeID", ex);
        }

    }

    public static EventLogData.EventLogDataTable SearchInbox(decimal eventRecipientID, string receivedDate)
    {
        EventLogDataTableAdapters.EventLogTableAdapter ta = new EventLogDataTableAdapters.EventLogTableAdapter();
        try
        {
            Nullable<DateTime> recDate = null;
            if (receivedDate != null)
            {
                recDate = DateTime.Parse(receivedDate);
                //EventLogData.EventLogDataTable dt = new EventLogData.EventLogDataTable();
                //ta.FillInboxLogByRecipientListIDAndDate(dt, eventRecipientID, recDate.Value);
                return ta.GetInboxLogDataByRecipientListIDAndDate(eventRecipientID, recDate.Value);
            }
            else
            {
                return ta.GetInboxLogDataByRecipientListIDAndDate(eventRecipientID, null);
            }

            
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Event Log data by EventTypeID", ex);
        }

    }

    //get all data EventLog
    public static EventLogData.EventLogSummaryDataGridDataTable SelectEventLogByNameTimeAndStatus(string eventTypeName, 
        Nullable<DateTime> eventTime, string deliveryStatus)
    {
        EventLogDataTableAdapters.EventLogSummaryDataGridTableAdapter ta = new EventLogDataTableAdapters.EventLogSummaryDataGridTableAdapter();
        EventLogData.EventLogSummaryDataGridDataTable dt = new EventLogData.EventLogSummaryDataGridDataTable();
        
        try
        {
            ta.FillByEvenTypeNameTimeAndDeliveryStatus(dt, eventTypeName, eventTime, deliveryStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Event Log data by Name, Time and Status",ex);
        }

    }

    //get all data EventLog
    public static EventLogData.EventLogSummaryDataGridDataTable SelectEventLogByIDTimeAndStatus(decimal eventTypeID, Nullable<DateTime> eventTime, string deliveryStatus)
    {
        EventLogDataTableAdapters.EventLogSummaryDataGridTableAdapter ta = new EventLogDataTableAdapters.EventLogSummaryDataGridTableAdapter();
        EventLogData.EventLogSummaryDataGridDataTable dt = new EventLogData.EventLogSummaryDataGridDataTable();

        try
        {
            ta.FillByIDTimeAndStatus(dt, eventTypeID, eventTime, deliveryStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load EventLog data by IDTime and Status", ex);
        }

    }

    public static EventTypeData.EventTypeDataTable getEventTypeName()
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
            throw new ApplicationException("Failed to load event type name data", ex);
        }
    }

    public static EventLogData.EventLogDataTable FillInbox(decimal EventRecipientListID, DateTime EventDate)
    {
        EventLogDataTableAdapters.EventLogTableAdapter ta = new EventLogDataTableAdapters.EventLogTableAdapter();
        EventLogData.EventLogDataTable dt = new EventLogData.EventLogDataTable();

        try
        {
            ta.FillInboxLogByRecipientListIDAndDate(dt, EventRecipientListID, EventDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to fill EventLog by Recipient ID and Date", ex);
        }
    }

    public static void SubmitBroadcast(string broadcastMessage, HttpRequest request)
    {
        try
        {
            EventLogDataTableAdapters.addEventQueueBroadcast ta = new EventLogDataTableAdapters.addEventQueueBroadcast();

            ta.uspAddEventQueue("Broadcast", Common.GetIPAddress(request), "%BROADCAST_MESSAGE%=" + broadcastMessage);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void AddEventQueue(string eventCode, HttpRequest request, string message)
    {
        EventLogDataTableAdapters.addEventQueueBroadcast ta = new EventLogDataTableAdapters.addEventQueueBroadcast();

        try
        {
            ta.uspAddEventQueue(eventCode, Common.GetIPAddress(request), message);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }
}
