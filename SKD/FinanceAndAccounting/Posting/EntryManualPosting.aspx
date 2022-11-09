<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryManualPosting.aspx.cs" Inherits="WebUI_New_EntryManualPosting" %>

<%@ Register Assembly="EcCustomControls" Namespace="EcCustomControls.EcTextBox" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc1" %>

<%@ Register src="../../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc2" %>

<%@ Register src="../../Lookup/CtlPostingCodeLookup.ascx" tagname="CtlPostingCodeLookup" tagprefix="uc3" %>

<%@ Register src="../../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Manual Posting</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
                    <td  colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red" Visible="False">
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
                            Posting No.</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtPostingNo" runat="server" Enabled="False"></asp:TextBox>
                        <asp:ObjectDataSource ID="ObjectDataSourceJournalHeader" runat="server" 
                            SelectMethod="GetJournalHeaderByJournalHeaderID" TypeName="Journal">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="journalHeaderId" QueryStringField="eID" 
                                    Type="Decimal" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                            Posting Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc2:CtlCalendarPickUp ID="CtlCalendarPickUpPostingDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                            Currency</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlCurrency" CssClass="required" runat="server" 
                            DataSourceID="ObjectDataSourceCurrency" DataTextField="CurrencyCode" 
                            DataValueField="CurrencyID"
                            AppendDataBoundItems="True">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ObjectDataSourceCurrency" runat="server" 
                            SelectMethod="GetCurrency" TypeName="Currency"></asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                            Ledger Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlLedgerType" CssClass="required" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="H">Clearing House</asp:ListItem>
                            <asp:ListItem Value="M">Clearing Member</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Journal Type</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="uiDdlJournalType" CssClass="required" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="A">Automatic</asp:ListItem>
                            <asp:ListItem Value="M">Manual</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                            Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtDescription" runat="server" Height="100px" 
                            TextMode="MultiLine" Width="290px"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDes" runat="server">
                    <td>
                            Approval Description</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:TextBox ID="uiTxtApprovalDesription" CssClass="required" runat="server" Height="100px" 
                            TextMode="MultiLine" Width="400px"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxtApprovalDesription" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                <Asp:GridView ID="uiDgJournal" runat="server" 
                    AutoGenerateColumns="False" Width="620" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="" 
                            DataKeyNames="JournalLineID" onrowdatabound="uiDgJournal_RowDataBound" 
                            onrowcancelingedit="uiDgJournal_RowCancelingEdit" 
                            onrowupdating="uiDgJournal_RowUpdating" 
                            onrowdeleting="uiDgJournal_RowDeleting" 
                            onrowediting="uiDgJournal_RowEditing1">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" >
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="Account" SortExpression="AccountID">
                            <EditItemTemplate>
                                <uc3:CtlPostingCodeLookup ID="CtlPostingCodeLookup" runat="server" 
                                />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="uiLblPostingCode" runat="server" Text='<%# Posting.GetPostingDescriptionByAccountId(decimal.Parse(DataBinder.Eval(Container.DataItem, "AccountID").ToString())) %>'></asp:Label>
                                <asp:Label ID="uiLblPostingCodeID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AccountID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Debit" SortExpression="DrAmount" ItemStyle-HorizontalAlign="Right">
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxtDrAmount" Text='<%# Bind("DrAmount") %>' ValidChar="0123456789.,-" CssClass="number" FilterTextBox="Money" runat="server"></cc2:FilteredTextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                            <div style="text-align:right">
                                <asp:Label  ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DrAmount", "{0:#,##0.##}") %>' CssClass="number" ></asp:Label>
                            </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit" SortExpression="CrAmount">
                            <EditItemTemplate>
                                <cc2:FilteredTextBox ID="uiTxtCrAmount" FilterTextBox="Money" CssClass="number" runat="server" Text='<%# Bind("CrAmount") %>'></cc2:FilteredTextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div style="text-align:right">
                                <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CrAmount", "{0:#,##0.###}") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" 
                            SortExpression="LineDescription">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtLineDescription" runat="server" Text='<%# Bind("LineDescription") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("LineDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clearing Member" SortExpression="Attribute1Code">
                            <EditItemTemplate>
                                <uc1:CtlClearingMemberLookup ID="CtlClearingMemberLookup" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="uiLblClearingMember" runat="server" Text='<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Attribute1Code").ToString())? "" : ClearingMember.GetClearingMemberCodeByClearingMemberID(decimal.Parse(DataBinder.Eval(Container.DataItem, "Attribute1Code").ToString())) %>'></asp:Label>
                                 
                                <asp:Label ID="uiLblClearingMemberID" Visible="false" runat="server" Text='<%# Bind("Attribute1Code") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute2Code" SortExpression="Attribute2Code" 
                            Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt2Code" runat="server" Text='<%# Bind("Attribute2Code") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("Attribute2Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute3Code" SortExpression="Attribute3Code" 
                            Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt3Code" runat="server" Text='<%# Bind("Attribute3Code") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("Attribute3Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute4Code" SortExpression="Attribute4Code" 
                            Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt4Code" runat="server" Text='<%# Bind("Attribute4Code") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("Attribute4Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute5Code" SortExpression="Attribute5Code" 
                            Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt5Code" runat="server" Text='<%# Bind("Attribute5Code") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("Attribute5Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute1Description" 
                            SortExpression="Attribute1Description" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt1Desc" runat="server" 
                                    Text='<%# Bind("Attribute1Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label10" runat="server" 
                                    Text='<%# Bind("Attribute1Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute2Description" 
                            SortExpression="Attribute2Description" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt2Desc" runat="server" 
                                    Text='<%# Bind("Attribute2Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" 
                                    Text='<%# Bind("Attribute2Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute3Description" 
                            SortExpression="Attribute3Description" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt3Desc" runat="server" 
                                    Text='<%# Bind("Attribute3Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label12" runat="server" 
                                    Text='<%# Bind("Attribute3Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute4Description" 
                            SortExpression="Attribute4Description" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt4Desc" runat="server" 
                                    Text='<%# Bind("Attribute4Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label13" runat="server" 
                                    Text='<%# Bind("Attribute4Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attribute5Description" 
                            SortExpression="Attribute5Description" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="uiTxtAtt5Desc" runat="server" 
                                    Text='<%# Bind("Attribute5Description") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label14" runat="server" 
                                    Text='<%# Bind("Attribute5Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceJournal" runat="server" 
                            SelectMethod="GetJournalLineByJournalHeaderID" TypeName="Journal">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="journalHeaderId" QueryStringField="eID" 
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
                        <asp:LinkButton ID="uiBtnAdd" runat="server" onclick="uiBtnAdd_Click">Add new</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click"  />
                    
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" Text="     Approve" 
                            onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" Text="    Reject" 
                                                    onclick="uiBtnReject_Click" />
                    
                        <asp:Button ID="uiBtnAddAndSave" runat="server" Text="     Save & Add" 
                            CssClass="button_addsave" onclick="uiBtnAddAndSave_Click" />
                        
                    
                        <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  Text="      Cancel" 
                            onclick="uiBtnCancel_Click" />
                    
                </tr>
                </table>
             </div>
             </div>
        </td>
    </tr>
</table>
</asp:Content>

