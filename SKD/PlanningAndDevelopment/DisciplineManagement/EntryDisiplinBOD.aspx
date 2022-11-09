<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryDisiplinBOD.aspx.cs" Inherits="WebUI_New_EntryDisiplinBOD" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlCMExchangeLookup.ascx" tagname="CtlCMExchangeLookup" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table style="width:100%;">
    <tr>
        <td>
          <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td class="form-content-menu" colspan="3">
                            <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Dicipline No</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxbDiciplineNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Name</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                        <asp:DropDownList ID="uiDdlName" runat="server" 
                            DataSourceID="ObjectDataSourceBODName" DataTextField="Name" 
                            DataValueField="BODID">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ObjectDataSourceBODName" runat="server" 
                            SelectMethod="FillDdl" TypeName="BOD"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Description</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxbDesc" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Action</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxbAction" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Approval Description</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxbApprovalDesc" runat="server" Height="55px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            &nbsp;</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="Button3_Click"  />
&nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                            onclick="uiBtnCancel_Click" />
                    &nbsp;<asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                            onclick="uiBtnDelete_Click" Text="     Delete" />
                            <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
&nbsp;<asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                            onclick="uiBtnApprove_Click" Text="     Approve" />
&nbsp;<asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" onclick="uiBtnReject_Click" 
                            Text="     Reject" />
                    </td>
                </tr>
                </table>
</asp:Content>

