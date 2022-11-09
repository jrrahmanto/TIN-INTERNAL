<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewDailyStock.aspx.cs" Inherits="PengelolaGudang_DailyStock_ViewDailyStock" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ct1" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlCommodityLookup.ascx" TagName="CtlCommodityLookup" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="w">
        <h1>View Manage Daily Stock</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td>Business Date</td>
                                    <td>:</td>
                                    <td><uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>Commodity</td>
                                    <td>:</td>
                                    <td><uc2:CtlCommodityLookup ID="uiDtpCommID" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" OnClick="uiBtnSearch_Click" /> &nbsp;
                                        <asp:Button ID="Button1" runat="server" CssClass="button_create" Text="    Create" onclick="uiBtnCreate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgStockWarehouse" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="StockId"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgStockWarehouse_PageIndexChanging"
                                OnSorting="uiDgStockWarehouse_Sorting" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("StockId", "~/PengelolaGudang/DailyStock/EntryDailyStock.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CommodityID" HeaderText="Commodity Code" SortExpression="CommodityID" />
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate" ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                                    <asp:BoundField DataField="Volume" HeaderText="Volume" SortExpression="Volume" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceStockWarehouse" runat="server" SelectMethod="GetStockWarehouse" TypeName="StockWarehouseNew">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="BusinessDate" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDtpCommID" Name="CommodityID" PropertyName="LookupTextBoxID" Type="Decimal" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </cc1:ProgressUpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>