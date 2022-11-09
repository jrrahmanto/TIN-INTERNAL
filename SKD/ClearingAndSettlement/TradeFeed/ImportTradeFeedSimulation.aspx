<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportTradeFeedSimulation.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ImportTradeFeedSimulation" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Import Trade Register For Closing Position Simulation</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td  colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                            Clearing Member</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100px">
                            Exchange</td>
                        <td style="width:10px">
                        :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlExchange" CssClass="required" runat="server" 
                            DataSourceID="ObjectDataSourceExchangeDll" DataTextField="ExchangeCode" 
                            DataValueField="ExchangeId">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceExchangeDll" runat="server" 
                            SelectMethod="SelectExchange" TypeName="Exchange"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bussiness Date</td>
                        <td>
                            :</td>
                        <td>
                            <%--<uc1:CtlCalendarPickUp ID="uiDtpBussDate" runat="server" />--%>
                            <asp:TextBox ID="uiDtBussDate" runat="server" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            File Name</td>
                        <td>
                        :</td>
                        <td>
                            <asp:FileUpload ID="uiUploadFile" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_import" 
                            onclick="uiBtnUpload_Click" Text="     Upload" />&nbsp;
                             <asp:Button ID="uiBtnSimulate" runat="server" CssClass="button_request" 
                            onclick="uiBtnSimulate_Click" Text="   Simulate" />&nbsp;
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                                Text="     Cancel" onclick="uiBtnCancel_Click" />&nbsp;
                            <asp:Button ID="uiBtnClean" runat="server" CssClass="button_delete" 
                                Text="     Clean" onclick="uiBtnClean_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="uiTxbLog" runat="server" Height="69px" TextMode="MultiLine" 
                                Width="780px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdateProgress runat="server" DisplayAfter="0" ID="UpdateProgress1">
                                <ProgressTemplate>
                                    <img alt="" src="../../Images/ajax-loader2.gif"
                                            style="width: 220px; height: 19px" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            &nbsp;</td>
                    </tr>
                </table>
                </div>
                </div>
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

