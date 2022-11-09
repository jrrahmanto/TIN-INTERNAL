<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryStressTestScenario.aspx.cs" Inherits="WebUI_RiskManagement_EntryStressTestScenario"  %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register src="../Lookup/CtlContractCommodityStressTestLookup.ascx" tagname="CtlContractCommodityStressTestLookup" tagprefix="uc3" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Manage Stress Test Scenario</h1>
       <table cellpadding="1" cellspacing="1" style="width:100%;">
       <tr>
                        <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                            Scenario Name</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                                                <asp:TextBox ID="uiTxtScenarioName" CssClass="required" runat="server" Width="158px" Height="18px" 
                                                    MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Scenario Date</td>
                        <td>
                            :</td>
                        <td>
                         <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                        </td>
                    </tr>
                    </table>
                    <table class="table-datagrid">
                    <tr>
                        <td colspan="3">
                <asp:Button ID="uiBtnCreate" runat="server" Text="  Add Contract" 
                    onclick="uiBtnCreate_Click" CssClass="button_addcontract" 
                    />
                <Asp:GridView ID="uiDgScenarioContract" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" onrowcancelingedit="uiDgScenarioContract_RowCancelingEdit" 
                                onrowdeleting="uiDgScenarioContract_RowDeleting" 
                                onrowediting="uiDgScenarioContract_RowEditing" 
                                onrowupdating="uiDgScenarioContract_RowUpdating" 
                                onrowdatabound="uiDgScenarioContract_RowDataBound">
                    <RowStyle CssClass="tblRowStyle"     />
                    <Columns>
                       <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:TemplateField HeaderText="Contract" SortExpression="ContractID">
                        <EditItemTemplate>
                              <uc3:CtlContractCommodityStressTestLookup  ID="CtlContractCommodityLookup" runat="server"/>
                              <asp:HiddenField ID="uiHiddenLow" runat="server" Value="" />
                              <asp:HiddenField ID="uiHiddenHigh" runat="server" Value="" />
                            </EditItemTemplate>
                            <ItemTemplate>
                              <asp:Label ID="uiLblContractName" runat="server" Text='<%# ScenarioContract.GetContractDataByContractID(decimal.Parse(DataBinder.Eval(Container.DataItem, "ContractID").ToString())) %>'></asp:Label>
                              <asp:Label ID="uiLblContractID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContractID") %>'></asp:Label>
                              <asp:HiddenField ID="uiHiddenLow" runat="server" Value="" />
                              <asp:HiddenField ID="uiHiddenHigh" runat="server" Value="" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ScenarioID" SortExpression="ScenarioID" 
                            Visible="False" />
                         <asp:TemplateField HeaderText="Scenario Type" SortExpression="ScenarioType">
                        <EditItemTemplate>
                                 <asp:DropDownList ID="uiDdlScenarioType" runat="server" AppendDataBoundItems="True"
                                   DataTextField="ScenarioContract" 
                                DataValueField="ContractID" OnSelectedIndexChanged="uiDdlScenarioType_SelectedIndexChanged" 
                                SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ScenarioType") %>' AutoPostBack="True">
                                <asp:ListItem Text="" Value="P">Percent</asp:ListItem>
                                <asp:ListItem Text="" Value="V">Value</asp:ListItem>
                            </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LblScenarioType" runat="server" Text='<%# Bind("ScenarioTypeDesc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base Price" SortExpression="BasePrice">
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxtBestPrice" runat="server" ValidChar="0123456789.,-" CssClass="number" FilterTextBox="Money" Text='<%# Bind("BasePrice","{0:#,##0.###}") %>'></cc2:FilteredTextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div style="text-align:right">
                                <asp:Label ID="LblBasePrice" CssClass="number" runat="server" Text='<%# Bind("BasePrice","{0:#,##0.###}") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Low" SortExpression="Low">
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxtLow" FilterTextBox="Money" ValidChar="0123456789.,-" Text='<%# Bind("Low","{0:#,##0.###}") %>' CssClass="number" runat="server"></cc2:FilteredTextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div style="text-align:right" >
                                <asp:Label ID="LblLow" CssClass="number" runat="server" Text='<%# Bind("Low","{0:#,##0.###}") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="High" SortExpression="High">
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxtHigh" CssClass="number" ValidChar="0123456789.,-" FilterTextBox="Money" runat="server" Text='<%# Bind("High","{0:#,##0.###}") %>'></cc2:FilteredTextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div style="text-align:right">
                                <asp:Label ID="LblHigh" CssClass="number" runat="server" Text='<%# Bind("High", "{0:#,##0.###}") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                        </td>
                    </tr>
                                        <tr>
                                            <td colspan="3">
                <asp:ObjectDataSource ID="ObjectDataSourceScenarioContract" runat="server" 
                                                    SelectMethod="SelectScenarioContractData" TypeName="ScenarioContract">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="scenarioId" QueryStringField="eID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                                            </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="uiBtnRun" runat="server" Text="     Run" 
                                onclick="uiBtnRun_Click" CssClass="button_runandsave" />
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="     Save" 
                                onclick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnCancel" runat="server" Text="     Cancel" onclick="uiBtnCancel_Click" 
                                CssClass="button_cancel" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        
    </table>
</asp:Content>

