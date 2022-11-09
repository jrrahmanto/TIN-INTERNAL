<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailOutGoingFeed.aspx.cs" Inherits="Administration_DetailOutGoingFeed" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Detail Outgoing Feed</h1>
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
                            Feed Type</td>
                        <td>
                        :</td>
                        <td>
                            <asp:Label ID="uiLblFeedType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Feed No</td>
                        <td>
                        :</td>
                        <td>
                            <asp:Label ID="uiLblFeedNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Business Date</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="uiLblBusinessDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Feed Message</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="uiLblFeedMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Submitted Status</td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:Label ID="uiLblSubmittedStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Submitted Time</td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:Label ID="uiLblSubmittedTime" runat="server"></asp:Label>
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
