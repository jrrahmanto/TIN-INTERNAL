<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewEventRecipient.aspx.cs" Inherits="Administration_EventManagement_ViewEventRecipient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Event Recipient</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
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
                                        EventTypeCode
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlEventTypeCode" runat="server" DataSourceID="ObjectDataSourceEventTypeCode"
                                            DataTextField="EventTypeCode" DataValueField="EventTypeCode">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="ObjectDataSourceEventTypeCode" runat="server" SelectMethod="getEventTypeCode"
                                            TypeName="EventRecipient"></asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Event Receipient Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtEventRecipientName" runat="server"></asp:TextBox>
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
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgEventTypeReceipient" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="EventTypeID,EventRecipientListID"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgEventReceipientList_PageIndexChanging"
                                OnSorting="uiDgEventReceipientList_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("EventTypeID", "~/Administration/EventManagement/EntryEventRecipient.aspx?typeID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EventTypeCode" HeaderText="Event Type Code" SortExpression="EventTypeCode" />
                                    <asp:BoundField DataField="EventRecipientName" HeaderText="Event Recipient Name"
                                        SortExpression="EventRecipientName" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventTypeReceipient" runat="server" TypeName="EventRecipient"
                                SelectMethod="SelectEventRecipientByEventTypeCodeAndEventRecipientName" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlEventTypeCode" Name="eventTypeCode" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtEventRecipientName" Name="eventRecipientName"
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
