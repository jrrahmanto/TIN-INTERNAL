<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailWithdrawal.aspx.cs" Inherits="FinanceAndAccounting_DetailWithdrawal" %>

<%@ Register Src="../Lookup/CtlTradeProgress.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Create Withdrawal</h1>
    <asp:Panel ID="Panel1" runat="server">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td>Trade Register
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiAmount" CssClass="required" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbSecfund" runat="server" /> Use Secfund
                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="uiBtnCreate" CssClass="button_create" runat="server" Text="     Create" OnClick="uiBtnCreate_Click" />
                                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" OnClick="uiBtnCancel_Click" Text="      Cancel" />
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBankAccount" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="id"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgBankAccount_PageIndexChanging"
                                OnSorting="uiDgBankAccount_Sorting" PageSize="15" OnRowDataBound="uiDgWithdrawal_RowDataBound">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" SortExpression="id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="uiID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="businessDate" HeaderText="Transaction Date" SortExpression="businessDate" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="participant_code" HeaderText="Participan Code" SortExpression="participant_code">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="seller_name" HeaderText="Seller Name" SortExpression="seller_name">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="account" HeaderText="Account" SortExpression="account">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="bank_name" HeaderText="bank Name" SortExpression="bank_name">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="exchangeReff" HeaderText="Exchange Reff" SortExpression="exchangeReff">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Amount" SortExpression="amount">
                                        <ItemTemplate>
                                            <asp:Label ID="amount" runat="server" Text='<%# Bind("total_transfer") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("total_transfer") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Secfund" SortExpression="secfund">
                                        <ItemTemplate>
                                            <asp:Label ID="secfund" runat="server" Text='<%# Bind("secfund") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("secfund") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Transfer" SortExpression="transfer">
                                        <ItemTemplate>
                                            <asp:Label ID="transfer" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text=""></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("id", "~/FinanceAndAccounting/DeleteWithdrawal.aspx?id={0}") %>'
                                                Text="Delete"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>No Record</EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBankAccount" runat="server" SelectMethod="SelectBankAccountByAccountNoAndStatus"
                                TypeName="BankAccount" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <%--                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlAccountType" Name="accountType" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlBankCode" Name="bankId" PropertyName="SelectedValue"
                                        Type="Decimal" />--%>
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="ProgressID" PropertyName="LookupTextBoxID"
                                        Type="Decimal" />
                                    <%--                                    <asp:ControlParameter ControlID="uiDdlAccountStatus" Name="AccStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlCurrency" Name="ccyId" PropertyName="SelectedValue"
                                        Type="Decimal" />--%>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <%--                   <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

