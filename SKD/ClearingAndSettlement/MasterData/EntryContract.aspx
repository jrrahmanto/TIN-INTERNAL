<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryContract.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryContract" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc3" %>
<%@ Register src="../../Lookup/CtlExchangeLookup.ascx" tagname="CtlExchangeLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlProductLookup.ascx" tagname="CtlProductLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlSubCategory.ascx" tagname="CtlSubCategoryLookup" tagprefix="uc5" %>
<%@ Register src="../../Controls/CtlMonthYear.ascx" tagname="CtlMonthYear" tagprefix="uc5" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register assembly="EcCustomControls" namespace="EcCustomControls.EcTextBox" tagprefix="cc2" %>

<%@ Register src="../../Controls/CtlContractlCalenderPickUp.ascx" tagname="CtlContractlCalenderPickUp" tagprefix="uc6" %>
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
    <h1>Manage Contract</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
          <tr>
            <td >
            <div class="shadow_view">
            <div class="box_view">
         <table class="table-row">
        <tr>
            <td style="width:150px">
                Commodity Code</td>
            <td style="width:10px">
                        :</td>
            <td colspan="2">
                <uc3:CtlCommodityLookup ID="uiCtlCommodity" runat="server" />
                <asp:Button ID="uiBtnRefresh" runat="server" onclick="uiBtnRefresh_Click" 
                    Text="Refresh" />
            </td>
        </tr>
        <tr>
            <td >
                Contract Month</td>
            <td>
                :</td>
                                                <td colspan="2">
                                                    <uc5:CtlMonthYear ID="uiDtpMonthYear" runat="server" />
                                                </td>
                                            </tr>
        <tr>
                                                <td >
                                                    Start Date</td>
            <td>
                :</td>
                                                <td colspan="2">
                                                    <uc4:CtlCalendarPickUp ID="uiDtpStartDate" runat="server" />
                                                </td>
                                            </tr>
        <tr>
                                                <td >
                                                    Start Spot Date</td>
            <td>
                :</td>
                                                <td colspan="2">
                                                    <uc6:CtlContractlCalenderPickUp ID="uiDtpStartSpot" runat="server" />
                                                </td>
                                            </tr>
        <tr>
                                                <td >
                                                    End Spot Date</td>
            <td>
                :</td>
                                                <td colspan="2">
                                                    <uc6:CtlContractlCalenderPickUp ID="uiDtpEndSpot" runat="server" />
                                                </td>
                                            </tr>
        <tr>
            <td >
                Contract Size</td>
            <td>
                :</td>
            <td colspan="2">
                <cc2:FilteredTextBox ID="uiTxbContractSize" runat="server" 
                    FilterTextBox="Money" CssClass="number-required" 
                    ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>
        <tr>
            <td >
                                                    Home Currency</td>
            <td>
                                                    :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlHomeCurrency" runat="server" 
                                                        DataSourceID="ObjectDataSource1" DataTextField="CurrencyCode" 
                                                        DataValueField="CurrencyID">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                                        SelectMethod="GetCurrency" TypeName="Currency">
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td >
                Settlement Type</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlSettlementType" CssClass="required" runat="server">
                    <asp:ListItem Value="D">Delivery</asp:ListItem>
                    <asp:ListItem Value="R">Rolling</asp:ListItem>
                    <asp:ListItem Value="C">Cash Settlement</asp:ListItem>
                    <asp:ListItem Value="G">Gulir</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
                                                <td >
                Unit</td>
            <td>
                        :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbUnit" CssClass="required" runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr id="startDateHide" runat="server" visible="false">
            <td >
                                                    Effective Start Date</td>
            <td>
                :</td>
            <td colspan="2">
                <uc4:CtlCalendarPickUp ID="uiDtpEffStartDate" runat="server" />
            </td>
        </tr>
        <tr id="endDateHide" runat="server" visible="false">
            <td >
                Effective End Date</td>
            <td>
                :</td>
            <td colspan="2">
                <uc4:CtlCalendarPickUp ID="uiDtpEffEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Cross Currency Product</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlCrossCurrencyProduct" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>IDR</asp:ListItem>
                    <asp:ListItem>USD</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <%--<tr>
            <td >
                Cross Currency</td>
            <td>
                        :</td>
            <td>
                <asp:DropDownList ID="uiddlCrossCurrency" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="REG">Regular</asp:ListItem>
                    <asp:ListItem Value="REL">Real</asp:ListItem>
                    <asp:ListItem>PEG</asp:ListItem>
                    <asp:ListItem Value="NA">Not Available</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <td>
                Settlement Factor</td>
            <td>
                        :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlSettlementFactor" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="CP">Cross Product</asp:ListItem>
                    <asp:ListItem Value="PC">Product Cross</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td >
                VM/IRCA Calculation Type</td>
            <td>
                        :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlVMIRCACal" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="D">Direct</asp:ListItem>
                    <asp:ListItem Value="I">Indirect</asp:ListItem>
                    <asp:ListItem Value="N">Not Required</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
      <%--  <tr>
            <td >
                PEG</td>
            <td>
                        :</td>
            <td>
                <cc2:FilteredTextBox ID="uiTxbPEG" CssClass="number" runat="server" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>--%>
        <tr>
            <td >
                Day Reference</td>
            <td>
                        :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbDayReference" runat="server" CssClass="number" Width="51px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="uiTxbDayReference_FilteredTextBoxExtender" 
                            runat="server" Enabled="True" 
                            TargetControlID="uiTxbDayReference" ValidChars="0123456789">
                        </cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td >
                Margin Spot</td>
            <td>
                :</td>
            <td colspan="2">
                <cc2:FilteredTextBox ID="uiTxbMarginSpot" runat="server" 
                    CssClass="number-required" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox></td>
        </tr>
        <tr>
            <td >
                Margin Remote</td>
            <td>
                :</td>
            <td colspan="2">
                <cc2:FilteredTextBox ID="uiTxbMarginRemote" runat="server" 
                    CssClass="number-required" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>
        <tr>
            <td >
                Margin Tender</td>
            <td>
                :</td>
            <td colspan="2">
                <cc2:FilteredTextBox ID="uiTxbMarginTender" runat="server" CssClass="number" 
                    FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>
        <tr>
            <td >
                Calendar Spread Remote Margin</td>
            <td>
                :</td>
            <td colspan="2">
                <cc2:FilteredTextBox ID="uiTxbCalSpreadMargin" runat="server" CssClass="number" 
                    FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>
        <%--<tr>
            <td >
                Divisor</td>
            <td>
                        :</td>
            <td>
                <cc2:FilteredTextBox ID="uiTxbDivisor" runat="server" CssClass="number" 
                    FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>--%>
        <tr>
            <td >
                New Contract</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:CheckBox ID="uiChkKIE" runat="server" />
            </td>
        </tr>
        <tr>
            <td >
                Tender Request</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlTenderReqType" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="R">Randomized</asp:ListItem>
                    <asp:ListItem Value="F">FIFO</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:150px">
               Sub Category</td>
            <td style="width:10px">
                        :</td>
            <td colspan="2">
                <uc5:CtlSubCategoryLookup ID="uiCtlSubCategory" runat="server"  />
                <asp:Button ID="Button1" runat="server" onclick="uiBtnRefresh_Click" 
                    Text="Refresh" />
            </td>
        </tr>
        <tr>
                                                <td >
                                                    Contract
                                                    Description</td>
            <td>
                :</td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="uiTXbDescription" runat="server" Height="75px" Width="400px" 
                                                        TextMode="MultiLine"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" 
                    ControlToValidate="uiTXbDescription" ErrorMessage="Max. 500 char" 
                    ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
        <tr id="TRAction" runat="server">
            <td >
                Action</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbAction" runat="server" Enabled="False"></asp:TextBox>
                <asp:Label ID="uiLblAction" runat="server"></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                    &nbsp;</td>
            <td>
                &nbsp;</td>
            <td >K1
                                                                    </td>
            <td >K2</td>
        </tr>
         <tr>
            <td>
                Mode</td>
            <td>
                :</td>
            <td class="style3" >
                <asp:DropDownList ID="uiDdlbModeK1" runat="server">
                <asp:ListItem Value="REG">Float</asp:ListItem>
                <asp:ListItem Value="PEG">PEG</asp:ListItem>
                </asp:DropDownList> 
            </td>
            <td class="style4">
                <asp:DropDownList ID="uiDdlbModeK2" runat="server">
                <asp:ListItem Value="REG">Float</asp:ListItem>
                <asp:ListItem Value="PEG">PEG</asp:ListItem>
                </asp:DropDownList>
             </td> 
        </tr> 
        <tr>
            <td>
                Value K</td>
            <td>
                :</td>
            <td>
            <cc2:FilteredTextBox ID="uiTxbValueK1" runat="server" 
                    CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                <%--<asp:TextBox ID="uiTxbValueK" runat="server"></asp:TextBox>--%>
               <%-- <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    Operator="DataTypeCheck" Type="Double" 
                ControlToValidate="uiTxbValueK" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
            </td>
            <td class="style4">
             <cc2:FilteredTextBox ID="uiTxbValueK2" runat="server" 
                    CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
            </td>
        </tr>
         <tr>
            <td>
                Contract Ref K</td>
            <td>
                :</td>
            <td>
            <uc6:CtlContractCommodityLookup ID="uiCtlContractRefK1" 
                            runat="server" />
             <%--<asp:TextBox ID="uiTxbContractRefK" runat="server"></asp:TextBox>--%>
               <%-- <asp:CompareValidator ID="CompareValidator3" runat="server" 
                    Operator="DataTypeCheck" Type="Double" 
                ControlToValidate="uiTxbContractRefK" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
                
            </td>
            <td class="style4">
             <uc6:CtlContractCommodityLookup ID="uiCtlContractRefK2" 
                            runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Mode D</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlbModeD" runat="server">
                <asp:ListItem Value="REG">Float</asp:ListItem>
                <asp:ListItem Value="PEG">PEG</asp:ListItem>
                </asp:DropDownList> 
            </td> 
        </tr> 
        <tr>
            <td>
                Value D</td>
            <td>
                :</td>
            <td colspan="2">
            <cc2:FilteredTextBox ID="uiTxbValueD" runat="server" 
                    CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox></td>
                 <%-- <asp:TextBox ID="uiTxbValueD" runat="server"></asp:TextBox>--%>
                <%--<asp:CompareValidator ID="CompareValidator4" runat="server" 
                    Operator="GreaterThan" Type="Double" 
                ControlToValidate="uiTxbValueD" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
                
            </td>
        </tr>
         <tr>
            <td>
                Contract Ref D</td>
            <td>
                :</td>
            <td colspan="2">
            <uc6:CtlContractCommodityLookup ID="uiCtlContractRefD" 
                            runat="server" />
                <%--<asp:TextBox ID="uiTxbContractRefD" runat="server"></asp:TextBox>
                 <asp:CompareValidator ID="CompareValidator5" runat="server" 
                    Operator="DataTypeCheck" Type="Double" 
                ControlToValidate="uiTxbContractRefD" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
            </td>
        </tr>
         <tr>
            <td>
                Mode IM</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlbModeIM" runat="server">
                <asp:ListItem Value="LOT">Per Lot</asp:ListItem>
                <asp:ListItem Value="PER">Percentage</asp:ListItem>
                <asp:ListItem Value="MAX">Max</asp:ListItem>
                </asp:DropDownList> 
            </td> 
        </tr> 
         <tr>
            <td>
                Percentage Remote IM</td>
            <td>
                :</td>
            <td colspan="2">
             <cc2:FilteredTextBox ID="uiTxbPercentageRemoteIM" runat="server" 
                    CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                <%--<asp:TextBox ID="uiTxbPercentageRemoteIM" runat="server"></asp:TextBox>--%>
                 <%--<asp:CompareValidator ID="CompareValidator6" runat="server" 
                    Operator="DataTypeCheck" Type="Double" 
                ControlToValidate="uiTxbPercentageRemoteIM" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
            </td>
        </tr>
        <tr>
            <td>
                Percentage Spot IM</td>
            <td>
                :</td>
            <td colspan="2">
            <cc2:FilteredTextBox ID="uiTxbPercentageSpotIM" runat="server" 
                    CssClass="number" FilterTextBox="Money" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                <%--<asp:TextBox ID="uiTxbPercentageSpotIM" runat="server"></asp:TextBox>--%>
                <%--<asp:CompareValidator ID="CompareValidator7" runat="server" 
                    Operator="DataTypeCheck" Type="Double" 
                ControlToValidate="uiTxbPercentageSpotIM" ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
            </td>
        </tr>
        <tr>
            <td>
                Mode Fee</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlbModeFee" runat="server">
                <asp:ListItem Value="LOT">Per Lot</asp:ListItem>
                <asp:ListItem Value="PER">Percentage</asp:ListItem>
                <asp:ListItem Value="MAX">Max</asp:ListItem>
                </asp:DropDownList> 
            </td> 
        </tr>
        <tr>
            <td> Commodity Type</td>
            <td> :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbCommType" MaxLength="20" runat="server" Enabled="false"></asp:TextBox>
            </td> 
        </tr>
        <tr>
            <td> Quality</td>
            <td> :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbQuality" MaxLength="20" runat="server" Enabled="false"></asp:TextBox>
            </td> 
        </tr>
        <tr>
            <td> Regional</td>
            <td> :</td>
            <td colspan="2">
                <asp:DropDownList ID="uiDdlRegional" runat="server" DataSourceID="SqlDataSource1" 
                    DataTextField="RegionName" DataValueField="RegionID" Enabled="false"></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SKDConnectionString %>" SelectCommand="select RegionID, Code + ' - ' + Name as RegionName
from SKD.Region
where ApprovalStatus = 'A'
order by Code"></asp:SqlDataSource>
            </td> 
        </tr>
        <tr id="TRApproval" runat="server">
            <td >
                Approval Description</td>
            <td>
                :</td>
            <td colspan="2">
                <asp:TextBox ID="uiTxbApprovalDesc" CssClass="required" runat="server" Height="75px" 
                    TextMode="MultiLine" Width="400px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" 
                    ControlToValidate="uiTxbApprovalDesc" ErrorMessage="Max. 100 char" 
                    ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                    Text="      Save" onclick="uiBtnSave_Click"   />
                    <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" Text="     Delete" onclick="uiBtnDelete_Click" 
                    />
                    <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                    <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                    Text="     Approve" onclick="uiBtnApprove_Click"  />
                    <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="     Reject" onclick="uiBtnReject_Click" 
                    />
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                    onclick="uiBtnCancel_Click" CausesValidation="False" />
                    </td>
        </tr>
        </table>
        </div>
        </div>
        </td>
        </tr>
    </table>
</asp:Content>

