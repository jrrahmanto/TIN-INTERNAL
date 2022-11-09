<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewRiskType.aspx.cs" Inherits="WebUI_New_ViewRiskType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View RiskType</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
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
                                        Risk Type Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtRiskTypeCode" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr visible="false" runat="server">
                                    <td>
                                        Approval Status
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server" Style="margin-left: 0px">
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgRiskType" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" OnPageIndexChanging="uiDgRiskType_PageIndexChanging"
                                OnSorting="uiDgRiskType_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("RiskTypeID", "~/RiskManagement/MasterData/EntryRiskType.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RiskTypeCode" HeaderText="Risk Type" SortExpression="RiskTypeCode" />
                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status" Visible="False" />
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceRiskType" runat="server" SelectMethod="GetRiskTypeByRiskTypeCodeAndApprovalStatus"
                                TypeName="RiskType">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtRiskTypeCode" Name="riskTypeCode" PropertyName="Text"
                                        Type="String" />
                                    <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
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
