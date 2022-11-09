<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewSettlementPrice.aspx.cs" Inherits="WebUI_New_ViewSettlementPrice" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../../LookUp/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc2" %>

<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Settlement Price</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    
     <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
    
        <tr>
            <td>  <div class="shadow_view">
            <div class="box_view">
               <table class="table-row">
                   
                  
                    <tr >
                        <td style="width:100px">
                            Business Date</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            
                            <uc1:CtlCalendarPickUp ID="CtlCalendarBusinessDate" runat="server" />
                        </td>
                    </tr>
                    <tr >
                        <td>
                            Status</td>
                        <td>
                            :</td>
                        <td>
                            
                            <asp:DropDownList ID="uiDdlAction" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="A">Approved</asp:ListItem>
                                <asp:ListItem Value="P">Proposed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr >
                        <td style="width:100px">
                            Commodity</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            
                            <uc2:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr >
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search"
                                Text="     Search" onclick="uiBtnSearch_Click" />
                            <asp:Button ID="uiBtnEdit" runat="server" CssClass="button_edit" onclick="uiBtnEdit_Click" 
                                Text="     Edit" />
                            <asp:Button ID="uiBtnViewTransaction" CssClass="button_search" runat="server" 
                                Text="     View" onclick="uiBtnViewTransaction_Click" />
                            <asp:Button ID="uiBtnPrint" runat="server" onclick="uiBtnPrint_Click" 
                                Text="    Print" CssClass="button_print" />
                        </td>
                    </tr>
                    </table>
                    <table class="table-datagrid">
                      <tr >
                        <td colspan="3">
                            <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" 
                                Text="     Create" onclick="uiBtnCreate_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                        <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                <asp:GridView ID="uiDgSettlementPrice" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataKeyNames="SettleID,ContractID" EmptyDataText="No Records"
                                AllowPaging="True" 
                                onpageindexchanging="uiDgSettlementPrice_PageIndexChanging" 
                                onsorting="uiDgSettlementPrice_Sorting" AllowSorting="True" PageSize="50">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="BusinessDate" HeaderText="BusinessDate" 
                            SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}" 
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="CommName" HeaderText="Contract"  SortExpression="CommName" Visible="false"/>
                        <asp:BoundField DataField="CommodityCode" HeaderText="Code"  SortExpression="CommodityCode" />
                        <asp:BoundField DataField="ContractYearMonth" HeaderText="Contract Month"  SortExpression="ContractYearMonth" />
                        <asp:BoundField DataField="SettlementPrice" HeaderText="Settlement Price"  ItemStyle-HorizontalAlign="Right"
                            SortExpression="SettlementPrice" DataFormatString="{0:#,##0.##########}" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="SettlementPriceTypeDesc" 
                            HeaderText="Settlement Price Type" 
                            SortExpression="SettlementPriceTypeDesc" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ActionDesc" HeaderText="Action" 
                            SortExpression="ActionDesc" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceSettlementPrice" runat="server" 
                                SelectMethod="GetSettlementByBusinessDateApprovalComm" TypeName="SettlementPrice" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarBusinessDate" Name="bussDate" 
                                        PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlAction" Name="approval" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="CtlCommodityLookup1" Name="commodityId"
                                        PropertyName="LookupTextBoxID" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                                </Triggers>
                            </cc1:ProgressUpdatePanel>
                        </td>
                    </tr>
                    </table>
                </div>
                </div>
            </td>
        </tr>
        </table>
</asp:Content>



