<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryClearingMember.aspx.cs" Inherits="WebUI_MemberManagement_EntryClearingMember"%>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>

<%@ Register src="../../Lookup/CtlContractLookup.ascx" tagname="CtlContractLookup" tagprefix="uc3" %>
<%@ Register src="../../Lookup/CtlCMExchangeLookup.ascx" tagname="CtlCMExchangeLookup" tagprefix="uc4" %>

<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc5" %>

<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
    
        <cc1:TabContainer CssClass="ajax__tab_lightblue-theme"  runat="server" 
            ID="uiTabsClearingMember" ActiveTabIndex="2">
            <cc1:TabPanel   runat="server" ID="uiPnlClearingMember" HeaderText="Clearing Member">
                <HeaderTemplate>
                    Participant
               </HeaderTemplate>
                

<ContentTemplate>
<table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr>
                    <td colspan="3">
                    <asp:BulletedList ID="uiBlCMError" runat="server" ForeColor="Red">
                        </asp:BulletedList>
                    </td>
                </tr>
                <tr>
                <td>
                <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                    <tr>
                    <td>
                        Participant Code</td><td >:</td><td ><asp:TextBox ID="uiTxbCMCode" runat="server" MaxLength="5"></asp:TextBox>
                        </td></tr><tr><td>
                    Participant Name</td><td >:</td><td >
                    <asp:TextBox ID="uiTxbCMName" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                    </td></tr><tr><td>
                    Effective Start Date</td><td >:</td><td >
                    <uc2:CtlCalendarPickUp ID="uiDtpStartDate" runat="server" />
                    </td></tr><tr><td>
                        Effective End Date</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDtpEndDate" runat="server" />
                        </td></tr><tr><td>
                        PPKP</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbPPKP" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td></tr><tr><td>
                        Website</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbWebsite" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td></tr><tr><td>
                        Status</td><td >:</td><td >
                        <asp:DropDownList ID="uiDdlStatus" runat="server" Enabled="False">
                            <asp:ListItem Value="N">Normal</asp:ListItem>
                            <asp:ListItem Value="F">Freeze</asp:ListItem>
                            <asp:ListItem Value="S">Suspend</asp:ListItem>
                            <asp:ListItem Value="T">Terminate</asp:ListItem>
                            <asp:ListItem Value="V">Verified</asp:ListItem>
                        </asp:DropDownList>
                        </td></tr><tr><td>
                        Agreement No</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbAgreementNo" runat="server" MaxLength="100" Width="350px"></asp:TextBox>
                        </td></tr><tr><td>
                        Agreement Date</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDTPAgreementDate" runat="server" />
                        </td></tr><tr><td>
                        Exchange Status</td><td >:</td><td >
                        <asp:DropDownList ID="uiDdlExchangeStatus" runat="server">
                            <asp:ListItem Value="P">Pemegang Saham</asp:ListItem>
                            <asp:ListItem Value="S">Seat</asp:ListItem>
                            <asp:ListItem Value="N">Non Seat</asp:ListItem>
                        </asp:DropDownList>
                        </td></tr><tr><td>
                        Certificate No</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbCertificateNO" runat="server" MaxLength="100" 
                            Width="350px"></asp:TextBox>
                        </td></tr><tr><td>
                        Certificate Date</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDtpCertificateDate" runat="server" />
                        </td></tr><tr><td>
                        COFFTRA License No.</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbSpaTraderNo" runat="server" Width="354px" MaxLength="100" ></asp:TextBox>
                        </td></tr><tr><td>
                        COFFTRA License Date</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDtpSPATraderDate" runat="server" />
                        </td></tr><tr><td>
                        SPA License No.</td><td >:</td><td >
                        <asp:TextBox ID="uiTxbSpaBrokerNo" runat="server" Width="354px"  MaxLength="100"></asp:TextBox>
                        </td></tr><tr><td>
                        SPA License Date</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDtpSPABrokerDate" runat="server" />
                        </td></tr><tr><td>
                        PALN License No</td><td >:</td><td >
                        <asp:TextBox ID="uiTXbPALNLicense" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                        </td></tr><tr><td>
                        Company Anniversary</td><td >:</td><td >
                        <uc2:CtlCalendarPickUp ID="uiDtpAnniversary" runat="server" />
                        </td></tr><tr><td>
                        Company Logo</td><td >:</td><td >
                        <asp:FileUpload ID="uiUploadFileLogo" runat="server" Width="214px" />
                        &nbsp;<asp:Button ID="uiDownloadLogo" runat="server" OnClick="uiDownloadLogo_Click" 
                            Text="Download Logo" />
                        </td></tr><tr><td>
                        Initial Margin Multiplier</td><td >:</td><td >
                        <cc2:FilteredTextBox ID="uiTxbCMInitialMarginMultiplier" runat="server" 
                            FilterInterval="250" CssClass="number" FilterMode="ValidChars" FilterTextBox="Money" 
                            FilterType="Custom" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                        
                        </td></tr>
                    <tr>
                        <td>
                            Min Req Initial Margin IDR</td>
                        <td >
                            :</td>
                        <td >
                            <cc2:FilteredTextBox ID="uiTxbMinReqInitialMarginIDR" runat="server" 
                                 ValidChar="0123456789.,-" CssClass="number" FilterTextBox="Money" FilterInterval="250" 
                                FilterMode="ValidChars" FilterType="Custom"></cc2:FilteredTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Min Req Initial Margin USD</td>
                        <td >
                            :</td>
                        <td >
                            <cc2:FilteredTextBox ID="uiTxbMinReqInitialMarginUSD" runat="server" 
                                FilterInterval="250" FilterMode="ValidChars" CssClass="number" FilterTextBox="Money" 
                                FilterType="Custom" ValidChar="01234567890.,-"></cc2:FilteredTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtAddress" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtEmail" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <!--<tr>
                        <td>
                            Region</td>
                        <td >
                            :</td>
                        <td>
                            <asp:DropDownList ID ="uiDdlRegion" runat ="server" DataTextField ="Code" DataValueField ="RegionID"
                                 Width="350px" AppendDataBoundItems="True">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                    </tr>-->
                    <tr>
                        <td>
                            Phone Number</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtPhoneNumber" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person Name</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtContactPerson" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person Number</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtContactPhone" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Account Number</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtCMAccNo" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Account Name</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtCMAccNm" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           Bank Name</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtCMBankName" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Registration Date</td>
                        <td >
                            :</td>
                        <td>
                            <uc2:CtlCalendarPickUp ID="uiDtpRegDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Domisili</td>
                        <td >
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtDomisili" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No Akta Pendiri
                        </td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnAktaPendiri" runat="server" OnClick="uiBtnAktaPendiri_Click" Text="Download"/>&nbsp;
                            <asp:Button ID="uiBtnAktaPendiri2" runat="server" OnClick="uiBtnAktaPendiri2_Click" Text="Delete"/>&nbsp;
                            <asp:Label ID="uiLblAktaPendiri" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No Akta Perubahan
                        </td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnPerubahan" runat="server" OnClick="uiBtnPerubahan_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnPerubahan2" runat="server" OnClick="uiBtnPerubahan2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblPerubahan" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Domisili Perusahaan
                        </td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnDomisili" runat="server" OnClick="uiBtnDomisili_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnDomisili2" runat="server" OnClick="uiBtnDomisili2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblDomisili" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            NPWP</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnNpwp" runat="server" OnClick="uiBtnNpwp_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnNpwp2" runat="server" OnClick="uiBtnNpwp2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblNpwp" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Identitas Kepabeaan</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnKepabeaan" runat="server" OnClick="uiBtnKepabeaan_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnKepabeaan2" runat="server" OnClick="uiBtnKepabeaan2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblKepabeaan" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Eksportir Terdaftar Timah</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnTerdaftar" runat="server" OnClick="uiBtnTerdaftar_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnTerdaftar2" runat="server" OnClick="uiBtnTerdaftar2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblTerdaftar" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Perizinan Instansi Eksportir</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnEksportir" runat="server" OnClick="uiBtnEksportir_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnEksportir2" runat="server" OnClick="uiBtnEksportir2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblEksportir" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SIUP</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnSiup" runat="server" OnClick="uiBtnSiup_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnSiup2" runat="server" OnClick="uiBtnSiup2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblSiup" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            NIB</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnNib" runat="server" OnClick="uiBtnNib_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnNib2" runat="server" OnClick="uiBtnNib2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblNib" runat="server" Visible="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Identitas Diri Pengurus</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnIdentitas" runat="server" OnClick="uiBtnIdentitas_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnIdentitas2" runat="server" OnClick="uiBtnIdentitas2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblIdentitas" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Laporan Keuangan</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnKeuangan" runat="server" OnClick="uiBtnKeuangan_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnKeuangan2" runat="server" OnClick="uiBtnKeuangan2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblKeuangan" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Surat Referensi Bank Luar Negeri</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnBankLuar" runat="server" OnClick="uiBtnBankLuar_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnBankLuar2" runat="server" OnClick="uiBtnBankLuar2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblBankLuar" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Company Profile</td>
                        <td >
                            :</td>
                        <td>
                            <asp:Button ID="uiBtnCProfile" runat="server" OnClick="uiBtnCProfile_Click" Text="Download" />&nbsp;
                            <asp:Button ID="uiBtnCProfile2" runat="server" OnClick="uiBtnCProfile2_Click" Text="Delete" />&nbsp;
                            <asp:Label ID="uiLblCProfile" runat="server" Visible="false"/>
                        </td>
                    </tr>
                    <tr id="trAction" runat="server">
                        <td runat="server">
                            Action
                        </td>
                        <td runat="server" >
                            :
                        </td>
                        <td runat="server" >
                            <asp:TextBox ID="uiTXbAction" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trApprovalDesc" runat="server">
                        <td runat="server">
                            Approval Desc.
                        </td>
                        <td  runat="server">
                            :
                        </td>
                        <td  runat="server">
                            <asp:TextBox ID="uitxbApprovalDesc" runat="server" Width="400px" Height="100px" 
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="uitxbApprovalDesc"
                                ErrorMessage="Max. 100 char" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
                            <asp:Button ID="uiBtnSave" runat="server" CssClass="button_save" 
                                OnClick="uiBtnSave_Click" Text="      Save" />
                            <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                                OnClick="uiBtnDelete_Click" Text="Delete" />
                            <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                OnClick="uiBtnApprove_Click" Text="Approve" />
                            <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                OnClick="uiBtnReject_Click" Text="Reject" />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                                OnClick="uiBtnCancel_Click" Text="      Cancel" />
                        </td>
                    </tr>
                    </table>
                 </div>  
                 </div>
                 </td>
                    </tr>
                    </table>    
            </ContentTemplate>
            
            
            
            

