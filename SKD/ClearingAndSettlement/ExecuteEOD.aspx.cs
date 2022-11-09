using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections;

[Serializable]
public partial class WebUI_ClearingAndSettlement_ExecuteEOD : System.Web.UI.Page
{

    private int Revision
    {
        get { return (int)ViewState["Revision"]; }
        set { ViewState["Revision"] = value; }
    }

    public DateTime BusinessDateReport
    {
        get { return (DateTime)ViewState["BusinessDateReport"]; }
        set { ViewState["BusinessDateReport"] = value; }
    }

    private string Folder
    {
        get { return (string)ViewState["Folder"]; }
        set { ViewState["Folder"] = value; }
    }

    private ClearingMemberData.ClearingMemberDataTable dtCM
    {
        get { return (ClearingMemberData.ClearingMemberDataTable)ViewState["dtCM"]; }
        set { ViewState["dtCM"] = value; }
    }

    private StringBuilder LogReport
    {
        get { return (StringBuilder)ViewState["LogReport"]; }
        set { ViewState["LogReport"] = value; }
    }

    private int WorkThread
    {
        get { return (int)ViewState["WorkThread"]; }
        set { ViewState["WorkThread"] = value; }
    }

    private int CompleteThread
    {
        get { return (int)ViewState["CompleteThread"]; }
        set { ViewState["CompleteThread"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLWizard1.Visible = false;
        uiBLWizard1.Items.Clear();
        //uiBLWizard2.Visible = false;
        //uiBLWizard2.Items.Clear();
        uibtnGenerate.Visible = false;

        if (!Page.IsPostBack)
        {
            this.Session["LongActionProgress"] = 0;

            LogReport = new StringBuilder();

        }
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);

        //swLog.Close();

        //if (Session["IsReRunRandomize"] != null)
        //{
        //    Session.Remove("IsReRunRandomize");
        //}
        if (Session["TradeDate"] != null)
        {
            Session.Remove("TradeDate");
        }
        if (Session["StartDate"] != null)
        {
            Session.Remove("StartDate");
        }
        if (Session["EODType"] != null)
        {
            Session.Remove("EODType");
        }
    }

    protected void uiBtnRerunPrerequisites_Click(object sender, EventArgs e)
    {
        RunPrerequisites();
    }

    protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        uiLblFinish.Visible = true;
        uiBLWizardEODProgress.Visible = false;
        uiBLWizardEODProgress.Items.Clear();

        string ipAddress = "";

        try
        {
            string hostName = System.Net.Dns.GetHostName();
            ipAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

            LogReport = new StringBuilder();

            BusinessDateReport = DateTime.Parse(CtlCalendarPickUpStartDate.Text);

            ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("Start of process EOD {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));

            ExecuteEOD();

            TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
            TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

            ta.FillByBusinessDateApproved(dt, BusinessDateReport);

            EventType.AddEventQueue("EOD Success",
                string.Format("%EOD_DATE%={0};%NUMTRX%={1}", BusinessDateReport.ToString("dd/MM/yyyy"), dt.Count), ipAddress);
            AddQueueSummaryEOD();

            ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("End of process EOD {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            e.Cancel = true;
            uiBLWizardEODProgress.Visible = true;
            uiBLWizardEODProgress.Items.Add(ex.Message);

            EventType.AddEventQueue("EOD Failed",
                string.Format("%EOD_DATE%={0};%REASON_MESSAGE%={1}", BusinessDateReport.ToString("dd/MM/yyyy"), ex.Message), ipAddress);

        }

    }

    protected void uiWzEOD_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {

    }

    protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        BusinessDateReport = DateTime.Parse(CtlCalendarPickUpStartDate.Text);
        Session["Busdate"] = CtlCalendarPickUpStartDate.Text;
        //bool isTradeStatus = false;

        //Validation on next button event
        if (uiWzEOD.ActiveStepIndex == 0)
        {
            //swLog.WriteLine("[{0:hh:mm:ss.fff}] entering (uiWzEOD.ActiveStepIndex == 0)", DateTime.Now);
            //swLog.Flush();

            uiBLWizard1.Visible = false;
            uiBLWizard1.Items.Clear();
            if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
            {
                uiBLWizard1.Visible = true;
                uiBLWizard1.Items.Add("Business date is required.");
                e.Cancel = true;
                return;
            }
            else
            {
                //Check trade status is P 
                //isTradeStatus = EODTradeProgress.GetByBusDateAndTradeStatus(BusinessDateReport);
                //if (isTradeStatus == true)
                //{
                //     uiBLWizard1.Visible = true;
                //    uiBLWizard1.Items.Add("Please check due date settlement before running EOD");
                //    e.Cancel = true;
                //    return;
                //}
                //Check wheather is Do EOD or ReDo EOD
                Nullable<DateTime> lastEOD = Parameter.GetParameterLastEOD();
                if (lastEOD.HasValue)
                {
                    if (uiChkIsRedo.Checked)
                    {
                        if (DateTime.Parse(CtlCalendarPickUpStartDate.Text).Date > (Parameter.GetParameterLastEOD().Value.Date.AddDays(1)))
                        {
                            uiBLWizard1.Visible = true;
                            uiBLWizard1.Items.Add(string.Format("Redo EOD process only can be execute less than {0}", lastEOD.Value.AddDays(1).Date.ToString("dd-MMM-yyyy")));
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        if (DateTime.Parse(CtlCalendarPickUpStartDate.Text).Date != (Parameter.GetParameterLastEOD().Value.Date.AddDays(1)))
                        {
                            uiBLWizard1.Visible = true;
                            uiBLWizard1.Items.Add(string.Format("EOD process only can be execute on {0}", lastEOD.Value.AddDays(1).Date.ToString("dd-MMM-yyyy")));
                            e.Cancel = true;
                            return;
                        }
                    }

                    //Reset all needed session
                    Session.Remove("StartDate");
                    Session.Remove("EODType");

                    Session["IsRedo"] = uiChkIsRedo.Checked;
                    //Session["IsReRunRandomize"] = uiChkReRunRandomize.Checked;
                    if (Session["StartDate"] == null)
                    {
                        Session.Add("StartDate", CtlCalendarPickUpStartDate.Text);
                    }
                    //if (Session["EODType"] == null)
                    //{
                    //    if (uiRbnGlobal.Checked)
                    //    {
                    //        Session.Add("EODType", uiRbnGlobal.Text);
                    //    }
                    //    else if (uiRbnContract.Checked)
                    //    {
                    //        Session.Add("EODType", uiRbnContract.Text);
                    //    }
                    //    else if (uiRbnTrade.Checked)
                    //    {
                    //        Session.Add("EODType", uiRbnTrade.Text);
                    //    }
                    //}

                    uiBlContract.Visible = false;

                    //if (uiRbnGlobal.Checked)
                    //{
                    try
                    {
                        //string log = "End of Day Calculation is in progress";

                        //Label lbl = (Label)UpdateProgress1.FindControl("uiLblProgress");
                        //lbl.Text = log;


                        ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("Start of process EOD validation {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));

                        RunPrerequisites();

                        ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("End of process EOD validation {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));
                    }
                    catch (Exception ex)
                    {


                        e.Cancel = true;
                        uiBLWizard1.Visible = true;
                        uiBLWizard1.Items.Add("Failed to run prerequisites" + " - " + ex.Message);


                        return;
                    }
                    //}

                    try
                    {
                        

                        RunEODExchangeInfo();



                    }
                    catch (Exception ex)
                    {

                        e.Cancel = true;
                        uiBLWizard1.Visible = true;
                        uiBLWizard1.Items.Add("Failed to run Exchange Info" + " - " + ex.Message);

                        return;
                    }
                }
                else
                {
                    uiBLWizard1.Visible = true;
                    uiBLWizard1.Items.Add("Parameter LastEOD not set.");
                    e.Cancel = true;
                    return;
                }
            }
        }
        //else if (uiWzEOD.ActiveStepIndex == 1)
        //{

        //    //uiBLWizard2.Visible = false;
        //    //uiBLWizard2.Items.Clear();

        //    //if (uiRbnTrade.Checked)
        //    //{
        //    //    if (string.IsNullOrEmpty(CtlCalendarPickUpTradeDate.Text))
        //    //    {
        //    //        uiBLWizard2.Visible = true;
        //    //        uiBLWizard2.Items.Add("Trade date is required.");
        //    //        e.Cancel = true;
        //    //        return;
        //    //    }

        //    //    Session.Remove("TradeDate");
        //    //    if (Session["TradeDate"] == null)
        //    //    {
        //    //        Session.Add("TradeDate", CtlCalendarPickUpTradeDate.Text);
        //    //    }
        //    //}
        //    //if (uiRbnContract.Checked)
        //    //{
        //    //    string contractList = "";
        //    //    foreach (ListItem item in uiChkContract.Items)
        //    //    {
        //    //        if (item.Selected)
        //    //        {
        //    //            contractList += "," + item.Value;
        //    //        }
        //    //    }

        //    //    if (string.IsNullOrEmpty(contractList))
        //    //    {
        //    //        uiBLWizard2.Visible = true;
        //    //        uiBLWizard2.Items.Add("Contract is required.");
        //    //        e.Cancel = true;
        //    //        return;
        //    //    }
        //    //}
        //}
        else if (uiWzEOD.ActiveStepIndex == 2)
        {

            //Check if prerequisites passed or not
            int errValidation = 0;
            foreach (GridViewRow dr in uiDgPrerequisiteStatus.Rows)
            {
                if (dr.Cells[2].Text != "Passed")
                {
                    errValidation++;
                    
                }
            }

            if (errValidation > 0)
            {
                tblErrorPrerequisites.Visible = true;
                uiLblErrorPrerequisites.Text = "Please check prerequisites before running EOD.";
                e.Cancel = true;
            }

        }
        //else if (uiWzEOD.ActiveStepIndex == 3)
        //{
        //    //uiLblFinish.Visible = true;
        //    //uiBLWizardEODProgress.Visible = false;
        //    //uiBLWizardEODProgress.Items.Clear();

        //    //string ipAddress = "";

        //    //try
        //    //{
        //    //    string hostName = System.Net.Dns.GetHostName();
        //    //    ipAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

        //    //    LogReport = new StringBuilder();

        //    //    BusinessDateReport = DateTime.Parse(CtlCalendarPickUpStartDate.Text);

        //    //    ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("Start of process EOD {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));

        //    //    ExecuteEOD();

        //    //    TradefeedDataTableAdapters.TradeFeedTableAdapter ta = new TradefeedDataTableAdapters.TradeFeedTableAdapter();
        //    //    TradefeedData.TradeFeedDataTable dt = new TradefeedData.TradeFeedDataTable();

        //    //    ta.FillByBusinessDateApproved(dt, BusinessDateReport);

        //    //    EventType.AddEventQueue("EOD Success",
        //    //        string.Format("%EOD_DATE%={0};%NUMTRX%={1}", BusinessDateReport.ToString("dd/MM/yyyy"), dt.Count), ipAddress);
        //    //    AddQueueSummaryEOD();

        //    //    ApplicationLog.Insert(DateTime.Now, "EOD", "I", string.Format("End of process EOD {0}", BusinessDateReport.ToString("dd-MMM-yyyy")), User.Identity.Name, Common.GetIPAddress(this.Request));
        //    //}]
        //    //catch (Exception ex)
        //    //{
        //    //    e.Cancel = true;
        //    //    uiBLWizardEODProgress.Visible = true;
        //    //    uiBLWizardEODProgress.Items.Add(ex.Message);

        //    //    EventType.AddEventQueue("EOD Failed",
        //    //        string.Format("%EOD_DATE%={0};%REASON_MESSAGE%={1}", BusinessDateReport.ToString("dd/MM/yyyy"), ex.Message), ipAddress);

        //    //}

        //}
        //else if (uiWzEOD.ActiveStepIndex == 4)
        //{
        //    //Send File to External
        //    uiBLSendFileError.Visible = false;
        //    uiBLSendFileError.Items.Clear();
        //    try
        //    {
        //        if (!EOD.IsHoliday(DateTime.Parse(CtlCalendarPickUpStartDate.Text)))
        //        {
        //            LogReport = new StringBuilder();

        //            SendFileByFTP();

        //            if (!string.IsNullOrEmpty(LogReport.ToString()))
        //            {
        //                throw new ApplicationException(LogReport.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        e.Cancel = true;
        //        uiBLSendFileError.Visible = true;
        //        uiBLSendFileError.Items.Add(string.Format("Failed to send via ftp - {0}.", ex.Message));
        //    }
        //}

        //if (uiWzEOD.ActiveStepIndex == 0 && uiRbnGlobal.Checked)
        //{
        //    uiWzEOD.ActiveStepIndex = 2;
            
        //}
        //else if (uiWzEOD.ActiveStepIndex == 0 && !uiRbnGlobal.Checked)
        //{
        //    uiWzEOD.ActiveStepIndex = 1;
        //}

    }

    protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
    {
        //Session["StartDate"] = CtlCalendarPickUpStartDate.Text;
        ////Execute something in active changed event
        //if (uiWzEOD.ActiveStepIndex == 1)
        //{
        //    if (uiRbnContract.Checked)
        //    {
        //        uiChkContract.Visible = true;
        //        uiTbTradeDate.Visible = false;
        //    }
        //    else if (uiRbnTrade.Checked)
        //    {
        //        uiChkContract.Visible = false;
        //        uiTbTradeDate.Visible = true;
        //    }
        //}
        //else if (uiWzEOD.ActiveStepIndex == 2)
        //{

        //    if (uiRbnGlobal.Checked)
        //    {
        //        uiLblFinish.Text = "You have selected to perform End of Day recalculation for the whole Clearing Members. (Global)";
        //    }
        //    else if (uiRbnContract.Checked)
        //    {
        //        uiLblFinish.Text = "You have selected to perform End of Day recalculation for the following contracts:";
        //        uiBlContract.Visible = true;

        //        uiBlContract.Items.Clear();
        //        foreach (ListItem item in uiChkContract.Items)
        //        {
        //            if (item.Selected)
        //            {
        //                uiBlContract.Items.Add(item.Text);
        //            }
        //        }
        //    }
        //    else if (uiRbnTrade.Checked)
        //    {
        //        uiLblFinish.Text = string.Format("You have selected to perform End of Day recalculation for all the trades that has been corrected on {0}", CtlCalendarPickUpTradeDate.Text);
        //    }
        //}
        //else if (uiWzEOD.ActiveStepIndex == 4)
        //{

        //}
        //else if (uiWzEOD.ActiveStepIndex == 5)
        //{

        //}
    }

    private void RunEODExchangeInfo()
    {
        try
        {
            uiDgExchangeInfo.DataSource = ObjectDataSourceEODExchangeInfo;
            uiDgExchangeInfo.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to execute Exchange Info", ex);
        }
    }

    private void RunPrerequisites()
    {
        try
        {
            uiDgPrerequisiteStatus.DataSource = ObjectDataSourcePrerequisitesEOD;
            uiDgPrerequisiteStatus.DataBind();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void ExecuteEOD()
    {
        //Process EOD
        try
        {

            ProcessEOD();

        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to process EOD", ex);
        }

        if (!uiChkIsRedo.Checked)
        {
            //Set BusinessDate = null
            Parameter.UpdateParameterDateValueByCode("BusinessDate", null, User.Identity.Name, DateTime.Now);

            //Set LastEOD = Input Date
            Parameter.UpdateParameterDateValueByCode("LastEOD", DateTime.Parse(CtlCalendarPickUpStartDate.Text), User.Identity.Name, DateTime.Now);
        }

        //Generate EOD Reports
        try
        {
            if (!EOD.IsHoliday(DateTime.Parse(CtlCalendarPickUpStartDate.Text)))
            {
                Revision = EOD.GetMaxRevisionInvContractPositionEOD(BusinessDateReport);

                //GenerateEODReports();
               // GenerateNewEODReports();

                if (!string.IsNullOrEmpty(LogReport.ToString()))
                {
                    throw new ApplicationException(LogReport.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to generate report - " + ex.Message, ex);
        }
    }

    private void ProcessEOD()
    {
        //Session["IsReRunRandomize"] = uiChkReRunRandomize.Checked;
        EOD.ProcessEOD(DateTime.Parse(CtlCalendarPickUpStartDate.Text), "EOD",
                "GLOBAL", null, null, null,
                User.Identity.Name, Common.GetIPAddress(this.Request),
                true);

        //if (uiRbnGlobal.Checked)
        //{
        //    EOD.ProcessEOD(DateTime.Parse(CtlCalendarPickUpStartDate.Text), "EOD",
        //        uiRbnGlobal.Text.ToUpper(), null, null, null,
        //        User.Identity.Name, Common.GetIPAddress(this.Request),
        //        bool.Parse(Session["IsReRunRandomize"].ToString()));
        //}
        //else if (uiRbnContract.Checked)
        //{
        //    string contractList = "";
        //    foreach (ListItem item in uiChkContract.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            contractList += "," + item.Value;
        //        }
        //    }

        //    EOD.ProcessEOD(DateTime.Parse(CtlCalendarPickUpStartDate.Text), "EOD",
        //        uiRbnContract.Text.ToUpper(), contractList.Substring(1),
        //        null, null, User.Identity.Name, Common.GetIPAddress(this.Request),
        //        bool.Parse(Session["IsReRunRandomize"].ToString()));
        //}
        //else if (uiRbnTrade.Checked)
        //{
        //    EOD.ProcessEOD(DateTime.Parse(CtlCalendarPickUpStartDate.Text), "EOD",
        //        uiRbnTrade.Text.ToUpper(), null,
        //        DateTime.Parse(Session["TradeDate"].ToString()), null,
        //        User.Identity.Name, Common.GetIPAddress(this.Request),
        //        bool.Parse(Session["IsReRunRandomize"].ToString()));
        //}
    }

    //private static ManualResetEvent[] resetEvents;\

    private ManualResetEvent[] resetEvents
    {
        get { return (ManualResetEvent[])ViewState["resetEvents"]; }
        set { ViewState["resetEvents"] = value; }
    }

    private int? CurrentResetEvents
    {
        get { return (int)ViewState["CurrentResetEvents"]; }
        set { ViewState["CurrentResetEvents"] = value; }
    }
    private int CurrentThread
    {
        get { return (int)ViewState["CurrentThread"]; }
        set { ViewState["CurrentThread"] = value; }
    }

    private StreamWriter sw
    {
        get { return (StreamWriter)ViewState["sw"]; }
        set { ViewState["sw"] = value; }
    }

    private void GenerateEODReports()
    {
        //Get list of reports
        EODData.ReportDataTable dtListReports = EOD.GetListReports(Server.MapPath("~/App_Data/ListReports.xml"));
        DataView dv = new DataView(dtListReports);
        dv.RowFilter = "Type = 'EOD' or Type = 'EOM'";

        //Get list of Clearing Member
        dtCM = ClearingMember.GetActiveClearingMember(BusinessDateReport);

        try
        {
            CurrentThread = 0;

            //resetEvents = new ManualResetEvent[dv.Count];

            ThreadPool.SetMaxThreads(20, 1000);
            foreach (EODData.ReportRow drListReports in (EODData.ReportDataTable)dv.Table)
            {
                //Check wheather is end of month
                //to generate end of month reports
                if (drListReports.Type == "EOM")
                {
                    if (EOD.IsMonthLastDay(BusinessDateReport) == false)
                    {
                        int currentThread = CurrentThread;
                        Interlocked.Increment(ref currentThread);
                        CurrentThread = currentThread;
                        continue;
                    }
                }

                string info = string.Format("ReportName:{0}, GenerateType:{1}, StartTime:{2}", drListReports.Name,
                    drListReports.GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

                //resetEvents[CurrentResetEvents.Value] = new ManualResetEvent(false);
                //ClsEODObjAsyn cls = new ClsEODObjAsyn();
                //cls.reportListPath = drListReports;
                //cls.currentThread = CurrentResetEvents.Value;

                if (drListReports.GenerateType == "AK")
                {
                    try
                    {

                        // resetEvents[CurrentResetEvents.Value] = new ManualResetEvent(false);
                        //_list.Add(resetEvents[CurrentResetEvents.Value]);
                        //ClsEODObjAsyn cls = new ClsEODObjAsyn();
                        //cls.reportListPath = drListReports;
                        //cls.currentThread = CurrentResetEvents.Value;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAKEODReport), drListReports);
                        Thread.Sleep(500);

                        //ProcessAKEODReport(drListReports);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process AK EOD Reports in parallel", ex);
                    }
                }
                else if (drListReports.GenerateType == "All")
                {
                    try
                    {
                        //resetEvents[CurrentResetEvents.Value] = new ManualResetEvent(false);
                        //_list.Add(resetEvents[CurrentResetEvents.Value]);
                        //ClsEODObjAsyn cls = new ClsEODObjAsyn();
                        //cls.reportListPath = drListReports;
                        //cls.currentThread = CurrentResetEvents.Value;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAllEODReport), drListReports);
                        Thread.Sleep(500);
                        //ProcessAllEODReport(drListReports);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process All AK EOD Reports in parallel", ex);
                    }
                }

                //wait until current thread is finished
                //resetEvents[CurrentResetEvents.Value].WaitOne();
                //info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", drListReports.Name,
                //               drListReports.GenerateType, DateTime.Now);
                //sw.WriteLine(info);
                //sw.Flush();
                //CurrentResetEvents++;
            }

            //Looping until all thread finished
            //WaitHandle.WaitAll(resetEvents);

            //DisposeThread();


            while (dv.Count != CurrentThread)
            {
                Thread.Sleep(3000);
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        finally
        {
        }
    }

    private void ProcessAKEODReport(Object cls)//EODData.ReportRow drListReports)
    {
        foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        {
            if (((EODData.ReportRow)cls).Name == "NotaPemberitahuanMarginCall")
            {
                if (EOD.IsGenerateMarginCall(BusinessDateReport, drCM.CMID) == false)
                {
                    continue;
                }
            }

            SetEODReport((EODData.ReportRow)cls, CtlCalendarPickUpStartDate.Text, drCM.CMID, drCM.Code);
        }
        string info = "";
        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
        //foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        //{
        //    SetEODReport(((ClsEODObjAsyn)cls).reportListPath, CtlCalendarPickUpStartDate.Text, drCM.CMID, drCM.Code);
        //}
        //string info = "";
        //info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((ClsEODObjAsyn)cls).reportListPath.Name,
        //               ((ClsEODObjAsyn)cls).reportListPath.GenerateType, DateTime.Now);
        //sw.WriteLine(info);
        //sw.Flush();
        //resetEvents[((ClsEODObjAsyn)cls).currentThread].Set();        
        //CurrentResetEvents++;
        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessAllEODReport(Object cls)// EODData.ReportRow drListReports)
    {
        SetEODReport((EODData.ReportRow)cls, CtlCalendarPickUpStartDate.Text, null, null);
        string info = "";
        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
        //SetEODReport(((ClsEODObjAsyn)cls).reportListPath, CtlCalendarPickUpStartDate.Text, null, null);
        //string info = "";
        //info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((ClsEODObjAsyn)cls).reportListPath.Name,
        //               ((ClsEODObjAsyn)cls).reportListPath.GenerateType, DateTime.Now);
        //sw.WriteLine(info);
        //sw.Flush();
        //resetEvents[((ClsEODObjAsyn)cls).currentThread].Set();    
        //CurrentResetEvents++;
        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void SetEODReport(EODData.ReportRow drListReports,
        string businessDate, Nullable<decimal> cmId, string cmCode)
    {
        try
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            rp.Add(new ReportParameter("businessDate", businessDate));
            if (drListReports.GenerateType == "AK")
            {
                rp.Add(new ReportParameter("clearingMemberId", new string[] { cmId.ToString() }));
            }
            else if (drListReports.GenerateType == "All")
            {
                rp.Add(new ReportParameter("clearingMemberId", new string[] { null }));
            }

            ReportViewer rptViewer = new ReportViewer();

            rptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            rptViewer.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();

            rptViewer.ServerReport.ReportPath = drListReports.Path;
            rptViewer.ServerReport.SetParameters(rp);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            string formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE].ToString();

            byte[] bytes = rptViewer.ServerReport.Render(
              formatRender,
              null, out mimeType, out encoding, out extension,
              out streamids, out warnings);

            string filename = "";
            string filepath = "";

            string formatType = "";
            switch (formatRender.ToUpper())
            {
                case "PDF":
                    formatType = "pdf";
                    break;
                case "EXCEL":
                    formatType = "xls";
                    break;
                case "CSV":
                    formatType = "csv";
                    break;
                default:
                    formatType = "pdf";
                    break;
            }

            if (drListReports.GenerateType == "AK")
            {
                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);

                //<BusinessDate>-<version>_<Buyer.ParticipantCode>_<eDO>.<filetype>
                //<BusinessDate>-<version>_<Buyer.ParticipantCode>_<eDO>_<Seller.ParticipantCode>.<filetype>

                if (drListReports.Name == "eDO")
                {
                    EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                    filename = string.Format("{0}-{1}_{2}_{3}_{4}.{5}",
                        DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                        cmCode, drListReports.Name, cmCode, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }
                else
                {
                    EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                    filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                        DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                        cmCode, drListReports.Name, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }

                /*
                filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                    DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                    cmCode, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                */
            }
            else if (drListReports.GenerateType == "All")
            {
                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), BusinessDateReport);

                filename = string.Format("{0}-{1}_{2}.{3}",
                    DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"),
                    Revision, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
            }

            if (!File.Exists(filepath))
            {
                Common.CreateFileByFileStream(bytes, filepath);
            }
        }
        catch (Exception ex)
        {
            if (drListReports.GenerateType == "AK")
            {
                LogReport.AppendLine(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"));
            }
            else if (drListReports.GenerateType == "All")
            {
                LogReport.AppendLine(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                   drListReports.Name, drListReports.GenerateType, "", ex.Message, "<br>"));
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BusinessDateReport = DateTime.Parse(CtlCalendarPickUpStartDate.Text);
        Revision = EOD.GetMaxRevisionInvContractPositionEOD(BusinessDateReport);
       // GenerateEODReports();
       // GenerateNewEODReports();

    }

    protected void uibtnGenerate_Click(object sender, EventArgs e)
    {
        //ApplicationLog.CreateAndWriteLog("Start", "EOD");
        BusinessDateReport = DateTime.Parse(CtlCalendarPickUpStartDate.Text);
        //ApplicationLog.CreateAndWriteLog("BusinessDateReport", "EOD");
        Revision = EOD.GetMaxRevisionInvContractPositionEOD(BusinessDateReport);
        //ApplicationLog.CreateAndWriteLog("Revision", "EOD");
        GenerateNewEODReports();
        
    }
    private void AddQueueSummaryEOD()
    {

        EODData.SummaryEODTradeFeedDataTable dtTradeFeed = EOD.GetSummaryEODTradefeed(BusinessDateReport);
        foreach (EODData.SummaryEODTradeFeedRow dr in dtTradeFeed)
        {
            string hostName = System.Net.Dns.GetHostName();
            string ipAddress = Common.GetIPAddress(this.Request);

            EventType.AddEventQueue("EOD TradeFeed",
                   string.Format("%EXCHANGE_NAME%={0};%START_TIME%={1};%END_TIME%={2};" +
                   "%NUM_OF_SUCCESS_TF%={3};%NUM_OF_FAILED_TF%={4};%NUM_OF_EXCEPTION_TF%={5}",
                   dr.ExchangeName, dr.StartTime, dr.EndTime, dr.NumberSuccess, dr.NumberFailure, dr.NumberException), ipAddress);
        }

        EODData.SummaryEODSettlementPriceDataTable dtSettlementPrice = EOD.GetSummaryEODSettlementPrice(BusinessDateReport);
        foreach (EODData.SummaryEODSettlementPriceRow dr in dtSettlementPrice)
        {
            string hostName = System.Net.Dns.GetHostName();
            string ipAddress = Common.GetIPAddress(this.Request);

            EventType.AddEventQueue("EOD SP",
                       string.Format("%EXCHANGE_NAME%={0};%START_TIME%={1};%END_TIME%={2};" +
                       "%NUM_OF_SUCCESS_SP%={3};%NUM_OF_FAILED_SP%={4};%NUM_OF_EXCEPTION_SP%={5}",
                       dr.ExchangeName, dr.StartTime, dr.EndTime, dr.NumberSuccess, dr.NumberFailure, dr.NumberException), ipAddress);
        }


    }

    #region New Rpt EOD
    private void GenerateNewEODReports()
    {
        //Get list of reports
        //ApplicationLog.CreateAndWriteLog("dtListReports", "EOD");
        EODData.ReportDataTable dtListReports = EOD.GetListReports(Server.MapPath("~/App_Data/ListReports.xml"));
        DataView dv = new DataView(dtListReports);
        dv.RowFilter = "Type = 'EOD' or Type = 'EOM'";

        //Get list of Clearing Member
       // ApplicationLog.CreateAndWriteLog("dtCM", "EOD");
        dtCM = ClearingMember.GetActiveClearingMember(BusinessDateReport);
      //  ApplicationLog.CreateAndWriteLog( "row : " + dtCM.Rows.Count , "EOD");
        try
        {
            
            CurrentThread = 0;

            //resetEvents = new ManualResetEvent[dv.Count];
            ThreadPool.SetMaxThreads(20, 1000);
            foreach (EODData.ReportRow drListReports in (EODData.ReportDataTable)dv.Table)
            {
                //Check wheather is end of month
                //to generate end of month reports
                if (drListReports.Type == "EOM")
                {
                    if (EOD.IsMonthLastDay(BusinessDateReport) == false)
                    {
                        int currentThread = CurrentThread;
                        Interlocked.Increment(ref currentThread);
                        CurrentThread = currentThread;
                        continue;
                    }
                }

                string info = string.Format("ReportName:{0}, GenerateType:{1}, StartTime:{2}", drListReports.Name,
                    drListReports.GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

                if (drListReports.GenerateType == "AK")
                {
                    try
                    {
                        //ApplicationLog.CreateAndWriteLog("AK", "EOD");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessNewAKEODReport), drListReports);
                        Thread.Sleep(500);

                        //ProcessAKEODReport(drListReports);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process AK EOD Reports in parallel", ex);
                    }
                }
                else if (drListReports.GenerateType == "All")
                {
                    try
                    {
                        //ApplicationLog.CreateAndWriteLog("All", "EOD");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessNewAllEODReport), drListReports);
                        Thread.Sleep(500);
                        //ProcessAllEODReport(drListReports);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to process All AK EOD Reports in parallel", ex);
                    }
                }

                //wait until current thread is finished
                //resetEvents[CurrentResetEvents.Value].WaitOne();
                //info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", drListReports.Name,
                //               drListReports.GenerateType, DateTime.Now);
                //sw.WriteLine(info);
                //sw.Flush();
                //CurrentResetEvents++;
            }

            //Looping until all thread finished
            //WaitHandle.WaitAll(resetEvents);

            //DisposeThread();


            while (dv.Count != CurrentThread)
            {
                Thread.Sleep(3000);
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        finally
        {
        }
    }

    private void ProcessNewAKEODReport(Object cls)//EODData.ReportRow drListReports)
    {

        foreach (ClearingMemberData.ClearingMemberRow drCM in dtCM)
        {
            //ApplicationLog.CreateAndWriteLog(((EODData.ReportRow)cls).Name, "EOD");
            if (((EODData.ReportRow)cls).Name == "ListeDO")
            {
                if (RptEOD.IsGenerateMarginCall(BusinessDateReport, drCM.CMID) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "NotaPemberitahuanPembayaran")
            {
                if (RptEOD.IsGenerateNotaPemberitahuan(BusinessDateReport, drCM.CMID,drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "eDO")
            {
                
                if (RptEOD.IsGenerateeDO(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }
            if (((EODData.ReportRow)cls).Name == "TradeRegister")
            {
                if (RptEOD.IsGenerateTradeReg(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "DFS")
            {
                if (RptEOD.IsGenerateDFS(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiSeller")
            {
                if (RptEOD.IsGenerateRincianDFSSeller(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }

            if (((EODData.ReportRow)cls).Name == "RincianDFSperTransaksiBuyer")
            {
                if (RptEOD.IsGenerateRincianDFSBuyer(BusinessDateReport, drCM.CMID, drCM.Code) == false)
                {
                    //ApplicationLog.CreateAndWriteLog("False", "EOD");
                    continue;
                }
            }

            SetNewEODReport((EODData.ReportRow)cls, CtlCalendarPickUpStartDate.Text, drCM.CMID, drCM.Code);
        }
        
        string info = "";
        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void ProcessNewAllEODReport(Object cls)// EODData.ReportRow drListReports)
    {
        
        SetNewEODReport((EODData.ReportRow)cls, CtlCalendarPickUpStartDate.Text, null, null);
        string info = "";

        info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((EODData.ReportRow)cls).Name,
                       ((EODData.ReportRow)cls).GenerateType, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

        //SetEODReport(((ClsEODObjAsyn)cls).reportListPath, CtlCalendarPickUpStartDate.Text, null, null);
        //string info = "";
        //info = string.Format("ReportName:{0}, GenerateType:{1}, EndTime:{2}", ((ClsEODObjAsyn)cls).reportListPath.Name,
        //               ((ClsEODObjAsyn)cls).reportListPath.GenerateType, DateTime.Now);
        //sw.WriteLine(info);
        //sw.Flush();
        //resetEvents[((ClsEODObjAsyn)cls).currentThread].Set();    
        //CurrentResetEvents++;
        this.Session["LongActionProgress"] = (int)this.Session["LongActionProgress"] + 1;
        int currentThread = CurrentThread;
        Interlocked.Increment(ref currentThread);
        CurrentThread = currentThread;
    }

    private void SetNewEODReport(EODData.ReportRow drListReports,
        string businessDate, Nullable<decimal> cmId, string cmCode)
    {
        try
        {
            List<ReportParameter> rp = new List<ReportParameter>();
            ReportViewer rptViewer = new ReportViewer();

            string formatRender = "";

            string nmRpt = drListReports.Name;

            //if (drListReports.Name == "TradeRegister")
            //{

            //    rp.Add(new ReportParameter("BusinessDate", businessDate));
            //}
            //else
            //{
            //    rp.Add(new ReportParameter("businessDate", businessDate));
            //}

            if (drListReports.Name != "TradeRegister")
            {
                rp.Add(new ReportParameter("businessDate", businessDate));
            }

            if (drListReports.GenerateType == "AK")
            {
                if (drListReports.Name == "eDO")
                {
                    rp.Add(new ReportParameter("ExRef", new string[] { null }));
                }
                else if (drListReports.Name == "TradeRegister")
                {
                    rp.Add(new ReportParameter("BusinessDate", businessDate));
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("cmID", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "DFS")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("cmId", new string[] { cmId.ToString() }));
                }
                else if (drListReports.Name == "NotaPemberitahuanPembayaran")
                {
                    rp.Add(new ReportParameter("Code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                else
                {
                    rp.Add(new ReportParameter("code", new string[] { cmCode.ToString() }));
                    rp.Add(new ReportParameter("CmId", new string[] { cmId.ToString() }));
                }
                
                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_AK].ToString();
            }
            else if (drListReports.GenerateType == "All")
            {
                if (drListReports.Name == "TradeRegister")
                {
                    rp.Add(new ReportParameter("businessDate", businessDate));
                    rp.Add(new ReportParameter("code", new string[] { null }));
                    rp.Add(new ReportParameter("CmId", new string[] { null }));
                }
                else if (drListReports.Name == "DFS")
                {
                    rp.Add(new ReportParameter("Code", new string[] { null }));
                    rp.Add(new ReportParameter("cmId", new string[] { null }));
                }
                else if (drListReports.Name == "NotaPemberitahuanPembayaran")
                {
                    rp.Add(new ReportParameter("Code", new string[] { null }));
                    rp.Add(new ReportParameter("CmId", new string[] { null }));
                }
                formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE_ALL].ToString();
                
            }


            rptViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_REPORT_SERVER].ToString());
            
            rptViewer.ServerReport.ReportServerCredentials =
                    new ReportServerCredentials();
            rptViewer.ServerReport.ReportPath = drListReports.Path;
            rptViewer.ServerReport.SetParameters(rp);
            
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            //string formatRender = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_EOD_GENERATE_TYPE].ToString();


            byte[] bytes = rptViewer.ServerReport.Render(
              formatRender,
              null, out mimeType, out encoding, out extension,
              out streamids, out warnings);

            string filename = "";
            string filepath = "";

            string formatType = "";
            switch (formatRender.ToUpper())
            {
                case "PDF":
                    formatType = "pdf";
                    break;
                case "EXCEL":
                    formatType = "xls";
                    break;
                case "CSV":
                    formatType = "csv";
                    break;
                default:
                    formatType = "pdf";
                    break;
            }

            if (drListReports.GenerateType == "AK")
            {

                if(drListReports.Name == "eDO")
                {
                    EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                    filename = string.Format("{0}-{1}_{2}_{3}_{4}.{5}",
                        DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                        cmCode, drListReports.Name, cmCode, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }
                else
                {
                    EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                    filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                        DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                        cmCode, drListReports.Name, formatType);
                    filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                        BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                }

                /*
                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(), BusinessDateReport);
                filename = string.Format("{0}-{1}_{2}_{3}.{4}",
                    DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"), Revision,
                    cmCode, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_AK].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                */
                
            }
            else if (drListReports.GenerateType == "All")
            {

                EOD.CheckEODReportDirectory(
                    System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(), BusinessDateReport);
                filename = string.Format("{0}-{1}_{2}.{3}",
                    DateTime.Parse(CtlCalendarPickUpStartDate.Text).ToString("yyyyMMdd"),
                    Revision, drListReports.Name, formatType);
                filepath = string.Format("{0}{1}\\{2}\\{3}\\{4}", System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_EOD_ALL].ToString(),
                    BusinessDateReport.Year, BusinessDateReport.Month, BusinessDateReport.Day, filename);
                
            }


            //if (!File.Exists(filepath))
            //{
            //    Common.CreateFileByFileStream(bytes, filepath);
            //}
            Common.CreateFileByFileStream(bytes, filepath);
        }
        catch (Exception ex)
        {
            if (drListReports.GenerateType == "AK")
            {
                ApplicationLog.CreateAndWriteLog(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"), "ReportEOD");     
                LogReport.AppendLine(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"));
            }
            else if (drListReports.GenerateType == "All")
            {
                ApplicationLog.CreateAndWriteLog(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                    drListReports.Name, drListReports.GenerateType, cmCode, ex.Message, "<br>"), "ReportEOD");
                LogReport.AppendLine(string.Format("Failed generating report : {0}, generate type : {1}, clearing member : {2},  error : {3} {4}",
                   drListReports.Name, drListReports.GenerateType, "", ex.Message, "<br>"));
            }
           
        }
    }
    #endregion


}
