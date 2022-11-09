<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryBankPDM.aspx.cs" Inherits="WebUI_New_EntryBankPDM" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h1>Manage Bank PDM</h1>
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
                            Bank Code</td>
                    <td style="width:10px">
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtBankCode" CssClass="required" MaxLength="20" runat="server" Width="174px"></asp:TextBox>
                                                            </td>
                </tr>
                <tr>
                    <td>
                            BI Code</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtBICode" CssClass="required" MaxLength="50" runat="server" Width="217px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Name</td>
                    <td>
         :</td>
                    <td>
                        <asp:TextBox ID="uiTxtName" CssClass="required" MaxLength="50" runat="server"  Width="330px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Branch                        Branch</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtBranch" MaxLength="50" runat="server"  Width="219px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Division</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtDivision" runat="server" MaxLength="50" Width="219px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Address</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAddress" runat="server" Height="80px" MaxLength="100"
                            TextMode="MultiLine" Width="400px"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="uiTxtAddress" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>
                            Province</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtProvince" MaxLength="50" Width="219" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            City</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtCity" MaxLength="50" Width="219" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            PostCode</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtPostCode" MaxLength="20" Width="150" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Country</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtCountry" Width="219" MaxLength="50" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Phone Number</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtPhoneNo" MaxLength="20" Width="160" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Fax</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtFax" MaxLength="20" Width="160" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Contact 1</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtContact1" MaxLength="20" Width="160" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Contact 2</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtContact2" MaxLength="20" Width="160" runat="server" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                            Description</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtDescription" Height="80px" MaxLength="100"
                            TextMode="MultiLine" Width="400px" runat="server"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="uiTxtDescription" ErrorMessage="Max. 500 Character" 
                                ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td>
                            Stamp</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtStamp" MaxLength="20" Width="160" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trAction" runat="server">
                    <td>
                            <asp:Label ID="uiLblAction" runat="server" Text="Action"></asp:Label>
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtAction" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trApprovalDesc" runat="server">
                    <td>
                            <asp:Label ID="uiLblApprovalDescription" runat="server" Text="Approval Description"></asp:Label>
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="uiTxtApporvalDesc" CssClass="required" runat="server" Height="100px" 
                            TextMode="MultiLine" MaxLength="100" Width="400px"></asp:TextBox>
                                                            &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator3" runat="server" 
                                ControlToValidate="uiTxtApporvalDesc" ErrorMessage="Max. 100 Character" 
                                ValidationExpression="^[\s\S]{0,100}$"></asp:RegularExpressionValidator></td>
                </tr>
                
                <tr>
                    <td>
                            &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" 
                            Text="      Save" onclick="uiBtnSave_Click"  />
                        <asp:Button ID="uiBtnDelete" runat="server" CssClass="button_delete" 
                                Text="      Delete" onclick="uiBtnDelete_Click" 
                            CausesValidation="False" />
                              <cc1:ConfirmButtonExtender ID="uiBtnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="uiBtnDelete"></cc1:ConfirmButtonExtender>
                        <asp:Button ID="uiBtnApprove" runat="server" CssClass="button_approve" 
                                Text="     Approve" onclick="uiBtnApprove_Click" />
                        <asp:Button ID="uiBtnReject" runat="server" CssClass="button_reject" 
                                Text="    Reject" onclick="uiBtnReject_Click" />
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

