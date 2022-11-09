<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewEventReceipientList.aspx.cs" Inherits="Administration_EventManagement_ViewEventReceipientList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel runat="server" DefaultButton="uiBtnSearch">
        <h1>View Event Recipient List</h1>
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
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
                                        Receipient Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtEventReceipientListName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        UserName
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtUserName" runat="server"></asp:TextBox>
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
                <td colspan="3">
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgEventReceipientList" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="EventRecipientListID"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgEventReceipientList_PageIndexChanging"
                                OnSorting="uiDgEventReceipientList_Sorting" PageSize="15" EmptyDataText="No record found.">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("EventRecipientListID", "~/Administration/EventManagement/EntryEventReceipientList.aspx?eType=edit&id={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EventRecipientName" HeaderText="Event Recipient Name"
                                        SortExpression="EventRecipientName" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventReceipientList" runat="server" TypeName="EventReceipientList"
                                SelectMethod="SelectEventReceipientListByEventReceipientListNameAndUserName">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtEventReceipientListName" Name="eventReceipientName"
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtUserName" Name="eventUserName" PropertyName="Text"
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
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
