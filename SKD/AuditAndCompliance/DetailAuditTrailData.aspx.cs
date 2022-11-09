using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AuditAndCompliance_DetailAuditTrailData : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    #region "    Use Case    "
    protected void Page_Load(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            BindData();
        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAuditTrailData.aspx");
    }
    #endregion
    
    #region "    Supporting Method    "
    public void BindData()
    {
        AuditTrailData.AuditTrailDataTable dt = new AuditTrailData.AuditTrailDataTable();
        dt = AuditTrail.SearchAuditTrailByID(decimal.Parse(currentID));
        if (dt.Count > 0)
        {
            uiLblActionName.Text = dt[0].UserAction;
            uiLblLogMessage.Text = dt[0].LogMessage;
            uiLblLogTime.Text = dt[0].LogTime.ToString("dd-MMM-yyyy");
            uiLblUserName.Text = dt[0].UserName;
            uiLblTableName.Text = dt[0].ApplicationTable;
        }
    }
    #endregion

}
