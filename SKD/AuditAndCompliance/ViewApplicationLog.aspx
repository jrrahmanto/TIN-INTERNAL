<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewApplicationLog.aspx.cs" Inherits="AuditAndCompliance_ViewApplicationLog" %>

<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Application Log</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
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
                                        Application Module
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlApplModule" runat="server" DataSourceID="odsApplModule"
                                            DataTextField="ApplicationModule" DataValueField="ApplicationModule" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Classification
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlClassification" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="E">Error</asp:ListItem>
                                            <asp:ListItem Value="W">Warning</asp:ListItem>
                                            <asp:ListItem Value="I">Information</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        User Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" Width="179px"></asp:TextBox>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgApplicationLog" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="LogTime"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgApplicationLog_PageIndexChanging"
                                OnSorting="uiDgApplicationLog_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="LogTime" DataNavigateUrlFormatString="~/AuditAndCompliance/DetailApplicationLog.aspx?logTime={0:yyyy-MM-ddTHH_mm_ss.fff}"
                                        Text="Detail" />
                                    <asp:BoundField DataField="LogTime" HeaderText="Log Time" ReadOnly="True" SortExpression="LogTime"
                                        DataFormatString="{0:dd-MMM-yy HH:mm:ss}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ApplicationModule" HeaderText="Application Module" SortExpression="ApplicationModule"  ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClassificationDesc" HeaderText="ClassificationDesc" SortExpression="Classification" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" ItemStyle-HorizontalAlign="Center"/>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceApplicationLog" runat="server" SelectMethod="SelectApplicationLogByLogTime"
                                TypeName="ApplicationLog" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="logTime" PropertyName="text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="ddlApplModule" Name="applicationModule" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="ddlClassification" Name="classification" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="txtUserName" Name="userName" PropertyName="Text"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsApplModule" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="FillModule" TypeName="ApplicationLog">
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
