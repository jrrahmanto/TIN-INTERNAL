<%@ Page  Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryRiskProfile.aspx.cs" Inherits="RiskManagement_EntryRiskProfile" %>

<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Manage Risk Profile</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td>
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                </asp:BulletedList>
                <asp:BulletedList ID="uiBLErrorDetail" runat="server" ForeColor="Red" Visible="False">
                </asp:BulletedList>
            </td>
        </tr>
        <tr>
            <td>
                <div class="shadow_view">
                    <div class="box_view">
                        <table class="table-row">
                            <tr>
                                <td style="width:150px">
                                    Clearing Member ID
                                </td>
                                <td style="width:10px">
                                    :
                                </td>
                                <td>
                                    <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Start Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <uc2:CtlCalendarPickUp ID="CtlCalendarPickUpStartDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    End Date
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <uc2:CtlCalendarPickUp ID="CtlCalendarPickUpEndDate" runat="server" />
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
                <asp:GridView ID="uiDgRiskProfile" runat="server" AutoGenerateColumns="False" Width="100%"
                    MouseHoverRowHighlightEnabled="True" RowHighlightColor="" DataKeyNames="RiskProfileID,RiskTypeID"
                    OnRowCancelingEdit="uiDgRiskProfile_RowCancelingEdit" OnRowDeleting="uiDgRiskProfile_RowDeleting"
                    OnRowEditing="uiDgRiskProfile_RowEditing" PageSize="15" OnRowUpdating="uiDgRiskProfile_RowUpdating">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="RiskTypeID" HeaderText="RiskTypeID" ReadOnly="True" SortExpression="RiskTypeID"
                            Visible="False" />
                        <asp:TemplateField HeaderText="Risk Type">
                            <EditItemTemplate>
                                <asp:DropDownList ID="uiDdlRiskType" runat="server" AppendDataBoundItems="True" DataSourceID="ObjectDataSourceRiskType"
                                    DataTextField="RiskType" DataValueField="RiskTypeID" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "RiskTypeID") %>'>
                                    <asp:ListItem Text="" Value="0"> </asp:ListItem>
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="ObjectDataSourceRiskType" runat="server" SelectMethod="GetRiskType"
                                    TypeName="RiskProfile"></asp:ObjectDataSource>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="uiLblRiskType" runat="server" Text='<%# RiskProfile.GetRiskTypeByRiskTypeID(decimal.Parse(DataBinder.Eval(Container.DataItem, "RiskTypeID").ToString())) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Impact" SortExpression="Impact">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtImpact" runat="server" Text='<%# Bind("Impact") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Impact") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Likelihood" SortExpression="Likelihood">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtLikelihood" runat="server" Text='<%# Bind("Likelihood") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Likelihood") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceRiskProfile" runat="server" SelectMethod="GetRiskProfileDetailByRiskProfileID"
                    TypeName="RiskProfile">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="riskProfileID" QueryStringField="eID" Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="uiBtnAddNewRow" runat="server" OnClick="uiBtnAddNewRow_Click">Add new</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                    OnClick="uiBtnSave_Click" />
                <asp:Button ID="uiBtnDelete" CssClass="button_delete" runat="server" Text="      Delete"
                    OnClick="uiBtnDelete_Click" />
                <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                    OnClick="uiBtnCancel_Click" CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
