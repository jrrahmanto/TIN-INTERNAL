<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewTransferSPAApproval.aspx.cs" Inherits="WebUI_New_ViewTransferSPAApproval" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcPanel" TagPrefix="cc1" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Transfer SPA Approval</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                    <div class="shadow_view">
                        <div class="box_view">
                            <table class="table-row">
                                <tr>
                                    <td style="width:100px">
                                        Transfer Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUpTransfer" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <nobr>Clearing Member Receiver</nobr>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Transfer Type
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlTransferType" runat="server" Width="184px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="MM">Member to Member</asp:ListItem>
                                            <asp:ListItem Value="AA">Account to Account</asp:ListItem>
                                            <asp:ListItem Value="BT">Bulk Transfer</asp:ListItem>
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
                                        <asp:DropDownList ID="uiDdlStatus" runat="server" Width="120px">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="P">Proposed</asp:ListItem>
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
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
                    <cc1:ProgressUpdatePanel ID="ProgressUpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgTransferPosition" runat="server" AutoGenerateColumns="False"
                                Width="100%" MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="TransferNo,ApprovalStatus"
                                OnPageIndexChanging="uiDgTransferPosition_PageIndexChanging" OnRowDataBound="uiDgTransferPosition_RowDataBound"
                                OnSorting="uiDgTransferPosition_Sorting" AllowPaging="True" AllowSorting="True"
                                PageSize="15">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="EntryTransferSPAApproval.aspx?menu=<%# Request.QueryString["menu"] %>&eType=edit&eID=<%# DataBinder.Eval(Container.DataItem, "TransferSpaID")  %>">
                                                Edit </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Transfer ID" DataField="TransferID" SortExpression="TransferID"
                                        InsertVisible="False" ReadOnly="True" Visible="False"></asp:BoundField>
                                    <asp:BoundField DataField="TransferNo" HeaderText="Transfer No" SortExpression="TransferNo" />
                                    <asp:BoundField DataField="TransferDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Transfer Date"
                                        SortExpression="TransferDate">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Transfer Type" SortExpression="TransferType">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblTransferType" runat="server" Text='<%# Bind("TransferType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TransferType") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Clearing Member Receiver" SortExpression="DestCMID">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblCMID" runat="server" Text='<%# Bind("DestCMID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DestCMID") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTransferPosition" runat="server" SelectMethod="GetTransferSpaPositionByTransferDateDestCMTypeApproval"
                                TypeName="TransferSpa" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarPickUpTransfer" Name="transferDate" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="CtlClearingMemberLookup1" Name="destClearingMember"
                                        PropertyName="LookupTextBoxID" Type="Decimal" />
                                    <asp:ControlParameter ControlID="uiDdlTransferType" Name="transferType" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlStatus" Name="approvalStatus" PropertyName="SelectedValue"
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
            <tr>
                <td id="tdImage" runat="server">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
