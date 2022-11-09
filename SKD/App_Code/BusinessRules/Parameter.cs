using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for Parameter
/// </summary>
public class Parameter
{
    public const string PARAM_BUSINESS_DATE = "BusinessDate";

	public Parameter()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static Nullable<DateTime> GetParameterBusinessDate()
    {
        ParameterData.ParameterDataTable dt = new ParameterData.ParameterDataTable();
        ParameterData.ParameterRow dr = null;
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();
        Nullable<DateTime> businessDate = null;

        try
        {
            ta.FillByCodeAndApprovalStatus(dt, "BusinessDate", "A");
            if (dt.Count > 0)
            {
                dr = dt[0];
                if (!dr.IsDateValueNull())
                {
                    businessDate = dr.DateValue;
                }                
            }

            return businessDate;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static Nullable<DateTime> GetParameterLastEOD()
    {
        ParameterData.ParameterDataTable dt = new ParameterData.ParameterDataTable();
        ParameterData.ParameterRow dr = null;
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();
        Nullable<DateTime> lastEOD = null;
        try
        {
            ta.FillByCodeAndApprovalStatus(dt, "LastEOD", "A");
            if (dt.Count > 0)
            {
                dr = dt[0];
                lastEOD = dr.DateValue;
            }

            return lastEOD;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ParameterData.ParameterRow GetParameterByCodeAndApprovalStatus(string code, string approvalStatus)
    {
        ParameterData.ParameterDataTable dt = new ParameterData.ParameterDataTable();
        ParameterData.ParameterRow dr = null;
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();

        try
        {
            ta.FillByCodeAndApprovalStatus(dt, code, approvalStatus);
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

    public static void UpdateParameterDateValueByCode(string code, Nullable<DateTime> dateValue,
            string lastUpdatedBy, DateTime lastUpdatedDate)
    {
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();

        try
        {
            ta.UpdateParameterDateValueByCode(dateValue, lastUpdatedBy, lastUpdatedDate, code);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ParameterData.ParameterGlobalDataTable SelectParameterByCode(string paramCode)
    {
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();
        ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();

        try
        {
            ta.FillByParameterCodeAndStatus(dt, paramCode, null);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load paramater data");
        }

    }

    public static ParameterData.ParameterGlobalDataTable SelectParameterByCodeAndApprovalStatus(string paramCode, string approvalStatus)
    {
        ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();

        try
        {
            ta.FillByParameterCodeAndStatus(dt, paramCode, approvalStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static ParameterData.ParameterGlobalRow SelectParameterByParameterID(decimal parameterID)
    {
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();
        ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();
        ParameterData.ParameterGlobalRow dr = null;
        try
        {
            ta.FillByParameterID(dt, parameterID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load parameter data");
        }

    }

    public static void ProposeParameter(string code, Nullable<decimal> numericValue, string createdBy, DateTime createDate,
                                          Nullable<DateTime> dateValue, string stringValue, string lastUpdateBy,
                                          DateTime startDate, Nullable<DateTime> endDate, DateTime lastUpdateDate,
                                          string approvalDesc, string action, string userName, decimal OriginalID)
    {
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(code, startDate, "P", numericValue, dateValue, stringValue, createdBy,
                         createDate, lastUpdateBy, lastUpdateDate, endDate, 
                         approvalDesc, action, OriginalID);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: ParameterCode={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|NumericValue={3}|StringValue={4}|DateValue={5}",
                                         code.ToString(), startDate.ToString("dd-MM-yyyy"), Convert.ToDateTime(endDate).Date,
                                         Convert.ToDecimal(numericValue), stringValue.ToString(),
                                         Convert.ToDateTime(dateValue).Date);
                AuditTrail.AddAuditTrail("Parameter", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
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

    public static void ApproveParameter(decimal parameterID, string userName, string approvaldesc)
    {
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();
        ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();

        try
        {
            try
            {
                ta.FillByParameterID(dt, parameterID);

                decimal prevOriginalID = Convert.ToDecimal(ta.GetOriginalIDPrevStartDate(dt[0].EffectiveStartDate,
                                         dt[0].Code, dt[0].OriginalID));

                DateTime? nextStartDate = Convert.ToDateTime(ta.GetNextStartDate(dt[0].Code, dt[0].EffectiveStartDate, dt[0].OriginalID));

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    // Update end date of previous record
                    if (prevOriginalID != 0)
                    {
                        ta.UpdateEndDateByParameterID(dt[0].EffectiveStartDate.AddDays(-1), prevOriginalID);
                    }

                    // Update end date of current record
                    if (nextStartDate > DateTime.MinValue)
                    {
                        dt[0].EffectiveEndDate = nextStartDate.Value.AddDays(-1);
                    }

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        if (!dt[0].IsEffectiveEndDateNull())
                        {
                            if (!dt[0].IsDateValueNull())
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           dt[0].NumericValue, dt[0].DateValue, dt[0].StringValue, userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                           approvaldesc, null, null, dt[0].ParameterID);
                                }
                                else
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           null, dt[0].DateValue, dt[0].StringValue, userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                           approvaldesc, null, null, dt[0].ParameterID);
                                }
                            }
                            else
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                              dt[0].NumericValue, null, dt[0].StringValue, userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                              approvaldesc, null, null, dt[0].ParameterID);
                                }
                                else
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           null, null, dt[0].StringValue, userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                           approvaldesc, null, null, dt[0].ParameterID);
                                }
                            }
                        }
                        else
                        {
                            if (!dt[0].IsDateValueNull())
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                              dt[0].NumericValue, dt[0].DateValue, dt[0].StringValue,
                                                              userName, DateTime.Now, null,
                                                              approvaldesc, null, null, dt[0].ParameterID);
                                }
                                else
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           null, dt[0].DateValue, dt[0].StringValue,
                                                           userName, DateTime.Now, null,
                                                           approvaldesc, null, null, dt[0].ParameterID);
                                }
                            }
                            else
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                              dt[0].NumericValue, null, dt[0].StringValue,
                                                              userName, DateTime.Now, null,
                                                              approvaldesc, null, null, dt[0].ParameterID);
                                }
                                else
                                {
                                    ta.ApprovedProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                             null, null, dt[0].StringValue,
                                                             userName, DateTime.Now, null,
                                                             approvaldesc, null, null, dt[0].ParameterID);
                                }
                            }
                        }

                        logMessage = string.Format("Approved Insert: ParameterCode={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|NumericValue={3}|StringValue={4}|DateValue={5}",
                                                         dt[0].Code,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                         dt[0].IsNumericValueNull() ? "" : dt[0].NumericValue.ToString(),
                                                         dt[0].StringValue,
                                                         dt[0].IsDateValueNull() ? "" : dt[0].DateValue.ToString("dd-MM-yyy"));

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        if (dt[0].IsEffectiveEndDateNull())
                        {
                            if (!dt[0].IsDateValueNull())
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                              dt[0].NumericValue, dt[0].DateValue, dt[0].StringValue,
                                                              userName, DateTime.Now, null,
                                                              approvaldesc, null, dt[0].OriginalID);
                                }
                                else
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           null, dt[0].DateValue, dt[0].StringValue,
                                                          userName, DateTime.Now, null,
                                                           approvaldesc, null, dt[0].OriginalID);
                                }
                            }
                            else
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                                dt[0].NumericValue, null, dt[0].StringValue,
                                                                userName, DateTime.Now, null,
                                                                approvaldesc, null, dt[0].OriginalID);
                                }
                                else
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                              null, null, dt[0].StringValue,
                                                             userName, DateTime.Now, null,
                                                               approvaldesc, null, dt[0].OriginalID);
                                }
                            }
                        }
                        else
                        {
                            if (!dt[0].IsDateValueNull())
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                            dt[0].NumericValue, dt[0].DateValue, dt[0].StringValue,
                                                            userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                            approvaldesc, null, dt[0].OriginalID);
                                }
                                else
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                           null, dt[0].DateValue, dt[0].StringValue,
                                                           userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                           approvaldesc, null, dt[0].OriginalID);
                                }
                            }
                            else
                            {
                                if (!dt[0].IsNumericValueNull())
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                            dt[0].NumericValue, null, dt[0].StringValue,
                                                            userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                            approvaldesc, null, dt[0].OriginalID);
                                }
                                else
                                {
                                    ta.ApprovedUpdateProposedItem(dt[0].Code, dt[0].EffectiveStartDate, "A",
                                                        null, null, dt[0].StringValue,
                                                        userName, DateTime.Now, dt[0].EffectiveEndDate,
                                                        approvaldesc, null, dt[0].OriginalID);
                                }
                            }
                        }

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].ParameterID);
                        logMessage = string.Format("Approved Update: ParameterCode={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|NumericValue={3}|StringValue={4}|DateValue={5}",
                                                         dt[0].Code,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                         dt[0].IsNumericValueNull() ? "" : dt[0].NumericValue.ToString(),
                                                         dt[0].StringValue,
                                                         dt[0].IsDateValueNull() ? "" : dt[0].DateValue.ToString("dd-MM-yyy"));
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].ParameterID);
                        logMessage = string.Format("Approved Delete: ParameterCode={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|NumericValue={3}|StringValue={4}|DateValue={5}",
                                                         dt[0].Code,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                         dt[0].IsNumericValueNull() ? "" : dt[0].NumericValue.ToString(),
                                                         dt[0].StringValue,
                                                         dt[0].IsDateValueNull() ? "" : dt[0].DateValue.ToString("dd-MM-yyy"));
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Parameter", AuditTrail.APPROVE, logMessage, userName, ActionFlagDesc);

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

    public static void RejectProposedParameter(decimal parameterId, string userName)
    {
        ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();
        ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByParameterID(dt, parameterId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Reject : ParameterCode={0}|EffectiveStartDate={1}|EffectiveEndDate={2}|NumericValue={3}|StringValue={4}|DateValue={5}",
                                                         dt[0].Code,
                                                         dt[0].EffectiveStartDate,
                                                         dt[0].IsEffectiveEndDateNull() ? "" : dt[0].EffectiveEndDate.ToString("dd-MM-yyyy"),
                                                         dt[0].IsNumericValueNull() ? "" : dt[0].NumericValue.ToString(),
                                                         dt[0].StringValue,
                                                         dt[0].IsDateValueNull() ? "" : dt[0].DateValue.ToString("dd-MM-yyy"));
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }
                ta.DeleteRejectItem(parameterId);

                AuditTrail.AddAuditTrail("Parameter", AuditTrail.PROPOSE, logMessage, userName, "Reject" + ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    public static decimal GetMaxRevision(DateTime InputDate)
    {
        decimal _maxRevision = 0;
        ParameterDataTableAdapters.Revision ta = new ParameterDataTableAdapters.Revision();

        try
        {
            _maxRevision = Convert.ToDecimal(ta.GetMaxRevision(InputDate));
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to get maximum revision of EOD", ex);
        }

        return _maxRevision;
    }

    public static void UpdateParameterStringValueByCode(string code, string stringValue,
            string lastUpdatedBy, DateTime lastUpdatedDate)
    {
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();

        try
        {
            ta.UpdateParameterStringValueByCode(stringValue, lastUpdatedBy, lastUpdatedDate, code);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateParameterNumericValueByCode(string code, decimal numericValue,
            string lastUpdatedBy, DateTime lastUpdatedDate)
    {
        ParameterDataTableAdapters.ParameterTableAdapter ta = new ParameterDataTableAdapters.ParameterTableAdapter();

        try
        {
            ta.UpdateParameterNumericValueByCode(numericValue, lastUpdatedBy, lastUpdatedDate, code);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

}
