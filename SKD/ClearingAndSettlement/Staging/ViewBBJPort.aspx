<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewBBJPort.aspx.cs" Inherits="ClearingAndSettlement_Staging_ViewBBJPort" %>

<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <caption>
            <h1>View Staging Port</h1>
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
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" OnClick="uiBtnSearch_Click" Text="     Search" />
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
        </caption>
    </table>
    <table class="table-datagrid">
        <tr>
            <td>
                <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="uiDgPort" runat="server" AutoGenerateColumns="False" Width="100%"
                            MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="Code"
                            AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgPort_PageIndexChanging"
                            OnSorting="uiDgPort_Sorting">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="SelectPort" TypeName="StagingPort" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
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