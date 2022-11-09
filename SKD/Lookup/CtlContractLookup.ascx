<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlContractLookup.ascx.cs"
    Inherits="Lookup_CtlContractLookup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
        Parent = document.getElementById('<%= uiDgContract.ClientID %>');
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

    function HideModalContractPopup<%= uiBtnLookup_ModalPopupExtenderContract.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderContract.ClientID %>');
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
<input id="uiTxtLookupIDContract" runat="server" type="hidden" />
<asp:TextBox ID="uiTxtLookupContract" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
&nbsp;<asp:Button ID="uiBtnLookupContract" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderContract" runat="server"
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupContract" PopupControlID="Panel1"
    OnOkScript="SetValue();" CancelControlID="uiBtnCancel" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" Width="500px" Height="500px" DefaultButton="uiBtnSearch">
    <table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Contract Lookup
            </td>
            <td align="right">
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width: 100%; height=100%">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width: 100%;">
                            <tr class="form-content-menu">
                                <td class="style2">
                                    Commodity Name
                                </td>
                                <td class="separator">
                                    :
                                </td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCommodityCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="style2">
                                </td>
                                <td class="separator">
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" OnClick="uiBtnSearch_Click"
                                        Text="     Search" />
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
                        <asp:GridView ID="uiDgContract" runat="server" AllowPaging="True" AllowSorting="True" EmptyDataText="No Record"
                            AutoGenerateColumns="False" DataKeyNames="CommName" MouseHoverRowHighlightEnabled="True"
                            OnPageIndexChanging="uiDgClearingMember_PageIndexChanging" OnSorting="uiDgClearingMember_Sorting"
                            RowHighlightColor="" Width="100%" OnRowDataBound="uiDgClearingMember_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contract ID" SortExpression="ContractID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblContractID" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CommName" HeaderText="Name" ReadOnly="True" SortExpression="CommName" />
                            </Columns>
                            <HeaderStyle CssClass="tblHeaderStyle" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceContract" runat="server" SelectMethod="GetContractByCommodityCode"
                            TypeName="Contract">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCommodityCode" Name="CommodityCode" PropertyName="Text"
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
