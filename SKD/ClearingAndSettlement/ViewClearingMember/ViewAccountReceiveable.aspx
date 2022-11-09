<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewAccountReceiveable.aspx.cs" Inherits="ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodity"  %>


<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1"%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Account Receivable</h1>
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
        <tr >
            <td>
                
            </td>
            <td>
                
            </td>
        
            
        </tr>
        <tr>
            <td>
            
                <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID ="uiLblFeeSeller" runat="server" Text="Seller Transaction Fee"></asp:Label>
                            <asp:Label ID ="uiLblFeeSellerValue" runat="server" Text="1000"></asp:Label>
                          <asp:GridView ID="uiDgvSellerFee" runat="server" AutoGenerateColumns="False" Width="100%"
                                DataKeyNames="ProgressID" MouseHoverRowHighlightEnabled="True" RowHighlightColor=""
                                AllowPaging="True" AllowSorting="True" >
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:BoundField DataField="SellerCode" HeaderText="Seller Code" 
                                        SortExpression="SellerCode" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Transaction Date" SortExpression="BusinessDate" 
                                                    DataFormatString ="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="Volume" HeaderText="Volume (TON)" 
                                        SortExpression="Volume" DataFormatString ="{0:#,###}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SellerFee" HeaderText="Amount" 
                                        SortExpression="SellerFee" DataFormatString ="{0:#,###}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerPaymentDue" HeaderText="Due Date (Seller-KBI)"
                                        SortExpression="BuyerPaymentDue" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerPaymentDate" HeaderText="Due Date (KBI-PKJ)"
                                        SortExpression="BuyerPaymentDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    
                                </Columns>
                              <EmptyDataTemplate>
                                  No Records Available for Seller
                              </EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID ="odsForcedClosedTrade" runat="server" SelectMethod="GetAccReceivableByBusinessDate"
                                TypeName="Inquery" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUpStartDate" Name="startBussDate" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUpEndDate" Name="endBussDate" PropertyName="Text" Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>                         
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
                
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                
            </td>
            <td>
               
            </td>
            
        </tr>
        <tr>
            <td>
            
                <asp:Panel ID="Panel2" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID ="uiLblBuyerFee" runat="server" Text="Buyer Transaction Fee" ></asp:Label>
                             <asp:Label ID ="uiLblBuyerFeeValue" runat="server" text ="1000"></asp:Label>
                          <asp:GridView ID="uiDgvBuyerFee" runat="server" AutoGenerateColumns="False" Width="100%"
                                DataKeyNames="ProgressID" MouseHoverRowHighlightEnabled="True" RowHighlightColor=""
                                AllowPaging="True" AllowSorting="True" >
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    
                                    <asp:BoundField DataField="BuyerCode" HeaderText="Buyer Code" 
                                        SortExpression="BuyerCode" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Transaction Date" SortExpression="BusinessDate" 
                                                    DataFormatString ="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="Volume" HeaderText="Volume (TON)" 
                                        SortExpression="Volume" DataFormatString ="{0:#,###}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerFee" HeaderText="Amount" 
                                        SortExpression="BuyerFee" DataFormatString ="{0:#,###}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerPaymentDue" HeaderText="Due Date (Buyer-KBI)"
                                        SortExpression="BuyerPaymentDue" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerPaymentDate" HeaderText="Due Date (KBI-PKJ)"
                                        SortExpression="BuyerPaymentDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    
                                </Columns>
                              <EmptyDataTemplate>
                                  No Records Available for Buyer
                              </EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID ="odsBuyerFee" runat="server" SelectMethod="GetAccReceivableByBusinessDate"
                                TypeName="Inquery" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUpStartDate" Name="startBussDate" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID ="CtlCalendarPickUpEndDate" Name="endBussDate" PropertyName="Text" Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>                         
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
                
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>

