using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_EntryDueDateSettlement : System.Web.UI.Page
{
    private DateTime BusinessDateReport
    {
        get { return (DateTime)ViewState["BusinessDateReport"]; }
        set { ViewState["BusinessDateReport"] = value; }
    }
    private decimal ProgressID
    {
        get { return decimal.Parse(Request.QueryString["progressId"].ToString()); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Items.Clear();
        uiBlError.Visible = false;

        if (!Page.IsPostBack)
        {
            BindData();
            uiRbnDefault.Checked = true;
            uiDtpCarryForward.SetDisabledCalendar(true);
            uiDtpCarryForward.DisabledCalendar = true;
            uiDtpCarryForward.SetCalendarValue("");
            uiTxtCarryForward.Enabled = false;
        }
    }

    protected void uiRbnDefault_CheckedChanged(object sender, EventArgs e)
    {
        uiDtpCarryForward.SetDisabledCalendar(true);
        uiDtpCarryForward.DisabledCalendar = true;
        uiTxtCarryForward.Enabled = false;
        uiDtpCarryForward.SetCalendarValue("");
    }

    protected void uiRbnCarryForward_CheckedChanged(object sender, EventArgs e)
    {
        uiDtpCarryForward.SetDisabledCalendar(false);
        uiDtpCarryForward.DisabledCalendar = false;
        uiTxtCarryForward.Enabled = true;
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            try
            {
                string dueAction = "";
                Nullable<DateTime> carryForwardDate = null;
                if (uiRbnDefault.Checked)
                {
                    dueAction = "Default";
                }
                else
                {
                    dueAction = "CarryForward";
                }

                if (!string.IsNullOrEmpty(uiDtpCarryForward.Text))
                {
                    carryForwardDate = DateTime.Parse(uiDtpCarryForward.Text);
                }

                //DateTime currentDate = EODTradeProgress.GetDefaultDateByExchangeRef(uiTxtDueType.Text, uiTxtExchangeRef.Text).AddDays(1);  // DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"));
                DateTime currentDate = DateTime.Parse(Session["Busdate"].ToString()); //BusinessDateReport;
                EODTradeProgress.UpdateEodTradeProgress(uiTxtDueType.Text, dueAction, ProgressID,
                currentDate, carryForwardDate, uiTxtCarryForward.Text,
                this.User.Identity.Name, DateTime.Now);

                Response.Redirect("DueDateSettlement.aspx");
            }
            catch (Exception ex)
            {
                uiBlError.Items.Add(ex.Message);
                uiBlError.Visible = true;
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DueDateSettlement.aspx");
    }

    private void BindData()
    {
        try
        {
            EODTradeProgressDataTableAdapters.ViewDueDateSettlementTableAdapter ta = new EODTradeProgressDataTableAdapters.ViewDueDateSettlementTableAdapter();
            EODTradeProgressData.ViewDueDateSettlementDataTable dt = new EODTradeProgressData.ViewDueDateSettlementDataTable();
            EODTradeProgressData.ViewDueDateSettlementRow dr = null;

            ta.FillByProgressID(dt, ProgressID);
            if (dt.Count > 0)
            {
                dr = dt[0];
                uiTxtBusinessDate.Text = dr.BusinessDate.ToString("dd-MM-yyyy");
                uiTxtTradedPrice.Text = dr.Price.ToString("#,##0.00");
                uiTxtQuantity.Text = dr.Volume.ToString("#,##0.00");
                uiTxtExchangeRef.Text = dr.ExchangeRef;
                uiTxtSellerCode.Text = dr.SellerId;
                uiTxtBuyerCode.Text = dr.BuyerId;
                uiTxtContractMonth.Text = dr.ContractMonth;
                if (!dr.IsBuyerOutstandingNull())
                {
                    uiTxtOutstanding.Text = dr.BuyerOutstanding.ToString("#,##0.00");
                }
                uiTxtDueType.Text = dr.DueType;
                if (dr.DueType.Equals("Fulfillment"))
                {
                    uiLblDueType.Text = "Seller Due";
                    uiTrCarryForward.Visible = false;
                    uiTrCarryForwardNotes.Visible = false;
                }
                else
                {
                    uiLblDueType.Text = "Buyer Due";
                    uiTrCarryForward.Visible = true;
                    uiTrCarryForwardNotes.Visible = true;

                    EODTradeProgressData.EODTradeProgressRow drEodTradeProgres = EODTradeProgress.GetEodTradeProgressByExchangeRef(dr.ExchangeRef);
                    if (drEodTradeProgres != null)
                    {
                        if (!drEodTradeProgres.IsSellerCarryForwardNull())
                        {
                            uiTrTxtCarryForward.Text = drEodTradeProgres.SellerCarryForward.ToString("dd-MM-yyyy");
                        }
                        if (!drEodTradeProgres.IsSellerCarryForwardNoteNull())
                        {
                            uiTrTxtCarryForwardNote.Text = drEodTradeProgres.SellerCarryForwardNote;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        bool isHoliday = true;
        uiBlError.Items.Clear();
        uiBlError.Visible = false;

        if (uiRbnCarryForward.Checked)
        {
            if (string.IsNullOrEmpty(uiDtpCarryForward.Text))
            {
                uiBlError.Items.Add("Carry Forward date is required.");
            }
            else
            {
                isHoliday = Holiday.ValidHoliday(DateTime.Parse(uiDtpCarryForward.Text));
                if (isHoliday == false)
                {
                    uiBlError.Items.Add("Carry Forward date is Holiday .");
                }
            }
            //else
            //{
            //    if (DateTime.Parse(uiDtpCarryForward.Text) < DateTime.Today)
            //    {
            //        uiBlError.Items.Add("Carry Forward date cannot be less than today.");
            //    }
            //}
        }

        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }
}