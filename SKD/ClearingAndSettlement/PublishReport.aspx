<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PublishReport.aspx.cs" Inherits="ClearingAndSettlement_PublishReport" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 310px;
        }
        .style2
        {
            width: 180px;
        }
        .style3
        {
            width: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                        <td class="style2">
                            EOD Date</td>
                        <td class="style3">
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUpEOD" runat="server" />
                        </td>
                    </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;</td>
                            <td class="style3">
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="uiBtnPublish" runat="server" onclick="uiBtnPublish_Click" 
                                    Text="Publish" />
                            </td>
                        </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td class="style3">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1">
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

