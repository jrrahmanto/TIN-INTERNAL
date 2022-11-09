using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for ClsImportBankTransaction
/// </summary>
public class ClsImportBankTransaction
{
    [DelimiterRecord(0,"")] public int No;
    [DelimiterRecord(1,"")] public string TransactionTime;
    [DelimiterRecord(2,"")] public string SourceAccount;
    [DelimiterRecord(3,"")] public string DestinationAccount;
    [DelimiterRecord(4,"")] public string TenantName;
    [DelimiterRecord(5,"")] public string Amount;
    [DelimiterRecord(6,"")] public string Trace;
    [DelimiterRecord(7,"")] public string Channel;
}