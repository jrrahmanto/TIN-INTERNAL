using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Administration_EventManagement_DetailEventLog : System.Web.UI.Page
{

    private decimal eventID
    {
        get
        {
            try
            {
                return Decimal.Parse(Request.QueryString["eventId"]);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

    private decimal recListId
    {
        get
        {
            try
            {
                return Decimal.Parse(Request.QueryString["recListId"]);
            }
            catch (Exception)
            {
                return -1;
            } 
        }
    }

    private DateTime timeId
    {
        get
        {
            try
            {
                return DateTime.ParseExact(Request.QueryString["time"], "yyyy-MM-ddTHH_mm_ss.fff", new System.Globalization.CultureInfo("en-us"));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }

    private string callerPage
    {
        get
        {
            try
            {
                return Request.QueryString["callerPage"];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uiBLError.Visible = false;
            if (!IsPostBack)
            {
                if (eventID != -1 && recListId != -1 && timeId != DateTime.MinValue)
                {
                    //load data
                    //bind data
                    bindData();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (callerPage == "B")
        {
            Response.Redirect("~/ClearingAndSettlement/MasterData/ViewManageBroadcast.aspx");
        }
        else
        {
            Response.Redirect("ViewEventLog.aspx");
        }
    }

    #region SupportingMethod

    private void bindData()
    {
        try
        {

            EventLogData.EventLogDataTable dt = new EventLogData.EventLogDataTable();
            dt = EventLog.SelectEventTypeByEventTypeID(dt, Convert.ToDecimal(eventID), Convert.ToDecimal(recListId), Convert.ToDateTime(timeId));
            if (dt.Rows.Count > 0)
            {
                uiLblEventTypeName.Text = dt[0].EventTypeName;
                uiLblEventTime.Text = dt[0].EventTime.ToString("dd-MMM-yyyy HH:mm:ss.fff");
                uiLblEventReceipientName.Text = dt[0].EventRecipientName;
                uiLblDeliveryChannel.Text= dt[0].ChannelDesc;
                uiLblDeliveryStatus.Text = dt[0].DeliveryStatus;
                uiLblDeliveryMessage.Text = dt[0].DeliveryMessage;
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }


    #endregion
}
