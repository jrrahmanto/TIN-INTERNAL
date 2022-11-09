<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewOutGoingFeed.aspx.cs" Inherits="Administration_ViewOutGoingFeed"  %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Outgoing Feed</h1>
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
                                        Feed Type</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlFeedType" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="B">Bank Transaction</asp:ListItem>
                                            <asp:ListItem Value="A">Opening Balance</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Feed No</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtFeedNo" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="uiTxtFeedNo_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" TargetControlID="uiTxtFeedNo" 
                                            ValidChars="0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Business Date</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc2:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Submitted Status</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlSubmittedStatus" runat="server">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="Y">Submitted</asp:ListItem>
                                            <asp:ListItem Value="N">Queue</asp:ListItem>
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
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgOutGoingFeed" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                                AllowSorting="True" 
                                OnPageIndexChanging="uiDgOutGoingFeed_PageIndexChanging" 
                                OnSorting="uiDgOutGoingFeed_Sorting" 
                                onrowdatabound="uiDgOutGoingFeed_RowDataBound">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="feedID"
                                        DataNavigateUrlFormatString="~/Administration/DetailOutGoingFeed.aspx?eID={0}"
                                        Text="Detail" HeaderText="Detail">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:HyperLinkField>
                                    <asp:TemplateField HeaderText="Feed Type" SortExpression="FeedType" Visible="True"><ItemTemplate>
                                    <asp:Label ID="uiLblFeedType" runat="server" Text='<%# Bind("FeedType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:Label ID="uiLblFeedType" runat="server" Text='<%# Eval("FeedType") %>'></asp:Label>
                                    </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FeedNo" HeaderText="Feed No" SortExpression="FeedNo">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BusinessDate" HeaderText="Business Date"
                                        SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Submitted Status" SortExpression="SubmittedStatus" Visible="True"><ItemTemplate>
                                    <asp:Label ID="uiLblSubmittedStatus" runat="server" Text='<%# Bind("SubmittedStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:Label ID="uiLblSubmittedStatus" runat="server" Text='<%# Eval("SubmittedStatus") %>'></asp:Label>
                                    </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceOutGoingFeed" runat="server" SelectMethod="SelectOutGoingFeedByCriteria"
                                TypeName="OutGoing" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiDdlFeedType" Name="feedType" PropertyName="SelectedValue"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtFeedNo" Name="feedNo" PropertyName="Text"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="businessDate" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlSubmittedStatus" Name="submittedStatus" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
