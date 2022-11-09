<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryInvestor.aspx.cs" Inherits="WebUI_New_EntryInvestor" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlExchangeMemberLookup.ascx" tagname="CtlExchangeMemberLookup" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="0" cellspacing="0" style="width:100%;">
<tr>
                    <td colspan="3">
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
                            Account</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbInvestorCode" CssClass="required"  runat="server"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td>
                            ExchangeMember</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:CtlExchangeMemberLookup ID="CtlExchangeMemberLookup1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Inhouse Account</td>
                    <td>
                        :</td>
                    <td>
                        <asp:CheckBox ID="uiChlInhouseAcc" runat="server" />
                    </td>
                </tr>
                <tr id="TRAction" runat="server">
                    <td>
                            Action Flag</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbAction" runat="server" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td> Account Type</td>
                    <td> : </td>
                    <td>
                        <asp:DropDownList ID="uiDdlAccountType" runat="server" Enabled="false">
                            <asp:ListItem Value="R">Regular</asp:ListItem>
                            <asp:ListItem Value="S">Special</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td> Group Product</td>
                    <td> :</td>
                    <td>
                        <asp:TextBox ID="uiTxbGroupProduct" runat="server" MaxLength="20" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td> Investor Status</td>
                    <td> :</td>
                    <td>
                        <asp:TextBox ID="uiTxbInvStatus" runat="server" MaxLength="20" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr id="TRApproval" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxbApprovalDesc" CssClass="required" runat="server" Height="100px" 
                            TextMode="MultiLine" Width="400px"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxbApprovalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="     Save" onclick="uiBtnSave_Click" />
&nbsp;<asp:Button ID="uiBtnCancel" CssClass="button_cancel" runat="server" Text="      Cancel" 
                            onclick="uiBtnCancel_Click" />
                    &nbsp;<asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                            Text="     Delete" onclick="uiBtnDelete_Click" />
&nbsp;<asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                            onclick="uiBtnApprove_Click" />
&nbsp;<asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject" 
                            onclick="uiBtnReject_Click" />
                    </td>
                </tr>
                </table>
             </div>
             </div>
             </td>
             </tr>
             </table>
</asp:Content>


