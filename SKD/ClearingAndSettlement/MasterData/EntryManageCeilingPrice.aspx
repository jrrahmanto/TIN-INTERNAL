<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EntryManageCeilingPrice.aspx.cs" Inherits="ClearingAndSettlement_MasterData_EntryManageCeilingPrice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Ceiling Price</h1>
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
                                <td>
                                    Ceiling Price
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtCeilingPrice" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Floor Price
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="uiTxtFloorPrice" runat="server"></asp:TextBox>
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
                                    <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" OnClick="uiBtnSave_Click" />
                                    <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel" OnClick="uiBtnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
