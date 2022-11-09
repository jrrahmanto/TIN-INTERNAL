<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlTradeProgress.ascx.cs" Inherits="Lookup_CltTradeRegister" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc3" %>

<script type="text/javascript" language="javascript">



    function HideModalPopupCommodity<%= uiBtnLookup_ModalPopupExtenderCommodity.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderCommodity.ClientID %>');
        modal.hide();
    }


</script>

<style type="text/css">
    .style1 {
        width: 422px;
    }
</style>
<input id="uiTxtLookupIDCommodity" runat="server" type="hidden" />
<asp:TextBox ID="uiTxtLookupCommodity" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
&nbsp;<asp:Button ID="uiBtnLookupCommodity" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderCommodity" runat="server"
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupCommodity"
    PopupControlID="Panel1Commodity"
    CancelControlID="uiBtnCancelCommodity"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1Commodity" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
    <table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">Trade Progress </td>
            <td align="right">
                <asp:Button ID="uiBtnCancelCommodity" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width: 100%;">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width: 100%;">

<%--                            <tr class="form-content-menu">
                                <td class="form-content-menu">Bussiness Date</td>
                                <td class="separator">:</td>
                                <td class="right_search_criteria">
                                    <uc3:CtlCalendarPickUp ID="uiDate" runat="server" />
                                </td>
                            </tr>--%>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">Seller Name</td>
                                <td class="separator">:</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiSellerName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">Exchange Ref</td>
                                <td class="separator">:</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiExchangeRef" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="form-content-menu"></td>
                                <td class="separator"></td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search"
                                        OnClick="uiBtnSearch_Click" Text="     Search" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:GridView ID="uiDgCommodity" runat="server" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ProgressID"
                            MouseHoverRowHighlightEnabled="True" EmptyDataText="No Record"
                            OnPageIndexChanging="uiDgCommodity_PageIndexChanging"
                            OnSorting="uiDgCommodity_Sorting" RowHighlightColor="" Width="100%"
                            OnRowDataBound="uiDgCommodity_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Progress ID" SortExpression="ProgressID"
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblCommodityID" runat="server" Text='<%# Bind("ProgressID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ProgressID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProgressID" HeaderText="ProgressID" ReadOnly="True"
                                    SortExpression="ProgressID" />
                                <asp:BoundField DataField="ExchangeRef" HeaderText="ExchangeRef"
                                    SortExpression="ExchangeRef" />
                                <asp:BoundField DataField="Name" HeaderText="Seller"
                                    SortExpression="Name" />
                                <asp:BoundField DataField="buyer" HeaderText="Buyer"
                                    SortExpression="buyer" />
                                <asp:TemplateField HeaderText="amount" SortExpression="amount">
                                    <ItemTemplate>
                                        <asp:Label ID="amount" runat="server" Text='<%# Bind("amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceCommodity" runat="server"
                            SelectMethod="GetDataLookup" TypeName="CommodityDataTableAdapters.CommodityLookupTableAdapter"
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiSellerName" Name="CommName"
                                    PropertyName="Text" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsExchange" runat="server"
                            DeleteMethod="Delete" InsertMethod="Insert"
                            OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetActiveOnly"
                            TypeName="ExchangeDataTableAdapters.ExchangeTableAdapter"
                            UpdateMethod="Update">
                            <DeleteParameters>
                                <asp:Parameter Name="Original_ExchangeCode" Type="String" />
                                <asp:Parameter Name="Original_ApprovalStatus" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="ExchangeIPAddress" Type="String" />
                                <asp:Parameter Name="LocalIPAddress" Type="String" />
                                <asp:Parameter Name="LocalPort" Type="Int32" />
                                <asp:Parameter Name="CreatedBy" Type="String" />
                                <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                                <asp:Parameter Name="ApprovalDesc" Type="String" />
                                <asp:Parameter Name="ExchangeType" Type="String" />
                                <asp:Parameter Name="ExchangeName" Type="String" />
                                <asp:Parameter Name="ActionFlag" Type="String" />
                                <asp:Parameter Name="TenderFlag" Type="String" />
                                <asp:Parameter Name="TransferFlag" Type="String" />
                                <asp:Parameter Name="OriginalId" Type="Decimal" />
                                <asp:Parameter Name="Original_ExchangeCode" Type="String" />
                                <asp:Parameter Name="Original_ApprovalStatus" Type="String" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ExchangeCode" Type="String" />
                                <asp:Parameter Name="ApprovalStatus" Type="String" />
                                <asp:Parameter Name="ExchangeIPAddress" Type="String" />
                                <asp:Parameter Name="LocalIPAddress" Type="String" />
                                <asp:Parameter Name="LocalPort" Type="Int32" />
                                <asp:Parameter Name="CreatedBy" Type="String" />
                                <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                                <asp:Parameter Name="ApprovalDesc" Type="String" />
                                <asp:Parameter Name="ExchangeType" Type="String" />
                                <asp:Parameter Name="ExchangeName" Type="String" />
                                <asp:Parameter Name="ActionFlag" Type="String" />
                                <asp:Parameter Name="TenderFlag" Type="String" />
                                <asp:Parameter Name="TransferFlag" Type="String" />
                                <asp:Parameter Name="OriginalId" Type="Decimal" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
