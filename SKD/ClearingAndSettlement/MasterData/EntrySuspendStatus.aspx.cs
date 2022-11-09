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
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System.Transactions;


public partial class ClearingAndSettlement_MasterData_EntrySuspendStatus : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private decimal eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(Request.QueryString["eID"]);
            }
        }
        set { ViewState["eID"] = value; }
    }

    private List<string> ErrMessage
    {
        get { return (List<string>)ViewState["ErrMessage"]; }
        set { ViewState["ErrMessage"] = value; }
    }

    static string pkjBaseUrl = ConfigurationManager.AppSettings["PKJ_BASE_URL"];
    static string pkjUserName = ConfigurationManager.AppSettings["PKJ_USERNAME"];
    static string pkjPassword = ConfigurationManager.AppSettings["PKJ_PASSWORD"];
    static string maxTotalError = ConfigurationManager.AppSettings["MAX_TOTAL_ERROR"];

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            uiBLError.Visible = false;
         
            if (!Page.IsPostBack)
            {
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    //uiTxtEntryType.Enabled = false;
                    bindData();
                }
                SetAccessPage();
            }
          
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";
        string accStatus = "";
        
        SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(CtlBankAccountLookup.LookupTextBoxID));
        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                if (uiChkStatus.Checked == true)
                {
                    accStatus = "A";
                }
                else
                {
                    accStatus = "S";
                }
                // Case Update/Revision
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    {
                        actionFlag = "U";
                    }
                }

                SuspendAccStatus.ProposedInsertSuspend(dr.Code, dr.EMID, dr.Name, User.Identity.Name,
                                                        dr.InvestorID, accStatus, uiTxtReason.Text, "KBI", actionFlag);
                Response.Redirect("ViewSuspend.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewSuspend.aspx");
    }
    
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(eID));
                
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                SuspendAccStatus.ProposedDeleteSuspend(dr.Code, dr.EMID, dr.Name, User.Identity.Name,
                                                        Convert.ToDecimal(dr.InvestorID), dr.AccountStatus, dr.AccountStatusReason, dr.SuspendedBy, "D");
                
            }
            Response.Redirect("ViewSuspend.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            string suspendBy = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (uiChkStatus.Checked == false)
                    {
                        suspendBy = "KBI";
                    }

                    // Guard for editing proposed record
                    SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                    if (Convert.ToString(eID) != "")
                    {
                        SuspendAccStatus.ApproveSuspend(dr.InvestorID, uiTxtApprovalDesc.Text, null, User.Identity.Name, suspendBy, dr.OriginalInvestorID);
                        //Send Message to LimaKilo
                       // string token = DoLogin();

                      //  SendMessageToLimaKilo(token, User.Identity.Name);

                        //if (ErrMessage.Count > 0)
                        //    throw new ApplicationException("");
                    }
                    scope.Complete();
                    Response.Redirect("ViewSuspend.aspx",false);
                    
                }
                    
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }     
    }
    
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                
                SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    SuspendAccStatus.Reject(dr.InvestorID, null, null, User.Identity.Name);
                    
                }
                Response.Redirect("ViewSuspend.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }

    protected void uiChkStatus_CheckedChanged(object sender, EventArgs e)
    {
        //if (uiChkStatus.Checked == true)
        //{
        //    uiChkStatus.Text = "Active";
        //}
        //else
        //{
        //    uiChkStatus.Text = "Non Active";
        //}
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            

            if (mp.IsMaker || mp.IsChecker)
            {
                if (eType == "edit")
                {
                    SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(CtlBankAccountLookup.LookupTextBoxID));

                    if (uiChkStatus.Checked == false)
                    {
                        if (!dr.IsAccountStatusNull())
                        {
                            if (dr.AccountStatus == "S" && dr.ApprovalStatus == "A")
                            {
                                //if (mp.IsChecker)
                                //{
                                //    if (dr.ApprovalStatus == "A")
                                        uiBLError.Items.Add("AccountCode status is already Suspend.");
                                //}
                                
                            }
                        }
                        
                        
                    }

                    if (uiChkStatus.Checked == true)
                    {
                        if (!dr.IsAccountStatusNull())
                        {
                            if (dr.AccountStatus == "A" && dr.ApprovalStatus == "A")
                            {
                                //if (mp.IsChecker && dr.ApprovalStatus=="A")
                                 uiBLError.Items.Add("AccountCode status is already Active.");
                            }

                            if (dr.AccountStatus == "S")
                            {
                                if (!dr.IsSuspendedByNull())
                                {
                                    if (dr.SuspendedBy == "PKJ")
                                    {
                                        uiBLError.Items.Add("Authority violation, cannot update AccountCode status.");
                                    }
                                }
                                
                            }
                        }
                            
                        
                    }

                    
                }
                
            }

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            
            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }

            return isValid;

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void bindData()
    {
        try
        {
            
            SuspendAccStatusData.InvestorRow dr = SuspendAccStatus.FillByID(Convert.ToDecimal(eID));
            
            CtlBankAccountLookup.SetBankAccountValue(dr.InvestorID.ToString(), dr.Code + "-" + dr.Name);
            if (dr.AccountStatus == "A")
            {
                uiChkStatus.Checked = true;
            }
            else
            {
                uiChkStatus.Checked = false;
            }

            if (!dr.IsAccountStatusReasonNull())
            {
                uiTxtReason.Text = dr.AccountStatusReason;
            }

            
            
            string actionDesc = "";
            //cek actionflag null
            if (!dr.IsActionFlagNull())
            {
                if (dr.ActionFlag == "I")
                {
                    actionDesc = "New Record";
                }
                else if (dr.ActionFlag == "U")
                {
                    actionDesc = "Revision";
                }
                else if (dr.ActionFlag == "D")
                {
                    actionDesc = "Delete";
                }
            }
            uiTxtAction.Text = actionDesc;

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            trAction.Visible = mp.IsChecker || mp.IsViewer;
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            if (eType == "edit")
            {
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;


            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                // ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                // SuretyBondData.VSuretyBondRow dr = SuretyBond.SelectSuretyBondByID(Convert.ToDecimal(eID));
                
                uiChkStatus.Enabled = false;
                uiTxtReason.Enabled = false;
                //CtlBondIssuer.SetDisabledExchange(true);
                uiTxtAction.Enabled = false;
             }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    private string DoLogin()
    {
        string token = "";
        List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
        listParam.Add(new KeyValuePair<string, string>("email", pkjUserName));
        listParam.Add(new KeyValuePair<string, string>("password", pkjPassword));
        string resLogin = APIHelper.Post(
            string.Format("{0}/{1}", pkjBaseUrl, "api/auth/kliring"),
            null, listParam);
        LoginResult.Rootobject objLogin =
            JsonConvert.DeserializeObject<LoginResult.Rootobject>(resLogin);
        if (objLogin.error == null)
        {
            token = objLogin.result.accessToken;
        }
        else
        {
            throw new ApplicationException(objLogin.message);
        }

        return token;
    }

    /// <summary>
    /// Send data to 5Kilo
    /// </summary>
    /// <param name="token"></param>
    private void SendMessageToLimaKilo(string token, string userNm)
    {
        try
        {
            string username = userNm;
            int totalErrorSettlement = 0;
            int totalErrorPayment = 0;
            int totalSuccessSettlement = 0;
            int totalSuccessPayment = 0;

            List<KeyValuePair<string, string>> listHeader = new List<KeyValuePair<string, string>>();
            listHeader.Add(new KeyValuePair<string, string>("x-access-token", token));
            // maxTotalError = "0";
            //Settlement
            EODTradeProgressData.EODTradeProgressDataTable dtProgressSettlementSubmittedNull =
                EODTradeProgress.GetBySettlementSubmittedTimeNull();
            foreach (EODTradeProgressData.EODTradeProgressRow drProgress in dtProgressSettlementSubmittedNull)
            {
                if (totalErrorSettlement >= int.Parse(maxTotalError))
                {
                   
                    ErrMessage.Add(
                       string.Format("Number of errors exceed total limit errors, the remaining message will not be submitted. {0} of {1} settlement messages are submitted.",
                           totalSuccessSettlement, dtProgressSettlementSubmittedNull.Count));
                    break;
                }

                try
                {
                    List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
                    listParam.Add(new KeyValuePair<string, string>("exchangeRef", drProgress.ExchangeRef));
                    listParam.Add(new KeyValuePair<string, string>("buyerId", drProgress.BuyerId));
                    listParam.Add(new KeyValuePair<string, string>("sellerId", drProgress.SellerId));
                    listParam.Add(new KeyValuePair<string, string>("productCode", drProgress.ProductCode));
                    listParam.Add(new KeyValuePair<string, string>("contractMonth", drProgress.ContractMonth.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("volume", drProgress.Volume.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("price", drProgress.Price.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("date", drProgress.BusinessDate.ToString("yyyy-MM-dd")));

                    string resSettlement = APIHelper.Post(
                        string.Format("{0}/{1}", pkjBaseUrl, "api/settlements"),
                        listHeader, listParam);

                    SettlementResult.Rootobject objSettlement =
                        JsonConvert.DeserializeObject<SettlementResult.Rootobject>(resSettlement);
                    if (objSettlement.error == null)
                    {
                        DateTime now = DateTime.Now;
                        drProgress.SettlementSubmittedTime = now;
                        drProgress.LastUpdatedBy = username;
                        drProgress.LastUpdatedDate = now;
                        EODTradeProgress.UpdateProgress(drProgress);
                        totalSuccessSettlement += 1;
                    }
                    else
                    {
                        totalErrorSettlement += 1;
                        ErrMessage.Add(string.Format("Settlement ExRef {0} failed: {1}", drProgress.ExchangeRef,
                            objSettlement.message));
                    }
                }
                catch (Exception ex)
                {
                    totalErrorSettlement += 1;
                    ErrMessage.Add(string.Format("Settlement ExRef {0} failed: {1}", drProgress.ExchangeRef,
                        ex.Message));
                }

            }
            ApplicationLog.Insert(DateTime.Now, "Publish Report", "I",
                    string.Format("{0} settlement messages are submitted", totalSuccessSettlement),
                    User.Identity.Name, Common.GetIPAddress(this.Request));

            //Payment
            EODTradeProgressData.EODTradeProgressDataTable dtProgressFullPaymentSubmittedDateNull =
                EODTradeProgress.GetByFullPaymentSubmittedTimeNull();
            foreach (EODTradeProgressData.EODTradeProgressRow drProgress in dtProgressFullPaymentSubmittedDateNull)
            {
                if (totalErrorPayment >= int.Parse(maxTotalError))
                {
                    ErrMessage.Add(
                       string.Format("Number of errors exceed total limit errors, the remaining message will not be submitted. {0} of {1} payment messages are submitted.",
                           totalSuccessPayment, dtProgressFullPaymentSubmittedDateNull.Count));
                    break;
                }

                try
                {
                    List<KeyValuePair<string, string>> listParam = new List<KeyValuePair<string, string>>();
                    listParam.Add(new KeyValuePair<string, string>("exchangeRef", drProgress.ExchangeRef));
                    listParam.Add(new KeyValuePair<string, string>("amount", drProgress.Amount.ToString()));
                    listParam.Add(new KeyValuePair<string, string>("date", drProgress.SellerReceive90Percent.ToString("yyyy-MM-dd")));

                    string resPayment = APIHelper.Post(
                        string.Format("{0}/{1}", pkjBaseUrl, "api/payments"),
                        listHeader, listParam);

                    PaymentResult.Rootobject objPayment =
                        JsonConvert.DeserializeObject<PaymentResult.Rootobject>(resPayment);
                    if (objPayment.error == null)
                    {
                        DateTime now = DateTime.Now;
                        drProgress.FullPaymentSubmittedTime = now;
                        drProgress.LastUpdatedBy = username;
                        drProgress.LastUpdatedDate = now;
                        EODTradeProgress.UpdateProgress(drProgress);
                        totalSuccessPayment += 1;
                    }
                    else
                    {
                        totalErrorPayment += 1;
                        ErrMessage.Add(string.Format("Payment ExRef {0} failed: {1}", drProgress.ExchangeRef,
                            objPayment.message));
                    }
                }
                catch (Exception ex)
                {
                    totalErrorPayment += 1;
                    ErrMessage.Add(string.Format("Payment ExRef {0} failed: {1}", drProgress.ExchangeRef,
                        ex.Message));
                }

            }
            ApplicationLog.Insert(DateTime.Now, "Publish Report", "I",
                     string.Format("{0} payment messages are submitted", totalSuccessPayment),
                     User.Identity.Name, Common.GetIPAddress(this.Request));
        }
        catch (Exception ex)
        {
            ErrMessage.Add(string.Format("Failed to Send Message to LimaKilo: {0}", ex.Message));
        }
    }

    #endregion



   
}
