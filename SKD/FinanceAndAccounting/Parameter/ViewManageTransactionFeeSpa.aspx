<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewManageTransactionFeeSpa.aspx.cs" Inherits="FinanceAndAccounting_Parameter_ViewManageTransactionFeeSpa" Title="Untitled Page" %>

<%@ Register Src="../../Lookup/CtlCommodityLookup.ascx" TagName="CtlCommodityLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 416px;
        }
        .style2
        {
            width: 162px;
        }
        .style3
        {
            width: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Manage Transaction Fee Spa</h1>
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
                                    <td class="style2">
                                        CM Type
                                    </td>
                                    <td class="style3">
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="uiDdlCmType" Width="65px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Text="Broker" Value="B" />
                                            <asp:ListItem Text="Trader" Value="T" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        Approval Status
                                    </td>
                                    <td class="style3">
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                    </td>
                                    <td class="style3">
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
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgTransactionFeeSpa" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                                AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="EffectiveStartDate,ApprovalStatus,CMType"
                                OnPageIndexChanging="uiDgTransactionFeeSpa_PageIndexChanging" OnSorting="uiDgTransactionFeeSpa_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("TransactionFeeSPAID", "~/FinanceAndAccounting/Parameter/EntryManageTransactionFeeSpa.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Clearing Member Type" DataField="CMDesc" SortExpression="CMDesc" />
                                    <asp:BoundField DataField="EffectiveStartDate" 
                                        DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective Start Date" 
                                        SortExpression="EffectiveStartDate">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UpperLimit" DataFormatString="{0:#,##0.###}" 
                                        HeaderText="Upper Limit" SortExpression="UpperLimit">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTransactionFeeSpa" runat="server" SelectMethod="SelectTransactionFeeSPAByCmTypeAndStatus"
                                TypeName="TransactionFee" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlCmType" Name="CMType" PropertyName="SelectedValue"
                                        Type="String" />
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
