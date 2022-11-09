<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryGroup.aspx.cs" Inherits="UserManagement_EntryGroup" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Group</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    
    <tr>
      <td  colspan="3">
      <asp:Label ID="uiLblWarning" runat="server" Font-Bold="True" ForeColor="#FF3300" Visible="False"></asp:Label>
       </td>
   </tr>
        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                    </tr>
       <table class="table-datagrid">
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td  >
                <asp:GridView ID="uiDgExchangeMember" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataSourceID="ObjectDataSource1">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField HeaderText="Page Name" DataField="pagename" 
                            SortExpression="pagename" />
                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                SelectMethod="GetPageData" TypeName="Roles">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="uiFldMapPath" Name="path" PropertyName="Value" 
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
       </table>
                    <tr>
                        <td  >
                            <asp:HiddenField ID="uiFldMapPath" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
                        </td>
                    </tr>
                </table>
            </div>
            </div>
            </td>
       </tr>
 </table>
</asp:Content>

