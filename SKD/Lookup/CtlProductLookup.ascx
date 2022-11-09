<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlProductLookup.ascx.cs" Inherits="Lookup_CtlProductLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="<%=ResolveUrl("~/Styles/StyleSheet.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=ResolveUrl("~/Styles/Style.css")%>" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

    
    function HideModalPopupProduct<%= uiBtnLookup_ModalPopupExtenderProduct.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderProduct.ClientID %>');
        modal.hide();
    }
</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDProduct" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupProduct" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupProduct" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderProduct" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupProduct"
    PopupControlID="Panel1Product"
    CancelControlID="uiBtnCancelProduct"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1Product" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Product Lookup</td>
            <td align="right">
                <asp:Button ID="uiBtnCancelProduct" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Product Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtProductCode" runat="server"></asp:TextBox>
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
                    <td valign=top>
                        <asp:GridView ID="uiDgProduct" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ProductCode" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgProduct_PageIndexChanging" 
                            
                            onsorting="uiDgProduct_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgProduct_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                  <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product ID" SortExpression="ProductID" 
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblProductID" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ProductID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" ReadOnly="True" 
                                    SortExpression="ProductCode" />
                                  <asp:BoundField DataField="ExchangeTypeDesc" HeaderText="Exchange Type" 
                                      SortExpression="ExchangeTypeStatus" />
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceProduct" runat="server" 
                            SelectMethod="GetProductByProductCodeAndApprovalStatus" TypeName="Product">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtProductCode" Name="productCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>