<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <title>Error Page</title>
    <style type="text/css">
        .info
        {
            background-color: #FFFF99;
            border: solid 1px #C0C0C0;
        }
        .style1
        {
            height: 48px;
            width: 57px;
        }
        .style2
        {
            width: 57px;
        }
        .style3
        {
            background-color: #FFFF99;
            border: solid 1px #C0C0C0;
            width: 57px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td style="background-color: #97C4E9;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/left.png" Height="100" alt="Indonesian Derivatives Clearing House" />
            </td>
            <td style="text-align: right; background-color: #97C4E9;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/right.png" Width="345" alt="Indonesian Derivatives Clearing House" />
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td style="background-color: #3D7EBC">
                &nbsp;
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table runat="server" id="tblMain" style="border: 1px solid #C0C0C0; width: 100%;"
                    cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="header_title" style="border-width: 1px; border-color: #C0C0C0; border-bottom-style: solid"
                            colspan="2">
                        </td>
                    </tr>
                    <tr><td>
                    <table>
                    <tr>
                        <td class="style1">
                            <asp:Image ID="uiImgError" ImageUrl="~/Images/error.png" AlternateText="Error" runat="server"
                                Height="37px" Width="37px" />
                        </td>
                        <td style="vertical-align: middle; display: inline;">
                            <h2>
                                <asp:Label ID="uiLblMessage" runat="server" ForeColor="#BB0000" Text="Error"></asp:Label>
                            </h2>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                        </td>
                        <td colspan="2" valign="top">
                            <h3>
                                <asp:Label ID="uiLblErrorMessage" runat="server"></asp:Label>
                            </h3>
                        </td>
                    </tr>
                    </table>
                    </td></tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="uiLblDescription" runat="server">
                            </asp:Label>
                            <asp:Panel ID="PanelHeader" runat="server" Width="100%">
                            <table style="width: 100%;">
                                    <tr>
                                        <td colspan=3>
                                            Click here to view details:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Image ID="ImageDetail" runat="server" ImageUrl="~/Images/expand.png" />
                                            
                                        </td>
                                    </tr>
                            </table>
                            </asp:Panel>
                            <asp:Panel ID="PanelDetail" runat="server" Width="100%">
                            <table style="width: 100%;">
                                    <tr>
                                        <td class="style3">
                                            <h4>
                                                Url:</h4>
                                        </td>
                                        <td class="info">
                                                <asp:Label ID="uiLblOriginalUrl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3">
                                            <h4>
                                                Stack
                                                <br />
                                                Trace:</h4>
                                        </td>
                                        <td class="info">
                                                <asp:Label ID="uiLblStackTrace" runat="server" Font-Size="Smaller"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="Panel1_CollapsiblePanelExtender" TargetControlID="PanelDetail"
                                ExpandControlID="PanelHeader" CollapseControlID="PanelHeader" runat="server"
                                Enabled="True" ExpandedText="(Collapse...)" CollapsedText="(Expand...)" ImageControlID="ImageDetail"
                                Collapsed="true" CollapsedSize="0" ExpandedSize="300" ExpandedImage="~/Images/collapse.png" CollapsedImage="~/Images/expand.png">
                            </cc1:CollapsiblePanelExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
