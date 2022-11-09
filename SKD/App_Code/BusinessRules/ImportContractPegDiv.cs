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
public class ImportContractPegDiv : ExcelImportHandler 
{

    private ContractData.ContractPegDivDataTable dt; 
    
    private string userName;
    private decimal exchangeID;
    private DateTime bussDate;

    public ImportContractPegDiv(Type recordType, string userN, decimal exchID)
        : base(recordType)
    {
        this.ExcelSheetName = "ContractPegDiv$";
        userName = userN;
        exchangeID = exchID;
        bussDate = DateTime.Today;
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
                    dt = new ContractData.ContractPegDivDataTable();
                }

                //Guard when there is no line to parse
                if (obj.ToString().StartsWith("\t"))
                {
                    return;
                }

                object objParse = null;
                objParse = LineToObject(obj.ToString());
                ClsImportContractPegDiv cls = (ClsImportContractPegDiv)objParse;               

                // Validation
                StringBuilder sb = new StringBuilder();
                decimal commId = 0;
                decimal contractId = 0;
                try
                {
                    CommodityData.CommodityDataTable dtComm = Commodity.GetCommodityByExchangeIdApprovalCommCodeActive(exchangeID, "A",
                        cls.CommodityCode.ToUpper(), bussDate);
                    if (dtComm.Count == 0)
                    {
                        throw new ApplicationException(String.Format("Line {0}: Commodity not found.", this.CurrentLine-1));
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
                        cls.month, cls.year, "A", bussDate);
                    if (dtContract.Count == 0)
                    {
                        throw new ApplicationException(String.Format("Line {0}: Contract not found.", this.CurrentLine-1));
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

                ContractData.ContractPegDivRow dr = null;
                dr = dt.NewContractPegDivRow();
                dr.BussDate = bussDate;
                dr.ExchangeId = exchangeID;
                dr.PEG = cls.Peg;
                dr.Divisor = cls.Divisor;
                dr.CommodityCode = cls.CommodityCode;
                dr.ContractMonth = cls.month;
                dr.ContractYear = cls.year;
                dr.UserName = userName;

                dt.AddContractPegDivRow(dr);
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
                Contract.ImportPegDiv(dt);
            }
        }            
    }

}
