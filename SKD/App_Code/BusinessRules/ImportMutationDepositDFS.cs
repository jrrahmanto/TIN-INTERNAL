using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for ImportMutationDeposit
/// </summary>
public class ImportMutationDepositDFS : ExcelImportHandler 
{
	private MutationDepositDFS.MutationDepositDFSDataTable dt;
    
    private string userName;
    private DateTime bussDate;
    private int trxNo;

    public ImportMutationDepositDFS(Type recordType, string userN, DateTime businessDate, int transactionNo)
        : base(recordType)
    {
        //this.ExcelSheetName = "Sheet1$";
        userName = userN;
        bussDate = businessDate;
        trxNo = transactionNo+1;
    }

    public override void Initialize()
    {
        base.Initialize();
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public override void ProcessRow(object obj)
    {
 
        // Start of data
        if (this.CurrentLine >= 2)
        {

            try
            {
                if (dt == null)
                {
                    dt = new MutationDepositDFS.MutationDepositDFSDataTable();
                }

                //Guard when there is no line to parse
                if (obj.ToString().StartsWith("\t"))
                {
                    return;
                }

                object objParse = null;
                objParse = LineToObject(obj.ToString());
                ClsImportMutationDeposit cls = (ClsImportMutationDeposit)objParse;
                StringBuilder sb = new StringBuilder();
                decimal bankAccountID = 0;
                DateTime mutationDate = DateTime.MinValue;
                decimal amount = 0;

                //Validation
                // 1. Date
                try
                {
                    mutationDate = DateTime.Parse(cls.MutationDate);

                }
                catch(Exception ex)
                {
                    sb.Append("Mutation Date is not valid: " + ex.Message);
                }

                // 2. BankAccountNo
                try
                {
                    if (cls.BankAccount == "")
                        throw new ApplicationException("Bank Account may not be empty.");

                    //BankData.BankAccountDataTable dtBankAccount = BankAccount.SelectBankAccountbyAccountNoAndBankID(1, cls.DestinationAccount, paymentDate);
                    BankData.BankAccountDataTable dtBankAccount = BankAccount.SelectBankAccountbyBankIDAndEffectiveEndDate(cls.BankAccount, mutationDate);
                    if (dtBankAccount.Count == 0)
                    {
                        throw new ApplicationException("Bank Account is not registered: " + cls.BankAccount + ".");
                    }
                    else
                        bankAccountID = dtBankAccount[0].BankAccountID;
                }
                catch (Exception ex)
                {
                    if (sb.Length != 0) sb.Append(", ");
                    sb.Append(ex.Message);
                }

                // 3. Amount
                try
                {
                    amount = decimal.Parse(cls.Amount);
                }
                catch(Exception)
                {
                    if (sb.Length != 0) sb.Append(", ");
                    sb.Append("Amount is not a valid number.");
                }

                if (sb.Length > 0)
                {
                    throw new ApplicationException(sb.ToString());
                }

                // pass all validation
                //Bank.InsertBankTransaction("A", paymentDate, bussDate, "D", bankAccountID, null, null, null, null, null, 1, "",
                //    amount, "D", "D", cls.Channel, cls.Trace, userName, DateTime.Now, userName, DateTime.Now, null);


                MutationDepositDFS.MutationDepositDFSRow dr = null;

                dr = dt.NewMutationDepositDFSRow();
                dr.MutationNo = trxNo;
                dr.BankAcctID = bankAccountID;
                dr.Amount = amount;
                dr.EntryDate = bussDate;

                dr.CreatedBy = userName;
                dr.CreatedDate = DateTime.Now;
                dr.LastUpdatedBy = userName;
                dr.LastUpdatedDate = DateTime.Now;
                dr.ApprovalStatus = "A";

                dt.AddMutationDepositDFSRow(dr);

                trxNo++;
            }
            catch (Exception ex)
            {
                this.IsCancel = false;
                throw new ApplicationException(ex.Message, ex);
            }
        }
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        if (this.TotalError > 0)
        {
            if (dt != null)
            {
                dt.RejectChanges();
            }
        }
        else
        {
            if (dt != null)
            {
                MutationDeposit.ImportMutationDepositDFS(dt);
            }
        }            
    }

    private int MonthStringToInt(string strMonth)
    {
        int intMonth = 0;

        switch (strMonth)
        {
            case "1":
            case "JAN": 
            case "JANUARY":
            case "JANUARI":
                intMonth = 1;
                break;

            case "2":
            case "FEB":
            case "PEB":
            case "FEBRUARY":
            case "PEBRUARI":
                intMonth = 2;
                break;

            case "3":
            case "MAR":
            case "MARCH":
            case "MARET":
                intMonth = 3;
                break;

            case "4":
            case "APR":
            case "APRIL":
                intMonth = 4;
                break;

            case "5":
            case "MAY":
            case "MEI":
                intMonth = 5;
                break;

            case "6":
            case "JUN":
            case "JUNE":
            case "JUNI":
                intMonth = 6;
                break;

            case "7":
            case "JUL":
            case "JULY":
            case "JULI":
                intMonth = 7;
                break;

            case "8":
            case "AUG":
            case "AGU":
            case "AGUST":
            case "AUGUST":
            case "AGUSTUS":
                intMonth = 8;
                break;

            case "9":
            case "SEP":
            case "SEPT":
            case "SEPTEMBER":
                intMonth = 9;
                break;

            case "10":
            case "OCT":
            case "OKT":
            case "OCTOBER":
            case "OKTOBER":
                intMonth = 10;
                break;

            case "11":
            case "NOV":
            case "NOP":
            case "NOVEMBER":
            case "NOPEMBER":
                intMonth = 11;
                break;

            case "12":
            case "DEC":
            case "DES":
            case "DECEMBER":
            case "DESEMBER":
            case "GULIR":
            case "ROL":
            case "ROLLING":
                intMonth = 12;
                break;

            default:
                break;
        }

        if (intMonth == 0) throw new Exception("Unknown month input: " + strMonth);

        return intMonth;
    }
}