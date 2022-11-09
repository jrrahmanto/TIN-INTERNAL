<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportTradeFeed.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ImportTradeFeed" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Import Trade Register</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
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
                                <td style="width: 100px">Exchange</td>
                                <td style="width: 10px">:</td>
                                <td>
                                    <asp:DropDownList ID="uiDdlExchange" CssClass="required" runat="server"
                                        DataSourceID="ObjectDataSourceExchangeDll" DataTextField="ExchangeCode"
                                        DataValueField="ExchangeId">
                                    </asp:DropDownList>
                                    <asp:ObjectDataSource ID="ObjectDataSourceExchangeDll" runat="server"
                                        SelectMethod="SelectExchange" TypeName="Exchange"></asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>Bussiness Date</td>
                                <td>:</td>
                                <td>
                                    <uc1:CtlCalendarPickUp ID="uiDtpBussDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>File Name</td>
                                <td>:</td>
                                <td>
                                    <asp:FileUpload ID="uiUploadFile" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_import"
                                        OnClick="uiBtnUpload_Click" Text="     Upload" />
                                    &nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"
                                        Text="     Cancel" OnClick="uiBtnCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="uiTxbLog" runat="server" Height="298px" TextMode="MultiLine"
                                        Width="780px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdateProgress runat="server" DisplayAfter="0" ID="UpdateProgress1">
                                        <ProgressTemplate>
                                            <img alt="" src="../../Images/ajax-loader2.gif"
                                                style="width: 220px; height: 19px" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

