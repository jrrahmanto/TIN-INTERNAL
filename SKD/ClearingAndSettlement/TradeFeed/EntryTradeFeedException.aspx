<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryTradeFeedException.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_EntryTradeFeedException" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <h1>Manage Trade Register Exception</h1>
   <table cellpadding="1" cellspacing="1" style="width:100%;">
   <tr>
            <td>
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
         <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
    <table class="table-datagrid">
       
        <tr>
            <td >
                <asp:DetailsView ID="uiDvTradeFeedException" runat="server" 
                    AutoGenerateRows="False" 
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    DataKeyNames="TradeFeedID,BusinessDate,ExchangeID,ApprovalStatus" 
                    DataSourceID="ObjectDataSourceTradeFeedException" GridLines="Horizontal" 
                    Height="50px" Width="600px">
                    <RowStyle CssClass="tblRowStyle" />
                    <Fields>
                        <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeed ID" 
                            ReadOnly="True" SortExpression="TradeFeedID" />
                        <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" 
                            ReadOnly="True" SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}"/>
                        <asp:BoundField DataField="ExchangeID" HeaderText="Exchange ID" ReadOnly="True" 
                            SortExpression="ExchangeID" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" 
                            ReadOnly="True" SortExpression="ApprovalStatus" />
                        <asp:BoundField DataField="Message" HeaderText="Message" 
                            SortExpression="Message" />
                        <asp:BoundField DataField="ApprovalDesc" HeaderText="Approval Description" 
                            SortExpression="ApprovalDesc" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" 
                            SortExpression="CreatedBy" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" 
                            SortExpression="CreatedDate" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}"/>
                        <asp:BoundField DataField="LastUpdatedBy" HeaderText="Last Updated By" 
                            SortExpression="LastUpdatedBy" />
                        <asp:BoundField DataField="LastUpdatedDate" HeaderText="Last Updated Date" 
                            SortExpression="LastUpdatedDate" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}"/>
                    </Fields>
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="ObjectDataSourceTradeFeedException" runat="server" 
                    SelectMethod="GetData" TypeName="TradefeedException">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="exchangeID" QueryStringField="exchangeId" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="businessDate" QueryStringField="businessDate" 
                            Type="DateTime" />
                        <asp:QueryStringParameter Name="tradefeedID" QueryStringField="tradeFeedId" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:DetailsView ID="uiDvRawTradeFeed" runat="server" AutoGenerateRows="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" DataKeyNames="ExchangeId,TradeFeedID,BusinessDate" 
                    DataSourceID="ObjectDataSourceRawTradeFeed" GridLines="Horizontal" 
                    Height="50px" Width="600px">
                    <RowStyle CssClass="tblRowStyle" />
                    <Fields>
                        <asp:BoundField DataField="ExchangeId" HeaderText="ExchangeId" ReadOnly="True" 
                            SortExpression="ExchangeId" />
                        <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeed ID" 
                            ReadOnly="True" SortExpression="TradeFeedID" />
                        <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" 
                            ReadOnly="True" SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}"/>
                        <asp:BoundField DataField="TradeTime" HeaderText="Trade Time" 
                            SortExpression="TradeTime" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}"/>
                        <asp:BoundField DataField="TradeReceivedTime" HeaderText="Trade Received Time" 
                            SortExpression="TradeReceivedTime" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}"/>
                        <asp:BoundField DataField="TradeTimeOffset" HeaderText="Trade Time Offset" 
                            SortExpression="TradeTimeOffset" />
                        <asp:BoundField DataField="ProductCode" HeaderText="Product" 
                            SortExpression="ProductCode" />
                        <asp:BoundField DataField="MonthContract" HeaderText="Month Contract" 
                            SortExpression="MonthContract" />
                        <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:#,##0.##########}"/>
                        <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                        <asp:BoundField DataField="BuyerCMCode" HeaderText="Buyer Clearing Member" 
                            SortExpression="BuyerCMCode" />
                        <asp:BoundField DataField="BuyerEMCode" HeaderText="Buyer Exchange Member" 
                            SortExpression="BuyerEMCode" />
                        <asp:BoundField DataField="BuyerInvCode" HeaderText="Buyer Account" 
                            SortExpression="BuyerInvCode" />
                        <asp:BoundField DataField="SellerCMCode" HeaderText="Seller Clearing Member" 
                            SortExpression="SellerCMCode" />
                        <asp:BoundField DataField="SellerEMCode" HeaderText="Seller Exchange Member" 
                            SortExpression="SellerEMCode" />
                        <asp:BoundField DataField="SellerInvCode" HeaderText="Seller Account" 
                            SortExpression="SellerInvCode" />
                        <asp:BoundField DataField="BuyerInvGiveUpCode" HeaderText="Buyer Account GiveUp Code" 
                            SortExpression="BuyerInvGiveUpCode" />
                        <asp:BoundField DataField="BuyerGiveUpComm" HeaderText="Buyer GiveUp Comm" 
                            SortExpression="BuyerGiveUpComm" />
                        <asp:BoundField DataField="BuyTotLeg" HeaderText="Buyer Total Leg" 
                            SortExpression="BuyTotLeg" />
                        <asp:BoundField DataField="BuyerTrdType" HeaderText="Buyer Trade Type" 
                            SortExpression="BuyerTrdType" />
                        <asp:BoundField DataField="BuyerCompTradeType" HeaderText="Buyer Comp Trade Type" 
                            SortExpression="BuyerCompTradeType" />
                        <asp:BoundField DataField="BuyerRef" HeaderText="Buyer Ref" 
                            SortExpression="BuyerRef" />
                        <asp:BoundField DataField="SellerInvGiveUpCode" 
                            HeaderText="Seller Account GiveUp Code" SortExpression="SellerInvGiveUpCode" />
                        <asp:BoundField DataField="SellerTotalLeg" HeaderText="Seller Total Leg" 
                            SortExpression="SellerTotalLeg" />
                        <asp:BoundField DataField="SellerGiveUpComm" HeaderText="Seller GiveUp Commision" 
                            SortExpression="SellerGiveUpComm" />
                        <asp:BoundField DataField="SellerRef" HeaderText="Seller Ref" 
                            SortExpression="SellerRef" />
                        <asp:BoundField DataField="SellerTrdType" HeaderText="Seller Trade Type" 
                            SortExpression="SellerTrdType" />
                        <asp:BoundField DataField="SellerCompTradeType" 
                            HeaderText="Seller Comp Trade Type" SortExpression="SellerCompTradeType" />
                        <asp:BoundField DataField="TradeStrikePrice" HeaderText="Trade Strike Price" 
                            SortExpression="TradeStrikePrice" />
                        <asp:BoundField DataField="TradeVersion" HeaderText="Trade Version" 
                            SortExpression="TradeVersion" />
                        <asp:BoundField DataField="TradeOptType" HeaderText="Trade Opt Type" 
                            SortExpression="TradeOptType" />
                        <asp:BoundField DataField="ContraIndicator" HeaderText="Contra Indicator" 
                            SortExpression="ContraIndicator" />
                        <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" 
                            SortExpression="ExchangeRef" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" 
                            SortExpression="CreatedBy" />
                    </Fields>
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="ObjectDataSourceRawTradeFeed" runat="server" 
                    SelectMethod="GetData" TypeName="Tradefeed">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="exchangeID" QueryStringField="exchangeId" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="tradefeedID" QueryStringField="tradeFeedId" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="businessDate" QueryStringField="businessDate" 
                            Type="DateTime" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                            <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                    Text="     Approve" onclick="uiBtnApprove_Click"  />
                            <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" onclick="uiBtnReject_Click" 
                    />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                                Text="      Cancel" onclick="uiBtnCancel_Click" 
                            />
                        </td>
        </tr>
    </table>
    </div>
            </div>
            </td>
        </tr>
        </table>
</asp:Content>

