<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryMutasiDepositDFS.aspx.cs" Inherits="FinanceAndAccounting_EntryMutasiDepositDFS" %>

<%@ Register src="../Controls/PdfViewer.ascx" tagname="PdfViewer" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>
<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc3" %>
<%@ Register src="../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc4" %>
<%@ Register src="../Lookup/CtlBankAccountLookup.ascx" tagname="CtlBankAccountLookup" tagprefix="uc5" %>
<%@ Register src="../Lookup/CtlBankLookup.ascx" tagname="CtlBankLookup" tagprefix="uc6" %>

<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcTextBox" tagprefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Mutation Deposit DFS</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     <tr >
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="red" 
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
                        <td >
                            <asp:ObjectDataSource ID="ObjectDataSourceBankTransaction" runat="server" 
                                SelectMethod="GetMutationDepositbyMutationNo" TypeName="MutationDeposit">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="mutationNo" QueryStringField="eID" 
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Entry Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:CtlCalendarPickUp ID="CtlCalendarPickUpReceiveTime" runat="server" />
                            
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td>
                            Bank Account</td>
                        <td>
                            :</td>
                        <td>
                            <uc5:CtlBankAccountLookup ID="CtlBankAccountLookupSource" runat="server" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            Amount</td>
                        <td>
                            :</td>
                        <td>
                            <%--<cc2:FilteredTextBox ID="uiTxtAmount" CssClass="number-required" 
                                FilterTextBox="Money" runat="server" ValidChar="0123456789.,-()"></cc2:FilteredTextBox>--%>
                            <asp:TextBox ID="uiTxtAmount" CssClass="number-required" runat="server"  />
                        </td>
                    </tr>
                    
                                      
                    <tr id="trApprovalDesc" runat="server">
                        <td>
                            Approval Description</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Height="100px" Width="400px" 
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
                                
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
                                
                        <asp:Button ID="uiBtnAddAndSave" runat="server" Text="     Save & Add" 
                                CssClass="button_addsave" onclick="uiBtnAddAndSave_Click" />
                                
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

