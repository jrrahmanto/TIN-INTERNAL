<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CTLMultiCLearingMemberLookup.ascx.cs" Inherits="Lookup_CTLMultiCLearingMemberLookup" %>
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
    
    function setSelection(ev)
    {
        var uiIDCM = document.getElementById('<%= uiTxtLookupIDClearingMember.ClientID %>');
        var uiCM = document.getElementById('<%= uiTxtLookupClearingMember.ClientID %>');

        var uiTempNames = document.getElementById('<%= uiTempNames.ClientID %>');
        var uiTempIds = document.getElementById('<%= uiTempIds.ClientID %>');

        var gvDrv = document.getElementById("<%= uiDgClearingMember.ClientID %>");

        var cmIds = uiTempIds.value;
        var cmNames = uiTempNames.value;

        // value and names
        var parent;
        var target;
        
        if (document.all == null)
        {
            // Non IE
            target = ev.currentTarget;
            parent = ev.currentTarget.parentNode;
        }
        else
        {
            // IE
            target = ev.srcElement;
            parent = ev.srcElement.parentElement;
        }

        var sibling = parent.nextSibling;

        var ctlValue = sibling.getElementsByTagName('SPAN')[0].innerHTML;
        var ctlId = sibling.getElementsByTagName('INPUT')[0].value;

        // remove item
        var found = false;
        if (cmIds == ctlId)
        {
            cmIds = '';
            cmNames = '';
        }
        else if ((cmIds + ' ').substr(0, ctlId.length + 1) == ctlId + ',')
        {
            cmIds = cmIds.replace(ctlId + ',', '');
            //cmNames = cmNames.replace(ctlValue + ',', '');
            found = true;
        }
        else if ((" " + cmIds).substr(cmIds.length - ctlId.length, ctlId.length + 1) == ',' + ctlId)
        {
            cmIds = cmIds.replace(',' + ctlId, '');
            //cmNames = cmNames.replace(',' + ctlValue, '');
            found = true;
        }
        else 
        {
            cmIds = cmIds.replace(',' + ctlId + ',', ',');
            //cmNames = cmNames.replace(',' + ctlValue + ',', ',');
            found = true;
        }
        
        if (found)
        {
            if ((cmNames + ' ').substr(0, ctlValue.length + 1) == ctlValue + ',')
            {
                cmNames = cmNames.replace(ctlValue + ',', '');
                found = true;
            }
            else if ((" " + cmNames).substr(cmNames.length - ctlValue.length, ctlValue.length + 1) == ',' + ctlValue)
            {
                cmNames = cmNames.replace(',' + ctlValue, '');
                found = true;
            }
            else 
            {
                cmNames = cmNames.replace(',' + ctlValue + ',', ',');
                found = true;
            }        
        }

        // add if selected
        if (target.checked == true)
        {
            if (cmIds == '') 
            {
                cmIds = ctlId;
                cmNames = ctlValue;
            }
            else 
            {
                cmIds = cmIds + ',' + ctlId;
                cmNames = cmNames + ',' + ctlValue;
            }
        }
        
        uiTempIds.value = cmIds.split(',').sort().join(',');
        uiTempNames.value = cmNames.split(',').sort().join(',');

    }
    
    function onLookupClosing()
    {
        var uiIDCM = document.getElementById('<%= uiTxtLookupIDClearingMember.ClientID %>');
        var uiCM = document.getElementById('<%= uiTxtLookupClearingMember.ClientID %>');
        var uiTempNames = document.getElementById('<%= uiTempNames.ClientID %>');
        var uiTempIds = document.getElementById('<%= uiTempIds.ClientID %>');
        var uiGrid = document.getElementById("<%= uiDgClearingMember.ClientID %>");

        // reset temporary ids & names
        uiTempNames.value = uiCM.value;
        uiTempIds.value = uiIDCM.value;
        
        // reset checkboxes
        for (ii = 1; ii < uiGrid.rows.length; ii++)
        {
            if (uiGrid.rows[ii].getElementsByTagName('INPUT').length >= 2)
            {
                var uiChk = uiGrid.rows[ii].getElementsByTagName('INPUT')[0];
                var uiId = uiGrid.rows[ii].getElementsByTagName('INPUT')[1];
                var strId = uiId.value;
                
                if (strId == uiTempIds.value || 
                    (" " + uiTempIds.value).substr(uiTempIds.value.length - strId.length, strId.length + 1) == ',' + strId || 
                    (uiTempIds.value + ' ').substr(0, strId.length + 1) == strId + ',' || 
                    uiTempIds.value.indexOf(strId) >= 0)
                {
                    uiChk.checked = true;
                }
                else
                {
                    uiChk.checked = false;
                }

            }
        }

        return HideModalPopupClearingMember<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>();        
    }
    
    function ManipulateGrid(textClientID,valueCLientID)
    {
        var uiIDCM = document.getElementById('<%= uiTxtLookupIDClearingMember.ClientID %>');
        var uiCM = document.getElementById('<%= uiTxtLookupClearingMember.ClientID %>');
        var uiTempNames = document.getElementById('<%= uiTempNames.ClientID %>');
        var uiTempIds = document.getElementById('<%= uiTempIds.ClientID %>');
        
        uiCM.value = uiTempNames.value;
        uiIDCM.value = uiTempIds.value;

        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderClearingMember.ClientID %>');
        modal.hide();
        
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
    <input id="uiTempIds" runat="server" type="hidden" />      
    <input id="uiTempNames" runat="server" type="hidden" />      

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
    <asp:Panel ID="Panel1ClearingMember" runat="server" Width="500px" CssClass="tbl_lookup">
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
                                        <input id="uiChbCMID" runat="server" type="checkbox" value="Select" onclick="setSelection(event);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField HeaderText="CMID" SortExpression="CMID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblCMID" runat="server" Text='<%# Bind("CMID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"  Text='<%# Bind("CMID") %>'></asp:TextBox>
                                        
                                    </EditItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code" SortExpression="Code">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                        <input type="hidden" id="test2" runat="server" value='<%# Bind("CMID") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                        <input type="hidden" id="test1" runat="server" value='<%# Bind("CMID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                <tr>
                    <td valign="top">
                        <asp:Button ID="uiBtnSelect" runat="server" Text="Select" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>