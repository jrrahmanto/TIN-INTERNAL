<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ImportBankTransaction.aspx.cs" Inherits="FinanceAndAccounting_ImportBankTransaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
<%@ Register Src="../Lookup/CtlClearingMemberLookup.ascx" TagName="CtlClearingMemberLookup"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>View Bank Transaction</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
     <tr>
                    <td colspan="3">
                            <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                        Entry Date</td>
                    <td>
                        :</td>
                    <td>
                        <uc1:ctlcalendarpickup ID="CtlCalendarBusinessDate" runat="server" />
                    </td>
                </tr>                
                <tr>
                    <td>
                            Bank</td>
                    <td>
                        :</td>
                    <td>
                            <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="odsBank" DataTextField="Name" DataValueField="BankID">
                            </asp:DropDownList>

                    </td>
                </tr>
                <tr>
                    <td>
                            File Name</td>
                    <td>
                        :</td>
                    <td>
                            <asp:FileUpload ID="uiUploadFile" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnUpload" runat="server" CssClass="button_create" 
                            onclick="uiBtnUpload_Click" Text="     Upload" />
                &nbsp;<asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel" Text="     Cancel" 
                            onclick="uiBtnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                            &nbsp;</td>
                    <%--<td>
                        Please remember: this Upload feature is auto approval with DELETE existing Settlement Price!</td>--%>
                </tr>
                <tr>
                    <td class="style1">
                            </td>
                    <td class="style1">
                        </td>
                    <td class="style1">
                        </td>
                </tr>
                <tr>
                    <td colspan="3">
                            <asp:TextBox ID="uiTxbLog" runat="server" Height="298px" TextMode="MultiLine" 
                                Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                        <img alt="" src="../../Images/ajax-loader2.gif"
                                            style="width: 220px; height: 19px" />
                            </ProgressTemplate>
                            </asp:UpdateProgress> &nbsp;<asp:ObjectDataSource ID="odsBank" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByBankOnly" TypeName="BankDataTableAdapters.BankTableAdapter"></asp:ObjectDataSource>
                    </td>
                </tr>
                    </table>
             </div>
             </div>
        </td>
    </tr>
    </table>
        
</asp:Content>
