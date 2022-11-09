<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageInterestRateDifferent.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryManageInterestRateDifferent" Title="Untitled Page" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td class="form-content-menu">
                            Commodity Code</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Effective Date</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="TextBox2">
                            </cc1:CalendarExtender>
                                                            </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            IRCA Value</td>
                    <td class="separator">
                     <td   class="right_search_criteria">
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            &nbsp;</td>
                    <td class="separator">
                        &nbsp;</td>
                    <td class="right_search_criteria">
                        <asp:Button ID="Button3" CssClass="button_save" runat="server" Text="      Save"  />
&nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" />
                    
                </tr>
                </table>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>

