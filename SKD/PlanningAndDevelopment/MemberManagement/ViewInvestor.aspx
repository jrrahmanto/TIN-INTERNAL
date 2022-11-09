<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewInvestor.aspx.cs" Inherits="WebUI_New_ViewInvestor" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlCMExchangeLookup.ascx" TagName="CtlCMExchangeLookup"
    TagPrefix="uc2" %>
<%@ Register src="../../Lookup/CtlExchangeMemberLookup.ascx" tagname="CtlExchangeMemberLookup" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        View Account</h1>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td colspan="3">
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                        </asp:BulletedList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                        </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width:150px">
                                    Clearing Member
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Exchange Member
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc3:CtlExchangeMemberLookup ID="CtlExchangeMemberLookup1" runat="server" />
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
    </table>
    <table class="table-datagrid">
        <tr>
            <td>
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                    OnClick="uiBtnCreate_Click" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="uiDgInvestor" runat="server" AutoGenerateColumns="False" Width="100%"
                            MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                            DataKeyNames="InvestorID"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgInvestor_PageIndexChanging" 
                            onsorting="uiDgInvestor_Sorting" AllowSorting="True">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("InvestorID", "~/PlanningAndDevelopment/MemberManagement/EntryInvestor.aspx?id={0}") %>'
                                            Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExchangeMember" HeaderText="Exchange Member" SortExpression="ExchangeMember">
                                </asp:BoundField>
                                <asp:BoundField DataField="ClearingMemberCode" HeaderText="CM Code" SortExpression="ClearingMemberCode">
                                </asp:BoundField>
                                <asp:BoundField DataField="ClearingMemberName" HeaderText="CM Name" SortExpression="ClearingMemberName">
                                </asp:BoundField>
                                <asp:BoundField DataField="CODE" HeaderText="Account" SortExpression="CODE">
                                    <ItemStyle Width="75px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InHouseFlag" HeaderText="InHouseFlag" SortExpression="InHouseFlag">
                                    <ItemStyle Width="75px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceInvestor" runat="server" SelectMethod="FillBySearchCriteria"
                            TypeName="Investor" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="cmid" PropertyName="LookupTextBoxID"
                                    Type="Decimal" />
                                <asp:ControlParameter ControlID="CtlExchangeMemberLookup1" Name="emid" PropertyName="LookupTextBoxID"
                                    Type="Decimal" />
                                <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" EventName="Click"/>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        
    </table>
</asp:Content>
