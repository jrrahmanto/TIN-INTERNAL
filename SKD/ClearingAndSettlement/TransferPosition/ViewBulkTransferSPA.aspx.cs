using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

public partial class WebUI_New_ViewBulkTransferSpa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Visible = false;
    }


    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            TransferSpaDataTableAdapters.QueriesTableAdapter ta = new TransferSpaDataTableAdapters.QueriesTableAdapter();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                try
                {
                    ta.uspTrfSpaCreateBulk(DateTime.Parse(Session["BusinessDate"].ToString()), decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID)
                                            , decimal.Parse(CtlClearingMemberLookupDestination.LookupTextBoxID), User.Identity.Name, uiTxtDescription.Text, "By Form");

                    scope.Complete();

                    ResetForm();
                }
                catch (Exception ex)
                {
                    uiBlError.Visible = true;
                    uiBlError.Items.Add(ex.Message);
                }
            }

        }
    }

    private void ResetForm()
    {
        CtlClearingMemberLookupSender.SetClearingMemberValue("", "");
        CtlClearingMemberLookupDestination.SetClearingMemberValue("", "");
        uiTxtDescription.Text = "";
    }

    /// <summary>
    /// Entry Validation (note by MP, 20121128) 
    /// * Business Date must be set (means already Begin Of Day)
    /// * Sender Clearing Member must be set 
    /// * Receipient Clearing Member must be set 
    /// * Sender and Recipient must not be same Clearing Member
    /// * If Sender is TRADER then Receipient must have Cluster already 
    ///   with all associated partner (Broker) 
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBlError.Visible = false;
        uiBlError.Items.Clear();

        if (Session["BusinessDate"] == null)
        {
            uiBlError.Items.Add("Bulk transfer is not allow, please process start of day first.");
        }
        if (string.IsNullOrEmpty(CtlClearingMemberLookupSender.LookupTextBox))
        {
            uiBlError.Items.Add("Sender is required.");
        }
        if (string.IsNullOrEmpty(CtlClearingMemberLookupDestination.LookupTextBox))
        {
            uiBlError.Items.Add("Receiver is required.");
        }
        if (!string.IsNullOrEmpty(CtlClearingMemberLookupSender.LookupTextBox) &&
            !string.IsNullOrEmpty(CtlClearingMemberLookupDestination.LookupTextBox))
        {
            if (CtlClearingMemberLookupDestination.LookupTextBoxID ==
               CtlClearingMemberLookupSender.LookupTextBox)
            {
                uiBlError.Items.Add("Clearing Member Sender and Receiver could not be the same.");
            }
            else
            {
                if (TransferSpa.IsBroker(decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID)) != 
                    TransferSpa.IsBroker(decimal.Parse(CtlClearingMemberLookupDestination.LookupTextBoxID)))
                {
                    uiBlError.Items.Add("Source Clearing member and destination Clearing member must be both Broker.");
                }
                else if (!TransferSpa.IsBroker(decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID)))
                {
                    uiBlError.Items.Add("Bulk Transfer SPA is allowed only for Broker to Broker.");
                }
                else
                {
                    // post-cond: both are same type
                    bool isClusterExist = true;

                    TransferSpaData.uspTrfSpaGetNonExistClusterDataTable dtNonClusterExist = new TransferSpaData.uspTrfSpaGetNonExistClusterDataTable();
                    TransferSpaDataTableAdapters.uspTrfSpaGetNonExistClusterTableAdapter taNonClusterExist = new TransferSpaDataTableAdapters.uspTrfSpaGetNonExistClusterTableAdapter();
                    TableAdapterHelper.SetAllCommandTimeouts(taNonClusterExist, 300);
                    try
                    {
                        taNonClusterExist.FillNonExistCluster(dtNonClusterExist, decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID)
                                                        , decimal.Parse(CtlClearingMemberLookupDestination.LookupTextBoxID)
                                                        , DateTime.Parse(Session["BusinessDate"].ToString()));


                        if (dtNonClusterExist != null && dtNonClusterExist.Rows.Count > 0 )
                        {
                            isClusterExist = false;
                            foreach (TransferSpaData.uspTrfSpaGetNonExistClusterRow dr in dtNonClusterExist)
                            {
                                uiBlError.Items.Add(String.Format("Cluster for commodity : {0} has not been setup.", dr.CommCodeName));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        uiBlError.Items.Add(ex.Message);
                    }
                }
            }
            
        }



        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

}
