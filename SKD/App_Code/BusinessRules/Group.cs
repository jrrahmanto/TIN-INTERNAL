using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Transactions;
using System.Web.Security;

/// <summary>
/// Summary description for Group
/// </summary>
public class Group
{
    public Group()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //private static List<string> listOfPage = new List<string>();

    //public static List<string> GetFiles(string path)
    //{
    //    List<string> listOfPage = new List<string>();
    //    if (Directory.Exists(path))
    //    {
    //        string[] subdirectoryEntries = Directory.GetDirectories(path);
    //        foreach (string subdirectory in subdirectoryEntries)
    //        {
    //            string[] fileEntries = Directory.GetFiles(subdirectory);

    //            foreach (string fileName in fileEntries)
    //            {
    //                if (fileName.Contains(".aspx") && !fileName.Contains(".cs"))
    //                {
    //                    listOfPage.Add(fileName.Replace(path + "\\", "").Replace("\\", " - "));
    //                }
    //            }

    //            string[] subsubDirectory = Directory.GetDirectories(subdirectory);

    //            foreach (string item in subsubDirectory)
    //            {
    //                string[] fileEntries2 = Directory.GetFiles(item);

    //                foreach (string item2 in fileEntries2)
    //                {
    //                    if (item2.Contains(".aspx") && !item2.Contains(".cs"))
    //                    {
    //                        listOfPage.Add(item2.Replace(path + "\\", "").Replace("\\"," - "));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return listOfPage;
    //}

    //const int HowDeepToScan = 4;

    //public static List<string> ProcessDir(string sourceDir, int recursionLvl)
    //{
    //    if (recursionLvl <= HowDeepToScan)
    //    {
    //        // Process the list of files found in the directory.
    //        string[] fileEntries = Directory.GetFiles(sourceDir);
    //        foreach (string fileName in fileEntries)
    //        {
    //            if (fileName.Contains(".aspx") && !fileName.Contains(".cs"))
    //            {
    //                listOfPage.Add(fileName.Replace(sourceDir + "\\", "").Replace("\\", " - "));
    //            }
    //        }

    //        // Recurse into subdirectories of this directory.
    //        string[] subdirEntries = Directory.GetDirectories(sourceDir);
    //        foreach (string subdir in subdirEntries)
    //            // Do not iterate through reparse points
    //            if ((File.GetAttributes(subdir) &
    //                 FileAttributes.ReparsePoint) !=
    //                     FileAttributes.ReparsePoint)

    //                ProcessDir(subdir, recursionLvl + 1);
    //    }

    //    return listOfPage;
    //}

    //public static GroupData.PageDataTable GetPageData(string path)
    //{
    //    GroupData.PageDataTable dt = new GroupData.PageDataTable();

    //    List<string> listOfPage = ProcessDir(path,0);
    //    foreach (string item in listOfPage)
    //    {
    //        dt.AddPageRow(item, "", "", "","");
    //    }
    //    return dt;
    //}



    public static GroupData.ApplicationPageDataTable getApplicationPage(decimal groupID)
    {
        GroupDataTableAdapters.ApplicationPageTableAdapter ta = new GroupDataTableAdapters.ApplicationPageTableAdapter();
        GroupData.ApplicationPageDataTable dt = new GroupData.ApplicationPageDataTable();

        ta.Fill(dt, groupID);
        return dt;

    }

    public static GroupData.ApplicationPageDataTable getInsertApplicationPage()
    {
        GroupDataTableAdapters.ApplicationPageTableAdapter ta = new GroupDataTableAdapters.ApplicationPageTableAdapter();
        GroupData.ApplicationPageDataTable dt = new GroupData.ApplicationPageDataTable();

        ta.FillInsertByGroupID(dt);
        return dt;
    }

    public static GroupData.GroupDataTable FillByGroupName(string groupName)
    {
        GroupDataTableAdapters.GroupTableAdapter ta = new GroupDataTableAdapters.GroupTableAdapter();
        return ta.GetDataByGroupName(groupName);
    }

    public static GroupData.GroupDataTable GetGroupData(string pageName)
    {
        GroupDataTableAdapters.GroupTableAdapter ta = new GroupDataTableAdapters.GroupTableAdapter();
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();

        try
        {
            ta.Fill(dt);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load data", ex);
        }

        return dt;
    }

    public static void InsertGroup(string groupName, string description,
                                     string userName, List<string> checkedList)
    {
        GroupDataTableAdapters.GroupTableAdapter groupTA = new GroupDataTableAdapters.GroupTableAdapter();
        GroupDataTableAdapters.RoleSOPageTableAdapter roleTa = new GroupDataTableAdapters.RoleSOPageTableAdapter();
        GroupDataTableAdapters.ApplicationPage1TableAdapter appTa = new GroupDataTableAdapters.ApplicationPage1TableAdapter();
        using (TransactionScope scope = new TransactionScope())
        {
            groupTA.Insert(groupName, description, userName, DateTime.Now, userName, DateTime.Now);
            decimal maxGroupID = Convert.ToDecimal(groupTA.getMaxGroupID());
            foreach (string item in checkedList)
            {
                string[] checkArr = item.Split(new char[] { '|' });
                //string aaaa = "aaa";
                //if (checkArr[0] == "DKP_ViewCommodity")
                //{
                //    aaaa = "asd";
                //}
                roleTa.Insert(maxGroupID, checkArr[1], appTa.getUrlbyPageName(checkArr[0]).ToString());
            }

            string logMessage = string.Format("{0}|{1}", groupName, description);
            AuditTrail.AddAuditTrail("Group", "Insert", logMessage, userName, "Insert");
            scope.Complete();
        }
    }

