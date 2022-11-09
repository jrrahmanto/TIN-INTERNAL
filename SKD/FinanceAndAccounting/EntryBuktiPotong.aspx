<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryBuktiPotong.aspx.cs" Inherits="FinanceAndAccounting_EntryBuktiPotong"  %>


<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcTextBox" tagprefix="cc1" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc11" %>
<%@ Register src="../Controls/CtlYearMonthBuktiPotong.ascx" tagname="CtlYearMonthBuktiPotong" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage Bukti Potong</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     <tr >
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="red" 
                                Visible="False">
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
                            Business Date</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Clearing Member</td>
                        <td>
                            :</td>
                        <td>
                            <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tax Number</td>
                        <td>
                            :</td>
                        <td>
                            <cc1:FilteredTextBox ID="uiTxtTaxNo" MaxLength="20"  runat="server" ValidChar="0123456789"></cc1:FilteredTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tax</td>
                        <td>
                            :</td>
                        <td>
                            <cc1:FilteredTextBox ID="uiTxtTax" runat="server" FilterTextBox="Money" CssClass="number" ValidChar="0123456789.,"></cc1:FilteredTextBox>
                                                                    </td>
                    </tr>
                    <tr>
                        <td>
                            Period</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:CtlYearMonthBuktiPotong ID="CtlYearMonthBuktiPotong1" runat="server" />
                                                                    </td>
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
                            <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="100px" Width="400px" 
                                MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxtApprovalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" 
                                onclick="uiBtnSave_Click"  />
                                
                                    <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" Text="      Delete"
                                        OnClick="uiBtnDelete_Click" CausesValidation="False" />
                                    <cc11:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server"
                                        ConfirmText="Are you sure you want to delete?" Enabled="True" 
                                TargetControlID="uiBtnDelete">
                                    </cc11:ConfirmButtonExtender>
                                
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
                                
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                                onclick="uiBtnCancel_Click" CausesValidation="False"/>
                                
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

