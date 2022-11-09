<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlContractCommodityLookup.ascx.cs" Inherits="Lookup_CtlContractCommodityLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">


   function HideModalPopupContractCommodity<%= uiBtnLookup_ModalPopupExtenderContractCommodity.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderContractCommodity.ClientID %>');
        modal.hide();
    }

</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
    .style2
    {
        width: 158px;
    }
</style>
    <input id="uiTxtLookupIDContractCommodity" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupContractCommodity" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupContractCommodity" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderContractCommodity" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupContractCommodity"
    PopupControlID="Panel1"
    OnOkScript="SetValue();";
    CancelControlID="uiBtnCancel"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width:100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Contract Commodity Lookup</td>
            <td align="right">
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;height:100%">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                        <tr class="form-content-menu">
                                <td class="style2">
                                    Exchange</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:DropDownList ID="uiDdlExchange" runat="server" DataSourceID="odsExchange" 
                                        DataTextField="ExchangeCode" DataValueField="ExchangeId">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Commodity Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCommodityCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Commodity Name</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCommodityName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Currency</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:DropDownList ID="uiDdlCurrency" runat="server" >
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="1">IDR</asp:ListItem>
                                        <asp:ListItem Value="2">USD</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr class="right_search_criteria">
                                <td class="form-content-menu">
                                </td>
                                <td class="separator">
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" 
                                        onclick="uiBtnSearch_Click" Text="     Search" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>                    
                    <td valign="top">
                        <asp:GridView ID="uiDgContractCommodity" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="CommName" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgContractCommodity_PageIndexChanging" 
                            
                            onsorting="uiDgContractCommodity_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgContractCommodity_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField HeaderText="Contract ID" SortExpression="ContractID" 
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblContractCommodityID" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExchangeName" HeaderText="Exchange" 
                                    SortExpression="ExchangeName" />
                                <asp:BoundField DataField="CommodityCode" HeaderText="Code" 
                                    SortExpression="CommodityCode" />
                                <asp:BoundField DataField="CommName" HeaderText="Name" ReadOnly="True" 
                                    SortExpression="CommName" />
                                <asp:BoundField DataField="ContractYear" HeaderText="Year" 
                                    ReadOnly="True" SortExpression="ContractYear" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ContractMonthPadding" HeaderText="Month" 
                                    ReadOnly="True" SortExpression="ContractMonthPadding">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" 
                                    ReadOnly="True" SortExpression="Currency">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceContract" runat="server" 
                            SelectMethod="GetContractByCommodityCode" TypeName="Contract" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCommodityCode" Name="CommodityCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiDdlExchange" Name="exchangeID" 
                                    PropertyName="SelectedValue" Type="Decimal" />
                                <asp:ControlParameter ControlID="uiTxtCommodityName" Name="commName" 
                                    PropertyName="Text" Type="String" />
                                <asp:ControlParameter ControlID="uiDdlCurrency" Name="homeCurrency" 
                                    PropertyName="SelectedValue" Type="Decimal" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsExchange" runat="server" DeleteMethod="Delete" 
                            InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
                            SelectMethod="GetActiveOnly" 
                            TypeName="ExchangeDataTableAdapters.ExchangeTableAdapter" UpdateMethod="Update">
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
