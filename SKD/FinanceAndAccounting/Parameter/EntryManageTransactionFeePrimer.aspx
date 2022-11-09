<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageTransactionFeePrimer.aspx.cs" Inherits="FinanceAndAccounting_Parameter_EntryManageTransactionFeePrimer" Title="Untitled Page" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc1" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Transaction Fee Primer</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
                    <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                </tr>
    <tr>
        <td>
        <div class="shadow_view">
            <div class="box_view">
            <table class="table-row">
                
                <tr>
                    <td style="width:150px">
                            Exchange</td>
                    <td style="width:10px">
                        :</td>
                    <td colspan="2">
                            <asp:DropDownList ID="uiDdlExchange" runat="server" DataSourceID="odsExchange" 
                                DataTextField="ExchangeCode" DataValueField="ExchangeId">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsExchange" runat="server" 
                                OldValuesParameterFormatString="original_{0}" 
                                SelectMethod="GetDataExchangePrimer" 
                                TypeName="ExchangeDataTableAdapters.ExchangeDdlTableAdapter">
                            </asp:ObjectDataSource>
                        </td>
                </tr>
                
                <tr>
                    <td style="width:150px">
                            Commodity Code</td>
                    <td style="width:10px">
                        :</td>
                    <td colspan="2">
                            <uc1:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                        </td>
                </tr>
                <tr>
                    <td>
                            CM&nbsp; Type</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <asp:DropDownList runat="server" CssClass="required" ID="uiDdlCmType" Width="65px">
                            <asp:ListItem Text="Broker" Value="B" />
                            <asp:ListItem Text="Trader" Value="T" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Effective Start Date</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
                                                            &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td colspan="2">
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Upper Limit</td>
                    <td>
                        :</td>
                    <td class="style2">
                        <cc2:FilteredTextBox ID="uiTxtUpperLimit" CssClass="number" 
                            FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style2">
                        Transaction</td>
                    <td>
                        Cash Settlement</td>
                </tr>
                <tr>
                    <td>
                            Clearing Fee</td>
                    <td>
                        :</td>
                    <td class="style2">
                        <cc2:FilteredTextBox ID="uiTxtClearingFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtCSClearingFee" CssClass="number" 
                            FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Exchange Fee</td>
                    <td>
                        :</td>
                    <td class="style2">
                        <cc2:FilteredTextBox ID="uiTxtExchangeFee" FilterTextBox="Money" CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtCSExchangeFee" FilterTextBox="Money" 
                            CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Compensation Fund</td>
                    <td>
                        :</td>
                    <td class="style2">
                        <cc2:FilteredTextBox ID="uiTxtCompensationFund" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtCSCompensationFund" CssClass="number" 
                            FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Third Party Fee</td>
                    <td>
                        :</td>
                    <td class="style2">
                        <cc2:FilteredTextBox ID="uiTxtThirdPartyFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td>
                        <cc2:FilteredTextBox ID="uiTxtCSThirdPartyFee" CssClass="number" 
                            FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr id="trAction" runat="server">
                    <td>
                            Action</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="2">
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="2">
                        <asp:TextBox ID="uiTxtApprovalDesc" runat="server" CssClass="required" 
                            Height="100px" MaxLength="100" Width="400px" 
                            TextMode="MultiLine"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                runat="server" ControlToValidate="uiTxtApprovalDesc" 
                                ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style2" colspan="2">
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click"  />
                        <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                                Text="      Delete" onclick="uiBtnDelete_Click" />
     <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                            Text="      Cancel" onclick="uiBtnCancel_Click" CausesValidation="False" />
                    </td>
                </tr>
                </table>
              </div>
            </div>
            </td>
        </tr>
        
    </table>

</asp:Content>
