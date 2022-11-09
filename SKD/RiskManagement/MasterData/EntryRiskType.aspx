<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryRiskType.aspx.cs" Inherits="WebUI_New_EntryRiskType" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage RiskType</h1>
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
                            Risk Type Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtRiskTypeCode" CssClass="required" MaxLength="20" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Risk Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtRiskType" CssClass="required" MaxLength="20" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr visible="false" runat="server"> 
                    <td>
                            Effective Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarStartDate" runat="server" />
                    </td>
                </tr>
                <tr visible="false" runat="server">
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtDescription" runat="server" MaxLength="100" TextMode="MultiLine" Height="68px" 
                            Width="305px"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trAction" runat="server" visible="false">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server" visible="false">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" MaxLength="100" 
                            TextMode="MultiLine" Height="100px" 
                            Width="400px"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
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
                                Text="      Delete" onclick="uiBtnDelete_Click" 
                            CausesValidation="False" />
                                <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" Visible="False" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" Visible="False" />
                    
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                            Text="      Cancel" onclick="uiBtnCancel_Click" />
                    
                </tr>
                </table>
             </div>
             </div>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>

