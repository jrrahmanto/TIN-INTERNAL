using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;


/// <summary>
/// Summary description for Holiday
/// </summary>
public class Holiday
{
    //get all data from dataset
    public static HolidayData.HolidayRow SelectHolidayByHolidayDate(decimal holidayID)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        HolidayData.HolidayRow dr = null;
        try
        {
            ta.FillByHolidayId(dt, holidayID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load holiday data");
        }
    }

    //get date and holidaytype from dataset
    public static HolidayData.HolidayDataTable SelectHolidayByHolidayDateAndHolidayType(DateTime holidayDate, string holidayType, Nullable<decimal> exchangeId, Nullable<decimal> commodity)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        try
        {
           ta.FillByHolidayDateAndAllCriteria(dt, holidayDate, holidayType, exchangeId, commodity);
           return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load holiday data");
        }
    }

    //get all data from dataset
    public static HolidayData.HolidayDataTable SelectHolidayByHolidayDateAndAction(Nullable<DateTime> holidayDate1,Nullable<DateTime> holidayDate2, string Approval, string holidayType)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        try
        {

            ta.FillByHolidayIdAndApprovalStatus(dt, holidayDate1,holidayDate2, Approval, holidayType);
            return dt;

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load holiday data");
        }
    }

    public static void ProposeHolidayDate(DateTime holidayDate, string description,
                                          string holidayType, Nullable<decimal> exchangeID,
                                          Nullable<decimal> commodityID, decimal originalID,
                                          string action, string userName)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();

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

                if (holidayType == "G")
                {
                    ta.Insert(holidayDate.Date, holidayType, null, null, "P",
                            description, userName, DateTime.Now, userName, DateTime.Now, 
                             "", originalID, action);
                   
                    logMessage = string.Format("Proposed Value: holidaydate={0}|description={1}|holidaytype={2}",
                                            holidayDate.ToString("dd-MM-yyyy"),
                                            description.ToString(),
                                            holidayType.ToString());

                    AuditTrail.AddAuditTrail("Holiday", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

                }
                else if (holidayType == "E")
                {
                    ta.Insert(holidayDate.Date, holidayType, exchangeID, null, "P",
                            description, userName, DateTime.Now, userName, DateTime.Now, 
                             "", originalID, action);
                    logMessage = string.Format("Proposed Value: holidaydate={0}|description={1}|holidaytype={2}"+
                                            "|exchangeid={3}",
                                            holidayDate.ToString("dd-MM-yyyy"),
                                            description.ToString(),
                                            holidayType.ToString(),
                                            exchangeID.ToString());

                    AuditTrail.AddAuditTrail("Holiday", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                }
                else if (holidayType == "P")
                {
                  
                        ta.Insert(holidayDate.Date, holidayType, null, Convert.ToDecimal(commodityID), "P",
                             description, "", DateTime.Now, "", DateTime.Now, 
                             "", originalID, action);
                        logMessage = string.Format("Proposed Value:holidaydate={0}|description={1}|holidaytype={2}" +
                                            "|exchangeid={3}|commodityid={4}",
                                                holidayDate.ToString("dd-MM-yyyy"),
                                                description.ToString(),
                                                holidayType.ToString(),
                                                exchangeID.ToString(),
                                                commodityID);
                        AuditTrail.AddAuditTrail("Holiday", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                                    
                }
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


    public static void ProposedDeletedHolidayDate(List<string> deletedList, string userName)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                {
                    foreach (string item in deletedList)
                    {
                        //proposed delete
                        ta.FillByHolidayId(dt, Convert.ToDecimal(item));
                        if (dt.Count > 0)
                        {
                            ta.Insert(dt[0].HolidayDate, dt[0].HolidayType, dt[0].ExchangeId, dt[0].CommodityID, 
                                "P", dt[0].Description, "", DateTime.Now, "", DateTime.Now, 
                         "", dt[0].HolidayId, "D");

                            logMessage = string.Format("Proposed Delete: {0}|{1}|{2}|{3}|{4}",
                                                        dt[0].HolidayDate,
                                                        dt[0].Description,
                                                        dt[0].HolidayType,
                                                        dt[0].ExchangeId,
                                                        dt[0].CommodityID);
                        }
                        //add audit trail
                        AuditTrail.AddAuditTrail("Holiday", AuditTrail.PROPOSE, logMessage, userName,"Delete");
                    }
                }
                scope.Complete();
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


    public static void ApproveHolidayId(decimal holidayID, string userName, string approvalDesc)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();

        try
        {
               try
            {
                ta.FillByHolidayId(dt, holidayID);

               using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";
                    
                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (dt[0].HolidayType == "G")
                        {
                            ta.ApprovedProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                "A", dt[0].Description, userName, DateTime.Now, null, approvalDesc,
                                               null, null, dt[0].HolidayId);

                        }
                        else if (dt[0].HolidayType == "E")
                        {
                            ta.ApprovedProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                               "A", dt[0].Description, userName, DateTime.Now, null, approvalDesc,
                                              null, null, dt[0].HolidayId);
                        }
                        else if (dt[0].HolidayType == "P")
                        {
                            ta.ApprovedProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                 "A", dt[0].Description, userName, DateTime.Now, dt[0].CommodityID, approvalDesc,
                                                null, null, dt[0].HolidayId);
                        }

                        logMessage = string.Format("Approved Insert: holidaydate={0}|description={1}|holidaytype={2}" +
                                            "|exchangeid={3}|commodityid={4}",
                                                           dt[0].HolidayDate,
                                                           dt[0].Description,
                                                           dt[0].HolidayType,
                                                           dt[0].IsExchangeIdNull() ? "" : dt[0].ExchangeId.ToString(),
                                                           dt[0].IsCommodityIDNull() ? "" : dt[0].CommodityID.ToString());
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (dt[0].HolidayType == "G")
                        {
                            ta.ApprovedUpdateProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                     "A", dt[0].Description, userName,
                                                    DateTime.Now, null, approvalDesc, null, null, dt[0].OriginalId);
                        }
                        else if (dt[0].HolidayType == "E")
                        {
                            ta.ApprovedUpdateProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                                     "A", dt[0].Description, userName,
                                                    DateTime.Now, null, approvalDesc, null, null, dt[0].OriginalId);
                        }
                        else if (dt[0].HolidayType == "P")
                        {
                            ta.ApprovedUpdateProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                     "A", dt[0].Description, userName,
                                                    DateTime.Now, dt[0].CommodityID, approvalDesc, null, null, dt[0].OriginalId);
                        }
                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].HolidayId);

                        logMessage = string.Format("Approved Update: holidaydate={0}|description={1}|holidaytype={2}" +
                                            "|exchangeid={3}|commodityid={4}",
                                                           dt[0].HolidayDate,
                                                           dt[0].Description,
                                                           dt[0].HolidayType,
                                                           dt[0].IsExchangeIdNull() ? "" : dt[0].ExchangeId.ToString(),
                                                           dt[0].IsCommodityIDNull() ? "" : dt[0].CommodityID.ToString());
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalId);
                        ta.DeleteProposedItem(dt[0].HolidayId);

                        logMessage = string.Format("Approved Delete: holidaydate={0}|description={1}|holidaytype={2}" +
                                            "|exchangeid={3}|commodityid={4}",
                                                           dt[0].HolidayDate,
                                                           dt[0].Description,
                                                           dt[0].HolidayType,
                                                           dt[0].IsExchangeIdNull() ? "" : dt[0].ExchangeId.ToString(),
                                                           dt[0].IsCommodityIDNull() ? "" : dt[0].CommodityID.ToString());
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Holiday", AuditTrail.APPROVE, logMessage, userName,"Approve " + ActionFlagDesc);

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

    public static void ApproveHoliday(List<string> approvalList, string userName)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        
       
        try
        {
            //check effective start date proposed item
            //if in between
            //1. if action insert new then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //2. if action updated then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            //3. if action deleted then update effective end date previous record to effective start date proposed item - 1
            //  and update effective end date proposed item to effective start date next record - 1 
            
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string holidayID in approvalList)
                {
                    string logMessage = "";
                    ta.FillByHolidayId(dt, Convert.ToDecimal(holidayID));
                    if (dt.Count > 0)
                    {
                      

                            //update record
                            if (dt[0].ActionFlag == "I")
                            {
                                if (dt[0].HolidayType == "G")
                                {
                                    ta.ApprovedProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                         "A", dt[0].Description, userName, DateTime.Now, null, "",
                                                        null, null, dt[0].HolidayId);

                                    logMessage = string.Format("Approved Insert: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                                else if (dt[0].HolidayType == "E")
                                {
                                    ta.ApprovedProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                                        "A", dt[0].Description, userName, DateTime.Now, null, "",
                                                       null, null, dt[0].HolidayId);

                                    logMessage = string.Format("Approved Insert: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                                else if (dt[0].HolidayType == "P")
                                {
                                    ta.ApprovedProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                                        "A", dt[0].Description, userName, DateTime.Now, dt[0].CommodityID, "",
                                                       null, null, dt[0].HolidayId);

                                    logMessage = string.Format("Approved Insert: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                            }
                            else if (dt[0].ActionFlag == "U")
                            {
                                if (dt[0].HolidayType == "G")
                                {
                                    ta.ApprovedUpdateProposedItem(null, dt[0].HolidayDate, dt[0].HolidayType,
                                                           "A", dt[0].Description, userName,
                                                          DateTime.Now, null, "", null, null, dt[0].OriginalId);

                                    //delete proposed record
                                    ta.DeleteProposedItem(dt[0].HolidayId);

                                    logMessage = string.Format("Approved Update: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                                else if (dt[0].HolidayType == "E")
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                                           "A", dt[0].Description, userName,
                                                          DateTime.Now, null, "", null, null, dt[0].OriginalId);

                                    //delete proposed record
                                    ta.DeleteProposedItem(dt[0].HolidayId);

                                    logMessage = string.Format("Approved Update: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                                else if (dt[0].HolidayType == "P")
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].ExchangeId, dt[0].HolidayDate, dt[0].HolidayType,
                                                           "A", dt[0].Description, userName,
                                                          DateTime.Now, dt[0].CommodityID, "", null, null, dt[0].OriginalId);

                                    //delete proposed record
                                    ta.DeleteProposedItem(dt[0].HolidayId);

                                    logMessage = string.Format("Approved Update: {0}|{1}|{2}",
                                                                       dt[0].HolidayDate,
                                                                       dt[0].Description,
                                                                       dt[0].HolidayType);
                                }
                            }
                            else if (dt[0].ActionFlag == "D")
                            {
                                ta.DeleteProposedItem(dt[0].OriginalId);
                                ta.DeleteProposedItem(dt[0].HolidayId);

                                logMessage = string.Format("Approved Delete: {0}|{1}|{2}",
                                                                   dt[0].HolidayDate,
                                                                   dt[0].Description,
                                                                   dt[0].HolidayType);
                            }
                            string ActionFlagDesc = "";
                            switch (dt[0].ActionFlag)
                            {
                                case "I": ActionFlagDesc = "Insert"; break;
                                case "U": ActionFlagDesc = "Update"; break;
                                case "D": ActionFlagDesc = "Delete"; break;
                            }
                            AuditTrail.AddAuditTrail("Holiday", AuditTrail.APPROVE, logMessage, userName, "Approve " + ActionFlagDesc);
                    }
                }
                scope.Complete();
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


    public static void RejectProposedHolidayDate(decimal holidayId, string userName, Nullable<decimal> exchangeID,
                                                Nullable<decimal> commodityId)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {

                string logMessage = "";
                ta.FillByHolidayId(dt, holidayId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {

                    logMessage = string.Format("Reject : holidaydate={0}|description={1}|holidaytype={2}" +
                                            "|exchangeid={3}|commodityid={4}",
                                                       dt[0].HolidayDate,
                                                           dt[0].Description,
                                                           dt[0].HolidayType,
                                                           dt[0].IsExchangeIdNull() ? "" : dt[0].ExchangeId.ToString(),
                                                           dt[0].IsCommodityIDNull() ? "" : dt[0].CommodityID.ToString());
                   
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.DeleteRejectItem(holidayId);

                AuditTrail.AddAuditTrail("Holiday", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static void RejectHoliday(List<string> rejectedList, string userName)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (string item in rejectedList)
                {
                    string logMessage = "";
                    ta.FillByHolidayId(dt, Convert.ToDecimal(item));
                    string ActionFlagDesc = "";
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Rejected: {0}|{1}|{2}",
                                                          dt[0].HolidayDate,
                                                           dt[0].Description,
                                                           dt[0].HolidayType);
                       
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                    }

                    ta.DeleteRejectItem(Convert.ToDecimal(item));
                    AuditTrail.AddAuditTrail("Holiday", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static bool ValidHoliday(DateTime holiday)
    {
        HolidayDataTableAdapters.HolidayTableAdapter ta = new HolidayDataTableAdapters.HolidayTableAdapter();
        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
        bool isValid = true;
        try
        {
            ta.FillByHolidayDate(dt, holiday);
            if (dt.Rows.Count > 0)
            {
                isValid = false;
            }
            return isValid;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load holiday data");
        }
    }

    #region supporting method

    public static HolidayData.ExchangeDataTable getExchangeCode()
    {
        HolidayDataTableAdapters.ExchangeTableAdapter ta = new HolidayDataTableAdapters.ExchangeTableAdapter();
        HolidayData.ExchangeDataTable dt = new HolidayData.ExchangeDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load exchange code data");
        }
    }

    public static HolidayData.CommodityDataTable getProductCode()
    {
        HolidayDataTableAdapters.CommodityTableAdapter ta = new HolidayDataTableAdapters.CommodityTableAdapter();
        HolidayData.CommodityDataTable dt = new HolidayData.CommodityDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load commodity code data");
        }
    }


    #endregion


}