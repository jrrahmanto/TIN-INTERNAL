<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MainPage.aspx.cs" Inherits="MainPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td style="border-style: solid; border-width: thin">
                <table style="width: 100%;" __designer:mapid="54">
                    <tr __designer:mapid="55">
                        <td __designer:mapid="56">
                            <asp:DropDownList ID="uiDdlRpt1" runat="server" Width="250px" OnSelectedIndexChanged="uiDdlRpt1_SelectedIndexChanged"
                                AutoPostBack="True" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceRpt1" runat="server" SelectMethod="GetListReports"
                                TypeName="EOD">
                                <SelectParameters>
                                    <asp:SessionParameter DefaultValue="" Name="mapPath" SessionField="ListReportFile"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr __designer:mapid="5b">
                        <td __designer:mapid="5c">
                            <rsweb:ReportViewer ID="uiRptViewer1" runat="server" ProcessingMode="Remote" ShowCredentialPrompts="False"
                                ShowParameterPrompts="False" Width="100%" ShowToolBar="False">
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border-style: solid; border-width: thin">
                <table style="width: 100%;" __designer:mapid="54">
                    <tr>
                        <td>
                            <asp:DropDownList ID="uiDdlRpt2" runat="server" Width="250px" OnSelectedIndexChanged="uiDdlRpt2_SelectedIndexChanged"
                                AutoPostBack="True" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceRpt2" runat="server" SelectMethod="GetListReports"
                                TypeName="EOD">
                                <SelectParameters>
                                    <asp:SessionParameter DefaultValue="" Name="mapPath" SessionField="ListReportFile"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <rsweb:ReportViewer ID="uiRptViewer2" runat="server" ProcessingMode="Remote" ShowCredentialPrompts="False"
                                ShowParameterPrompts="False" Width="100%" ShowToolBar="False">
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="border-style: solid; border-width: thin">
                <table style="width: 100%;" __designer:mapid="54">
                    <tr>
                        <td>
                            <asp:DropDownList ID="uiDdlRpt3" runat="server"  Width="250px" OnSelectedIndexChanged="uiDdlRpt3_SelectedIndexChanged"
                                AutoPostBack="True" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceRpt3" runat="server" SelectMethod="GetListReports"
                                TypeName="EOD">
                                <SelectParameters>
                                    <asp:SessionParameter DefaultValue="" Name="mapPath" SessionField="ListReportFile"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <rsweb:ReportViewer ID="uiRptViewer3" runat="server" ProcessingMode="Remote" ShowCredentialPrompts="False"
                                ShowParameterPrompts="False" Width="100%" ShowToolBar="False">
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border-style: solid; border-width: thin">
                <table style="width: 100%;" __designer:mapid="54">
                    <td>
                        <asp:DropDownList ID="uiDdlRpt4" runat="server" Width="250px" OnSelectedIndexChanged="uiDdlRpt4_SelectedIndexChanged"
                            AutoPostBack="True" AppendDataBoundItems="True">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ObjectDataSourceRpt4" runat="server" SelectMethod="GetListReports"
                            TypeName="EOD">
                            <SelectParameters>
                                <asp:SessionParameter DefaultValue="" Name="mapPath" SessionField="ListReportFile"
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
        </tr>
        <tr>
            <td>
                <rsweb:ReportViewer ID="uiRptViewer4" runat="server" ProcessingMode="Remote" ShowCredentialPrompts="False"
                    ShowParameterPrompts="False" Width="100%" ShowToolBar="False">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
</asp:Content>
