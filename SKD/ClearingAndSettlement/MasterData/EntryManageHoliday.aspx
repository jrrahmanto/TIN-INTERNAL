<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryManageHoliday.aspx.cs" Inherits="WebUI_ClearingAndSettlement_EntryManageHoliday" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../Lookup/CtlCommodityLookup.ascx" TagName="CtlCommodityLookup"
    TagPrefix="uc1" %>
<%@ Register Src="../../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Holiday</h1>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td colspan="3">
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
                                <td style="width:100px">
                                    Date
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc2:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Description
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtDescription" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Holiday Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlHolidayType" CssClass="required" AutoPostBack="true" runat="server"
                                        OnSelectedIndexChanged="uiDdlHolidayType_SelectedIndexChanged">
                                        <asp:ListItem Value="G">Global</asp:ListItem>
                                        <asp:ListItem Value="E">Exchange</asp:ListItem>
                                        <asp:ListItem Value="P">Commodity</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trExchange" runat="server">
                                <td>
                                    Exchange
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="uiDdlExchange" Enabled="true" runat="server" DataSourceID="ObjectDataSourceExchange"
                                        DataTextField="ExchangeCode" DataValueField="ExchangeId">
                                    </asp:DropDownList>
                                    <asp:ObjectDataSource ID="ObjectDataSourceExchange" runat="server" SelectMethod="getExchangeCode"
                                        TypeName="Holiday"></asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr id="trProduct" runat="server">
                                <td valign="top">
                                    Commodity</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc1:CtlCommodityLookup ID="CtlCommodityLookup1" runat="server" />
                                </td>
                            </tr>
                             <tr id="trAction" runat="server">
                                <td>
                                    <asp:Label ID="uiLblAction" runat="server" Text="Action"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxbAction" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr id="trApprovalDesc" runat="server">
                                <td>
                                    <asp:Label ID="uiLblApprovalDesc" runat="server" Text="Approval Description"></asp:Label>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxbApporvalDesc" CssClass="required" MaxLength="100"  runat="server" Height="78px"
                                        TextMode="MultiLine" Width="400px"></asp:TextBox>
                                &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                runat="server" ControlToValidate="uiTxbApporvalDesc" 
                                ErrorMessage="Max. 100 Character" ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                            </tr>
                           
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                        OnClick="uiBtnSave_Click" />
                                    <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" OnClick="uiBtnDelete_Click"
                                        Text="      Delete" />
                                    <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server"
                                        ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve"
                                        OnClick="uiBtnApprove_Click" />
                                    <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject"
                                        OnClick="uiBtnReject_Click" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                        OnClick="uiBtnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
