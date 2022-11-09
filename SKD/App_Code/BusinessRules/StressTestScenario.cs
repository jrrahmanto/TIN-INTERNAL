using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Transactions;

/// <summary>
/// Summary description for StressTestScenario
/// </summary>
public class StressTestScenario
{
    #region "-- Stress Test Scenario --"

    public static StressTestScenarioData.StressTestScenarioDataTable SelectScenario(string scenarioName)
    {
        StressTestScenarioData.StressTestScenarioDataTable dt = new StressTestScenarioData.StressTestScenarioDataTable();
        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter ta = new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();

        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static StressTestScenarioData.StressTestScenarioDataTable SelectScenarioName(string scenarioName)
    {
        StressTestScenarioData.StressTestScenarioDataTable dt = new StressTestScenarioData.StressTestScenarioDataTable();
        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter ta = new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();

        try
        {
            ta.FillByStressTestScenario(dt, scenarioName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }


    public static void UpdateStressTestScenario(string scenarioName, DateTime scenarioDate, decimal scenarioID, 
        string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate,
        ScenarioContractData.ScenarioContractDataTable dtNewScenarioContract)
    {

        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter taStressTestScenario = new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();

        try
        {
            string logMessage = "";
            using (TransactionScope scope = new TransactionScope())
            {
               //Check StressTestScenario : If nothing then insert else update
                if (scenarioID == 0)
                {
                    //insert stress test scenario
                    taStressTestScenario.InsertStressTestScenario(scenarioName, scenarioDate, createdBy,
                        createdDate, lastUpdatedBy, lastUpdatedDate);

                    //insert audittrail
                    logMessage = string.Format("Proposed Value: scenarioName={0}|scenarioDate={1}",
                                      scenarioName, Convert.ToDateTime(scenarioDate));
                    AuditTrail.AddAuditTrail("StressTestScenario", "Insert", logMessage, lastUpdatedBy, "Insert");
                }
                else
                {
                    //update stress test scenario
                    taStressTestScenario.UpdateStressTestScenario(scenarioName, scenarioDate, lastUpdatedBy, lastUpdatedDate, 
                                                                scenarioID);

                    //insert audittrail
                    logMessage = string.Format("Proposed Value: scenarioName={0}|scenarioDate={1}",
                                      scenarioName, Convert.ToDateTime(scenarioDate));
                    AuditTrail.AddAuditTrail("StressTestScenario", "Update", logMessage, lastUpdatedBy,"Update");
                }

                //insert scenario contract
                ScenarioContractData.ScenarioContractRow drNewScenarioContract = null;
                ScenarioContractDataTableAdapters.ScenarioContractTableAdapter taScenarioContract =
                    new ScenarioContractDataTableAdapters.ScenarioContractTableAdapter();
                ScenarioContractData.ScenarioContractDataTable dtScenarioContract =
                    new ScenarioContractData.ScenarioContractDataTable();

                taScenarioContract.FillByScenarioID(dtScenarioContract, scenarioID);

                scenarioID = (decimal)taStressTestScenario.GetMaxScenarioId();

                //Process Scenario Contract : add/update/delete data
                foreach (ScenarioContractData.ScenarioContractRow drScenarioContract in dtNewScenarioContract)
                {
                    drNewScenarioContract = dtScenarioContract.FindByContractIDScenarioID(drScenarioContract.ContractID, scenarioID);

                    if (drNewScenarioContract == null)
                    {
                        drNewScenarioContract = dtScenarioContract.NewScenarioContractRow();
                    }

                    drNewScenarioContract.ContractID = drScenarioContract.ContractID;
                    drNewScenarioContract.ScenarioID = scenarioID;
                    drNewScenarioContract.ScenarioType = drScenarioContract.ScenarioType;
                    drNewScenarioContract.BasePrice = drScenarioContract.BasePrice;
                    drNewScenarioContract.Low = drScenarioContract.Low;
                    drNewScenarioContract.High = drScenarioContract.High;

                    if (drNewScenarioContract.RowState == DataRowState.Detached)
                    {
                        dtScenarioContract.AddScenarioContractRow(drNewScenarioContract);
                    }
                }

                foreach (ScenarioContractData.ScenarioContractRow drScenarioContract in dtScenarioContract)
                {
                    if (drScenarioContract.RowState == DataRowState.Unchanged)
                    {
                        drScenarioContract.Delete();
                    }
                }
                
                taScenarioContract.Update(dtScenarioContract);
                scope.Complete();
          
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                throw new ApplicationException("Contract with this scenario already exist.");
            }
            else
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }

    }

    public static StressTestScenarioData.StressTestScenarioRow SelectDataByScenarioId(decimal scenarioID)
    {
        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter ta = new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();
        StressTestScenarioData.StressTestScenarioDataTable dt = new StressTestScenarioData.StressTestScenarioDataTable();
        StressTestScenarioData.StressTestScenarioRow dr = null;
        try
        {
            ta.Fill(dt);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load scenario test data");
        }
    }

    public static StressTestScenarioData.StressTestScenarioRow SelectAllByScenarioId(decimal scenarioID)
    {
        StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter ta = new StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter();
        StressTestScenarioData.StressTestScenarioDataTable dt = new StressTestScenarioData.StressTestScenarioDataTable();
        StressTestScenarioData.StressTestScenarioRow dr = null;
        try
        {
            ta.FillByScenarioID(dt, scenarioID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load scenario test data");
        }
    }

    #endregion

    #region " --- Trade feed --- "
    //public static ScenarioContractData.TradeFeedDataTable getLastPriceTradeFeed(decimal contractId)
    //{

    //    ScenarioContractDataTableAdapters.TradeFeedTableAdapter taTradeFeed = new ScenarioContractDataTableAdapters.TradeFeedTableAdapter();
    //    ScenarioContractData.TradeFeedDataTable dtTradeFeed = new ScenarioContractData.TradeFeedDataTable();

    //    try
    //    {
    //        taTradeFeed.GetLastPrice(dtTradeFeed, contractId);
    //        return dtTradeFeed;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ApplicationException("Failed to load trade feed data");
    //    }
    //}

    #endregion
}
