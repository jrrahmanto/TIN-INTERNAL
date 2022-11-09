<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewAuditTrailData.aspx.cs" Inherits="AuditAndCompliance_ViewAuditTrailData" %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>View Audit Trail Data</h1>
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
                        <td style="width:100px">
                            Table</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                        <asp:DropDownList ID="uiDdlApplicationTable" runat="server" 
                                DataSourceID="odsApplicationTable" DataTextField="ApplicationTable" 
                                DataValueField="ApplicationTable" >
                        </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsApplicationTable" runat="server" 
                                SelectMethod="GetApplicationTableDDL" TypeName="AuditTrail">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Log
                            Date From</td>
                        <td>
                            :         <td>
                            <uc1:CtlCalendarPickUp ID="uiDtpLogFrom" runat="server" />
&nbsp;To
                            <uc1:CtlCalendarPickUp ID="uiDtpLogTo" runat="server" />
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Action</td>
                        <td>
                            :</td>
                        <td>
                        <asp:DropDownList ID="uiDdlAction" runat="server" DataSourceID="odsUserAction" 
                                DataTextField="UserAction" DataValueField="UserAction">
                        </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsUserAction" runat="server" 
                                SelectMethod="GetUserActionDDL" TypeName="AuditTrail">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            User Name</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="uiTxbUserName" runat="server" Width="217px"></asp:TextBox>
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
                <asp:GridView ID="uiDgAuditTrail" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" AllowPaging="True" 
                    onpageindexchanging="uiDgAuditTrail_PageIndexChanging" 
                    DataKeyNames="AuditrailID" EmptyDataText="No Record Found" PageSize="25">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" 
                                    NavigateUrl='<%# Eval("AuditrailID", "~/AuditAndCompliance/DetailAuditTrailData.aspx?id={0}") %>' 
                                    Text="edit" ImageUrl="~/Images/edit.gif"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ApplicationTable" HeaderText="Table Name" 
                            SortExpression="ApplicationTable" />
                        <asp:BoundField HeaderText="Date" DataField="LogTime" SortExpression="LogTime">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UserAction" HeaderText="Action" 
                            SortExpression="UserAction" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" 
                            SortExpression="UserName" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ObjectDataSource ID="odsAuditTrail" runat="server"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>

