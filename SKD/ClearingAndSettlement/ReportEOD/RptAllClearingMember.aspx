<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RptAllClearingMember.aspx.cs" Inherits="ClearingAndSettlement_ReportEOD_ClearingMemberReport_RptAllClearingMember" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../Controls/PdfViewer.ascx" TagName="PdfViewer" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Report All Clearing Member</h1>
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
                                <td style="width:100px">
                                    Business Date
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Revision
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtRevision" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" Font-Italic="True" Text="(Leave blank to get latest revision)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Report
                                </td>
                                <td>
                                    :
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
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" CssClass="button_download" runat="server" Text="     Download"
                                        OnClick="uiBtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
