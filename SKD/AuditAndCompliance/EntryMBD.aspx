<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryMBD.aspx.cs" Inherits="AuditAndCompliance_EntryMBD" %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage MBD</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
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
                Clearing Member</td>
            <td>
                :</td>
            <td>
                <uc2:CtlClearingMemberLookup ID="uiCtlCm" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Effective Start Date</td>
            <td>
                :</td>
            <td>
                <uc1:CtlCalendarPickUp ID="uiDtpStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Effective End Date</td>
            <td>
                :</td>
            <td>
                <uc1:CtlCalendarPickUp ID="uiDtpEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Upload Date</td>
            <td>
                :</td>
            <td >
                <uc1:CtlCalendarPickUp ID="uiDtpUploadDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Fund</td>
            <td>
                :</td>
            <td >
                <asp:TextBox ID="uiTxbMBDFund" CssClass="number-required" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td >
                Value</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="uiTxbMBDValue" CssClass="number-required" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr id="TRApproval" runat="server">
            <td >
                <asp:Label ID="uiLblApprovalDesc" runat="server" Text="Approval Description"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="uiTxbApporvalDesc" runat="server" Height="78px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
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
        <tr>
            <td >
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click" />
                        <asp:Button ID="uiBtnDelete" runat="server" 
                    CssClass="button_delete"  onclick="uiBtnDelete_Click" 
                            Text="      Delete" Visible="False" />
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                            onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject" 
                                                    onclick="uiBtnReject_Click" />
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                            onclick="uiBtnCancel_Click"/>
            </td>
        </tr>
        </table>
        </div>
        </div>
         </td>
        </tr>
    </table>
</asp:Content>

