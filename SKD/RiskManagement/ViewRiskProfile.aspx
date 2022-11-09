<%@ Page  Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRiskProfile.aspx.cs" Inherits="RiskManagement_ViewRiskProfile" %>

<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Risk Profile</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
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
                            <asp:GridView ID="uiDgRiskProfile" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="CMID"
                                OnPageIndexChanging="uiDgRiskProfile_PageIndexChanging" OnRowDataBound="uiDgRiskProfile_RowDataBound"
                                OnSorting="uiDgRiskProfile_Sorting" AllowPaging="True" AllowSorting="True" PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="EntryRiskProfile.aspx?eType=edit&eID=<%# DataBinder.Eval(Container.DataItem, "RiskProfileID")  %>">
                                                Edit </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="ViewImpactMatrix.aspx?eID=<%# DataBinder.Eval(Container.DataItem, "RiskProfileID")  %>">
                                                Matrix </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RiskProfileID" HeaderText="RiskProfileID" InsertVisible="False"
                                        SortExpression="RiskProfileID" Visible="False" />
                                    <asp:BoundField DataField="CMID" HeaderText="Clearing Member" SortExpression="CMID"
                                        ReadOnly="True" />
                                    <asp:BoundField HeaderText="Start Date" DataField="StartDate" SortExpression="StartDate"
                                        DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EndDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="End Date"
                                        SortExpression="EndDate" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceRiskProfile" runat="server" SelectMethod="GetRiskProfileByClearingMemberID"
                                TypeName="RiskProfile" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="clearingMemberID"
                                        PropertyName="LookupTextBoxID" Type="Decimal" />
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
                <td id="tdImage" runat="server">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
