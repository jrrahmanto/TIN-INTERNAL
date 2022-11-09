<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManageBankAccount.aspx.cs" Inherits="FinanceAndAccounting_Parameter_ViewManageKBIAccount" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Manage Bank Account</h1>
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
                                    <td style="width: 150px">Account type
                                    </td>
                                    <td style="width: 10px">:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlAccountType" runat="server">
                                            <asp:ListItem Value="RD">Rekening Deposit</asp:ListItem>
                                            <asp:ListItem Value="RS">Rekening Settlement</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Bank Code
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlBankCode" runat="server" DataSourceID="ObjectDataSourceBank"
                                            DataTextField="Code" DataValueField="BankID">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="ObjectDataSourceBank" runat="server" SelectMethod="GetBank"
                                            TypeName="Bank"></asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Participant
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Currency
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlCurrency" runat="server">
                                            <%--<asp:ListItem Value="1">IDR</asp:ListItem>--%>
                                            <asp:ListItem Value="5">USD</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Status
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server">
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Account Status
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlAccountStatus" runat="server">
                                            <asp:ListItem Value="R">Registered</asp:ListItem>
                                            <asp:ListItem Value="A">Activated</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
                                        <asp:Button ID="uiBtnDownload" runat="server" CssClass="button_download"
                                            Text="      Download" OnClick="uiBtnDownload_Click" />
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
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBankAccount" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="AccountNo,ApprovalStatus,BankID,EffectiveStartDate"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgBankAccount_PageIndexChanging"
                                OnSorting="uiDgBankAccount_Sorting" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("BankAccountID", "~/FinanceAndAccounting/Parameter/EntryManageBankAccount.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Participant Name" DataField="Name" ReadOnly="True"
                                        SortExpression="Name">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Bank Code" DataField="BankCode" ReadOnly="True"
                                        SortExpression="BankCode">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EffectiveStartDate"
                                        DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective Start Date"
                                        SortExpression="EffectiveStartDate">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccountNo" HeaderText="Account No" SortExpression="AccountNo"
                                        ReadOnly="True" />
                                    <asp:BoundField HeaderText="Account Type" DataField="AccountType"
                                        SortExpression="AccountType" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>No Record</EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBankAccount" runat="server" SelectMethod="SelectBankAccountByAccountNoAndStatus"
                                TypeName="BankAccount" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlAccountType" Name="accountType" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlBankCode" Name="bankId" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="cmId" PropertyName="LookupTextBoxID"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="uiDdlAccountStatus" Name="AccStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlCurrency" Name="ccyId" PropertyName="SelectedValue"
                                        Type="Decimal" />

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
                <td></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
