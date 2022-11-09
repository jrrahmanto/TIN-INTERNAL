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
public class ImportSettlementPrice : ExcelImportHandler 
{

    private SettlementPriceData.SettlementPriceDataTable dt; // = new SettlementPriceData.ImportDataDataTable();
    
    private string userName;
    private decimal exchangeID;
    private DateTime bussDate;
    private bool autoApproval;

    public ImportSettlementPrice(Type recordType, string userN, decimal exchID, DateTime businessDate, bool autoApprv)
        : base(recordType)
    {
        //this.ExcelSheetName = "Sheet1$";
        userName = userN;
        exchangeID = exchID;
        bussDate = businessDate;
        autoApproval = autoApprv;
    }

    public override void Initialize()
    {
        base.Initialize();
        
    }

    public override void ProcessRow(object obj)
    {
        if (this.CurrentLine == 3)
        {
            DateTime tmpBussDate;

            try
            {
                tmpBussDate = Convert.ToDateTime(obj.ToString().Trim(), new System.Globalization.CultureInfo("en-us"));
            }
            catch
            {
                try
                {
                    tmpBussDate = Convert.ToDateTime(obj.ToString().Trim(), new System.Globalization.CultureInfo("id-id"));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to get business date.", ex);
                }
            }

            if (tmpBussDate != bussDate)
            {
                throw new ApplicationException("Business date in Excel (" + tmpBussDate.ToString("dd-MMM-yyyy") + ") must equal with on entry.");
            }
            else
            {
                if (autoApproval) SettlementPrice.DeleteByBusinessDate(exchangeID, bussDate);
            }

        }
        if (this.CurrentLine >= 6)
        {
            //parse
            // ad to datatable
            //dt.AddImportDataRow(
            try
            {
                if (dt == null)
                {
                    dt = new SettlementPriceData.SettlementPriceDataTable();
                }

                //Guard when there is no line to parse
                if (obj.ToString().StartsWith("\t"))
                {
                    return;
                }

                object objParse = null;
                objParse = LineToObject(obj.ToString());
                ClsImportSettlePrice cls = (ClsImportSettlePrice)objParse;               

                //Validation
                StringBuilder sb = new StringBuilder();
                decimal commId = 0;
                decimal contractId = 0;
                try
                {
                    CommodityData.CommodityDataTable dtComm = Commodity.GetCommodityByExchangeIdApprovalCommCodeActive(exchangeID, "A",
                        cls.CMName.ToUpper(), bussDate);
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
                try
                {
                    ContractData.ContractDataTable dtContract =  Contract.GetContractByCommodityIdContractMonthYearApprovalActive(commId,
                        MonthStringToInt(cls.month), cls.year, "A", bussDate);
                    if (dtContract.Count == 0)
                    {
                        throw new ApplicationException("Contract not found.");
                    }
                    contractId = dtContract[0].ContractID;
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);
                }

                if (sb.Length > 0)
                {
                    throw new ApplicationException(sb.ToString());
                }


                SettlementPriceData.SettlementPriceRow dr = null;
                dr = dt.NewSettlementPriceRow();
                dr.BusinessDate = bussDate;
                dr.ContractID = contractId;
                dr.SettlementPriceType = "N";
                dr.ApprovalStatus = (autoApproval)? "A": "P";
                dr.SettlementPrice = cls.settlePrice;
                dr.CreatedBy = userName;
                dr.CreatedDate = DateTime.Now;
                dr.LastUpdatedBy = userName;
                dr.LastUpdatedDate = DateTime.Now;
                if (!autoApproval) dr.ActionFlag = "I";

                dt.AddSettlementPriceRow(dr);
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
                SettlementPrice.ImportSettlementPrice(dt);
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
