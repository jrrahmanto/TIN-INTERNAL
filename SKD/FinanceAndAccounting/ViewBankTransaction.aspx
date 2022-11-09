<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewBankTransaction.aspx.cs" Inherits="FinanceAndAccounting_ViewBankTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Bank Transaction</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:150px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:150px">
                                        Entry Date
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
                                        Participant
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
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
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-datagrid">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
                                        <%--<asp:Button ID="uiBtnBulkApproval" runat="server" CssClass="button_search" 
                                             Text="      Approve" onclick="uiBtnBulkApproval_Click" />--%>
                                        <asp:Button ID="uiBtnImport" CssClass="button_import" runat="server" Text="      Import" 
                                            OnClick ="uiBtnImport_Click" />
                                         <asp:Button ID="uiBtnDownload" CssClass="button_download" runat="server" 
                                Text="     Download" onclick="uiBtnDownload_Click" />
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
                    &nbsp;</td>                
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBankTransaction" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TransactionNo,ApprovalStatus"
                                OnRowDataBound="uiDgBankTransException_RowDataBound" AllowPaging="True" AllowSorting="True"
                                OnPageIndexChanging="uiDgBankTransException_PageIndexChanging" OnSorting="uiDgBankTransException_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                   <%-- <asp:BoundField DataField="ReceiveTime" HeaderText="Receive Time" DataFormatString="{0:dd-MMM-yyyy HH:mm}"
                                        ItemStyle-HorizontalAlign="Center" SortExpression="ReceiveTime">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/Images/edit.gif" 
                                                NavigateUrl='<%# Eval("TransactionNo", "~/FinanceAndAccounting/EntryBankTransaction.aspx?eType=edit&eID={0}") %>' 
                                                Text="edit"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TransactionNo" HeaderText="Transaction No" 
                                        ReadOnly="True" SortExpression="TransactionNo" Visible="False" />
                                    <asp:BoundField DataField="ReceiveTime" 
                                        DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderText="Entry Date"
                                        ItemStyle-HorizontalAlign="Center" SortExpression="ReceiveTime">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Participant" HeaderText="Participant" SortExpression="Participant"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccountNo" HeaderText="VA Number" SortExpression="AccountNo"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccountType" HeaderText="Account Type" 
                                        SortExpression="AccountType">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Mutation" HeaderText="Mutation" 
                                        ItemStyle-HorizontalAlign="Center" SortExpression="Mutation">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="ReferenceType" HeaderText="Reference Type" 
                                        ItemStyle-HorizontalAlign="Center" SortExpression="Mutation">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"
                                        DataFormatString="{0:#,##0.##}" SortExpression="Amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No Record</EmptyDataTemplate>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBankTransaction" runat="server" SelectMethod="GetBankTransactionTimeAppStatusCM"
                                TypeName="Bank" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <%--<asp:Parameter Name="receiveTime" Type="DateTime" />--%>
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUp1" Name="receiveTime" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="clearingMemberId"
                                        PropertyName="LookUpTextBoxID" Type="Decimal" />
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
        </table>
        <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
            
                <asp:Panel ID="Panel2" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                           <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote" 
                                ShowParameterPrompts="False" Width="100%">
                            </rsweb:ReportViewer>

                                            
                        </ContentTemplate>
                       
                    </asp:UpdatePanel>
                </asp:Panel>
                
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
