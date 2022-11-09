<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShippingInstruction.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ShippingInstruction" %>

<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>
<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            height: 85px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Approval Shipping Instruction</h1>
    <asp:Panel ID="pnlException" runat="server" DefaultButton="uiBtnSearch">
        <form id="form2">
            <div>
                <table cellpadding="1" cellspacing="1" style="width: 100%;">
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
                                            <td style="width:10px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>Exchange</td>
                                            <td>:</td>
                                            <td>
                                                <asp:DropDownList ID="uiDdlExchange" runat="server" 
                                                    DataSourceID="ObjectDataSourceExchangeDll" DataTextField="ExchangeCode" 
                                                    DataValueField="ExchangeId">
                                                </asp:DropDownList>
                                                <asp:ObjectDataSource ID="ObjectDataSourceExchangeDll" runat="server" 
                                                    SelectMethod="SelectExchange" TypeName="Exchange"></asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Business Date</td>
                                            <td>:</td>
                                            <td><uc3:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td><asp:Button ID="uiBtnSearch" runat="server" Text="     Search" CssClass="button_search" onclick="uiBtnSearch_Click" />&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="uiDgTradeFeed0" runat="server" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="TradeFeedID" 
                                        MouseHoverRowHighlightEnabled="True" 
                                        onpageindexchanging="uiDgTradeFeed_PageIndexChanging" 
                                        onrowdatabound="uiDgTradeFeed_RowDataBound" onsorting="uiDgTradeFeed_Sorting" 
                                        PageSize="15" RowHighlightColor="" Width="100%">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <a href='ApprovalSI.aspx?eType=edit&amp;tradeFeedId=<%# DataBinder.Eval(Container.DataItem, "TradeFeedID") %>&amp;exchangeId=<%# DataBinder.Eval(Container.DataItem, "ExchangeId") %>&amp;businessDate=<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "BusinessDate").ToString()).Date.ToString("yyyy/MM/dd") %>&amp;approvalStatus=<%# DataBinder.Eval(Container.DataItem, "ApprovalStatus") %>' >
                                                    Approval </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ExchangeId" HeaderText="ExchangeId" 
                                                Visible="False" />
                                            <asp:BoundField DataField="BusinessDate" HeaderText="BusinessDate" 
                                                Visible="False" />
                                            <asp:BoundField DataField="ApprovalStatus" HeaderText="ApprovalStatus" 
                                                Visible="False" />
                                            <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeedID" 
                                                SortExpression="TradeFeedID">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ExchangeName" HeaderText="Exchange" 
                                                SortExpression="ExchangeName" />
                                            <asp:BoundField DataField="CommName" HeaderText="Contract" 
                                                SortExpression="CommName" />
                                            <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                <ItemTemplate>
                                                    <asp:Label ID="uiLblPrice0" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Price") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" 
                                                SortExpression="Quantity">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Seller" SortExpression="SellerCMCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="uiLblSellerCmId0" runat="server" 
                                                        Text='<%# Bind("SellerCMCode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="uiLblSellerCmCode0" runat="server" 
                                                        Text='<%# Bind("SellerCMCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Buyer" SortExpression="BuyerCMCode">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("BuyerCMCode") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="uiLblBuyerCmId0" runat="server" 
                                                        Text='<%# Bind("BuyerCMCode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="uiLblBuyerCmCode0" runat="server" 
                                                        Text='<%# Bind("BuyerCMCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ShippingInstructionFlag" HeaderText="Status" 
                                                SortExpression="ShippingInstructionFlag">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="uiLblContractId0" runat="server" 
                                                        Text='<%# Bind("ContractID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table class="table-datagrid">
                            <tr>
                                <td valign="top" style="width: 100%">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="uiDgTradeFeed" runat="server" AutoGenerateColumns="False" 
                                            DataKeyNames="TradeFeedID" MouseHoverRowHighlightEnabled="True" 
                                            RowHighlightColor="" Width="100%" AllowPaging="True" AllowSorting="True" 
                                            onpageindexchanging="uiDgTradeFeed_PageIndexChanging" 
                                            onsorting="uiDgTradeFeed_Sorting" PageSize="50" 
                                            onrowdatabound="uiDgTradeFeed_RowDataBound">
                                            <RowStyle CssClass="tblRowStyle" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Detail">
                                                    <ItemTemplate>
                                                        <a href='ApprovalSI.aspx?eType=edit&tradeFeedId=<%# DataBinder.Eval(Container.DataItem, "TradeFeedID") %>&exchangeId=<%# DataBinder.Eval(Container.DataItem, "ExchangeId") %>&businessDate=<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "BusinessDate").ToString()).Date.ToString("yyyy/MM/dd") %>&approvalStatus=<%# DataBinder.Eval(Container.DataItem, "ApprovalStatus") %>'>
                                                            Approval
                                                        </a>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ExchangeId" HeaderText="ExchangeId" 
                                                    Visible="False" />
                                                <asp:BoundField DataField="BusinessDate" HeaderText="BusinessDate" 
                                                    Visible="False" />
                                                <asp:BoundField DataField="ApprovalStatus" HeaderText="ApprovalStatus" 
                                                    Visible="False" />
                                                <asp:BoundField DataField="ExchangeRef" HeaderText="ExchangeRef" 
                                                    Visible="True">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeedID" 
                                                    SortExpression="TradeFeedID" Visible="false">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ExchangeName" HeaderText="Exchange" 
                                                    SortExpression="ExchangeName">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CommName" HeaderText="Contract" 
                                                    SortExpression="CommName" Visible="false"/>
                                                <asp:BoundField DataField="CommCode" HeaderText="Code" 
                                                    SortExpression="CommCode" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ContractYearMonth" HeaderText="Contract Month" 
                                                    SortExpression="ContractYearMonth" ItemStyle-HorizontalAlign="Center"  />
                                                <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="uiLblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Price") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" 
                                                    SortExpression="Quantity" >
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="CM Seller" SortExpression="SellerCMCode" itemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="uiLblSellerCmId" Visible="false" runat="server" Text='<%# Bind("SellerCMCode") %>'></asp:Label>
                                                        <asp:Label ID="uiLblSellerCmCode" runat="server" Text='<%# Bind("SellerCMCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CM Buyer" SortExpression="BuyerCMCode" itemStyle-HorizontalAlign="Center">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("BuyerCMCode") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="uiLblBuyerCmId" Visible="false" runat="server" Text='<%# Bind("BuyerCMCode") %>'></asp:Label>
                                                        <asp:Label ID="uiLblBuyerCmCode" runat="server" Text='<%# Bind("BuyerCMCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="StatusShipping" HeaderText="Status" 
                                                    SortExpression="StatusShipping">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField  Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="uiLblContractId" Visible="false" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            <br />
            <br />
        </form>
    </asp:Panel>
</asp:Content>