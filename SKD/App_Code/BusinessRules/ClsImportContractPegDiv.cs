using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for ClsImportContractPegDiv
/// </summary>
public class ClsImportContractPegDiv
{
    [DelimiterRecord(0, "")] public string CommodityCode;
    [DelimiterRecord(1, "")] public int month;
    [DelimiterRecord(2, "")] public int year;
    [DelimiterRecord(3, "")] public decimal Peg;
    [DelimiterRecord(4, "")] public decimal Divisor;
}
