using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class RiskManagement_IntradayMargin : System.Web.UI.Page
{
    DateTime businessDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        businessDate = DateTime.Now.Date;

        if (!IsPostBack)
        {
            LoadData();
        }

        CheckBox uiChkHeader = (CheckBox) uiDgIDMPrice.HeaderRow.FindControl("uiChkUpdate");
        uiChkHeader.Attributes.Add("onclick", "setAllCheckBox(this, 'chkUpdate');");
    }

    protected void uiBtnExecuteIntradayMargin_Click(object sender, EventArgs e)
    {
        IntradayMarginDataTableAdapters.IDMPriceTableAdapter taIDMPrice = new IntradayMarginDataTableAdapters.IDMPriceTableAdapter();
        HtmlInputCheckBox uiChkUpdate;
        TextBox uiTxtPrice;
        HiddenField uiHdnContractID;

        foreach (GridViewRow row in uiDgIDMPrice.Rows)
        {
            uiChkUpdate = (HtmlInputCheckBox) row.FindControl("uiChkUpdate");

            if (uiChkUpdate.Checked)
            {
                uiTxtPrice = (TextBox) row.FindControl("uiTxtPrice");
                uiHdnContractID = (HiddenField) row.FindControl("uiHdnContractID");
                taIDMPrice.Update(decimal.Parse(uiTxtPrice.Text), businessDate, decimal.Parse(uiHdnContractID.Value));
            }
        }

        Response.Redirect("ViewIntradayMargin.aspx?r=1&date=" + DateTime.Now.ToString("d-MMM-yyyy"));
    }
    
    private void LoadData()
    {
        IntradayMarginDataTableAdapters.IDMPriceTableAdapter taIDMPrice = new IntradayMarginDataTableAdapters.IDMPriceTableAdapter();
        taIDMPrice.InsertBlank(businessDate, User.Identity.Name);

        IntradayMarginData.IDMPriceDataTable dtPrice = new IntradayMarginData.IDMPriceDataTable();
        taIDMPrice.Fill(dtPrice, businessDate);

        uiDgIDMPrice.DataSource = dtPrice;
        uiDgIDMPrice.DataBind();
    }



















}
