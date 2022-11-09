using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for ClsImportSettlePrice
/// </summary>
public class ClsImportSettlePrice
{
	 [DelimiterRecord(0,"")] public string CMName;
     [DelimiterRecord(1,"")] public string month;
     [DelimiterRecord(3,"")] public int year;
     [DelimiterRecord(2,"")] public decimal settlePrice;
}
