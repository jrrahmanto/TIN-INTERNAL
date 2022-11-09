<%@ Control Language="C#" AutoEventWireup="true" CodeFile="frmImportCtl.ascx.cs" Inherits="WebUI_frmImportCtl" %>
<script language="javascript" type="text/javascript">
function showWait2()
{
    if ($get('uiFU').value.length > 0)
    {
        $get('UpdateProgress1').style.display = 'block';
    }
}
</script>

<table style="width:100%;" runat="server" id="tblMain">
    <tr>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <br />
                    <br />
                    <table style="width:100%;">
                        <tr>
                            <td>
                                <asp:FileUpload ID="uiFU" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="uiBtnImport" runat="server" onclick="uiBtnImport_Click" 
                                    Text="Import" OnClientClick="javascript:showWait2();"/>
                                &nbsp;<asp:Button ID="uiBtnCancel" runat="server" onclick="uiBtnCancel_Click" 
                                    Text="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="uiTxtLog" runat="server" Height="350px" ReadOnly="True" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                                <br />
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                    AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                                    <ProgressTemplate>
                                        <img alt="" src="../Images/ajax-loader2.gif" 
                                            style="width: 220px; height: 19px" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="uiBtnImport"  />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
                    &nbsp;</td>
    </tr>
    <tr>
        <td align="right">
                                    &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
                    &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
</table>
