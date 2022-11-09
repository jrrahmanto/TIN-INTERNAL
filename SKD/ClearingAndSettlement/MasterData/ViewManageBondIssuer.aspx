<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewManageBondIssuer.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewManageBondIssuer" Title="Untitled Page" %>
 <%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Manage Bond Issuer</h1>
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
                                        Issuer Name
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtIssuerName" runat="server"></asp:TextBox>
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
                                    <asp:GridView ID="uiDgBondIssuer" runat="server" AutoGenerateColumns="False" Width="100%"
                                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                                        AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="BondIssuerID,ApprovalStatus"
                                        OnPageIndexChanging="uiDgBondIssuer_PageIndexChanging" OnSorting= "uiDgBondIssuer_Sorting">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("BondIssuerID", "~/ClearingAndSettlement/MasterData/EntryBondIssuer.aspx?eType=edit&eID={0}") %>'
                                                        Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BondIssuerName" HeaderText="Issuer Name" SortExpression="BondIssuerName" />
                                            <asp:BoundField DataField="approvalDesc" HeaderText="Status" SortExpression="approvalDesc">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourceBondIssuer" runat="server" SelectMethod="GetDataBondIssuerAndApprovalStatus"
                                        TypeName="BondIssuer">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="uiTxtIssuerName" Name="issuerNm" PropertyName="Text"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                                Type="String" />
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

