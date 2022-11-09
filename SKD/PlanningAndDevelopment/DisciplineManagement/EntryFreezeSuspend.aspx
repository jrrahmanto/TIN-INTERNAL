<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryFreezeSuspend.aspx.cs" Inherits="WebUI_New_EntryFreezeSuspend" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td class="form-content-menu">
                            CM Code</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                            &nbsp;</td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Efective Date</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk" runat="server"></asp:TextBox>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Letter No.</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtPpkp" runat="server"></asp:TextBox>
                                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Letter Date</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtPpkp0" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Description</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtPpkp1" runat="server"></asp:TextBox>
                    </td>
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
</table>
</asp:Content>