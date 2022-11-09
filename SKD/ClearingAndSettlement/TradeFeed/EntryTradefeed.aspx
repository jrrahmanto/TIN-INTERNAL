<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryTradefeed.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_EntryTradefeed" %>

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
  <h1>Manage Trade Register</h1>
    <asp:Panel ID="pnlException" runat="server">
      
      <table cellpadding="1" cellspacing="1" style="width:100%;">
      
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
                 
                <tr>
                    <td style="width:100px">
                        TradeFeed ID
                    </td>
                    <td style="width:10px">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbTradeFeedID" CssClass="required" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bussiness Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="uiDtpBussDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Exchange
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="uiDdlExchange" CssClass="required" runat="server" DataSourceID="ObjectDataSourceExchangeDll"
                            DataTextField="ExchangeCode" DataValueField="ExchangeId">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ObjectDataSourceExchangeDll" runat="server" SelectMethod="GetExchanges"
                            TypeName="Exchange" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trade Time
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="uiDtpTradeTIme" runat="server" />
                        &nbsp;<asp:TextBox ID="uiTxtTransTime" runat="server" Height="20px" Width="75px"></asp:TextBox><cc1:MaskedEditExtender ID="uiTxtTransTime_MaskedEditExtender" runat="server" Mask="99:99"
                            MaskType="Time" TargetControlID="uiTxtTransTime" UserTimeFormat="TwentyFourHour">
                        </cc1:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Trade Received Timer
                    </td>
                    <td class="style3">
                        :
                    </td>
                    <td class="style3">
                        <uc1:CtlCalendarPickUp ID="uiDtpTradeReceived" runat="server" />
                        &nbsp;<asp:TextBox ID="uiTxtReceiveTime" runat="server" Height="20px" Width="75px"></asp:TextBox><cc1:MaskedEditExtender ID="uiTxtReceiveTime_MaskedEditExtender" runat="server" Enabled="True"
                            TargetControlID="uiTxtReceiveTime" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour">
                        </cc1:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trade Time Offset
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbTradeTimeOfset" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Contract
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc6:CtlContractCommodityLookup ID="uiCtlContract" 
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Price
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxbPrice" FilterTextBox="Money" runat="server" CssClass="number-required" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Quantity
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbQuantity" CssClass="number-required" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Buyer Clearing Member
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc3:CtlClearingMemberLookup ID="uiCtlBuyerCM" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Buyer Exchange Member
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc4:CtlExchangeMemberLookup ID="uiCtlBuyerEM" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Buyer Account
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc5:CtlInvestorLookup ID="uiCtlBuyerInvestor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Clearing Member
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc3:CtlClearingMemberLookup ID="uiCtlSellerCM" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Exchange Member
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc4:CtlExchangeMemberLookup ID="uiCtlSellerEM" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Account
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <uc5:CtlInvestorLookup ID="uiCtlSellerINvestor" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Buyer Trade Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerTradeType" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="Tr3" runat="server">
                    <td>
                        Buyer Account Give Up Code
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerInvestorGiveUpCode" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr id="Tr4" runat="server">
                    <td>
                        Buyer Give Up Comm
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerGiveUpCom" runat="server" MaxLength="24"></asp:TextBox>
                        <cc1:MaskedEditExtender ID="uiTxbBuyerGiveUpCom_MaskedEditExtender" 
                            runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" 
                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                            Mask="99,999,999,999,999.9999" MaskType="Number" MessageValidatorTip="False" 
                            TargetControlID="uiTxbBuyerGiveUpCom">
                        </cc1:MaskedEditExtender>
                    </td>
                </tr>
                <tr id="Tr5" runat="server">
                    <td>
                        Buyer Comp Trade Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerCompTradeTradeType" runat="server" MaxLength="1"></asp:TextBox>
                    </td>
                </tr>
                <tr id="Tr6" runat="server">
                    <td>
                        Buyer Reference
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerReference" runat="server" Height="70px" MaxLength="50"
                            Width="270px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Account Give Up Code
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerInvestorGiveUpCode" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Give Up Comm
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxbSellerGiveUpComm" runat="server" 
                            CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Reference
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerReference" runat="server" Height="70px" 
                            TextMode="MultiLine" Width="270px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Trade Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerTradeType" runat="server" MaxLength="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Com Trade Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerComTradeType" runat="server" MaxLength="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Total Leg
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbSellerTotalLeg" runat="server" MaxLength="24"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trade Strike Price
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxbTradeSrike" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trade Version
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbTradeVersion" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Buy Total Leg
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbBuyerTotalLeg" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trade Option Type
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbTradeOpt" runat="server" MaxLength="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Contra Indicator
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbContractIndicator" runat="server" MaxLength="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Exchange Reference
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbExchangeReference" runat="server" Height="70px" 
                            MaxLength="50" TextMode="MultiLine" Width="270px"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                        Approval Description
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxbApprovalDesc" CssClass="required" runat="server" Height="93px" MaxLength="100"
                            TextMode="MultiLine" Width="400px"></asp:TextBox>
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
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                            OnClick="uiBtnSave_Click" />
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve"
                            OnClick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                            Text="    Reject" OnClick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                            OnClick="uiBtnCancel_Click" Text="      Cancel" />
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

