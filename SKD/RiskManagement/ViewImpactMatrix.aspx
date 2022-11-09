<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewImpactMatrix.aspx.cs" Inherits="RiskManagement_ViewImpactMatrix" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <rsweb:ReportViewer ID="uiRptRiskProfileMatrix" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" Height="400px" ProcessingMode="Remote" 
        ShowCredentialPrompts="False" ShowFindControls="False" 
        ShowPageNavigationControls="False" ShowParameterPrompts="False" Width="100%">
        <ServerReport ReportServerUrl="" />
    </rsweb:ReportViewer>
</asp:Content>