</cc1:TabPanel>
            <cc1:TabPanel runat="server" ID="uiPnlManagementStructure" HeaderText="Management Structure">
                <HeaderTemplate>
                    Management Structure
            
            
            </HeaderTemplate>
                

<ContentTemplate>
                    <table cellpadding="1" cellspacing="1" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:BulletedList ID="uiBlBODError" runat="server" ForeColor="Red" Visible="False">
                                </asp:BulletedList>
                                
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <div class="shadow_view">
                        <div class="box_view">
                        <table class="table-row">
                        <tr >
                            <td style="width:150px">
                                Name</td>
                            <td style="width:10px">
                                :
                            </td>
                            <td >
                                <asp:TextBox ID="uiTxbBODSearchName" runat="server" Width="207px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr >
                            <td>
                                Board Position
                            </td>
                            <td >
                                :
                            </td>
                            <td >
                                <asp:DropDownList ID="uiDdlBODSearchPosition" runat="server" Height="22px" 
                                    Width="127px">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="CE">Komisaris Utama</asp:ListItem>
                                    <asp:ListItem Value="CO">Komisaris</asp:ListItem>
                                    <asp:ListItem Value="PD">Direktur Utama</asp:ListItem>
                                    <asp:ListItem Value="DR">Direktur</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr >
                            <td>
                                Aproval Status
                            </td>
                            <td >
                                :
                            </td>
                            <td >
                                <asp:DropDownList ID="uiDdlBODhApprovalStatus" runat="server">
                                    <asp:ListItem Value="A">Approved</asp:ListItem>
                                    <asp:ListItem Value="P">Proposed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr >
                            <td>
                                &nbsp;
                            </td>
                            <td >
                                &nbsp;
                            </td>
                            <td >
                                <asp:Button ID="uiBtnSearchBOD" runat="server" CssClass="button_search" 
                                    OnClick="uiBtnSearchBOD_Click" Text="     Search" />
                                &nbsp;</td>
                        </tr>
                    </table>
                        </div>
                        </div>
                        </td>
                        </tr>
                    </table>
                
                    <table class="table-datagrid">
                    <tr><td>
                        <asp:Button ID="uiBtnCreateBOD" runat="server" CssClass="button_create" 
                            OnClick="uiBtnCreateBOD_Click" Text="    Create" />
                        </td></tr>
                        <tr>
                            <td>
                                <asp:GridView ID="uiDGBOD" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="BODNo" 
                                    EmptyDataText="No Record" MouseHoverRowHighlightEnabled="True" 
                                    OnPageIndexChanging="uiDGBOD_PageIndexChanging" 
                                    OnRowCommand="uiDGBOD_RowCommand" OnSorting="uiDGBOD_Sorting" 
                                    RowHighlightColor="" Width="100%">
                                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    <Columns>
                                        <asp:CommandField SelectText="Edit" ShowSelectButton="True" />
                                        <asp:BoundField DataField="BODNo" HeaderText="BOD No" ReadOnly="True" 
                                            SortExpression="BODNo">
                                            <HeaderStyle ForeColor="White" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                            <HeaderStyle ForeColor="White" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IDNO" HeaderText="ID No." SortExpression="IDNO">
                                            <HeaderStyle ForeColor="White" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BoardPositionDesc" HeaderText="Board Position" 
                                            SortExpression="BoardPositionDesc">
                                            <HeaderStyle ForeColor="White" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" 
                                            SortExpression="ApprovalStatusDesc" />
                                        <asp:TemplateField Visible="False" HeaderText="BODID" >
                                            <ItemTemplate>
                                            <asp:Label ID="uiLblBODID" runat="server" Text='<%# Bind("BODID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                    <RowStyle CssClass="tblRowStyle" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ObjectDataSource ID="odsBOD" runat="server" 
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataBySearchCriteria" 
                                    TypeName="BODDataTableAdapters.BODTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="uiHiddBODCMID" Name="CMID" 
                                            PropertyName="Value" Type="Decimal" />
                                        <asp:ControlParameter ControlID="uiTxbBODSearchName" Name="name" 
                                            PropertyName="Text" Type="String" />
                                        <asp:ControlParameter ControlID="uiDdlBODSearchPosition" Name="boardPosition" 
                                            PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="uiDdlBODhApprovalStatus" Name="approvalStatus" 
                                            PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </table>
                    
                    <table ID="pnlEditMgt" runat="server" cellpadding="1" cellspacing="1" 
                        style="width:100%;">
                        <tr runat="server">
                        <td runat="server">
                        <div class="shadow_view">
                        <div class="box_view">
                        <table class="table-row">
                        <tr runat="server">
                            <td runat="server" >
                                CM Code
                            </td>
                            <td runat="server" >
                                &nbsp;
                            </td>
                            <td runat="server" >
                                <asp:Label ID="uiLblBODCMCode" runat="server"></asp:Label>
                                <asp:HiddenField ID="uiHiddBODCMID" runat="server" />
                                <asp:HiddenField ID="uiHiddBODno" runat="server" />
                                <asp:HiddenField ID="uiHiddAppStatus" runat="server" />
                                <asp:HiddenField ID="uiHiddenBODStatus" runat="server" />
                                <asp:HiddenField ID="uiHiddenBODID" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                BOD No
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server" >
                                <asp:TextBox ID="uiTxbBODNo" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Name
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server" >
                                <asp:TextBox ID="uiTxbBODName" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Date of Birth
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server" >
                                <uc2:CtlCalendarPickUp ID="uiDtpBODDOB" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Place of Birth
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server" >
                                <asp:TextBox ID="uiTxbPlaceOfBirth" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Address
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODAddress" runat="server" Height="50px" 
                                    TextMode="MultiLine" Width="350px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                                    ControlToValidate="uiTxbBODAddress" ErrorMessage="Max. 100 char" 
                                    ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                City
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODCity" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Province
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODProvince" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Postal Code
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbPostalCode" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                ID No.
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbIDNo" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Mobile Phone No.
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODMobilePhoneNo" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Direct
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODDirect" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Email
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODEmail" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Position
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlBODPosition" runat="server" Height="22px" 
                                    Width="127px">
                                    <asp:ListItem Value="CE">Komisaris Utama</asp:ListItem>
                                    <asp:ListItem Value="CO">Komisaris</asp:ListItem>
                                    <asp:ListItem Value="PD">Direktur Utama</asp:ListItem>
                                    <asp:ListItem Value="DR">Direktur</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Last Education
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbLastEducation" runat="server" MaxLength="50" 
                                    Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Work Experience
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODWorkExperience" runat="server" 
                                    Width="400px" Height="125px" TextMode="MultiLine"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" 
                                    ControlToValidate="uiTxbBODWorkExperience" ErrorMessage="Max. 500 char" 
                                    ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Signature Check Authorization
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:CheckBox ID="uiChkSignatureAuthor" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Signature Management Speciment
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:FileUpload ID="uiUploadSignatureMgm" runat="server" />
                                &nbsp;<asp:Button ID="uiBtnBODDownloadSigPic" runat="server" Enabled="False" 
                                    Text="Download Picture" onclick="uiBtnBODDownloadSigPic_Click" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Certificate No.
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODCertificateNo" runat="server" MaxLength="50" 
                                    Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Certificate Date
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpBODCertificateDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Management Picture
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:FileUpload ID="uiUploadMgmPic" runat="server" />
                                &nbsp;<asp:Button ID="uiBtnBODDownloadMgmPic" runat="server" Enabled="False" 
                                    Text="Download Picture" onclick="uiBtnBODDownloadMgmPic_Click" />
                                <table style="width: 100%;">
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Effective Start Date
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpBODStartDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                Effective End Date
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpBODEndDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" ID="trActionBOD">
                            <td runat="server" >
                                Action
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODAction" runat="server" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr ID="trApprovalDescBOD" runat="server">
                            <td runat="server" >
                                Approval Description
                            </td>
                            <td runat="server" >
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbBODApprovalDesc" runat="server" Height="75px" 
                                    TextMode="MultiLine" Width="350px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="uiTxbBODApprovalDesc" ErrorMessage="Max. 100 char" 
                                    ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" >
                                &nbsp;
                            </td>
                            <td runat="server" >
                                &nbsp;
                            </td>
                            <td runat="server">
                                <asp:Button ID="uiBtnSaveBOD" runat="server" CssClass="button_save" 
                                    OnClick="uiBtnSaveBOD_Click" Text="      Save" />
                                <asp:Button ID="uiBtnDeleteBOD" runat="server" CssClass="button_delete" 
                                    OnClick="uiBtnBODDelete_Click" Text="    Delete" />
                                <asp:Button ID="uiBtnApproveBOD" runat="server" CssClass="button_approve" 
                                    OnClick="uiBtnBODApprove_Click" Text="    Approve" />
                                <asp:Button ID="uiBtnRejectBOD" runat="server" CssClass="button_reject" 
                                    OnClick="uiBtnBODReject_Click" Text="    Reject" />
                                <asp:Button ID="uiBtnCancelBOD" runat="server" CssClass="button_cancel" 
                                    OnClick="uiBtnCancelBOD_Click" Text="      Cancel" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                        </div>
                        </div>
                        </td>
                        </tr>
                    </table>
            </ContentTemplate>



