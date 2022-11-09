<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryIRCA.aspx.cs" Inherits="WebUI_New_EntryIRCA" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>


<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script language="javascript" type="text/javascript">
        function SelectAll(CheckBoxControl,ctlName) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgIRCA') > -1)) {
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
(document.forms[0].elements[i].name.indexOf('uiDgIRCA') > -1)) {
                        var test = document.forms[0].elements[i].name;
                        if (test.indexOf(ctlName) != -1) {
                            document.forms[0].elements[i].checked = false;
                        }
                    }
                }
            }
        }
    </script>

<h1>Manage IRCA</h1>
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
                   
                    <tr id="trEffectiveStartDate" runat="server">
                        <td >
                            Effective Start Date</td>
                        <td>
                            :</td>
                        <td>
                            <uc1:CtlCalendarPickUp ID="CtlCalendarEffectiveStartDate" runat="server" />
                        </td>
                    </tr>
                    <tr id="trButtonAdd" runat="server">
                        <td >
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                         
                            <asp:Button ID="uiBtnAdd" CssClass="button_create" runat="server" 
                                Text="     Add"  onclick="uiBtnAdd_Click" />
                         
                        </td>
                    </tr>
                    </table>
                    <table class="table-datagrid">
                    <tr >
                        <td  colspan="3">
                <Asp:GridView ID="uiDgIRCA" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" EmptyDataText="No Records" 
                    PageSize="40" 
                                DataKeyNames="CommodityID,EffectiveStartDate,ApprovalStatus"  >
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="CheckList">
                            <ItemTemplate>
                                <asp:CheckBox ID="uiChkList" Checked="false" runat="server" />
                            </ItemTemplate>
                             <HeaderTemplate>
                               <input id="uiChkAllList" onclick="SelectAll(this,'uiChkList')" type="checkbox" />
                             </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="EffectiveStartDate" 
                            HeaderText="EffectiveStartDate" SortExpression="EffectiveStartDate" 
                            ReadOnly="True" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:TemplateField HeaderText="Commodity">
                            <ItemTemplate>
                                <asp:Label ID="uiLblCommodity" Visible="false" runat="server" Text='<%# Bind("CommodityID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CommodityID") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CommodityID" SortExpression="CommodityID">
                            <EditItemTemplate>
                                <asp:Label ID="uiLblCommodityId" runat="server" Text='<%# Eval("CommodityID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="uiLblCommodityId" runat="server" Text='<%# Bind("commodityName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="IRCAValue" SortExpression="IRCAValue" 
                            ConvertEmptyStringToNull="False">
                              <EditItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtIRCAValue" FilterTextBox="Custom" Text=' <%# Bind("IRCAValue","{0:#,##0.######}") %>' CssClass="number-datagrid-required" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                              </EditItemTemplate>
                              <ItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtIRCAValue" FilterTextBox="Custom" Text=' <%# Bind("IRCAValue","{0:#,##0.######}") %>' CssClass="number-datagrid-required" runat="server" ValidChar="0123456789.,-"></cc2:FilteredTextBox>
                               </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EffectiveEndDate" HeaderText="EffectiveEndDate" 
                            SortExpression="EffectiveEndDate" />
                        <asp:BoundField DataField="ActionDesc" HeaderText="Action Description" 
                            SortExpression="ActionDesc" />
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
                        <asp:TemplateField HeaderText="ApprovalDesc" SortExpression="ApprovalDesc">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" CssClass="required" runat="server" Text='<%# Bind("ApprovalDesc") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="uiTxtApprovalDesc" CssClass="required" runat="server" Text='<%# Bind("ApprovalDesc") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="IRCAID" SortExpression="IRCAID" 
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblIRCAID" runat="server" Text='<%# Bind("IRCAID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("IRCAID") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                        
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceTrxIRCA" runat="server" 
                                SelectMethod="GetIRCAByTransaction" TypeName="IRCA">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="effectiveStart" QueryStringField="startDate" 
                                        Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                <Asp:GridView ID="uiDgCommodity" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" EmptyDataText="No Records" 
                    PageSize="40" AllowPaging="True" 
                                onpageindexchanging="uiDgCommodity_PageIndexChanging"  
                                onsorting="uiDgCommodity_Sorting">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="CommodityID" SortExpression="CommodityID">
                            <EditItemTemplate>
                                <asp:Label ID="uiLblCommdityId" runat="server" 
                                    Text='<%# Eval("CommodityID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="uiLblCommdityName" runat="server" 
                                Text='<%# Bind("commodityName") %>'></asp:Label>
                                <asp:Label ID="uiLblCommodityId" Visible="false" runat="server" 
                                Text='<%# Bind("commodityId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="IRCAValue" SortExpression="IRCAValue" 
                            ConvertEmptyStringToNull="False">
                              <EditItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtIRCAValueCommodity" Text=' <%# Bind("IRCAValue", "{0:#,##0.######}") %>' FilterTextBox="Custom" ValidChar="0123456789.,-" CssClass="number-datagrid-required" runat="server"></cc2:FilteredTextBox>
                              </EditItemTemplate>
                              <ItemTemplate>
                                  <cc2:FilteredTextBox ID="uiTxtIRCAValueCommodity" FilterTextBox="Custom" ValidChar="0123456789.,-" CssClass="number-datagrid-required" runat="server"></cc2:FilteredTextBox>             
                              </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" ForeColor="White" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                            <asp:ObjectDataSource ID="ObjectDataSourceIRCA" runat="server" 
                                SelectMethod="GetIRCAByEffectiveStartDate" TypeName="IRCA">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="effectiveStart" QueryStringField="startDate" 
                                        Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ObjectDataSourceCommodity" runat="server" 
                                SelectMethod="GetCommodityByEffectiveStartDate" TypeName="IRCA">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="CtlCalendarEffectiveStartDate" 
                                        Name="effectiveStart" PropertyName="Text" Type="DateTime" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr id="trAllButton" runat="server">
                        <td align="center" colspan="3">
                            <asp:Button ID="uiBtnSave" runat="server" CssClass="button_save" 
                                Text="     Save" onclick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnApprove" CssClass="button_approve" 
                    runat="server" Text="     Approve" onclick="uiBtnApprove_Click" 
                     />
                            <asp:Button ID="uiBtnReject" CssClass="button_reject" 
                    runat="server" Text="      Reject" onclick="uiBtnReject_Click" 
                    />
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
