<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlYearMonthBuktiPotong.ascx.cs" Inherits="Controls_CtlYearMonthBuktiPotong" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:DropDownList ID="uiDdlMonth" runat="server" Width="75px">
    <asp:ListItem></asp:ListItem>
    <asp:ListItem Value="1">1</asp:ListItem>
    <asp:ListItem Value="2">2</asp:ListItem>
    <asp:ListItem Value="3">3</asp:ListItem>
    <asp:ListItem Value="4">4</asp:ListItem>
    <asp:ListItem Value="5">5</asp:ListItem>
    <asp:ListItem Value="6">6</asp:ListItem>
    <asp:ListItem Value="7">7</asp:ListItem>
    <asp:ListItem Value="8">8</asp:ListItem>
    <asp:ListItem Value="9">9</asp:ListItem>
    <asp:ListItem Value="10">10</asp:ListItem>
    <asp:ListItem Value="11">11</asp:ListItem>
    <asp:ListItem Value="12">12</asp:ListItem>
</asp:DropDownList>
<asp:TextBox ID="uiTxbYear" runat="server" MaxLength="4" Width="70px"></asp:TextBox>
<cc1:FilteredTextBoxExtender ID="uiTxbYear_FilteredTextBoxExtender" 
    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="uiTxbYear">
</cc1:FilteredTextBoxExtender>
&nbsp;