</cc1:TabPanel>
            <cc1:TabPanel runat="server" ID="uiPanelCluster" HeaderText="Cluster">
                
                <HeaderTemplate>
                    Cluster
                
                
            </HeaderTemplate>
                
                
<ContentTemplate>
                    <table cellpadding="1" cellspacing="1" style="width:100%;">
                        <tr>
                            <td>
                                <asp:BulletedList ID="uiBlProductError" runat="server" ForeColor="Red">
                                </asp:BulletedList>
                            </td>
                        </tr>
                        <tr>
                        <td>
                         <div class="shadow_view">
                        <div class="box_view">
                             <table class="table-row">
                        <tr>
                            <td style="width:150px">
                                Exchange</td>
                            <td style="width:10px">
                                :</td>
                            <td>
                                <asp:DropDownList ID="uiDdlProductExchange" runat="server" 
                                    DataTextField="ExchangeCode" DataValueField="ExchangeID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Approval Status</td>
                            <td >
                                :</td>
                            <td>
                                <asp:DropDownList ID="uiDdlProductApprovalStatus" runat="server">
                                    <asp:ListItem Value="A">Approved</asp:ListItem>
                                    <asp:ListItem Value="P">Proposed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td >
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="uiBtnProductSearch" runat="server" CssClass="button_search" 
                                    OnClick="uiBtnProductSearch_Click" Text="     Search" />
                                &nbsp;</td>
                        </tr>
                    </table>
                        </div>
                        </div>
                    </td>
                    </tr>
                    </table>
                
                
                    <table class="table-datagrid">
                        <tr>
                            <td>
                                <asp:Button ID="uiBtnCreateProduct" runat="server" CssClass="button_create" 
                                    OnClick="uiBtnCreateProduct_Click" Text="     Create" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="uiDGProduct" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ClusterID" 
                                    EmptyDataText="No Record" MouseHoverRowHighlightEnabled="True" 
                                    OnPageIndexChanging="uiDGProduct_PageIndexChanging" 
                                    OnRowCommand="uiDGProduct_RowCommand" OnSorting="uiDGProduct_Sorting" 
                                    RowHighlightColor="" Width="100%" EnableModelValidation="True">
                                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    <Columns>
                                        <asp:ButtonField CommandName="Select" Text="edit" />
                                        <asp:BoundField DataField="TraderCMCode" HeaderText="Trader" 
                                            SortExpression="TraderCMCode" />
                                        <asp:BoundField DataField="CommodityCode" HeaderText="Commodity" ReadOnly="True" 
                                            SortExpression="CommodityCode" />
                                        <asp:BoundField DataField="BrokerCMCode" HeaderText="Broker Clearing Member" 
                                            ReadOnly="True" SortExpression="BrokerCMCode" />
                                        <asp:BoundField DataField="EffectiveStartDate" 
                                            DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Effective Start Date" 
                                            ReadOnly="True" SortExpression="EffectiveStartDate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" 
                                            SortExpression="ApprovalStatusDesc">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                    <RowStyle CssClass="tblRowStyle" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsProduct" runat="server" 
                                    OldValuesParameterFormatString="original_{0}" 
                                    SelectMethod="GetDataBySearchCriteria" 
                                    TypeName="ClusterDataTableAdapters.ClusterSearchTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="uiHiddenProductTraderCMID" Name="CMID" 
                                            PropertyName="Value" Type="Decimal" />
                                        <asp:ControlParameter ControlID="uiDdlProductApprovalStatus" 
                                            Name="approvalStatus" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="uiDdlProductExchange" Name="ExchangeID" 
                                            PropertyName="SelectedValue" Type="Decimal" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </table>
                    
                    <table ID="pnlEditProduct" runat="server" cellpadding="1" cellspacing="1" 
                        style="width:100%;">
                        <tr runat="server">
                            <td runat="server">
                            <div class="shadow_view">
                            <div class="box_view">
                                <table class="table-row">
                                    <tr>
                                        <td style="width:150px">
                                            Trader</td>
                                        <td style="width:10px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="uiLblProductCMCode" runat="server"></asp:Label>
                                            <asp:Label ID="uiLblTraderCMID" runat="server" Text="Label" Visible="False"></asp:Label>
                                            <asp:HiddenField ID="uiHiddenProductTraderCMID" runat="server" />
                                            <asp:HiddenField ID="uiHiddenProductClusterID" runat="server" />
                                            <asp:HiddenField ID="uiHiddenProductStatus" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Commodity</td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            
                                            
                                            <uc6:CtlCommodityLookup ID="CtlContractCommodityLookup1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Broker
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <uc1:CtlClearingMemberLookup ID="CtlCMBroker" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Effective StartDate
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <uc2:CtlCalendarPickUp ID="uiDtpProductStartDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Effective End Date
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <uc2:CtlCalendarPickUp ID="uiDtpProductEndDate" runat="server" />
                                        </td>
                                    </tr>
                                    <tr ID="trActionProduct" runat="server">
                                        <td runat="server">
                                            Action Flag
                                        </td>
                                        <td runat="server">
                                            :
                                        </td>
                                        <td runat="server">
                                            <asp:TextBox ID="uiTxbProductActionFlag" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr ID="trApprovalDescProduct" runat="server">
                                        <td runat="server">
                                            Approval Description
                                        </td>
                                        <td runat="server">
                                            :
                                        </td>
                                        <td runat="server">
                                            <asp:TextBox ID="uiTxbProductApprovalDesc" runat="server" Height="75px" 
                                                TextMode="MultiLine" Width="350px"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                ControlToValidate="uiTxbProductApprovalDesc" ErrorMessage="Max. 100 char" 
                                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="uiBtnSaveProduct" runat="server" CssClass="button_save" 
                                                OnClick="uiBtnSaveProduct_Click" Text="      Save" />
                                            <asp:Button ID="uiBtnDeleteProduct" runat="server" CssClass="button_delete" 
                                                OnClick="uiBtnProductDelete_Click" Text="     Delete" />
                                            <asp:Button ID="uiBtnApproveProduct" runat="server" CssClass="button_approve" 
                                                OnClick="uiBtnProductApprove_Click" Text="Approve" />
                                            <asp:Button ID="uiBtnRejectProduct" runat="server" CssClass="button_reject" 
                                                OnClick="uiBtnProductReject_Click" Text="     Reject" />
                                            <asp:Button ID="uiBtnCancelProduct" runat="server" CssClass="button_cancel" 
                                                OnClick="uiBtnCancelProduct_Click" Text="      Cancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </div>
                            </td>
                        </tr>
                    </table>
                
                
            </ContentTemplate>
            

