<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewRiskTypeColor.aspx.cs" Inherits="WebUI_New_ViewRiskTypeColor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>View RiskType Color</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr >
                        <td>
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
</table>
<table class="table-datagrid">
        <tr>
            <td>
                <Asp:GridView ID="uiDgRiskColor" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataKeyNames="Impact,Likelihood,ApprovalStatus" 
                    onpageindexchanging="uiDgRiskColor_PageIndexChanging" 
                    onrowcommand="uiDgRiskColor_RowCommand" 
                    onrowediting="uiDgRiskColor_RowEditing" 
                    onrowupdating="uiDgRiskColor_RowUpdating" 
                    onsorting="uiDgRiskColor_Sorting" 
                    onrowcancelingedit="uiDgRiskColor_RowCancelingEdit" PageSize="50" 
                    AllowPaging="True" AllowSorting="True" 
                    onrowdatabound="uiDgRiskColor_RowDataBound" 
                    onrowdeleting="uiDgRiskColor_RowDeleting">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" DeleteText="Reject" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:ButtonField CommandName="reject" Text="Delete" Visible="true" />
                        <asp:TemplateField HeaderText="Impact" SortExpression="Impact">
                            <ItemTemplate>
                                <asp:Label ID="uiLblImpact" runat="server" Text='<%# Bind("Impact") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtImpact" runat="server" Text='<%# Eval("Impact") %>'></asp:TextBox>
                                 
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Likelihood" SortExpression="Likelihood">
                            <ItemTemplate>
                                <asp:Label ID="uiLblLikelihood" runat="server" Text='<%# Bind("Likelihood") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtLikelihood" runat="server" Text='<%# Eval("Likelihood") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Risk Color" SortExpression="RiskColor">
                            <ItemTemplate>                       
                                <asp:Label ID="uiLblRiskColor" Visible="false" runat="server" Text='<%# Bind("RiskColor") %>'></asp:Label>   
                                <asp:TextBox BackColor='<%# System.Drawing.ColorTranslator.FromHtml(DataBinder.Eval(Container.DataItem, "RiskColor").ToString())  %>' 
                                    ID="TextBox1" ReadOnly="true" runat="server"></asp:TextBox>
                             </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtRiskColor" runat="server" Text='<%# Bind("RiskColor") %>'></asp:TextBox>
                                <asp:Button ID="uiBtnRiskColor" runat="server" Text="..." />
                                <cc1:ColorPickerExtender ID="ColorPickerExtender1" PopupButtonID="uiBtnRiskColor" TargetControlID="uiTxtRiskColor" runat="server">
                                </cc1:ColorPickerExtender>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ApprovalStatus" SortExpression="ApprovalStatus" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Bind("ApprovalStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="uiLblApprovalStatus" runat="server" Text='<%# Eval("ApprovalStatus") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ApprovalDesc" SortExpression="ApprovalDesc" 
                            Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ApprovalDesc") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtApprovalDesc" runat="server" Text='<%# Bind("ApprovalDesc") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceRiskColor" runat="server" 
                    SelectMethod="GetRiskColorByApprovalStatus" TypeName="RiskProfile">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="A" Name="approvalStatus" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="uiBtnAdd" runat="server" onclick="uiBtnAdd_Click">Add new</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>

