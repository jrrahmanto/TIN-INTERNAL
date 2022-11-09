using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RptEOD
/// </summary>
public class RptEOD
{

    public static RptEODData.ListeDODataTable GetRptListeDO(DateTime eDODate)
    {
        try
        {
            RptEODData.ListeDODataTable dt = new RptEODData.ListeDODataTable();
            RptEODDataTableAdapters.ListeDOTableAdapter ta = new RptEODDataTableAdapters.ListeDOTableAdapter();
            ta.Fill(dt, eDODate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static bool IsGenerateMarginCall(DateTime businessDate,
       decimal clearingMemberID)
    {
        bool isGenerate = false;

        RptEODData.ListeDODataTable dt = new RptEODData.ListeDODataTable();
        RptEODDataTableAdapters.ListeDOTableAdapter ta = new RptEODDataTableAdapters.ListeDOTableAdapter();

        try
        {
            ta.Fill(dt, businessDate);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static bool IsGenerateNotaPemberitahuan(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.NotaPemberitahuanDataTable dt = new  RptEODData.NotaPemberitahuanDataTable();
        RptEODDataTableAdapters.NotaPemberitahuanTableAdapter ta = new RptEODDataTableAdapters.NotaPemberitahuanTableAdapter();

        try
        {
            ta.Fill(dt, buyerInvoice, clearingMemberID, code);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsGenerateeDO(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.eDODataTable dt = new RptEODData.eDODataTable();
        RptEODDataTableAdapters.eDOTableAdapter ta = new RptEODDataTableAdapters.eDOTableAdapter();

        try
        {
            ta.Fill(dt,null,  clearingMemberID, code,buyerInvoice);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsGenerateTradeReg(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.TradeRegisterDataTable dt = new RptEODData.TradeRegisterDataTable();
        RptEODDataTableAdapters.TradeRegisterTableAdapter ta = new RptEODDataTableAdapters.TradeRegisterTableAdapter();

        try
        {
            ta.Fill(dt, buyerInvoice,code, clearingMemberID);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static bool IsGenerateDFS(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.DFSDataTable dt = new RptEODData.DFSDataTable();
        RptEODDataTableAdapters.DFSTableAdapter ta = new RptEODDataTableAdapters.DFSTableAdapter();

        try
        {
            ta.Fill(dt, buyerInvoice, code, clearingMemberID);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static bool IsGenerateRincianDFSSeller(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.RincianDFSSellerDataTable dt = new RptEODData.RincianDFSSellerDataTable();
        RptEODDataTableAdapters.RincianDFSSellerTableAdapter ta = new RptEODDataTableAdapters.RincianDFSSellerTableAdapter();

        try
        {
            ta.Fill(dt,clearingMemberID,buyerInvoice, code);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static bool IsGenerateRincianDFSBuyer(DateTime buyerInvoice,
       decimal clearingMemberID, string code)
    {
        bool isGenerate = false;

        RptEODData.RincianDFSBuyerDataTable dt = new RptEODData.RincianDFSBuyerDataTable();
        RptEODDataTableAdapters.RincianDFSBuyerTableAdapter ta = new RptEODDataTableAdapters.RincianDFSBuyerTableAdapter();

        try
        {
            ta.Fill(dt, clearingMemberID, code, buyerInvoice);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
    public static bool IsGeneratTradeRegistryListing(decimal clearingMemberID, DateTime businessDate, string code)
    {
        bool isGenerate = false;

        RptEODData.TradeRegisterDataTable dt = new RptEODData.TradeRegisterDataTable();
        RptEODDataTableAdapters.TradeRegisterTableAdapter ta = new RptEODDataTableAdapters.TradeRegisterTableAdapter();

        try
        {
            ta.Fill(dt, businessDate, code, clearingMemberID);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsGenerateTradeConfirmationBuyer(DateTime businessDate, decimal clearingMemberID)
    {
        bool isGenerate = false;

        RptEODData.TradeConfirmationDataTable dt = new RptEODData.TradeConfirmationDataTable();
        RptEODDataTableAdapters.TradeConfirmationTableAdapter ta = new RptEODDataTableAdapters.TradeConfirmationTableAdapter();

        try
        {
            ta.FillTradeConfirmationByBusinessDateAndCMID(dt, businessDate, clearingMemberID);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsGenerateTradeConfirmationSeller(DateTime businessDate, decimal clearingMemberID)
    {
        bool isGenerate = false;

        RptEODData.TradeConfirmationDataTable dt = new RptEODData.TradeConfirmationDataTable();
        RptEODDataTableAdapters.TradeConfirmationTableAdapter ta = new RptEODDataTableAdapters.TradeConfirmationTableAdapter();

        try
        {
            ta.FillTradeConfirmationByBusinessDateAndCMID(dt, businessDate, clearingMemberID);

            if (dt.Count > 0)
            {
                isGenerate = true;
            }

            return isGenerate;
        }
        catch (Exception ex)
        {

            throw new ApplicationException(ex.Message, ex);
        }
    }
}