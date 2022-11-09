<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryExchangeMember.aspx.cs" Inherits="WebUI_New_EntryExchangeMember" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Entry Exchange Member</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td>
         <div class="shadow_view">
            <div class="box_view">
            <table class="table-row">
                <tr>
                    <td style="width:150px">
                            CM Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Exchange Member Code</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtKodeAk0" runat="server"></asp:TextBox>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Exchange Member Name</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtKodeAk1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Exchange</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                            <asp:ListItem>BBJ Primer</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="Button3" CssClass="button_save" runat="server" Text="      Save"  />
&nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" />
                    </td>
                </tr>
                </table>
             </div>
             </div>
             </td>
             </tr>
             </table>
</asp:Content>

