<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewClearingMember.aspx.cs" Inherits="WebUI_New_ViewClearingMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Participant</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red" Visible="False">
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
                                        Participant Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtCMCode" runat="server" MaxLength="20"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Exchange
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlExchange" runat="server" DataTextField="ExchangeCode"
                                            DataValueField="ExchangeCode">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Membership Type
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlMembership" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="B">Buyer</asp:ListItem>
                                            <asp:ListItem Value="S">Seller</asp:ListItem>
                                        </asp:DropDownList>
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
                                            <asp:ListItem Value="R">Reject</asp:ListItem>
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
                            <asp:GridView ID="uiDgClearingMember" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" OnRowCommand="uiDgClearingMember_RowCommand"
                                DataKeyNames="CMID,CMProfileID" EmptyDataText="No Record"
                                AllowPaging="True" AllowSorting="True" 
                                onpageindexchanging="uiDgClearingMember_PageIndexChanging" 
                                onsorting="uiDgClearingMember_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="CMID,CMProfileID" DataNavigateUrlFormatString="~/PlanningAndDevelopment/MemberManagement/EntryClearingMember.aspx?id={0}&amp;profileID={1}"
                                        Text="edit"/>
                                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" ReadOnly="True" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    <asp:BoundField DataField="CMStatusDesc" HeaderText="Participant Status" SortExpression="CMStatusDesc"
                                        ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EffectiveStartDate" HeaderText="Effective Start Date"
                                        SortExpression="EffectiveStartDate" DataFormatString="{0:dd-MMM-yyyy}" 
                                        ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" 
                                        ItemStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetCMProfileBySearchCriteria"
                                TypeName="ClearingMember" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtCMCode" Name="code" PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlMembership" Name="cmType" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlExchange" Name="exchangeCode" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
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
