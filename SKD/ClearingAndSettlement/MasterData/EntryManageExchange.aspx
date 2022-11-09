<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageExchange.aspx.cs" Inherits="WebUI_ClearingAndSettlement_EntryManageExchange" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <h1>Manage Exchange</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     
     <tr>
         <td>
             <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
         </td>
     </tr>
     
        <tr>
            <td>
             <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                     <tr>
                        <td>
                            Exchange Code</td>
                        <td>
                        :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxtExchangeCode" CssClass="required" MaxLength="20" Width="170" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Exchange Name</td>
                        <td >
                        :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxtExhangeName" MaxLength="50" Width="170" CssClass="required" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Exchange IP Address</td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxtExchangeIP" CssClass="required" MaxLength="50" Width="170" runat="server"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                                runat="server" ControlToValidate="uiTxtExchangeIP" 
                                ErrorMessage="Please enter a valid Exchange IP Address." 
                                ValidationExpression=^(?:(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)\.){3}(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)$></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Local IP Address</td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxtLocalIP" MaxLength="50" CssClass="required" Width="170"  runat="server"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
                                runat="server" ControlToValidate="uiTxtLocalIP" 
                                ErrorMessage="Please enter a valid Local IP Address." 
                                ValidationExpression="^(?:(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)\.){3}(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Local Port</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtLocalPort" Width="170" CssClass="number-required"  runat="server"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="uiTxtLocalPort_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="uiTxtLocalPort">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Exchange Type
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="uiDdlExchangeType" CssClass="required" runat="server">
                                <asp:ListItem Value="P">Primer</asp:ListItem>
                                <asp:ListItem Value="S">SPA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Tender Flag</td>
                        <td >
                            :</td>
                        <td >
                            <asp:DropDownList ID="uiDdlTenderFlag" CssClass="required" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Transfer Flag</td>
                        <td >
                            :</td>
                        <td >
                            <asp:DropDownList ID="uiDdlTransferFlag" CssClass="required" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Exchange IP Address Outbound</td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbExchangeIPAddressOutbound" MaxLength="50" CssClass="required" Width="170"  runat="server"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator4" 
                                runat="server" ControlToValidate="uiTxtLocalIP" 
                                ErrorMessage="Please enter a valid Local IP Address." 
                                ValidationExpression="^(?:(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)\.){3}(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Local IP Address Outbound</td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbLocalIPAddressOutbound" MaxLength="50" CssClass="required" Width="170"  runat="server"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                                runat="server" ControlToValidate="uiTxtLocalIP" 
                                ErrorMessage="Please enter a valid Local IP Address." 
                                ValidationExpression="^(?:(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)\.){3}(?:1\d{0,2}|[3-9]\d?|2(?:[0-5]{1,2}|\d)?|0)$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td >
                            Local Port Outbound</td>
                        <td >
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxbLocalPortOutbound" Width="170" CssClass="number-required"  runat="server"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" FilterType="Numbers" 
                                TargetControlID="uiTxtLocalPort">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr id="trAction" runat="server">
                        <td valign="top">
                            <asp:Label ID="uiLblAction" runat="server" Text="Action"></asp:Label>
                        </td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtAction" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trApprovalDesc" runat="server">
                        <td valign="top">
                            <asp:Label ID="uiLblApprovalDescription" runat="server" Text="Approval Description"></asp:Label>
                        </td>
                        <td valign="top">
                            :</td>
                        <td >
                            <asp:TextBox ID="uiTxtApporvalDesc" CssClass="required" MaxLength="100" runat="server" Height="78px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                runat="server" ControlToValidate="uiTxtApporvalDesc" 
                                ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;</td>
                        <td valign="top">
                            &nbsp;</td>
                        <td >
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

