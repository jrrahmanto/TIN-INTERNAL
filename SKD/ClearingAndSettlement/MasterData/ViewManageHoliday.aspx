<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManageHoliday.aspx.cs" Inherits="WebUI_ClearingAndSettlement_ViewManageHoliday" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
    <%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Manage Holiday</h1>
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
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:100px">
                                        Holiday Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarHolidayDate" runat="server" />
                                        &nbsp; TO
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarHolidayDate2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Holiday Type
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlHolidayType" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="G">Global</asp:ListItem>
                                            <asp:ListItem Value="E">Exchange</asp:ListItem>
                                            <asp:ListItem Value="P">Commodity</asp:ListItem>
                                        </asp:DropDownList>
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
                                        <asp:DropDownList ID="uiDdlAction" runat="server">
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
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgHoliday" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="HolidayId"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgHoliday_PageIndexChanging"
                                OnSorting="uiDgHoliday_Sorting" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("HolidayId", "~/ClearingAndSettlement/MasterData/EntryManageHoliday.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="HolidayDate" HeaderText="Holiday Date" SortExpression="HolidayDate"
                                        ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HolidayTypeSummary" HeaderText="Holiday Type" ReadOnly="True"
                                        SortExpression="HolidayTypeSummary" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                                        Visible="False" />
                                    <asp:BoundField DataField="ExchangeCode" HeaderText="Exchange Code" SortExpression="ExchangeCode" />
                                    <asp:BoundField DataField="CommodityCode" HeaderText="Commodity Code" 
                                        SortExpression="CommodityCode" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceHoliday" runat="server" SelectMethod="SelectHolidayByHolidayDateAndAction"
                                TypeName="Holiday" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarHolidayDate" Name="holidayDate1" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="CtlCalendarHolidayDate2" Name="holidayDate2" 
                                        PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlAction" Name="Approval" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlHolidayType" Name="holidayType" PropertyName="SelectedValue"
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
