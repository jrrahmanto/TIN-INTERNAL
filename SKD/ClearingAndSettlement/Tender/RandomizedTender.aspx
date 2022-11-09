<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RandomizedTender.aspx.cs" Inherits="WebUI_New_RandomizedTender" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc3" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>


<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <h1>Randomized Tender</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
 <tr >
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                            Business Date</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
            <asp:Button ID="uiBtnRunRandom" runat="server" CssClass="button_rerunrandom" 
                text="   Run Randomize" onclick="uiBtnRerun_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tender Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contract</td>
                        <td>
                            :</td>
                        <td>
                            <uc4:CtlContractCommodityLookup ID="CtlContractLookup1" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Account</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:ctlinvestorlookup ID="CtlInvestorLookup1" runat="server" />
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
                <asp:GridView ID="uiDgTender" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataKeyNames="TenderNo,ApprovalStatus" 
                    onpageindexchanging="uiDgTender_PageIndexChanging" 
                    onrowdatabound="uiDgTender_RowDataBound" 
                    onsorting="uiDgTender_Sorting" AllowPaging="True" 
                    AllowSorting="True" PageSize="15">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField>
                             <ItemTemplate>
                                <a href="RandomizedTenderDetail.aspx?tenderId=<%# DataBinder.Eval(Container.DataItem, "TenderID")  %>&tenderDate=<%# DataBinder.Eval(Container.DataItem, "TenderDate") %>">Detail
                                 </a>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="TenderID" InsertVisible="False" 
                            SortExpression="TenderID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("TenderID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("TenderID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tender No" DataField="TenderNo" 
                            SortExpression="TenderNo" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField DataField="TenderDate" HeaderText="Tender Date" 
                            SortExpression="TenderDate" DataFormatString="{0:dd-MMM-yyyy}" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="ContractID" SortExpression="ContractID" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="uiLblContractId" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seller Account" SortExpression="SellerInvID">
                            <ItemTemplate>
                                <asp:Label ID="uiLblSellerInvId" runat="server" Text='<%# Bind("SellerInvID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SellerInvID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DeliveryLocation" 
                            HeaderText="Delivery Location" SortExpression="DeliveryLocation" />
                        <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                            <ItemTemplate>
                                <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                </td>
        </tr>
        <tr>
            <td>
                <asp:ObjectDataSource ID="ObjectDataSourceTender" runat="server" 
                    SelectMethod="GetTenderByDateContractInvApproval" 
                    TypeName="Tender" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="CtlCalendarPickUp2" Name="tenderDate" 
                            Type="DateTime" PropertyName="Text" />
                        <asp:ControlParameter ControlID="CtlContractLookup1" Name="contractId" 
                            PropertyName="LookupTextBoxID" Type="Decimal" />
                        <asp:ControlParameter ControlID="CtlInvestorLookup1" Name="sellerInvId" 
                            PropertyName="LookupTextBoxID" Type="Decimal" />
                        <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

