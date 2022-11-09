<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <div runat="server" style="align-content:center; width:100%">
        <table style="margin: 0 auto;">
            <tr>
                <td colspan="2"><h2>Data yang membutuhkan Persetujuan</h2></td>
            </tr>
        </table>
        <br />
        <table border="1" cellspacing="0" style="margin: 0 auto;">
            <tr style="background-color: #233A51; color: white; text-align: center;">
                <td style="width: 350px"><b>Notification</b></td>
                <td style="width: 120px"><b>Pending Item</b></td>
            </tr>
            <tr>
                <td <% if (uiLblCounterCM.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>>Approval Pendaftaran Kepesertaan</td>
                <td align="center" <% if (uiLblCounterCM.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>><asp:Label ID="uiLblCounterCM" runat="server"/></td>
            </tr>
            <tr>
                <td <% if (uiLblCounterBA.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>>Approval Bank Account</td>
                <td align="center" <% if (uiLblCounterBA.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>><asp:Label ID="uiLblCounterBA" runat="server"/></td>
            </tr>
            <tr>
                <td <% if (uiLblCounterBT.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>>Approval Bank Transaction</td>
                <td align="center" <% if (uiLblCounterBT.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>><asp:Label ID="uiLblCounterBT" runat="server"/></td>
            </tr>
            <tr>
                <td <% if (uiLblCounterTF.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>>Approval Shipping Instructions</td>
                <td align="center" <% if (uiLblCounterTF.Text != "0") { %>style="animation: blinking 1s infinite;"<% } %>><asp:Label ID="uiLblCounterTF" runat="server"/></td>
            </tr>
        </table>
    </div>
    <br />
    <br />
</asp:Content>
