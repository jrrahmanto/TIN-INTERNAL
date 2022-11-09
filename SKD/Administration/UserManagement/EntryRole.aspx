<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryRole.aspx.cs" Inherits="WebUI_New_EntryRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function SelectAll(CheckBoxControl,ctlName) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
(document.forms[0].elements[i].name.indexOf('uiDgEntryGroup') > -1)) {
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
(document.forms[0].elements[i].name.indexOf('uiDgEntryGroup') > -1)) {
                        var test = document.forms[0].elements[i].name;
                        if (test.indexOf(ctlName) != -1) {
                            document.forms[0].elements[i].checked = false;
                        }
                    }
                }
            }
        }
    </script>

<h1>Manage Role</h1>

    <table cellpadding="1" cellspacing="1" style="width: 100%;">
    
    <tr>
       <td  colspan="3">
            <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red"></asp:BulletedList>
       </td>
    </tr>
    
        <tr>
            <td>
            <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td style="width:100px">
                            Group
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbGroup" CssClass="required" runat="server" Width="165px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Description
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbDesc" runat="server"  Height="70px" TextMode="MultiLine" 
                                Width="400px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Access Page
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                        
                            <asp:GridView ID="uiDgEntryGroup" runat="server" AutoGenerateColumns="False" Width="600px"
                                MouseHoverRowHighlightEnabled="True" RowHighlightColor="">
                                <RowStyle CssClass="tblRowStyle" />
                                <Columns>
                                    <asp:BoundField HeaderText="Page" DataField="PageName" SortExpression="Page" >
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Viewer">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="uiChkViewer" runat="server" Checked='<%# Eval("Viewer") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <input id="uiChkAllViewer" onclick="SelectAll(this,'uiChkViewer')" type="checkbox" />
                                            <asp:Label ID="uiLblChkViewer" runat="server" Text="    Viewer"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maker">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="uiChkMaker" runat="server" Checked='<%# Eval("Maker") %>' />
                                        </ItemTemplate>
                                         <HeaderTemplate>
                                            <input id="uiChkAllMaker" onclick="SelectAll(this,'uiChkMaker')" type="checkbox" />
                                             <asp:Label ID="uiLblChkMaker" runat="server" Text="     Maker"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Checker">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="uiChkChecker"  runat="server" Checked='<%# Eval("Checker") %>'/>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <input id="uiChkAllChecker" onclick="SelectAll(this,'uiChkChecker')" type="checkbox" />
                                            <asp:Label ID="uiLblChkChecker" runat="server" Text="    Checker"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="tblHeaderStyle" />
                                <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                    <td >
                      &nbsp;
                    </td>
                    <td >
                      &nbsp;
                    </td>
                    <td>
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                OnClick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnDelete" runat="server" Text="     Delete" OnClick="uiBtnDelete_Click"
                                CssClass="button_delete" />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="      Cancel"
                                OnClick="uiBtnCancel_Click" />
                        </td>
                    </tr>
                </table>
             </div>
             </div>
            </td>
        </tr>
    </table>
</asp:Content>
