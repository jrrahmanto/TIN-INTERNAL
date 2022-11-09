<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewHistoricalSuretyBondReverse.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewHistoricalSuretyBondReverse" %>
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
        <h1>View Historical Reverse Surety Bond Amount</h1>
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
        </table>
        <table class="table-datagrid">
                    <tr>
                        <td>
                          
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="uiDgSurety" runat="server" AutoGenerateColumns="False" Width="100%"
                                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" AlslowPaging="True"
                                        AllowSorting="True" PageSize="15" AllowPaging="True" DataKeyNames="SuretyBondID,ApprovalStatus"
                                        OnPageIndexChanging="uiDgSurety_PageIndexChanging" OnSorting="uiDgSurety_Sorting">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            
                                            <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" SortExpression="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}"  />
                                            <asp:BoundField DataField="ExchangeRef" HeaderText="Exchange Ref" SortExpression="ExchangeRef" />
                                            <asp:BoundField DataField="Code" HeaderText="Account Code" SortExpression="Code" />
                                            <asp:BoundField DataField="ParticipantName" HeaderText="ParticipantName" SortExpression="ParticipantName" >
                                                </asp:BoundField>
                                            <asp:BoundField DataField="ContraAccount" HeaderText="ContraAccount" SortExpression="ContraAccount" />
                                            <asp:BoundField DataField="ContraParticipantName" HeaderText="Contra Participant Name" SortExpression="ContraParticipantName" />
                                             <asp:BoundField DataField="BondSerialNo" HeaderText="Bond SerialNo" SortExpression="BondSerialNo" />
                                            <asp:BoundField DataField="BondIssuerName" HeaderText="Bond IssuerName" SortExpression="BondIssuerName" />
                                            <asp:BoundField DataField="UsageAmount" HeaderText="Usage Amount" SortExpression="UsageAmount" DataFormatString="{0:#,##0.##########}" />
                                            <asp:BoundField DataField="BondRemainAmount" HeaderText="Bond Remain Amount" SortExpression="BondRemainAmount" DataFormatString="{0:#,##0.##########}"/>
                                            <asp:BoundField DataField="BusinessDateReverse" HeaderText="BusinessDate Reverse" SortExpression="BusinessDateReverse" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="DateDefault" HeaderText="Default Date" SortExpression="DateDefault" />
                                            <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Status" SortExpression="ApprovalStatusDesc">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>No Records Available</EmptyDataTemplate>
                                        <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourceSurety" runat="server" SelectMethod="GetHistoricalSuretyBondReverse"
                                        TypeName="SuretyBondReverse">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="uiDtpBusDate" Name="busdate" PropertyName="Text"
                                                Type="DateTime" />
                                            <asp:ControlParameter ControlID="uiTxtExcRef" Name="exchangeRef" PropertyName="Text"
                                                Type="String" />
                                            <asp:Parameter DefaultValue="A" Name="approvalStatus" 
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
