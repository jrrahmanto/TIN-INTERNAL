<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewBulkTransfer.aspx.cs" Inherits="WebUI_New_ViewBulkTransfer" %>



<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>View Bulk Transfer</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
         <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                <table  class="table-row">
        <tr>
            <td style="width:200px">
                                                        Clearing Member Sender</td>
            <td style="width:10px">
                        :</td>
            <td>
                <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookupSender" 
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                                                        Clearing Member Receiver</td>
            <td>
                        :</td>
            <td>
                <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookupDestination" 
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                            Description</td>
            <td>
                        :</td>
            <td>
                <asp:TextBox ID="uiTxtDescription" runat="server" Height="78px" 
                    TextMode="MultiLine" Width="288px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                    Text="          Request" onclick="uiBtnSave_Click"  />
            </td>
        </tr>
    </table>
    </div>
                </div>
            </td>
        </tr>
        </table>
</asp:Content>

