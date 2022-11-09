<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CtlSubCategory.ascx.cs" Inherits="Lookup_CtlSubCategory" %>
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
        Parent = document.getElementById('<%= uiDgSubCategory.ClientID %>');
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

    function HideModalPopupSubCategory<%= uiBtnLookup_ModalPopupExtenderSubCategory.ClientID %>() {
        var modal = $find('<%= uiBtnLookup_ModalPopupExtenderSubCategory.ClientID %>');
        modal.hide();
    }

    function SetValueSubCategory(value) {
       window.document.forms[0].<%= uiTxtLookupSubCategory.ClientID %>.value = value;
    }
    
    function enable<%= uiBtnLookup_ModalPopupExtenderSubCategory.ClientID %>(elname, val){
        if(window.document.forms[0].elname){
            window.document.forms[0].elname.disabled=val;
        }
        
    }

    function setValue<%= uiBtnLookup_ModalPopupExtenderSubCategory.ClientID %>(elname, val) {
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
    <input id="uiTxtLookupIDSubCategory" runat="server" type="hidden" />      
     <asp:TextBox ID="uiTxtLookupSubCategory" runat="server" BackColor="#FCF8C5" EnableViewState="False"></asp:TextBox>
     &nbsp;<asp:Button ID="uiBtnLookupSubCategory" runat="server" Text="Lookup" />
<cc1:ModalPopupExtender ID="uiBtnLookup_ModalPopupExtenderSubCategory" runat="server" 
    DynamicServicePath="" Enabled="True" TargetControlID="uiBtnLookupSubCategory"
    PopupControlID="Panel1SubCategory" 
    OnOkScript="SetValueSubCategory();";
    CancelControlID="uiBtnCancelSubCategory"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1SubCategory" runat="server" Width="500px" CssClass="tbl_lookup" DefaultButton="uiBtnSearch">
<table style="width: 100%;" class="header_lookup">
        <tr>
            <td class="style1">
                Sub Category Lookup</td>
            <td align="right">
                
                <asp:Button ID="uiBtnCancelSubCategory" runat="server" CssClass="close-icon" />
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
                                    Sub Category Code</td>
                                <td class="separator">
                                    :</td>
                                <td class="right_search_criteria">
                                    <asp:TextBox ID="uiTxtCMCode" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="form-content-menu">
                                <td class="form-content-menu">
                                    Sub Category Desc</td>
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
                        <asp:GridView ID="uiDgSubCategory" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="SubCategoryCode" 
                            MouseHoverRowHighlightEnabled="True"  EmptyDataText="No Record"
                            onpageindexchanging="uiDgSubCategory_PageIndexChanging" 
                            
                            onsorting="uiDgSubCategory_Sorting" RowHighlightColor="" Width="100%" 
                            onrowdatabound="uiDgSubCategory_RowDataBound" PageSize="15">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <input id="uiBtnSelect" runat="server" type="button" value="Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 
                                <asp:TemplateField HeaderText="SubCategoryID" SortExpression="SubCategoryID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="uiLblCMID" runat="server" Text='<%# Bind("SubCategoryID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SubCategoryID") %>'></asp:TextBox>
                                        
                                    </EditItemTemplate>
                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="SubCategoryCode" HeaderText="Code" ReadOnly="True" 
                                    SortExpression="SubCategoryCode" />
                                <asp:BoundField DataField="Description" HeaderText="Desc" SortExpression="Name" />
                            </Columns>
                            <headerstyle CssClass="headerStyle_Datagrid" ForeColor="White" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceSubCategory" runat="server" 
                            SelectMethod="GetSubCategoryByCodeAndDesc" TypeName="SubCategory" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="uiTxtCMCode" Name="code" PropertyName="Text" 
                                    Type="String" />
                                <asp:ControlParameter ControlID="uiTxtCMName" Name="desc" PropertyName="Text" 
                                    Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>