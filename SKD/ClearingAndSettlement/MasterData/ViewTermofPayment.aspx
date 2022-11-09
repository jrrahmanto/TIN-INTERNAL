<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewTermofPayment.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewTermofPayment" %>

<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup"
    TagPrefix="uc2" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Manage Term of Payment</h1>
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
                                        Periode
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc3:CtlCalendarPickUp ID="CtlCalendarPeriod" runat="server" />
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
                            <asp:GridView ID="uiDgTermOfPayment" runat="server" AutoGenerateColumns="False" Width="100%" 
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TermID"
                                AllowPaging="True" AllowSorting="True"  PageSize="15" OnPageIndexChanging="uiDgTermOfPayment_PageIndexChanging"
                                OnSorting="uiDgTermOfPayment_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("TermID", "~/clearingandsettlement/masterdata/EntryTermofPayment.aspx?eType=edit&eID={0}") %>'
                                                Text="Edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Start Payment Date" DataField="StartPaymentDate" ReadOnly="True" 
                                        SortExpression="StartPaymentDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="End Payment Date" DataField="EndPaymentDate" ReadOnly="True" 
                                        SortExpression="EndPaymentDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DueDateKBIPKJ" 
                                        DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Due Date KBI PKJ"
                                        SortExpression="DueDateKBIPKJ" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ApprovalDesc" HeaderText="Approval Status" SortExpression="ApprovalDesc"
                                        ReadOnly="True" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    </Columns>
                                <EmptyDataTemplate>No Record</EmptyDataTemplate>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTermOfPayment" runat="server" SelectMethod="SelectTermPaymentByDateAppStatus"
                                TypeName="TermPayment" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <%--<asp:ControlParameter ControlID="uiTxtAccountNo" Name="accountNo" PropertyName="Text"
                                        Type="String" />--%>
                                    <asp:ControlParameter ControlID ="CtlCalendarPeriod" Name="startDate" PropertyName="Text" Type="DateTime" />
                                    <asp:ControlParameter ControlID ="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="Text" Type="String" />

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
