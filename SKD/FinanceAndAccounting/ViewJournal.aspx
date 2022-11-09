<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="ViewJournal.aspx.cs" Inherits="FinanceAndAccounting_ViewJournal" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../Lookup/CtlContractLookup.ascx" TagName="CtlContractLookup" TagPrefix="uc1" %>
<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc2" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc3" %>
<%@ Register Src="../Lookup/CtlContractCommodityLookup.ascx" TagName="CtlContractCommodityLookup" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style3 {
            height: 85px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Export Journal</h1>
    <form id="form2">

        <table border="0" cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red"></asp:BulletedList>
                </td>
            </tr>
            <td>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table border="0" class="table-row">
                                <tr>
                                    <td style="width: 10px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Transaction Date</td>
                                    <td>:</td>
                                    <td>
                                        <uc3:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="uiBtnDownload" runat="server" Text="     Search" CssClass="button_search" OnClick="uiBtnDownload_Click" />&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote"
                                    ShowParameterPrompts="False" Width="100%">
                                </rsweb:ReportViewer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</asp:Content>
