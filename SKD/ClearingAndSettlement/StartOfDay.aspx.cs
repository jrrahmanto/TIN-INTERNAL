using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Configuration;

public partial class ClearingAndSettlement_StartOfDay : System.Web.UI.Page
{
    private DateTime NewBusinessDate
    {
        get { return (DateTime)ViewState["NewBusinessDate"]; }
        set { ViewState["NewBusinessDate"] = value; }
    }


    string pkjBaseUrl = ConfigurationManager.AppSettings["PKJ_BASE_URL"];
    string pkjUserName = ConfigurationManager.AppSettings["PKJ_USERNAME"];
    string pkjPassword = ConfigurationManager.AppSettings["PKJ_PASSWORD"];

    string pkjAccessToken = ConfigurationManager.AppSettings["PJK_ACCESS_TOKEN"];


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

                if (!cbRetrieveData.Checked)
                {
                    if (EOD.IsHoliday(DateTime.Parse(CtlCalendarPickUp1.Text)))
                    {
                        uiBLErrorSetParameter.Visible = true;
                        uiBLErrorSetParameter.Items.Add("The date you are input is holiday");
                        e.Cancel = true;
                    }
                    else
                    {
                        Nullable<DateTime> businessDate = null;
                        businessDate = Parameter.GetParameterBusinessDate();
                        if (businessDate.HasValue)
                        {
                            uiBLErrorSetParameter.Visible = true;
                            uiBLErrorSetParameter.Items.Add("Start of Day is already processed");
                            e.Cancel = true;
                        }
                        else
                        {
                            NewBusinessDate = DateTime.Parse(CtlCalendarPickUp1.Text);
                        }
                    }
                }
                else
                    NewBusinessDate = DateTime.Parse(CtlCalendarPickUp1.Text);

            }
        }
    }

    protected void uiWzStartOfDay_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {

    }

    protected void uiWzStartOfDay_ActiveStepChanged(object sender, EventArgs e)
    {

    }

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
        //Execute EOD when there is holiday between Max BusinesDate ContractPosition and input date
        //DateTime lastEOD = Parameter.GetParameterLastEOD().Value;
        DateTime newBusinessDate = DateTime.Parse(CtlCalendarPickUp1.Text);
        if (!cbRetrieveData.Checked)
        {
            for (DateTime lastEOD = Parameter.GetParameterLastEOD().Value; lastEOD < newBusinessDate.AddDays(-1); lastEOD = lastEOD.AddDays(1))
            {
                //EOD.ProcessEOD(lastEOD.AddDays(1).Date, "EOD", "GLOBAL", null, null, null, null, User.Identity.Name);
                EOD.ProcessEOD(lastEOD.AddDays(1).Date, "EOD",
                    "GLOBAL", null, null, null,
                    User.Identity.Name, Common.GetIPAddress(this.Request),true);

                //Set LastEOD = Input Date
                Parameter.UpdateParameterDateValueByCode("LastEOD", lastEOD.AddDays(1).Date, User.Identity.Name, DateTime.Now);
                SOD.ProcessSOD(newBusinessDate, User.Identity.Name, Common.GetIPAddress(this.Request));
            }
        }

        //Update business Date
        try
        {
            Parameter.UpdateParameterDateValueByCode(Parameter.PARAM_BUSINESS_DATE, DateTime.Parse(CtlCalendarPickUp1.Text), User.Identity.Name, DateTime.Now);
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

        //Process SOD
        try
        {
            SOD.ProcessSOD(newBusinessDate, User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to process SOD: " + ex.Message, ex);
        }

        //Get Staging BGR
        //try
        //{
        //    SellerAllocationStagingDataTableAdapters.SellerAllocationTableAdapter sasta = new SellerAllocationStagingDataTableAdapters.SellerAllocationTableAdapter();
        //    SellerAllocationStagingData.SellerAllocationDataTable selldt = new SellerAllocationStagingData.SellerAllocationDataTable();

        //    SellerAllocationDataTableAdapters.SellerAllocationTableAdapter sata = new SellerAllocationDataTableAdapters.SellerAllocationTableAdapter();
        //    SellerAllocationData.SellerAllocationDataTable sadt = new SellerAllocationData.SellerAllocationDataTable();

        //    sasta.FillByBusinessDate(selldt, newBusinessDate);

        //    foreach(SellerAllocationStagingData.SellerAllocationRow saRow in selldt.Rows){
        //        sata.Insert(newBusinessDate, saRow.sellerid, saRow.productid, int.Parse(saRow.volume), 0, User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, saRow.sellerid + "4577");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new ApplicationException("Failed to get DB Staging: " + ex.Message, ex);
        //}
    }
}
