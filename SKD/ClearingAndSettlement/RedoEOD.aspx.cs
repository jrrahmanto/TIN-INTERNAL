using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_RedoEOD : System.Web.UI.Page
{
    private DateTime NewBusinessDate
    {
        get { return (DateTime)ViewState["NewBusinessDate"]; }
        set { ViewState["NewBusinessDate"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLErrorSetParameter.Visible = false;
        uiBLErrorSetParameter.Items.Clear();
        uiBLErrorSendFile.Visible = false;
        uiBLErrorSendFile.Items.Clear();
    }

    protected void uiWzStartOfDay_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (uiWzStartOfDay.ActiveStepIndex == 0)
        {
            uiBLErrorSetParameter.Visible = false;
            uiBLErrorSetParameter.Items.Clear();
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                uiBLErrorSetParameter.Visible = true;
                uiBLErrorSetParameter.Items.Add("Business date is required");
                e.Cancel = true;
            }
            else
            {
                
            }
        }
    }

    protected void uiWzStartOfDay_PreviousButtonClick(object sender, WizardNavigationEventArgs e){ }

    protected void uiWzStartOfDay_ActiveStepChanged(object sender, EventArgs e){ }

    protected void uiWzStartOfDay_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        string hostName = System.Net.Dns.GetHostName();
        string ipAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

        try
        {
            ProcessStartOfDay();
            ApplicationLog.Insert(DateTime.Now, "Start Of Day", "I", "Process Start Of Day", User.Identity.Name, Common.GetIPAddress(this.Request));
            EventType.AddEarlyWarningContractMessage(NewBusinessDate);
            Session["BusinessDate"] = NewBusinessDate;

            EventType.AddEventQueue("SOD Status",
               string.Format("%SOD_DATE_TIME%={0};%SOD_STATUS%={1}", NewBusinessDate.ToString("dd/MM/yyyy"), "succeded"), ipAddress);
        }
        catch (Exception ex)
        {
            uiBLErrorSendFile.Visible = true;
            uiBLErrorSendFile.Items.Add(ex.Message);
            e.Cancel = true;
            ApplicationLog.Insert(DateTime.Now, "Start Of Day", "E", "Process Start Of Day Failed", User.Identity.Name, Common.GetIPAddress(this.Request));
            EventType.AddEventQueue("SOD Status",
                string.Format("%SOD_DATE_TIME%={0};%SOD_STATUS%={1}", NewBusinessDate.ToString("dd/MM/yyyy"), "failed"), ipAddress);
        }
    }

    private void ProcessStartOfDay()
    {
        //Update business Date
        try
        {
            Parameter.UpdateParameterDateValueByCode(Parameter.PARAM_BUSINESS_DATE,
                DateTime.Parse(CtlCalendarPickUp1.Text), User.Identity.Name, DateTime.Now);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to update parameter business date: " + ex.Message, ex);
        }

        //Process outgoing feed
        try
        {
            EOD.ProcessOutgoingFeed();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to process outgoing feed: " + ex.Message, ex);
        }

        try
        {
            DoLogin();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to login: " + ex.Message, ex);
        }


        //get and process high low price
        try
        {
            GetAndProcessHighLowPrice();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to get and process high low price: " + ex.Message, ex);
        }

        //get and process seller allocation
        try
        {
            GetAndProcessSellerAllocation(NewBusinessDate);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to get and process seller allocation: " + ex.Message, ex);
        }

        try
        {
            SOD.ProcessSOD(NewBusinessDate, User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to process SOD: " + ex.Message, ex);
        }
    }

    private void DoLogin()
    {
        
    }

    private void GetAndProcessHighLowPrice()
    {
        
    }

    private void GetAndProcessSellerAllocation(DateTime businessDate)
    {
        
    }
}