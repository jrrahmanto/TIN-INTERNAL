<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlBankAccountLookup.ascx.cs" Inherits="Lookup_CtlBankAccountLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />--%>
<%--<link href="<%=ResolveUrl("~/Styles/StyleSheet.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=ResolveUrl("~/Styles/Style.css")%>" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">
    

    function HideModalPopupBankAccount<%= uiBtnLookup_ModalPopupExtenderBankAccount.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderBankAccount.ClientID %>');
        modal.hide();
    }
  

</script>

<style type="text/css">
    .style1
    {
        width: 622px;
    }
    .style2
    {
        background-color: #82B5E8;
        font-family: Arial;
        font-size: 12px;
        font-weight: normal;
        color: #000;
        padding-left: 10px;
        width: 180px;
    }
</style>
    <input id="uiTxtLookupIDBankAccount" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupBankAccount" BackColor="#FCF8C5" runat="server" EnableViewState="False" Width="300"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupBankAccount" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderBankAccount" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupBankAccount"
    PopupControlID="Panel1BankAccount"
    CancelControlID="uiBtnCancelBankAccount"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1BankAccount" runat="server" Width="652px" 
    CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Bank Account Lookup</td>
            <td align="right">
                
                <asp:Button ID="uiBtnCancelBankAccount" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Account Type</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:DropDownList ID="uiDdlAccType" runat="server">
                                        <%--<asp:ListItem Value="RTLK">Rekening Terpisah Lembaga Kliring</asp:ListItem>--%>
                                        <asp:ListItem Value="RD">Rekening Deposit</asp:ListItem>
                                        <asp:ListItem Value="RS">Rekening Settlement</asp:ListItem>
                                        <%--<asp:ListItem Value="RTI">Rekening Terpisah Investor</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Bank</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:DropDownList ID="uiDdlBank" runat="server" DataSourceID="odsBank" 
                                        DataTextField="Name" DataValueField="BankID">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Account No</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtAccountNo" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="style2">
                                    Clearing Member Name</td>
                                <td class="separator">
                                    :</td>
                                <td>
                                    <asp:TextBox ID="uiTxtCMName" runat="server" Width="270px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="style2">
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
                        <asp:GridView ID="uiDgBankAccount" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="AccountNo,BankID,ApprovalStatus,EffectiveStartDate" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgBankAccount_PageIndexChanging" 
                            
                            onsorting="uiDgBankAccount_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgBankAccount_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BankAccountID" InsertVisible="False" 
                                    SortExpression="BankAccountID" Visible="False">
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblBankAccountID" runat="server" Text='<%# Bind("BankAccountID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BankAccountID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AccountType" HeaderText="Account Type" 
                                    SortExpression="AccountType" />
                                <asp:BoundField DataField="BankName" HeaderText="Bank" 
                                    SortExpression="BankName" />
                                <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" 
                                    SortExpression="CurrencyCode" />
                                <asp:BoundField DataField="CMName" HeaderText="Clearing Member" 
                                    SortExpression="CMName" />
                                <asp:BoundField DataField="InvestorID" HeaderText="Investor ID" 
                                    SortExpression="InvestorID"/>
                                <asp:BoundField DataField="InvestorCode" HeaderText="Investor Code" 
                                    SortExpression="InvestorCode"/>
                                <asp:BoundField DataField="InvestorName" HeaderText="Investor" 
                                    SortExpression="InvestorName" />
                                 <asp:BoundField DataField="AccountNo" HeaderText="Account #" ReadOnly="True" 
                                    SortExpression="AccountNo" />
                                 
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsBankAccount" runat="server" 
                            SelectMethod="SelectBankAccountBySearchCriteria" TypeName="BankAccount" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCMName" Name="CMName" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiDdlAccType" Name="AccountType" 
                                    PropertyName="SelectedValue" Type="String" />
                                <asp:ControlParameter ControlID="uiDdlBank" Name="BankID" 
                                    PropertyName="SelectedValue" Type="Decimal" />
                                <asp:ControlParameter ControlID="uiTxtAccountNo" Name="accountNo" PropertyName="Text" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsBank" runat="server" 
                            OldValuesParameterFormatString="original_{0}" 
                            SelectMethod="GetDataByBankCodeAndStatus" 
                            TypeName="BankDataTableAdapters.BankTableAdapter">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="" Name="BankCode" Type="String" />
                                <asp:Parameter DefaultValue="A" Name="ApprovalStatus" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>