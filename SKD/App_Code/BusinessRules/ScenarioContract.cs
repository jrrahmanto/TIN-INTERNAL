using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Transactions;

/// <summary>
/// Summary description for ScenarioContract
/// </summary>
public class ScenarioContract
{
    public static void AddScenarioContract(decimal contractID, decimal scenarioID, string scenarioType, decimal basePrice,
                                      decimal low, decimal high, string username)
    {
        ScenarioContractDataTableAdapters.ScenarioContractTableAdapter ta = new ScenarioContractDataTableAdapters.ScenarioContractTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(contractID, scenarioID, scenarioType, basePrice, low, high);

                logMessage = string.Format("Insert Value: contractID={0}|scenarioID={1}|scenarioType={2}|basePrice={3}|low={4}|high={5}",
                                            contractID,
                                            scenarioID,
                                            scenarioType,
                                            basePrice,
                                            low,
                                            high);
                AuditTrail.AddAuditTrail("ScenarioContract", "Insert", logMessage, username, "Insert");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static void UpdateScenarioContract(decimal contractID, decimal scenarioID, string scenarioType, decimal basePrice,
                                      decimal low, decimal high, string username)
    {
        ScenarioContractDataTableAdapters.ScenarioContractTableAdapter ta = new ScenarioContractDataTableAdapters.ScenarioContractTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Update(scenarioType, basePrice, low, high, contractID, scenarioID);

                logMessage = string.Format("Update Value: contractID={0}|scenarioID={1}|scenarioType={2}|basePrice={3}|low={4}|high={5}",
                                            contractID,
                                            scenarioID,
                                            scenarioType,
                                            basePrice,
                                            low,
                                            high);
                AuditTrail.AddAuditTrail("ScenarioContract", "Update", logMessage, username,"Update");

                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }
    }

    public static ScenarioContractData.ScenarioContractDataTable SelectScenarioContractData(decimal scenarioId)
    {
        ScenarioContractData.ScenarioContractDataTable dt = new ScenarioContractData.ScenarioContractDataTable();
        ScenarioContractDataTableAdapters.ScenarioContractTableAdapter ta = new ScenarioContractDataTableAdapters.ScenarioContractTableAdapter();

        try
        {
            ta.FillByScenarioID(dt, scenarioId);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetContractDataByContractID(decimal contractId)
    {
        ScenarioContractData.ContractDataTable dtContract = new ScenarioContractData.ContractDataTable();
        ScenarioContractData.ContractRow drContract = null;
        ScenarioContractDataTableAdapters.ContractTableAdapter taContract = new ScenarioContractDataTableAdapters.ContractTableAdapter();

        string contractCode = "";

        try
        {
            taContract.FillByContractID(dtContract, contractId);
            if (dtContract.Count > 0)
            {
                drContract = dtContract[0];
                contractCode = drContract.CommodityCode.ToString() + " " + drContract.ContractYear + drContract.ContractMonth;
            }

            return contractCode;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

}
