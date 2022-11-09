using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_EntryTradeFeedException : System.Web.UI.Page
{
    private decimal ExchangeID
    {
        get { return decimal.Parse(Request.QueryString["exchangeId"]); }
    }

    private decimal TradeFeedID
    { 
        get {return decimal.Parse(Request.QueryString["tradeFeedId"]); }
    }

    private DateTime BusinessDate
    {
        get { return DateTime.Parse(Request.QueryString["businessDate"]); }
    }

    private string ApprovalStatus
    {
        get { return Request.QueryString["approvalStatus"]; }
    }

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetAccessPage();
            // uiBLError.Visible = false;
            //uiBLError.Items.Clear();
        }
         catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Menu))
        {
            Response.Redirect("ViewTradeFeedException.aspx");
        }
        else
        {
            Response.Redirect("ViewTradeFeedException.aspx?menu=hide");
        }       
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        { 
            TradefeedException _tfExceptionHandler = new TradefeedException();

            try
            {
                _tfExceptionHandler.Approve(ExchangeID,
                    BusinessDate,
                    TradeFeedID, User.Identity.Name);

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTradeFeedException.aspx");
                }
                else
                {
                    Response.Redirect("ViewTradeFeedException.aspx?menu=hide");
                }       
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }        
        }        
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            TradefeedException _tfExceptionHandler = new TradefeedException();
            try
            {
                _tfExceptionHandler.Reject(ExchangeID,
                                       BusinessDate,
                                       TradeFeedID, User.Identity.Name);

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTradeFeedException.aspx");
                }
                else
                {
                    Response.Redirect("ViewTradeFeedException.aspx?menu=hide");
                }       
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }        
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        TradefeedException _tfExceptionHandler = new TradefeedException();
        TradefeedData.TradefeedExceptionDataTable dt =  _tfExceptionHandler.GetData(ExchangeID, BusinessDate, TradeFeedID);
        
        if (dt[0].ApprovalStatus == "A")
        {
            uiBLError.Items.Add("This record has been approved.");
        }
        else if (dt[0].ApprovalStatus == "R")
        {
            uiBLError.Items.Add("This record has been rejected.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
               //nothing
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
