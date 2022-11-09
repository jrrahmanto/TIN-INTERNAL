<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryEventRecipient.aspx.cs" Inherits="Administration_EventManagement_EntryEventRecipient" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage Event Recipient</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">

    <tr>
       <td colspan="3">
       <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False"></asp:BulletedList>
       </td>
    </tr>

        <tr>
            <td>
             <div class="shadow_view">
             <div class="box_view">
                <table class="table-row">
                    
                    <tr>
                        <td style="width:200px">
                            EventType Code</td>
                        <td >
                        :</td>
                        <td >
                            <asp:DropDownList ID="uiDdlEventTypeCode" runat="server" CssClass="required"
                                DataSourceID="ObjectDataSourceEventTypeCode" DataTextField="EventTypeCode" 
                                DataValueField="EventTypeID">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventTypeCode" runat="server"
                                SelectMethod="getEventTypeCode" TypeName="EventRecipient">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            ReceipientList Name</td>
                        <td >
                        :</td>
                        <td >
                            <asp:CheckBoxList ID="uiChkRecipientList" CssClass="required" runat="server">
                            </asp:CheckBoxList>
                            <asp:ObjectDataSource ID="ObjectDataSourceEventRecipientName" runat="server" 
                                SelectMethod="getEventRecipientListName" TypeName="EventRecipient">
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                                Text="      Save" onclick="uiBtnSave_Click"  />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                                Text="      Cancel" onclick="uiBtnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            </div>
            </td>
        </tr>
        </table>
            </asp:Content>

