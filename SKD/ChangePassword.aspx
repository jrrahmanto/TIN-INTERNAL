<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Change Password</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td  colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                <tr>
                <td style="width:150px">
                Old Password</td>
                <td style="width:10px">
                :</td>
            <td>
                <asp:TextBox ID="uiTxtOldPassword" runat="server" TextMode="Password" CssClass="required"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                New Password</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="uiTxtNewPassword" runat="server" TextMode="Password" CssClass="required"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Retype Password</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="uiTxtReTypePassword" runat="server" TextMode="Password" CssClass="required"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="uiTxtNewPassword" ControlToValidate="uiTxtReTypePassword" 
                    ErrorMessage="New password does not match."></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                                Text="     Save" onclick="uiBtnSave_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                            Text="      Cancel" onclick="uiBtnCancel_Click" CausesValidation="False" />
                    
            </td>
        </tr>
        </table>
        </div>
        </div>
        </td>
        </tr>
        </table>
</asp:Content>

