<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewSettlementPriceException.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewSettlementPriceException"  %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View Settlement Price Exception</h1>
    <asp:Panel ID="pnlException" runat="server">
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
                    </asp:BulletedList>
                </td>
            </tr>
             <tr>
            <td>
             <div class="shadow_view">
            <div class="box_view">
            <table class="table-row">
            <tr>
                <td style="width:100px">
                    Exchange
                </td>
                <td style="width:10px">
                    :
                </td>
                <td>
                    <asp:DropDownList ID="ddlExchange" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Business Date
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
                    Approval Status
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:DropDownList ID="ddlApprovalStatus" runat="server">
                        <asp:ListItem Selected="True" Value="P">Proposed</asp:ListItem>
                        <asp:ListItem Value="A">Approved</asp:ListItem>
                        <asp:ListItem Value="R">Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:ObjectDataSource ID="odsRawTradefeed" runat="server" SelectMethod="GetData"
                        TypeName="Tradefeed">
                        <SelectParameters>
                            <asp:SessionParameter Name="exchangeID" SessionField="SelectedExchangeID" Type="Decimal" />
                            <asp:SessionParameter Name="tradefeedID" SessionField="SelectedTradefeedID" Type="Decimal" />
                            <asp:SessionParameter Name="businessDate" SessionField="SelectedBusinessDate" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="        Search"
                        CssClass="button_search" />
                </td>
            </tr>
        </table>
        </div>
                </div>
            </td>
        </tr>
        <table class="table-datagrid">
        <tr>
        <td>
        <div style="text-align: center" runat="server" id="pnlRaw">
            <asp:GridView ID="uiDgSettlementPriceException" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" MouseHoverRowHighlightEnabled="True" OnPageIndexChanging="uiDgSettlementPriceException_PageIndexChanging"
                OnRowDataBound="uiDgSettlementPriceException_RowDataBound" OnSorting="uiDgSettlementPriceException_Sorting"
                PageSize="15" RowHighlightColor="" Width="100%" DataKeyNames="SPID,BusinessDate,ExchangeID,ApprovalStatus">
                <RowStyle CssClass="tblRowStyle" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='DetailSettlementPriceException.aspx?menu=<%# Request.QueryString["menu"] %>&SPID=<%# Eval("SPID") %>&exchangeId=<%# Eval("ExchangeID") %>&businessDate=<%# Eval("BusinessDate") %>&approvalStatus=<%# Eval("ApprovalStatus") %>'>
                                Detail </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SPID" HeaderText="SettlemenPriceID" ReadOnly="True"
                        SortExpression="SPID" />
                    <asp:BoundField DataField="BusinessDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="BusinessDate"
                        ReadOnly="True" SortExpression="BusinessDate" />
                    <asp:TemplateField HeaderText="ExchangeID" SortExpression="ExchangeID">
                        <EditItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("ExchangeID") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="uiLblExchangeId" runat="server" Text='<%# Bind("ExchangeID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ApprovalStatus" SortExpression="ApprovalStatus">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
                </Columns>
                <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSourceSettlementPriceException" 
                runat="server" SelectMethod="GetSettlementPriceExceptionByExchangeIdBusinessDateApproval"
                TypeName="SettlementPriceException" 
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlExchange" Name="exchangeId" PropertyName="SelectedValue"
                        Type="Decimal" />
                    <asp:ControlParameter ControlID="CtlCalendarPickUp1" Name="businessDate" PropertyName="Text"
                        Type="DateTime" />
                    <asp:ControlParameter ControlID="ddlApprovalStatus" Name="approvalStatus" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        </td>
        </tr>
        </table>
    </asp:Panel>
</asp:Content>

