<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlClearingMemberLookup.ascx.cs" Inherits="Lookup_CtlClearingMemberLookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%--<link href="../Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
<link href="../Styles/Style.css" rel="stylesheet" type="text/css" />--%>
<script type="text/javascript" language="javascript">

    function CheckOtherIsCheckedByGVID(spanChk) {

        var IsChecked = spanChk.checked;
        if (IsChecked) {

            spanChk.parentElement.parentElement.style.backgroundColor = '#228b22';
            spanChk.parentElement.parentElement.style.color = 'white';
        }

        var CurrentRdbID = spanChk.id;
        var Chk = spanChk;
        Parent = document.getElementById('<%= uiDgClearingMember.ClientID %>');
        var items = Parent.getElementsByTagName('input');

        for (i = 0; i < items.length; i++) {
            if (items[i].id != CurrentRdbID && items[i].type == "radio") {

                if (items[i].checked) {
                    items[i].checked = false;
                    items[i].parentElement.parentElement.style.backgroundColor = 'white';
                    items[i].parentElement.parentElement.style.color = 'black';
                }
            }
        }
    }

    function HideModalPopupClearingMember<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>');
        modal.hide();
    }

    function SetValueClearingMember(value) {
       window.document.forms[0].<%= uiTxtLookupClearingMember.ClientID %>.value = value;
    }
    
    function enable<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>(elname, val){
        if(window.document.forms[0].elname){
            window.document.forms[0].elname.disabled=val;
        }
        
    }

    function setValue<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>(elname, val) {
        if(window.document.forms[0].elname){
            window.document.forms[0].elname.value=val;            
        }
    }
 
</script>

<style type="text/css">
    .style1
    {
        width: 422px;
    }
</style>
    <input id="uiTxtLookupIDClearingMember" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupClearingMember" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupClearingMember" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderClearingMember" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupClearingMember"
    PopupControlID="Panel1ClearingMember" 
    OnOkScript="SetValueClearingMember();";
    CancelControlID="uiBtnCancelClearingMember"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1ClearingMember" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Clearing Member Lookup</td>
            <td align="right">
                
                <asp:Button ID="uiBtnCancelClearingMember" runat="server" CssClass="close-icon" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" style="width:100%;height:100%">
                <tr>
                    <td>
                        <table cellpadding="1" cellspacing="1" style="width:100%;">
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Clearing Member Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCMCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Clearing Member Name</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCMName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="right_search_criteria">
                                <td class="form-content-menu">
                                </td>
                                <td class="separator">
                                </td>
                                <td>
                                    <asp:Button ID="uiBtnSearch" runat="server" CssClass="button_search" 
                                        onclick="uiBtnSearch_Click" Text="     Search" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>                    
                    <td valign="top">
                        <asp:GridView ID="uiDgClearingMember" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Code" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgClearingMember_PageIndexChanging" 
                            
                            onsorting="uiDgClearingMember_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgClearingMember_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField HeaderText="CMID" SortExpression="CMID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblCMID" runat="server" Text='<%# Bind("CMID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CMID") %>'></asp:TextBox>
                                        
                                    </EditItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="Code" HeaderText="Code" ReadOnly="True" 
                                    SortExpression="Code" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceClearingMember" runat="server" 
                            SelectMethod="GetClearingMemberByCodeAndName" TypeName="ClearingMember" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCMCode" Name="code" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtCMName" Name="name" PropertyName="Text" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>