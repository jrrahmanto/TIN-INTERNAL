<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewIntradayMargin.aspx.cs" Inherits="RiskManagement_ViewIntradayMargin" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="shadow_view">
        <div class="box_view">
            <table class="table-row">
                <tr>
                    <td style="width:150px">
                        Business Date
                    </td>
                    <td style="width:10px">
                        :
                    </td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="uiDtBusinessDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button CssClass="button_search" ID="uiBtnSearch" runat="server" 
                            Text="    Search" onclick="uiBtnSearch_Click"></asp:Button>
                    </td>
                </tr>
                </table>
        </div>
    </div>
    <rsweb:ReportViewer ID="uiRptIntradayMargin" runat="server" Font-Names="Verdana"
        Font-Size="8pt" Height="400px" ProcessingMode="Remote" 
        ShowCredentialPrompts="False" ShowParameterPrompts="False"
        Width="100%">
        <ServerReport ReportServerUrl="" />
    </rsweb:ReportViewer>
</asp:Content>
