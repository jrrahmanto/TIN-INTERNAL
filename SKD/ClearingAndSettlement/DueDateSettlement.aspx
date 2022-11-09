<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DueDateSettlement.aspx.cs" Inherits="ClearingAndSettlement_DueDateSettlement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Due Date Settlement</h1>
    <asp:Panel ID="pnlException" runat="server">
        <form id="form2">
            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                        </asp:BulletedList>
            <div>
                <table class="table-datagrid">
                    <tr>
                        <td valign="top" style="width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="uiDgDueDate" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="ProgressID" MouseHoverRowHighlightEnabled="True"
                                        RowHighlightColor="" Width="100%" AllowPaging="True" AllowSorting="True"
                                        OnPageIndexChanging="uiDgDueDate_PageIndexChanging"
                                        OnSorting="uiDgDueDate_Sorting" PageSize="50" 
                                        OnRowDataBound="uiDgDueDate_RowDataBound" EnableModelValidation="True">
                                        <RowStyle CssClass="tblRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Detail">
                                                <ItemTemplate>
                                                    <a href='EntryDueDateSettlement.aspx?progressId=<%# DataBinder.Eval(Container.DataItem, "ProgressID") %>'>Detail
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BusinessDate" HeaderText="BusinessDate"
                                                DataFormatString="{0:dd-MM-yyyy}" />
                                            <asp:BoundField DataField="Price" HeaderText="TradePrice"
                                                DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField DataField="Volume" HeaderText="Quantity"
                                                SortExpression="Volume" DataFormatString="{0:#,##0.00}">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ExchangeRef" HeaderText="ExchangeRef"
                                                SortExpression="ExchangeRef" />
                                            <asp:BoundField DataField="SellerId" HeaderText="SellerCode" 
                                                SortExpression="SellerId" />
                                            <asp:BoundField DataField="BuyerId" HeaderText="BuyerCode"
                                                SortExpression="BuyerId"  >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ContractMonth" HeaderText="Contract Month"
                                                ItemStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BuyerOutstanding" HeaderText="Outstanding"
                                                DataFormatString="{0:#,##0.00}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DueType" HeaderText="DueType" />
                                        </Columns>
                                        <EmptyDataTemplate>No Records Found</EmptyDataTemplate>
                                        <HeaderStyle CssClass="tblHeaderStyle" ForeColor="White" />
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
        </form>
    </asp:Panel>

</asp:Content>