    public static void UpdateGroup(string groupName, string description, string originalID,
                                    string userName, List<string> checkedList)
    {
        //GroupDataTableAdapters.GroupTableAdapter groupTA = new GroupDataTableAdapters.GroupTableAdapter();
        //GroupDataTableAdapters.RoleSOPageTableAdapter roleTa = new GroupDataTableAdapters.RoleSOPageTableAdapter();
        //GroupDataTableAdapters.ApplicationPage1TableAdapter appTa = new GroupDataTableAdapters.ApplicationPage1TableAdapter();
        //GroupData.RoleSOPageDataTable roleDT = new GroupData.RoleSOPageDataTable();
        //GroupData.GroupDataTable groupDT = new GroupData.GroupDataTable();
        //RoleDataTableAdapters.RoleSOPageTableAdapter roleSOPageTa = new RoleDataTableAdapters.RoleSOPageTableAdapter();
        //RoleData.RoleSOPageDataTable RoleSoDT = new RoleData.RoleSOPageDataTable();
        //UserDataTableAdapters.UserGroupTableAdapter userGroupTa = new UserDataTableAdapters.UserGroupTableAdapter();
        //UserData.UserGroupDataTable userGroupDt = new UserData.UserGroupDataTable();
        //RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter userInRoleTA = new RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter();
        //RoleDataTableAdapters.aspnet_RolesTableAdapter rolesTa = new RoleDataTableAdapters.aspnet_RolesTableAdapter();
        //RoleData.aspnet_RolesDataTable rolesDT = new RoleData.aspnet_RolesDataTable();

        //using (TransactionScope scope = new TransactionScope())
        //{
        //    groupTA.UpdateByGroupID(groupName, description, userName, DateTime.Now, Convert.ToDecimal(originalID));
        //    roleTa.DeleteByGroupID(Convert.ToDecimal(originalID));

        //    foreach (string item in checkedList)
        //    {
        //        string[] checkArr = item.Split(new char[] { '|' });
        //        string url = appTa.getUrlbyPageName(checkArr[0]).ToString();
        //        roleTa.Insert(Convert.ToDecimal(originalID), checkArr[1], url);
        //    }

        //    userGroupDt = userGroupTa.GetDataByGroupID(Convert.ToDecimal(originalID));

        //    //delete user in role
        //    foreach (UserData.UserGroupRow dr in userGroupDt)
        //    {
        //        userInRoleTA.DeleteByUserID(dr.UserID);
        //    }

        //    roleSOPageTa.FillByGroupID(RoleSoDT, Convert.ToDecimal(originalID));

        //    //insert user in role
        //    foreach (UserData.UserGroupRow drUser in userGroupDt)
        //    {
        //        foreach (RoleData.RoleSOPageRow dr in RoleSoDT)
        //        {
        //            rolesDT.Clear();
        //            rolesTa.FillByRoleName(rolesDT, Tools.ComputeMD5(dr.Page, "") + "_" + dr.SecurityObject);
        //            if (roleDT.Count > 0)
        //            {
        //                userInRoleTA.Insert(drUser.UserID, rolesDT[0].RoleId);
        //            }
        //        }
        //    }

        //    string logMessage = string.Format("{0}|{1}", groupName, description);
        //    AuditTrail.AddAuditTrail("Group", "Update", logMessage, userName);
        //    scope.Complete();
        //}

        GroupDataTableAdapters.GroupTableAdapter taGroup = new GroupDataTableAdapters.GroupTableAdapter();
        GroupData.GroupDataTable dtGroup = new GroupData.GroupDataTable();
        GroupDataTableAdapters.RoleSOPageTableAdapter taRoleSOPage = new GroupDataTableAdapters.RoleSOPageTableAdapter();
        GroupData.RoleSOPageDataTable dtRoleSOPage = new GroupData.RoleSOPageDataTable();
        GroupDataTableAdapters.UserGroupTableAdapter taUserGroup = new GroupDataTableAdapters.UserGroupTableAdapter();
        GroupData.UserGroupDataTable dtUserGroup = new GroupData.UserGroupDataTable();
        GroupDataTableAdapters.ApplicationPage1TableAdapter taApp = new GroupDataTableAdapters.ApplicationPage1TableAdapter();

        List<string> deletedRoles = new List<string>();
        List<string> addedRoles = new List<string>();
        List<string> users = new List<string>();

        TransactionOptions options = new TransactionOptions();
        options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        options.Timeout = new TimeSpan(0, 15, 0);
        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        {
            taGroup.FillByGroupID(dtGroup, decimal.Parse(originalID));
            taRoleSOPage.FillByGroupID(dtRoleSOPage, decimal.Parse(originalID));
            taUserGroup.FillByGroupID(dtUserGroup, decimal.Parse(originalID));

            // Update Group
            dtGroup[0].GroupName = groupName;
            dtGroup[0].Description = description;

            // Remove deleted roles
            string hashPage;
            List<GroupData.RoleSOPageRow> deletedRows = new List<GroupData.RoleSOPageRow>();
            foreach (GroupData.RoleSOPageRow rowRoleSOPage in dtRoleSOPage)
            {
                string item = taApp.getPageNameByUrl(rowRoleSOPage.Page) + "|" + rowRoleSOPage.SecurityObject;
                if (!checkedList.Contains(item))
                {
                    hashPage = Tools.ComputeMD5(rowRoleSOPage.Page.Trim().ToLower(), "") + "_" + rowRoleSOPage.SecurityObject;
                    if (!deletedRoles.Contains(hashPage))
                    {
                        deletedRoles.Add(hashPage);
                    }
                    deletedRows.Add(rowRoleSOPage);
                }
            }

            foreach (GroupData.RoleSOPageRow rowRoleSOPage in deletedRows)
            {
                rowRoleSOPage.Delete();
            }

            // Add new roles to RoleSOPage
            string filter, page, securityObject, url;
            foreach (string item in checkedList)
            {
                securityObject = item.Split('|')[1];
                page = item.Split('|')[0];
                url = taApp.getUrlbyPageName(page).ToString();
                
                //string asdasd = "asdasdasd";
                //if (url.Contains("ViewManageBroadcast.aspx"))
                //{
                //    asdasd = "asdasd";
                //}
                //if (url.Contains("ViewTradeFeedException.aspx"))
                //{
                //    asdasd = "asdasd";
                //}
                //if (url.Contains("ViewRptClearingHouseAccountBalance.aspx"))
                //{
                //    asdasd = "asdasd";
                //}
                //if (url.Contains("ViewRptSummaryDailyShortage"))
                //{
                //    asdasd = "xxx";
                //}
                filter = string.Format("GroupID = {0} and SecurityObject = '{1}' and Page = '{2}'",
                            originalID, securityObject, url);
                if (dtRoleSOPage.Select(filter).Length == 0)
                {
                    hashPage = Tools.ComputeMD5(url.Trim().ToLower(), "") + "_" + securityObject;
                    addedRoles.Add(hashPage);
                    dtRoleSOPage.AddRoleSOPageRow(dtGroup[0], securityObject, url);
                }
            }


            // Update roles
            foreach (GroupData.UserGroupRow rowUserGroup in dtUserGroup)
            {
                users.Add(rowUserGroup.UserName);
            }

            foreach (string role in deletedRoles)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }

            foreach (string role in addedRoles)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }

