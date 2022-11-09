using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StagingPort
/// </summary>
public class StagingPort
{
    public static StagingData.PortDataTable SelectPort()
    {
        StagingDataTableAdapters.PortTableAdapter ta = new StagingDataTableAdapters.PortTableAdapter();
        StagingData.PortDataTable dt = new StagingData.PortDataTable();

        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load port data");
        }
    }
}