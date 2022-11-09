<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryInterest.aspx.cs" Inherits="WebUI_New_EntryInterest" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td>
        <div class="shadow_view">
            <div class="box_view">
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td style="width:100px">
                            Interest</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtKodeAk0" CssClass="required" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="uiTxtKodeAk0_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="uiTxtKodeAk0">
                        </cc1:CalendarExtender>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td >
                            Currency</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="required" ID="uiDdl">
                            <asp:ListItem Text="IDR" />
                            <asp:ListItem Text="USD" />
                        </asp:DropDownList>
                        </td>
                </tr>
                <tr>
                    <td >
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
