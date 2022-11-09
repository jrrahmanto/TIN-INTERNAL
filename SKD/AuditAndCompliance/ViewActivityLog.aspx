<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewActivityLog.aspx.cs" Inherits="AuditAndCompliance_ViewActivityLog" %>

<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <caption>
                <h1>View Activity Log</h1>
                <tr>
                    <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" ForeColor="#FF3300"
                            Visible="False"></asp:Label>
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
                                            Log Time
                                        </td>
                                        <td style="width:10px">
                                            :
                                        </td>
                                        <td>
                                            <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Log Activity
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="uiTxtLogActivity" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Source IP
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="uiTxtSourceIP" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Username
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="uiTxtUserName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" OnClick="uiBtnSearch_Click"
                                                Text="     Search" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </caption>
        </table>
        <table class="table-datagrid">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgActivityLog" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="LogTime"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgActivityLog_PageIndexChanging"
                                OnSorting="uiDgActivityLog_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="LogTime" DataNavigateUrlFormatString="~/AuditAndCompliance/DetailActivityLog.aspx?logTime={0:yyyy-MM-ddTHH_mm_ss.fff}"
                                        Text="Detail" />
                                    <asp:BoundField DataField="LogTime" HeaderText="Log Time" ReadOnly="True" SortExpression="LogTime">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LogActivity" HeaderText="Log Activity" SortExpression="LogActivity" />
                                    <asp:BoundField DataField="SourceIP" HeaderText="Source IP" SortExpression="SourceIP" />
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceActivityLog" runat="server" TypeName="ActivityLog"
                                SelectMethod="SelectActivityLogByLogTime">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="logTime" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiTxtLogActivity" Name="activityLog" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtSourceIP" Name="sourceIP" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtUserName" Name="userName" PropertyName="Text"
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
