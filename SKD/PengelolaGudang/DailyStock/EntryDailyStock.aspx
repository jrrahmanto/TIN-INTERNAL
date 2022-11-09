<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryDailyStock.aspx.cs" Inherits="PengelolaGudang_DailyStock_EntryDailyStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ct1" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlCommodityLookup.ascx" TagName="CtlCommodityLookup" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Daily Stock</h1>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
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
                                <td>Commodity Code</td>
                                <td>:</td>
                                <td><uc2:CtlCommodityLookup ID="uiDtpCommID" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Warehouse Location</td>
                                <td>:</td>
                                <td><asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Volume</td>
                                <td>:</td>
                                <td><asp:TextBox ID="txtVolume" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" OnClick="uiBtnSave_Click" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel" OnClick="uiBtnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>