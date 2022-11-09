<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewRole.aspx.cs" Inherits="WebUI_New_ViewRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Group</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:100px">
                                        Group
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        &nbsp;<asp:TextBox ID="uiTxtGroup" runat="server" Width="500px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
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
                    <asp:Button ID="Button1" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDGGroup" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" 
                                DataKeyNames="GroupID" AllowPaging="True" 
                                onpageindexchanging="uiDGGroup_PageIndexChanging" 
                                onsorting="uiDGGroup_Sorting" AllowSorting="True">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("GroupID", "~/Administration/UserManagement/EntryRole.aspx?id={0}") %>'
                                                Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="GroupName" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                </Columns>
                                <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                OldValuesParameterFormatString="original_{0}" SelectMethod="FillByGroupName"
                                TypeName="Group">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtGroup" Name="groupName" 
                                        PropertyName="Text" Type="String" />
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
    </asp:Panel>
</asp:Content>
