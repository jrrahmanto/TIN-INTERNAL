<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BulkApproveBankTransaction.aspx.cs" Inherits="FinanceAndAccounting_BulkApproveBankTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <script language="javascript" type="text/javascript">
    function SelectAll(CheckBoxControl, ctlName) {
        if (CheckBoxControl.checked == true) {
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgBankTransaction') > -1)) {
                    var test = document.forms[0].elements[i].name;
                    if (test.indexOf(ctlName) != -1) {
                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
        }
        else {
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgBankTransaction') > -1)) {
                    var test = document.forms[0].elements[i].name;
                    if (test.indexOf(ctlName) != -1) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
    }
    </script> <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>Bulk Approval Bank Transaction</h1>
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
                                        Clearing Member
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
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-datagrid">
                                <tr>
                                    <td align="center">
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
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBankTransaction" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TransactionNo,ApprovalStatus"
                                OnRowDataBound="uiDgBankTransException_RowDataBound" AllowPaging="True" AllowSorting="True"
                                OnPageIndexChanging="uiDgBankTransException_PageIndexChanging" 
                                OnSorting="uiDgBankTransException_Sorting" PageSize="3">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="uiChkList" runat="server" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <input id="uiChkAll" onclick="SelectAll(this,'uiChkList')" type="checkbox" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TransactionNo" HeaderText="TransactionNo" ReadOnly="True"
                                        SortExpression="TransactionNo" Visible="False" />
                                    <asp:BoundField DataField="TransactionTime" HeaderText="Transaction Time" SortExpression="TransactionTime"
                                        DataFormatString="{0:dd-MMM-yyyy HH:mm}" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                   <%-- <asp:BoundField DataField="ReceiveTime" HeaderText="Receive Time" DataFormatString="{0:dd-MMM-yyyy HH:mm}"
                                        ItemStyle-HorizontalAlign="Center" SortExpression="ReceiveTime">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="TransactionTypeDesc" HeaderText="Transaction Type" SortExpression="TransactionTypeDesc"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Source Acct ID" SortExpression="SourceAcctID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblSourceAccount" runat="server" Text='<%# Bind("SourceAcctID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SourceAcctID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AccountType" HeaderText="Account Type" 
                                        SortExpression="AccountType">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SourceCMName" HeaderText="Source Clearing Member" 
                                        ItemStyle-HorizontalAlign="Left" SortExpression="SourceCMName">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DestCMName" HeaderText="Destination Clearing Member" 
                                        ItemStyle-HorizontalAlign="Left" SortExpression="DestCMName">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" 
                                        SortExpression="CurrencyCode" />
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
                                    <asp:TemplateField HeaderText="Approval Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="uiTxtApprovalDesc" runat="server" Width="250"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBankTransaction" runat="server" SelectMethod="GetBankTransactionByTransTimeApprovalStatusClearingMember"
                                TypeName="Bank" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="transactionTime" PropertyName="Text"
                                        Type="DateTime" />
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
            <tr>
                <td>
                    <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                        onclick="uiBtnApprove_Click" Text="     Approve" />
                    <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                        onclick="uiBtnReject_Click" Text="     Reject" />
                    <asp:Button ID="uiBtnCancel" runat="server" CausesValidation="False" 
                        CssClass="button_cancel" onclick="uiBtnCancel_Click" Text="      Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
