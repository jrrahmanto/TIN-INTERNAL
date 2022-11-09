<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryBankTransactionException.aspx.cs" Inherits="FinanceAndAccounting_EntryBankTransactionException" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage Bank Transaction Exception</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="#FF3300" 
                                Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
        <tr>
            <td>
             <div class="shadow_view">
            <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td colspan="3">
                            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                                CellPadding="4" DataSourceID="ObjectDataSourceRawBankTransaction" 
                                ForeColor="#333333" GridLines="None" Height="50px" Width="536px">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
                                <RowStyle BackColor="#EFF3FB" />
                                <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <Fields>
                                    <asp:BoundField DataField="TransactionDate" HeaderText="TransactionDate" 
                                        SortExpression="TransactionDate" />
                                    <asp:BoundField DataField="TransactionSeq" HeaderText="TransactionSeq" 
                                        SortExpression="TransactionSeq" />
                                    <asp:BoundField DataField="BankID" HeaderText="BankID" 
                                        SortExpression="BankID" />
                                    <asp:BoundField DataField="TransactionTime" HeaderText="TransactionTime" 
                                        SortExpression="TransactionTime" />
                                    <asp:BoundField DataField="ReceiveTime" HeaderText="ReceiveTime" 
                                        SortExpression="ReceiveTime" />
                                    <asp:BoundField DataField="BankReference" HeaderText="BankReference" 
                                        SortExpression="BankReference" />
                                    <asp:BoundField DataField="SourceBank" HeaderText="SourceBank" 
                                        SortExpression="SourceBank" />
                                    <asp:BoundField DataField="SourceAccount" HeaderText="SourceAccount" 
                                        SortExpression="SourceAccount" />
                                    <asp:BoundField DataField="DestinationBank" HeaderText="DestinationBank" 
                                        SortExpression="DestinationBank" />
                                    <asp:BoundField DataField="DestinationAccount" HeaderText="DestinationAccount" 
                                        SortExpression="DestinationAccount" />
                                    <asp:BoundField DataField="MutationType" HeaderText="MutationType" 
                                        SortExpression="MutationType" />
                                    <asp:BoundField DataField="TransactionType" HeaderText="TransactionType" 
                                        SortExpression="TransactionType" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" 
                                        SortExpression="Amount" />
                                    <asp:BoundField DataField="News" HeaderText="News" SortExpression="News" />
                                </Fields>
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:DetailsView>
                            <asp:ObjectDataSource ID="ObjectDataSourceRawBankTransaction" runat="server" 
                                SelectMethod="GetRawBankTransactionByTransDateSeqBankID" TypeName="Bank">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="transactionDate" QueryStringField="transDate" 
                                        Type="DateTime" />
                                    <asp:QueryStringParameter Name="transactionSeq" QueryStringField="transSeq" 
                                        Type="Int32" />
                                    <asp:QueryStringParameter Name="bankID" QueryStringField="bankId" 
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width:150px">
                            Account Type</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlAccountType" runat="server" Width="263px" 
                                AppendDataBoundItems="True" Height="16px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="RNI">Rekening Nasabah Investor</asp:ListItem>
                                <asp:ListItem Value="RTI">Rekening Terpisah Investor</asp:ListItem>
                                <asp:ListItem Value="RTPS">Rekening Terpisah Pialang Seggregated</asp:ListItem>
                                <asp:ListItem Value="RTLK">Rekening Terpisah Lembaga Kliring</asp:ListItem>
                                <asp:ListItem Value="RTA">Rekening Terpisah Administrasi</asp:ListItem>
                                <asp:ListItem Value="RTPU">Rekening Terpisah Pialang Unseggregated</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Clearing Member ID</td>
                        <td>
                            :</td>
                        <td id="uiTxtCM">

                            <uc2:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />                        </td>
                    </tr>
                    <tr>
                        <td>
                            Approval Description</td>
                        <td>
                            :</td>
                        <td id="uiTxtCM">

                            <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="100px" Width="350px" 
                                MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                        &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxtApprovalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" 
                                onclick="uiBtnSave_Click"  />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                                onclick="uiBtnCancel_Click" CausesValidation="False"/>
                                
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
</asp:Content>

