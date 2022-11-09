<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageBankAccount.aspx.cs" Inherits="FinanceAndAccounting_Parameter_EntryManageKBIAccount"  %>

<%@ Register src="../../Lookup/CtlBAClearingMemberLookup.ascx" tagname="CtlBAClearingMemberLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlBAInvestorLookup.ascx" tagname="CtlBAInvestorLookup" tagprefix="uc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Bank Account</h1>

<table cellpadding="1" cellspacing="1" style="width:100%;">
 <tr>
                    <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        
                        </td>
                       <td>
                           
                       </td>
                </tr>
    <tr>
        <td>
        <div class="shadow_view">
            <div class="box_view">
            <table class="table-row">
               
                <tr>
                    <td style="width:150px">
                            Bank Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlBankCode" CssClass="required" runat="server" 
                            DataSourceID="ObjectDataSourceBankId" DataTextField="Code" 
                            DataValueField="BankID">
                        </asp:DropDownList>
                                                            <asp:ObjectDataSource ID="ObjectDataSourceBankId" runat="server" 
                            SelectMethod="GetBank" TypeName="Bank"></asp:ObjectDataSource>
                                                            </td>
                </tr>
                <tr>
                    <td>
                            Account No</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAccountNo" CssClass="required" MaxLength="20" Width="160" runat="server"></asp:TextBox>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Account Type</td>
                    <td>
                        :</td>
                    <td>
                            <asp:DropDownList ID="uiDdlAccountType" CssClass="required" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="uiDdlAccountType_SelectedIndexChanged">
                                <asp:ListItem Value="RD">Rekening Deposit</asp:ListItem>
                                <asp:ListItem Value="RS">Rekening Settlement</asp:ListItem>
                                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                      Currency </td>
                    <td>
                        :</td>
                    <td>
                            <asp:DropDownList ID="uiDdlCurrency" runat="server">
                                <asp:ListItem Value="1">IDR</asp:ListItem>
                                <asp:ListItem Value="5">USD</asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trCM" runat="server">
                    <td>
                            Participant</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlBAClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                        <input type="hidden" name="hdnCMCode" value="" />
                    </td>
                </tr>
                <tr id="trInvestor" runat="server">
                    <td>
                            Investor</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlBAInvestorLookup ID="CtlInvestorLookup1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc3:CtlCalendarPickUp ID="CtlCalendarPickUpStartDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Account Status
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="uiDdlAccountStatus" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="R">Registered</asp:ListItem>
                            <asp:ListItem Value="A">Activated</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc3:CtlCalendarPickUp ID="CtlCalendarPickUpEndDate" runat="server" />
                    </td>
                </tr>
                 <tr id="trAction" runat="server">
                    <td>
                            <asp:Label ID="uiLblAction" runat="server" Text="Action"></asp:Label>
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            <asp:Label ID="uiLblApprovalDescription" runat="server" Text="Approval Description"></asp:Label>
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtApporvalDesc" MaxLength="100" runat="server" Height="100px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                runat="server" ControlToValidate="uiTxtApporvalDesc" 
                                ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
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

