<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalApp.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ApprovalApp" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc3" %>
<%@ Register src="../../Lookup/CtlExchangeMemberLookup.ascx" tagname="CtlExchangeMemberLookup" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            height: 85px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <h1>Manage Form APP</h1>
    <asp:Panel ID="pnlException" runat="server">
      
      <table cellpadding="1" cellspacing="1" style="width:100%;">
      
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red"></asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
            <table class="table-row">
                <tr>
                    <td style="width:150px">Progress ID</td>
                    <td style="width:10px">:</td>
                    <td>
                        <asp:TextBox ID="uiTxbProgressID" CssClass="required" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Bussiness Date</td>
                    <td>:</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="uiDtpBussDate" runat="server" Enabled="False" ReadOnly="True"/>
                    </td>
                </tr>
                <tr>
                    <td>Exchange Ref</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="uiTxbExchangeRef" CssClass="required" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Buyer ID</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerId" CssClass="required" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Seller ID</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerId" CssClass="required" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Product Code</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="uiTxbProductCode" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Volume</td>
                    <td>:</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxbVolume" FilterTextBox="Money" runat="server" Enabled="False" ReadOnly="True"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Price</td>
                    <td>:</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxbPrice" FilterTextBox="Money" runat="server" Enabled="False" ReadOnly="True"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>APP File</td>
                    <td>:</td>
                    <td>
                        <asp:Button ID="uiBtnApp" runat="server" OnClick="uiBtnApp_Click" Text="Download" />&nbsp;
                        <asp:Label ID="uiLblApp" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" OnClick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject" OnClick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" OnClick="uiBtnCancel_Click" Text="      Cancel" />
                    </td>
                </tr>
            </table>
            </div>
                </div>
            </td>
        </tr>
        </table>
    </asp:Panel>
</asp:Content>