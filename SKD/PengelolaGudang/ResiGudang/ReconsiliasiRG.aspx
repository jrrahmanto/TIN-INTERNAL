<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReconsiliasiRG.aspx.cs" Inherits="PengelolaGudang_ResiGudang_ReconsiliasiRG" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <h1>View Rekonsiliasi Resi Gudang</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td>Business Date</td>
                                    <td>:</td>
                                    <td><uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" OnClick="uiBtnSearch_Click" /> &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>