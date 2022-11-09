using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ImportTradeFeed
/// </summary>
public class ImportTradeFeedSimulation : TextImportHandler
{

    private string user;
    private decimal exchangeID;
    private DateTime bussDate;
    private string sessionUniqId; 
    
    private TradefeedData.RawTradeFeedSimulationDataTable dt;

    #region "   Member Variables   "

    // hold data
    private decimal _identifier;
    private DateTime _tradeTime;
    private int _offset;
    private string _productCode;
    private string _settlementMonth;
    private string _optionCode;
    private decimal _strikePrice;
    private int _tradeVersion;
    private decimal _price;
    private decimal _qty;
    private string _contraIndicator;
    private string _exchangeReference;

    private string _sellerClearingMemberCode;
    private string _sellerExchangeMemberCode;
    private string _sellerTraderTypeCode;
    private string _sellerAccountCode;
    private string _sellerMemberGiveUpCode;
    private decimal _sellerGiveUpCommission;
    private string _sellerCompositeTradeTypeCode;
    private int _sellerTotalLegs;
    private string _sellerReference;

    private string _buyerClearingMemberCode;
    private string _buyerExchangeMemberCode;
    private string _buyerTraderTypeCode;
    private string _buyerAccountCode;
    private string _buyerMemberGiveUpCode;
    private decimal _buyerGiveUpCommission;
    private string _buyerCompositeTradeTypeCode;
    private int _buyerTotalLegs;
    private string _buyerReference;

    // hold remaining message
    private string _remainingMessage;
    #endregion


    public ImportTradeFeedSimulation(Type recordType, string userName, decimal exchID
            , DateTime bussinessDate, string sessionId)
        : base(recordType)
	{
        user = userName;
        exchangeID = exchID;
        bussDate = bussinessDate;
        if (!string.IsNullOrEmpty(sessionId))
        {
            sessionUniqId = sessionId;
        }
        else
        {
            sessionUniqId = Guid.NewGuid().ToString();
        }

        dt = new TradefeedData.RawTradeFeedSimulationDataTable();
	}

    public override void Initialize()
    {
        base.Initialize();

    }

