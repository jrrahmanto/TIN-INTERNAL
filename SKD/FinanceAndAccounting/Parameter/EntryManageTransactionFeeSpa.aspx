<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManageTransactionFeeSpa.aspx.cs" Inherits="FinanceAndAccounting_Parameter_EntryManageTransactionFeeSpa" Title="Untitled Page" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc1" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc2" %>

<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 583px;
        }
        .style2
        {
        }
        .style3
        {
            width: 180px;
        }
        .style4
        {
            width: 182px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Transaction Fee SPA</h1>
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
                    <td>
                            Exchange</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="3">
                        <asp:DropDownList ID="uiDdlExchange" runat="server" DataSourceID="odsExchange" 
                            DataTextField="ExchangeCode" DataValueField="ExchangeId">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsExchange" runat="server" 
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataExchangeSpa" 
                            TypeName="ExchangeDataTableAdapters.ExchangeDdlTableAdapter">
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>Mode Fee</td>
                    <td>:</td>
                    <td class="style2" colspan="3">
                    <asp:DropDownList runat="server" CssClass="required" ID="uiDdlModeFee" Width="65px" 
                            AutoPostBack="True"
                            onload="uiDdlModeFee_SelectedIndexChanged" 
                            onselectedindexchanged="uiDdlModeFee_SelectedIndexChanged">
                            <asp:ListItem Text="Per Lot" Value="LOT" />
                            <asp:ListItem Text="Percentage" Value="PER" />
                            <asp:ListItem Text="Max" Value="MAX"/>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                            CM&nbsp; Type</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="3">
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
                    <td class="style2" colspan="3">
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
                                                            &nbsp;</td>
                </tr>
                <tr>
                    <td>
                            Effective End Date</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="3">
                        <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveEndDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Upper Limit</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="3">
                        <cc2:FilteredTextBox ID="uiTxtUpperLimit" CssClass="number-required" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Mode Kurs</td>
                    <td>:</td>
                    <td class="style2" colspan="3">
                    <asp:DropDownList runat="server" CssClass="required" ID="uiDdlModeKurs" Width="65px">
                            <asp:ListItem Text="Float" Value="REG" />
                            <asp:ListItem Text="PEG" Value="PEG" />
                        </asp:DropDownList>    
                    </td>
                </tr>
                <tr>
                    <td>Fixed Kurs</td>
                    <td>:</td>
                    <td class="style2" colspan="3">
                     <cc2:FilteredTextBox ID="uiTxtFixedKurs" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Contract Ref Kurs</td>
                    <td>:</td>
                    <td class="style2" colspan="3">
                    <uc6:CtlContractCommodityLookup ID="uiCtlContractRefKurs" runat="server" />                    
                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style4">
                        Transaction Fixed</td>
                    <td class="style3">Transaction Percentage</td>
                    <td>
                        Cash Settlement</td>
                </tr>
                <tr>
                    <td>
                            Clearing Fee</td>
                    <td>
                        :</td>
                    <td class="style4">
                        <cc2:FilteredTextBox ID="uiTxtClearingFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td class="style3">
                        <cc2:FilteredTextBox ID="uiTxtPctClearingFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
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
                    <td class="style4">
                        <cc2:FilteredTextBox ID="uiTxtExchangeFee" FilterTextBox="Money" CssClass="number" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td class="style3">
                        <cc2:FilteredTextBox ID="uiTxtPctExchangeFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
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
                    <td class="style4">
                        <cc2:FilteredTextBox ID="uiTxtCompensationFund" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td class="style3">
                        <cc2:FilteredTextBox ID="uiTxtPctCompensationFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
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
                    <td class="style4">
                        <cc2:FilteredTextBox ID="uiTxtThirdPartyFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
                    </td>
                    <td class="style3">
                        <cc2:FilteredTextBox ID="uiTxtPctThirdPartyFee" CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-" runat="server"></cc2:FilteredTextBox>
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
                    <td class="style2" colspan="3">
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        :</td>
                    <td class="style2" colspan="3">
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
                    <td class="style2" colspan="3">
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