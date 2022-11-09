<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryExchangeRate.aspx.cs" Inherits="WebUI_New_EntyExchangeRate" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Exchange Rate</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
                        <td  colspan="3">
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
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
                            Source
                            Currency</td>
                       
                        <td style="width:10px">
                            :</td>
                       
                        <td>
                            <asp:DropDownList ID="uiDdlSourceCurrency" CssClass="required" runat="server" 
                                DataSourceID="ObjectDataSource1" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyID">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                SelectMethod="getCurrency" TypeName="ExchangeRate"></asp:ObjectDataSource>
                        </td>
                    </tr>
                <tr>
                        <td >
                            Destination Currency</td>
                       
                        <td>
                            :</td>
                       
                        <td>
                            <asp:DropDownList ID="uiDdlDestinationCurrency" CssClass="required" runat="server" 
                                DataSourceID="ObjectDataSource1" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                <tr>
                        <td >
                            Exchange Rate Type</td>
                       
                        <td>
                            :</td>
                       
                        <td>
                            <asp:DropDownList ID="uiDdlExchangeRate" CssClass="required" runat="server">
                                <asp:ListItem Value="B">BI</asp:ListItem>
                                <asp:ListItem Value="T">Tax</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                <tr>
                       
                        <td >
                            Effective Start Date</td>
                       
                        <td>
                            :</td>
                        <td >
                            <uc1:CtlCalendarPickUp ID="uiDtpStartDate" runat="server" />
                        </td>
                    </tr>
                <tr>
                       
                        <td >
                            Effective End Date</td>
                       
                        <td>
                            :</td>
                        <td >
                            <uc1:CtlCalendarPickUp ID="uiDtpEndDate" runat="server" />
                        </td>
                    </tr>
                <tr>
                       
                        <td >
                            Rate</td>
                       
                        <td>
                            :</td>
                        <td>
                            <cc2:FilteredTextBox ID="uiTxbRate" FilterTextBox="Money" runat="server" ValidChar="0123456789.,-" CssClass="number-required"></cc2:FilteredTextBox>
                        </td>
                    </tr>
                    
                    <tr id="TRAction" runat="server">
                        <td >
                            <asp:Label ID="uiLblAction" runat="server" Text="Action"></asp:Label>
                        </td>
                       
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="uiTxbAction" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    
                <tr id="TRApproval" runat="server">
                       
                        <td >
                            <asp:Label ID="uiLblApprovalDesc" runat="server" Text="Approval Description"></asp:Label>
                        </td>
                       
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="uiTxbApporvalDesc" CssClass="required" runat="server" Height="78px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxbApporvalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                    </tr>
                <tr>
                    <td >
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click" />
                        <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete"  onclick="uiBtnDelete_Click" 
                            Text="      Delete" />
                            <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                            onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject" 
                                                    onclick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                            onclick="uiBtnCancel_Click" CausesValidation="False"/>
                    </td>
                </tr>
                </table>
            <div></div>
            </td>
        </tr>
    </table>
            
</asp:Content>

