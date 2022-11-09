<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IntradayMargin.aspx.cs" Inherits="RiskManagement_IntradayMargin" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
        function setAllCheckBox(el, className) {
            var chks = document.getElementsByTagName("input");
            for (ii = 0; ii < chks.length; ii++) {
                if (chks[ii].className == className && chks[ii].type == "checkbox") {
                    chks[ii].checked = el.checked;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td style="text-align: center">
                <asp:GridView ID="uiDgIDMPrice" runat="server" 
                    AutoGenerateColumns="False" HorizontalAlign="Center"
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" 
                    DataKeyNames="BusinessDate,ContractID">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Update">
                            <HeaderTemplate>
                                <asp:CheckBox ID="uiChkUpdate" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" class="chkUpdate" id="uiChkUpdate" runat="server" name="uiChkUpdate" />
                                <asp:HiddenField ID="uiHdnContractID" runat="server" Value='<%# Bind("ContractID") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="uiChkUpdate" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContractName" HeaderText="Contract Name" 
                            SortExpression="ContractName" ReadOnly="True" >
                        <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Price" SortExpression="Price">
                            <ItemTemplate>
                                <asp:TextBox ID="uiTxtPrice" runat="server" CssClass="number" BorderWidth="0" Width="98%" Height="100%" Text='<%# Bind("Price", "{0:#,##0.####}") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Price") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="140px" />
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="text-align:center">
                <asp:Button ID="uiBtnExecuteIntradayMargin" CssClass="button_execute" 
                    runat="server" Text="        Execute Intraday Margin" 
                    onclick="uiBtnExecuteIntradayMargin_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

