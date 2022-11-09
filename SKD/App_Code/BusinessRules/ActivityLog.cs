using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for ActivityLog
/// </summary>
public class ActivityLog
{

    //get all data ActivityLog from dataset to datagrid
    public static ActivityLogData.ActivityLogDataTable SelectActivityLogByLogTimeAll(ActivityLogData.ActivityLogDataTable dt, DateTime logTime)
    {
        ActivityLogDataTableAdapters.ActivityLogTableAdapter ta = new ActivityLogDataTableAdapters.ActivityLogTableAdapter();
        try
        {
            ta.FillByLogTime(dt, logTime);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load activitylog data");
        }

    }
    
	public static ActivityLogData.ActivityLogDataTable SelectActivityLogByLogTime(Nullable<DateTime> logTime, string activityLog, string sourceIP, string userName)
	{
        ActivityLogDataTableAdapters.ActivityLogTableAdapter ta = new ActivityLogDataTableAdapters.ActivityLogTableAdapter();
        ActivityLogData.ActivityLogDataTable dt = new ActivityLogData.ActivityLogDataTable();

        try
        {
            ta.FillByLogTimeAll(dt, logTime, activityLog, sourceIP, userName);
                return  dt;
        }
        catch (Exception ex)
        { 
            throw new Exception("Failed to load activity log data");
        }
	}
}
