<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewContract.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewContract" %>

<%@ Register Src="../../Lookup/CtlCommodityLookup.ascx" TagName="CtlCommodityLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Controls/CtlMonthYear.ascx" TagName="CtlMonthYear" TagPrefix="uc2" %>
<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcPanel" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="uiBtnSearch">
        <h1>View Contract</h1>
        <table cellpadding="1" cellspacing="1" style="width: 100%;">
            <tr>
                <td colspan="3">
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
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
                                        Commodity Code
                                    </td>
                                    <td style="width:10px">
                                        :
                                    </td>
                                    <td>
                                        <uc1:CtlCommodityLookup ID="uiDtpCommID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contract Month / Year
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <uc2:CtlMonthYear ID="uiDtpMonthYear" runat="server" />
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
                                        &nbsp;
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
                    <cc1:ProgressUpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="uiDgContract" runat="server" AutoGenerateColumns="False" Width="100%"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="ContractID"
                                AllowPaging="True" AllowSorting="True" OnPageIndexChanging="uiDgContract_PageIndexChanging"
                                OnSorting="uiDgContract_Sorting">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("ContractID", "~/ClearingAndSettlement/MasterData/EntryContract.aspx?id={0}") %>'
                                                Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CommName" HeaderText="Commodity" SortExpression="CommName" />
                                    <asp:BoundField DataField="ContractYear" HeaderText="Contract Year" 
                                        SortExpression="ContractYear" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ContractMonth" HeaderText="Contract Month" 
                                        SortExpression="ContractMonth" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EffectiveStartDate" HeaderText="Effective Start Date"
                                        SortExpression="EffectiveStartDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <%--Yang ada di database adalah ApprovalDesc dan ApprovalStatus--%>
                                    <asp:BoundField DataField="ApprovalStatusDesc" HeaderText="Status" 
                                        SortExpression="ApprovalStatusDesc" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsContract" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
                                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataBySearchCriteria"
                                        TypeName="ContractDataTableAdapters.ContractTableAdapter" UpdateMethod="Update">
                                        <DeleteParameters>
                                            <asp:Parameter Name="Original_CommodityID" Type="Decimal" />
                                            <asp:Parameter Name="Original_ContractMonth" Type="Int32" />
                                            <asp:Parameter Name="Original_ContractYear" Type="Int32" />
                                            <asp:Parameter Name="Original_ApprovalStatus" Type="String" />
                                            <asp:Parameter Name="Original_EffectiveStartDate" Type="DateTime" />
                                        </DeleteParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="ContractSize" Type="Decimal" />
                                            <asp:Parameter Name="SettlementType" Type="String" />
                                            <asp:Parameter Name="Description" Type="String" />
                                            <asp:Parameter Name="Unit" Type="String" />
                                            <asp:Parameter Name="StartDate" Type="DateTime" />
                                            <asp:Parameter Name="EndSpot" Type="DateTime" />
                                            <asp:Parameter Name="StartSpot" Type="DateTime" />
                                            <asp:Parameter Name="PEG" Type="Decimal" />
                                            <asp:Parameter Name="VMIRCACalType" Type="String" />
                                            <asp:Parameter Name="SettlementFactor" Type="String" />
                                            <asp:Parameter Name="DayRef" Type="Int32" />
                                            <asp:Parameter Name="Divisor" Type="Decimal" />
                                            <asp:Parameter Name="MarginTender" Type="Decimal" />
                                            <asp:Parameter Name="MarginSpot" Type="Decimal" />
                                            <asp:Parameter Name="MarginRemote" Type="Decimal" />
                                            <asp:Parameter Name="CalSpreadRemoteMargin" Type="Decimal" />
                                            <asp:Parameter Name="IsKIE" Type="String" />
                                            <asp:Parameter Name="CreatedBy" Type="String" />
                                            <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                            <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                            <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                                            <asp:Parameter Name="EffectiveEndDate" Type="DateTime" />
                                            <asp:Parameter Name="ApprovalDesc" Type="String" />
                                            <asp:Parameter Name="HomeCurrencyID" Type="Decimal" />
                                            <asp:Parameter Name="CrossCurr" Type="String" />
                                            <asp:Parameter Name="CrossCurrProduct" Type="String" />
                                            <asp:Parameter Name="OriginalContractID" Type="Decimal" />
                                            <asp:Parameter Name="ActionFlag" Type="String" />
                                            <asp:Parameter Name="TenderReqType" Type="String" />
                                            <asp:Parameter Name="Original_CommodityID" Type="Decimal" />
                                            <asp:Parameter Name="Original_ContractMonth" Type="Int32" />
                                            <asp:Parameter Name="Original_ContractYear" Type="Int32" />
                                            <asp:Parameter Name="Original_ApprovalStatus" Type="String" />
                                            <asp:Parameter Name="Original_EffectiveStartDate" Type="DateTime" />
                                        </UpdateParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="uiDtpCommID" Name="CommodityID" PropertyName="LookupTextBoxID"
                                                Type="Decimal" />
                                            <asp:ControlParameter ControlID="uiDtpMonthYear" Name="ContractMonth" PropertyName="Month"
                                                Type="Int32" />
                                            <asp:ControlParameter ControlID="uiDtpMonthYear" Name="ContractYear" PropertyName="Year"
                                                Type="Int32" />
                                            <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="ApprovalStatus" PropertyName="SelectedValue"
                                                Type="String" />
                                        </SelectParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="CommodityID" Type="Decimal" />
                                            <asp:Parameter Name="ContractMonth" Type="Int32" />
                                            <asp:Parameter Name="ContractYear" Type="Int32" />
                                            <asp:Parameter Name="ApprovalStatus" Type="String" />
                                            <asp:Parameter Name="EffectiveStartDate" Type="DateTime" />
                                            <asp:Parameter Name="ContractSize" Type="Decimal" />
                                            <asp:Parameter Name="SettlementType" Type="String" />
                                            <asp:Parameter Name="Description" Type="String" />
                                            <asp:Parameter Name="Unit" Type="String" />
                                            <asp:Parameter Name="StartDate" Type="DateTime" />
                                            <asp:Parameter Name="EndSpot" Type="DateTime" />
                                            <asp:Parameter Name="StartSpot" Type="DateTime" />
                                            <asp:Parameter Name="PEG" Type="Decimal" />
                                            <asp:Parameter Name="VMIRCACalType" Type="String" />
                                            <asp:Parameter Name="SettlementFactor" Type="String" />
                                            <asp:Parameter Name="DayRef" Type="Int32" />
                                            <asp:Parameter Name="Divisor" Type="Decimal" />
                                            <asp:Parameter Name="MarginTender" Type="Decimal" />
                                            <asp:Parameter Name="MarginSpot" Type="Decimal" />
                                            <asp:Parameter Name="MarginRemote" Type="Decimal" />
                                            <asp:Parameter Name="CalSpreadRemoteMargin" Type="Decimal" />
                                            <asp:Parameter Name="IsKIE" Type="String" />
                                            <asp:Parameter Name="CreatedBy" Type="String" />
                                            <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                            <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                            <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                                            <asp:Parameter Name="EffectiveEndDate" Type="DateTime" />
                                            <asp:Parameter Name="ApprovalDesc" Type="String" />
                                            <asp:Parameter Name="HomeCurrencyID" Type="Decimal" />
                                            <asp:Parameter Name="CrossCurr" Type="String" />
                                            <asp:Parameter Name="CrossCurrProduct" Type="String" />
                                            <asp:Parameter Name="OriginalContractID" Type="Decimal" />
                                            <asp:Parameter Name="ActionFlag" Type="String" />
                                            <asp:Parameter Name="TenderReqType" Type="String" />
                                        </InsertParameters>
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
