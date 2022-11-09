using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Transfer
/// </summary>
public class TransferSpa
{
	public TransferSpa()
	{
		//
		// TODO: Add constructor logic here
		//
    }

    private const string TRANSFER_TYPE_AA = "AA";
    private const string TRANSFER_TYPE_MM = "MM";

    #region "-- Transfer SPA Validation --"
    public static bool IsBroker(decimal cmId)
    {
        bool retVal = true;

        TransferSpaData.uspTrfSpaIsBrokerDataTable dt = new TransferSpaData.uspTrfSpaIsBrokerDataTable();
        TransferSpaDataTableAdapters.uspTrfSpaIsBrokerTableAdapter ta = new TransferSpaDataTableAdapters.uspTrfSpaIsBrokerTableAdapter();
        try
        {
            ta.Fill(dt, cmId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error found: " + ex.Message);
        }

        if ((dt == null) || (dt.Rows.Count == 0) || (int.Parse(dt.Rows[0][0].ToString()) != 1)) 
        {
            retVal = false;
        }

        return retVal;
    }

    #endregion

    #region "-- Transfer Position --"
    public static TransferData.TransferPositionDataTable GetTransferPositionByTransferDateDestCMApprovalStatus(Nullable<DateTime> transferDate, 
        Nullable<decimal> destClearingMemberId, string approvalStatus)
    {
        TransferData.TransferPositionDataTable dt = new TransferData.TransferPositionDataTable();
        TransferDataTableAdapters.TransferPositionTableAdapter ta = new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            ta.FillByTransferDateDestCMApprovalStatus(dt,transferDate, destClearingMemberId, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message,ex);
        }
    }

