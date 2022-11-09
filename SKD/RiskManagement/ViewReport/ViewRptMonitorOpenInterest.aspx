<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptMonitorOpenInterest.aspx.cs" Inherits="RiskManagement_ViewReport_MonitorOpenInterest" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc1" %>
<%@ Register src="../../Controls/CtlMonthYear.ascx" tagname="CtlMonthYear" tagprefix="uc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Report Monitor Open Interest</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr >
                        <td style="width:150px">
                            Exchange</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlExchange" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="ObjectDataSourceExchange" DataTextField="ExchangeCode" 
                                DataValueField="ExchangeId" Height="17px" Width="129px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceExchange" runat="server" 
                                SelectMethod="GetExchanges" TypeName="Exchange"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr >
                        <td >
                            Currency</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlCurrency" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="ObjectDataSourceCurrency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyID" Height="16px" Width="130px">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceCurrency" runat="server" 
                                SelectMethod="GetCurrency" TypeName="Currency"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr >
                        <td >
                            Contract</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlContractCommodityLookup ID="CtlContractCommodityLookup1" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr >
                        <td >
                            Business Date From</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:CtlCalendarPickUp ID="CtlCalendarPickUpBusinessDateFrom" runat="server" />
                        </td>
                    </tr>
                    <tr >
                        <td >
                            Business Date To</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:CtlCalendarPickUp ID="CtlCalendarPickUpBusinessDateTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
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

