<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlExchangeMemberLookup.ascx.cs" Inherits="Lookup_CtlExchangeMemberLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">



    function HideModalPopupExchangeMember<%= uiBtnLookup_ModalPopupExtenderExchangeMember.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderExchangeMember.ClientID %>');
        modal.hide();
    }


</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDExchangeMember" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupExchangeMember" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupExchangeMember" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender  ID="uiBtnLookup_ModalPopupExtenderExchangeMember" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupExchangeMember"
    PopupControlID="Panel1ExchangeMember"
    CancelControlID="uiBtnCancelExchangeMember"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1ExchangeMember" runat="server" Width="500px" CssClass="tbl_lookup">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Exchange Member Lookup</td>
            <td align="right">
                <asp:Button ID="uiBtnCancelExchangeMember" runat="server"  CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1ExchangeMember" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Exchange Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtExCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Clearing Member Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCMCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Exchange Member Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtEMCode" runat="server"></asp:TextBox>
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
                        <asp:GridView ID="uiDgExchangeMember" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False"  EmptyDataText="No Record"
                            MouseHoverRowHighlightEnabled="True" 
                            onpageindexchanging="uiDgExchangeMember_PageIndexChanging" 
                            
                            onsorting="uiDgExchangeMember_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgExchangeMember_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ExchangeMemberID" SortExpression="EMID" 
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblEMID" runat="server" Text='<%# Bind("EMID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("EMID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange" SortExpression="ExchangeCode">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ExchangeCode") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ExchangeCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clearing Member" SortExpression="CMCode">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CMCode") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CMCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange Member" 
                                    SortExpression="ExchangeMemberCode">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" 
                                            Text='<%# Bind("ExchangeMemberCode") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblExchangeMemberCode" runat="server" Text='<%# Bind("ExchangeMemberCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceExchangeMember" runat="server" 
                            SelectMethod="GetExchangeMemberCompleteByExCmEmCode" 
                            TypeName="ExchangeMember" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtExCode" Name="exchangeCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtCMCode" Name="cmCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtEMCode" Name="emCode" PropertyName="Text" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>