</cc1:TabPanel>

            <cc1:TabPanel runat="server" ID="uiPanelExchange" HeaderText="Exchange">
                <ContentTemplate>
                    <table cellpadding="1" cellspacing="1" style="width:100%;">
                        <tr>
                            <td colspan="3">
                                <asp:BulletedList ID="uiBlCMExchError" runat="server" ForeColor="Red" Visible="False">
                                </asp:BulletedList>
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <div class="shadow_view">
                        <div class="box_view">
                        <table class="table-row">
                        <tr>
                            <td style="width:150px">
                                Approval Status</td>
                            <td style="width:10px">
                                :</td>
                            <td>
                                <asp:DropDownList ID="uiDdlCMExchApprovalStatus" runat="server">
                                    <asp:ListItem Value="A">Approved</asp:ListItem>
                                    <asp:ListItem Value="P">Proposed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                &nbsp;</td>
                            <td>
                                :</td>
                            <td>
                                <asp:Button ID="uiBtnSearchCMExchange" runat="server" CssClass="button_search" 
                                    OnClick="uiBtnSearchCMExchange_Click" Text="      Search" />
                                &nbsp;</td>
                        </tr>
                    </table>
                        </div>
                        </div>
                        </td>
                        </tr>
                        </table>
                
                
                    <table class="table-datagrid">
                        <tr>
                            <td>
                                <asp:Button ID="uiBtnCreateCMExchange" runat="server" CssClass="button_create" 
                                    OnClick="uiBtnCreateCMExchange_Click" Text="Create" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="uiDgCMExchMember" runat="server" AllowPaging="True" 
                                    AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="CMExchangeId" 
                                    EmptyDataText="No Record" MouseHoverRowHighlightEnabled="True" 
                                    OnPageIndexChanging="uiDgCMExchMember_PageIndexChanging" 
                                    OnRowCommand="Ecgridview8_RowCommand" OnSorting="uiDgCMExchMember_Sorting" 
                                    RowHighlightColor="" Width="100%">
                                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    <Columns>
                                        <asp:ButtonField CommandName="Select" Text="edit" />
                                        <asp:TemplateField HeaderText="CMExchangeID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="uiLblCMExchangeID" runat="server" 
                                                    Text='<%# Bind("CMExchangeID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CMExchangeCode" HeaderText="Exchange Code" 
                                            SortExpression="CMExchangeCode" />
                                        <asp:BoundField DataField="CMTypeDesc" HeaderText="Membership Type" 
                                            SortExpression="CMTypeDesc" />
                                        <asp:BoundField DataField="EffectiveStartDate" 
                                            DataFormatString="{0:dd-MMM-yyyy}" HeaderText="EffectiveStartDate" 
                                            ReadOnly="True" SortExpression="EffectiveStartDate" />
                                        <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" 
                                            SortExpression="ApprovalStatusDesc" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        &nbsp;
                                    </EmptyDataTemplate>
                                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                    <RowStyle CssClass="tblRowStyle" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsCMExchange" runat="server" 
                                    OldValuesParameterFormatString="original_{0}" 
                                    SelectMethod="GetDataByCMIDApproval" 
                                    TypeName="ClearingMemberExchangeDataTableAdapters.ClearingMemberExchangeTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="uiHiddenCMID" Name="cmid" PropertyName="Value" 
                                            Type="Decimal" />
                                        <asp:ControlParameter ControlID="uiDdlCMExchApprovalStatus" 
                                            Name="ApprovalStatus" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </table>
                    
                    <table ID="pnlEditExchange" runat="server" cellpadding="1" cellspacing="1" 
                        style="width:100%;">
                    <tr runat="server">
                    <td runat="server">
                    <div class="shadow_view">
                    <div class="box_view">
                    <table class="table-row">
                        <tr runat="server">
                            <td runat="server">
                                CM Code</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:Label ID="uiLblCMCodeExchange" runat="server"></asp:Label>
                                <asp:HiddenField ID="uiHiddenCMID" runat="server" />
                                <asp:HiddenField ID="uiHiddenCMExchangeStatus" runat="server" />
                                <asp:HiddenField ID="uiHiddenExchID" runat="server" />
                                <asp:HiddenField ID="uiHiddenStartDate" runat="server" />
                                <asp:HiddenField ID="uiHiddenCMExchangeID" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Exchange</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlCMExchange" runat="server" 
                                    DataTextField="ExchangeCode" DataValueField="ExchangeId" Enabled="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Membership Type</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlMembershipType" runat="server" Enabled="False" 
                                    Height="22px" Width="125px">
                                    <asp:ListItem Value="B">Buyer</asp:ListItem>
                                    <asp:ListItem Value="S">Seller</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Exchange Member Code</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbExchangeMemberCode" runat="server" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Exchange License No</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbExchangeLicenseNo" runat="server" Enabled="False" 
                                    MaxLength="100" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Exchange LIcense Date</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpExchangeLicenseDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Effective Start Date</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpExchangeStartDate" runat="server" />
                            </td>
                        </tr>
                        <tr ID="Tr1" runat="server">
                            <td runat="server" ID="Td1">
                                Effective End Date</td>
                            <td runat="server" ID="Td2">
                                :</td>
                            <td runat="server" ID="Td3">
                                <uc2:CtlCalendarPickUp ID="uiDtpExchangeEndDate" runat="server" />
                            </td>
                        </tr>
                        <tr ID="trActionExchange" runat="server">
                            <td runat="server">
                                Action
                            </td>
                            <td runat="server">
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbCMExchActionFlag" runat="server" Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" ID="trApprovalDescExchange">
                            <td runat="server">
                                Approval Description </td>
                            <td runat="server">
                                : </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbCMExchbApprovalDesc" runat="server" Height="75px" 
                                    TextMode="MultiLine" Width="350px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                    ControlToValidate="uiTxbCMExchbApprovalDesc" ErrorMessage="Max. 100 char" 
                                    ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                <asp:Button ID="uiBtnSaveExchange" runat="server" CssClass="button_save" 
                                    OnClick="uiBtnSaveExchange_Click" Text="      Save" />
                                <asp:Button ID="uiBtnDeleteExchange" runat="server" CssClass="button_delete" 
                                    OnClick="uiBtnDeleteExchange_Click" Text="Delete" />
                                <asp:Button ID="uiBtnApproveExchange" runat="server" CssClass="button_approve" 
                                    OnClick="uiBtnApproveExchange_Click" Text="Approve" />
                                <asp:Button ID="uiBtnRejectExchange" runat="server" CssClass="button_reject" 
                                    OnClick="uiBtnRejectExchange_Click" Text="Reject" />
                                <asp:Button ID="uiBtnCancelExchange" runat="server" CssClass="button_cancel" 
                                    OnClick="uiBtnCancelExchange_Click" Text="      Cancel" />
                            </td>
                        </tr>
                    </table>
                    </div>
                    </div>
                    </td>
                    </tr>
                    </table>
                
                
            
                
                
            
            </ContentTemplate>
            

