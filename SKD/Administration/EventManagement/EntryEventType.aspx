<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryEventType.aspx.cs" Inherits="Administration_EventManagement_EntryEventType" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
 <h1>Manage Event Type</h1>

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
                        <td style="width:150px">
                            EventType Code</td>
                        <td >
                        :</td>
                        <td >
                            <asp:TextBox ID="uiTxtEventTypeCode" CssClass="required" MaxLength="20" Width="170" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Name</td>
                        <td >
                        :</td>
                        <td >
                            <asp:TextBox ID="uiTxtEventTypeName" runat="server" MaxLength="100" 
                                Width="363px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            SMS Message</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtSmsMessage" runat="server" Height="71px" 
                                TextMode="MultiLine" Width="400px" MaxLength="500"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="uiTxtSmsMessage" ErrorMessage="Max. 500 Character" 
                                ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Email Message</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtEmailMessage" runat="server" Height="71px" 
                                TextMode="MultiLine" Width="400px" MaxLength="500"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxtEmailMessage" ErrorMessage="Max. 500 Character" 
                                ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Application Message</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtApplicationMessage" runat="server" Height="71px" 
                                TextMode="MultiLine" Width="400px" MaxLength="500"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxtApplicationMessage" ErrorMessage="Max. 500 Character" 
                                ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator></td>
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

