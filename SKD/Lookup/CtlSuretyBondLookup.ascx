<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlSuretyBondLookup.ascx.cs" Inherits="Lookup_CtlSuretyBondLookup" %>
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
    <input id="uiTxtLookupIDSurety" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupSurety" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
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
                Bond Issuer Lookup</td>
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
                                    Investor Name</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtInvestorName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Account Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtAccCode" runat="server"></asp:TextBox>
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
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="SuretyBondID,ApprovalStatus" 
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
                                <asp:TemplateField HeaderText="BondID" InsertVisible="False" 
                                    SortExpression="BondID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblBondID" runat="server" Text='<%# Bind("SuretyBondID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("SuretyBondID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BondSerialNo" HeaderText="Bond Serial Number" ReadOnly="True" 
                                    SortExpression="BondSerialNo" />
                                <asp:BoundField DataField="Code" HeaderText="Account Code" ReadOnly="true"
                                    SortExpression ="Code" />
                                <asp:BoundField DataField="InvestorName" HeaderText="Investor Name" ReadOnly="true"
                                    SortExpression ="InvestorName" />

                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceSuretyBond" runat="server" 
                            SelectMethod="GetDataForLookUpSuretyBond" 
                            TypeName="SuretyBond">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtInvestorName" Name="investorName" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtAccCode" Name="accCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
                                <asp:Parameter DefaultValue="Y" Name="ActiveStatus" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>