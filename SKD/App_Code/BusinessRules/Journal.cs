using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Data;

/// <summary>
/// Summary description for Journal
/// </summary>
public class Journal
{

    #region "-- JournalHeader --"
    public static JournalData.JournalHeaderDataTable
        GetJournalByJournalNoTransactionDateApprovalStatus(Nullable<int> journalNo, 
        Nullable<DateTime> transactionDate, string approvalStatus)
    {
        JournalData.JournalHeaderDataTable dt = new JournalData.JournalHeaderDataTable();
        JournalDataTableAdapters.JournalHeaderTableAdapter ta = new JournalDataTableAdapters.JournalHeaderTableAdapter();

        try
        {
            ta.FillByJournalNoTransactionDateApprovalStatus(dt, journalNo, transactionDate, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static JournalData.JournalHeaderDataTable 
        GetJournalHeaderByJournalNoTransDateDescriptionApprovalStatus(
        int? journalNo, DateTime? transactionDate, string ledgerType, string journalType,
        string headerDescription ,string approvalStatus)
    {
        JournalData.JournalHeaderDataTable dt = new JournalData.JournalHeaderDataTable();
        JournalDataTableAdapters.JournalHeaderTableAdapter ta = new JournalDataTableAdapters.JournalHeaderTableAdapter();

        try
        {
            ta.FillByJournalNoTransDateDescApproval(dt, journalNo, transactionDate, journalType, ledgerType, headerDescription, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {	
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static JournalData.JournalHeaderRow GetJournalHeaderByJournalHeaderID(decimal journalHeaderId)
    {
        JournalData.JournalHeaderDataTable dt = new JournalData.JournalHeaderDataTable();
        JournalData.JournalHeaderRow dr = null;
        JournalDataTableAdapters.JournalHeaderTableAdapter ta = new JournalDataTableAdapters.JournalHeaderTableAdapter();

        try
        {
            ta.FillByJournalHeaderID(dt, journalHeaderId);
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

    public static void UpdateJournal(decimal journalHeaderId, int journalNo, DateTime transactionDate, string ledgerType, 
        string approvalStatus,decimal currencyId, string journalType, string headerDescription, 
        string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate, 
        string approvalDesc, JournalData.JournalLineDataTable dtNewJournalLine)
    {
        //Journal Header
        JournalDataTableAdapters.JournalHeaderTableAdapter taJournalHeader = new JournalDataTableAdapters.JournalHeaderTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Check Journal Header : If nothing then insert else update
                if (journalHeaderId == 0)
                {
                    JournalData.MaxJournalNoDataTable dtMaxJournalLine = new JournalData.MaxJournalNoDataTable();
                    JournalDataTableAdapters.MaxJournalNoTableAdapter taMaxJournalLine = new JournalDataTableAdapters.MaxJournalNoTableAdapter();
                    taMaxJournalLine.Fill(dtMaxJournalLine);
                    if (dtMaxJournalLine[0].IsJournalNoNull())
                    {
                        journalNo = 1;
                    }
                    else
                    {
                        journalNo = dtMaxJournalLine[0].JournalNo + 1;
                    }
                    journalHeaderId = (decimal)taJournalHeader.InsertJournalHeader(journalNo, 
                        transactionDate, ledgerType, approvalStatus, currencyId, 
                        journalType, headerDescription, createdBy, createdDate, 
                        lastUpdatedBy, lastUpdatedDate, approvalDesc);
                }
                else
                {
                    taJournalHeader.UpdateJournalHeader(approvalStatus, approvalDesc, 
                        lastUpdatedBy, lastUpdatedDate, journalHeaderId);
                }
            
                //Journal Line
                JournalData.JournalLineDataTable dtJournalLine = 
                    new JournalData.JournalLineDataTable();
                JournalData.JournalLineRow drNewJournalLine = null;
                JournalDataTableAdapters.JournalLineTableAdapter taJournalLine = 
                    new JournalDataTableAdapters.JournalLineTableAdapter();
                taJournalLine.FillByJournalHeaderID(dtJournalLine, journalHeaderId);
                
                //Process Journal Line : add/update/delete data
                decimal iiJournalLine = 0;
              
                
                foreach (JournalData.JournalLineRow drJournalLine in dtNewJournalLine)
                {
                    drNewJournalLine = dtJournalLine.FindByJournalLineID(drJournalLine.JournalLineID);
                    
                    if (drNewJournalLine == null)
                    {
                        drNewJournalLine = dtJournalLine.NewJournalLineRow();
                        JournalData.MaxJournalLineIDDataTable dtMaxJournalLine =
                            new JournalData.MaxJournalLineIDDataTable();
                        JournalDataTableAdapters.MaxJournalLineIDTableAdapter taMaxJournalLine =
                            new JournalDataTableAdapters.MaxJournalLineIDTableAdapter();
                        //taMaxJournalLine.Fill(dtMaxJournalLine);
                        //if (dtMaxJournalLine[0].IsJournalLineIDNull())
                        //{
                        //    if (iiJournalLine == 0)
                        //    {
                        //        iiJournalLine = 1;
                        //    }
                        //    drNewJournalLine.JournalLineID = iiJournalLine;

                        //    iiJournalLine++;
                        //}
                        //else
                        //{
                        //    if (iiJournalLine == 0)
                        //    {
                        //        iiJournalLine = dtMaxJournalLine[0].JournalLineID + 1;
                        //    }

                        //    drNewJournalLine.JournalLineID = iiJournalLine;

                        //    iiJournalLine++;
                        //}
                    }

                    drNewJournalLine.JournalHeaderID = journalHeaderId;
                    drNewJournalLine.AccountID = drJournalLine.AccountID;
                    drNewJournalLine.CurrencyID = currencyId;
                    if (!drJournalLine.IsDrAmountNull())
                    {
                        drNewJournalLine.DrAmount = drJournalLine.DrAmount;
                    }
                    if (!drJournalLine.IsCrAmountNull())
                    {
                        drNewJournalLine.CrAmount = drJournalLine.CrAmount;
                    }                    
                    drNewJournalLine.LineDescription = drJournalLine.LineDescription;
                    drNewJournalLine.Attribute1Code = drJournalLine.Attribute1Code;
                    drNewJournalLine.Attribute2Code = drJournalLine.Attribute2Code;
                    drNewJournalLine.Attribute3Code = drJournalLine.Attribute3Code;
                    drNewJournalLine.Attribute4Code = drJournalLine.Attribute4Code;
                    drNewJournalLine.Attribute5Code = drJournalLine.Attribute5Code;
                    drNewJournalLine.Attribute1Description = drJournalLine.Attribute1Description;
                    drNewJournalLine.Attribute2Description = drJournalLine.Attribute2Description;
                    drNewJournalLine.Attribute3Description = drJournalLine.Attribute3Description;
                    drNewJournalLine.Attribute4Description = drJournalLine.Attribute4Description;
                    drNewJournalLine.Attribute5Description = drJournalLine.Attribute5Description;
                    drNewJournalLine.eType = "";

                    if (drNewJournalLine.RowState == DataRowState.Detached)
                    {
                        dtJournalLine.AddJournalLineRow(drNewJournalLine);
                    }

                    //taJournalLine.Insert(iiJournalLine, drJournalLine.AccountID, journalHeaderId, currencyId, drJournalLine.DrAmount, drJournalLine.CrAmount, "", "", "", "", "", "", "", "", "", "", "");
                }
                
                foreach (JournalData.JournalLineRow drJournalLine in dtJournalLine)
                {
                    if (drJournalLine.RowState == DataRowState.Unchanged)
                    {
                        drJournalLine.Delete();
                    }
                }

                taJournalLine.Update(dtJournalLine);

                string log = string.Format("JournalNo:{0}|TransactionDate:{1}|LedgerType:{2}|JournalHeaderID:{3}|" + 
                    "CurrencyID:{4}|JournalType:{5}|HeaderDescription:{6}", 
                    journalNo, transactionDate, ledgerType, journalHeaderId, currencyId, journalType, headerDescription);
                if (approvalStatus == "P")
                {
                    AuditTrail.AddAuditTrail("JournalHeader", AuditTrail.PROPOSE, log, createdBy,"Propose");
                }
                else if (approvalStatus == "A")
                {
                    AuditTrail.AddAuditTrail("JournalHeader", AuditTrail.APPROVE, log, createdBy,"Approve");
                }
                else if (approvalStatus == "R")
                {
                    AuditTrail.AddAuditTrail("JournalHeader", AuditTrail.REJECT, log, createdBy,"Reject");
                }

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
    #endregion

    #region "-- JournalLine --"
    public static JournalData.JournalLineDataTable GetJournalLineByJournalHeaderID(decimal journalHeaderId)
    {
        JournalData.JournalLineDataTable dt = new JournalData.JournalLineDataTable();
        JournalDataTableAdapters.JournalLineTableAdapter ta = new JournalDataTableAdapters.JournalLineTableAdapter();

        try
        {
            ta.FillByJournalHeaderID(dt, journalHeaderId);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion

}
