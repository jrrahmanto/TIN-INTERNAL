using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Dashboard
/// </summary>
public class DashboardHandle
{
    public DashboardHandle()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DashboardData.DashboardRow GetDashboardByUserName(string userName)
    {
        DashboardData.DashboardDataTable dt = new DashboardData.DashboardDataTable();
        DashboardData.DashboardRow dr = null;
        DashboardDataTableAdapters.DashboardTableAdapter ta = new DashboardDataTableAdapters.DashboardTableAdapter();

        try
        {
            ta.FillByUserName(dt, userName);
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

    public static void ProcessDashboard(string userName,
        string report1, string report2, string report3, string report4)
    {
        DashboardData.DashboardDataTable dt = new DashboardData.DashboardDataTable();
        DashboardDataTableAdapters.DashboardTableAdapter ta =
            new DashboardDataTableAdapters.DashboardTableAdapter();

        try
        {
            ta.FillByUserName(dt, userName);
            if (dt.Count > 0)
            {
                ta.UpdateByUserName(report1, report2, report3, report4, userName); 
            }
            else
            {
                ta.InsertDashboard(userName, report1, report2, report3, report4);
            }
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
