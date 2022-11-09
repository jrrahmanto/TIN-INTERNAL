<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RptClearingMember.aspx.cs" Inherits="ClearingAndSettlement_ReportEOD_ClearingMemberReport_RptClearingMember" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../Controls/PdfViewer.ascx" TagName="PdfViewer" TagPrefix="uc3" %>
<%@ Register Src="../../Lookup/CtlBondIssuerLookup.ascx" TagName="CtlBondIssuerLookup" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Report Clearing Member</h1>
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
                                <td style="width: 100px">Business Date
                                </td>
                                <td style="width: 10px">:
                                </td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Clearing Member
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>CM Seller
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup2" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Font-Italic="True" Text="(filled in for notification purchaces)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Bond Issuer</td>
                                <td>:</td>
                                <td>
                                    <uc4:CtlBondIssuerLookup ID="CtlBondIssuer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Revision
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtRevision" runat="server"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td>Report
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlReport" runat="server">
                                    </asp:DropDownList>
                                    <asp:ObjectDataSource ID="ObjectDataSourceReport" runat="server" SelectMethod="GetListReports"
                                        TypeName="EOD">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="" Name="mapPath" SessionField="ListReportFile"
                                                Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" CssClass="button_download" runat="server" Text="     Download"
                                        OnClick="uiBtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" Visible="false">
                                            <ContentTemplate>
                                                <rsweb:reportviewer id="uiRptViewer" runat="server" processingmode="Remote"
                                                    showparameterprompts="False" width="100%">
                                            </rsweb:reportviewer>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
