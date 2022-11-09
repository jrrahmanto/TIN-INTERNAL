<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewManageCurrency.aspx.cs" Inherits="WebUI_ClearingAndSettlement_ViewManageCurrency" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td class="form-header-menu">
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" 
                    Text="    Create" onclick="uiBtnCreate_Click" />
      <asp:Button ID="uiBtnDelete" CssClass="button_delete" runat="server" Text="     Delete" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="form-content-menu" colspan="3">
                            <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" 
                                ForeColor="#FF3300" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Currency Code</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="uiDdlCurrencyCode" runat="server" 
                                DataSourceID="ObjectDataSourceCurrency" DataValueField="CurrencyCode">
                                <asp:ListItem Value="DISPLAY ALL"></asp:ListItem>
                                <asp:ListItem Value="USD"></asp:ListItem>
                                <asp:ListItem Value="IDR"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="right_search_criteria">
                        <td class="form-content-menu">
                        </td>
                        <td class="separator">
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="uiDgManageCurrency" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataKeyNames="CurrencyCode" 
                    DataSourceID="ObjectDataSourceCurrency">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                    <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="uiChkList" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CurrencyCode" HeaderText="CurrencyCode" 
                            SortExpression="CurrencyCode" ReadOnly="True" />
                        <asp:BoundField DataField="Description" HeaderText="Description" 
                            SortExpression="Description" />
                        <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="CurrencyId" 
                            DataNavigateUrlFormatString="~/ClearingAndSettlement/MasterData/EntryManageCurrency.aspx?id={0}" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceCurrency" runat="server" 
                    SelectMethod="SelectCurrencyByCurrencyCode" TypeName="Currency">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="uiDdlCurrencyCode" Name="currencyCode" 
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

