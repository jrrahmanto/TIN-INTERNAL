<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewTradeFeedAdjustment.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ViewTradeFeedAdjustment" %>

<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>

<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <h1>View Trade Register Adjustment</h1>
    <form id="form2">
    <div>
        <asp:Panel ID="pnlException" runat="server">
            <table cellpadding="1" cellspacing="1" style="width: 100%;">
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
                        Exchange</td>
                    <td style="width:10px">
                        :</td>
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
                    <td >
                        Business Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc3:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td >
                        Contract</td>
                    <td>
                        :</td>
                    <td>
                        <uc4:CtlContractCommodityLookup ID="CtlContractLookup1" 
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td >
                        Clearing Member</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Tradefeed ID</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbTradeFeedID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSearch" runat="server" 
                    Text="     Search" CssClass="button_search" onclick="uiBtnSearch_Click" />
                        &nbsp;</td>
                </tr>
            </table>
            </div>
                </div>
            </td>
        </tr>
        </table>
            <table class="table-datagrid">
                <tr>
                    <td valign="top" style="width: 100%">
                        <asp:GridView ID="uiDgTradeFeed" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="TradeFeedID" MouseHoverRowHighlightEnabled="True" 
                            RowHighlightColor="" Width="100%" AllowPaging="True" AllowSorting="True" 
                            onpageindexchanging="uiDgTradeFeed_PageIndexChanging" 
                            onsorting="uiDgTradeFeed_Sorting" PageSize="50" 
                            onrowdatabound="uiDgTradeFeed_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <a href='EntryTradeFeed.aspx?eType=adjust&tradeFeedId=<%# DataBinder.Eval(Container.DataItem, "TradeFeedID") %>&exchangeId=<%# DataBinder.Eval(Container.DataItem, "ExchangeId") %>&businessDate=<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "BusinessDate").ToString()).Date.ToString("yyyy/MM/dd") %>&approvalStatus=<%# DataBinder.Eval(Container.DataItem, "ApprovalStatus") %>' >
                                            Edit
                                        </a>
                                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeedID" 
                                    SortExpression="TradeFeedID" >
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExchangeName" HeaderText="Exchange" 
                                    SortExpression="ExchangeName" />
                                     <asp:BoundField DataField="CommCode" HeaderText="Code" 
                                    SortExpression="CommCode" ItemStyle-HorizontalAlign="Center" >
                                         <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
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
                                <asp:BoundField DataField="SellerCMCode" HeaderText="Seller" 
                                    SortExpression="SellerCMCode" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BuyerCMCode" HeaderText="Buyer" 
                                    SortExpression="BuyerCMCode" >
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblContractId" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
    <br />
    </form>
</asp:Content>