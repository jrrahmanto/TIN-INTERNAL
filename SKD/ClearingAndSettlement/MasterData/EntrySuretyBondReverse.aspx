<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntrySuretyBondReverse.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntrySuretyBondReverse"  %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlBankAccountSuretyBondLookup.ascx" tagname="CtlBankAccountSuretyBondLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlBondIssuerLookup.ascx" tagname="CtlBondIssuerLookup" tagprefix="uc3" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc5" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Reverse Surety Bond Amount</h1>
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
                            Business Date</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtBusDate" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox> 
                                                            
                    </td>
                </tr>
                <tr>
                    <td>
                            Exchange Ref</td>
                    <td>
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtExchangeRef" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Account Code</td>
                    <td>
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtAccCode" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Participant Name</td>
                    <td>
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtParticipantName" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Bond Serial Number</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtBondSN" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Bond Issuer Name</td>
                    <td>
                        :</td>
                    <td>
                            <asp:TextBox ID="uiTxtBondIssuerName" CssClass="required" runat="server" MaxLength="20" Width="160"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                           Usage Amount</td>
                    <td>
                        :</td>
                    <td>
                            <cc2:FilteredTextBox ID="ftbUsageAmount" FilterTextBox="Money" 
                CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                           Bond Remain Amount</td>
                    <td>
                        :</td>
                    <td>
                            <cc2:FilteredTextBox ID="ftbBondRemainAmount" FilterTextBox="Money" 
                CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                 <tr>
                    <td>
                            Default Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc4:CtlCalendarPickUp ID="uiDtpBuyerDefault" runat="server" /> 
                        
                        
                    </td>
                </tr>
                <%-- <tr>
                    <td>
                            Seller Default</td>
                    <td>
                        :</td>
                    <td>
                        <uc4:CtlCalendarPickUp ID="uiDtpSellerDefault" runat="server" /> 
                        
                    </td>
                </tr>--%>
                
                <tr>
                    <td style="width:100px">
                            Reverse Amount</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                            <asp:DropDownList ID="uiDdlReverseAmount" CssClass="required" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td style="width:100px">
                            Notes</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtNotes" CssClass="text" runat="server" Height="80px" MaxLength="100" Width="400px" 
                            TextMode="MultiLine"></asp:TextBox>
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


