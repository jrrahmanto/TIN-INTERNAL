<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewIjinUsaha.aspx.cs" Inherits="WebUI_New_ViewIjinUsaha" %>

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
            &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            CM Code</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk" runat="server"></asp:TextBox>
                        &nbsp;<asp:Button ID="Button1" runat="server" Text="..." Width="26px" />
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Certificate Type</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                        <asp:DropDownList ID="uiDdlPemberiSanksi" runat="server" Height="16px" 
                            Width="229px">
                            <asp:ListItem>Akte Perubahan Anggaran Dasar</asp:ListItem>
                            <asp:ListItem>Akte Pendirian Perusahaan</asp:ListItem>
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
                </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="uiDgIjinUsaha" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" onrowcommand="uiDgIjinUsaha_RowCommand">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:ButtonField Text="Edit" CommandName="edit" />
                        <asp:BoundField DataField="kode" HeaderText="No." SortExpression="url" />
                        <asp:BoundField HeaderText="Notary Name"></asp:BoundField>
                        <asp:BoundField HeaderText="Legalization Date" />
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

