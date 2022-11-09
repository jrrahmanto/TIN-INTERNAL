<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryEventReceipientList.aspx.cs" Inherits="Administration_EventManagement_EntryEventReceipientList"  %>
<%@ Register Assembly ="AjaxControlToolkit" Namespace ="AjaxControlToolkit" TagPrefix ="cc1" %>
<%@ Register src="../../Lookup/CtlUserMemberLookup.ascx" tagname="CtlUserMemberLookup" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage Event Recipient List</h1>
 
<table cellpadding="1" cellspacing="1" style="width:100%;">

        <tr>
          <td colspan="3">
          <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
          </td>
        </tr>

        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td style="width:200px">
                            EventReceipientName</td>
                        <td>
                        :</td>
                        <td >
                            <asp:TextBox ID="uiTxtEventReceipientName" CssClass="required" Width="170" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Phone Number</td>
                        <td >
                        :</td>
                        <td >
                            <asp:TextBox ID="uiTxtPhoneNumber"  runat="server" MaxLength="50" Width="200"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="uiTxtPhoneNumber_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="uiTxtPhoneNumber" 
                                ValidChars="+1234567890">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Email Address</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtEmailAddress" runat="server" MaxLength="100" Width="250"></asp:TextBox>
                         </td>
                    </tr>
                    <tr>
                        <td  valign="top">
                            User ID</td>
                        <td  valign="top">
                            :</td>
                        <td >
                            <uc1:CtlUserMemberLookup ID="CtlUserMemberLookup1"  runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                                Text="      Save" onclick="uiBtnSave_Click"  />
                        <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                                Text="      Delete" onclick="uiBtnDelete_Click" />
                                <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                                Text="      Cancel" onclick="uiBtnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            </div>
            </td>
        </tr>
    </table>
            </asp:Content>

