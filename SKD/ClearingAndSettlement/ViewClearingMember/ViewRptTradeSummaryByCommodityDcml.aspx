<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptTradeSummaryByCommodityDcml.aspx.cs" Inherits="ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodityDcml"  %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1"%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Decimal Trade Summary</h1>
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
                        <td>
                            Start Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc4:CtlCalendarPickUp ID="CtlCalendarPickUpStartDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            End Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc4:CtlCalendarPickUp ID="CtlCalendarPickUpEndDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Type SPA</td>
                        <td>
                            :</td>
                        <td>
                            
                            <asp:DropDownList ID="uiDdlTypeSpa" runat="server">
                                <asp:ListItem>SPA - Reguler</asp:ListItem>
                                <asp:ListItem>SPA - Desimal</asp:ListItem>
                            </asp:DropDownList>
                            
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

