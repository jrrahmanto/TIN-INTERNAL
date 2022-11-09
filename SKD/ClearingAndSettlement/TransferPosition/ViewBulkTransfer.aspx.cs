using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_ViewBulkTransfer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Visible = false;
    }


    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            // Clear constraint for transfer request
            TransferData.TransferRequestDataTable dtTransferReq = new TransferData.TransferRequestDataTable();
            TransferData.TransferRequestRow drTransferReq = null;
            dtTransferReq.TransferIDColumn.AllowDBNull = true;
            dtTransferReq.TransferReqIDColumn.AllowDBNull = true;

            // Get any position created by Sender Clearing Member from:
            // 1.  New Trade as Buyer (query from TradeFeed) 
            // 2.  New Trade as Seller (query from TradeFeed)  
            // 3.  Carry Forward Position (query from InvContractPositionEOD) 
            //     note: Settlement Price must be uploaded before any Transfer Position 
            //           then EOD can be started 
            TransferData.BulkTransferContractPositionDataTable dtBulk =
                Transfer.GetBulkTransferContractPositionByBusinessDateClearingMemberId(DateTime.Parse(Session["BusinessDate"].ToString()), decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID));

            // Get Transfered Position which already requested with same business date 
            TransferData.TransferPositionDataTable dtTransferPosition =
                Transfer.GetTransferPositionByTransferDateTypeSourceDestCMID(DateTime.Parse(Session["BusinessDate"].ToString()), "BT",
                decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID), decimal.Parse(CtlClearingMemberLookupDestination.LookupTextBoxID));
            if (IsValidEntry(dtBulk, dtTransferPosition) == true)
            {
                try
                {

                    foreach (TransferData.BulkTransferContractPositionRow drBulk in dtBulk)
                    {
                        drTransferReq = dtTransferReq.NewTransferRequestRow();

                        drTransferReq.InvestorID = drBulk.InvestorID;
                        drTransferReq.ContractID = drBulk.ContractID;
                        drTransferReq.TradePosition = drBulk.TradePosition;
                        drTransferReq.Position = drBulk.Position;
                        drTransferReq.Price = drBulk.Price;
                        drTransferReq.Quantity = drBulk.OpenPosition;

                        dtTransferReq.AddTransferRequestRow(drTransferReq);
                    }

                    Transfer.UpdateTransferPosition(0, 0, "A", DateTime.Parse(Session["BusinessDate"].ToString()), "BT",
                        decimal.Parse(CtlClearingMemberLookupSender.LookupTextBoxID),
                        decimal.Parse(CtlClearingMemberLookupDestination.LookupTextBoxID),
                        uiTxtDescription.Text, User.Identity.Name, DateTime.Now,
                        User.Identity.Name, DateTime.Now, null, dtTransferReq);

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
        }
       
        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

    /// <summary>
    /// Entry Validation (note by MP, 20121128) 
    /// * Business Date must be set (means already Begin Of Day)
    /// * Sender Clearing Member must be set 
    /// * Receipient Clearing Member must be set 
    /// * Sender and Recipient must not be same Clearing Member
    /// </summary>
    /// <param name="dtBulk"></param>
    /// <param name="dtTransferPosition"></param>
    /// <returns></returns>
    private bool IsValidEntry(
        TransferData.BulkTransferContractPositionDataTable dtBulk,
        TransferData.TransferPositionDataTable dtTransferPosition)
    {
        bool isValid = true;
        uiBlError.Visible = false;
        uiBlError.Items.Clear();

        if (string.IsNullOrEmpty(Session["BusinessDate"].ToString()))
        {
            uiBlError.Items.Add("Start of Day has not been executed yet.");
        }
        if (string.IsNullOrEmpty(CtlClearingMemberLookupSender.LookupTextBoxID))
        {
            uiBlError.Items.Add("Source clearing member is required.");
        }
        if (string.IsNullOrEmpty(CtlClearingMemberLookupDestination.LookupTextBoxID))
        {
            uiBlError.Items.Add("Destination clearing member is required.");
        }

        if (dtBulk.Count == 0)
        {
            uiBlError.Items.Add("There is no transaction to be transferred");
        }
        if (dtTransferPosition.Count > 0)
        {
            uiBlError.Items.Add("Transaction has been transferred.");
        }

        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isValid = false;
        }

        return isValid;
    }
}
