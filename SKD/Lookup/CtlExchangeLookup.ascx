<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlExchangeLookup.ascx.cs" Inherits="Lookup_CtlExchangeLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

  

    function HideModalPopupExchange<%= uiBtnLookup_ModalPopupExtenderExchange.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderExchange.ClientID %>');
        modal.hide();
    }


</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDExchange" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupExchange" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupExchange" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderExchange" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupExchange"
    PopupControlID="Panel1Exchange"
    CancelControlID="uiBtnCancelExchange"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1Exchange" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Exchange Lookup</td>
            <td align="right">
                
                <asp:Button ID="uiBtnCancelExchange" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;height=100%">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Exchange Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtExchangeCode" runat="server"></asp:TextBox>
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
                        <asp:GridView ID="uiDgExchange" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ExchangeCode,ApprovalStatus" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgExchange_PageIndexChanging" 
                            
                            onsorting="uiDgExchange_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgExchange_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                              <asp:TemplateField>
                                <ItemTemplate>
                                    <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ExchangeId" InsertVisible="False" 
                                    SortExpression="ExchangeId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblExchangeID" runat="server" Text='<%# Bind("ExchangeId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ExchangeId") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExchangeCode" HeaderText="ExchangeCode" ReadOnly="True" 
                                    SortExpression="ExchangeCode" />
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceExchange" runat="server" 
                            SelectMethod="GetExchangeByExchangeCodeAndApprovalStatus" 
                            TypeName="Exchange">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtExchangeCode" Name="exchangeCode" PropertyName="Text" 
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