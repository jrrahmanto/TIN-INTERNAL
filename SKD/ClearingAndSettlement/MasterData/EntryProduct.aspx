<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryProduct.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryProduct"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Product</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
                    <td colspan="3">
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
                    <td style="width:100px">
                            Product Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtProductCode" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                                                            
                                                            </td>
                </tr>
                <tr>
                    <td>
                            Product Name</td>
                    <td>
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtProductName" CssClass="required" runat="server" MaxLength="50" Width="250"></asp:TextBox>
                                                            </td>
                </tr>
                <tr>
                    <td>
                            ExchangeType</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlExchangeType" CssClass="required" runat="server">
                            <asp:ListItem Value="S">SPA</asp:ListItem>
                            <asp:ListItem Value="P">PRIMER</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trAction" runat="server">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAction" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="80px" MaxLength="100" Width="400px" 
                            TextMode="MultiLine"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="uiTxtApprovalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click"  />
                        <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                                Text="      Delete" onclick="uiBtnDelete_Click" />
     <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
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


