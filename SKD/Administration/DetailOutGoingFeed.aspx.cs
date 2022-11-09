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

public partial class Administration_DetailOutGoingFeed : System.Web.UI.Page
{
    private decimal eID
    {
        get
        {
            try
            {
                return Decimal.Parse(Request.QueryString["eID"]);
            }
            catch (Exception)
            {
                return -1;
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
                if (eID != -1)
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
       Response.Redirect("ViewEventLog.aspx");
    }

    #region SupportingMethod

    private void bindData()
    {
        try
        {

            OutGoingData.OutgoingFeedDataTable dt = new OutGoingData.OutgoingFeedDataTable();
            dt = OutGoing.SelectOutGoingFeedByFeedID(dt, eID);
            if (dt.Rows.Count > 0)
            {
                uiLblFeedType.Text = dt[0].FeedType;
                uiLblFeedNo.Text = dt[0].FeedNo.ToString();
                uiLblFeedMessage.Text = dt[0].FeedMessage;
                uiLblBusinessDate.Text= dt[0].BusinessDate.ToString("dd-MMM-yyyy");
                uiLblSubmittedStatus.Text = dt[0].SubmittedStatus;
                if (dt[0].IsSubmittedTimeNull())
                {
                 uiLblSubmittedTime.Text = "";
                }
                    else
                {
                    uiLblSubmittedTime.Text = dt[0].SubmittedTime.ToShortTimeString();
                }
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
