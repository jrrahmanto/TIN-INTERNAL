<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewExchangeRate.aspx.cs" Inherits="WebUI_New_ViewExchangeRate" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
    <%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <caption>
            <h1>View Exchange Rate</h1>
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
                                    <td style="width:100px">
                                        Effective Date
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
                                        Exchange Rate Type
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlExchType" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="T">Tax</asp:ListItem>
                                            <asp:ListItem Value="B">BI</asp:ListItem>
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
                                        <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" 
                                            OnClick="uiBtnSearch_Click" Text="     Search" />
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
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                    OnClick="uiBtnCreate_Click" />
                <asp:Button ID="uiBtnApprove" CssClass="button_approve" runat="server" Text="     Approve"
                    OnClick="uiBtnApprove_Click" Visible="False" />
                <asp:Button ID="uiBtnReject" CssClass="button_reject" runat="server" Text="      Reject"
                    OnClick="uiBtnReject_Click" Visible="False" />
                <asp:Button ID="uiBtnDelete" CssClass="button_delete" runat="server" Text="       Delete"
                    OnClick="uiBtnDelete_Click" Visible="False" />
            </td>
        </tr>
        <tr>
            <td>
                <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="uiDgExchangeRate" runat="server" AutoGenerateColumns="False" Width="100%"
                            MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="ExchangeRateID"
                            AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgExchangeRate_PageIndexChanging"
                            OnSorting="uiDgExchangeRate_Sorting">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ExchangeRateID", "~/ClearingAndSettlement/MasterData/EntryExchangeRate.aspx?eType=edit&id={0}") %>'
                                            Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="uiChkList" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SourceCurrencyDesc" HeaderText="Source Currency" SortExpression="SourceCurrencyDesc">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DestCurrencyDesc" HeaderText="Destination Currency" SortExpression="DestCurrencyDesc">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExchRateStartDate" HeaderText="Effective Date" SortExpression="ExchRateStartDate"
                                    DataFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ExchRateTypeDesc" HeaderText="Exchange Rate Type" SortExpression="ExchRateTypeDesc">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" DataFormatString="{0:#,###.###}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="SelectExchangeRateByAction"
                            TypeName="ExchangeRate" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="EffectiveDate" PropertyName="Text"
                                    Type="DateTime" />
                                <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="Approval" PropertyName="SelectedValue"
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiDdlExchType" Name="exchRateType" PropertyName="SelectedValue"
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
