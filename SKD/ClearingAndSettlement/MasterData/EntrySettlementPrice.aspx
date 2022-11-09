<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntrySettlementPrice.aspx.cs" Inherits="WebUI_New_EntrySettlementPrice" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script language="javascript" type="text/javascript">
    function SelectAll(CheckBoxControl, ctlName) {
        if (CheckBoxControl.checked == true) {
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgSettlementPrice') > -1)) {
                    var test = document.forms[0].elements[i].name;
                    if (test.indexOf(ctlName) != -1) {
                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
        }
        else {
            var i;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgSettlementPrice') > -1)) {
                    var test = document.forms[0].elements[i].name;
                    if (test.indexOf(ctlName) != -1) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }
    }
    </script>
    <h1>Manage Settlement Price</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
    <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
        <tr>
            <td>
              <div class="shadow">
            <div class="box">
               <table class="table-row">
                    
                    <tr id="trBusinessDate" runat="server">
                        <td >
                            Business Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarBusinessDate" runat="server" />
                        </td>
                    </tr>
                    <tr id="trSettlementPriceType" runat="server">
                        <td >
                            Settlement Price Type</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlSettlePriceType" runat="server">
                                <asp:ListItem Value="N">Normal</asp:ListItem>
                                <asp:ListItem Value="U">Urgent</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trButtonAdd" runat="server">
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="uiBtnAdd" runat="server" CssClass="button_create"
                                Text="     Add"  Visible="False" OnClick="uiBtnAdd_Click"/>
                            <asp:Button ID="uiBtnImport" runat="server" CssClass="button_import" 
                    Text="    Import" onclick="uiBtnImport_Click1" />
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="3">
                <asp:GridView ID="uiDgSettlementPrice" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" DataKeyNames="SettleID,ContractID" EmptyDataText="No Records" 
                                AllowPaging="True" onrowdatabound="uiDgSettlementPrice_RowDataBound" 
                                onpageindexchanging="uiDgSettlementPrice_PageIndexChanging" PageSize="50">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="uiChkList" runat="server" />
                            </ItemTemplate>
                            <HeaderTemplate>
                              <input id="uiChkAll" onclick="SelectAll(this,'uiChkList')" type="checkbox" />
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Settle ID">
                            <ItemTemplate>
                                <asp:Label ID="uiLblSettleId" Visible="false" runat="server" Text='<%# Bind("SettleID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SettleID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BusinessDate" HeaderText="Date" 
                            DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Contract">
                            <ItemTemplate>
                                <asp:Label ID="uiLblContractID" Visible="false" runat="server" Text='<%# Bind("ContractID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ContractID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="CommodityCode" HeaderText="Code" />
                        <asp:BoundField DataField="ContractYearMonth" HeaderText="Contract Month" />
                        <asp:TemplateField HeaderText="Settlement Price" ConvertEmptyStringToNull="False">
                            <ItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxbSettlePriceEdit" runat="server" Text='<%# Bind("SettlementPrice", "{0:#,##0.##########}") %>' FilterTextBox="Money" CssClass="number-datagrid-required" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxbSettlePriceEdit" runat="server" Text='<%# Bind("SettlementPrice", "{0:#,##0.##########}") %>' FilterTextBox="Money" CssClass="number-datagrid-required" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                             </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SettlementPriceTypeDesc" 
                            HeaderText="Settlement Price Type" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Label ID="uiLblActionDesc" Text='<%# Eval("ActionFlag") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="CreatedBy">
                            <ItemTemplate>
                                <asp:Label ID="uiLblCreatedBy" Visible="false" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="CreatedDate">
                            <ItemTemplate>
                                <asp:Label ID="uiLblCreatedDate" Visible="false" runat="server" Text='<%# Bind("CreatedDate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CreatedDate") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approval Description">
                            <ItemTemplate>
                                <asp:TextBox ID="uiTxtApprovalDesc" runat="server" Width="250" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTrxSettlementPrice" runat="server" 
                                SelectMethod="GetSettlementByTransaction" TypeName="SettlementPrice" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="bussDate" QueryStringField="businessDate" 
                                        Type="DateTime" />
                                    <asp:QueryStringParameter Name="approval" QueryStringField="approval" 
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr >
                        <td  colspan="3">
                <Asp:GridView ID="uiDgContract" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" EmptyDataText="No Records" AllowPaging="True" onpageindexchanging="uiDgContract_PageIndexChanging" 
                                onsorting="uiDgContract_Sorting" PageSize="50">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="EffectiveStartDate" 
                            HeaderText="EffectiveStartDate" SortExpression="EffectiveStartDate" 
                            ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}" Visible="False" 
                            ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="bussinessDate" HeaderText="Bussiness Date" 
                            SortExpression="bussinessDate" Visible="False" 
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Contract" SortExpression="CommName">
                            <ItemTemplate>
                                <asp:Label ID="uiLblCommName" runat="server" 
                                    Text='<%# Bind("CommName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="uiLblContractID" runat="server" 
                                    Text='<%# Eval("ContractID") %>'></asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        
                            
                     
                          <asp:TemplateField HeaderText="Settlement Price" 
                            ConvertEmptyStringToNull="False">
                              <EditItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtSettlementPriceValueContract" Text=' <%# Bind("SettlementPrice", "{0:#,##0.##########}") %>' FilterTextBox="Money" CssClass="number-datagrid-required" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                              </EditItemTemplate>
                              <ItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtSettlementPriceValueContract" Text=' <%# Bind("SettlementPrice", "{0:#,##0.##########}") %>' FilterTextBox="Money" CssClass="number-datagrid-required" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                               </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ActionDesc" HeaderText="Status" 
                            SortExpression="ActionDesc" Visible="False" 
                            ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <headerstyle CssClass="headerStyle_Datagrid" BorderStyle="None" 
                        ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceSettlementPrice" runat="server" 
                                SelectMethod="GetSettlementByBusinessDate" TypeName="SettlementPrice" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="bussDate" QueryStringField="businessDate" 
                                        Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ObjectDataSourceContract" runat="server" 
                                SelectMethod="GetContractByEffectiveStartDate" TypeName="SettlementPrice">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarBusinessDate" Name="bussDate" 
                                        PropertyName="Text" Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            </td>
                    </tr>
                    <tr id="trAllButton" runat="server">
                        <td align="center" colspan="3">
                            <asp:Button ID="uiBtnSave" runat="server" CssClass="button_save" 
                                Text="     Save" onclick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                                onclick="uiBtnApprove_Click" />
                            <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="     Reject" onclick="uiBtnReject_Click" />
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                            Text="      Cancel" onclick="uiBtnCancel_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        
    </table>
    </asp:Content>


