<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlBankLookup.ascx.cs" Inherits="Lookup_CtlBankLookup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace ="AjaxControlToolkit" TagPrefix ="cc1" %>

<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">
    

    function HideModalPopupBank<%= uiBtnLookup_ModalPopupExtenderBank.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderBank.ClientID %>');
        modal.hide();
    }
  

</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDBank" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupBank" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupBank" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderBank" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupBank"
    PopupControlID="Panel1Bank"
    CancelControlID="uiBtnCancelBank"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1Bank" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Bank Lookup</td>
            <td align="right">
                
                <asp:Button ID="uiBtnCancelBank" runat="server" CssClass="close-icon" />
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
                                    Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCode" runat="server"></asp:TextBox>
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
                        <asp:GridView ID="uiDgBank" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Code,ApprovalStatus" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgBank_PageIndexChanging" 
                            
                            onsorting="uiDgBank_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgBank_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BankID" InsertVisible="False" 
                                    SortExpression="BankID">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblBankID" runat="server" Text='<%# Bind("BankID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BankID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Code" HeaderText="Code" 
                                    SortExpression="Code" ReadOnly="True" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />                                
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceBank" runat="server" 
                            SelectMethod="GetBankDataByCodeAndApprovalStatus" TypeName="Bank" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCode" Name="code" PropertyName="Text" 
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