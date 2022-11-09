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

public partial class AuditAndCompliance_DetailActivityLog : System.Web.UI.Page
{
    private DateTime timeId
    {
        get
        {
            try
            {
                return DateTime.ParseExact(Request.QueryString["logTime"], "yyyy-MM-ddTHH_mm_ss.fff", new System.Globalization.CultureInfo("en-us"));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();

        if (!IsPostBack)
        {
            if (timeId != DateTime.MinValue)
            {
                //load data
                //bind data
                bindData();
            }
        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewActiviyLog.aspx");
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnCancel.Visible = mp.IsMaker;
    }

    #region SupportingMethod

    private void bindData()
    {
        ActivityLogData.ActivityLogDataTable dt = new ActivityLogData.ActivityLogDataTable();
        dt = ActivityLog.SelectActivityLogByLogTimeAll(dt, Convert.ToDateTime(timeId));
        if (dt.Rows.Count > 0)
        {
            uiLblLogTime.Text = dt[0].LogTime.ToString("dd-MMM-yyyy.fff");
            uiLblLogActivity.Text = dt[0].LogActivity;
            uiLblLogMessage.Text = dt[0].LogMessage;
            uiLblSourceIP.Text = dt[0].SourceIP;
            uiLblUserName.Text = dt[0].UserName;
        }
    }


    #endregion

}
