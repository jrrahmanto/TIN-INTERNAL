using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Transactions;

/// <summary>
/// Summary description for SKDUser
/// </summary>
public class SKDUser
{
    public SKDUser()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static GroupData.GroupDataTable fillGroupByUserid(Guid userID)
    {
        GroupDataTableAdapters.GroupTableAdapter ta = new GroupDataTableAdapters.GroupTableAdapter();
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();

        try
        {
            ta.FillBySelectedUserID(dt, userID);
            return dt;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public static GroupData.GroupDataTable fillGroup()
    {
        GroupDataTableAdapters.GroupTableAdapter ta = new GroupDataTableAdapters.GroupTableAdapter();
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();

        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static UserData.aspnet_UsersDataTable GetUserByUserNameLike(string userName)
    {
        UserDataTableAdapters.aspnet_UsersTableAdapter ta = new UserDataTableAdapters.aspnet_UsersTableAdapter();
        UserData.aspnet_UsersDataTable dt = new UserData.aspnet_UsersDataTable();

        try
        {
            ta.FillByUsernameLike(dt, userName);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static UserData.aspnet_UsersDataTable GetUserByUserName(string userName)
    {
        UserDataTableAdapters.aspnet_UsersTableAdapter ta = new UserDataTableAdapters.aspnet_UsersTableAdapter();
        UserData.aspnet_UsersDataTable dt = new UserData.aspnet_UsersDataTable();

        try
        {
            ta.FillByUserName(dt, userName);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static UserData.UserProfileDataTable GetUserByUserID(Guid userID)
    {
        UserDataTableAdapters.UserProfileTableAdapter ta = new UserDataTableAdapters.UserProfileTableAdapter();
        UserData.UserProfileDataTable dt = new UserData.UserProfileDataTable();

        try
        {
            ta.FillByUserID(dt, userID);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static UserData.UserAttributesDataTable FillAttribute(Guid userID)
    {
        try
        {
            UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
            return ta.GetDataByUserID(userID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static void SaveUser(string currentID, string userName,
                                string nameDesc, string isLocked, string isDisabled,
                                string password, string isInternal,
                                string startTime, string endTime,
                                List<string> groupList, Nullable<decimal> CMID,
                                string currentUserName)
    {
        if (currentID != null)
        {

        }
        else
        {
            //add user 
            //add group
            //add roles
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 15, 0);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,options))
            {
                try
                {
                    UserDataTableAdapters.UserGroupTableAdapter userGroupTa = new UserDataTableAdapters.UserGroupTableAdapter();
                    RoleDataTableAdapters.RoleSOPageTableAdapter roleSOPageTa = new RoleDataTableAdapters.RoleSOPageTableAdapter();
                    RoleData.RoleSOPageDataTable RoleSoDT = new RoleData.RoleSOPageDataTable();
                    RoleDataTableAdapters.aspnet_RolesTableAdapter roleTa = new RoleDataTableAdapters.aspnet_RolesTableAdapter();
                    RoleData.aspnet_RolesDataTable roleDT = new RoleData.aspnet_RolesDataTable();
                    RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter userInRoleTA = new RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter();
                    UserDataTableAdapters.UserAttributesTableAdapter attributeTa = new UserDataTableAdapters.UserAttributesTableAdapter();
                    UserDataTableAdapters.aspnet_MembershipTableAdapter aspMemTA = new UserDataTableAdapters.aspnet_MembershipTableAdapter();

                    // Create user using membership class
                    Membership.CreateUser(userName, password);

                    // Get membership 
                    MembershipUser CurrentUser = Membership.GetUser(userName);
                    CurrentUser.Comment = nameDesc;

                    // Unlock user
                    if (isLocked == "N")
                    {
                        CurrentUser.UnlockUser();
                    }

                    // Disable / enable user
                    if (isDisabled == "Y")
                    {
                        CurrentUser.IsApproved = false;
                    }
                    else if (isDisabled == "N")
                    {
                        CurrentUser.IsApproved = true;
                    }

                    // Update user
                    Membership.UpdateUser(CurrentUser);


                    GroupData.GroupDataTable dt = new GroupData.GroupDataTable();
                    Guid userGuid = new Guid(CurrentUser.ProviderUserKey.ToString());

                    string pass = aspMemTA.getPasswordByUserID(userGuid);

                    attributeTa.Insert(userGuid, isInternal, startTime, endTime, pass, pass, pass, pass, pass, "", "", "", "", "", CMID);

                    ////Roles 
                    //foreach (string item in groupList)
                    //{
                    //    dt.Clear();
                    //    dt = Group.FillByGroupName(item);
                    //    if (dt.Count > 0)
                    //    {
                    //        userGroupTa.Insert(userName, dt[0].GroupID);
                    //        RoleSoDT.Clear();
                    //        roleSOPageTa.FillByGroupID(RoleSoDT, dt[0].GroupID);
                    //    }

                    //    foreach (RoleData.RoleSOPageRow dr in RoleSoDT)
                    //    {
                    //        roleDT.Clear();
                    //        roleTa.FillByRoleName(roleDT, Tools.ComputeMD5(dr.Page, "") + "_" + dr.SecurityObject.ToUpper());
                    //        if (roleDT.Count > 1)
                    //        {
                    //            userInRoleTA.Insert(userGuid, roleDT[0].RoleId);
                    //        }
                    //    }
                    //}

                    // Add groups to user
                    GroupData.GroupDataTable dtGroup = new GroupData.GroupDataTable();
                    GroupData.GroupRow rowGroup;
                    dtGroup = SKDUser.fillGroup();

                    UserData.UserGroup2DataTable dtUserGroup = new UserData.UserGroup2DataTable();
                    UserDataTableAdapters.UserGroup2TableAdapter taUserGroup = new UserDataTableAdapters.UserGroup2TableAdapter();
                    taUserGroup.Fill(dtUserGroup, userName);

                    // Delete removed groups
                    List<UserData.UserGroup2Row> deletedGroups = new List<UserData.UserGroup2Row>();
                    foreach (UserData.UserGroup2Row row in dtUserGroup)
                    {
                        rowGroup = dtGroup.FindByGroupID(row.GroupID);
                        if (groupList.Contains(rowGroup.GroupName))
                        {
                            // Remove group that already added from the group list
                            groupList.Remove(rowGroup.GroupName);
                        }
                        else
                        {
                            // Add the row to delete list
                            deletedGroups.Add(row);
                        }

                    }

                    foreach (UserData.UserGroup2Row row in deletedGroups)
                    {
                        row.Delete();
                    }

                    // Add new group
                    GroupData.GroupRow[] groupRows;
                    string groupDesc = null;

                    foreach (string newGroup in groupList)
                    {
                        groupRows = (GroupData.GroupRow[])dtGroup.Select("GroupName = '" + newGroup + "'");
                        if (groupRows.Length > 0)
                        {
                            dtUserGroup.AddUserGroup2Row(groupRows[0].GroupID, userName);
                        }
                        // Create group description for audit trail
                        if (groupDesc != null)
                            groupDesc = groupDesc + ",";
                        groupDesc = groupDesc + newGroup;

                    }

                    taUserGroup.Update(dtUserGroup);


                    // Add group pages to roles
                    UserData.UserPagesDataTable dtUserPages = new UserData.UserPagesDataTable();
                    UserDataTableAdapters.UserPagesTableAdapter taUserPages = new UserDataTableAdapters.UserPagesTableAdapter();
                    taUserPages.Fill(dtUserPages, userName);

                    // Remove all roles
                    string[] allUserRoles = Roles.GetRolesForUser(userName);
                    if (allUserRoles.Length > 0)
                    {
                        Roles.RemoveUserFromRoles(userName, allUserRoles);
                    }

                    foreach (UserData.UserPagesRow rowUserPages in dtUserPages)
                    {
                        string rolename = Tools.ComputeMD5(rowUserPages.Page.Trim().ToLower(), "") + "_" + rowUserPages.SecurityObject.ToUpper();
                        if (!Roles.RoleExists(rolename))
                        {
                            Roles.CreateRole(rolename);
                        }

                        Roles.AddUserToRole(userName, rolename);
                    }

                    EventReceipientList.AddEventReceipientList(userName, "", "", userGuid, currentUserName);


                    //EventRecipient.AddSingleRecipient(EventRecipient.GetEventTypeID("Broadcast"), EventReceipientList.GetRecipientListID(userName), currentUserName);

                    string cmCode = null;
                    if (CMID != null)
                    {
                        cmCode = ClearingMember.GetClearingMemberCodeByClearingMemberID(CMID.Value);
                    }
                    string logMessage = string.Format("UserName :{0} | Name :{1} | " +
                                                      "Locked :{2}| Disabled: {3} | Internal :{4} | " +
                                                      " Start Time : {5} | End Time :{6} | " +
                                                      " Clearing Member :{7} | Groups : {8} ", userName, nameDesc,
                                                      isLocked, isDisabled, isInternal, startTime, endTime, cmCode, groupDesc);
                    AuditTrail.AddAuditTrail("UserManagement", "Insert", logMessage, currentUserName, "Insert");
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    string exMessage;
                    if (ex.Message.Contains("The password supplied is invalid"))
                    {
                        exMessage = "The password supplied is invalid.  Passwords must confirm to the password strength requirements configured for the default provider.";
                    }
                    else
                    {
                        exMessage = ex.Message;
                    }
                    throw new ApplicationException(exMessage);
                }

            }

        }
    }

    public static void Update(string currentID, string userName,
                               string nameDesc, string isLocked, string isDisabled,
                               string password, string isInternal,
                               string startTime, string endTime,
                               List<string> groupList, Nullable<decimal> CMID,
                                string currentUserName)
    {

        ////add user 
        ////add group
        ////add roles
        //using (TransactionScope scope = new TransactionScope())
        //{
        //    try
        //    {

        //        UserDataTableAdapters.UserGroupTableAdapter userGroupTa = new UserDataTableAdapters.UserGroupTableAdapter();
        //        RoleDataTableAdapters.RoleSOPageTableAdapter roleSOPageTa = new RoleDataTableAdapters.RoleSOPageTableAdapter();
        //        RoleData.RoleSOPageDataTable RoleSoDT = new RoleData.RoleSOPageDataTable();
        //        RoleDataTableAdapters.aspnet_RolesTableAdapter roleTa = new RoleDataTableAdapters.aspnet_RolesTableAdapter();
        //        RoleData.aspnet_RolesDataTable roleDT = new RoleData.aspnet_RolesDataTable();
        //        RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter userInRoleTA = new RoleDataTableAdapters.aspnet_UsersInRolesTableAdapter();
        //        UserDataTableAdapters.UserAttributesTableAdapter attributeTa = new UserDataTableAdapters.UserAttributesTableAdapter();
        //        UserData.UserAttributesDataTable attributeDT = new UserData.UserAttributesDataTable();
        //        UserDataTableAdapters.aspnet_MembershipTableAdapter aspMemTA = new UserDataTableAdapters.aspnet_MembershipTableAdapter();

        //        MembershipUser CurrentUser = Membership.GetUser(userName);
        //        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();
        //        Guid userGuid = new Guid(CurrentUser.ProviderUserKey.ToString());
        //        string pass = aspMemTA.getPasswordByUserID(userGuid);
        //        string newPass = "";
        //        if (isLocked == "N")
        //        {
        //            CurrentUser.UnlockUser();
        //            Membership.UpdateUser(CurrentUser);
        //        }

        //        if (password != "")
        //        {
        //            attributeTa.FillByUserID(attributeDT, userGuid);
        //            CurrentUser.ChangePassword(CurrentUser.ResetPassword(), password);
        //            Membership.UpdateUser(CurrentUser);
        //            newPass = aspMemTA.getPasswordByUserID(userGuid);
        //            bool isAllreadyUsed = false;
        //            if (attributeDT.Count > 0)
        //            {
        //                if (attributeDT[0].History01 == newPass ||
        //                    attributeDT[0].History02 == newPass ||
        //                    attributeDT[0].History03 == newPass ||
        //                    attributeDT[0].History04 == newPass ||
        //                    attributeDT[0].History05 == newPass)
        //                {
        //                    isAllreadyUsed = true;
        //                }
        //            }

        //            if (!isAllreadyUsed)
        //            {
        //                attributeTa.Update(attributeDT[0].InternalFlag,
        //                                   attributeDT[0].StartTime,
        //                                   attributeDT[0].EndTime,
        //                                   newPass,
        //                                   attributeDT[0].History01,
        //                                   attributeDT[0].History02,
        //                                   attributeDT[0].History03,
        //                                   attributeDT[0].History04,
        //                                   null, null, null, null, null, 
        //                                   CMID,userGuid);                
        //            }
        //            else
        //            {
        //                throw new ApplicationException("Password has been used.");
        //            }
        //        }

        //        userInRoleTA.DeleteByUserID(userGuid);
        //        //Add User to Roles 
        //        foreach (string item in groupList)
        //        {
        //            dt.Clear();
        //            dt = Group.FillByGroupName(item);
        //            if (dt.Count > 0)
        //            {
        //                userGroupTa.Insert(userGuid, dt[0].GroupID);
        //                RoleSoDT.Clear();
        //                roleSOPageTa.FillByGroupID(RoleSoDT, dt[0].GroupID);
        //                foreach (RoleData.RoleSOPageRow dr in RoleSoDT)
        //                {
        //                    roleDT.Clear();
        //                    roleTa.FillByRoleName(roleDT, Tools.ComputeMD5(dr.Page, "") + "_"+ dr.SecurityObject);
        //                    if (roleDT.Count > 0)
        //                    {
        //                        userInRoleTA.Insert(userGuid, roleDT[0].RoleId);
        //                    }
        //                }
        //            }
        //        }

        //        scope.Complete();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        string groupDesc = null;

        try
        {

            using (TransactionScope scope = new TransactionScope())
            {
                MembershipUser user = Membership.GetUser(userName);
                Guid userId = (Guid)user.ProviderUserKey;

                user.Comment = nameDesc;

                /******************* User data ********************/
                // Unlock user
                if (isLocked == "N")
                {
                    user.UnlockUser();
                }

                // Disable / enable user
                if (isDisabled == "Y")
                {
                    user.IsApproved = false;
                }
                else if (isDisabled == "N")
                {
                    user.IsApproved = true;
                }

                // Reset password
                if (password != "")
                {
                    user.ChangePassword(user.ResetPassword(), password);
                }

                // Update user
                Membership.UpdateUser(user);

                /******************* Group data ********************/
                // Add groups to user
                GroupData.GroupDataTable dtGroup = new GroupData.GroupDataTable();
                GroupData.GroupRow rowGroup;
                dtGroup = SKDUser.fillGroup();

                UserData.UserGroup2DataTable dtUserGroup = new UserData.UserGroup2DataTable();
                UserDataTableAdapters.UserGroup2TableAdapter taUserGroup = new UserDataTableAdapters.UserGroup2TableAdapter();
                taUserGroup.Fill(dtUserGroup, userName);

                // Delete removed groups
                List<UserData.UserGroup2Row> deletedGroups = new List<UserData.UserGroup2Row>();
                foreach (UserData.UserGroup2Row row in dtUserGroup)
                {
                    rowGroup = dtGroup.FindByGroupID(row.GroupID);
                    if (groupList.Contains(rowGroup.GroupName))
                    {
                        // Remove group that already added from the group list
                        groupList.Remove(rowGroup.GroupName);
                    }
                    else
                    {
                        // Add the row to delete list
                        deletedGroups.Add(row);
                    }

                }

                foreach (UserData.UserGroup2Row row in deletedGroups)
                {
                    row.Delete();
                }

                // Add new group
                GroupData.GroupRow[] groupRows;
                foreach (string newGroup in groupList)
                {
                    groupRows = (GroupData.GroupRow[])dtGroup.Select("GroupName = '" + newGroup + "'");
                    if (groupRows.Length > 0)
                    {
                        dtUserGroup.AddUserGroup2Row(groupRows[0].GroupID, userName);
                    }

                    // Create group description for audit trail
                    if (groupDesc != null)
                        groupDesc = groupDesc + ",";
                    groupDesc = groupDesc + newGroup;
                }

                taUserGroup.Update(dtUserGroup);


                // Add group pages to roles
                UserData.UserPagesDataTable dtUserPages = new UserData.UserPagesDataTable();
                UserDataTableAdapters.UserPagesTableAdapter taUserPages = new UserDataTableAdapters.UserPagesTableAdapter();
                taUserPages.Fill(dtUserPages, userName);

                // Remove all roles
                string[] allUserRoles = Roles.GetRolesForUser(userName);
                if (allUserRoles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(userName, allUserRoles);
                }

                foreach (UserData.UserPagesRow rowUserPages in dtUserPages)
                {
                    string rolename = Tools.ComputeMD5(rowUserPages.Page.Trim().ToLower(), "") + "_" + rowUserPages.SecurityObject.ToUpper();
                    if (!Roles.RoleExists(rolename))
                    {
                        Roles.CreateRole(rolename);
                    }

                    Roles.AddUserToRole(userName, rolename);
                }

                /******************* User attributes data ********************/

                //update user attributes
                UserDataTableAdapters.UserAttributesTableAdapter attributeTa = new UserDataTableAdapters.UserAttributesTableAdapter();
                UserDataTableAdapters.aspnet_MembershipTableAdapter aspMemTA = new UserDataTableAdapters.aspnet_MembershipTableAdapter();
                UserData.UserAttributesDataTable attributeDT = new UserData.UserAttributesDataTable();

                attributeTa.UpdateUserAttributeWOPwd(isInternal, startTime, endTime, CMID, userId);

                if (password != "")
                {
                    attributeTa.FillByUserID(attributeDT, userId);
                    string newPass;
                    newPass = aspMemTA.getPasswordByUserID(userId);
                    bool isAllreadyUsed = false;

                    if (attributeDT.Count > 0)
                    {
                        if (attributeDT[0].History01 == newPass ||
                            attributeDT[0].History02 == newPass ||
                            attributeDT[0].History03 == newPass ||
                            attributeDT[0].History04 == newPass ||
                            attributeDT[0].History05 == newPass)
                        {
                            isAllreadyUsed = true;
                        }
                    }

                    if (!isAllreadyUsed)
                    {
                        attributeTa.Update(isInternal,
                                           startTime,
                                           endTime,
                                           newPass,
                                           attributeDT[0].History01,
                                           attributeDT[0].History02,
                                           attributeDT[0].History03,
                                           attributeDT[0].History04,
                                           null, null, null, null, null,
                                           CMID, userId);
                    }
                    else
                    {
                        throw new ApplicationException("Password has been used.");
                    }
                }

                /******************* Audit Trail ********************/
                string CMCode = "";
                if (CMID != null)
                {
                    CMCode = ClearingMember.GetClearingMemberCodeByClearingMemberID(CMID.Value);
                }
                string logMessage = string.Format("UserName :{0} | Name :{1} | " +
                                                         "Locked :{2}| Disabled: {3} | Internal :{4} | " +
                                                         " Start Time : {5} | End Time :{6} | " +
                                                         " Clearing Member :{7} | Groups : {8}", userName, nameDesc,
                                                         isLocked, isDisabled, isInternal, startTime, endTime, CMCode, groupDesc);
                AuditTrail.AddAuditTrail("UserManagement", "Update", logMessage, currentUserName, "Update");

                // Commit transaction
                scope.Complete();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    public static string[] GetUserGroups(Guid userId, string userName)
    {
        // Add groups to user
        GroupData.GroupDataTable dtGroup = new GroupData.GroupDataTable();
        dtGroup = SKDUser.fillGroup();

        UserData.UserGroup2DataTable dtUserGroup = new UserData.UserGroup2DataTable();
        UserDataTableAdapters.UserGroup2TableAdapter taUserGroup = new UserDataTableAdapters.UserGroup2TableAdapter();
        taUserGroup.Fill(dtUserGroup, userName);

        if (dtUserGroup.Count > 0)
        {
            string[] ret = new string[dtUserGroup.Count];

            for (int ii = 0; ii < dtUserGroup.Count; ii++)
            {
                ret[ii] = dtGroup.FindByGroupID(dtUserGroup[ii].GroupID).GroupName;
            }
            return ret;
        }
        else
        {
            return new string[0];
        }

    }

    public static UserData.UserGroupDataTable FillUserGroupByUserName(string userName)
    {
        UserDataTableAdapters.UserGroupTableAdapter ta = new UserDataTableAdapters.UserGroupTableAdapter();

        return ta.GetDataByUserName(userName);
    }

    public static string GetInternalFlag(string userName)
    {
        UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
        return ta.GetInternalFlag(userName).ToString();
    }

    public static UserData.UserAttributesDataTable FillAttributeByUserName(string userName)
    {
        UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
        return ta.GetDataByUserName(userName);
    }

    public static bool isUserExist(string userName)
    {
        UserDataTableAdapters.UserAttributesTableAdapter ta = new UserDataTableAdapters.UserAttributesTableAdapter();
        int countUser = int.Parse(ta.CountUser(userName).ToString());
        bool isExist = false;
        if (countUser > 0)
        {
            isExist = true;
        }
        return isExist;
    }
}
