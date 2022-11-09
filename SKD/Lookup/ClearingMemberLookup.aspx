<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultPage.master" AutoEventWireup="true" CodeFile="ClearingMemberLookup.aspx.cs" Inherits="Lookup_ClearingMemberLookup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="1" cellspacing="1" style="width:100%">
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1" style="width:100%;">
                <tr class="form-content-menu">
                    <td class="form-content-menu">
                        Clearing Member Code</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxtCMCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="form-content-menu">
                    <td class="form-content-menu">
                        Clearing Member Name</td>
                    <td class="separator">
                        :</td>
                    <td class="right_search_criteria">
                        <asp:TextBox ID="uiTxtCMName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr class="right_search_criteria">
                    <td class="form-content-menu">
                    </td>
                    <td class="separator">
                    </td>
                    <td>
                        <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" 
                            Text="     Search" onclick="uiBtnSearch_Click" />
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
            <asp:GridView ID="uiDgClearingMember" runat="server" 
                AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Code" 
                MouseHoverRowHighlightEnabled="True" PageSize="20" RowHighlightColor="" 
                Width="100%" onrowdatabound="uiDgClearingMember_RowDataBound" 
                AllowPaging="True" onpageindexchanging="uiDgClearingMember_PageIndexChanging" 
                onsorting="uiDgClearingMember_Sorting">
                <RowStyle CssClass="tblRowStyle" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CMID" HeaderText="CMID" SortExpression="CMID" />
                    <asp:BoundField DataField="Code" HeaderText="Code" ReadOnly="True" 
                        SortExpression="Code" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                </Columns>
                <headerstyle CssClass="tblHeaderStyle" />
                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSourceClearingMember" runat="server" 
                SelectMethod="GetClearingMemberByCodeAndName" TypeName="ClearingMember">
                <SelectParameters>
                    <asp:ControlParameter ControlID="uiTxtCMCode" Name="code" PropertyName="Text" 
                        Type="String" />
                    <asp:ControlParameter ControlID="uiTxtCMName" Name="name" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
</table>
</asp:Content>

