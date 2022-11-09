<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageParameterForAutoSuspension.aspx.cs" Inherits="RiskManagement_MasterData_EntryManageParameterForAutoSuspension" Title="Untitled Page" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Parameter For Auto Suspension</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td colspan="3">
            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
            </asp:BulletedList>
        </td>
   </tr>
    <tr>
        <td colspan="3">
            <asp:BulletedList ID="uiBLSuccess" runat="server" ForeColor="Blue" Visible="False">
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
                            Unit In-Charge</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtUIC" runat="server" CssClass="required" MaxLength="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Person In-Charge</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtPIC" runat="server" CssClass="required" MaxLength="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Email CC1</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtEmailCC1" runat="server" CssClass="required" MaxLength="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Email CC2</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtEmailCC2" runat="server" CssClass="required" MaxLength="50" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                            Limit Threshold</td>
                    <td>
                        :</td>
                    <td>
                        <cc1:FilteredTextBox ID="uiTxtLimitThreshold" CssClass="number-required" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc1:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                            Available Margin Tolerance</td>
                    <td>
                        :</td>
                    <td>
                        <cc1:FilteredTextBox ID="uiTxtAMTole" CssClass="number-required" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc1:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Limit Time</td>
                    <td>
                        :</td>
                    <td>
                        <cc1:FilteredTextBox ID="uiTxtLimitTime_hh" CssClass="number-required" 
                            FilterTextBox="Normal" ValidChar="0123456789.,-" runat="server" MaxLength="2" 
                            Width="29px"></cc1:FilteredTextBox>
                        :
                        <cc1:FilteredTextBox ID="uiTxtLimitTime_mm" CssClass="number-required" 
                            FilterTextBox="Normal" ValidChar="0123456789.,-" runat="server" MaxLength="2" 
                            Width="29px"></cc1:FilteredTextBox>
                        (hh:mm)
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                            Activation Modules</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlActivationModule" runat="server">
                            <asp:ListItem>YES</asp:ListItem>
                            <asp:ListItem>NO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnUpdate" CssClass="button_approve" runat="server" 
                            Text="      Update" onclick="uiBtnUpdate_Click"  />
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

