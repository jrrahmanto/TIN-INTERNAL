<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewForceClosedTrade.aspx.cs" Inherits="ClearingAndSettlement_ViewClearingMember_ViewRptTradeSummaryByCommodity"  %>


<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1"%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View ForceClosed Trade</h1>
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
        <tr>
            <td>
            
                <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                          <asp:GridView ID="uiDgvForceClosedTrade" runat="server" AutoGenerateColumns="False" Width="100%"
                                DataKeyNames="ProgressID" MouseHoverRowHighlightEnabled="True" RowHighlightColor=""
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgvForceClosedTrade_PageIndexChanging">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate" 
                                                    DataFormatString ="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" 
                                        SortExpression="ExchangeRef" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SellerId" HeaderText="Seller Code" 
                                        SortExpression="SellerId" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerId" HeaderText="Buyer Code" 
                                        SortExpression="BuyerId" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" 
                                        SortExpression="ProductCode" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ContractMonth" HeaderText="Contract Month" 
                                        SortExpression="ContractMonth" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Volume" HeaderText="Volume" 
                                        SortExpression="Volume" DataFormatString ="{0:#,###}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Trade Amount" 
                                        SortExpression="Amount" DataFormatString ="{0:#,###}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ForcedClosed" HeaderText="Forced Closed Date"
                                        SortExpression="ForcedClosed" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ForcedClosedRef" HeaderText="Closing Ref"
                                        SortExpression="ForcedClosedRef"   >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ForcedClosedRemarks" HeaderText="Closing Remarks"
                                        SortExpression="ForcedClosedRemarks"  >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID ="odsForcedClosedTrade" runat="server" SelectMethod="GetForceClosedTradeByBusinessDate"
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
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

