<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewEventLog.aspx.cs" Inherits="Administration_EventManagement_ViewEventLog" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Event Log</h1>
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
                                        Event Type Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlEventTypeName" runat="server" DataSourceID="ObjectDataSourceEventTypeName"
                                            DataTextField="Name" DataValueField="EventTypeID" Width="300px">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="ObjectDataSourceEventTypeName" runat="server" SelectMethod="getEventTypeName"
                                            TypeName="EventLog"></asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Event Time
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Delivery Status
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlDeliveryStatus" runat="server" Width="100px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Success</asp:ListItem>
                                            <asp:ListItem>Failed</asp:ListItem>
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
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgEventLog" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                                AllowSorting="True" OnPageIndexChanging="uiDgEventLog_PageIndexChanging" OnSorting="uiDgEventLog_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="EventTypeID,EventRecipientListID,EventTime"
                                        DataNavigateUrlFormatString="~/Administration/EventManagement/DetailEventLog.aspx?eventId={0}&recListId={1}&time={2:yyyy-MM-ddTHH_mm_ss.fff}"
                                        Text="Detail" HeaderText="Detail">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:HyperLinkField>
                                    <asp:BoundField DataField="EventTypeName" HeaderText="Event Type Name" SortExpression="EventTypeName"
                                        ReadOnly="True" />
                                    <asp:BoundField DataField="EventTime" HeaderText="Event Time" SortExpression="EventTime"
                                        DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EventRecipientName" HeaderText="Event Receipient Name"
                                        SortExpression="EventRecipientName" />
                                    <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" SortExpression="DeliveryStatus">
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventLog" runat="server" SelectMethod="SelectEventLogByIDTimeAndStatus"
                                TypeName="EventLog" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlEventTypeName" Name="eventTypeID" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="eventTime" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlDeliveryStatus" Name="deliveryStatus" PropertyName="SelectedValue"
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
    </asp:Panel>
</asp:Content>
