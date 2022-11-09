<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryTenderApproval.aspx.cs" Inherits="ClearingAndSettlement_Tender_EntryTenderApproval" %>

<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc5" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc6" %>
<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc7" %>

<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Tender Approval</h1>
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
                        <td style="width:100px">
                            Contract<asp:ObjectDataSource ID="ObjectDataSourceTender" 
                                runat="server" SelectMethod="GetTenderByTenderId" 
                                TypeName="Tender">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="tenderId" QueryStringField="eID" 
                                        Type="Decimal" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlContractCommodityLookup ID="CtlContractCommodityLookup1" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Seller Account</td>
                        <td>
                            :</td>
                        <td>
                            <uc6:CtlInvestorLookup ID="CtlInvestorLookupSeller" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Delivery Location</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtDeliveryLocation" runat="server" Height="94px" 
                                TextMode="MultiLine" Width="329px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Approval Description </td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="94px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                
                <table id="tblSearch" runat="server" class="table-row">
                    <tr>
                        <td style="width:100px">
                            Commodity</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc7:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Account</td>
                        <td>
                            :</td>
                        <td>
                            <uc6:CtlInvestorLookup ID="CtlInvestorLookup1" runat="server" />
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
      
        </table>
        <table class="table-datagrid">
        <tr>
            <td>
                <asp:GridView ID="uiDgContractPosition" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" 
                    onpageindexchanging="uiDgContractPosition_PageIndexChanging" 
                    onrowdatabound="uiDgContractPosition_RowDataBound" 
                    onsorting="uiDgContractPosition_Sorting" AllowPaging="True" 
                    AllowSorting="True" PageSize="15" 
                    onselectedindexchanging="uiDgContractPosition_SelectedIndexChanging">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField SelectText="Add" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Account" SortExpression="InvestorID">
                            <ItemTemplate>
                                <asp:Label ID="uiLblInvestorID" Visible="false" runat="server" Text='<%# Bind("InvestorID") %>'></asp:Label>
                                <asp:Label ID="uiLblInvestor" runat="server" Text='<%# Bind("InvestorID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("InvestorID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Commodity" SortExpression="CommodityID">
                            <ItemTemplate>
                                <asp:Label ID="uiLblCommodity" runat="server" Text='<%# Bind("CommodityID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CommodityID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Spot" SortExpression="StartSpot">
                            <ItemTemplate>
                                <asp:Label ID="uiLblStartSpot" runat="server" Text='<%# Bind("StartSpot") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("StartSpot") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade Position" SortExpression="TradePosition">
                            <ItemTemplate>
                                <asp:Label ID="uiLblTradePosition" runat="server" Text='<%# Bind("TradePosition") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("TradePosition") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Position" SortExpression="Position">
                            <ItemTemplate>
                                <asp:Label ID="uiLblPosition" runat="server" Text='<%# Bind("Position") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Position") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" SortExpression="OpenPosition">
                            <ItemTemplate>
                                <asp:Label ID="uiLblOpenPosition" runat="server" Text='<%# Bind("OpenPosition") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OpenPosition") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price" SortExpression="Price">
                            <ItemTemplate>
                                <asp:Label ID="uiLblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ContractID" SortExpression="ContractID" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="uiLblContractID" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="uiTxtPosition" runat="server" Text=''></asp:TextBox>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceTransferContractPosition" runat="server" 
                    SelectMethod="GetTransferContractPositionByBusinessDateInvestorCommodity" 
                    TypeName="Transfer" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:SessionParameter Name="businessDate" SessionField="BusinessDate" 
                            Type="DateTime" />
                        <asp:ControlParameter ControlID="CtlInvestorLookup1" Name="investorId" 
                            PropertyName="LookupTextBoxID" Type="Decimal" />
                        <asp:ControlParameter ControlID="CtlCommodityLookup1" Name="commodityId" 
                            PropertyName="LookupTextBoxID" Type="Decimal" />
                        <asp:SessionParameter Name="clearingMemberId" SessionField="ClearingMemberID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td id="tdImage" runat="server">
                <asp:GridView ID="uiDgTenderRequest" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" PageSize="15" 
                    
                    DataKeyNames="Price,TradePosition,TenderID" 
                    onrowdatabound="uiDgTenderRequest_RowDataBound" 
                    onselectedindexchanging="uiDgTenderRequest_SelectedIndexChanging">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField SelectText="Remove" ShowSelectButton="True" />
                        <asp:BoundField DataField="Price" HeaderText="Price" 
                            SortExpression="Price" ReadOnly="True" />
                        <asp:BoundField DataField="TradePosition" HeaderText="TradePosition" ReadOnly="True" 
                            SortExpression="TradePosition" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" 
                            SortExpression="Quantity" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceTenderRequest" runat="server" 
                    SelectMethod="GetTenderRequestByTenderId" TypeName="Tender">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="tenderId" QueryStringField="eID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                    Text="     Approve" onclick="uiBtnApprove_Click" />
                <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" 
                    onclick="uiBtnReject_Click" />
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                    onclick="uiBtnCancel_Click" Text="      Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>

