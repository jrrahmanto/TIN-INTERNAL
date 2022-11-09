<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewTradeFeedProxyLog.aspx.cs" Inherits="AuditAndCompliance_ViewTradeFeedProxyLog" %>

<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
    <h1>View TradeFeed Proxy Log</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <caption>
                <tr>
                    <td>
                        <div class="shadow_view">
                            <div class="box_view">
                                <table class="table-row">
                                    <tr>
                                        <td style="width:100px">
                                            Exchange
                                        </td>
                                        <td style="width:10px">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="uiDdlExchange" runat="server" AppendDataBoundItems="True" DataSourceID="ObjectDataSourceExchange"
                                                DataTextField="ExchangeName" DataValueField="ExchangeId" Width="126px">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceExchange" runat="server" SelectMethod="GetExchanges"
                                                TypeName="Exchange"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Log Date
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" OnClick="uiBtnSearch_Click"
                                                Text="     Search" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </caption>
        </table>
        <table class="table-datagrid">
            <tr>
                <td>
                    <ContentTemplate>
                            <asp:GridView ID="uiDgTradeFeedLog" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="ExchangeId,LogDate,LogSequence"
                                OnPageIndexChanging="uiDgTradeFeedLog_PageIndexChanging" OnRowCommand="uiDgTradeFeedLog_RowCommand"
                                OnRowDataBound="uiDgTradeFeedLog_RowDataBound" OnSorting="uiDgTradeFeedLog_Sorting"
                                PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:ButtonField CommandName="download" Text="Download">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:TemplateField HeaderText="Exchange Id" SortExpression="ExchangeId">
                                        <EditItemTemplate>
                                            <asp:Label ID="uiLblExchangeId" runat="server" Text='<%# Eval("ExchangeId") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblExchange" runat="server" Text='<%# Bind("ExchangeId") %>'></asp:Label>
                                            <asp:Label ID="uiLblExchangeId" runat="server" Visible="false" Text='<%# Bind("ExchangeId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Log Date" SortExpression="LogDate">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("LogDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblLogDate" runat="server" Text='<%# Bind("LogDate", "{0:yyyy/MM/dd}") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("LogDate", "{0:dd-MMM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Log Sequence" SortExpression="LogSequence">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("LogSequence") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblLogSeq" runat="server" Text='<%# Bind("LogSequence") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTradeFeedLog" runat="server" SelectMethod="GetTradeFeedProxyLogByExchangeLogDate"
                                TypeName="TradeFeedProxyLog">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlExchange" Name="exchangeId" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="logDate" PropertyName="Text"
                                        Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
