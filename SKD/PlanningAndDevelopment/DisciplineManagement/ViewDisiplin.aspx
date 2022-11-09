<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewDisiplin.aspx.cs" Inherits="WebUI_New_ViewSuspend" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
                <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Discipline Type</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem>Suspend</asp:ListItem>
                                <asp:ListItem>Freeze</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            CM Code</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk" runat="server"></asp:TextBox>
                        &nbsp;<asp:Button ID="Button1" runat="server" Text="..." />
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Yang Memberikan Sanksi</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                        <asp:DropDownList ID="uiDdlPemberiSanksi" runat="server" Height="16px" 
                            Width="126px">
                            <asp:ListItem>Bappebti</asp:ListItem>
                            <asp:ListItem>BBJ</asp:ListItem>
                            <asp:ListItem>KBI</asp:ListItem>
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
                <Asp:GridView ID="uiDgSuspend" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" onrowcommand="uiDgSuspend_RowCommand">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:ButtonField Text="Edit" CommandName="edit" />
                        <asp:BoundField DataField="kode" HeaderText="CM Code" SortExpression="url" />
                        <asp:BoundField HeaderText="Yang Memberi Sanksi"></asp:BoundField>
                        <asp:BoundField HeaderText="Letter No." />
                        <asp:BoundField HeaderText="Efective Date" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

