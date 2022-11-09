<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageFee.aspx.cs" Inherits="FinanceAndAccounting_Parameter_EntryManageFee" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>
<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Fee</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
                    <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                </tr>
    <tr>
        <td>
        <div class="shadow_view" style="width:890px">
            <div class="box_view" style="width:885px">
            <table class="table-row" >
                <tr>
                    <td style="width:150px">
                            Commodity Code</td>
                    <td style="width:10px">
                        :</td>
                    <td colspan="4">
                        <uc1:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective Start Date</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <uc2:CtlCalendarPickUp ID="CtlCalendarStartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <uc2:CtlCalendarPickUp ID="CtlCalendarEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td style="text-align:center">
                        Clearing</td>
                    <td style="text-align:center">
                        Exchange</td>
                    <td style="text-align:center">
                        Compensation Fund</td>
                    <td style="text-align:center">
                        Third Party Fee</td>
                </tr>
                <tr>
                    <td>
                            Tender</td>
                    <td>
                        :</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtTNClearing" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtTNExchange" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtTNCompensationFund" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtTNThirdPartyFee" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Clearing Fund</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <cc2:FilteredTextBox ID="uiTxtClearingFund" CssClass="number" FilterTextBox="Money" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    &nbsp;</td>
                </tr>
                <tr id="trAction" runat="server">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="uiTxtApprovalDesc" runat="server" CssClass="required" 
                            Height="100px" MaxLength="100" Width="420px" 
                            TextMode="MultiLine"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                runat="server" ControlToValidate="uiTxtApprovalDesc" 
                                ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="4">
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

