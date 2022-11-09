<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManagePostingGroup.aspx.cs" Inherits="WebUI_FinanceAndAccounting_ViewManagePostingGroup" %>

<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Manage Posting Group</h1>
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
                                    <td style="width:150px">
                                        Posting Group Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtPostingGroupCode" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Status
                                    </td>
                                    <td>
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
                    <asp:GridView ID="uiDgPostingGroup" runat="server" AutoGenerateColumns="False" Width="100%"
                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                        AllowSorting="True" PageSize="15">
                        <RowStyle CssClass="tblRowStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("PostingGroupID", "~/FinanceAndAccounting/EntryManagePostingGroup.aspx?eType=edit&eID={0}") %>'
                                        Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="PostingGroupCode" HeaderText="Posting Group" SortExpression="PostingGroupCode" />
                            <asp:BoundField DataField="EffectiveStartDate" 
                                HeaderText="Effective Start Date" ReadOnly="True"
                                SortExpression="EffectiveStartDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description">
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSourcePostingGroup" runat="server" SelectMethod="GetPostingGroupByGroupCodeAndApprovalStatus"
                        TypeName="Posting">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="uiTxtPostingGroupCode" Name="groupCode" PropertyName="Text"
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
