<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewCollateral.aspx.cs" Inherits="WebUI_New_ViewCollateral" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Collateral</h1>
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
                                    <td>
                                        Clearing Member</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Lodgement No
                                    </td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="uiTxtLodgementNo" runat="server"></asp:TextBox>
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
                                        <asp:DropDownList ID="uiDdlApprovalStatus" runat="server" Style="margin-left: 0px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
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
                </td>
            </tr>
            <tr>
                <td>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                    <asp:GridView ID="uiDgCollateral" runat="server" AutoGenerateColumns="False" Width="100%"
                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                        AllowSorting="True" DataKeyNames="LodgementNo,ApprovalStatus" OnPageIndexChanging="uiDgCollateral_PageIndexChanging"
                        OnSorting="uiDgCollateral_Sorting" PageSize="15">
                        <RowStyle CssClass="tblRowStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("LodgementID", "~/RiskManagement/MasterData/EntryCollateral.aspx?eType=edit&eID={0}") %>'
                                        Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Lodgement No" DataField="LodgementNo" SortExpression="LodgementNo" />
                            <asp:BoundField HeaderText="Lodgement Type" DataField="LodgementTypeDesc" 
                                SortExpression="LodgementTypeDesc" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EffectiveNominal" HeaderText="Effective Nominal" 
                                SortExpression="EffectiveNominal" DataFormatString="{0:#,###.###}" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Lodgement Date" DataField="LodgementDate" SortExpression="LodgementDate"
                                DataFormatString="{0:dd-MMM-yyyy}" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CMCode" HeaderText="Clearing Member" SortExpression="CMCode" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSourceCollateral" runat="server" SelectMethod="SelectCollateralByCollateralNoAndStatus"
                        TypeName="Collateral">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="uiTxtLodgementNo" Name="collateralNo" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                Type="String" />
                            <asp:ControlParameter ControlID="CtlClearingMemberLookup1" 
                                Name="clearingMember" PropertyName="LookupTextBoxID" Type="Decimal" />
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
