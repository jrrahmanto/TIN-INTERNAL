<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryTukarFisikBerjangka.aspx.cs" Inherits="ClearingAndSettlement_Tender_EntryTukarFisikBerjangka" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc4" %>
<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc3" %>
<%@ Register src="../../Lookup/CtlExchangeLookup.ascx" tagname="CtlExchangeLookup" tagprefix="uc2" %>
<%@ Register src="../../Lookup/CtlProductLookup.ascx" tagname="CtlProductLookup" tagprefix="uc1" %>
<%@ Register src="../../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc5" %>
<%@ Register src="../../Lookup/CtlInvestorLookup.ascx" tagname="CtlInvestorLookup" tagprefix="uc6" %>
<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>
        Entry Tukar Fisik Berjangka</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
                <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red">
                </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width:100px">
                                    Tender Date</td>
                                <td style="width:10px">
                                    :</td>
                                <td>
                                    <uc4:CtlCalendarPickUp ID="uiDtpTenderDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contract</td>
                                <td>
                :</td>
                                <td>
                                    <uc5:CtlContractCommodityLookup ID="uiCTLContract" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seller Clearing Member</td>
                                <td>
                                    :</td>
                                <td>
                                    <uc7:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                                                        Seller Account</td>
                                <td>
                :</td>
                                <td>
                                    <uc6:CtlInvestorLookup ID="uiCTLSellerInvestor" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seller Price</td>
                                <td>
                        :</td>
                                <td>
                                    <asp:TextBox ID="uiTxbSellerPrice" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seller Quantity</td>
                                <td>
                                                    :</td>
                                <td>
                                    <asp:TextBox ID="uiTxbSellerQuantity" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seller Trade Position</td>
                                <td>
                :</td>
                                                                    <td>
                                                                        <asp:DropDownList ID="uiDdlSellerTradePosition" runat="server">
                                                                            <asp:ListItem Value="BF">Brought-Forward</asp:ListItem>
                                                                            <asp:ListItem Value="NT">New Trade</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Buyer Clearing Member</td>
                                                                    <td>
                                                                        :</td>
                                                                    <td>
                                                                        <uc7:CtlClearingMemberLookup ID="CtlClearingMemberLookup2" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Buyer Account</td>
                                <td>
                        :</td>
                                <td>
                                    <uc6:CtlInvestorLookup ID="uiCtlBuyerInvestor" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Buyer Price</td>
                                <td>
                :</td>
                                <td>
                                    <asp:TextBox ID="uiTxbBuyerPrice" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Buyer Quantity</td>
                                <td>
                :</td>
                                <td>
                                    <asp:TextBox ID="uiTxbBuyerQuantity" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Buyer Trade Position</td>
                                <td>
                :</td>
                                <td>
                                    <asp:DropDownList ID="uiDdlBuyerPosition" runat="server">
                                        <asp:ListItem Value="BF">Brought-Forward</asp:ListItem>
                                        <asp:ListItem Value="NT">New Trade</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivery Location</td>
                                <td>
                        :</td>
                                <td>
                                    <asp:TextBox ID="uiTxbDeliveryLocation" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                &nbsp;</td>
                                <td>
                &nbsp;</td>
                                <td>
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                    Text="      Save" onclick="Button3_Click"  />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

