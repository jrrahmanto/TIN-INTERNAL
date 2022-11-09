<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewDisiplinAK.aspx.cs" Inherits="WebUI_New_ViewDisiplinAK" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Displin AK</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
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
                                        CM Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookupSanction" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Yang Memberikan Sanksi
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlPemberiSanksi" runat="server" Height="22px" Width="126px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="BA">Bappebti</asp:ListItem>
                                            <asp:ListItem Value="B">BBJ</asp:ListItem>
                                            <asp:ListItem Value="K">KBI</asp:ListItem>
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
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Approve</asp:ListItem>
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
                <td colspan="3">
                    <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" Text="    Create"
                        OnClick="uiBtnCreate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgDisiplinAK" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="SanctionNo,ApprovalStatus" EmptyDataText="No Record"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgPostingGroup_PageIndexChanging"
                                OnSorting="uiDgPostingGroup_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField Text="Edit" DataNavigateUrlFields="SanctionNo, ApprovalStatus"
                                        DataNavigateUrlFormatString="~/PlanningAndDevelopment/DisciplineManagement/EntryDisiplinAK.aspx?eType=edit&eID={0}&status={1}"
                                        HeaderText="Edit">
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:HyperLinkField>
                                    <asp:BoundField DataField="CMCode" HeaderText="CM Code" SortExpression="CMCode" />
                                    <asp:BoundField HeaderText="Yang Memberi Sanksi" DataField="SanctionSourceDesc" SortExpression="SanctionSourceDesc">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Letter No." DataField="SanctionNo" SortExpression="SanctionNo" />
                                    <asp:BoundField HeaderText="Efective Date" DataField="StartDate" DataFormatString="{0:dd-MMM-yyyy}"
                                        SortExpression="StartDate" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceDisiplinAK" runat="server" SelectMethod="FillBySearchCriteriaAll"
                                TypeName="Sanction" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlPemberiSanksi" Name="sanctionSource" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookupSanction" Name="CMID" PropertyName="LookupTextBoxID"
                                        Type="Decimal" />
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
