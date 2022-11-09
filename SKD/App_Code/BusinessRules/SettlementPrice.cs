using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for SettlementPrice
/// </summary>
public class SettlementPrice
{
    SettlementPriceExceptionDataTableAdapters.RawTradeFeedSPTableAdapter _spRawHandler = new SettlementPriceExceptionDataTableAdapters.RawTradeFeedSPTableAdapter();

    public SettlementPriceExceptionData.RawTradeFeedSPDataTable GetData(decimal exchangeID, decimal SPID, DateTime businessDate)
    {
        return _spRawHandler.GetDataByPK(businessDate, exchangeID, SPID);
    }


    public static SettlementPriceData.SettlementPriceDataTable GetSettlementByTransaction(DateTime bussDate, string approval)
    {
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

        try
        {
            ta.FillByBusinessDateTrx(dt, bussDate, approval);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.ContractDataTable GetContractByEffectiveStartDate(DateTime bussDate)
    {
        SettlementPriceData.ContractDataTable dt = new SettlementPriceData.ContractDataTable();
        SettlementPriceDataTableAdapters.ContractTableAdapter ta = new SettlementPriceDataTableAdapters.ContractTableAdapter();
        
        try
        {
            ta.FillByContractActive(dt, bussDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.SettlementPriceDataTable GetSettlementByBusinessDate(DateTime bussDate)
    {
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

        try
        {
            ta.FillByBusinessDateEdit(dt, bussDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.SettlementPriceDataTable GetSettlementByBusinessDateApproval(DateTime bussDate, string Approval)
    {
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

        try
        {
            ta.FillBySearchCriteria(dt, bussDate,Approval);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    /// <summary>
    /// GetSettlementByBusinessDateApprovalComm
    /// </summary>
    /// <param name="bussDate"></param>
    /// <param name="Approval"></param>
    /// <param name="commodityId"></param>
    /// <returns></returns>
    public static SettlementPriceData.SettlementPriceDataTable GetSettlementByBusinessDateApprovalComm(DateTime bussDate
            , string Approval, int? commodityId)
    {
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

        try
        {
            ta.FillBySearchCriteria2(dt, bussDate, Approval, commodityId);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.SettlementPriceDataTable GetSettlementByCriteria(DateTime bussDate, 
                                                                                       decimal contractID,
                                                                                       string settlePriceType)
    {
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

        try
        {
            ta.FillByCriteria(dt, bussDate, contractID, settlePriceType);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.SettlementPriceCreateEditDataTable Fill(DateTime bussDate, string approval)
    {
        SettlementPriceDataTableAdapters.SettlementPriceCreateEditTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceCreateEditTableAdapter();
        try
        {
            return ta.GetData(bussDate, approval);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static SettlementPriceData.SettlementPriceCreateEditDataTable FillForCreate(DateTime bussDate, string settlePriceType)
    {
        SettlementPriceDataTableAdapters.SettlementPriceCreateEditTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceCreateEditTableAdapter();
        try
        {
            return ta.GetDataCreate(bussDate, settlePriceType);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void ProcessSettlementPrice(DateTime businessDate, SettlementPriceData.SettlementPriceDataTable dtNewSettlementPrice, string userName)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        SettlementPriceData.SettlementPriceDataTable dtSettlementPrice = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceData.SettlementPriceRow drNewSettlementPrice = null;
        ta.FillByBusinessDate(dtSettlementPrice, businessDate);

        try
        {
            foreach (SettlementPriceData.SettlementPriceRow dr in dtNewSettlementPrice)
            {
                string ActionFlagDesc = "";
                if (dr.ActionFlag == "I")
                {
                    ActionFlagDesc = "Insert";

                    drNewSettlementPrice = dtSettlementPrice.FindByBusinessDateContractIDSettlementPriceTypeApprovalStatus(dr.BusinessDate, dr.ContractID, dr.SettlementPriceType, dr.ApprovalStatus);

                    if (drNewSettlementPrice == null)
                    {
                        drNewSettlementPrice = dtSettlementPrice.NewSettlementPriceRow();
                    }

                    drNewSettlementPrice.BusinessDate = businessDate;
                    drNewSettlementPrice.ContractID = dr.ContractID;
                    drNewSettlementPrice.SettlementPriceType = dr.SettlementPriceType;
                    drNewSettlementPrice.SettlementPrice = dr.SettlementPrice;
                    drNewSettlementPrice.ApprovalStatus = dr.ApprovalStatus;
                    drNewSettlementPrice.CreatedBy = dr.CreatedBy;
                    drNewSettlementPrice.CreatedDate = dr.CreatedDate;
                    drNewSettlementPrice.LastUpdatedBy = dr.LastUpdatedBy;
                    drNewSettlementPrice.LastUpdatedDate = dr.LastUpdatedDate;

                    if (!dr.IsApprovalDescNull())
                    {
                        drNewSettlementPrice.ApprovalDesc = dr.ApprovalDesc;
                    }
                    if (!dr.IsOriginalSettleIDNull())
                    {
                        drNewSettlementPrice.OriginalSettleID = dr.OriginalSettleID;
                    }
                    else
                    {
                        drNewSettlementPrice.OriginalSettleID = 0;
                    }

                    if (!dr.IsActionFlagNull())
                    {
                        drNewSettlementPrice.ActionFlag = dr.ActionFlag;
                    }

                    if (drNewSettlementPrice.RowState == System.Data.DataRowState.Detached)
                    {
                        dtSettlementPrice.AddSettlementPriceRow(drNewSettlementPrice);
                    }
                    
                    ta.Update(dtSettlementPrice);
                    
                    string logMessage = string.Format("Proposed Insert, ContractID:{0} | " +
                                                      "SettlementPriceType:N | " +
                                                      "BussinesDate:{1} | " +
                                                      "SettlementPrice:{2}",
                                                      dr.ContractID,
                                                      businessDate.ToShortDateString(),
                                                      dr.SettlementPrice);
                    AuditTrail.AddAuditTrail("SettlementPrice", AuditTrail.PROPOSE, logMessage, userName,ActionFlagDesc);

                }
                else if (dr.ActionFlag == "U")
                {
                    ActionFlagDesc = "Update";
                    drNewSettlementPrice = dtSettlementPrice.FindByBusinessDateContractIDSettlementPriceTypeApprovalStatus(dr.BusinessDate, dr.ContractID, dr.SettlementPriceType, dr.ApprovalStatus);

                    if (drNewSettlementPrice == null)
                    {
                        drNewSettlementPrice = dtSettlementPrice.NewSettlementPriceRow();
                    }

                    drNewSettlementPrice.BusinessDate = businessDate;
                    drNewSettlementPrice.ContractID = dr.ContractID;
                    drNewSettlementPrice.SettlementPriceType = dr.SettlementPriceType;
                    drNewSettlementPrice.SettlementPrice = dr.SettlementPrice;
                    drNewSettlementPrice.ApprovalStatus = dr.ApprovalStatus;
                    drNewSettlementPrice.CreatedBy = dr.CreatedBy;
                    drNewSettlementPrice.CreatedDate = dr.CreatedDate;
                    drNewSettlementPrice.LastUpdatedBy = dr.LastUpdatedBy;
                    drNewSettlementPrice.LastUpdatedDate = dr.LastUpdatedDate;

                    if (!dr.IsApprovalDescNull())
                    {
                        drNewSettlementPrice.ApprovalDesc = dr.ApprovalDesc;
                    }
                    if (!dr.IsOriginalSettleIDNull())
                    {
                        drNewSettlementPrice.OriginalSettleID = dr.OriginalSettleID;
                    }
                    else
                    {
                        drNewSettlementPrice.OriginalSettleID = 0;
                    }

                    if (!dr.IsActionFlagNull())
                    {
                        drNewSettlementPrice.ActionFlag = dr.ActionFlag;
                    }

                    if (drNewSettlementPrice.RowState == System.Data.DataRowState.Detached)
                    {
                        dtSettlementPrice.AddSettlementPriceRow(drNewSettlementPrice);
                    }

                    ta.Update(dtSettlementPrice);

                    string logMessage = string.Format("Proposed Update, ContractID:{0} | " +
                                                      "SettlementPriceType:N | " +
                                                      "BussinesDate:{1} | " +
                                                      "SettlementPrice:{2}",
                                                      dr.ContractID,
                                                      businessDate.ToShortDateString(),
                                                      dr.SettlementPrice);
                    AuditTrail.AddAuditTrail("SettlementPrice", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);

                }                
            }

        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                throw new ApplicationException("Record Already Exist.");
            }else
            {
                throw new ApplicationException(ex.Message, ex);
            }
            
        }
    }


    public static void ImportSettlementPrice(SettlementPriceData.SettlementPriceDataTable dt)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();

                ta.Update(dt);

                string logMessage = string.Format("Import settlement price", "");
                AuditTrail.AddAuditTrail("SettlementPrice", "Insert", logMessage, dt[0].CreatedBy, "Insert");
                
                scope.Complete();
            }
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static void ProposedInsert(SettlementPriceData.ImportDataDataTable dt, 
                                      decimal exchangeID, DateTime businessDate, string userName)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        try
        {
            decimal commID;
            decimal contractID;
            int month = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (SettlementPriceData.ImportDataRow dr in dt)
                {
                    if (dr.Month.ToUpper() == "JAN")
                    {
                        month = 1;
                    }
                    else if (dr.Month.ToUpper() == "FEB")
                    {
                        month = 2;
                    }
                    else if (dr.Month.ToUpper() == "MAR")
                    {
                        month = 3;
                    }
                    else if (dr.Month.ToUpper() == "APR")
                    {
                        month = 4;
                    }
                    else if (dr.Month.ToUpper() == "MEI")
                    {
                        month = 5;
                    }
                    else if (dr.Month.ToUpper() == "JUN")
                    {
                        month = 6;
                    }
                    else if (dr.Month.ToUpper() == "JUL")
                    {
                        month = 7;
                    }
                    else if (dr.Month.ToUpper() == "AUG")
                    {
                        month = 8;
                    }
                    else if (dr.Month.ToUpper() == "SEP")
                    {
                        month = 9;
                    }
                    else if (dr.Month.ToUpper() == "OKT")
                    {
                        month = 10;
                    }
                    else if (dr.Month.ToUpper() == "NOV")
                    {
                        month = 11;
                    }
                    else if (dr.Month.ToUpper() == "DEC")
                    {
                        month = 12;
                    }

                    commID = Commodity.GetCommodityID(exchangeID, dr.CommName.Trim().ToUpper());
                    contractID = Contract.GetContractID(commID, month, Convert.ToInt32(dr.Year));
                    if (contractID != 0)
                    {
                        ta.Insert(businessDate, contractID, "N", "P", dr.Price, userName, DateTime.Now, userName,
                              DateTime.Now, null, null, "I");
                    }
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                throw new ApplicationException("Records are already exist. Please import new record.");
            }
            else
            {
                throw ex;
            }
            
        }
    }

   
    public static void Approve(List<string> settlePriceList,
                           DateTime businessDate, string userName)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string[] settleArr;
                foreach (string item in settlePriceList)
                {
                    settleArr = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    ta.FillBySettleID(dt, Convert.ToDecimal(settleArr[7]));

                    SettlementPriceData.SettlementPriceRow dr = SettlementPrice.SelectSettlementBySettleID(Convert.ToDecimal(settleArr[7]));

                    if (dr.ActionFlag == "I")
                    {
                        ta.ApproveInsert("A", settleArr[6], null, Convert.ToDecimal(settleArr[7]));
                        string logMessage = string.Format("Approve Insert, ContractID:{0} | " +
                                                      "SettlementPriceType:N | " +
                                                      "BussinesDate:{1} | " +
                                                      "SettlementPrice:{2}",
                                                      settleArr[0],
                                                      businessDate.ToShortDateString(),
                                                      settleArr[2]);
                       AuditTrail.AddAuditTrail("SettlementPrice", AuditTrail.APPROVE, logMessage, userName,"Approve Insert");
                    }
                    else if (dr.ActionFlag == "U")
                    {
                      
                        ta.ApproveUpdate("A", dr.SettlementPrice, userName, DateTime.Now, settleArr[6],
                                             dr.OriginalSettleID);
                        ta.DeleteBySettleID(Convert.ToDecimal(dr.SettleID));
                        string logMessage = string.Format("Approve Update, ContractID:{0} | " +
                                                     "SettlementPriceType:N | " +
                                                     "BussinesDate:{1} | " +
                                                     "SettlementPrice:{2}",
                                                     settleArr[0],
                                                     businessDate.ToShortDateString(),
                                                     settleArr[2]);
                        AuditTrail.AddAuditTrail("SettlementPrice", AuditTrail.APPROVE, logMessage, userName, "Approve Update");
                    }
                }
                scope.Complete();
            }
           
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex) ;
        }
    }

    public static void Reject(List<string> settlePriceList, DateTime businessDate, string userName)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string[] settleArr;
                foreach (string item in settlePriceList)
                {
                    string logMessage = "";
                    settleArr = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    ta.FillBySettleID(dt, Convert.ToDecimal(settleArr[7]));
                    string ActionFlagDesc = "";
                    if (dt.Count > 0)
                    {
                        logMessage = string.Format("Rejected: {0}|{1}|{2}",
                                                          dt[0].BusinessDate,
                                                           dt[0].SettlementPrice,
                                                            dt[0].SettlementPriceType);
                      
                        switch (dt[0].ActionFlag)
                        {
                            case "I": ActionFlagDesc = "Insert"; break;
                            case "U": ActionFlagDesc = "Update"; break;
                            case "D": ActionFlagDesc = "Delete"; break;
                        }
                    }

                    ta.DeleteBySettleID(Convert.ToDecimal(settleArr[7]));
                    AuditTrail.AddAuditTrail("SettlementPrice", AuditTrail.REJECT, logMessage, userName, "Reject " + ActionFlagDesc);
                }
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static SettlementPriceData.SettlementPriceRow SelectSettlementBySettleID(decimal settleID)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        SettlementPriceData.SettlementPriceRow dr = null;
        try
        {
            ta.FillBySettleID(dt, settleID);

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

    public static void DeleteByBusinessDate(decimal exchangeId, DateTime bussDate)
    {
        SettlementPriceDataTableAdapters.SettlementPriceTableAdapter ta = new SettlementPriceDataTableAdapters.SettlementPriceTableAdapter();
        SettlementPriceData.SettlementPriceDataTable dt = new SettlementPriceData.SettlementPriceDataTable();
        try
        {
            ta.FillByUsedSettlementPrice(dt, bussDate);
            if (dt.Rows.Count > 0)
            {
                throw new ApplicationException("Settlement price " + bussDate.ToString("dd-MMM-yyyy") + " has been used.");
            }
            ta.uspSettlementPriceDeleteByBussExchange(bussDate, exchangeId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Can not delete Settlement Price: " + ex.Message, ex);
        }
    }

}
