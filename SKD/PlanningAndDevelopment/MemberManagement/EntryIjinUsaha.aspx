<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryIjinUsaha.aspx.cs" Inherits="WebUI_New_EntryIjinUsaha" %>
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
                            CM Code</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk" runat="server"></asp:TextBox>
                        &nbsp;<asp:Button ID="Button1" runat="server" Text="..." Width="26px" />
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Akte Pendirian Perusahaan</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                         <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" 
                            runat="server" 
                            TargetControlID="Panel4" 
                            CollapseControlID="Panel3"
                            ExpandControlID="Panel3"
                            Collapsed="true"
                            >
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="Panel3" runat="server" Height="0">
                            Akte Pendirian Perusahaan
                        </asp:Panel>
                        <asp:Panel ID="Panel4" runat="server">
                            <table cellpadding="1" cellspacing="1" style="width:100%;">
                                <tr>
                                    <td class="form-content-menu">
                                        Notary Name</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNamaNotarisAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        No. </td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNoAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Date</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtTglAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="uiTxtTglAktePendirianPerusahaan_CalendarExtender" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="uiTxtTglAktePendirianPerusahaan">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Legalization No.</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNoPengesahanAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Legalization Date</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtTglPengesahanAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Basis Asset</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtModalDasarAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Deposit Asset</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtModalDisetorAktePendirianPerusahaan" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Akte Perubahan Anggaran Dasar</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:LinkButton ID="uiBtnAktePerubahanAnggaranDasar" runat="server" 
                            onclick="uiBtnAktePerubahanAnggaranDasar_Click">Manage Akte Perubahan Anggaran Dasar</asp:LinkButton>
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Persetujuan Keanggotaan Bursa</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" 
                            runat="server" 
                            TargetControlID="Panel8" 
                            CollapseControlID="Panel7"
                            ExpandControlID="Panel7"
                            Collapsed="true"
                            >
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="Panel7" runat="server" Height="0">
                            Persetujuan Keanggotaan Bursa
                        </asp:Panel>
                        <asp:Panel ID="Panel8" runat="server">
                             <table cellpadding="1" cellspacing="1" style="width:100%;">
                                <tr>
                                    <td class="form-content-menu">
                                        No.</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNomorPersetujuanKeanggotaanBursa" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Date</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtTglPersetujuanKeanggotaanBursa" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="uiTxtTglPersetujuanKeanggotaanBursa_CalendarExtender" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="uiTxtTglPersetujuanKeanggotaanBursa">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Status</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:DropDownList ID="uiDdlStatusDiBursa" runat="server" Height="16px" 
                                            Width="140px">
                                            <asp:ListItem>Pemegang Saham</asp:ListItem>
                                            <asp:ListItem>Seat</asp:ListItem>
                                            <asp:ListItem>Non Seat</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>                           
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Persetujuan Bappebti</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" 
                            runat="server" 
                            TargetControlID="Panel10" 
                            CollapseControlID="Panel9"
                            ExpandControlID="Panel9"
                            Collapsed="true"
                            >
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="Panel9" runat="server" Height="0">
                            Persetujuan Bappebti
                        </asp:Panel>
                        <asp:Panel ID="Panel10" runat="server">
                              <table cellpadding="1" cellspacing="1" style="width:100%;">
                                <tr>
                                    <td class="form-content-menu">
                                        Permit No.</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNoIjinPenyelenggaraSpaPersetujuanBappebti" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="uiTxtNoIjinPenyelenggaraSpaPersetujuanBappebti_CalendarExtender" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="uiTxtNoIjinPenyelenggaraSpaPersetujuanBappebti">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Permit Date</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtTglIjinPenyelenggaraSpaPersetujuanBappebti" 
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        No. Ijin Peserta/Penyelenggara SPA</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtNoIjinPesertaSpaPersetujuanBappebti" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Tgl. Ijin Peserta/Penyelenggara SPA</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtTglIjinPesertaSpaPersetujuanBappebti" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Permit PALN</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtIjinPaln" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>
                           
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Persetujuan Keanggotaan Kliring</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" 
                            runat="server" 
                            TargetControlID="Panel2" 
                            CollapseControlID="Panel1"
                            ExpandControlID="Panel1"
                            Collapsed="true"
                            >
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="Panel1" runat="server" Height="0">
                            Persetujuan Keanggotaan Kliring
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server">
                              <table cellpadding="1" cellspacing="1" style="width:100%;">
                                <tr>
                                    <td class="form-content-menu">
                                        No. Perjanjian KBI-AK</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                  <tr>
                                      <td class="form-content-menu">
                                          Tgl. Perjanjian KBI-AK</td>
                                      <td class="separator">
                                          :</td>
                                      <td class="right_search_criteria">
                                          <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                      </td>
                                  </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Certificate No.</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Certificate Date</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" 
                                            runat="server" Enabled="True" 
                                            TargetControlID="uiTxtNoIjinPenyelenggaraSpaPersetujuanBappebti">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Laporan Akuntan Publik</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:LinkButton ID="uiBtnManageLaporanAkuntanPublik" runat="server" 
                            onclick="uiBtnManageLaporanAkuntanPublik_Click">Manage Laporan Akuntan Publik</asp:LinkButton>
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            NPWP</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" 
                            runat="server" 
                            TargetControlID="Panel6" 
                            CollapseControlID="Panel5"
                            ExpandControlID="Panel5"
                            Collapsed="true"
                            >
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="Panel5" runat="server" Height="0">
                            NPWP
                        </asp:Panel>
                        <asp:Panel ID="Panel6" runat="server">
                              <table cellpadding="1" cellspacing="1" style="width:100%;">
                                <tr>
                                    <td class="form-content-menu">
                                        NPWP No.</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                  <tr>
                                      <td class="form-content-menu">
                                          Name</td>
                                      <td class="separator">
                                          :</td>
                                      <td class="right_search_criteria">
                                          <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                      </td>
                                  </tr>
                                <tr>
                                    <td class="form-content-menu">
                                        Address</td>
                                    <td class="separator">
                                        :</td>
                                    <td class="right_search_criteria">
                                        <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                        </asp:Panel>
                        </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            PPKP</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                                        <asp:TextBox ID="uiTxtPpkp" runat="server"></asp:TextBox>
                                    </td>
                </tr>
                <tr>
                    <td class="form-content-menu">
                            Bank Account No.</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                                        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Manage Bank Account</asp:LinkButton>
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
                    </td>
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

