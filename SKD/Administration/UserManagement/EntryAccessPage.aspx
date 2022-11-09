<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EntryAccessPage.aspx.cs" Inherits="WebUI_New_EntryAccessPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <h1>Manage Access Page</h1>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
    
     <tr>
        <td>
        <asp:BulletedList ID="uiBlError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
        </td>
    </tr>
    
        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                   
                    <tr>
                        <td style="width:150px">
                            Page Name
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbPageName" CssClass="required" runat="server" Width="500px"></asp:TextBox>
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
                            <asp:TextBox ID="uiTxbDescription" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Application Name
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:DropDownList ID="uiDdlApplicationName" CssClass="required" runat="server">
                                <asp:ListItem>SKD</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            URL
                        </td>
                        <td >
                            :
                        </td>
                        <td >
                            <asp:TextBox ID="uiTxbUrlName" CssClass="required" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;
                        </td>
                        <td >
                            &nbsp;
                        </td>
                        <td >
                            <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save"
                                OnClick="uiBtnSave_Click" />
                            <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" OnClick="uiBtnDelete_Click"
                                Text="     Delete" />
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
