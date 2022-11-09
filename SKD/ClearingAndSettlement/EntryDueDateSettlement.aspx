<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryDueDateSettlement.aspx.cs" Inherits="ClearingAndSettlement_EntryDueDateSettlement" %>


<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Trade Register</h1>
    <asp:Panel ID="pnlException" runat="server">
      
      <table cellpadding="1" cellspacing="1" style="width:100%;">
      
      <tr>
                    <td colspan="3">
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
                    <td>
                        Bussiness Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtBusinessDate" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Traded Price
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtTradedPrice" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Quantity
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtQuantity" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Exchange Ref
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtExchangeRef" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Seller Code
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtSellerCode" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Buyer Code
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtBuyerCode" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Contract Month
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtContractMonth" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Outstanding
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtOutstanding" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        DueType
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTxtDueType" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="uiTrCarryForward" runat="server">
                    <td>
                        Seller Carry Forward
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTrTxtCarryForward" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="uiTrCarryForwardNotes" runat="server">
                    <td>
                        Seller Carry Forward Note
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="uiTrTxtCarryForwardNote" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="uiLblDueType" runat="server" ></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        
                        <asp:RadioButton ID="uiRbnDefault" runat="server" AutoPostBack="True" GroupName="DueType" Text="Default" OnCheckedChanged="uiRbnDefault_CheckedChanged" />
                        <br />
                        <asp:RadioButton ID="uiRbnCarryForward" runat="server" AutoPostBack="True" GroupName="DueType" Text="Carry Forward" OnCheckedChanged="uiRbnCarryForward_CheckedChanged" />
                        <br />
                         <uc1:CtlCalendarPickUp ID="uiDtpCarryForward" runat="server" />
                        
                        <br />
                        <asp:TextBox ID="uiTxtCarryForward" runat="server" Height="100px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                        
                    </td>
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
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" 
                            OnClick="uiBtnCancel_Click" Text="      Cancel" />
                    </td>
                </tr>
            </table>
            </div>
                </div>
            </td>
        </tr>
        </table>
    </asp:Panel>
</asp:Content>

