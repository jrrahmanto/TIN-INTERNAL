<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlCMExchangeLookup.ascx.cs" Inherits="Lookup_CtlCMExchangeLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

    function CheckOtherIsCheckedByGVID(spanChk) {

        var IsChecked = spanChk.checked;
        if (IsChecked) {

            spanChk.parentElement.parentElement.style.backgroundColor = '#228b22';
            spanChk.parentElement.parentElement.style.color = 'white';
        }

        var CurrentRdbID = spanChk.id;
        var Chk = spanChk;
        Parent = document.getElementById('<%= uiDgClearingMember.ClientID %>');
        var items = Parent.getElementsByTagName('input');

        for (i = 0; i < items.length; i++) {
            if (items[i].id != CurrentRdbID && items[i].type == "radio") {

                if (items[i].checked) {
                    items[i].checked = false;
                    items[i].parentElement.parentElement.style.backgroundColor = 'white';
                    items[i].parentElement.parentElement.style.color = 'black';
                }
            }
        }
    }

    function HideModalCMExchangePopup<%= uiBtnLookup_ModalPopupExtenderCMExchange.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderCMExchange.ClientID %>');
        modal.hide();
    }

    function SetValue(value) {
       window.document.forms[0].<%= uiTxtLookupCMExchange.ClientID %>.value = value;
    }

</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDCMExchange" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupCMExchange" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupCMExchange" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderCMExchange" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupCMExchange"
    PopupControlID="Panel1"
    OnOkScript="SetValue();";
    CancelControlID="uiBtnCancel"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" Width="500px" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Clearing Member Exchange Lookup</td>
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
                                    Clearing Member Exchange Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCMExchangeCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Clearing Member Type</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:DropDownList ID="uiDdlCMType" runat="server">
                                        <asp:ListItem Value="B">Broker</asp:ListItem>
                                        <asp:ListItem Value="T">Trader</asp:ListItem>
                                    </asp:DropDownList>
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
                    <td valign="top">
                        <asp:GridView ID="uiDgClearingMember" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="CMExchangeCode" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgClearingMember_PageIndexChanging" 
                            
                            onsorting="uiDgClearingMember_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgClearingMember_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblCMExchange" runat="server" Text='<%# Bind("CMID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CMExchangeCode" HeaderText="Code" ReadOnly="True" 
                                    SortExpression="CMExchangeCode" />
                                <asp:BoundField HeaderText="Name" />
                            </Columns>
                            <headerstyle CssClass="tblHeaderStyle" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceClearingMember" runat="server" 
                            SelectMethod="fillLookup" TypeName="ClearingMemberExchange">
                            <SelectParameters>
                                <asp:SessionParameter Name="CMID" SessionField="CMIDLookup" Type="Decimal" />
                                <asp:ControlParameter ControlID="uiTxtCMExchangeCode" Name="CMExchangeCode" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiDdlCMType" Name="CMType" PropertyName="SelectedValue" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>