<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryRptFee.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_EntryRptFee" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Edit Report Fee</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td>
                <%--<asp:BulletedList Visible="true" ID="uiBlError" runat="server" ForeColor="Red"></asp:BulletedList>--%>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width: 100px">Tgl Bayar
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <uc3:CtlCalendarPickUp ID="uiDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                        OnClick="uiBtnSave_Click" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                        OnClick="uiBtnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

