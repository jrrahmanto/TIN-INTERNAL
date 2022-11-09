using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;


/// <summary>
/// Summary description for EventType
/// </summary>
public class EventType
{
    public static void AddEventQueue(string eventTypeCode, string parameters, string sourceIp)
    {
        EventTypeDataTableAdapters.QueriesTableAdapter ta = new EventTypeDataTableAdapters.QueriesTableAdapter();

        try
        {
            ta.uspAddEventQueue(eventTypeCode, sourceIp, parameters);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void AddEarlyWarningContractMessage(DateTime BusinessDate)
    {
        EventTypeDataTableAdapters.QueriesTableAdapter ta = new EventTypeDataTableAdapters.QueriesTableAdapter();

        try
        {
            ta.uspMsgEarlyWarningContract(BusinessDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    //get all data eventtype from dataset to datagrid
    public static EventTypeData.EventTypeRow SelectEventTypeByEventTypeID(decimal eventTypeID)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();
        EventTypeData.EventTypeDataTable dt = new EventTypeData.EventTypeDataTable();
        EventTypeData.EventTypeRow dr = null;
        try
        {
            ta.FillByEventTypeID(dt, eventTypeID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load eventtype data");
        }

    }

    //get all data eventtype from eventtype by code and name
    public static EventTypeData.EventTypeDataTable SelectEventTypeByEventTypeCodeAndName(string eventTypeCode, string eventTypeName)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();
        EventTypeData.EventTypeDataTable dt = new EventTypeData.EventTypeDataTable();

        try
        {
            ta.FillByEventTypeCodeAndName(dt, eventTypeCode, eventTypeName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load eventtype data");
        }

    }


    public static void AddEventType(string eventTypeCode, string eventTypeName, bool smsChannel,
                                      bool emailChannel, bool applicationChannel, string smsMessage,
                                       string emailMessage, string applicationMessage, string username)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(eventTypeCode, eventTypeName, smsChannel, emailChannel, applicationChannel, smsMessage,
                            emailMessage, applicationMessage);

                logMessage = string.Format("Insert Value: eventTypeCode={0}|eventTypeName={1}|smsChannel={2}|emailChannel={3}|applicationChannel={4}|smsMessage={5}|emailMessage={6}|applicationMessage={7}",
                                            eventTypeCode, 
                                            eventTypeName, 
                                            smsChannel, 
                                            emailChannel, 
                                            applicationChannel, 
                                            smsMessage,
                                            emailMessage, 
                                            applicationMessage);
                AuditTrail.AddAuditTrail("EventType", "Insert", logMessage, username,"Insert");

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

    public static void UpdateEventType(string eventTypeCode, string eventTypeName, bool smsChannel,
                                      bool emailChannel, bool applicationChannel, string smsMessage,
                                       string emailMessage, string applicationMessage, string username)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(eventTypeName, smsChannel, emailChannel, applicationChannel, smsMessage,
                            emailMessage, applicationMessage,eventTypeCode);

                logMessage = string.Format("Update Value: eventTypeCode={0}|eventTypeName={1}|smsChannel={2}|emailChannel={3}|applicationChannel={4}|smsMessage={5}|emailMessage={6}|applicationMessage={7}",
                                            eventTypeCode,
                                            eventTypeName,
                                            smsChannel,
                                            emailChannel,
                                            applicationChannel,
                                            smsMessage,
                                            emailMessage,
                                            applicationMessage);
                AuditTrail.AddAuditTrail("EventType", "Update", logMessage, username,"Update");

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

    public static void DeleteEventType(decimal eventTypeId, string userName)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();
        EventTypeData.EventTypeDataTable dt = new EventTypeData.EventTypeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByEventTypeID(dt, eventTypeId);

                if (dt.Count > 0)
                {
                    logMessage = string.Format("Delete Value: eventTypeCode={0}|eventTypeName={1}|smsChannel={2}|emailChannel={3}|applicationChannel={4}|smsMessage={5}|emailMessage={6}|applicationMessage={7}",
                                                          dt[0].EventTypeCode,
                                                          dt[0].Name,
                                                          dt[0].SMSChannel,
                                                          dt[0].EmailChannel,
                                                          dt[0].ApplicationChannel,
                                                          dt[0].SMSMessage,
                                                          dt[0].EmailMessage,
                                                          dt[0].ApplicationMessage);
                }
                ta.DeleteEventTypeCode(eventTypeId);

                AuditTrail.AddAuditTrail("EventType", "Delete", logMessage, userName,"Delete");
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


    public static void DeleteEventTypeList(List<string> deleteList, string userName)
    {
        EventTypeDataTableAdapters.EventTypeTableAdapter ta = new EventTypeDataTableAdapters.EventTypeTableAdapter();
        EventTypeData.EventTypeDataTable dt = new EventTypeData.EventTypeDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string item in deleteList)
                {
                    string logMessage = "";
                    ta.FillByEventTypeID(dt, Convert.ToDecimal(item));
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Delete Value: eventTypeCode={0}|eventTypeName={1}|smsChannel={2}|emailChannel={3}|applicationChannel={4}|smsMessage={5}|emailMessage={6}|applicationMessage={7}",
                                                          dt[0].EventTypeCode,
                                                          dt[0].Name,
                                                          dt[0].SMSChannel,
                                                          dt[0].EmailChannel,
                                                          dt[0].ApplicationChannel,
                                                          dt[0].SMSMessage,
                                                          dt[0].EmailMessage,
                                                          dt[0].ApplicationMessage);
                    }

                    ta.DeleteEventTypeCode(Convert.ToDecimal(item));
                    AuditTrail.AddAuditTrail("EventType", "Deleted", logMessage, userName,"Delete");
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

}
