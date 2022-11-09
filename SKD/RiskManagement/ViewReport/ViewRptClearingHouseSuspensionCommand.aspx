<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptClearingHouseSuspensionCommand.aspx.cs" Inherits="RiskManagement_ViewReport_ViewRptClearingHouseSuspensionCommand"  %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Clearing House Suspension Command</h1>
        <table cellpadding="1" cellspacing="1" style="width:100%;">
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
                                    <td style="width:150px">
                                        Bussines Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc3:CtlCalendarPickUp ID="CtlCalendarPeriod" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Suspension
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlSuspension" runat="server" DataTextField="Suspension"
                                            DataValueField="Suspension">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="L">Limit</asp:ListItem>
                                            <asp:ListItem Value="S">Suspension</asp:ListItem>
                                            <asp:ListItem Value="R">Release</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="uiBtnView" CssClass="button_view" runat="server" 
                                        Text="     View" onclick="uiBtnView_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote" 
                                    ShowParameterPrompts="False" Width="100%">
                                </rsweb:ReportViewer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
</asp:Content>