    /// <summary>
    /// Get Transfered Position 
    /// </summary>
    /// <param name="transferDate">Transfer Date</param>
    /// <param name="sourceCmId">Clearing Member ID Source</param>
    /// <param name="destCmId">Clearing Member ID Destination</param>
    /// <param name="transferType">Transfer Type (AA, MM, BT)</param>
    /// <param name="approvalStatus">Approval Status</param>
    /// <returns></returns>
    public static TransferSpaData.TransferSpaPositionDataTable 
        GetTransferPositionByTransferDateSourceDestCMTypeStatus(DateTime? transferDate,
        decimal? sourceCmId, decimal? destCmId, string transferType, string approvalStatus)
    {
        TransferSpaData.TransferSpaPositionDataTable dt = new TransferSpaData.TransferSpaPositionDataTable();
        TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter ta = new TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter();

        try
        {
            ta.FillByTransDateSourceDestCMTypeStatus(dt, transferDate, sourceCmId, destCmId, transferType, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Get Transfered Position
    /// </summary>
    /// <param name="transferId">TransferSpa ID</param>
    /// <returns></returns>
    public static TransferSpaData.TransferSpaPositionRow GetTransferPositionByTransferId(decimal transferSpaId)
    {
        TransferSpaData.TransferSpaPositionDataTable dt = new TransferSpaData.TransferSpaPositionDataTable();
        TransferSpaData.TransferSpaPositionRow dr = null;
        TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter ta = new TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter();

        try
        {
            ta.FillByTransferSpaID(dt, transferSpaId);
            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Create Transfer SPA Data:
    /// * Transfer SPA header will be recorded in table TransferSpaPosition 
    /// * Transfered OUT will be recorded in table TransferSpaRequest 
    /// * Transfered IN will be recorded in table TransferSpaAllocation 
    /// </summary>
    /// <param name="transferDate"></param>
    /// <param name="transferType"></param>
    /// <param name="sourceCMID"></param>
    /// <param name="destCMID"></param>
    /// <param name="desc"></param>
    /// <param name="approvalDesc"></param>
    /// <param name="createdBy"></param>
    /// <param name="dtNewTransferReqAlloc"></param>
    public static void ProcessCreateTransferSpa(
            DateTime transferDate, string transferType, decimal sourceCMID, decimal destCMID, 
            string desc, string approvalDesc, string createdBy, 
            TransferSpaData.TransferSpaReqAllocDataTable dtNewTransferReqAlloc)
    {
        TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter taTransferSpaPos =
                new TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                // Create Header record: TransferSpaPosition
                decimal transferSpaID = 0;
                object dummy;
                dummy = taTransferSpaPos.TrfSpaAddNewPosition(transferDate, transferType, sourceCMID, destCMID,
                                                                        createdBy, desc, approvalDesc);
                transferSpaID = decimal.Parse(dummy.ToString());

                // Create Transfer SPA OUT record: TransferSpaRequest 
                // always create SPA IN record also, if only if not "MM" 
                TransferSpaDataTableAdapters.TransferSpaRequestTableAdapter taTransferSpaOut = 
                        new TransferSpaDataTableAdapters.TransferSpaRequestTableAdapter();

                decimal counterReq = 1;
                foreach (TransferSpaData.TransferSpaReqAllocRow drReqAlloc in dtNewTransferReqAlloc)
                {
                    taTransferSpaOut.Insert(drReqAlloc.SrcInvestorID, drReqAlloc.SrcCPInvestorID, 
                            drReqAlloc.ContractID, drReqAlloc.TradePosition, drReqAlloc.Position, 
                            transferSpaID, drReqAlloc.Price, counterReq, drReqAlloc.Quantity);

                    // Create Transfer SPA IN record: TransferSpaAllocation (If and Only "AA")
                    if (transferType != "MM")
                    {
                        // post-cond: Transfer SPA is Account2Account 
                        //            It is auto-approved, then create also for Allocation 
                        TransferSpaDataTableAdapters.TransferSpaAllocationTableAdapter taTransferSpaIn =
                                new TransferSpaDataTableAdapters.TransferSpaAllocationTableAdapter();

                        taTransferSpaIn.Insert(drReqAlloc.ContractID, drReqAlloc.DestInvestorID,
                                drReqAlloc.DestCPInvestorID, drReqAlloc.TradePosition, drReqAlloc.Position,
                                transferSpaID, drReqAlloc.Price, counterReq, drReqAlloc.Quantity,
                                createdBy, DateTime.Now, createdBy, DateTime.Now);
                    }
                    
                    ++counterReq;
                }

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static TransferSpaData.TransferSpaPositionDataTable GetTransferSpaPositionByTransferDateDestCMTypeApproval(Nullable<DateTime> transferDate,
            Nullable<decimal> destClearingMember, string transferType, string approvalStatus)
    {
        TransferSpaData.TransferSpaPositionDataTable dt = new TransferSpaData.TransferSpaPositionDataTable();
        TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter ta = new TransferSpaDataTableAdapters.TransferSpaPositionTableAdapter();

        if (string.IsNullOrEmpty(transferType)) transferType = "";
        if (string.IsNullOrEmpty(approvalStatus)) approvalStatus = "";

        try
        {
            ta.FillByParams(dt, transferDate, transferType, destClearingMember, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ApproveRejectTranferSpa(decimal transferSpaId, string transferType, string approvalStatus,
            string approvalDesc, string updatedBy)
    {
        TransferSpaDataTableAdapters.QueriesTableAdapter ta = new TransferSpaDataTableAdapters.QueriesTableAdapter();
        if (transferType == "MM")
        {
            // post-cond: TransferSpa Member2Member
            switch (approvalStatus)
            {
                case "A":
                    ta.uspTrfSpaApproveMM(transferSpaId, updatedBy, approvalDesc);
                    break;

                case "R":
                    ta.uspTrfSpaRejectMM(transferSpaId, updatedBy, approvalDesc);
                    break;

                default:
                    throw new ApplicationException("Approval Status is not recognized.");
            }
        }
        else
        {
            // post-cond: TransferSpa other than Member2Member
            ta.uspTrfSpaApproveRejectGeneric(transferSpaId, approvalStatus, updatedBy, approvalDesc);
        }
    }

    #endregion

    #region "-- TransferRequest --"
    /// <summary>
    /// Get Transfered Request
    /// </summary>
    /// <param name="transferSpaId">TransferSpa ID</param>
    /// <returns></returns>
    public static TransferSpaData.TransferSpaRequestDataTable GetTransferRequestByTransferID(decimal transferSpaId)
    {
        TransferSpaData.TransferSpaRequestDataTable dt = new TransferSpaData.TransferSpaRequestDataTable();
        TransferSpaDataTableAdapters.TransferSpaRequestTableAdapter ta = new TransferSpaDataTableAdapters.TransferSpaRequestTableAdapter();

        try
        {
            ta.FillByTransferSpaID(dt, transferSpaId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }    
    #endregion

    #region "-- TransferContractPosition --"
    /// <summary>
    /// Get Open Position either from New Trade and Carry Forward
    /// </summary>
    /// <param name="businessDate"></param>
    /// <param name="investorId"></param>
    /// <param name="commodityId"></param>
    /// <param name="clearingMemberId"></param>
    /// <returns></returns>
    public static TransferSpaData.OpenPositionTrfSpaDataTable
        GetTransferContractPositionByBusinessDateInvestorCommodity(DateTime businessDate, 
        Nullable<decimal> investorId, Nullable<decimal> commodityId, decimal clearingMemberId)
    {
        TransferSpaData.OpenPositionTrfSpaDataTable dt = new TransferSpaData.OpenPositionTrfSpaDataTable();
        TransferSpaDataTableAdapters.OpenPositionTrfSpaTableAdapter ta = new TransferSpaDataTableAdapters.OpenPositionTrfSpaTableAdapter();

        try
        {
            ta.Fill(dt, clearingMemberId, businessDate, investorId, commodityId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion

    #region "-- TransferAllocation --"
    
    #endregion

    /* 
     * Not deleted for historical reason and logic model 
     * 
    public static void UpdateTransferPosition(decimal transferId, decimal transferNo, string approvalStatus, 
            DateTime transferDate, string transferType, decimal sourceCMID, decimal destCMID, 
            string description, string createdBy, DateTime createdDate, string lastUpdatedBy, 
            DateTime lastUpdatedDate, string approvalDesc, 
            TransferData.TransferRequestDataTable dtNewTransferReq)
    {
        TransferDataTableAdapters.TransferPositionTableAdapter taTransferPos = new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (transferNo == 0)
                {
                    TransferData.MaxTransferNoDataTable dtMaxTransferPos = new TransferData.MaxTransferNoDataTable();
                    TransferDataTableAdapters.MaxTransferNoTableAdapter taMaxTransferPos = new TransferDataTableAdapters.MaxTransferNoTableAdapter();
                    taMaxTransferPos.Fill(dtMaxTransferPos);
                    if (dtMaxTransferPos[0].IsTransferNoNull())
                    {
                        transferNo = 1;
                    }
                    else
                    {
                        transferNo = dtMaxTransferPos[0].TransferNo + 1;
                    }
                    transferId = (decimal)taTransferPos.InsertTransferPosition(transferNo, approvalStatus, transferDate, 
                        transferType, sourceCMID, destCMID, description, createdBy, createdDate, 
                        lastUpdatedBy, lastUpdatedDate, approvalDesc);

                }
                else
                { 
                    //skip
                    //not yet implement
                }
            

                //Transfer Request
                TransferData.TransferRequestDataTable dtTransferReq = new TransferData.TransferRequestDataTable();
                TransferData.TransferRequestRow drNewTransferReq = null;
                TransferDataTableAdapters.TransferRequestTableAdapter taTransferReq = new TransferDataTableAdapters.TransferRequestTableAdapter();
                taTransferReq.FillByTransferID(dtTransferReq, transferId);

                //Transfer Allocation
                TransferData.TransferAllocationDataTable dtTransferAlloc = new TransferData.TransferAllocationDataTable();
                TransferData.TransferAllocationRow drTransferAlloc = null;
                TransferDataTableAdapters.TransferAllocationTableAdapter taTransferAlloc = new TransferDataTableAdapters.TransferAllocationTableAdapter();

                //Process Transfer Request                
                decimal iiTransferReqId = 0;
                foreach (TransferData.TransferRequestRow drTransferReq in dtNewTransferReq)
                {
                    drNewTransferReq = dtTransferReq.FindByInvestorIDContractIDTradePositionPositionTransferIDPrice(drTransferReq.InvestorID, drTransferReq.ContractID, drTransferReq.TradePosition, 
                        drTransferReq.Position, transferId, drTransferReq.Price);

                    if (drNewTransferReq == null)
                    {
                        drNewTransferReq = dtTransferReq.NewTransferRequestRow();
                        TransferData.MaxTransferReqIDDataTable dtMaxTransferReq = new TransferData.MaxTransferReqIDDataTable();
                        TransferDataTableAdapters.MaxTransferReqIDTableAdapter taMaxTransferReq = new TransferDataTableAdapters.MaxTransferReqIDTableAdapter();
                        taMaxTransferReq.Fill(dtMaxTransferReq);
                        if (dtMaxTransferReq[0].IsTransferReqIDNull())
                        {
                            if (iiTransferReqId == 0)
                            {
                                iiTransferReqId = 1;
                            }

                            drNewTransferReq.TransferReqID = iiTransferReqId;

                            iiTransferReqId++;
                        }
                        else
                        {
                            if (iiTransferReqId == 0)
                            {
                                iiTransferReqId = dtMaxTransferReq[0].TransferReqID + 1;
                            }

                            drNewTransferReq.TransferReqID = iiTransferReqId;

                            iiTransferReqId++;
                        }
                    }                    

                    drNewTransferReq.TransferID = transferId;
                    if (transferType == TRANSFER_TYPE_AA)
                    {
                        drNewTransferReq.InvestorID = drTransferReq.DestInvestorID;
                    }
                    else if (transferType == TRANSFER_TYPE_MM)
                    {
                        drNewTransferReq.InvestorID = drTransferReq.InvestorID;
                    } 
                    drNewTransferReq.ContractID = drTransferReq.ContractID;
                    drNewTransferReq.TradePosition = drTransferReq.TradePosition;
                    drNewTransferReq.Position = drTransferReq.Position;
                    drNewTransferReq.Price = drTransferReq.Price;
                    drNewTransferReq.Quantity = drTransferReq.Quantity;

                    if (drNewTransferReq.RowState == System.Data.DataRowState.Detached)
                    {
                        dtTransferReq.AddTransferRequestRow(drNewTransferReq);
                    }

                    //Process Transfer Allocation, if approved
                    if (approvalStatus == "A")
                    {
                        drTransferAlloc = dtTransferAlloc.NewTransferAllocationRow();
                        drTransferAlloc.ContractID = drTransferReq.ContractID;
                        if (transferType == TRANSFER_TYPE_AA)
                        {
                            drTransferAlloc.InvestorID = drTransferReq.InvestorID;
                        }
                        else if (transferType == TRANSFER_TYPE_MM)
                        {
                            drTransferAlloc.InvestorID = drTransferReq.InvestorID;
                        }                        
                        drTransferAlloc.TradePosition = drTransferReq.TradePosition;
                        drTransferAlloc.Position = drTransferReq.Position;
                        drTransferAlloc.TransferID = transferId;
                        drTransferAlloc.Price = drTransferReq.Price;
                        drTransferAlloc.TransferReqID = iiTransferReqId;
                        drTransferAlloc.Quantity = drTransferReq.Quantity;
                        drTransferAlloc.CreatedBy = createdBy;
                        drTransferAlloc.CreatedDate = createdDate;
                        drTransferAlloc.LastUpdatedBy = lastUpdatedBy;
                        drTransferAlloc.LastUpdatedDate = lastUpdatedDate;

                        dtTransferAlloc.AddTransferAllocationRow(drTransferAlloc);
                    }
                }

                taTransferReq.Update(dtTransferReq);

                if (approvalStatus == "A")
                {
                    taTransferAlloc.Update(dtTransferAlloc);
                }                

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    */ 

}
