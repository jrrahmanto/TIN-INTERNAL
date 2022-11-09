<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManageFee.aspx.cs" Inherits="FinanceAndAccounting_Parameter_ViewManageFee" %>

<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Manage Fee</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
    <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width:150px">
                                    Commodity Code
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Approval Status
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlApprovalStatus" runat="server" Width="126px">
                                        <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
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
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                    OnClick="uiBtnCreate_Click" />
            </td>
        </tr>
        <tr>
            <td>
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                <asp:GridView ID="uiDgFee" runat="server" AutoGenerateColumns="False"
                    Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" 
                                AllowPaging="True" AllowSorting="True" 
                                onpageindexchanging="uiDgFee_PageIndexChanging" onsorting="uiDgFee_Sorting">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Edit">
                             <ItemTemplate>
                               <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("FeeID", "~/FinanceAndAccounting/Parameter/EntryManageFee.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                             </ItemTemplate>
                             <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CommodityCode" HeaderText="Commodity Code" 
                            SortExpression="CommodityCode" />
                        <asp:BoundField HeaderText="EffectiveStartDate" DataField="EffectiveStartDate" 
                            DataFormatString="{0:dd-MMM-yyyy}" SortExpression="EffectiveStartDate" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Clearing Fund" DataField="ClearingFund" DataFormatString="{0:#,##0.###}"
                            SortExpression="ClearingFund">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Status" DataField="Status" SortExpression="Status" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceFee" runat="server" 
                                SelectMethod="SelectFeeByCommodityCodeAndStatus" TypeName="Fee">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCommodityLookup1" Name="commodityID" 
                                        PropertyName="LookupTextBoxID" Type="Decimal" />
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
            </td>
        </tr>
        </table>
</asp:Content>
