<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewInbox.aspx.cs" Inherits="Home_ViewInbox" %>

<%@ Register Src="Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        View Inbox</h1>
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
                                <td>
                                    Received Date
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
                <asp:GridView ID="uiDgInbox" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" Width="100%" EmptyDataText="No Record found" 
                    OnSorting="uiDgInbox_Sorting" onpageindexchanging="uiDgInbox_PageIndexChanging">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:HyperLinkField DataNavigateUrlFields="EventTypeID,EventRecipientListID,EventTime"
                            DataNavigateUrlFormatString="~/DetailInbox.aspx?eventId={0}&recListId={1}&time={2:yyyy-MM-ddTHH_mm_ss.fff}"
                            Text="Detail" />
                         <asp:BoundField DataField="EventTypeName" HeaderText="Type" 
                            SortExpression="EventTypeName" ReadOnly="True" />
                        <asp:BoundField DataField="EventTime" HeaderText="Time" 
                            SortExpression="EventTime" DataFormatString="{0:dd-MMM-yy HH:mm:ss}"/>
                        <asp:BoundField DataField="EventRecipientName" HeaderText="Receipient" 
                            SortExpression="EventRecipientName" />
                        <asp:BoundField DataField="DeliveryStatus" HeaderText="Delivery Status" SortExpression="DeliveryStatus" />
                    </Columns>
                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsEventLog" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="SearchInbox" TypeName="EventLog" OnDataBinding="odsEventLog_DataBinding">
                    <SelectParameters>
                        <asp:SessionParameter Name="eventRecipientID" SessionField="EventRecipientListID"
                            Type="Decimal" />
                        <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="receivedDate" PropertyName="Text"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="uiFLDEventRecipientID" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
