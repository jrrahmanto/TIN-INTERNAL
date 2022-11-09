<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryUser.aspx.cs" Inherits="WebUI_New_EntryUser" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <h1>Manage User</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
    
     <tr>
        <td>
        <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red"></asp:BulletedList>
        </td>
    </tr>
    
        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                   
                    <tr>
                        <td style="width:100px">
                            Login
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="uiTxbUserName" CssClass="required" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Password
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="uiTxbPassword" CssClass="required" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            User Name
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="uiTxbName" runat="server"></asp:TextBox>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td >
                            Internal
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:CheckBox ID="uiChkInternal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Clearing Member
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="uiCtlClearingMember" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Start Time
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="uiTxbStartTime" runat="server" Width="62px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Start time is not in correct format, expected format is hh:mm."
                                ControlToValidate="uiTxbStartTime" ValidationExpression="([0-1]\d|2[0-3]):([0-5]\d)"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            End Time
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="uiTxbEndTIme" runat="server" Width="62px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="End time is not in correct format, expected format is hh:mm."
                                ControlToValidate="uiTxbEndTIme" ValidationExpression="([0-1]\d|2[0-3]):([0-5]\d)"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Locked
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:CheckBox ID="uiChkLocked" CssClass="required" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Disabled</td>
                        <td >
                            :</td>
                        <td>
                            <asp:CheckBox ID="uiChkDisabled" CssClass="required" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            User Roles
                        </td>
                        <td >
                            :
                        </td>
                        <td>
                            <asp:CheckBoxList ID="uiChkGroup" CssClass="required" runat="server">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;
                        </td>
                        <td >
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                OnClick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                OnClick="uiBtnCancel_Click" />
                        </td>
                        </tr>
                </table>
             </div>
             </div>
            </td>
        </tr>
    </table>
</asp:Content>
