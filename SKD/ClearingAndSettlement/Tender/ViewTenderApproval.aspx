<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewTenderApproval.aspx.cs" Inherits="WebUI_New_ViewTenderApproval" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlContractLookup.ascx" TagName="CtlContractLookup"
    TagPrefix="uc2" %>
<%@ Register Src="../../Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup"
    TagPrefix="uc3" %>
<%@ Register Src="../../Lookup/CtlContractCommodityLookup.ascx" TagName="CtlContractCommodityLookup"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Tender Approval</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:100px">
                                        Tender Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contract
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc4:CtlContractCommodityLookup ID="CtlContractLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Account
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc3:CtlInvestorLookup ID="CtlInvestorLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trApprovalStatus" runat="server">
                                    <td>
                                        Approval Status
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlStatus" runat="server" Width="120px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
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
        </table>
        <table class="table-datagrid">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgTender" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TenderNo,ApprovalStatus"
                                OnPageIndexChanging="uiDgTender_PageIndexChanging" OnRowDataBound="uiDgTender_RowDataBound"
                                OnSorting="uiDgTender_Sorting" AllowPaging="True" AllowSorting="True" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="EntryTenderApproval.aspx?menu=<%# Request.QueryString["menu"] %>&eType=edit&eID=<%# DataBinder.Eval(Container.DataItem, "TenderID")  %>">
                                                Detail </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TenderID" InsertVisible="False" SortExpression="TenderID"
                                        Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("TenderID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("TenderID") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Tender No" DataField="TenderNo" SortExpression="TenderNo"
                                        ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="TenderDate" HeaderText="Tender Date" SortExpression="TenderDate"
                                        DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="ContractID" SortExpression="ContractID" Visible="False">
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
                                    <asp:BoundField DataField="DeliveryLocation" HeaderText="Delivery Location" SortExpression="DeliveryLocation" />
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
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTender" runat="server" SelectMethod="GetTenderByDateContractInvApproval"
                                TypeName="Tender" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="tenderDate" Type="DateTime"
                                        PropertyName="Text" />
                                    <asp:ControlParameter ControlID="CtlContractLookup1" Name="contractId" PropertyName="LookupTextBoxID"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="CtlInvestorLookup1" Name="sellerInvId" PropertyName="LookupTextBoxID"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="uiDdlStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdImage" runat="server">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
