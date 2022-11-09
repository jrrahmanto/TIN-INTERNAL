<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Withdrawal.aspx.cs" Inherits="FinanceAndAccounting_Withdrawal" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Entry Withdrawal</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td>Tanggal Transfer</td>
                                <td>:</td>
                                <td>
                                    <uc4:CtlCalendarPickUp ID="CtlCalendarPickUpBusinessDate" runat="server" />
                                    <br /><span>*di isi saat print withdrawal</span> 
                                </td>
                            </tr>

                            <tr>
                                <td>Seller</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="uiSeller" runat="server"></asp:TextBox>
                                     <br /><span>*di isi saat print withdrawal</span> 
                                </td>
                            </tr>
                            <tr>
                                <td>ExchangeReff</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="uiExchReff" runat="server"></asp:TextBox>
                                     <br /><span>*di isi saat print withdrawal</span> 
                                </td>
                            </tr>
                            <tr>
                                <td> &nbsp;</td>
                                <td> </td>
                                <td> </td>
                            </tr>
                            <tr>
                                <td>Bukti Transfer (Format .csv)</td>
                                <td>:</td>
                                <td>
                                    <asp:FileUpload ID="uiUploadFile" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_import"
                                        OnClick="uiBtnUpload_Click" Text="     Upload" />

                                    <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_download"
                                        OnClick="uiBtnSearch_Click" Text="     Search" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <%--                                    <asp:TextBox ID="uiTxbLog" runat="server" Height="298px" TextMode="MultiLine"
                                        Width="780px"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table class="table-datagrid" id="tblgrid" runat="server">
        <tr>
            <td>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="uiDgUser" runat="server" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id"
                            MouseHoverRowHighlightEnabled="True"
                            OnPageIndexChanging="uiDgUser_PageIndexChanging"
                            OnSorting="uiDgUser_Sorting"
                            PageSize="20" RowHighlightColor="" Width="100%"
                            OnRowDataBound="uiDgRptFee_RowDataBound">


                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" SortExpression="id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="uiID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("id", "~/FinanceAndAccounting/DetailWithdrawal.aspx?id={0}") %>'
                                            Text="Entry" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="date" HeaderText="Date"
                                    SortExpression="date" DataFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="description1" HeaderText="Description 1"
                                    SortExpression="description1"></asp:BoundField>

                                <asp:BoundField DataField="description2" HeaderText="Description 2"
                                    SortExpression="description2"></asp:BoundField>


                                <asp:TemplateField HeaderText="amount" SortExpression="amount">
                                    <ItemTemplate>
                                        <asp:Label ID="amount" runat="server" Text='<%# Bind("amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote"
                                ShowParameterPrompts="False" Width="100%">
                            </rsweb:ReportViewer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Content>



