using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for ClsMutationDeposit
/// </summary>
public class ClsImportMutationDeposit
{
    [DelimiterRecord(0,"")] public int No;
    [DelimiterRecord(1,"")] public string MutationDate;
    [DelimiterRecord(2,"")] public string BankAccount;
    [DelimiterRecord(3,"")] public string Amount;
}