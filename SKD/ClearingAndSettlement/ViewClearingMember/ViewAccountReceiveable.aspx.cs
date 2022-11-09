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
        //VisibleLbl(false);
        if (!Page.IsPostBack)
        {
            VisibleLbl(false);
        }
        //else
        //{
        //    VisibleLbl(true);
        //}

    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsValidGet())
            //{
            //VisibleLbl(true);
            FillForcedClosedTradeGrid();

            //}
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }       
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
    /// <summary>
    /// Visible label buyer and seller
    /// </summary>
    /// <param name="lbl"></param>
    private void VisibleLbl(bool lbl)
    {
        uiLblBuyerFee.Visible = lbl;
        uiLblBuyerFeeValue.Visible = lbl;
        uiLblFeeSeller.Visible = lbl;
        uiLblFeeSellerValue.Visible = lbl;

    }
    private void FillForcedClosedTradeGrid()
    {

        
        uiDgvSellerFee.DataSource = odsForcedClosedTrade;
        uiDgvBuyerFee.DataSource = odsBuyerFee;
        //IEnumerable dv = (IEnumerable)ObjectDataSourceBankTransaction.Select();
        IEnumerable dv = odsForcedClosedTrade.Select();
        DataView dve = (DataView)dv;
        IEnumerable dvBuyer = odsBuyerFee.Select();
        DataView dveBuyer = (DataView)dvBuyer;

        uiDgvSellerFee.DataSource = dve;
        uiDgvSellerFee.DataBind();
        uiDgvBuyerFee.DataSource = dveBuyer;
        uiDgvBuyerFee.DataBind();

        if (uiDgvBuyerFee.Rows.Count > 0 && uiDgvSellerFee.Rows.Count > 0)
        {
            VisibleLbl(true);
            uiLblBuyerFeeValue.Text = GetFee("BuyerFeePerKg", "A");
            uiLblFeeSellerValue.Text = GetFee("SellerFeePerKg", "A");
        }
        else
        {
            VisibleLbl(false);
            
        }
    }

    private string GetFee(string code, string approvalStatus)
    {
        string feeValue = "";
        try
        {
            

            ParameterData.ParameterRow  dr ;
            dr = Parameter.GetParameterByCodeAndApprovalStatus(code, approvalStatus);
            feeValue = dr.NumericValue.ToString();
            
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }

        return feeValue;
    }
}
