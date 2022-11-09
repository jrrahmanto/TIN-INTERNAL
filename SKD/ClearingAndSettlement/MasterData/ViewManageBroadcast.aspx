<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewManageBroadcast.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewManageBroadcast" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Manage Broadcast</h1>
    <table cellpadding="0" cellspacing="0" style="width:100%;">
         <tr >
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
                    <tr >
                        <td style="width:100px">
                            Event Time</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td >
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" 
                                Text="     Search" onclick="uiBtnSearch_Click" />
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
                <asp:gridview ID="uiDgEventLog" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="uiDgEventLog_PageIndexChanging" 
                    onsorting="uiDgEventLog_Sorting" EmptyDataText="No Record Found" >
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="EventTypeID,EventRecipientListID,EventTime" 
                            DataNavigateUrlFormatString="~/Administration/EventManagement/DetailEventLog.aspx?eventId={0}&recListId={1}&time={2:yyyy-MM-ddTHH_mm_ss.fff}&callerPage=B"
                            Text="Detail" />
                        <asp:BoundField DataField="EventTypeName" HeaderText="Event Type Name" 
                            SortExpression="EventTypeName" ReadOnly="True" />
                        <asp:BoundField DataField="EventTime" HeaderText="Event Time" 
                            SortExpression="EventTime" DataFormatString="{0:dd-MMM-yy HH:mm:ss}"/>
                        <asp:BoundField DataField="EventRecipientName" HeaderText="Event Receipient Name" 
                            SortExpression="EventRecipientName" />
                        <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" 
                            SortExpression="DeliveryStatus" />
                    </Columns>
                    <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:gridview>
                <asp:ObjectDataSource ID="ObjectDataSourceEventLog" runat="server" 
                    SelectMethod="SelectEventLogByNameTimeAndStatus" TypeName="EventLog" 
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Broadcast" Name="eventTypeName" Type="String" />
                        <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="eventTime" 
                            PropertyName="Text" Type="DateTime" />
                        <asp:Parameter DefaultValue="Success" Name="deliveryStatus" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

