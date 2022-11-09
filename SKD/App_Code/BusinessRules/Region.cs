using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Region
/// </summary>
public class Region
{
    public static RegionData.RegionDataTable SelectDdlRegion()
    {
        try
        {
            RegionData.RegionDataTable dt = new RegionData.RegionDataTable();
            RegionDataTableAdapters.RegionTableAdapter ta = new RegionDataTableAdapters.RegionTableAdapter();

            //dt.AddRegionRow("");
            //dt.AcceptChanges();
            ta.FillByDdlRegion(dt);

            return dt;  
        }
        catch (Exception ex)
        {
            throw new ApplicationException (ex.Message);
        }
    }
}