<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRptStressTestScenario.aspx.cs" Inherits="WebUI_FinanceAndAccounting_ViewManagePostingCode" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td class="form-header-menu">
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" 
                    Text="    Create" onclick="uiBtnCreate_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <Asp:GridView ID="uiDgManageExchange" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" >
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="uiChkDelete" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField Text="Edit" NavigateUrl="~/RiskManagement/EntryStressTestScenario.aspx" />
                        <asp:BoundField DataField="name" HeaderText="Scenario Name" 
                            SortExpression="name" />
                        <asp:HyperLinkField Text="Show" NavigateUrl="~/RiskManagement/ViewReport/StressTest.aspx" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="form-header-menu">
                <asp:Button ID="uiBtnDelete" CssClass="button_delete" runat="server" Text="   Delete" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                    DataFile="~/Data/Menu.xml" XPath="/Home/siteMapNode"></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

