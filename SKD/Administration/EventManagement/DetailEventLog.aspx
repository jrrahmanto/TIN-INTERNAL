<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailEventLog.aspx.cs" Inherits="Administration_EventManagement_DetailEventLog"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Detail Event Log</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;"> 
  <tr>
      <td colspan="3">
      <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
      </td>
 </tr>
 
        <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                
                    <tr>
                        <td style="width:200px;">
                            EventTypeName</td>
                        <td>
                        :</td>
                        <td>
                            <asp:Label ID="uiLblEventTypeName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            EventTime</td>
                        <td>
                        :</td>
                        <td>
                            <asp:Label ID="uiLblEventTime" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            EventReceipientName</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="uiLblEventReceipientName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Delivery Channel</td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:Label ID="uiLblDeliveryChannel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Status</td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:Label ID="uiLblDeliveryStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Message</td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:Label ID="uiLblDeliveryMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
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

