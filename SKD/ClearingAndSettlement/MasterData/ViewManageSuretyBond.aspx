<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManageSuretyBond.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewManageSuretyBond" %>
    <%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlBondIssuerLookup.ascx" tagname="CtlBondIssuerLookup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Manage Surety Bond</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                                    <td style="width:100px">
                                        Entry Type
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtEntryType" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                            Bond Issuer</td>
                                    <td>
                                        :</td>
                                    <td>
                                            <uc1:CtlBondIssuerLookup ID="CtlBondIssuer" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100px">
                                        Bond Serial Number
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtBondSN" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Approval Status
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
            <tr>
                <td>
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
                            <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="uiDgSurety" runat="server" AutoGenerateColumns="False" Width="100%"
                                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                                        AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="EntryType,ApprovalStatus"
                                        OnPageIndexChanging="uiDgSurety_PageIndexChanging" OnSorting="uiDgSurety_Sorting">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("SuretyBondID", "~/ClearingAndSettlement/MasterData/EntrySuretyBond.aspx?eType=edit&eID={0}") %>'
                                                        Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EntryType" HeaderText="Entry Type" SortExpression="EntryType" />
                                            <asp:BoundField DataField="IssuerName" HeaderText="Issuer Name" SortExpression="IssuerName" />
                                            <asp:BoundField DataField="AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                                            <asp:BoundField DataField="BondStatus" HeaderText="Bond Status" SortExpression="BondStatus" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="BondSerialNo" HeaderText="Bond SerialNo" SortExpression="BondSerialNo" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:#,##0.##########}" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="HaircutPct" HeaderText="HaircutPct" SortExpression="HaircutPct" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="ExpiredDate" HeaderText="Expired Date" SortExpression="ExpiredDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="ExpDateHaircut" HeaderText="ExpDate Haircut" SortExpression="ExpDateHaircut" DataFormatString="{0:dd-MMM-yyyy}" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="BondStatus" HeaderText="BondStatus" SortExpression="BondStatus"  >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="AccountBalance" HeaderText="AccountBalance" SortExpression="AccountBalance" DataFormatString="{0:#,##0.##########}" >
                                                <ItemStyle HorizontalAlign="Center" /></asp:BoundField>
                                            <asp:BoundField DataField="ApprovalDesc" HeaderText="Status" SortExpression="ApprovalDesc">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>No Records Available</EmptyDataTemplate>
                                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourceSurety" runat="server" SelectMethod="GetDataSuretyBondAndApprovalStatus"
                                        TypeName="SuretyBond">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="uiTxtEntryType" Name="entryType" PropertyName="Text"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="uiTxtBondSN" Name="bondSN" PropertyName="Text"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="CtlBondIssuer" Name="bondIssuer" PropertyName="LookupTextBoxID"
                                            Type="decimal" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                                </Triggers>
                            </cc1:ProgressUpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
