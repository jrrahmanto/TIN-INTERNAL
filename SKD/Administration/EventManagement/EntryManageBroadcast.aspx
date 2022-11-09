<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageBroadcast.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryManageBroadcast" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Broadcast</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
           <td colspan="3">
           <asp:Label ID="uiLblSuccess" runat="server" Visible="False"></asp:Label>
           </td>
        </tr>
        <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                            </asp:BulletedList>
                        </td>
         </tr>
        <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                    
                    <tr id="TRAction" runat="server">
                        <td style="width:150px">
                            <asp:Label ID="uiLblAction" runat="server" Text="Message"></asp:Label>
                        </td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxbBroadcastMessage" runat="server" Height="102px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                        &nbsp;</td>
                        <td>
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="       Submit" onclick="uiBtnSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                </table>
            </div>
            </div>
            </td>
            </tr>
    </table>      
</asp:Content>

