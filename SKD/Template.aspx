<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template.aspx.cs" Inherits="Template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="background-color: #97C4E9;">
                <img src="Images/left.png" height="100" alt="Indonesian Derivatives Clearing House" />
            </td>
            <td style="text-align: right; background-color: #97C4E9;">
                <img src="Images/right.png" width="345" alt="Indonesian Derivatives Clearing House" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="background-image: url('Images/bg_menu.gif'); background-repeat: repeat-x;">
                <asp:LoginView ID="LoginView1" runat="server" EnableViewState="false">
                    <AnonymousTemplate>
                        <span style="color: White">You are not logged in. <a style="color: White;" href="~/Login.aspx">
                            Please click here to login</a> </span>
                    </AnonymousTemplate>
                    <LoggedInTemplate>
                        <asp:Menu BorderWidth="0" ID="uiMainMenu" runat="server" DataSourceID="XmlDataSourceMenu"
                            StaticSubMenuIndent="20px" DynamicHorizontalOffset="1" Orientation="Horizontal"
                            CssClass="menu-main" ItemWrap="true" StaticEnableDefaultPopOutImage="false" DynamicEnableDefaultPopOutImage="true">
                            <DataBindings>
                                <asp:MenuItemBinding DataMember="siteMapNode" NavigateUrlField="url" TextField="title" />
                            </DataBindings>
                            <StaticMenuItemStyle CssClass="menu-static-menu-item" />
                            <StaticSelectedStyle CssClass="menu-static-selected" />
                            <StaticItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="leftmenu">
                                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="14" Height="100%" />
                                        </td>
                                        <td class="middlemenu">
                                            <%# Eval("Text") %>
                                        </td>
                                        <td class="rightmenu">
                                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="14" />
                                        </td>
                                    </tr>
                                </table>
                            </StaticItemTemplate>
                            <DynamicMenuStyle CssClass="menu-dynamic-menu" />
                            <DynamicMenuItemStyle CssClass="menu-dynamic-menu-item" />
                            <DynamicHoverStyle CssClass="menu-dynamic-hover" />
                        </asp:Menu>
                        <asp:XmlDataSource ID="XmlDataSourceMenu" runat="server" DataFile="~/App_Data/Menu.xml"
                            XPath="/Home/siteMapNode"></asp:XmlDataSource>
                    </LoggedInTemplate>
                </asp:LoginView>
            </td>
        </tr>
        <tr>
            <td style="background-color: #97C4E9; border-bottom: solid 1px #233A51">
                <asp:SiteMapPath ID="SiteMapPath1" runat="server" Font-Names="Verdana" Font-Size="11px"
                    PathSeparator=" : ">
                    <PathSeparatorStyle Font-Bold="True" ForeColor="#FFFFFF" />
                    <CurrentNodeStyle ForeColor="#FFFFFF" />
                    <NodeStyle Font-Bold="True" ForeColor="#92ACE2" />
                    <RootNodeStyle Font-Bold="True" ForeColor="#162950" />
                </asp:SiteMapPath>
            </td>
            <td style="background-color: #97C4E9; text-align: right; padding-right: 15px; color: #063E78;
                border-bottom: solid 1px #233A51">
                <asp:LoginView ID="LoginView2" runat="server">
                    <LoggedInTemplate>
                        Welcome <%= Page.User.Identity.Name %>
                        <asp:LinkButton ID="uiBtnLogout" runat="server" ForeColor="#063E78" Font-Bold="true">Logout</asp:LinkButton>
                    </LoggedInTemplate>
                </asp:LoginView>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="content" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" height="10" width="1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="10" height="7" />
                        </td>
                        <td>
                            <img src="Images/top_left.gif" width="7" />
                        </td>
                        <td class="top">
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" />
                        </td>
                        <td>
                            <img src="Images/top_right.gif" width="7" />
                        </td>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="1" height="7" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="1" />
                        </td>
                        <td class="left">
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="7" />
                        </td>
                        <td style="background-color: #E4F0FB;">
                            <div>
                                <h1>Content</h1>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </div>
                        </td>
                        <td class="right">
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="7" />
                        </td>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="10" height="7" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="10" />
                        </td>
                        <td>
                            <img src="Images/bottom_left.gif" width="7" />
                        </td>
                        <td class="bottom">
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" />
                        </td>
                        <td>
                            <img src="Images/bottom_right.gif" width="7" />
                        </td>
                        <td>
                            <asp:Image runat="server" ImageUrl="~/Images/spacer.gif" width="10" height="15" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="footer">
                <br />
                Copyright © 2010 PT Kliring Berjangka Indonesia (Persero). All Rights Reserved.
                <br />
                &nbsp;
            </td>
        </tr>
    </table>


    </form>
</body>
</html>
