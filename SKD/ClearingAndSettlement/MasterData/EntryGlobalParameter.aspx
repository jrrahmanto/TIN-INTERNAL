<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryGlobalParameter.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryGlobalParameter" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Global Parameter</h1>
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
                    <td style="width:150px">
                            Parameter Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtParamCode" runat="server" CssClass="required" MaxLength="50" Width="200"></asp:TextBox>
                                                            </td>
                </tr>
                <tr>
                    <td>
                            Effective Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
                                                            &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Numeric Value</td>
                    <td>
                        :</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtNumericValue" FilterTextBox="Money" CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Date Value</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarDateValue" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            String Value</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtStringValue" Height="80px" MaxLength="100" Width="300px" runat="server" 
                            TextMode="MultiLine"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxtStringValue" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr id="trAction" runat="server">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
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
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                            Text="      Cancel" onclick="uiBtnCancel_Click" CausesValidation="False" />
                    </td>
                </tr>
                </table>
                <div>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>


