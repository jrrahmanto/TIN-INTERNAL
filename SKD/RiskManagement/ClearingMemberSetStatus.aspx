<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClearingMemberSetStatus.aspx.cs" Inherits="RiskManagement_ClearingMemberSetStatus" Title="Clearing Member Set Status" %>

<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>
<%@ Register src="../Lookup/CtlExchangeLookup.ascx" tagname="CtlExchangeLookup" tagprefix="uc2" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register src="../Lookup/CtlExchangeMemberLookup.ascx" tagname="CtlExchangeMemberLookup" tagprefix="uc3" %>
<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>

<%@ Register src="../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc5" %>
<%@ Register src="../Controls/CtlMonthYear.ascx" tagname="CtlMonthYear" tagprefix="uc6" %>


<%@ Register src="../Lookup/CtlProductLookup.ascx" tagname="CtlProductLookup" tagprefix="uc7" %>


<%@ Register src="../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc8" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Clearing Member Set Status</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" 
                    Visible="False">
                </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:BulletedList ID="uiBLSuccess" runat="server" ForeColor="Blue" 
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
                        <td style="width:150px">
                            Clearing Member</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Status</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlStatus" runat="server" Width="129px">
                                <asp:ListItem Value="L">Limit</asp:ListItem>
                                <asp:ListItem Value="S">Suspend</asp:ListItem>
                                <asp:ListItem Value="R">Release</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reason</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxbReason" runat="server" Width="400px" Height="100px" 
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revTxbReason" runat="server" ControlToValidate="uiTxbReason"
                                ErrorMessage="Max. 40 chars" ValidationExpression="^[\s\S]{0,40}$"></asp:RegularExpressionValidator>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSubmit" CssClass="button_request" runat="server" 
                                Text="     Submit" onclick="uiBtnSubmit_Click" />
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

