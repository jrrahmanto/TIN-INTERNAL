<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryCollateral.aspx.cs" Inherits="WebUI_New_EntryCollateral" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Collateral</h1>
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
                            Clearing Member </td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Currency</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlCurrency" CssClass="required" runat="server" 
                            DataSourceID="ObjectDataSourceCurrency" DataTextField="CurrencyCode" 
                            DataValueField="CurrencyID">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ObjectDataSourceCurrency" runat="server" 
                            SelectMethod="GetCurrency" TypeName="Currency"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                            Lodgement No</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtLodgementNo" CssClass="required" MaxLength="18" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Lodgement Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarLodgementDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Collateral Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlCollateralType" runat="server" Width="126px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="C">Coverage Margin</asp:ListItem>
                            <asp:ListItem Value="S">Security Deposit</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Lodgement Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlLodgementType" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="D">Bank Deposit</asp:ListItem>
                            <asp:ListItem Value="S">Stock</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Issuer</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtIssuer" MaxLength="50" runat="server" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Issuer Type&nbsp;</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlIssuerType" runat="server">
                            <asp:ListItem Value="B">Bank</asp:ListItem>
                            <asp:ListItem Value="N">Non Bank</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Issuer Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarIssuerDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Maturity Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlCalendarPickUp ID="CtlCalendarMaturityDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Nominal</td>
                    <td>
                        :</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtNominal" FilterTextBox="Money" CssClass="number" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Haircut (in %)</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtHaircut" MaxLength="20" CssClass="number"  runat="server" AutoPostBack="true"
                            ontextchanged="uiTxtHaircut_TextChanged"></asp:TextBox>
                       <cc1:FilteredTextBoxExtender ID="uiTxtHaircut_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" 
                            TargetControlID="uiTxtHaircut" ValidChars="0123456789,.">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective Nominal</td>
                    <td>
                        :</td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtEffectiveNominal" runat="server" ReadOnly="true" FilterTextBox="Money" CssClass="number" ValidChar="0123456789.,-"></cc2:FilteredTextBox>    
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
                            TextMode="MultiLine"></asp:TextBox>
                                                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxtApprovalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                
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
                            Text="      Cancel" CausesValidation="False" onclick="uiBtnCancel_Click" />
                    </td>
                </tr>
                </table>
             </div>
             </div>
             </td>
             </tr>
             </table>
</asp:Content>