</cc1:TabPanel>
            <cc1:TabPanel runat="server" ID="uiPanelExchangeMember" HeaderText="Exchange Member">
                <ContentTemplate>

                    <table cellpadding="1" cellspacing="1" style="width:100%;">
                        <tr>
                            <td colspan="3">
                                <asp:BulletedList ID="uiBlEMError" runat="server" ForeColor="Red">
                                </asp:BulletedList>
                            </td>
                        </tr>
                        
                        <tr>
                        <td>
                        <div class="shadow_view">
                        <div class="box_view">
                        <table class="table-row">
                        <tr>
                            <td runat="server">
                                Exchange </td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlEMexchSearch" runat="server" 
                                    DataTextField="ExchangeCode" DataValueField="ExchangeID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td runat="server">
                                Approval Status</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlEMApprovalStat" runat="server">
                                    <asp:ListItem Value="A">Approved</asp:ListItem>
                                    <asp:ListItem Value="P">Proposed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                <asp:Button ID="uiBtnEMSearch" runat="server" CssClass="button_search" 
                                    Text="     Search" onclick="uiBtnEMSearch_Click" />
                                &nbsp;</td>
                        </tr>
                        </table>
                        </div>
                        </div>
                        </td>
                        </tr>
                   </table>
                   
                   <table class="table-datagrid">
                        <tr>
                            <td runat="server">
                                <asp:Button ID="uiBtnCreateEM" runat="server" CssClass="button_create" 
                                    OnClick="uiBtnEMCreate_Click" Text="     Create" />
                            </td>
                        </tr>
                        <tr>
                            <td runat="server">
                                <asp:GridView ID="uiDgExchangeMember" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" DataKeyNames="EMID" EmptyDataText="No Record" 
                                    MouseHoverRowHighlightEnabled="True" 
                                    OnPageIndexChanging="uiDgExchangeMember_PageIndexChanging" 
                                    OnRowCommand="uiDgExchangeMember_RowCommand" 
                                    OnSorting="uiDgExchangeMember_Sorting" RowHighlightColor="" Width="100%">
                                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    <Columns>
                                        <asp:ButtonField CommandName="Select" Text="edit" />
                                        <asp:BoundField DataField="ExchangeCode" HeaderText="Exchange Code" 
                                            SortExpression="ExchangeCode">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CODE" HeaderText="Code" SortExpression="CODE">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" />
                                        <asp:BoundField DataField="EffectiveStartDate" 
                                            DataFormatString="{0:dd-MMM-yyyy}" HeaderText="EffectiveStartDate" 
                                            ReadOnly="True" SortExpression="EffectiveStartDate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" 
                                            SortExpression="ApprovalStatusDesc">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="EMID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="uiLblEMID" runat="server" Text='<%# Bind("EMID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                    <RowStyle CssClass="tblRowStyle" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsExchangeMember" runat="server" 
                                    OldValuesParameterFormatString="original_{0}" 
                                    SelectMethod="GetDataBySearchCriteria" 
                                    TypeName="ExchangeMemberDataTableAdapters.ExchangeMemberSearchTableAdapter">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="uiHiddenEMCMID" Name="CMID" 
                                            PropertyName="Value" Type="Decimal" />
                                        <asp:ControlParameter ControlID="uiDdlEMApprovalStat" Name="approvalStatus" 
                                            PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="uiDdlEMexchSearch" Name="ExchangeID" 
                                            PropertyName="SelectedValue" Type="Decimal" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                        </table>
                    
                    <table id="pnlEditExchangeMember" runat="server" cellpadding="1" cellspacing="1" style="width: 100%;" >
                        <tr runat="server">
                            <td runat="server">
                            </td>
                        </tr>
                        <tr runat="server">
                        <td runat="server">
                        <div class="shadow_view">
                        <div class="box_view">
                        <table class="table-row">
                        <tr runat="server">
                            <td runat="server">
                                Code</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbEMCode" runat="server" MaxLength="5"></asp:TextBox>
                                <asp:HiddenField ID="uiHiddenEMid" runat="server" />
                                <asp:HiddenField ID="uiHiddenEMStatus" runat="server" />
                                <asp:HiddenField ID="uiHiddenEMCMID" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Name</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbEMName" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Exchange</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlEMexch" runat="server" DataTextField="ExchangeCode" 
                                    DataValueField="ExchangeID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Effective Start Date</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpEMStartDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Effective End Date</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <uc2:CtlCalendarPickUp ID="uiDtpEMEndDate" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                CM Representative</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:CheckBox ID="uiChkCMRep" runat="server" />
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Status</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:DropDownList ID="uiDdlEMStatus" runat="server">
                                    <asp:ListItem Value="A">Active</asp:ListItem>
                                    <asp:ListItem Value="I">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr ID="trActionExchangeMember" runat="server">
                            <td runat="server">
                                Action
                            </td>
                            <td runat="server">
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbEMAction" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                Mini Lot</td>
                            <td runat="server">
                                :</td>
                            <td runat="server">
                                <asp:CheckBox ID="uiChkMiniLot" runat="server" />
                            </td>
                        </tr>
                        <tr ID="trApprovalDescExchangeMember" runat="server">
                            <td runat="server">
                                Approval Desc
                            </td>
                            <td runat="server">
                                :
                            </td>
                            <td runat="server">
                                <asp:TextBox ID="uiTxbEMApprovalDesc" runat="server" Height="75px" 
                                    TextMode="MultiLine" Width="350px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" 
                                    ControlToValidate="uiTxbEMApprovalDesc" ErrorMessage="Max. 100 char" 
                                    ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                &nbsp;</td>
                            <td runat="server">
                                <asp:Button ID="uiBtnSaveEM" runat="server" CssClass="button_save" 
                                    OnClick="uiBtnSaveEM_Click" Text="     Save" />
                                <asp:Button ID="uiBtnDeleteEM" runat="server" CssClass="button_delete" 
                                    OnClick="uiBtnDeleteEM_Click" Text="     Delete" />
                                <asp:Button ID="uiBtnApproveEM" runat="server" CssClass="button_approve" 
                                    OnClick="uiBtnApproveEM_Click" Text="     Approve" />
                                <asp:Button ID="uiBtnRejectEM" runat="server" CssClass="button_reject" 
                                    OnClick="uiBtnRejectEM_Click" Text="     Reject" />
                                <asp:Button ID="uiBtnCancelEM" runat="server" CssClass="button_cancel" 
                                    OnClick="uiBtnCancelEM_Click" Text="     Cancel" />
                                &nbsp;</td>
                        </tr>
                    </table>
                        </div>
                        </div>
                    </td>
                    </tr>
                    </table>
                
                
                
                
                
                
            </ContentTemplate>
            

</cc1:TabPanel>
        </cc1:TabContainer>
        
    </div>
</asp:Content>

