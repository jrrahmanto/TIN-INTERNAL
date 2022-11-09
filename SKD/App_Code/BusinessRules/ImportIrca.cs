using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Text;
using System.Globalization;

/// <summary>
/// Summary description for ImportSettlementPrice
/// </summary>
public class ImportIrca : ExcelImportHandler 
{

    private IRCAData.IRCADataTable dt; // = new SettlementPriceData.ImportDataDataTable();
    
    private string userName;
    private string ApprovalDesc;
    private DateTime bussDate;

    public ImportIrca(Type recordType, string userN, string ApprvDesc)
        : base(recordType)
    {
        //this.ExcelSheetName = "Sheet1$";
        userName = userN;
        ApprovalDesc = ApprvDesc;
    }

    public override void Initialize()
    {
        base.Initialize();
        
    }

    public override void ProcessRow(object obj)
    {
        if (this.CurrentLine >= 1)
        {
            //parse
            // ad to datatable
            //dt.AddImportDataRow(
            try
            {
                if (dt == null)
                {
                    dt = new IRCAData.IRCADataTable();
                }

                //Guard when there is no line to parse
                if (obj.ToString().StartsWith("\t"))
                {
                    return;
                }

                object objParse = null;
                objParse = LineToObject(obj.ToString());
                ClsImportIrca cls = (ClsImportIrca)objParse;               

                //Validation
                StringBuilder sb = new StringBuilder();
                decimal commId = 0;
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.MinValue;
                cls.EffectiveEndDate = cls.EffectiveEndDate.TrimEnd('\r', '\n');
                try
                {
                    startDate = Convert.ToDateTime(cls.EffectiveStartDate, new System.Globalization.CultureInfo("en-us"));

                    if (!string.IsNullOrEmpty(cls.EffectiveEndDate)) {
                        endDate = Convert.ToDateTime(cls.EffectiveEndDate, new System.Globalization.CultureInfo("en-us"));
                    }
                }
                catch(Exception ex)
                {
                     throw new ApplicationException("Failed to get Effective Start Date or Effective End Date.", ex);
                }

                try
                {
                    CommodityData.CommodityDataTable dtComm = Commodity.GetCommodityByCode(cls.CommodityCode);
                    if (dtComm.Count == 0)
                    {
                        throw new ApplicationException("Commodity not found.");
                    }
                    commId = dtComm[0].CommodityID;
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);
                }

                if (sb.Length > 0)
                {
                    throw new ApplicationException(sb.ToString());
                }


                IRCAData.IRCARow dr = null;
                dr = dt.NewIRCARow();
                dr.CommodityID = commId;
                dr.EffectiveStartDate = startDate;
                if (endDate != DateTime.MinValue) 
                {
                    dr.EffectiveEndDate = endDate;
                }
                dr.IRCAValue = cls.IrcaValue;
                dr.ApprovalStatus = "A";
                dr.ApprovalDesc = ApprovalDesc;
                dr.CreatedBy = userName;
                dr.CreatedDate = DateTime.Now;
                dr.LastUpdatedBy = userName;
                dr.LastUpdatedDate = DateTime.Now;
                dt.AddIRCARow(dr);
            }
            catch (Exception ex)
            {
                this.IsCancel = true;
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
                IRCA.ImportIrcaAutoApprv(dt, userName);
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
