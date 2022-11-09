using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TradefeedSimulation
/// </summary>
public class TradefeedSimulation
{
    public TradefeedSimulation()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<string> ValidateContract()
    {
        List<string> result = new List<string>();

        TradefeedSimulationDataTableAdapters.ContractCheckTableAdapter ta = new TradefeedSimulationDataTableAdapters.ContractCheckTableAdapter();
        TradefeedSimulationData.ContractCheckDataTable dt = new TradefeedSimulationData.ContractCheckDataTable();

        try
        {
            ta.Fill(dt);

            foreach (TradefeedSimulationData.ContractCheckRow dr in dt.Rows)
            {
                result.Add(string.Format("Can not find reference Contract of {0} {1} {2}-{3}", dr.ExchangeCode,
                            dr.ProductCode, dr.YearContract, dr.MonthContract));
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(string.Format("Can not validate contract: {0}", ex.Message));
        }

        return result;
    }

    public static List<string> ValidateInvestor()
    {
        List<string> result = new List<string>();

        TradefeedSimulationDataTableAdapters.BuyerCheckTableAdapter taBuyer = new TradefeedSimulationDataTableAdapters.BuyerCheckTableAdapter();
        TradefeedSimulationData.BuyerCheckDataTable dtBuyer = new TradefeedSimulationData.BuyerCheckDataTable();

        TradefeedSimulationDataTableAdapters.SellerCheckTableAdapter taSeller = new TradefeedSimulationDataTableAdapters.SellerCheckTableAdapter();
        TradefeedSimulationData.SellerCheckDataTable dtSeller = new TradefeedSimulationData.SellerCheckDataTable();

        try
        {
            taBuyer.Fill(dtBuyer);
            foreach (TradefeedSimulationData.BuyerCheckRow dr in dtBuyer.Rows)
            {
                result.Add(string.Format("Can not find reference Buyer of {0}:{1} {2} {3}", dr.ExchangeCode,
                            dr.BuyerInvCode, dr.BuyerEMCode, dr.BuyerCMCode));
            }

            taSeller.Fill(dtSeller);
            foreach (TradefeedSimulationData.SellerCheckRow dr in dtSeller.Rows)
            {
                result.Add(string.Format("Can not find reference Seller of {0}:{1} {2} {3}", dr.ExchangeCode,
                            dr.SellerInvCode, dr.SellerEMCode, dr.SellerCMCode));
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(string.Format("Can not validate membership: {0}", ex.Message));
        }


        return result;
    }

    public static TradefeedSimulationData.SimulationValidateTradeDataTable GetValidateTradeSimulation(DateTime businessDate)
    {
        TradefeedSimulationData.SimulationValidateTradeDataTable dt = new TradefeedSimulationData.SimulationValidateTradeDataTable();
        TradefeedSimulationDataTableAdapters.SimulationValidateTradeTableAdapter ta = new TradefeedSimulationDataTableAdapters.SimulationValidateTradeTableAdapter();
        
        try
        {
            dt = ta.GetData(businessDate);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(string.Format("Can not validate trade: {0}", ex.Message));
        }
    }

    public static void Delete()
    {
        TradefeedSimulationDataTableAdapters.RawTradeFeedSimulationTableAdapter ta = new TradefeedSimulationDataTableAdapters.RawTradeFeedSimulationTableAdapter();

        try
        {
            ta.DeleteAll();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Delete Failed. " + ex.Message);
        }
    }
}
