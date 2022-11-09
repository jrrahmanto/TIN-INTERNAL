using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for Product
/// </summary>
public class Product
{
    public static ProductData.ProductDataTable GetProductByProductCodeAndApprovalStatus(string productCode, string approvalStatus)
    {
        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();

        try
        {
            ta.FillByProductCodeAndApprovalStatus(dt, productCode, approvalStatus);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message,ex);
        }
    }

    public static ProductData.ProductRow SelectProductByProductID(decimal productID)
    {
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();
        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();
        ProductData.ProductRow dr = null;
        try
        {
            ta.FillByProductID(dt, productID);

            if (dt.Count > 0)
            {
                dr = dt[0];
            }

            return dr;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load product data");
        }
    }

    public static ProductData.ProductDataTable SelectProductByCode(string productCode)
    {
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();
        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();

        try
        {
            ta.FillByProductCode(dt, productCode);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load product data");
        }

    }

    public static void ProposeProduct(string productCode, string exchangeType, string productName, DateTime createDate,
                                      string lastUpdateBy, DateTime lastUpdateDate, string approvalDesc, string action, 
                                      string userName, decimal OriginalID,  string createdBy)
    {
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();

        try
        {
            string logMessage;
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(productCode, "P", exchangeType, productName, approvalDesc, createdBy,
                         createDate, lastUpdateBy, lastUpdateDate, action, OriginalID);
                string ActionFlagDesc = "";
                switch (action)
                {
                    case "I": ActionFlagDesc = "Insert"; break;
                    case "U": ActionFlagDesc = "Update"; break;
                    case "D": ActionFlagDesc = "Delete"; break;
                }
                logMessage = string.Format("Proposed Value: ProductCode={0}|ExchangeType={1}|ProductName={2}",
                                         productCode.ToString(), exchangeType.ToString(), productName.ToString());

                AuditTrail.AddAuditTrail("Product", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                scope.Complete();
            }
        }
        catch (Exception ex)
        {
            string exMessage;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                exMessage = "Record is already exist or in pending approval.";
            }
            else
            {
                exMessage = ex.Message;
            }

            throw new ApplicationException(exMessage);
        }

    }

    public static void ApproveParameter(decimal productID, string userName, string approvalDesc)
    {
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();
        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();

        try
        {
            try
            {
                ta.FillByProductID(dt, productID);

                using (TransactionScope scope = new TransactionScope())
                {
                    string logMessage = "";

                    //update record
                    if (dt[0].ActionFlag == "I")
                    {
                        ta.ApprovedProposedItem(dt[0].ProductCode, "A", dt[0].ExchangeType, dt[0].ProductName, approvalDesc, 
                                                          userName, DateTime.Now,
                                                            null, null, dt[0].ProductID);


                      logMessage = string.Format("Approved Insert: ProductCode={0}|ExchangeType={1}|ProductName={2}",
                                                  dt[0].ProductCode, dt[0].ExchangeType, dt[0].ProductName);

                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        ta.ApprovedUpdateProposedItem(dt[0].ProductCode, "A", dt[0].ExchangeType, dt[0].ProductName, approvalDesc,
                                                   userName, DateTime.Now, null,
                                                     dt[0].OriginalID);

                        //delete proposed record
                        ta.DeleteProposedItem(dt[0].ProductID);
                        logMessage = string.Format("Approved Update: ProductCode={0}|ExchangeType={1}|ProductName={2}",
                                                     dt[0].ProductCode, dt[0].ExchangeType, dt[0].ProductName);

                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        ta.DeleteProposedItem(dt[0].OriginalID);
                        ta.DeleteProposedItem(dt[0].ProductID);
                        logMessage = string.Format("Approved Delete: ProductCode={0}|ExchangeType={1}|ProductName={2}",
                                                    dt[0].ProductCode, dt[0].ExchangeType, dt[0].ProductName);
                    }
                    string ActionFlagDesc = "";
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                    AuditTrail.AddAuditTrail("Product", AuditTrail.APPROVE, logMessage, userName, ActionFlagDesc);

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                errorMessage = "Record is already exist or in pending approval";
            }
            throw new ApplicationException(errorMessage);
        }
    }


    public static void RejectProposedProduct(decimal productId, string userName)
    {
        ProductDataTableAdapters.ProductTableAdapter ta = new ProductDataTableAdapters.ProductTableAdapter();
        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                string logMessage = "";
                ta.FillByProductID(dt, productId);
                string ActionFlagDesc = "";
                if (dt.Count > 0)
                {
                    logMessage = string.Format("Approved Delete: ProductCode={0}|ExchangeType={1}|ProductName={2}",
                                                dt[0].ProductCode, dt[0].ExchangeType, dt[0].ProductName);
                    
                    switch (dt[0].ActionFlag)
                    {
                        case "I": ActionFlagDesc = "Insert"; break;
                        case "U": ActionFlagDesc = "Update"; break;
                        case "D": ActionFlagDesc = "Delete"; break;
                    }
                }

                ta.DeleteRejectedItem(productId);

                AuditTrail.AddAuditTrail("Product", AuditTrail.PROPOSE, logMessage, userName, ActionFlagDesc);
                scope.Complete();
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }


}
