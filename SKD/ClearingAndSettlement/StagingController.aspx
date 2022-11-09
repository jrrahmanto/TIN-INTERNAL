<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StagingController.aspx.cs" Inherits="ClearingAndSettlement_StagingController" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>All Command Staging BGR, TKS, BBJ</h1>
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
                                <td style="width: 25%">Update BST From BGR</td>
                                <td style="width: 1%"></td>
                                <td>
                                    <asp:Button ID="uiBtnBgr" CssClass="button_view" runat="server" Text="     Run" onclick="uiBtnBgr_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>Update BST From TKS</td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnTks" CssClass="button_view" runat="server" Text="     Run" onclick="uiBtnTks_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>Update Price From LME</td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnLme" CssClass="button_view" runat="server" Text="     Run" onclick="uiBtnLme_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>Update Price From KLTM</td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnKltm" CssClass="button_view" runat="server" Text="     Run" onclick="uiBtnKltm_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

