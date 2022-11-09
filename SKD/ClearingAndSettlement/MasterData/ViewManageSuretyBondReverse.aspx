<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewManageSuretyBondReverse.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewManageSuretyBondReverse" %>
    <%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<%@ Register src="../../Lookup/CtlBondIssuerLookup.ascx" tagname="CtlBondIssuerLookup" tagprefix="uc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlBankAccountSuretyBondLookup.ascx" tagname="CtlBankAccountSuretyBondLookup" tagprefix="uc3" %>
<%@ Register Src="~/Lookup/CtlInvestorLookup.ascx" TagName="CtlInvestorLookup"
    TagPrefix="uc4" %>
<%@ Register Src="~/Lookup/CtlSuretyBondLookup.ascx" TagName="CtlSuretyBondLookup"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Manage Reverse Surety Bond Amount</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td>
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                                    <td style="width:100px">
                                        Business Date
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc2:CtlCalendarPickUp ID="uiDtpBusDate" runat="server" />        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                            Exchange Ref</td>
                                    <td>
                                        :</td>
                                    <td>
                                            <asp:TextBox ID="uiTxtExcRef" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width:100px">
                                            Account Code

                                    </td>
                                    <td style="width:10px">
                                        :</td>
                                    <td>
                                         <uc4:CtlInvestorLookup ID="CtlInvestorLookup1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100px">
                                        Bond Serial Number
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                         <uc5:CtlSuretyBondLookup ID="CtlBondSNLookup" runat="server" />
                                        
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
                                        <asp:Button ID="uiBtnHistorical" CssClass="button_request" runat="server" Text="     Historical"
                                            OnClick="uiBtnHistorical_Click" />
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
                            <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="uiDgSurety" runat="server" AutoGenerateColumns="False" Width="100%"
                                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                                        AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="EODSuretyBondUsageId,ApprovalStatus"
                                        OnPageIndexChanging="uiDgSurety_PageIndexChanging" OnSorting="uiDgSurety_Sorting">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Reverse">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("EODSuretyBondUsageId", "~/ClearingAndSettlement/MasterData/EntrySuretyBondReverse.aspx?eType=edit&eID={0}") %>'
                                                        Text="Reverse" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="TransactionDate" DataFormatString="{0:dd-MMM-yyyy}"  />
                                            <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" SortExpression="ExchangeRef" />
                                            <asp:BoundField DataField="AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                                            <asp:BoundField DataField="ParticipantName" HeaderText="ParticipantName" SortExpression="ParticipantName" >
                                                </asp:BoundField>
                                            <asp:BoundField DataField="ContraAccount" HeaderText="ContraAccount" SortExpression="ContraAccount" />
                                            <asp:BoundField DataField="ContraParticipantName" HeaderText="Contra Participant Name" SortExpression="ContraParticipantName" />
                                             <asp:BoundField DataField="BondSerialNo" HeaderText="Bond SerialNo" SortExpression="BondSerialNo" />
                                            <asp:BoundField DataField="BondIssuerName" HeaderText="Bond IssuerName" SortExpression="BondIssuerName" />
                                            <asp:BoundField DataField="UsageAmount" HeaderText="Usage Amount" SortExpression="UsageAmount" DataFormatString="{0:#,##0.##########}" />
                                            <asp:BoundField DataField="RemainAmount" HeaderText="Bond Remain Amount" SortExpression="BondRemainAmount" DataFormatString="{0:#,##0.##########}"/>
                                            <asp:BoundField DataField="BusinessDateReverse" HeaderText="BusinessDate Reverse" SortExpression="BusinessDateReverse" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="DefaultDate" HeaderText="Default Date" SortExpression="DefaultDate" DataFormatString="{0:dd-MMM-yyyy}"/>
                                            <%--<asp:BoundField DataField="SellerDefault" HeaderText="Seller Default" SortExpression="SellerDefault" />--%>
                                            <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Status" SortExpression="ApprovalStatusDesc">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>No Records Available</EmptyDataTemplate>
                                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourceSurety" runat="server" SelectMethod="GetSuretyBondReverse"
                                        TypeName="SuretyBondReverse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="uiDtpBusDate" Name="busdate" PropertyName="Text"
                                                Type="DateTime" />
                                            <asp:ControlParameter ControlID="uiTxtExcRef" Name="exchangeRef" PropertyName="Text"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="CtlBondSNLookup" Name="bondSN" PropertyName="LookupTextBox"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="CtlInvestorLookup1" Name="accCode" PropertyName="LookupTextBox"
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
        </table>
    </asp:Panel>
</asp:Content>
