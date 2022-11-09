<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReportFee.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_ReportFee" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Report Fee</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red" Visible="False">
                        </asp:BulletedList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width: 100px">Start Business Date
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <uc3:CtlCalendarPickUp ID="uiStart" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">End Business Date
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <uc3:CtlCalendarPickUp ID="uiEnd" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">Name
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:TextBox ID="uiName" runat="server"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">Inv Number
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:TextBox ID="uiInv" runat="server"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">Type
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <select style="width: 176px" id="uiDropdown" runat="server">
                                        <option value="e">All</option>
                                        <option value="Buyer">Buyer</option>
                                        <option value="Seller">Seller</option>
                                    </select>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">Status
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <select style="width: 176px" id="uiDropdown2" runat="server">
                                        <option value="2">All</option>
                                        <option value="0">Paid</option>
                                        <option value="1">Unpaid</option>
                                    </select>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                        OnClick="uiBtnSearch_Click" />

                                    <asp:Button ID="uiBtnPrint" CssClass="button_print" runat="server" Text="     Print"
                                        OnClick="uiBtnPrint_Click" />

                                    <asp:Button ID="uiBtnJournal" CssClass="button_download" runat="server" Text="  Journal"
                                        OnClick="uiBtnJournal_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table width="100%" text-align="right" border="0">
        <td style="text-align: right">
            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="     Save"
                OnClick="uiBtnSave_Click" />
        </td>
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
                                <%--                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("id", "~/ClearingAndSettlement/TradeFeed/EntryRptFee.aspx?id={0}") %>'
                                            Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="nama" HeaderText="Nama"
                                    SortExpression="nama"></asp:BoundField>
                                <asp:BoundField DataField="inv_number" HeaderText="Inv Number"
                                    SortExpression="inv_number"></asp:BoundField>

                                <%--                                <asp:BoundField DataField="ExchangeReff" HeaderText="ExchangeReff"
                                    SortExpression="ExchangeReff">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>

                                <%--                                <asp:BoundField DataField="pelaku" HeaderText="Pelaku"
                                    SortExpression="pelaku">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>

                                <%--                              <asp:BoundField DataField="CMID" HeaderText="CMID"
                                    SortExpression="CMID">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>

                                <asp:BoundField DataField="BusinessDate" HeaderText="Business Date"
                                    SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>

                                <%--                                <asp:BoundField DataField="productid" HeaderText="Product ID"
                                    SortExpression="productid">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>

                                <%--                                <asp:BoundField DataField="currency" HeaderText="Currency"
                                    SortExpression="currency">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>

                                <asp:BoundField DataField="kurs" HeaderText="Kurs"
                                    SortExpression="kurs">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="lot" HeaderText="Lot"
                                    SortExpression="lot">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="ton" HeaderText="Ton"
                                    SortExpression="ton">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Trx Value" SortExpression="TrxValue">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblPrice" runat="server" Text='<%# Bind("TrxValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("TrxValue") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trx Fee IDR" SortExpression="TrxFee">
                                    <ItemTemplate>
                                        <asp:Label ID="uiTrxFeeBeforePpn" runat="server" Text='<%# Bind("TrxFee") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("TrxFee") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trx Fee USD" SortExpression="TRXFEEDOLLAR">
                                    <ItemTemplate>
                                        <asp:Label ID="uiTRXFEEDOLLARBeforePpn" runat="server" Text='<%# Bind("TRXFEEDOLLAR") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("TRXFEEDOLLAR") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PPN" SortExpression="ppn">
                                    <ItemTemplate>
                                        <asp:Label ID="uippn" runat="server" Text='<%# Bind("ppn") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("ppn") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Fee IDR" SortExpression="TrxFee">
                                    <ItemTemplate>
                                        <asp:Label ID="uiTrxFee" runat="server" Text='<%# Bind("TrxFee") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("TrxFee") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Total Fee USD" SortExpression="TRXFEEDOLLAR">
                                    <ItemTemplate>
                                        <asp:Label ID="uiTRXFEEDOLLAR" runat="server" Text='<%# Bind("TRXFEEDOLLAR") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("TRXFEEDOLLAR") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:TemplateField>

                                <%--                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                    <ItemTemplate>
                                        <asp:Label ID="uistatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="uistatus2" runat="server" Text='<%# Bind("status") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tgl Pembayaran" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox Width="90%" ID="tgl_pembayaran" runat="server" placeholder="mm-dd-yyyy" MaxLength="10"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>

                                <%--                                <asp:BoundField DataField="tgl_pembayaran" HeaderText="Tgl Pembayaran"
                                    SortExpression="tgl_pembayaran" DataFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>--%>
                            </Columns>
                            <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
