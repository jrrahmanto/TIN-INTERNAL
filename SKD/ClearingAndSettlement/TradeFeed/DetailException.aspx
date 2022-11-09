<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailException.aspx.cs" Inherits="ClearingAndSettlement_TradeFeed_DetailException" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Detail Exception</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
 <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
<table class="table-row">
 <form id="form1">
    <div>
    
    </div>
    <asp:ObjectDataSource ID="odsException" runat="server" SelectMethod="GetData" 
        TypeName="TradefeedException"></asp:ObjectDataSource>
    <asp:DetailsView ID="dvException" runat="server" AutoGenerateRows="False" 
        DataKeyNames="TradeFeedID,BusinessDate,ExchangeID,ApprovalStatus" 
        DataSourceID="odsException" Height="50px" Width="490px">
        <Fields>
            <asp:BoundField DataField="TradeFeedID" HeaderText="TradeFeed ID" 
                ReadOnly="True" SortExpression="TradeFeedID" />
            <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" 
                ReadOnly="True" SortExpression="BusinessDate" />
            <asp:BoundField DataField="ExchangeID" HeaderText="Exchange" ReadOnly="True" 
                SortExpression="ExchangeID" />
            <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" 
                ReadOnly="True" SortExpression="ApprovalStatus" />
            <asp:BoundField DataField="Message" HeaderText="Message" 
                SortExpression="Message" />
            <asp:BoundField DataField="ApprovalDesc" HeaderText="Approval Description" 
                SortExpression="ApprovalDesc" />
            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" 
                SortExpression="CreatedBy" />
            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" 
                SortExpression="CreatedDate" />
            <asp:BoundField DataField="LastUpdatedBy" HeaderText="Last Updated By" 
                SortExpression="LastUpdatedBy" />
            <asp:BoundField DataField="LastUpdatedDate" HeaderText="Last Updated Date" 
                SortExpression="LastUpdatedDate" />
        </Fields>
    </asp:DetailsView>
    <br />
    <br />
    </form>
    </table>
 </div>
                </div>
            </td>
        </tr>
        </table>
 
</asp:Content>

