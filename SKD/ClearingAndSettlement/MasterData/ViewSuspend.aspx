<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewSuspend.aspx.cs" Inherits="WebUI_New_ViewSuspend" %>

<%@ Register Src="~/Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlCMExchangeLookup.ascx" TagName="CtlCMExchangeLookup"
    TagPrefix="uc2" %>
<%@ Register src="../../Lookup/CtlExchangeMemberLookup.ascx" tagname="CtlExchangeMemberLookup" tagprefix="uc3" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        View Suspend Status</h1>
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
                                    Account Code
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlInvestorLookup ID="CtlInvestorLookup1" runat="server" />
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
                                        <asp:ListItem Value=""></asp:ListItem>
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
                                    <asp:Button ID="uiBtnDownload" CssClass="button_view" runat="server" 
                                Text="     Download" onclick="uiBtnDownload_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table class="table-datagrid" id ="tblGrid" runat="server">
        <tr id="trGrid" runat="server">
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
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("InvestorID", "~/ClearingAndSettlement/MasterData/EntrySuspendStatus.aspx?eType=edit&eID={0}") %>'
                                            Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CODE" HeaderText="Account Code" SortExpression="CODE">
                                   
                                </asp:BoundField>
                               
                                <asp:BoundField DataField="Name" HeaderText="Account Name" SortExpression="Name">
                                </asp:BoundField>
                                <asp:BoundField DataField="AccountStatusDesc" HeaderText="Account Status" SortExpression="AccountStatusDesc">
                                </asp:BoundField>
                                 <asp:BoundField DataField="AccountStatusReason" HeaderText="Reason Status" SortExpression="AccountStatusReason">
                                </asp:BoundField>
                                 <asp:BoundField DataField="SuspendedBy" HeaderText="Suspended By" SortExpression="SuspendedBy">
                                </asp:BoundField>
                                <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Approval Status" SortExpression="ApprovalStatusDesc">
                                    <ItemStyle Width="75px" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceInvestor" runat="server" SelectMethod="FillBySearchCriteria"
                            TypeName="SuspendAccStatus" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <%--<asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="cmid" PropertyName="LookupTextBoxID"
                                    Type="Decimal" />--%>
                                <asp:ControlParameter ControlID="CtlInvestorLookup1" Name="code" PropertyName="LookupTextBox"
                                    Type="String" />
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
    <table class="table-datagrid" id="tblRpt" runat="server">
        <tr id="trRpt" runat ="server">
            <td>
                 
                 <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>

                           <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote" 
                                ShowParameterPrompts="False" Width="100%">
                            </rsweb:ReportViewer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                 </asp:Panel>
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
