<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewManageInterestRateDifferent.aspx.cs" Inherits="ClearingAndSettlement_MasterData_ViewManageInterestRateDifferent" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td class="form-header-menu">
                <asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" 
                    Text="    Create" onclick="uiBtnCreate_Click"  />
            </td>
        </tr>
        <tr>
            <td>
            &nbsp;</td>
        </tr>
        <tr>
            <td>
               <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Commodity Code</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxtKodeAk0" runat="server"></asp:TextBox>
                        &nbsp;<asp:Button ID="uiBtnSearch0" runat="server" Text="..." />
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Approval Status</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="DropDownList2" runat="server" Height="17px" Width="126px">
                                <asp:ListItem Value="Approve">Approve</asp:ListItem>
                                <asp:ListItem>Reject</asp:ListItem>
                            </asp:DropDownList>
                        &nbsp;</td>
                    </tr>
                    <tr class="right_search_criteria">
                        <td class="form-content-menu">
                        </td>
                        <td class="separator">
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="uiDgExchangeMember" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:ButtonField Text="Edit" CommandName="edit" />
                        <asp:BoundField DataField="kode" HeaderText="Commodity Code" 
                            SortExpression="url" />
                        <asp:BoundField HeaderText="Status" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

