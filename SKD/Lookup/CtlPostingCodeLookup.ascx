<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlPostingCodeLookup.ascx.cs" Inherits="Lookup_CtlPostingCodeLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

    function HideModalPopupPostingCode<%= uiBtnLookup_ModalPopupExtenderPostingCode.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderPostingCode.ClientID %>');
        modal.hide();
    }

    function SetValuePostingCode(value) {
       window.document.forms[0].<%= uiTxtLookupPostingCode.ClientID %>.value = value;
    }

</script>
<input id="uiTxtLookupIDPostingCode" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupPostingCode" BackColor="#FCF8C5" runat="server"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupPostingCode" runat="server" Text="Lookup" />
   <cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderPostingCode" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupPostingCode"
    PopupControlID="Panel1PostingCode"
    OnOkScript="SetValue();";
    CancelControlID="uiBtnCancelPostingCode"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1PostingCode" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td>
                Posting Code Lookup</td>
            <td align="right">
                <asp:Button ID="uiBtnCancelPostingCode" runat="server" CssClass="close-icon" />
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
                                    Acccount Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtPostingCode" runat="server"></asp:TextBox>
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
                        <asp:GridView ID="uiDgPostingCode" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="AccountCode,LedgerType,ApprovalStatus" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgPostingCode_PageIndexChanging" 
                            
                            onsorting="uiDgPostingCode_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgPostingCode_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="AccountID" InsertVisible="False" 
                                     SortExpression="AccountID" Visible="False">
                                     <ItemTemplate>
                                         <asp:Label ID="uiLblAccountID" runat="server" Text='<%# Bind("AccountID") %>'></asp:Label>
                                     </ItemTemplate>
                                     <EditItemTemplate>
                                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("AccountID") %>'></asp:TextBox>
                                     </EditItemTemplate>
                                 </asp:TemplateField>
                                <asp:BoundField DataField="AccountCode" HeaderText="Account Code" 
                                    SortExpression="AccountCode" ReadOnly="True" />
                                <asp:BoundField DataField="LedgerType" HeaderText="Ledger Type" ReadOnly="True" 
                                    SortExpression="LedgerType" />
                                <asp:BoundField DataField="AccountType" HeaderText="Account Type" 
                                    SortExpression="AccountType" />
                                 <asp:BoundField DataField="Description" HeaderText="Description" 
                                     SortExpression="Description" />
                            </Columns>
                            <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourcePostingCode" runat="server" 
                            SelectMethod="GetPostingCodeByAccountCodeAndApprovalStatus" 
                            TypeName="Posting" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtPostingCode" Name="accountCode" PropertyName="Text" 
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