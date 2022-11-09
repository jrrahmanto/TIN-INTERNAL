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

public partial class AuditAndCompliance_ViewAuditTrailData : System.Web.UI.Page
{
    #region "    Use Case   "
    
    #endregion
    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        AuditTrailData.AuditTrailDataTable dt = new AuditTrailData.AuditTrailDataTable();
        try
        {
            if (IsValidEntry())
            {
                try
                {
                    DateTime logTimeTo = DateTime.Now;
                    if (uiDtpLogTo.Text != "")
                    {
                        logTimeTo = DateTime.Parse(uiDtpLogTo.Text).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
                    }

                    dt = AuditTrail.SearchAuditTrail(uiDdlApplicationTable.SelectedValue, uiDdlAction.SelectedValue,
                                                    DateTime.Parse(uiDtpLogFrom.Text).AddHours(0).AddMinutes(0), logTimeTo,
                                                    uiTxbUserName.Text);
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage(ex);
                }
                uiDgAuditTrail.DataSource = dt;
                uiDgAuditTrail.DataBind();

                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void uiDgAuditTrail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgAuditTrail.PageIndex = e.NewPageIndex;
        AuditTrailData.AuditTrailDataTable dt = new AuditTrailData.AuditTrailDataTable();
        try
        {
            DateTime logTimeTo = DateTime.Now.Date;
            if (uiDtpLogTo.Text != "")
            {
                logTimeTo = DateTime.Parse(uiDtpLogTo.Text);
            }
            dt = AuditTrail.SearchAuditTrail(uiDdlApplicationTable.SelectedValue, uiDdlAction.SelectedValue,
                                            DateTime.Parse(uiDtpLogFrom.Text), logTimeTo,
                                            uiTxbUserName.Text);
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
       
        uiDgAuditTrail.DataSource = dt;
        uiDgAuditTrail.DataBind();

        dt.Dispose();
    }

    #region "    Supporting Method    "

    private bool IsValidEntry()
    {
        bool isValid = true;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();


        if (uiDtpLogFrom.Text == "")
        {
            uiBlError.Items.Add("Log Date From is required.");
        }
        
        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }
    #endregion
    
}
