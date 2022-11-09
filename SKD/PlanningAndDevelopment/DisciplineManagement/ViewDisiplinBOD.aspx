<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewDisiplinBOD.aspx.cs" Inherits="WebUI_New_ViewDisiplinBOD" %>


<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            background-color: #82B5E8;
            font-family: Arial;
            font-size: 12px;
            font-weight: normal;
            color: #000;
            padding-left: 10px;
            width: 115px;
        }
        .style3
        {
            background-color: #82B5E8;
            font-family: Arial;
            font-size: 12px;
            font-weight: normal;
            color: #000;
            padding-left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
              <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="style3" colspan="3">
                            <asp:Label ID="uiLblWarning" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="style3">
                            CM Code</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookupBODDicipline" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="style3">
                            Name</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:TextBox ID="uiTxbName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="style3">
                            Approval Status</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="uiDdlApprovalStatus" runat="server">
                                <asp:ListItem Value="A">Approved</asp:ListItem>
                                <asp:ListItem Value="P">Proposed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="right_search_criteria">
                        <td class="style3">
                        </td>
                        <td class="separator">
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" />
                        &nbsp;<asp:Button ID="uiBtnCreate" runat="server" CssClass="button_create" 
                                onclick="uiBtnCreate_Click" Text="     Create" />
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
                <Asp:GridView ID="uiDgDisiplinBOD" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" onrowcommand="uiDgDisiplinBOD_RowCommand" 
                    DataKeyNames="BODDiciplineID">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="BODName" HeaderText="Name" 
                            SortExpression="BODName" />
                        <asp:BoundField DataField="BODPosition" HeaderText="Position" 
                            SortExpression="BODPosition" />
                        <asp:BoundField DataField="CMName" HeaderText="CM Name" 
                            SortExpression="CMName" />
                        <asp:HyperLinkField DataNavigateUrlFields="BODDiciplineID" 
                            DataNavigateUrlFormatString="~/PlanningAndDevelopment/DisciplineManagement/EntryDisiplinBOD.aspx?id={0}" 
                            Text="edit" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

