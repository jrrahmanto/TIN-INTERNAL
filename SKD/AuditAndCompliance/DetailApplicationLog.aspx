<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DetailApplicationLog.aspx.cs" Inherits="AuditAndCompliance_DetailApplicationLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Detail Application Log</h1>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr>
                                <td colspan="3">
                                    <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" ForeColor="#FF3300"
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            
                            <tr>
                                <td  style="width:100px">
                                    Log Time
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblLogTime" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Application Module
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblApplicationModule" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Classification
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblClassification" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Log Message
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblLogMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Username
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblUserName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Source IP
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="uiLblSourceIP" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                        OnClick="uiBtnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
