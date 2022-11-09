<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryMonthlyFee.aspx.cs" Inherits="WebUI_New_EntryMonthlyFee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td class="form-content-menu">
                            Monthly Fee</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            &nbsp;</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                        <asp:Button runat="server" Text="      Save" CssClass="button_save" ID="Button26"></asp:Button>

&nbsp;<asp:Button runat="server" Text="      Cancel" CssClass="button_cancel" ID="uiBtnCancel15"></asp:Button>

                </tr>
                </table>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>

