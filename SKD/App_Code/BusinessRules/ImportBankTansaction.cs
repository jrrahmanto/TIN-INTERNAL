using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for ImportBankTansaction
/// </summary>
public class ImportBankTansaction : ExcelImportHandler 
{
	private BankData.BankTransactionDataTable dt;
    
    private string userName;
    private DateTime bussDate;
    private int trxNo;
    private decimal idBank;

    public ImportBankTansaction(Type recordType, string userN, DateTime businessDate, int transactionNo, decimal bankId)
        : base(recordType)
    {
        //this.ExcelSheetName = "Sheet1$";
        userName = userN;
        bussDate = businessDate;
        trxNo = transactionNo;
        idBank = bankId;
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
        if (this.CurrentLine >= 5)
        {

            try
            {
                if (dt == null)
                {
                    dt = new BankData.BankTransactionDataTable();
                }

                //Guard when there is no line to parse
                if (obj.ToString().StartsWith("\t"))
                {
                    return;
                }

                object objParse = null;
                objParse = LineToObject(obj.ToString());
                ClsImportBankTransaction cls = (ClsImportBankTransaction)objParse;
                StringBuilder sb = new StringBuilder();
                decimal bankAccountID = 0;
                string trxType = "";
                 DateTime paymentDate = DateTime.MinValue;
                decimal amount = 0;

                //Validation
                // 1. Date
                try
                {
                    DateTime tanggal;
                    DateTime.TryParseExact(cls.TransactionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggal);
                    paymentDate = tanggal;

                }
                catch(Exception ex)
                {
                    sb.Append("Payment Date is not valid: " + ex.Message);
                }

                // 2. BankAccountNo
                try
                {
                    if (cls.DestinationAccount == "")
                        throw new ApplicationException("Bank Account may not be empty.");

                    BankData.BankAccountDataTable dtBankAccount = BankAccount.SelectBankAccountbyAccountNoAndBankID(idBank, cls.DestinationAccount, paymentDate);
                   // BankData.BankAccountDataTable dtBankAccount = BankAccount.SelectBankAccountbyBankIDAndEffectiveEndDate(cls.DestinationAccount, paymentDate);
                    if (dtBankAccount.Count == 0)
                    {
                        throw new ApplicationException("Bank Account is not registered: " + cls.DestinationAccount + ".");
                    }
                    else
                        bankAccountID = dtBankAccount[0].BankAccountID;
                }
                catch (Exception ex)
                {
                    if (sb.Length != 0) sb.Append(", ");
                    sb.Append(ex.Message);
                }

                try
                {
                    BankData.BankAccountDataTable dtBankAccount = BankAccount.SelectBankAccountByAccountNo(cls.DestinationAccount);
                    if (dtBankAccount.Count == 0)
                    {
                        throw new ApplicationException("Account Number is not registered: " + cls.DestinationAccount + ".");
                    }
                    else
                    { 
                        trxType = dtBankAccount[0].AccountType;
                        
                    }
                }
                catch (Exception ex)
                {

                }

                // 3. Amount
                try
                {
                    int counter = cls.Amount.Split('.').Length - 1;
                    if(counter > 1)
                    {
                        //berarti formatnya 1.000.000,00
                        String nominal = cls.Amount.Replace(".", String.Empty).Replace(',', '.');
                        amount = decimal.Parse(nominal);
                        //sb.Append("counter > 1 : " + amount);
                    }
                    else
                    {
                        //berarti formatnya 1,000,000.00
                        amount = decimal.Parse(cls.Amount);
                        //sb.Append("counter = 1 : " + amount);
                    }
                    
                }
                catch(Exception)
                {
                    if (sb.Length != 0) sb.Append(", ");
                    sb.Append("Amount is not a valid number (" + cls.Amount + ").");
                }

                if (sb.Length > 0)
                {
                    throw new ApplicationException(sb.ToString());
                }

                // pass all validation
                //Bank.InsertBankTransaction("A", paymentDate, bussDate, "D", bankAccountID, null, null, null, null, null, 1, "",
                //    amount, "D", "D", cls.Channel, cls.Trace, userName, DateTime.Now, userName, DateTime.Now, null);


                BankData.BankTransactionRow dr = null;

                dr = dt.NewBankTransactionRow();
                dr.TransactionNo = trxNo +1 ;
                dr.SourceAcctID = bankAccountID;
                dr.TransactionTime = paymentDate;
                dr.Amount = amount;
                dr.TransactionType = "D";
                dr.MutationType = "C";
                dr.TransactionDescription = cls.Channel;
                dr.BankReference = cls.Trace;
                dr.ReceiveTime = bussDate;

                dr.CreatedBy = userName;
                dr.CreatedDate = DateTime.Now;
                dr.LastUpdatedBy = userName;
                dr.LastUpdatedDate = DateTime.Now;
                dr.ApprovalStatus = "A";

                dt.AddBankTransactionRow(dr);

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
                Bank.ImportBankTransaction(dt);
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