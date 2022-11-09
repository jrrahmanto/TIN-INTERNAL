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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

using System.Messaging;
using System.Transactions;
using System.Net;

public partial class RiskManagement_ClearingMemberSetStatus : System.Web.UI.Page
{
    private string MSMQ_SPA = System.Configuration.ConfigurationManager.AppSettings["MSMQ_SPA"];
    private string MSMQ_PRIMER = System.Configuration.ConfigurationManager.AppSettings["MSMQ_PRIMER"];

    private MessageQueue queSuspendClearingPrimer;
    private MessageQueue queSuspendClearingSpa;

    private DateTime? businessDate;
    string lastStatus = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        businessDate = Parameter.GetParameterBusinessDate();
        businessDate = (businessDate.HasValue) ? businessDate : DateTime.Now;

        string pathPrimer = ConstructMQPath(MSMQ_PRIMER);
        string pathSpa = ConstructMQPath(MSMQ_SPA);

        queSuspendClearingPrimer = new MessageQueue(pathPrimer);
        queSuspendClearingSpa = new MessageQueue(pathSpa);

        if (!Page.IsPostBack)
        {
            uiBLSuccess.Visible = false;
            uiBLSuccess.Items.Clear();
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
        }

    }

    protected void uiBtnSubmit_Click(object sender, EventArgs e)
    {
        uiBLError.Items.Clear();
        uiBLSuccess.Items.Clear();

        if (IsEntryValid())
        {
            // post-cond: valid for submitting status 


            try
            {
                System.Messaging.Message msg = new System.Messaging.Message();
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                transactionOptions.Timeout = new TimeSpan(0, 3, 0);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    // Construct message to be sent to Queue for TradeFeed service 
                    // example: msg.Body = "20141202|2|6|20141202154503|010|R|R 010";

                    // [0] = business date
                    // [1] = exchange id
                    // [2] = suspend no
                    // [3] = suspend time
                    // [4] = cm code
                    // [5] = suspend type
                    // [6] = description
                    ClsClearingHouseSuspend data = new ClsClearingHouseSuspend();
                    data.businessDate = (DateTime)businessDate;

                    // For Each Exchange, both sent 
                    ClearingMemberStatusData dsClearingMemberStatus = new ClearingMemberStatusData();
                    ClearingMemberStatusDataTableAdapters.ClearingHouseSuspenseTableAdapter taCHSuspense = new ClearingMemberStatusDataTableAdapters.ClearingHouseSuspenseTableAdapter();
                    ClearingMemberStatusDataTableAdapters.ClearingMemberSuspenseStatusTableAdapter taCMStatus = new ClearingMemberStatusDataTableAdapters.ClearingMemberSuspenseStatusTableAdapter();

                    // for Exchange: Primer 
                    data.exchangeId = 1;
                    dsClearingMemberStatus.ClearingHouseSuspense.Clear();

                    data.suspendNo = taCHSuspense.FillMaxSuspenseNo(dsClearingMemberStatus.ClearingHouseSuspense, data.exchangeId, data.businessDate);
                    if (dsClearingMemberStatus.ClearingHouseSuspense.Rows.Count > 0)
                    {
                        data.suspendNo = ((ClearingMemberStatusData.ClearingHouseSuspenseRow)dsClearingMemberStatus.ClearingHouseSuspense.Rows[0]).SuspenseNo;
                    }
                    else
                    {
                        data.suspendNo = 0;
                    }
                    data.suspendTime = DateTime.Now;
                    data.cmCode = CtlClearingMemberLookup1.LookupTextBox;
                    data.cmId = int.Parse(CtlClearingMemberLookup1.LookupTextBoxID);
                    data.suspendType = uiDdlStatus.SelectedValue;
                    data.description = uiTxbReason.Text;

                    data.suspendNo = data.suspendNo + 1;
                    msg.Body = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", data.businessDate.ToString("yyyyMMdd")
                                , data.exchangeId, data.suspendNo, data.suspendTime.ToString("yyyyMMddHHmmss")
                                , data.cmCode, data.suspendType, data.description);

                    decimal newId = -1;
                    Object retObj = null;

                    retObj = taCHSuspense.InsertQuery(data.businessDate, data.exchangeId, data.suspendNo, data.suspendTime
                                 , null, data.cmId, data.suspendType, data.description);
                    newId = (decimal)retObj;
                    taCMStatus.Insert(data.businessDate, data.exchangeId, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID)
                                , "C", newId, data.suspendType, data.suspendTime, "N", "Y");

                    queSuspendClearingPrimer.Send(msg); //TODO: uncomment this line! 
                    ApplicationLog.Insert(DateTime.Now, "Submit CM Status for Primer: " + uiDdlStatus.SelectedItem.Text, "I", "Command", Page.User.Identity.Name, Common.GetIPAddress(this.Request));

                    // for Exchange: Primer 
                    data.exchangeId = 2;
                    dsClearingMemberStatus.ClearingHouseSuspense.Clear();

                    data.suspendNo = taCHSuspense.FillMaxSuspenseNo(dsClearingMemberStatus.ClearingHouseSuspense, data.exchangeId, data.businessDate);
                    if (dsClearingMemberStatus.ClearingHouseSuspense.Rows.Count > 0)
                    {
                        data.suspendNo = ((ClearingMemberStatusData.ClearingHouseSuspenseRow)dsClearingMemberStatus.ClearingHouseSuspense.Rows[0]).SuspenseNo;
                    }
                    else
                    {
                        data.suspendNo = 0;
                    }

                    data.suspendNo = data.suspendNo + 1;

                    msg.Body = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", data.businessDate.ToString("yyyyMMdd")
                                , data.exchangeId, data.suspendNo, data.suspendTime.ToString("yyyyMMddHHmmss")
                                , data.cmCode, data.suspendType, data.description);

                    retObj = taCHSuspense.InsertQuery(data.businessDate, data.exchangeId, data.suspendNo, data.suspendTime
                                , null, data.cmId, data.suspendType, data.description);
                    newId = (decimal)retObj;
                    taCMStatus.Insert(data.businessDate, data.exchangeId, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID)
                                , "C", newId, data.suspendType, data.suspendTime, "N", "Y");

                    queSuspendClearingSpa.Send(msg); //TODO: uncomment this line! 
                    ApplicationLog.Insert(DateTime.Now, "Submit CM Status for SPA: " + uiDdlStatus.SelectedItem.Text, "I", "Command", Page.User.Identity.Name, Common.GetIPAddress(this.Request));

                    ClearingMemberStatusDataTableAdapters.QueriesTableAdapter taBroadcast = new ClearingMemberStatusDataTableAdapters.QueriesTableAdapter();
                    if (uiDdlStatus.SelectedValue == "R" && lastStatus == "S")
                    {
                        taBroadcast.uspBroadcastReleaseStatusCM(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), "S");
                    }
                    else if (uiDdlStatus.SelectedValue == "S")
                    {
                        taBroadcast.uspBroadcastSuspenseStatusCM(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), "S");
                    }
                    else
                    {
                        // Do nothing 
                    }

                    scope.Complete();
                }

                uiBLSuccess.Visible = true;
                uiBLSuccess.Items.Clear();
                uiBLSuccess.Items.Add(string.Format("Clearing Member Status for code {0} has been successfully set.", CtlClearingMemberLookup1.LookupTextBox));

            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }

        }
        else
        {
            uiBLError.Visible = true;
        }

    }

    /// <summary>
    /// Check whether all entries are valid:
    /// 1.  If Limit: last status of Clearing Member is not Limit 
    /// 2.  If Suspend: last status of Clearing Member is not Suspended 
    /// 3.  If Release: last status of Clearing Member is not emptied or Released 
    /// </summary>
    /// <returns></returns>
    private bool IsEntryValid()
    {
        bool validateResult = true;
        lastStatus = "";

        ClearingMemberStatusData.ClearingMemberSuspenseStatusDataTable dt = 
                ClearingMemberStatus.GetLastClearingMemberStatus((DateTime)businessDate, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID));

        if (dt.Rows.Count > 0)
        {
            // post-cond: this CM has ever been set as Limit/Suspend/Release 
            lastStatus = ((ClearingMemberStatusData.ClearingMemberSuspenseStatusRow)dt.Rows[0]).SuspenseType;

            if (uiDdlStatus.SelectedItem.Value == lastStatus)
            {
                uiBLError.Items.Add(string.Format("Can not set status as Clearing Member's last status"));

                validateResult = false;
            }
            else if (uiDdlStatus.SelectedItem.Value == "L" && lastStatus == "S")
            {
                uiBLError.Items.Add(string.Format("Can not set status as Limit when Clearing Member's status already Suspended"));

                validateResult = false;
            }
            else
            {
                // No Else :D
                validateResult = true;
            }
        }
        else
        {
            if (uiDdlStatus.SelectedItem.Value == "R")
            {
                uiBLError.Items.Add(string.Format("Can not set status as Release when Clearing Member's status not Limit or Suspend"));

                validateResult = false;
            }
            else
            {
                // No Else :D
                validateResult = true;
            }
        }

        return validateResult;
    }

    /// <summary>
    /// Construct Message Queue Path setting from configuration value
    /// </summary>
    /// <param name="mq"></param>
    /// <returns></returns>
    private string ConstructMQPath(string mq)
    {
        string path = "";

        string[] values = mq.Split(@"\".ToCharArray());
        IPAddress ip = null;
        string mqPath = "";
        if (IPAddress.TryParse(values[0], out ip))
        {
            mqPath = string.Format(@"FormatName:Direct=TCP:{0}\{1}\{2}",
                ip.ToString(), values[1], values[2]);
        }
        else
        {
            mqPath = string.Format(@"{0}\{1}\{2}",
                values[0], values[1], values[2]);
        }

        path = mqPath;

        return path;
    }

}
