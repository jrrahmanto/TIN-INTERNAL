<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailSettlementPriceException.aspx.cs" Inherits="ClearingAndSettlement_MasterData_DetailSettlementPriceException"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Settlement Price Exception</h1>
   <table cellpadding="1" cellspacing="1" style="width:100%;">
   <tr>
            <td>
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
            <td >
                <asp:DetailsView ID="uiDvSettlementPriceException" runat="server" 
                    AutoGenerateRows="False" BackColor="White" BorderColor="#E7E7FF" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    DataKeyNames="SPID,BusinessDate,ExchangeID,ApprovalStatus" 
                    DataSourceID="ObjectDataSourceSettlementPriceException" GridLines="Horizontal" 
                    Height="50px" Width="790px">
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <Fields>
                        <asp:BoundField DataField="SPID" HeaderText="Settlement Price ID" 
                            ReadOnly="True" SortExpression="SPID" />
                        <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" 
                            ReadOnly="True" SortExpression="BusinessDate" 
                            DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField DataField="ExchangeCode" HeaderText="Exchange" ReadOnly="True" 
                            SortExpression="ExchangeCode" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" 
                            ReadOnly="True" SortExpression="ApprovalStatus" />
                        <asp:BoundField DataField="Message" HeaderText="Message" 
                            SortExpression="Message" />
                         <asp:TemplateField HeaderText="Approval Desc" SortExpression="ApprovalDesc">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" CssClass="required" MaxLength="100" runat="server" Height="78px" 
                                TextMode="MultiLine" Width="400px" Text='<%# Bind("ApprovalDesc") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" MaxLength="100" runat="server" Height="78px" 
                                TextMode="MultiLine" Width="400px" Text='<%# Bind("ApprovalDesc") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" 
                            SortExpression="CreatedBy" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" 
                            SortExpression="CreatedDate" DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField DataField="LastUpdatedBy" HeaderText="Last Updated By" 
                            SortExpression="LastUpdatedBy" />
                        <asp:BoundField DataField="LastUpdatedDate" HeaderText="Last Updated Date" 
                            SortExpression="LastUpdatedDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    </Fields>
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <EditRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="ObjectDataSourceSettlementPriceException" runat="server" 
                    SelectMethod="GetData" TypeName="SettlementPriceException">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="exchangeID" QueryStringField="exchangeId" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="businessDate" QueryStringField="businessDate" 
                            Type="DateTime" />
                        <asp:QueryStringParameter Name="SPID" QueryStringField="SPID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:DetailsView ID="uiDvRawSettlementPrice" runat="server" AutoGenerateRows="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" DataKeyNames="ExchangeId,businessDate,SPID" 
                    DataSourceID="ObjectDataSourceRawSettlementPriceException" GridLines="Horizontal" 
                    Height="50px" Width="790px">
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <Fields>
                        <asp:BoundField DataField="ExchangeCode" HeaderText="Exchange" ReadOnly="True" 
                            SortExpression="ExchangeCode" />
                        <asp:BoundField DataField="SPID" HeaderText="Settlement Price ID" 
                            ReadOnly="True" SortExpression="SPID" />
                        <asp:BoundField DataField="BusinessDate" HeaderText="Business Date" 
                            ReadOnly="True" SortExpression="BusinessDate" 
                            DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField DataField="ReceivedTime" HeaderText="Received Time" 
                            SortExpression="ReceivedTime" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss.fff}" />
                        <asp:BoundField DataField="MonthContract" HeaderText="Month Contract" 
                            SortExpression="MonthContract" />
                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" 
                            SortExpression="ProductCode" />
                        <asp:BoundField DataField="SettlementPrice" HeaderText="Settlement Price" 
                            SortExpression="SettlementPrice" DataFormatString="{0:#,##0.##}" />
                    </Fields>
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <EditRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="ObjectDataSourceRawSettlementPriceException" runat="server" 
                    SelectMethod="GetData" TypeName="SettlementPrice">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="exchangeID" QueryStringField="exchangeId" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="SPID" QueryStringField="SPID" 
                            Type="Decimal" />
                        <asp:QueryStringParameter Name="businessDate" QueryStringField="businessDate" 
                            Type="DateTime" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                            <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                    Text="     Approve" onclick="uiBtnApprove_Click"  />
                            <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" onclick="uiBtnReject_Click" 
                    />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                                Text="      Cancel" onclick="uiBtnCancel_Click" 
                            />
                        </td>
        </tr>
    </table>
    </div>
            </div>
            </td>
        </tr>
        </table>
</asp:Content>

