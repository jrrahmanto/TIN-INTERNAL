using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Transfer
/// </summary>
public class Transfer
{
	public Transfer()
	{
		//
		// TODO: Add constructor logic here
		//
    }

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

    public static TransferData.TransferPositionDataTable 
        GetTransferPositionByTransferDateDestCMTypeApproval(Nullable<DateTime> transferDate, 
        Nullable<decimal> destClearingMember, string transferType, string approvalStatus)
    {
        TransferData.TransferPositionDataTable dt = new TransferData.TransferPositionDataTable();
        TransferDataTableAdapters.TransferPositionTableAdapter ta = new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            ta.FillByTransferDateDestCMTypeApproval(dt, transferDate, destClearingMember, 
                transferType, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static TransferData.TransferPositionRow GetTransferPositionByTransferId(decimal transferId)
    {
        TransferData.TransferPositionDataTable dt = new TransferData.TransferPositionDataTable();
        TransferData.TransferPositionRow dr = null;
        TransferDataTableAdapters.TransferPositionTableAdapter ta = new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            ta.FillByTransferID(dt, transferId);
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

    public static TransferData.TransferPositionDataTable 
        GetTransferPositionByTransferDateTypeSourceDestCMID(
        DateTime transferDate, string transferType, 
        decimal sourceCMID, decimal destCMID)
    {
        TransferData.TransferPositionDataTable dt = new TransferData.TransferPositionDataTable();
        TransferDataTableAdapters.TransferPositionTableAdapter ta = 
            new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            ta.FillByTransferDateTypeSourceDestCMID(dt, transferDate,
                sourceCMID, destCMID, transferType);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void UpdateTransferPosition(decimal transferId, decimal transferNo, string approvalStatus, 
        DateTime transferDate, string transferType, decimal sourceCMID, decimal destCMID, 
        string description, string createdBy, DateTime createdDate, string lastUpdatedBy, 
        DateTime lastUpdatedDate, string approvalDesc, TransferData.TransferRequestDataTable dtNewTransferReq)
    {
        TransferDataTableAdapters.TransferPositionTableAdapter taTransferPos = new TransferDataTableAdapters.TransferPositionTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
            {
                if (transferNo == 0)
                {
                    // post-cond : New Transfer 
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
                    // post-cond : Existing Transfer 
                    taTransferPos.UpdateTransferPosition(approvalStatus, lastUpdatedBy, lastUpdatedDate, approvalDesc, transferId);
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
                    drNewTransferReq = dtTransferReq.FindByInvestorIDContractIDTradePositionPositionTransferIDPrice(drTransferReq.InvestorID, 
                        drTransferReq.ContractID, drTransferReq.TradePosition, 
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
                    drNewTransferReq.InvestorID = drTransferReq.InvestorID;
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
                        if (dtTransferAlloc.FindByContractIDInvestorIDTradePositionPositionTransferIDPrice(drTransferReq.ContractID,
                            GetInvestorDefaultAccountByDestCMID(destCMID, drTransferReq.Position), drTransferReq.TradePosition,
                            drTransferReq.Position, transferId, drTransferReq.Price) == null)
                        {
                            drTransferAlloc = dtTransferAlloc.NewTransferAllocationRow();

                            drTransferAlloc.Price = drTransferReq.Price;
                            drTransferAlloc.Quantity = drTransferReq.Quantity;
                            drTransferAlloc.InvestorID = GetInvestorDefaultAccountByDestCMID(destCMID, drTransferReq.Position);
                            drTransferAlloc.TradePosition = drTransferReq.TradePosition;
                            drTransferAlloc.Position = drTransferReq.Position;
                            drTransferAlloc.TransferID = transferId;
                            drTransferAlloc.ContractID = drTransferReq.ContractID;
                            drTransferAlloc.TransferReqID = iiTransferReqId;
                            drTransferAlloc.CreatedBy = createdBy;
                            drTransferAlloc.CreatedDate = createdDate;
                            drTransferAlloc.LastUpdatedBy = lastUpdatedBy;
                            drTransferAlloc.LastUpdatedDate = lastUpdatedDate;

                            if (drTransferAlloc.RowState == System.Data.DataRowState.Detached)
                            {
                                dtTransferAlloc.AddTransferAllocationRow(drTransferAlloc);
                            }
                        }
                        else
                        {
                            //drTransferAlloc.Price += drTransferReq.Price;
                            drTransferAlloc.Quantity += drTransferReq.Quantity;
                        }
                    }
                }

                taTransferReq.Update(dtTransferReq);


                string log = string.Format("TransferNo:{0}|TransferID:{1}|TransferDate:{2}|" +
                    "TransferType:{3}|SourceCMID:{4}|DestCMID:{5}|Description:{6}", 
                    transferNo, transferId, transferDate, transferType, sourceCMID, destCMID, description);
                if (approvalStatus == "A")
                {
                    taTransferAlloc.Update(dtTransferAlloc);

                    AuditTrail.AddAuditTrail("TransferPosition", AuditTrail.APPROVE, log, lastUpdatedBy,"Approve Update");
                }
                else if (approvalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("TransferPosition", AuditTrail.REJECT, log, lastUpdatedBy, "Reject Update");
                }
                else if (approvalStatus == "P")
                {
                    AuditTrail.AddAuditTrail("TransferPosition", AuditTrail.PROPOSE, log, lastUpdatedBy,"Proposed Update");
                }

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static TransferData.BulkTransferContractPositionDataTable 
        GetBulkTransferContractPositionByBusinessDateClearingMemberId(DateTime businessDate, decimal clearingMemberId)
    {
        TransferData.BulkTransferContractPositionDataTable dt = new TransferData.BulkTransferContractPositionDataTable();
        TransferDataTableAdapters.BulkTransferContractPositionTableAdapter ta = new TransferDataTableAdapters.BulkTransferContractPositionTableAdapter();

        try
        {
            ta.Fill(dt, businessDate, clearingMemberId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static decimal GetInvestorDefaultAccountByDestCMID(decimal clearingMemberId, string position)
    {
        TransferData.GetInvestorIDDefaultAccountDataTable dt = new TransferData.GetInvestorIDDefaultAccountDataTable();
        TransferData.GetInvestorIDDefaultAccountRow dr = null;
        TransferDataTableAdapters.GetInvestorIDDefaultAccountTableAdapter ta = new TransferDataTableAdapters.GetInvestorIDDefaultAccountTableAdapter();
        decimal investorId = 0;

        try
        {
            if (position == "S")
            {
                ta.FillDefaultAccS(dt, clearingMemberId);
            }
            else if (position == "B")
            {
                ta.FillByDefaultAccB(dt, clearingMemberId);
            }

            if (dt.Count > 0)
            {
                dr = dt[0];
                investorId = dr.InvestorID;
            }

            return investorId;
        }
        catch (Exception ex)
        {            
            throw new ApplicationException(ex.Message, ex);
        }

    }
    #endregion

    #region "-- TransferRequest --"
    public static TransferData.TransferRequestDataTable GetTransferRequestByTransferID(decimal transferId)
    {
        TransferData.TransferRequestDataTable dt = new TransferData.TransferRequestDataTable();
        TransferDataTableAdapters.TransferRequestTableAdapter ta = new TransferDataTableAdapters.TransferRequestTableAdapter();

        try
        {
            ta.FillByTransferID(dt, transferId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }    
    #endregion

    #region "-- TransferContractPosition --"
    public static TransferData.TransferContractPositionDataTable
        GetTransferContractPositionByBusinessDateInvestorCommodity(DateTime businessDate, 
        Nullable<decimal> investorId, Nullable<decimal> commodityId)
    {
        TransferData.TransferContractPositionDataTable dt = new TransferData.TransferContractPositionDataTable();
        TransferDataTableAdapters.TransferContractPositionTableAdapter ta = new TransferDataTableAdapters.TransferContractPositionTableAdapter();

        try
        {
            ta.Fill(dt, businessDate, investorId, commodityId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion
}
