<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptCashSettlement.aspx.cs" Inherits="ClearingAndSettlement_ViewClearingMember_ViewRptCashSettlement"  %>


<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1"%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>


<%@ Register src="../../Controls/CtlMonthYear.ascx" tagname="CtlMonthYear" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 243px;
        }
        .style2
        {
            width: 143px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Report Cash Settlement</h1>
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
                        <td class="style2">
                            Period</td>
                        <td>
                            :</td>
                        <td>
                            <uc2:CtlMonthYear ID="CtlMonthYear1" runat="server" />
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" 
                                Text="     Search" onclick="uiBtnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Cash Settlement</td>
                        <td>
                        </td>
                        <td>
                                <asp:DropDownList ID="uiDdlCashSettlement" runat="server" 
                                DataSourceID="" DataTextField="BusinessDateDesc" 
                                DataValueField="BusinessDate">
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                                &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="uiBtnView" CssClass="button_view" runat="server" 
                                Text="     View" onclick="uiBtnView_Click" />
                            <asp:ObjectDataSource ID="odsCashSettlement" runat="server" 
                                OldValuesParameterFormatString="original_{0}" 
                                SelectMethod="GetCashSettlementData" 
                                TypeName="ReportDataTableAdapters.uspCashSettlementDateTableAdapter">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlMonthYear1" Name="monthDate" 
                                        PropertyName="Month" Type="Int32" />
                                    <asp:ControlParameter ControlID="CtlMonthYear1" Name="yearDate" 
                                        PropertyName="Year" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
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

