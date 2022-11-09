<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryTransferSPAApproval.aspx.cs" Inherits="ClearingAndSettlement_TransferPosition_EntryTransferSPAApproval" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Transfer Approval</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                <table  class="table-row">
                    <tr>
                        <td style="width:200px">
                            Clearing Member Sender</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <asp:ObjectDataSource ID="ObjectDataSourceTransferPosition" 
                                runat="server" SelectMethod="GetTransferPositionByTransferId" 
                                TypeName="TransferSpa">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="transferSpaId" QueryStringField="eID" 
                                        Type="Decimal" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookupSource" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Clearing Member Receiver</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookupDest" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtDescription" runat="server" Height="84px" 
                                TextMode="MultiLine" Width="304px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Approval Description</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="84px" 
                                TextMode="MultiLine" Width="400px"></asp:TextBox>
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
            <td id="tdImage" runat="server">
                <asp:GridView ID="uiDgTransferRequest" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" PageSize="50" 
                    
                    DataKeyNames="InvestorID,ContractID,TradePosition,Position,TransferSpaID,Price" 
                    onrowdatabound="uiDgTransferRequest_RowDataBound" 
                    onselectedindexchanging="uiDgTransferRequest_SelectedIndexChanging" 
                    AllowPaging="True" onpageindexchanging="uiDgTransferRequest_PageIndexChanging">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField SelectText="Remove" ShowSelectButton="True" Visible="False" />
                        <asp:BoundField DataField="TransferReqID" HeaderText="Transfer Req ID" 
                            SortExpression="TransferReqID" Visible="False" />
                        <asp:BoundField DataField="TransferSpaID" HeaderText="Transfer ID" ReadOnly="True" 
                            SortExpression="TransferSpaID" Visible="False" />
                        <asp:BoundField DataField="InvestorID" HeaderText="Account" ReadOnly="True" 
                            SortExpression="InvestorID" />
                        <asp:BoundField DataField="ContractID" HeaderText="Contract" ReadOnly="True" 
                            SortExpression="ContractID" />
                        <asp:BoundField DataField="TradePosition" HeaderText="Trade Position" 
                            ReadOnly="True" SortExpression="TradePosition" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Position" HeaderText="Position" ReadOnly="True" 
                            SortExpression="Position" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Price" HeaderText="Price" ReadOnly="True" 
                            SortExpression="Price" DataFormatString="{0:#,##0.##########}" >
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" 
                            SortExpression="Quantity">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceTransferRequest" runat="server" 
                    SelectMethod="GetTransferRequestByTransferID" TypeName="TransferSpa">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="transferSpaId" QueryStringField="eID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                    Text="     Approve" onclick="uiBtnApprove_Click" />
                <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" 
                    onclick="uiBtnReject_Click" />
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                    onclick="uiBtnCancel_Click" Text="      Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>

