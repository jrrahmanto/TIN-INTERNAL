<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewDeliveryListing.aspx.cs" Inherits="ClearingAndSettlement_Tender_ViewDeliveryListing"
    Title="Untitled Page" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlContractCommodityLookup.ascx" TagName="CtlContractCommodityLookup"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        View Delivery Listing</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td>
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
                                    Tender Date
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="uiDtpTenderDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Clearing Member
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contract
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc3:CtlContractCommodityLookup ID="CtlContractCommodityLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    List By
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlListBy" runat="server">
                                        <asp:ListItem>Buyer</asp:ListItem>
                                        <asp:ListItem>Seller</asp:ListItem>
                                    </asp:DropDownList>
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
                                    <asp:Button ID="uiBtnViewReport" runat="server" CssClass="button_search" OnClick="uiBtnViewReport_Click"
                                        Text="    View" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <rsweb:ReportViewer ID="uiRptTenderReport" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    Height="400px" ProcessingMode="Remote" ShowCredentialPrompts="False" ShowFindControls="False"
                    ShowPageNavigationControls="False" ShowParameterPrompts="False" Width="100%">
                    <ServerReport ReportServerUrl="" />
                </rsweb:ReportViewer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
