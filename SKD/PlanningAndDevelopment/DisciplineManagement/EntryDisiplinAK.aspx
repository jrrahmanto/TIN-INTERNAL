<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryDisiplinAK.aspx.cs" Inherits="WebUI_New_EntryDisiplinAK" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Entry Displin AK</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
                    <td  colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                            CM Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookupSanction" 
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Yang Memberikan Sanksi</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlSanctionSource" runat="server" Height="22px" 
                            Width="126px">
                            <asp:ListItem Value="BA">Bappebti</asp:ListItem>
                            <asp:ListItem Value="B">BBJ</asp:ListItem>
                            <asp:ListItem Value="K">KBI</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Penalty Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlPenaltyType" runat="server" Height="22px" 
                            Width="126px">
                            <asp:ListItem Value="MI">Minor</asp:ListItem>
                            <asp:ListItem Value="MJ">Major</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Sanction Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlSanctionType" runat="server" Height="22px" 
                            Width="126px">
                            <asp:ListItem>SP1</asp:ListItem>
                            <asp:ListItem>SP2</asp:ListItem>
                            <asp:ListItem>SP3</asp:ListItem>
                            <asp:ListItem Value="S">Suspend</asp:ListItem>
                            <asp:ListItem Value="F">Freeze</asp:ListItem>
                            <asp:ListItem Value="T">Terminate</asp:ListItem>
                            <asp:ListItem Value="FN">Fine</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Sanction Letter No.</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtSanctionNo" CssClass="required" MaxLength="100" Width="300px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Sanction Letter Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlCalendarPickUp ID="CtlCalendarSanctionDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlCalendarPickUp ID="CtlCalendarStartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            End Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlCalendarPickUp ID="CtlCalendarEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtDesc" runat="server" Height="52px" Width="380px" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxtDesc" ErrorMessage="Max. 500 Character" 
                                ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                            Fine Nominal</td>
                    <td>
                        :</td>
                    <td>
                        <cc2:FilteredTextBox ID="uitxtFineNominal" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Sanction Status</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlSanctionStatus" runat="server" Height="22px" 
                            Width="126px">
                            <asp:ListItem Value="A">Active</asp:ListItem>
                            <asp:ListItem Value="I">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                
                <tr id="trAction" runat="server">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbAction" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApproval" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbApprovalStatus" CssClass="required" runat="server" Height="100px"  Width="400px" 
                            TextMode="MultiLine"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxbApprovalStatus" ErrorMessage="Max. 100 Character" 
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
                            Text="     Delete" onclick="uiBtnDelete_Click" />
                            <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                            onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" 
                            onclick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                            onclick="uiBtnCancel_Click" CausesValidation="False" />
                    </td>
                </tr>
                </table>
            </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

