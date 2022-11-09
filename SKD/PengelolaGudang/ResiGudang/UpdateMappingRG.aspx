<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateMappingRG.aspx.cs" Inherits="PengelolaGudang_ResiGudang_UpdateMappingRG" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <h1>View Update Mapping Resi Gudang</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td>File Mapping RG</td>
                                    <td>:</td>
                                    <td>
                                        <asp:FileUpload ID="uiUfResiGudang" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="uiBtnDownload" CssClass="button_search" runat="server" Text="     Download" OnClick="uiBtnDownload_Click" /> &nbsp
                                        <asp:Button ID="uiBtnUpload" CssClass="button_search" runat="server" Text="     Upload" OnClick="uiBtnUpload_Click" /> &nbsp;
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