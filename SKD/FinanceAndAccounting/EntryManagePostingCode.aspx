<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryManagePostingCode.aspx.cs" Inherits="WebUI_FinanceAndAccounting_EntryManagePostingCode" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Posting Code</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                                    Account Code
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtCode" CssClass="required" MaxLength="20" runat="server"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Ledger Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlLedgerType" CssClass="required" runat="server">
                                        <asp:ListItem Value="H">Clearing House</asp:ListItem>
                                        <asp:ListItem Value="M">Clearing Member</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Account Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlAccountType" CssClass="required" runat="server">
                                        <asp:ListItem Value="A">Asset</asp:ListItem>
                                        <asp:ListItem Value="L">Liabilities</asp:ListItem>
                                        <asp:ListItem Value="E">Equity</asp:ListItem>
                                        <asp:ListItem Value="R">Revenue</asp:ListItem>
                                        <asp:ListItem Value="E">Expense</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Balance Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlBalance" CssClass="required" runat="server">
                                        <asp:ListItem Value="D">Debit</asp:ListItem>
                                        <asp:ListItem Value="C">Credit</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Display In Balance Sheet
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:CheckBox ID="uiChkDisplay" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Seq Display DFS
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtSeq" CssClass="number" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="uiTxtSeq_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="uiTxtSeq">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="style3">
                                    Description
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtDescription" runat="server" MaxLength="500" Height="59px" TextMode="MultiLine"
                                        Width="269px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trAction" runat="server">
                                <td class="style3">
                                    Action
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trApprovalDesc" runat="server">
                                <td>
                                    Approval Description
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtApprovalDescription" CssClass="required" runat="server" Height="100px"
                                        Width="400px" TextMode="MultiLine"></asp:TextBox>
                                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ControlToValidate="uiTxtApprovalDescription" ErrorMessage="Max. 100 Character"
                                        ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                        OnClick="uiBtnSave_Click" />
                                    <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" Text="      Delete"
                                        OnClick="uiBtnDelete_Click" CausesValidation="False" />
                                    <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server"
                                        ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve"
                                        OnClick="uiBtnApprove_Click" CausesValidation="False" />
                                    <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject"
                                        OnClick="uiBtnReject_Click" CausesValidation="False" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                        OnClick="uiBtnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
