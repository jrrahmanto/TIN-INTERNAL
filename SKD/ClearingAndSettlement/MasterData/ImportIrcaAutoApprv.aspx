<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="ImportIrcaAutoApprv.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ImportIrcaAutoApprv" %>

<%@ Register src="../../Controls/frmImportCtl.ascx" tagname="frmImportCtl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Import IRCA with Auto-Approval</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     <tr>
                    <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
                            </asp:BulletedList>
                    </td>
                </tr>
    <tr>
        <td>
         <div class="shadow">
            <div class="box">
            <table class="table-row">
               
                <tr>
                    <td style="width:100px">
                            Approval Desc</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox runat="server" ID="uiTxtApprovalDesc" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            File Name</td>
                    <td>
                        :</td>
                    <td>
                            <asp:FileUpload ID="uiUploadFile" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_create" 
                            onclick="uiBtnUpload_Click" Text="     Upload" />
&nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="     Cancel" 
                            onclick="uiBtnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                            <asp:TextBox ID="uiTxbLog" runat="server" Height="298px" TextMode="MultiLine" 
                                Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                        <img alt="" src="../../Images/ajax-loader2.gif"
                                            style="width: 220px; height: 19px" />
                            </ProgressTemplate>
                            </asp:UpdateProgress> &nbsp;</td>
                </tr>
                    </table>
             </div>
             </div>
        </td>
    </tr>
    </table>
</asp:Content>
