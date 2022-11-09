<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewExchangeMember.aspx.cs" Inherits="WebUI_New_ViewExchangeMember" %>


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
                </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="uiDgExchangeMember" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" onrowcommand="uiDgExchangeMember_RowCommand">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:ButtonField Text="Edit" CommandName="edit" />
                        <asp:BoundField DataField="kode" HeaderText="CM Code" SortExpression="url" 
                            Visible="False" />
                        <asp:BoundField HeaderText="Exchange Member Code"></asp:BoundField>
                        <asp:BoundField HeaderText="Name" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

