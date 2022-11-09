<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryInvoiceFeeClearing.aspx.cs" Inherits="FinanceAndAccounting_Invoicing_EntryInvoiceFeeClearing" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberInvoiceLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Invoice Fee Clearing</h1>

    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                    <table class="table-row">
                        <tr>
                            <td>Clearing Member</td>
                            <td>:</td>
                            <td>
                                <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Invoice Date</td>
                            <td>:</td>
                            <td>
                                <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp2" runat="server" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td>Invoice No</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="uiTxtInvoiceNo" MaxLength = "50" Width="160" runat="server"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Nominal</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="uiTxtNominal" MaxLength="50" Width="160" runat="server"></asp:TextBox>&nbsp;
                            </td>
                        </tr>--%>
                        <tr>
                            <td>File Invoice</td>
                            <td>:</td>
                            <td>
                                <asp:FileUpload ID="uiUfInvoice" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>File Faktur Pajak</td>
                            <td>:</td>
                            <td>
                                <asp:FileUpload ID="uiUfFakturPajak" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" onclick="uiBtnSave_Click"  />
                                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel" onclick="uiBtnCancel_Click" CausesValidation="False" />
                            </td>
                        </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>