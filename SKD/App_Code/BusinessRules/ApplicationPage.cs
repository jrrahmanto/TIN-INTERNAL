using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Web.Security;

/// <summary>
/// Summary description for ApplicationPage
/// </summary>
public class ApplicationPage
{
    public static ApplicationPageData.ApplicationPageDataTable FillByPageName(string pageName)
    {
        ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();
        return ta.GetDataByPageName(pageName);
    }

    public static ApplicationPageData.ApplicationPageDataTable FillByExactPageName(string pageName)
    {
        ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();
        return ta.GetDataByExactPageName(pageName);
    }
    
    public static void Insert(string pageName, string description, string applicationName, string url, string userName)
    {
        ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();

        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ta.Insert(pageName, description, applicationName, url);

                string logMessage = string.Format("Insert, PageName:{0} | Description:{1} |" +
                                                  "ApplicationName:{2} | URL:{3}", pageName,
                                                  description, applicationName, url);

                string hash = Tools.ComputeMD5(url, "");
                if (!Roles.RoleExists(hash + "_Viewer"))
                {
                    Roles.CreateRole(hash + "_Viewer");
                }
                if (!Roles.RoleExists(hash + "_Checker"))
                {
                    Roles.CreateRole(Tools.ComputeMD5(url, "") + "_Checker");
                }
                if (!Roles.RoleExists(hash + "_Maker"))
                {
                    Roles.CreateRole(Tools.ComputeMD5(url, "") + "_Maker");
                }
                AuditTrail.AddAuditTrail("ApplicationPage", "Insert", logMessage, userName, "Insert");
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

    public static void Update(string pageName, string description, string applicationName,
                              string url, string userName, string originalPageName)
    {
        ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();

        using (TransactionScope scope = new TransactionScope())
        {
            ta.Update(pageName, description, applicationName, url, originalPageName);
            string logMessage = string.Format("Update, PageName:{0} | Description:{1} |" +
                                              "ApplicationName:{2} | URL:{3}", pageName,
                                              description, applicationName, url);
            AuditTrail.AddAuditTrail("ApplicationPage", "Update", logMessage, userName, "Update");
            scope.Complete();
        }
    }

    public static void Delete(string pageName, string description, string applicationName,
                              string url, string userName, string originalPageName)
    {
        ApplicationPageDataTableAdapters.ApplicationPageTableAdapter ta = new ApplicationPageDataTableAdapters.ApplicationPageTableAdapter();
        RoleDataTableAdapters.aspnet_RolesTableAdapter roleTa = new RoleDataTableAdapters.aspnet_RolesTableAdapter();
        using (TransactionScope scope = new TransactionScope())
        {
            ta.Delete(pageName);
            string logMessage = string.Format("Delete, PageName:{0} | Description:{1} |" +
                                              "ApplicationName:{2} | URL:{3}", pageName,
                                              description, applicationName, url);
            roleTa.DeleteByRoleName(Tools.ComputeMD5(url, "") + "_Viewer");
            roleTa.DeleteByRoleName(Tools.ComputeMD5(url, "") + "_Checker");
            roleTa.DeleteByRoleName(Tools.ComputeMD5(url, "") + "_Maker");
            AuditTrail.AddAuditTrail("ApplicationPage", "Delete", logMessage, userName,"Delete");
            scope.Complete();
        }

    }
}
