<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewStressTestScenario.aspx.cs" Inherits="WebUI_RiskManagement_ViewStressTestScenario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Stress Test Scenario</h1>
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
                                        Scenario Name
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtScenarioName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
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
                <td colspan="3">
                    <asp:Button ID="uiBtnCreate" runat="server" Text="    Create" CssClass="button_create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgStressTestScenario" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                                AllowSorting="True" OnPageIndexChanging="uiDgStressTestScenario_PageIndexChanging"
                                OnSorting="uiDgStressTestScenario_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                                NavigateUrl='<%# Eval("ScenarioID", "~/RiskManagement/EntryStressTestScenario.aspx?eType=edit&eID={0}") %>' 
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:HyperLinkField Text="Show" DataNavigateUrlFormatString="~/RiskManagement/StressTest.aspx?ID={0}"
                                        DataNavigateUrlFields="ScenarioID" />
                                    <asp:BoundField DataField="ScenarioName" HeaderText="Scenario Name" SortExpression="ScenarioName" />
                                    <asp:BoundField DataField="ScenarioDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Scenario Date"
                                        SortExpression="ScenarioDate">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceStressTestScenario" runat="server" SelectMethod="SelectScenarioName"
                                TypeName="StressTestScenario">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtScenarioName" Name="scenarioName" PropertyName="Text"
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
        </table>
    </asp:Panel>
</asp:Content>
