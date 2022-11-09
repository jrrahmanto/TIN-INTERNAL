<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td style="border-style: solid; border-width: thin">
                 <table style="width: 100%;">
                   
                        <tr>
                            <td>
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
                        <tr>
                            <td>
                                <rsweb:reportviewer id="uiRptViewer1" runat="server" processingmode="Remote" showcredentialprompts="False"
                                    showparameterprompts="False" width="100%">
                                </rsweb:reportviewer>
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
                            <rsweb:reportviewer id="uiRptViewer2" runat="server" processingmode="Remote" showcredentialprompts="False"
                                showparameterprompts="False" width="100%">
                            </rsweb:reportviewer>
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
                            <asp:DropDownList ID="uiDdlRpt3" runat="server" Width="250px" OnSelectedIndexChanged="uiDdlRpt3_SelectedIndexChanged"
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
                            <rsweb:reportviewer id="uiRptViewer3" runat="server" processingmode="Remote" showcredentialprompts="False"
                                showparameterprompts="False" width="100%">
                            </rsweb:reportviewer>
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
                <rsweb:reportviewer id="uiRptViewer4" runat="server" processingmode="Remote" showcredentialprompts="False"
                    showparameterprompts="False" width="100%">
                </rsweb:reportviewer>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
</asp:Content>
