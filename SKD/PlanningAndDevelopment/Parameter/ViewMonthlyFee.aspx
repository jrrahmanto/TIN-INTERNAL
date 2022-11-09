<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewMonthlyFee.aspx.cs" Inherits="WebUI_New_ViewMonthlyFee" %>



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
                </td>
        </tr>
        <tr>
            <td>
                <Asp:GridView ID="uiDgExchangeMember" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="kode" HeaderText="MonthlyFee" 
                            SortExpression="url" />
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



