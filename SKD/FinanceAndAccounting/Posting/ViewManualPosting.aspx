<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManualPosting.aspx.cs" Inherits="WebUI_New_ViewManualPosting" %>

<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <h1>View Manual Posting</h1>
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
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
                                        Posting No.
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="uiTxtPostingNo" runat="server"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Posting Date
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Ledger Type</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlLedgerType" runat="server" CssClass="required">
                                            <asp:ListItem Value="M">Clearing Member</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Journal Type</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="uiDdlJournalType" runat="server" CssClass="required">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="A">Automatic</asp:ListItem>
                                            <asp:ListItem Value="M" Selected="True">Manual</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Description</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="uiTxtDescription" runat="server" Width="250px"></asp:TextBox>
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
                                            <asp:ListItem Value="A">Approved</asp:ListItem>
                                            <asp:ListItem Value="R">Rejected</asp:ListItem>
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
                          
                            <asp:GridView ID="uiDgPosting" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AllowPaging="True"
                                AllowSorting="True" DataKeyNames="JournalNo,TransactionDate,LedgerType,ApprovalStatus"
                                PageSize="15" OnPageIndexChanging="uiDgPosting_PageIndexChanging" OnSorting="uiDgPosting_Sorting"
                                OnRowDataBound="uiDgPosting_RowDataBound">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <a href="EntryManualPosting.aspx?eType=edit&eID=<%# DataBinder.Eval(Container.DataItem, "JournalHeaderID")  %>">
                                                Detail</a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JournalHeaderID" HeaderText="JournalHeaderID" InsertVisible="False"
                                        SortExpression="JournalHeaderID" Visible="False" />
                                    <asp:BoundField DataField="JournalNo" HeaderText="Journal No" SortExpression="JournalNo"
                                        ReadOnly="True" />
                                    <asp:BoundField DataField="TransactionDate" HeaderText="Posting Date" ReadOnly="True"
                                        SortExpression="TransactionDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="HeaderDescription">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblDescription" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem, "HeaderDescription").ToString().Length > 25 ? DataBinder.Eval(Container.DataItem, "HeaderDescription").ToString().Substring(0, 25) : DataBinder.Eval(Container.DataItem, "HeaderDescription").ToString())  %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="ApprovalStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourcePosting" runat="server" SelectMethod="GetJournalHeaderByJournalNoTransDateDescriptionApprovalStatus"
                                TypeName="Journal" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiTxtPostingNo" Name="journalNo" PropertyName="Text"
                                        Type="Int32" />
                                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="transactionDate" PropertyName="Text"
                                        Type="DateTime" />
                                    <asp:ControlParameter ControlID="uiDdlLedgerType" Name="ledgerType" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlJournalType" Name="journalType" 
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="uiTxtDescription" Name="headerDescription" 
                                        PropertyName="Text" Type="String" />
                                    <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="uiBtnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                        AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <img alt="" src="../../Images/ajax-loader2.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
