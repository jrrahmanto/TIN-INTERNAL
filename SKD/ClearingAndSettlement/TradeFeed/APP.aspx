<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="APP.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_APP" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup" TagPrefix="uc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Approval Form APP</h1>
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
                                            <td style="width:200px">&nbsp;</td>
                                            <td style="width:20px">&nbsp;</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>Exchange Ref</td>
                                            <td>:</td>
                                            <td><asp:TextBox ID="uiTxtExchangeRef" runat="server"></asp:TextBox></td>
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
                                        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ProgressID" 
                                        MouseHoverRowHighlightEnabled="True" 
                                        onpageindexchanging="uiDgTradeFeed_PageIndexChanging" 
                                        onrowdatabound="uiDgTradeFeed_RowDataBound" onsorting="uiDgTradeFeed_Sorting" 
                                        PageSize="15" RowHighlightColor="" Width="100%">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <a href='ApprovalApp.aspx?eType=edit&amp;progressId=<%# DataBinder.Eval(Container.DataItem, "ProgressID") %>'> Approval </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" SortExpression="ExchangeRef">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy HH:mm}" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BuyerId" HeaderText="BuyerId" SortExpression="BuyerId">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SellerId" HeaderText="SellerId" SortExpression="SellerId">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Product Code" SortExpression="ProductCode">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:#,##0.##}">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Volume" HeaderText="Volume" SortExpression="Volume" DataFormatString="{0:#,##0.##}">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AppStatus" HeaderText="Status" SortExpression="AppStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
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
                                        DataKeyNames="ProgressID" MouseHoverRowHighlightEnabled="True" 
                                        RowHighlightColor="" Width="100%" AllowPaging="True" AllowSorting="True" 
                                        onpageindexchanging="uiDgTradeFeed_PageIndexChanging" 
                                        onsorting="uiDgTradeFeed_Sorting" PageSize="50" 
                                        onrowdatabound="uiDgTradeFeed_RowDataBound">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <a href='ApprovalApp.aspx?eType=edit&amp;progressId=<%# DataBinder.Eval(Container.DataItem, "ProgressID") %>'> Approval </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" SortExpression="ExchangeRef">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy HH:mm}" >
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BuyerId" HeaderText="BuyerId" SortExpression="BuyerId">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SellerId" HeaderText="SellerId" SortExpression="SellerId">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProductCode" HeaderText="Product Code" SortExpression="ProductCode">
                                                <ItemStyle HorizontalAlign="Center"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:#,##0.##}">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Volume" HeaderText="Volume" SortExpression="Volume" DataFormatString="{0:#,##0.##}">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AppStatus" HeaderText="Status" SortExpression="AppStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
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