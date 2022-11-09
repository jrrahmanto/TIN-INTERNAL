<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewInterest.aspx.cs" Inherits="WebUI_New_ViewInterest" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
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
                            Interest </td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="uiTxtKodeAk_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="uiTxtKodeAk">
                            </cc1:CalendarExtender>
                        &nbsp;</td>
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
                <asp:GridView ID="uiDgExchangeMember" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" >
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:ButtonField Text="Edit" CommandName="edit" />
                        <asp:ButtonField Text="Delete" />
                        <asp:BoundField DataField="kode" HeaderText="Interest" SortExpression="url" />
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

