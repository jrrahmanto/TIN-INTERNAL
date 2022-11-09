<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryManagePostingGroup.aspx.cs" Inherits="WebUI_FinanceAndAccounting_EntryManagePostingGroup" %>

<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../Lookup/CtlPostingCodeLookup.ascx" TagName="CtlPostingCodeLookup"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Posting Group</h1>
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
                                    Group Code
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtGroupCode" CssClass="required" MaxLength="20" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
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
                                <td>
                                    Effective Start Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Effective End Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveEndDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Description
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtDescription" runat="server" Height="53px" TextMode="MultiLine"
                                        MaxLength="500" Width="253px"></asp:TextBox>
                                </td>
                            </tr>
                             <tr id="trAction" runat="server">
                                <td>
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
                                    <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" MaxLength="100"
                                        Height="100px" Width="400px" TextMode="MultiLine"></asp:TextBox>
                                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ControlToValidate="uiTxtApprovalDesc" ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
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
                                    <asp:GridView ID="uiDgPostingGroupAccount" runat="server" AutoGenerateColumns="False"
                                        Width="620" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="PostingGroupID,DrCrType,AccountID"
                                        PageSize="15" OnRowCancelingEdit="uiDgPostingGroupAccount_RowCancelingEdit" OnRowDeleting="uiDgRiskProfile_RowDeleting"
                                        OnRowEditing="uiDgPostingGroupAccount_RowEditing" OnRowUpdating="uiDgPostingGroupAccount_RowUpdating">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                            <asp:BoundField DataField="PostingGroupID" SortExpression="PostingGroupID" Visible="False" />
                                            <asp:TemplateField HeaderText="Dr Cr Type" SortExpression="DrCrType">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="uiDdlDrCr" runat="server" AppendDataBoundItems="True" DataTextField="Posting"
                                                        DataValueField="PostingGroupID" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "DrCrType") %>'>
                                                        <asp:ListItem Text="" Value="0"> </asp:ListItem>
                                                        <asp:ListItem Text="" Value="D">Debet</asp:ListItem>
                                                        <asp:ListItem Text="" Value="C">Credit</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourcePostingGroup" runat="server" SelectMethod="GetDrCr"
                                                     TypeName="Posting"></asp:ObjectDataSource>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDrCr" runat="server" Text='<%# Bind("DrCrTypeDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Account" SortExpression="AccountID">
                                                <EditItemTemplate>
                                                    <uc3:CtlPostingCodeLookup ID="CtlPostingCodeLookup" runat="server" />
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="uiLblPostingCode" runat="server" Text='<%# Posting.GetPostingAccountCodeByAccountID(decimal.Parse(DataBinder.Eval(Container.DataItem, "AccountID").ToString())) %>'></asp:Label>
                                                    <asp:Label ID="uiLblPostingCodeID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AccountID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="tblHeaderStyle" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourcePostingGroupAccount" runat="server" SelectMethod="GetPostingGroupAccountByPostingGroupId"
                                        TypeName="Posting">
                                        <SelectParameters>
                                            <asp:QueryStringParameter DefaultValue="" Name="postingGroupId" QueryStringField="eID"
                                                Type="Decimal" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
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
                                    <asp:LinkButton ID="uiBtnAdd" runat="server" OnClick="uiBtnAdd_Click">Add New</asp:LinkButton>
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
                                        OnClick="uiBtnDelete_Click" />
                                    <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server"
                                        ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve"
                                        OnClick="uiBtnApprove_Click" />
                                    <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject"
                                        OnClick="uiBtnReject_Click" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                        OnClick="uiBtnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
