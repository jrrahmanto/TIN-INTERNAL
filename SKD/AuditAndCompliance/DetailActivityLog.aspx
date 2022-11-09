<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailActivityLog.aspx.cs" Inherits="AuditAndCompliance_DetailActivityLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Detail Activity Log</h1>
<table cellpadding="0" cellspacing="0" style="width:100%;">
<tr>
                        <td colspan="3">
                            <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" 
                                ForeColor="#FF3300" Visible="False"></asp:Label>
                        </td>
                    </tr>

        <tr>
            <td>
              <div class="shadow_view">
                 <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td style="width:100px">
                            Log Time</td>
                        <td style="width:10px">
                        :</td>
                        <td>
                            <asp:Label ID="uiLblLogTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Log Activity</td>
                        <td>
                        :</td>
                        <td>
                            <asp:Label ID="uiLblLogActivity" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Log Message</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="uiLblLogMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Source IP</td>
                        <td valign="top">
                            :</td>
                        <td>
                            <asp:Label ID="uiLblSourceIP" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Username</td>
                        <td valign="top">
                            :</td>
                        <td>
                            <asp:Label ID="uiLblUserName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                                Text="      Cancel" onclick="uiBtnCancel_Click" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        </table>
</asp:Content>

