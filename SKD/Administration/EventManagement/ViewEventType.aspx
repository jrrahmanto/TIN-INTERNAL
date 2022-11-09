<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewEventType.aspx.cs" Inherits="Administration_EventManagement_ViewEventType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Event Type</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
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
                                        EventType Code
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtEventTypeCode" MaxLength="20" Width="170" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtName" MaxLength="100" Width="300" runat="server"></asp:TextBox>
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
                            <asp:GridView ID="uiDgEventType" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="EventTypeID"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgEventType_PageIndexChanging"
                                OnSorting="uiDgEventType_Sorting" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("EventTypeId", "~/Administration/EventManagement/EntryEventType.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EventTypeCode" HeaderText="Event Type Code" SortExpression="EventTypeCode"
                                        ReadOnly="True" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    <asp:BoundField DataField="SMSChannelDescription" HeaderText="SMS Channel" SortExpression="SMSChannelDescription" />
                                    <asp:BoundField DataField="EmailChannelDescription" HeaderText="Email Channel" SortExpression="EmailChannelDescription" />
                                    <asp:BoundField DataField="ApplicationChannelDescription" HeaderText="Application Channel"
                                        SortExpression="ApplicationChannelDescription" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventType" runat="server" SelectMethod="SelectEventTypeByEventTypeCodeAndName"
                                TypeName="EventType">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtEventTypeCode" Name="eventTypeCode" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtName" Name="eventTypeName" PropertyName="Text"
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
