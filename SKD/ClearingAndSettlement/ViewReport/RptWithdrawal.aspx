<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RptWithdrawal.aspx.cs" Inherits="ClearingAndSettlement_ViewReport_RptWithdrawal" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Report Withdrawal</h1>
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
                                <td>Business Date</td>
                                <td>:</td>
                                <td>
                                    <uc4:CtlCalendarPickUp ID="CtlCalendarPickUpBusinessDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Exchange Reff</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="uiExchReff" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Seller</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="uiSeller" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" CssClass="button_view" runat="server" Text="     View" OnClick="uiBtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td></td>
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
            <td></td>
        </tr>
    </table>
</asp:Content>