    public override void ProcessRow(object obj)
    {
        try
        {
            this.Parse(obj.ToString());

            if (dt == null)
            {
                dt = new TradefeedData.RawTradeFeedSimulationDataTable();
            }

            //dt.AddRawTradeFeedSimulationRow(exchangeID, _identifier, bussDate, _tradeTime, DateTime.Now, 
            //                      _offset, _optionCode, _productCode, decimal.Parse(_settlementMonth), 
            //                      _price, _qty, _contraIndicator, _exchangeReference, _sellerClearingMemberCode, 
            //                      _sellerMemberGiveUpCode, _sellerGiveUpCommission, _sellerReference, 
            //                      _sellerTraderTypeCode, _sellerCompositeTradeTypeCode, _sellerTotalLegs, 
            //                      _buyerAccountCode, _buyerTraderTypeCode, _buyerMemberGiveUpCode, 
            //                      _buyerGiveUpCommission, _buyerCompositeTradeTypeCode, _buyerReference, 
            //                      _strikePrice, _tradeVersion, _buyerTotalLegs, _sellerAccountCode, 
            //                      _buyerClearingMemberCode,  _productCode, _sellerExchangeMemberCode, _buyerExchangeMemberCode,
            //                      user, sessionUniqId);
            dt.AddRawTradeFeedSimulationRow(exchangeID, _identifier, bussDate, _tradeTime, DateTime.Now,
                                                _offset, _optionCode, _productCode, decimal.Parse(_settlementMonth),
                                                _price, _qty, _exchangeReference, _sellerClearingMemberCode,
                                                _sellerExchangeMemberCode, _sellerAccountCode, _buyerClearingMemberCode,
                                                _buyerExchangeMemberCode, _buyerAccountCode, _strikePrice,
                                                _contraIndicator, _sellerMemberGiveUpCode, _sellerGiveUpCommission,
                                                _sellerReference, _sellerTraderTypeCode, _sellerCompositeTradeTypeCode,
                                                _sellerTotalLegs, _buyerTraderTypeCode, _buyerMemberGiveUpCode,
                                                _buyerGiveUpCommission, _buyerCompositeTradeTypeCode, _buyerReference,
                                                _tradeVersion, _buyerTotalLegs, user, sessionUniqId);
        }
        catch (Exception ex)
        {
            this.IsCancel = true;
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void Parse(string message)
    {
        StringBuilder sb = new StringBuilder();

        // validate arguments
        if (message == "")
        {
            sb.AppendLine("E:Trade Message is Empty");
        }
        if (message.Substring(0, 1) != "T") 
        {
            sb.AppendLine("E:Argument is not Trade Message");
        }

        // ---------------------------- get data ---------------------------- 
        int intOut;
        decimal decimalOut;

        //Trade time
        if (int.TryParse(message.Substring(1, 6), out intOut) == false)
        {
            sb.AppendLine("D:Identifier is not a valid number");
        }
        else
        {
            _identifier = intOut;
        }
        try
        {
            _tradeTime = new DateTime(
                            int.Parse(message.Substring(7, 4)),
                            int.Parse(message.Substring(11, 2)),
                            int.Parse(message.Substring(13, 2)),
                            int.Parse(message.Substring(15, 2)),
                            int.Parse(message.Substring(17, 2)),
                            int.Parse(message.Substring(19, 2)),
                            int.Parse(message.Substring(22, 9)));
        }
        catch 
        { 
            sb.AppendLine("D:Trade time is not a valid date value"); 
        }
        if (int.TryParse(message.Substring(31, 4), out intOut) == false)
        {
            sb.AppendLine("D:Offset is not a valid number");
        }
        else if (intOut < -720 || intOut > 720)
        {
            sb.AppendLine("D:Offset out of range");
        }
        else
        {
            _offset = intOut;
        }
        _productCode = message.Substring(35, 20).Trim();
        
        if (int.TryParse(message.Substring(55, 4), out intOut) == false &&
            int.TryParse(message.Substring(59, 2), out intOut) == false)
        {
            sb.AppendLine("Invalid contract month");
        }
        else
        {
            _settlementMonth = message.Substring(55, 6);
            if (int.Parse(message.Substring(55, 4)) < 2000 || int.Parse(message.Substring(55, 4)) > 9999)
            {
                sb.AppendLine("D:MonthContract (Year) out of range");
            }
            if (int.Parse(message.Substring(59, 2)) < 1 || int.Parse(message.Substring(59, 2)) > 12)
            {
                sb.AppendLine("D:MonthContract (Month) out of range");
            }
        }

        _optionCode = message.Substring(61, 1);
        if (_optionCode != "P" && _optionCode != "C" && _optionCode != " ")
        {
            sb.AppendLine("C:Invalid Option Code");
        }
        if (decimal.TryParse(message.Substring(62, 10), out decimalOut) == false)
        {
            sb.AppendLine("D:Strike price is not a valid number");
        }
        else
        {
            _strikePrice = decimalOut;
        }
        if (int.TryParse(message.Substring(72, 1), out intOut) == false)
        {
            sb.AppendLine("D:Trade version is not a valid number");
        }
        else
        {
            _tradeVersion = intOut;
        }
        if (decimal.TryParse(message.Substring(73, 10), out decimalOut) == false)
        {
            sb.AppendLine("D:Price is not a valid number");
        }
        else
        {
            _price = decimalOut;
        }
        if (decimal.TryParse(message.Substring(83, 6), out decimalOut) == false)
        {
            sb.AppendLine("D:Qty is not a valid number");
        }
        else
        {
            _qty = decimalOut;
        }
        _contraIndicator = message.Substring(89, 1);

        if (_contraIndicator != "C" && _contraIndicator != " ")
        {
            sb.AppendLine("C:Invalid Contra Indicator");
        }
        _exchangeReference = message.Substring(90, 10).Trim();
        _sellerClearingMemberCode = message.Substring(100, 5).Trim();
        _sellerExchangeMemberCode = message.Substring(105, 5).Trim();

        _sellerTraderTypeCode = message.Substring(110, 1);
        if (_sellerTraderTypeCode != "T" && _sellerTraderTypeCode != " ")
        {
            sb.AppendLine("C:Invalid Seller Composite Trade Type Code");
        }
        if (_sellerTraderTypeCode != "L" && _sellerTraderTypeCode != " ")
        {
            sb.AppendLine("C:Invalid Seller Trade Type Code");
        }
        _sellerAccountCode = message.Substring(111, 10).Trim();
        _sellerMemberGiveUpCode = message.Substring(121, 5).Trim();
        if (decimal.TryParse(message.Substring(126, 10), out decimalOut) == false)
        {
            sb.AppendLine("D:Seller Give Up Commission is not a valid number");
        }
        else if (decimalOut < (decimal)-0.01 ||
            decimalOut > (decimal)9999999)
        {
            sb.AppendLine("D:Seller Give Up Commission out Offset range");
        }
        else
        {
            _sellerGiveUpCommission = decimalOut;
        }
        _sellerCompositeTradeTypeCode = message.Substring(136, 1);

        if (int.TryParse(message.Substring(137, 1), out intOut) == false)
        {
            sb.AppendLine("D:Seller Total Legs is not a valid number");
        }
        else
        {
            _sellerTotalLegs = intOut;
        }
        _sellerReference = message.Substring(138, 40).Trim();
        _buyerClearingMemberCode = message.Substring(178, 5).Trim();
        _buyerExchangeMemberCode = message.Substring(183, 5).Trim();
        _buyerTraderTypeCode = message.Substring(188, 1);
        if (_buyerTraderTypeCode != "L" && _buyerTraderTypeCode != " ")
        {
            sb.AppendLine("C:Invalid Buyer Trade Type Code");
        }
        _buyerAccountCode = message.Substring(189, 10).Trim();
        _buyerMemberGiveUpCode = message.Substring(199, 5).Trim();
        if (decimal.TryParse(message.Substring(204, 10), out decimalOut) == false)
        {
            sb.AppendLine("D:Buyer Give Up Commission is not a valid number");
        }
        else if (decimalOut < (decimal)-0.01 ||
            decimalOut > (decimal)9999999)
        {
            sb.AppendLine("D:Buyer Give Up Commission out Offset range");
        }
        else
        {
            _buyerGiveUpCommission = decimalOut;
        }
        _buyerCompositeTradeTypeCode = message.Substring(214, 1);

        if (_buyerCompositeTradeTypeCode != "T" && _buyerCompositeTradeTypeCode != " ")
        {
            sb.AppendLine("C:Invalid Buyer Composite Trade Type Code");
        }
        if (int.TryParse(message.Substring(215, 1), out intOut) == false)
        {
            sb.AppendLine("D:Buyer Total Legs is not a valid number");
        }
        else
        {
            _buyerTotalLegs = intOut;
        }
        _buyerReference = message.Substring(216, 40).Trim();

        // ---------------------------- end of get data ---------------------------- 
        if (sb.Length > 0)
        {
            throw new ApplicationException(sb.ToString());
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
            try
            {
                Tradefeed.InsertRawTradeFeedSimulation(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}
