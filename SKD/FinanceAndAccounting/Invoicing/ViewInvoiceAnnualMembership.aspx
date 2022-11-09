<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewInvoiceAnnualMembership.aspx.cs" Inherits="FinanceAndAccounting_Invoicing_ViewInvoiceAnnualMembership" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberInvoiceLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Invoice Annual Membership</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:150px">Invoice Date</td>
                                    <td style="width:10px">:</td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Clearing Member</td>
                                    <td>:</td>
                                    <td>
                                        <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-datagrid">
            <tr>
                <td>
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create" OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgInvoice" runat="server" AutoGenerateColumns="False" Width="100%" 
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="cmid, invoicedate, invoicetype"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgInvoice_PageIndexChanging"
                                OnSorting="uiDgInvoice_Sorting" PageSize="15" >
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ID", "~/FinanceAndAccounting/Invoicing/EntryInvoiceMonthlyMembership.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>--%>
                                    <asp:BoundField HeaderText="Code" DataField="Code" ReadOnly="True" SortExpression="Cmid">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" DataField="InvoiceDate" ReadOnly="True" SortExpression="InvoiceDate">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Path Invoice" DataField="pathInvoice" SortExpression="pathInvoice"/>
                                    <asp:BoundField HeaderText="Path Faktur Pajak" DataField="pathFakturPajak" SortExpression="pathFakturPajak" ReadOnly="True" />
                                </Columns>
                                <EmptyDataTemplate>No Record</EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceInvoice" runat="server" SelectMethod="SelectInvoiceByFilterAM"
                                TypeName="Invoice" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="invoicedate" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="cmid" PropertyName="LookupTextBoxID" Type="Decimal" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>