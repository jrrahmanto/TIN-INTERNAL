using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for ClsImportSettlePrice
/// </summary>
public class ClsImportIrca
{
	 [DelimiterRecord(0,"")] public string CommodityCode;
     [DelimiterRecord(1,"")] public string EffectiveStartDate;
     [DelimiterRecord(2,"")] public decimal IrcaValue;
     [DelimiterRecord(3,"")] public string EffectiveEndDate;
}
