using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_ApprovalSI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Items.Clear();
        uiBlError.Visible = false;
        noSi.Visible = false;
        SetAccessPage();

        if (!Page.IsPostBack)
        {
            if (eType == "edit")
            {
                BindData();
            }
            else if (eType == "adjust")
            {
                BindData();
            }
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            try
            {
                if (eType == "add")
                {
                    decimal tradeFeedId = Tradefeed.GetMaxTradeFeedIDByBusinessDate(DateTime.Parse(uiDtpBussDate.Text));
                    TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();
                    TradefeedData.TradeFeedRow dr = null;
                    dr = dt.NewTradeFeedRow();
                    dr.ExchangeId = decimal.Parse(uiDdlExchange.SelectedValue);
                    dr.TradeFeedID = tradeFeedId;
                    dr.BusinessDate = DateTime.Parse(uiDtpBussDate.Text).Date;
                    TimeSpan tsTransTime = TimeSpan.Parse(uiTxtTransTime.Text);
                    TimeSpan tsReceiveTime = TimeSpan.Parse(uiTxtReceiveTime.Text);
                    dr.TradeTime = DateTime.Parse(uiDtpTradeTIme.Text).Date.Add(tsTransTime);
                    dr.TradeReceivedTime = DateTime.Parse(uiDtpTradeReceived.Text).Date.Add(tsReceiveTime);
                    if (!string.IsNullOrEmpty(uiTxbTradeTimeOfset.Text))
                    {
                        dr.TradeTimeOffset = int.Parse(uiTxbTradeTimeOfset.Text);
                    }
                    dr.TradeOptType = uiTxbTradeOpt.Text;
                    dr.ContractID = decimal.Parse(uiCtlContract.LookupTextBoxID);
                    dr.Price = decimal.Parse(uiTxbPrice.Text);
                    dr.Quantity = int.Parse(uiTxbQuantity.Text);
                    if (!string.IsNullOrEmpty(uiTxbContractIndicator.Text))
                    {
                        dr.ContraIndicator = uiTxbContractIndicator.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbExchangeReference.Text))
                    {
                        dr.ExchangeRef = uiTxbExchangeReference.Text;
                    }
                    dr.SellerCMID = decimal.Parse(uiCtlSellerCM.LookupTextBoxID);
                    dr.SellerEMID = decimal.Parse(uiCtlSellerEM.LookupTextBoxID);
                    if (!string.IsNullOrEmpty(uiTxbSellerInvestorGiveUpCode.Text))
                    {
                        dr.SellerInvGiveUpCode = uiTxbSellerInvestorGiveUpCode.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerGiveUpComm.Text))
                    {
                        dr.SellerGiveUpComm = decimal.Parse(uiTxbSellerGiveUpComm.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerReference.Text))
                    {
                        dr.SellerRef = uiTxbSellerReference.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerTradeType.Text))
                    {
                        dr.SellerTrdType = uiTxbSellerTradeType.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerComTradeType.Text))
                    {
                        dr.SellCompTradeType = uiTxbSellerComTradeType.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerTotalLeg.Text))
                    {
                        dr.SellerTotalLeg = int.Parse(uiTxbSellerTotalLeg.Text);
                    }
                    dr.BuyerInvID = decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID);
                    if (!string.IsNullOrEmpty(uiTxbBuyerTradeType.Text))
                    {
                        dr.BuyerTrdType = uiTxbBuyerTradeType.Text;
                    }
                    dr.BuyerCMID = decimal.Parse(uiCtlSellerCM.LookupTextBoxID);
                    dr.BuyerEMID = decimal.Parse(uiCtlSellerEM.LookupTextBoxID);
                    if (!string.IsNullOrEmpty(uiTxbBuyerInvestorGiveUpCode.Text))
                    {
                        dr.BuyerInvGiveUpCode = uiTxbBuyerInvestorGiveUpCode.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerGiveUpCom.Text))
                    {
                        dr.BuyerGiveUpComm = decimal.Parse(uiTxbBuyerGiveUpCom.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerCompTradeTradeType.Text))
                    {
                        dr.BuyCompTradeType = uiTxbBuyerCompTradeTradeType.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerReference.Text))
                    {
                        dr.BuyerRef = uiTxbBuyerReference.Text;
                    }
                    if (!string.IsNullOrEmpty(uiTxbTradeSrike.Text))
                    {
                        dr.TradeStrikePrice = decimal.Parse(uiTxbTradeSrike.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbTradeVersion.Text))
                    {
                        dr.TradeVersion = int.Parse(uiTxbTradeVersion.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerTotalLeg.Text))
                    {
                        dr.BuyTotLeg = int.Parse(uiTxbBuyerTotalLeg.Text);
                    }
                    dr.SellerInvID = decimal.Parse(uiCtlSellerINvestor.LookupTextBoxID);
                    dr.CreatedBy = User.Identity.Name;
                    dr.CreatedDate = DateTime.Now;
                    dr.LastUpdatedBy = User.Identity.Name;
                    dr.LastUpdatedDate = DateTime.Now;
                    dt.AddTradeFeedRow(dr);

                    Tradefeed.InsertTradeFeed(dt);

                }
                else if (eType == "edit" || eType == "adjust")
                {
                    Nullable<int> timeOffSet = null;
                    Nullable<decimal> sellerGiveUpComm = null; ;
                    Nullable<int> sellerTotalLeg = null; ;
                    Nullable<decimal> buyerGiveUpComm = null; ;
                    Nullable<int> buyerTotalLeg = null; ;
                    Nullable<decimal> tradeStrike = null; ;
                    Nullable<int> tradeVersion = null; ;
                    if (!string.IsNullOrEmpty(uiTxbTradeTimeOfset.Text))
                    {
                        timeOffSet = int.Parse(uiTxbTradeTimeOfset.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerGiveUpComm.Text))
                    {
                        sellerGiveUpComm = decimal.Parse(uiTxbSellerGiveUpComm.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbSellerTotalLeg.Text))
                    {
                        sellerTotalLeg = int.Parse(uiTxbSellerTotalLeg.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerGiveUpCom.Text))
                    {
                        buyerGiveUpComm = decimal.Parse(uiTxbBuyerGiveUpCom.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbBuyerTotalLeg.Text))
                    {
                        buyerTotalLeg = int.Parse(uiTxbBuyerTotalLeg.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbTradeSrike.Text))
                    {
                        tradeStrike = decimal.Parse(uiTxbTradeSrike.Text);
                    }
                    if (!string.IsNullOrEmpty(uiTxbTradeVersion.Text))
                    {
                        tradeVersion = int.Parse(uiTxbTradeVersion.Text);
                    }

                    TimeSpan tsTransTime = TimeSpan.Parse(uiTxtTransTime.Text);
                    TimeSpan tsReceiveTime = TimeSpan.Parse(uiTxtReceiveTime.Text);

                    Tradefeed.UpdateTradeFeed(decimal.Parse(uiDdlExchange.SelectedValue), decimal.Parse(uiTxbTradeFeedID.Text),
                      DateTime.Parse(uiDtpBussDate.Text), DateTime.Parse(uiDtpTradeTIme.Text).Add(tsTransTime),
                      DateTime.Parse(uiDtpTradeReceived.Text).Add(tsReceiveTime), timeOffSet,
                      uiTxbTradeOpt.Text, decimal.Parse(uiCtlContract.LookupTextBoxID),
                      decimal.Parse(uiTxbPrice.Text), int.Parse(uiTxbQuantity.Text),
                      uiTxbContractIndicator.Text, uiTxbExchangeReference.Text,
                      decimal.Parse(uiCtlSellerCM.LookupTextBoxID), uiTxbSellerInvestorGiveUpCode.Text,
                      sellerGiveUpComm, uiTxbSellerReference.Text,
                      uiTxbSellerTradeType.Text, uiTxbSellerComTradeType.Text, sellerTotalLeg,
                      decimal.Parse(uiCtlBuyerInvestor.LookupTextBoxID), uiTxbBuyerTradeType.Text, uiTxbBuyerInvestorGiveUpCode.Text,
                      buyerGiveUpComm, uiTxbBuyerCompTradeTradeType.Text, uiTxbBuyerReference.Text,
                      tradeStrike, tradeVersion,
                      buyerTotalLeg, decimal.Parse(uiCtlSellerINvestor.LookupTextBoxID),
                      decimal.Parse(uiCtlBuyerCM.LookupTextBoxID), uiCtlSellerEM.LookupTextBoxID, decimal.Parse(uiCtlSellerEM.LookupTextBoxID),
                      decimal.Parse(uiCtlBuyerEM.LookupTextBoxID), "A", "",
                      CreatedBy, CreatedDate, User.Identity.Name, DateTime.Now);
                }

                if (eType == "edit")
                {
                    Response.Redirect("ShippingInstruction.aspx");
                }
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
        if (eType == "edit")
        {
            Response.Redirect("ShippingInstruction.aspx");
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            Nullable<int> timeOffSet = null;
            Nullable<decimal> sellerGiveUpComm = null; ;
            Nullable<int> sellerTotalLeg = null; ;
            Nullable<decimal> buyerGiveUpComm = null; ;
            Nullable<int> buyerTotalLeg = null; ;
            Nullable<decimal> tradeStrike = null; ;
            Nullable<int> tradeVersion = null; ;
            if (!string.IsNullOrEmpty(uiTxbTradeTimeOfset.Text))
            {
                timeOffSet = int.Parse(uiTxbTradeTimeOfset.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbSellerGiveUpComm.Text))
            {
                sellerGiveUpComm = decimal.Parse(uiTxbSellerGiveUpComm.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbSellerTotalLeg.Text))
            {
                sellerTotalLeg = int.Parse(uiTxbSellerTotalLeg.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbBuyerGiveUpCom.Text))
            {
                buyerGiveUpComm = decimal.Parse(uiTxbBuyerGiveUpCom.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbBuyerTotalLeg.Text))
            {
                buyerTotalLeg = int.Parse(uiTxbBuyerTotalLeg.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbTradeSrike.Text))
            {
                tradeStrike = decimal.Parse(uiTxbTradeSrike.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbTradeVersion.Text))
            {
                tradeVersion = int.Parse(uiTxbTradeVersion.Text);
            }
            //dt no invoice
            invoiceNumberTableAdapters.no_invoiceTableAdapter ta = new invoiceNumberTableAdapters.no_invoiceTableAdapter();
            invoiceNumber.no_invoiceDataTable dt = new invoiceNumber.no_invoiceDataTable();
            dt = ta.GetDataByLastId();

            var splitNumber = dt[0].no_invoice.ToString().Split('/');
            int lastNumber = (Convert.ToInt32(splitNumber[0].ToString())) + 1;

            var number = "";

            int day = Convert.ToInt32(DateTime.Now.Day.ToString());
            int year = Convert.ToInt32(DateTime.Now.Year.ToString());
            int month = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (year > Convert.ToInt32(splitNumber[4]))
            {
                number = "001";
                ta.Insert(number + "/KBI/TIMAH/" + month + "/" + year, noSi.Text);
            }
            else
            {
                number = lastNumber.ToString("D3");
                ta.Insert(number + "/KBI/TIMAH/" + month + "/" + year, noSi.Text);
            }

            Tradefeed.ApproveShippingInstruction(decimal.Parse(uiDdlExchange.SelectedValue), uiTxbExchangeReference.Text, DateTime.Parse(uiDtpBussDate.Text), CreatedBy, "A");

            Response.Redirect("ShippingInstruction.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            Nullable<int> timeOffSet = null;
            Nullable<decimal> sellerGiveUpComm = null; ;
            Nullable<int> sellerTotalLeg = null; ;
            Nullable<decimal> buyerGiveUpComm = null; ;
            Nullable<int> buyerTotalLeg = null; ;
            Nullable<decimal> tradeStrike = null; ;
            Nullable<int> tradeVersion = null; ;
            if (!string.IsNullOrEmpty(uiTxbTradeTimeOfset.Text))
            {
                timeOffSet = int.Parse(uiTxbTradeTimeOfset.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbSellerGiveUpComm.Text))
            {
                sellerGiveUpComm = decimal.Parse(uiTxbSellerGiveUpComm.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbSellerTotalLeg.Text))
            {
                sellerTotalLeg = int.Parse(uiTxbSellerTotalLeg.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbBuyerGiveUpCom.Text))
            {
                buyerGiveUpComm = decimal.Parse(uiTxbBuyerGiveUpCom.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbBuyerTotalLeg.Text))
            {
                buyerTotalLeg = int.Parse(uiTxbBuyerTotalLeg.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbTradeSrike.Text))
            {
                tradeStrike = decimal.Parse(uiTxbTradeSrike.Text);
            }
            if (!string.IsNullOrEmpty(uiTxbTradeVersion.Text))
            {
                tradeVersion = int.Parse(uiTxbTradeVersion.Text);
            }

            Tradefeed.ApproveShippingInstruction(decimal.Parse(uiDdlExchange.SelectedValue), uiTxbExchangeReference.Text, DateTime.Parse(uiDtpBussDate.Text), CreatedBy, "R");

            Response.Redirect("ShippingInstruction.aspx");
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBlError.Items.Clear();
        uiBlError.Visible = false;

        if (string.IsNullOrEmpty(uiDtpBussDate.Text))
        {
            uiBlError.Items.Add("Busines date is required.");
        }
        if (string.IsNullOrEmpty(uiDtpTradeTIme.Text))
        {
            uiBlError.Items.Add("Trade date is required.");
        }
        if (string.IsNullOrEmpty(uiTxtTransTime.Text))
        {
            uiBlError.Items.Add("Trade time is required.");
        }
        else
        {
            string[] strTime = uiTxtTransTime.Text.Split(":".ToCharArray());
            if (int.Parse(strTime[0]) < 0 || int.Parse(strTime[0]) > 23)
            {
                uiBlError.Items.Add("Invalid trade hour.");
            }
            if (int.Parse(strTime[1]) < 0 || int.Parse(strTime[1]) > 59)
            {
                uiBlError.Items.Add("Invalid trade minute.");
            }
        }
        if (string.IsNullOrEmpty(uiDtpTradeReceived.Text))
        {
            uiBlError.Items.Add("Trade receiver is required.");
        }
        if (string.IsNullOrEmpty(uiTxtReceiveTime.Text))
        {
            uiBlError.Items.Add("Receive time is required.");
        }
        else
        {
            string[] strTime = uiTxtReceiveTime.Text.Split(":".ToCharArray());
            if (int.Parse(strTime[0]) < 0 || int.Parse(strTime[0]) > 23)
            {
                uiBlError.Items.Add("Invalid receive hour.");
            }
            if (int.Parse(strTime[1]) < 0 || int.Parse(strTime[1]) > 59)
            {
                uiBlError.Items.Add("Invalid receive minute.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbTradeTimeOfset.Text))
        {
            int offsetOut;
            if (int.TryParse(uiTxbTradeTimeOfset.Text, out offsetOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for trade time offset.");
            }
        }
        if (string.IsNullOrEmpty(uiCtlContract.LookupTextBoxID))
        {
            uiBlError.Items.Add("Contract is required.");
        }
        if (string.IsNullOrEmpty(uiTxbPrice.Text))
        {
            uiBlError.Items.Add("Price is required.");
        }
        else
        {
            decimal priceOut;
            if (decimal.TryParse(uiTxbPrice.Text, out priceOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for price.");
            }
        }
        if (string.IsNullOrEmpty(uiTxbQuantity.Text))
        {
            uiBlError.Items.Add("Quantity is required.");
        }
        else
        {
            int quantityOut;
            if (int.TryParse(uiTxbQuantity.Text, out quantityOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for quantity.");
            }
        }
        if (string.IsNullOrEmpty(uiCtlSellerCM.LookupTextBoxID))
        {
            uiBlError.Items.Add("Seller clearing member is required.");
        }
        if (string.IsNullOrEmpty(uiCtlSellerEM.LookupTextBoxID))
        {
            uiBlError.Items.Add("Seller exchange member is required.");
        }
        if (string.IsNullOrEmpty(uiCtlBuyerInvestor.LookupTextBoxID))
        {
            uiBlError.Items.Add("Buyer investor is required.");
        }
        if (string.IsNullOrEmpty(uiCtlBuyerCM.LookupTextBoxID))
        {
            uiBlError.Items.Add("Buyer clearing member is required.");
        }
        if (string.IsNullOrEmpty(uiCtlBuyerEM.LookupTextBoxID))
        {
            uiBlError.Items.Add("Buyer exchange member is required.");
        }
        if (string.IsNullOrEmpty(uiCtlSellerINvestor.LookupTextBoxID))
        {
            uiBlError.Items.Add("Seller investor is required.");
        }
        if (!string.IsNullOrEmpty(uiTxbSellerTotalLeg.Text))
        {
            int sellerTotalLegOut;
            if (int.TryParse(uiTxbSellerTotalLeg.Text, out sellerTotalLegOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for Seller total leg.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbTradeVersion.Text))
        {
            int tradeVersionOut;
            if (int.TryParse(uiTxbTradeVersion.Text, out tradeVersionOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for trade version.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbBuyerTotalLeg.Text))
        {
            int buyerTotalLegOut;
            if (int.TryParse(uiTxbBuyerTotalLeg.Text, out buyerTotalLegOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for buyer total leg.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbSellerGiveUpComm.Text))
        {
            decimal sellerGiveUpCommOut;
            if (decimal.TryParse(uiTxbSellerGiveUpComm.Text, out sellerGiveUpCommOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for seller give up comm.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbBuyerGiveUpCom.Text))
        {
            decimal buyerGiveUpCommOut;
            if (decimal.TryParse(uiTxbBuyerGiveUpCom.Text, out buyerGiveUpCommOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for buyer give up comm.");
            }
        }
        if (!string.IsNullOrEmpty(uiTxbTradeSrike.Text))
        {
            decimal tradeStrikeOut;
            if (decimal.TryParse(uiTxbTradeSrike.Text, out tradeStrikeOut) == false)
            {
                uiBlError.Items.Add("Invalid numeric for trade strike.");
            }
        }
        
        if (uiBlError.Items.Count > 0)
        {
            isValid = false;
            uiBlError.Visible = true;
        }

        return isValid;
    }

    private void SetEnabledControls(bool b)
    {
        uiTxbTradeFeedID.Enabled = b;
        uiDtpBussDate.SetDisabledCalendar(!b);
        uiDdlExchange.Enabled = b;
        uiDtpTradeTIme.DisabledCalendar = !b;
        uiTxtTransTime.Enabled = b;
        uiDtpTradeReceived.DisabledCalendar = !b;
        uiTxtReceiveTime.Enabled = b;
        uiTxbTradeTimeOfset.Enabled = b;
        uiTxbTradeOpt.Enabled = b;
        uiCtlContract.DisabledLookupButton = !b;
        uiTxbPrice.Enabled = b;
        uiTxbQuantity.Enabled = b;
        uiTxbContractIndicator.Enabled = b;
        uiTxbExchangeReference.Enabled = b;
        uiCtlSellerCM.DisabledLookupButton = !b;
        uiCtlSellerEM.SetDisabledExchangeMember(!b);
        uiTxbSellerInvestorGiveUpCode.Enabled = b;
        uiTxbSellerGiveUpComm.Enabled = b;
        uiTxbSellerReference.Enabled = b;
        uiTxbSellerTradeType.Enabled = b;
        uiTxbSellerComTradeType.Enabled = b;
        uiCtlBuyerInvestor.SetDisabledInvestor(!b);
        uiTxbBuyerTradeType.Enabled = b;
        uiCtlBuyerCM.DisabledLookupButton = !b;
        uiCtlBuyerEM.SetDisabledExchangeMember(!b);
        uiTxbBuyerInvestorGiveUpCode.Enabled = b;
        uiTxbBuyerGiveUpCom.Enabled = b;
        uiTxbBuyerCompTradeTradeType.Enabled = b;
        uiTxbBuyerReference.Enabled = b;
        uiTxbTradeSrike.Enabled = b;
        uiTxbTradeVersion.Enabled = b;
        uiTxbBuyerTotalLeg.Enabled = b;
        uiCtlSellerINvestor.SetDisabledInvestor(!b);
        uiTxbSellerTotalLeg.Enabled = b;
    }

    private void SetVisibleControls(bool b)
    {
        uiBtnSave.Visible = b;
        uiBtnApprove.Visible = b;
        uiBtnReject.Visible = b;
    }

    private void BindData()
    {
        try
        {
            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

            dt = ta.GetDataByPrimaryKey(ExchangeID, TradeFeedID, BusinessDate, ApprovalStatus);

            if (dt.Count > 0)
            {
                noSi.Text = dt[0].NoSI;
                uiDdlExchange.SelectedValue = dt[0].ExchangeId.ToString();
                uiTxbTradeFeedID.Text = dt[0].TradeFeedID.ToString();
                uiDtpBussDate.SetCalendarValue(dt[0].BusinessDate.ToString("dd-MMM-yyyy"));
                uiDtpTradeReceived.SetCalendarValue(dt[0].TradeReceivedTime.ToString("dd-MMM-yyyy"));
                uiTxtReceiveTime.Text = dt[0].TradeReceivedTime.ToString("HH:mm");
                uiDtpTradeTIme.SetCalendarValue(dt[0].TradeTime.ToString("dd-MMM-yyyy"));
                uiTxtTransTime.Text = dt[0].TradeTime.ToString("HH:mm");
                if (!dt[0].IsTradeTimeOffsetNull())
                {
                    uiTxbTradeTimeOfset.Text = dt[0].TradeTimeOffset.ToString();
                }
                if (!dt[0].IsTradeOptTypeNull())
                {
                    uiTxbTradeOpt.Text = dt[0].TradeOptType.ToString();
                }

                uiCtlContract.SetContractCommodityValue(dt[0].ContractID.ToString(), Contract.GetContractByContractID2(dt[0].ContractID).CommName);
                uiTxbPrice.Text = dt[0].Price.ToString("#,##0.##");
                uiTxbQuantity.Text = dt[0].Quantity.ToString();
                if (!dt[0].IsContraIndicatorNull())
                {
                    uiTxbContractIndicator.Text = dt[0].ContraIndicator.ToString();
                }
                if (!dt[0].IsExchangeRefNull())
                {
                    uiTxbExchangeReference.Text = dt[0].ExchangeRef.ToString();
                }
                uiCtlSellerCM.SetClearingMemberValue(dt[0].SellerCMID.ToString(), ClearingMember.GetClearingMemberCodeByClearingMemberID(dt[0].SellerCMID));
                uiCtlSellerEM.SetExchangeMemberValue(dt[0].SellerEMID.ToString(), ExchangeMember.GetExchangeMemberNameByExchangeMemberId(dt[0].SellerEMID));
                if (!dt[0].IsSellerInvGiveUpCodeNull())
                {
                    uiTxbSellerInvestorGiveUpCode.Text = dt[0].SellerInvGiveUpCode;
                }
                if (!dt[0].IsSellerGiveUpCommNull())
                {
                    uiTxbSellerGiveUpComm.Text = dt[0].SellerGiveUpComm.ToString("#,##0.##");
                }
                if (!dt[0].IsSellerRefNull())
                {
                    uiTxbSellerReference.Text = dt[0].SellerRef;
                }
                if (!dt[0].IsSellerTrdTypeNull())
                {
                    uiTxbSellerTradeType.Text = dt[0].SellerTrdType;
                }
                if (!dt[0].IsSellCompTradeTypeNull())
                {
                    uiTxbSellerComTradeType.Text = dt[0].SellCompTradeType;
                }
                if (!dt[0].IsSellerTotalLegNull())
                {
                    uiTxbSellerTotalLeg.Text = dt[0].SellerTotalLeg.ToString();
                }

                uiCtlBuyerCM.SetClearingMemberValue(dt[0].BuyerCMID.ToString(), ClearingMember.GetClearingMemberCodeByClearingMemberID(dt[0].BuyerCMID));
                uiCtlBuyerEM.SetExchangeMemberValue(dt[0].BuyerEMID.ToString(), ExchangeMember.GetExchangeMemberNameByExchangeMemberId(dt[0].BuyerEMID));

                if (!dt[0].IsBuyerInvGiveUpCodeNull())
                {
                    uiTxbBuyerInvestorGiveUpCode.Text = dt[0].BuyerInvGiveUpCode;
                }
                if (!dt[0].IsBuyerGiveUpCommNull())
                {
                    uiTxbBuyerGiveUpCom.Text = dt[0].BuyerGiveUpComm.ToString("#,##0.##");
                }
                if (!dt[0].IsBuyCompTradeTypeNull())
                {
                    uiTxbBuyerCompTradeTradeType.Text = dt[0].BuyCompTradeType;
                }
                if (!dt[0].IsBuyerRefNull())
                {
                    uiTxbBuyerReference.Text = dt[0].BuyerRef;
                }
                if (!dt[0].IsTradeStrikePriceNull())
                {
                    uiTxbTradeSrike.Text = dt[0].TradeStrikePrice.ToString("#,##0.##");
                }
                if (!dt[0].IsTradeVersionNull())
                {
                    uiTxbTradeVersion.Text = dt[0].TradeVersion.ToString();
                }
                if (!dt[0].IsBuyTotLegNull())
                {
                    uiTxbBuyerTotalLeg.Text = dt[0].BuyTotLeg.ToString();
                }
                uiCtlSellerINvestor.SetInvestorValue(dt[0].SellerInvID.ToString(), Investor.GetNameInvestorByInvestorID(dt[0].SellerInvID));
                uiCtlBuyerInvestor.SetInvestorValue(dt[0].BuyerInvID.ToString(), Investor.GetNameInvestorByInvestorID(dt[0].BuyerInvID));
                CreatedBy = dt[0].CreatedBy;
                CreatedDate = dt[0].CreatedDate;

                uiLblShippingInstr.Text = dt[0].ShippingInstructionUrl;
            }
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnSave.Visible = mp.IsMaker;
        //uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;

        SetEnabledControls(false);

    }

    protected void uiBtnShippingInstr_Click(object sender, EventArgs e)
    {
        if (uiLblShippingInstr.Text != "" && uiLblShippingInstr.Text != null)
        {
            ShowFile(@"F:/Document/" + uiLblShippingInstr.Text);
        }
    }

    private void ShowFile(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = ReturnExtension(file.Extension.ToLower());
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }

    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    private decimal ExchangeID
    {
        get { return decimal.Parse(Request.QueryString["exchangeId"].ToString()); }
    }

    private decimal TradeFeedID
    {
        get { return decimal.Parse(Request.QueryString["tradeFeedId"].ToString()); }
    }

    private DateTime BusinessDate
    {
        get { return DateTime.Parse(Request.QueryString["businessDate"]); }
    }

    private string ApprovalStatus
    {
        get { return Request.QueryString["approvalStatus"]; }
    }

    private string eType
    {
        get { return Request.QueryString["eType"]; }
    }

    private string CreatedBy
    {
        get { return ViewState["CreatedBy"].ToString(); }
        set { ViewState["CreatedBy"] = value; }
    }

    private DateTime CreatedDate
    {
        get { return (DateTime)ViewState["CreatedDate"]; }
        set { ViewState["CreatedDate"] = value; }
    }
}