            //if (users.Count > 0)
            //{
            //    Roles.RemoveUsersFromRoles(users.ToArray(), deletedRoles.ToArray());
            //    Roles.AddUsersToRoles(users.ToArray(), addedRoles.ToArray());
            //}
            foreach (string user in users)
            {
                foreach (string role in deletedRoles)
                {
                    if (Roles.IsUserInRole(user, role))
                    {
                        Roles.RemoveUserFromRole(user, role);
                    }
                }

                foreach (string role in addedRoles)
                {
                    if (!Roles.IsUserInRole(user, role))
                    {
                        Roles.AddUserToRole(user, role);
                    }
                }
            }

            taGroup.Update(dtGroup);
            taRoleSOPage.Update(dtRoleSOPage);
            taUserGroup.Update(dtUserGroup);

            string logMessage = string.Format("{0}|{1}", groupName, description);
            AuditTrail.AddAuditTrail("Group", "Update", logMessage, userName, "Update");
            scope.Complete();
        }
    }

    public static void DeleteGroup(string originalID, string userName)
    {

        GroupDataTableAdapters.GroupTableAdapter groupTA = new GroupDataTableAdapters.GroupTableAdapter();
        GroupDataTableAdapters.RoleSOPageTableAdapter roleTa = new GroupDataTableAdapters.RoleSOPageTableAdapter();
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();

        using (TransactionScope scope = new TransactionScope())
        {
            groupTA.FillByGroupID(dt, Convert.ToDecimal(originalID));

            if (dt.Count > 0)
            {
                string logMessage = string.Format("{0}|{1}", dt[0].GroupName, dt[0].Description);
                roleTa.DeleteByGroupID(Convert.ToDecimal(originalID));
                groupTA.DeleteByGroupID(Convert.ToDecimal(originalID));
                AuditTrail.AddAuditTrail("Group", "Delete", logMessage, userName, "Delete");
            }
            scope.Complete();
        }

    }
}
