<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewWithDrawal.aspx.cs" Inherits="ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodity"  %>


<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1"%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Withdrawal Seller</h1>
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
                            Business Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc4:CtlCalendarPickUp ID="CtlCalendarPickUpStartDate" runat="server" />
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
                             <asp:Button ID="uiBtnDownload" CssClass="button_download" runat="server" 
                                Text="     Download" onclick="uiBtnDownload_Click" />
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
                          <asp:GridView ID="uiDgvWithDrawal" runat="server" AutoGenerateColumns="False" Width="100%" OnDataBound="uiDgvWithDrawal_DataBound"
                                DataKeyNames="AccountNo" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" OnRowCreated="uiDgvWithDrawal_RowCreated"
                                AllowPaging="True" ShowHeader="true" EmptyDataText="No Records Found" AllowSorting="True" OnPageIndexChanging="uiDgvWithDrawal_PageIndexChanging">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    
                                    <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="TransactionDate" 
                                                    DataFormatString ="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="Participant" HeaderText="Participant" 
                                        SortExpression="code" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SellerName" HeaderText="Seller Name" 
                                        SortExpression="SellerName" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" 
                                        SortExpression="BankName" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccountNo" HeaderText="AccountNo" 
                                        SortExpression="AccountNo" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" 
                                        SortExpression="ExchangeRef" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SellerAccount" HeaderText="SellerAccount" 
                                        SortExpression="SellerAccount" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" 
                                        SortExpression="Amount" DataFormatString ="{0:#,###}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                                                        
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID ="odsWithdrawal" runat="server" SelectMethod="GetWithdrawalByBusinessDate"
                                TypeName="WithDrawal" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUpStartDate" Name="businessDate" PropertyName="Text" Type="DateTime" />
                                    
                                </SelectParameters>
                            </asp:ObjectDataSource>                         
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
                
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
            
                <asp:Panel ID="Panel2" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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

