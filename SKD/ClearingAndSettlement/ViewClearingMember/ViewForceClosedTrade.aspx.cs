using System;
using System.Collections;
using System.Collections.Generic;
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
using Microsoft.Reporting.WebForms;



public partial class ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsValidGet())
            //{
               FillForcedClosedTradeGrid();
                
            //}
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
    }

    protected void uiDgvForceClosedTrade_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgvForceClosedTrade.PageIndex = e.NewPageIndex;
        FillForcedClosedTradeGrid();
    }
    /// <summary>
    /// Validation for parameter entry
    /// </summary>
    /// <returns></returns>
    private bool IsValidGet()
    {
        bool isValid = true;
        uiBLError.Items.Clear();

        // Check for start date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            uiBLError.Items.Add("Start Date is required.");
        }

        // Check for end date parameter
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            uiBLError.Items.Add("End Date is required.");
        }

        // Display if only error exist       
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
    private void FillForcedClosedTradeGrid()
    {
        uiDgvForceClosedTrade.DataSource = odsForcedClosedTrade;
        //IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        IEnumerable dv = odsForcedClosedTrade.Select();
        DataView dve = (DataView)dv;

        //if (!string.IsNullOrEmpty(SortOrder))
        //{
        //    dve.Sort = SortOrder;
        //}

        uiDgvForceClosedTrade.DataSource = dve;
        uiDgvForceClosedTrade.DataBind();
    }


   
}
