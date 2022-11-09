<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlUserMemberLookup.ascx.cs"
    Inherits="Lookup_CtlUserMemberLookup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">
    function HideModalPopupUser<%= uiBtnLookup_ModalPopupExtenderUser.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderUser.ClientID %>');
        modal.hide();
    }
</script>

<input id="uiTxtLookupIDUser" runat="server" type="hidden" />
<asp:TextBox ID="uiTxtLookupUser" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
&nbsp;<asp:Button ID="uiBtnLookupUser" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderUser" runat="server" DynamicServicePath=""
    Enabled="True" TargetControlID="uiBtnLookupUser" PopupControlID="Panel1User"
    OnOkScript="SetValueUser();" CancelControlID="uiBtnCancelUser" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1User" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
    <table style="width: 100%;" class="header_lookup">
        <tr>
            <td>
                Data User
            </td>
            <td align="right">
                <asp:Button ID="uiBtnCancelUser" CssClass="close-icon" runat="server" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width: 100%; height=100%">
                <tr valign="top">
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width: 100%;">
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Username
                                </td>
                                <td class="separator">
                                    :
                                </td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtUserName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="form-content-menu">
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
                <tr valign="top">
                    <td>
                        <asp:GridView ID="uiDgUser" runat="server" AllowPaging="True" AllowSorting="True" EmptyDataText="No Record"
                            AutoGenerateColumns="False" DataKeyNames="UserId" MouseHoverRowHighlightEnabled="True"
                            OnPageIndexChanging="uiDgUser_PageIndexChanging" OnSorting="uiDgUser_Sorting"
                            RowHighlightColor="" Width="100%" OnRowDataBound="uiDgUser_RowDataBound">
                            <RowStyle CssClass="tblRowStyle" VerticalAlign="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UserID" SortExpression="EMID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblUserID" runat="server" Text='<%# Bind("USERID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("USERID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" SortExpression="UserName" />
                            </Columns>
                            <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceUser" runat="server" SelectMethod="GetUserByUserNameLike"
                            TypeName="SKDUser" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtUserName" Name="userName" PropertyName="Text"
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
