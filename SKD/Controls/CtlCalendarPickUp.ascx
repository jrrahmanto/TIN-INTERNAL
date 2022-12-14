<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlCalendarPickUp.ascx.cs" Inherits="Controls_CtlCalendarPickUp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:TextBox ID="uiTxtCalendar" runat="server">
</asp:TextBox>
    <cc1:CalendarExtender ID="uiTxtCalendar_CalendarExtender" runat="server" 
    Enabled="True" TargetControlID="uiTxtCalendar" Format="dd-MMM-yyyy"
    PopupButtonID="uiImg">
</cc1:CalendarExtender>
<img id="uiImg" runat="server" alt="" src="~/Images/calendar.gif" />
&nbsp;<img id="uiImgEmpty" runat="server" alt="" src="~/Images/notcalendar.gif" />