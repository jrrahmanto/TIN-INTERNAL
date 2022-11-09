<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewMBD.aspx.cs" Inherits="AuditAndCompliance_ViewMBD" %>

<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View MBD</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     <tr >
                        <td>
                            <asp:BulletedList ID="uiBLError" ForeColor="Red" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
    
        <tr>
            <td> 
            <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    <tr >
                        <td style="width:150px">
                            Clearing Member</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlClearingMemberLookup ID="uiCtlCM" runat="server" />
                        </td>
                    </tr>
                    <tr >
                        <td>
                            Action</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlApprovalStatus" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="A">Approved</asp:ListItem>
                                <asp:ListItem Value="P">Proposed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" 
                                Text="     Search" onclick="uiBtnSearch_Click" />
&nbsp;<asp:Button ID="uiBtnImport" runat="server" CssClass="button_import" onclick="uiBtnImport_Click" 
                                Text="Import" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        </table>
        
        <table class="table-datagrid">
        <tr>
            <td>
                <asp:GridView ID="uiDgMBD" runat="server" AllowSorting="True" OnPageIndexChanging="uiDgMBD_PageIndexChanging"
                    AutoGenerateColumns="False" Width="100%" AllowPaging="True" 
                    MouseHoverRowHighlightEnabled="True" OnSorting="uiDgMBD_Sorting" PageSize="15"
                    RowHighlightColor="" DataKeyNames="MBDID">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                         <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("MBDID", "~/AuditAndCompliance/EntryMBD.aspx?eType=edit&eID={0}") %>'
                                    Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Code" HeaderText="CM Code" SortExpression="Code" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MBDFund" HeaderText="Fund" 
                            SortExpression="MBDFund" DataFormatString="{0:#,##0.##}" >
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MBDValue" HeaderText="Value" 
                            SortExpression="MBDValue" DataFormatString="{0:#,##0.##}" >
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsMBD" runat="server" 
                    SelectMethod="FillBySearchCriteria" TypeName="MBD">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="uiCtlCM" Name="CMID" 
                            PropertyName="LookupTextBoxID" Type="Decimal" />
                        <asp:ControlParameter ControlID="uiDdlApprovalStatus" Name="approvalStatus" 
                            PropertyName="SelectedValue" Type="String" />
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

