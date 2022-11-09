<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewGlobalParamater.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewGlobalParamater" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Global Parameter</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
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
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:150px">
                                        Parameter Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtParamCode" runat="server"></asp:TextBox>
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
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="uiDgGlobalParameter" runat="server" AutoGenerateColumns="False"
                            Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                            AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="Code,EffectiveStartDate,ApprovalStatus"
                            OnPageIndexChanging="uiDgGlobalParameter_PageIndexChanging" OnSorting="uiDgGlobalParameter_Sorting">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ParameterID", "~/ClearingAndSettlement/MasterData/EntryGlobalParameter.aspx?eType=edit&eID={0}") %>'
                                            Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Code" HeaderText="Parameter Code" SortExpression="Code" />
                                <asp:BoundField HeaderText="Effective Start Date" DataField="EffectiveStartDate"
                                    SortExpression="EffectiveStartDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            </Columns>
                            <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceGlobalParameter" runat="server" SelectMethod="SelectParameterByCodeAndApprovalStatus"
                            TypeName="Parameter">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtParamCode" Name="paramCode" PropertyName="Text"
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
