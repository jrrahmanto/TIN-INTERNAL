<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewIRCA.aspx.cs" Inherits="WebUI_New_ViewIRCA" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View IRCA</h1>
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
                                    <td style="width:100px">
                                        Effective Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
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
                                        <asp:Button ID="uiBtnEdit" CssClass="button_edit" runat="server" Text="     Edit"
                                            OnClick="uiBtnEdit_Click" />
                                        <asp:Button ID="uiBtnViewTransaction" CssClass="button_search" runat="server" Text="     View"
                                            OnClick="uiBtnViewTransaction_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table class="table-datagrid">
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                                            OnClick="uiBtnCreate_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="uiDgIRCA" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    MouseHoverRowHighlightEnabled="True" RowHighlightColor="" 
                                                    DataKeyNames="IRCAID" EmptyDataText="No Records" 
                                                    AllowPaging="True" AllowSorting="True" PageSize="40" OnPageIndexChanging="uiDgIRCA_PageIndexChanging"
                                                    OnSorting="uiDgIRCA_Sorting">
                                                    <RowStyle CssClass="tblRowStyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="EffectiveStartDate" HeaderText="Effective Date"
                                                            SortExpression="EffectiveStartDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Commodity Code" DataField="CommodityCode" SortExpression="CommodityCode" />
                                                        <asp:BoundField HeaderText="IRCA Value" DataField="IRCAValue" DataFormatString="{0:#,##0.######}" 
                                                            SortExpression="IRCAValue"  >
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Effective End Date" DataField="EffectiveEndDate" SortExpression="EffectiveEndDate"
                                                            DataFormatString="{0:dd-MMM-yyyy}" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ApprovalDesc" HeaderText="Status" 
                                                            SortExpression="ApprovalDesc" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="ObjectDataSourceIRCA" runat="server" SelectMethod="GetIRCAByEffectiveStartDateAndApprovalStatus"
                                                    TypeName="IRCA">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="CtlCalendarEffectiveStartDate" Name="effectiveStartDate"
                                                            PropertyName="Text" Type="DateTime" />
                                                        <asp:ControlParameter ControlID="uiDdlAction" Name="approvalStatus" PropertyName="SelectedValue"
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
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
