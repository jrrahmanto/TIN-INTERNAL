<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptClearingMemberCollateral.aspx.cs" Inherits="ClearingAndSettlement_ReportEOD_ViewClearingMember_ViewClearingMemberCollateral"  %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Report Clearing Member Collateral</h1>
<asp:Panel ID="Panel1" runat="server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
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
                        <td style="width:150px">
                            Business Date</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc6:CtlCalendarPickUp ID="CtlCalendarPickUpBusinessDate" runat="server" />
                        </td>
                    </tr>
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
                        <td>
                            Lodgement Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc6:CtlCalendarPickUp ID="CtlCalendarPickUpLodgement" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Function</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlCollateralType" runat="server" Width="126px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="C">Coverage Margin</asp:ListItem>
                                <asp:ListItem Value="S">Security Deposit</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Type</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlLodgementType" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="D">Bank Deposit</asp:ListItem>
                                <asp:ListItem Value="S">Stock</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Issuer</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlIssuerType" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="B">Bank</asp:ListItem>
                                <asp:ListItem Value="N">Non Bank</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Issuer Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc6:CtlCalendarPickUp ID="CtlCalendarPickUpIssuer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Maturity Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc6:CtlCalendarPickUp ID="CtlCalendarPickUpMaturity" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_view" runat="server" 
                                Text="     View" onclick="uiBtnSearch_Click" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                          
                            
                         <rsweb:ReportViewer ID="uiRptViewer" runat="server" Width="100%" 
                            ProcessingMode="Remote" ShowParameterPrompts="False">
                        </rsweb:ReportViewer>
                          
                            
                          
                        </ContentTemplate>
                    </asp:UpdatePanel>
                
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>

