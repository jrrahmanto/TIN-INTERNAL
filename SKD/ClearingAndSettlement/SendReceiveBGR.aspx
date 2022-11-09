<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SendReceiveBGR.aspx.cs" Inherits="ClearingAndSettlement_SendReceiveBGR" %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Send / Receive Manual BGR</h1>
    <table style="width:100%;">        
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    
                    <table style="width:100%;">
                    <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="#CC0000" 
                                Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Business Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUpEOD" runat="server" />
                        </td>
                    </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:CheckBox ID="cbGenerateReport" runat="server" Checked="True" 
                                    Text="Receive Seller Allocation" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:CheckBox ID="cbReplicate" runat="server" Checked="True" 
                                    Text="Send Settlement" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked="True" 
                                    Text="Send Payment" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" 
                                    Text="Receive Tracking" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="uiBtnProcess" runat="server" onclick="uiBtnProcess_Click" Text="Publish" />
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
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="500">
                <ProgressTemplate>
                    <img src="../Images/ajax-loader2.gif" />
                </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>