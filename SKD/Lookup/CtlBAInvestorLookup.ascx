<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlBAInvestorLookup.ascx.cs" Inherits="Lookup_CtlInvestorLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

    
    function HideModalPopupInvestor<%= uiBtnLookup_ModalPopupExtenderInvestor.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderInvestor.ClientID %>');
        modal.hide();
    }

     <%--function SetValueInvestor(value) {
       window.document.forms[0].<%= uiTxtLookupInvestor.ClientID %>.value = value;
    }--%>
</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
    .style2
    {
        background-color: #82B5E8;
        font-family: Arial;
        font-size: 12px;
        font-weight: normal;
        color: #000;
        padding-left: 10px;
        width: 143px;
    }
</style>
<input id="uiTxtLookupIDInvestor" runat="server" type="hidden" />    
    <asp:TextBox ID="uiTxtLookupInvestor" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
    &nbsp;<asp:Button ID="uiBtnLookupInvestor" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderInvestor" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupInvestor"
    PopupControlID="Panel1Investor"
    CancelControlID="uiBtnCancelInvestor"
     BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1Investor" runat="server" Width="500px"  CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Investor</td>
            <td align="right">
            <asp:Button ID="uiBtnCancelInvestor" runat="server" CssClass="close-icon" />
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
                                <td class="style2">
                                    Participant Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCmCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Investor Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtInvestorCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr runat="server" id="trInvestorName" class="form-content-menu" visible=false>
                                <td class="style2">
                                    Investor Name</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtInvestorName" runat="server"></asp:TextBox>
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
                        <asp:GridView ID="uiDgInvestor" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="InvestorID" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgInvestor_PageIndexChanging" 
                            onsorting="uiDgInvestor_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgInvestor_RowDataBound" >
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField HeaderText="Account" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblInvestorId" runat="server" Text='<%# Bind("InvestorID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("InvestorID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ClearingMemberCode" HeaderText="Clearing Member Code" 
                                    SortExpression="ClearingMemberCode" />

                                <asp:BoundField DataField="ClearingMemberName" HeaderText="Clearing Member Name" 
                                    SortExpression="ClearingMemberName" />

                                <asp:BoundField DataField="ExchangeMember" HeaderText="Exchange Member Code" 
                                    SortExpression="ExchangeMember" />
                                 
                                <asp:BoundField DataField="Code" HeaderText="Investor Code" 
                                    SortExpression="Code" />
                               
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceInvestor" runat="server" 
                            SelectMethod="GetCMExchangeInvestor" TypeName="Investor" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtInvestorCode" Name="investorCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtCmCode" Name="cmCode" 
                                    PropertyName="Text" Type="String" />
                                <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>