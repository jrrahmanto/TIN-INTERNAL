<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptWithHoldingTax.aspx.cs" Inherits="FinanceAndAccounting_ViewReport_ViewRptWithHoldingTax"  %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcTextBox" tagprefix="cc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Report With Holding Tax</h1>
   <table cellpadding="1" cellspacing="1" style="width:100%;">
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
                        <td style="width:120px">
                            &nbsp;Clearing Member</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Year</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtYear"  MaxLength="4" runat="server" Width="70px"></asp:textbox>
                            <cc2:FilteredTextBoxExtender ID="uiTxtYear_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="uiTxtYear">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" 
                                Text="     Search" onclick="uiBtnSearch_Click" />
                        </td>
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
        </table>
</asp:Content>

