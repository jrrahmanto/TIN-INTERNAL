<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewBuktiPotong.aspx.cs" Inherits="FinanceAndAccounting_ViewBuktiPotong" %>

<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>
<%@ Register src="../Controls/CtlYearMonthBuktiPotong.ascx" tagname="CtlYearMonthBuktiPotong" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Manage Bukti Potong</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
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
                                    <td style="width:150px">
                                        CMID</td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:150px">
                                        Period</td>
                                    <td style="width:10px">
                                        :</td>
                                    <td>
                                        <uc3:CtlYearMonthBuktiPotong ID="CtlYearMonthBuktiPotong1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Status
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search"
                                            OnClick="uiBtnSearch_Click" />
                                    </td>
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
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgBuktiPotong" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                                AllowSorting="True" DataKeyNames="BusinessDate,CMID,ApprovalStatus" OnPageIndexChanging="uiDgBuktiPotong_PageIndexChanging"
                                OnSorting="uiDgBuktiPotong_Sorting" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("TaxID", "~/FinanceAndAccounting/EntryBuktiPotong.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate"
                                        ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Name" HeaderText="Clearing Member Name" 
                                        SortExpression="Name" />
                                    <asp:BoundField DataField="TaxNo" DataFormatString="{0:#,##0.##}" 
                                        HeaderText="Tax No" SortExpression="TaxNo">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceBuktiPotong" runat="server" SelectMethod="SelectBuktiPotongByCriteria"
                                TypeName="BuktiPotong" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="CMID" PropertyName="LookUpTextBoxID"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="CtlYearMonthBuktiPotong1" Name="Period" 
                                        PropertyName="YearMonth" Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

