<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportMBD.aspx.cs" Inherits="AuditAndCompliance_ImportMBD" %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>
<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Import MBD</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        
        <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
        
        <tr>
            <td>
            <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td style="width:150px">
                            Clearing Member</td>
                        <td>
                        :</td>
                                                            <td>
                                                                <uc2:CtlClearingMemberLookup ID="uiCtlCm" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                            Bussiness Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="uiDtpBussDate" runat="server" />
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
                            <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_import" 
                            onclick="uiBtnUpload_Click" Text="     Upload" />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="     Cancel" 
                            />
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
                                Width="206%"></asp:TextBox>
                        </td>
                    </tr>
                    
                </table>
                </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

