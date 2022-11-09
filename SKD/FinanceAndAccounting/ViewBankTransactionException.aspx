<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewBankTransactionException.aspx.cs" Inherits="FinanceAndAccounting_ViewBankTransactionException" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Bank Transaction Exception</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:150px">
                                        Transaction Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Bank
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlBank" runat="server" Width="126px" AppendDataBoundItems="True"
                                            DataSourceID="ObjectDataSourceBank" DataTextField="Code" DataValueField="BankID">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="ObjectDataSourceBank" runat="server" SelectMethod="GetBank"
                                            TypeName="Bank"></asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Approval Status
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server" Width="126px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
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
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table class="table-datagrid">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBankTransException" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TransactionDate,TransactionSeq,BankID,ApprovalStatus"
                                OnRowDataBound="uiDgBankTransException_RowDataBound" AllowPaging="True" AllowSorting="True"
                                OnPageIndexChanging="uiDgBankTransException_PageIndexChanging" OnSorting="uiDgBankTransException_Sorting"
                                PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Adjust">
                                        <ItemTemplate>
                                            <a href="EntryBankTransactionException.aspx?transDate=<%# DataBinder.Eval(Container.DataItem, "TransactionDate") %>&transSeq=<%# DataBinder.Eval(Container.DataItem, "TransactionSeq") %>&bankId=<%# DataBinder.Eval(Container.DataItem, "BankID") %>&approvalStatus=<%# DataBinder.Eval(Container.DataItem, "ApprovalStatus") %>">
                                                Adjust </a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="TransactionDate"
                                        ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Transaction Seq" DataField="TransactionSeq" ReadOnly="True"
                                        SortExpression="TransactionSeq"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Bank" SortExpression="BankID">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblBank" runat="server" Text='<%# Bind("BankID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("BankID") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBankTransException" runat="server" SelectMethod="GetBankTransactionExceptionByTransDateBankIDApprovalStatus"
                                TypeName="Bank">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="transDate" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlBank" Name="bankId" PropertyName="SelectedValue"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
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
