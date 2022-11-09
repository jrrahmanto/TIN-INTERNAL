<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageParameterInterest.aspx.cs" Inherits="WebUI_FinanceAndAccounting_ManageParameterInterest" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="form-content-menu">
                            Tingkat Bunga</td>
                        <td class="separator">
                        :</td>
                                            <td class="right_search_criteria">
                                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;%</td>
                                        </tr>
                                        <tr>
                                            <td class="form-content-menu">
                                                Jumlah Hari</td>
                        <td class="separator">
                        :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-content-menu">
                            Pajak</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
&nbsp;%</td>
                    </tr>
                    <tr>
                        <td class="form-content-menu">
                        &nbsp;</td>
                        <td class="separator">
                        &nbsp;</td>
                        <td class="right_search_criteria">
                        <asp:Button ID="Button3" CssClass="button_save" runat="server" Text="      Save"  />
&nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" />
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
            &nbsp;</td>
        </tr>
        <tr>
            <td>
            &nbsp;</td>
        </tr>
    </table>
</asp:Content>

