using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for OutGoing
/// </summary>
public class OutGoing
{
    public static OutGoingData.OutgoingFeedDataTable SelectOutGoingFeedByCriteria(string feedType,string feedNo,
       Nullable<DateTime> businessDate, string submittedStatus)
    {
        OutGoingDataTableAdapters.OutgoingFeedTableAdapter ta = new OutGoingDataTableAdapters.OutgoingFeedTableAdapter();
        OutGoingData.OutgoingFeedDataTable dt = new OutGoingData.OutgoingFeedDataTable();

        try
        {
            ta.FillByCriteria(dt, feedType, feedNo, businessDate, submittedStatus);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Out going feed data", ex);
        }

    }

    public static OutGoingData.OutgoingFeedDataTable SelectOutGoingFeedByFeedID(OutGoingData.OutgoingFeedDataTable dt, decimal eID)
    {
        OutGoingDataTableAdapters.OutgoingFeedTableAdapter ta = new OutGoingDataTableAdapters.OutgoingFeedTableAdapter();
        try
        {
            ta.FillByFeedID(dt, eID);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load Outgoing Feed data by FeedID", ex);
        }

    }

}
