<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TIN Derivatives Clearing System</title>
    <link href="Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="Images/favicon.ico" type="image/x-icon" />
    <style type="text/css">
        .style6
        {
            width: 100%;
            float: left;
        }
    </style>
</head>
<body class="login-div">
    <form id="form1" runat="server">
    <div class="login-div">
    <center>
    
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td >
                                <asp:Label ID="uiLblReason" runat="server" Font-Bold="True" ForeColor="Red" 
                                    Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="login-form" >
                                <br />
                                <br />
                                <br />
                                <asp:Login ID="uiAuthLogin" runat="server" 
                                    DisplayRememberMe="False" Font-Names="Verdana" Font-Size="12px" 
                                    DestinationPageUrl="~/Default.aspx" 
                                    onauthenticate="Login1_Authenticate" 
                                    onloginerror="uiAuthLogin_LoginError" TitleText="" PasswordLabelText="" 
                                    UserNameLabelText="" LoginButtonImageUrl="~/Images/login.gif" 
                                    LoginButtonType="Image" Height="100px" Width="16px" UserName="" 
                                    onloggedin="uiAuthLogin_LoggedIn" >
                                    <TextBoxStyle Font-Size="12px" />
                                    <LayoutTemplate>
                                        <table border="0" cellpadding="1" cellspacing="0" 
                                            style="border-collapse:collapse;">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="0" style="height:100px;width:467px;" 
                                                        cellspacing="0">
                                                       
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                <table cellpadding="0" cellspacing="0" class="style6">
                                                                     <tr>
                                                            <td align="right" style="font:arial; font-size:small;font-weight:bold;color:White;padding-right:2px;" >
                                                                &nbsp;</td>
                                                            <td style="text-align: left" class="style4"  >
                                                                &nbsp;</td>
                                                        </tr>
                                                                     <tr>
                                                                         <td align="right" 
                                                                             style="font:arial; font-size:small;font-weight:bold;color:White;padding-right:2px;">
                                                                             User Name:
                                                                         </td>
                                                                         <td class="style4" style="text-align: left">
                                                                             <asp:TextBox ID="UserName" runat="server" Font-Size="12px" Width="120px" 
                                                                                 AutoCompleteType="Disabled"></asp:TextBox>
                                                                             <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                                                 ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                                                 ToolTip="User Name is required." ValidationGroup="uiAuthLogin">*</asp:RequiredFieldValidator>
                                                                         </td>
                                                                     </tr>
                                                        <tr>
                                                            <td align="right" style="font:arial; font-size:small;font-weight:bold;color:White;padding-right:2px;" >Password:
                                                            </td>
                                                            <td valign="top" style="text-align: left; vertical-align: top;">
                                                                <asp:TextBox ID="Password" runat="server" Font-Size="12px" TextMode="Password" 
                                                                    Width="120px" AutoCompleteType="Disabled"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                                    ControlToValidate="Password" ErrorMessage="Password is required." 
                                                                    ToolTip="Password is required." ValidationGroup="uiAuthLogin">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style5">
                                                                &nbsp;</td>
                                                            <td align="right" style="text-align: left" class="style5">
                                                                &nbsp;</td>
                                                        </tr>
                                                                     <tr>
                                                                         <td class="style5">
                                                                         </td>
                                                                         <td align="right" class="style5" style="text-align: left">
                                                                             <asp:ImageButton ID="LoginImageButton" runat="server" AlternateText="Log In" 
                                                                                 CommandName="Login" ForeColor="#284775" ImageUrl="~/Images/login.gif" 
                                                                                 ValidationGroup="uiAuthLogin" />
                                                                         </td>
                                                                     </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="text-align: center">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                             <td align="right" 
                                                                style="color:#FF3300; font-weight:bold; text-align: left; font-family: Arial;">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                    <LoginButtonStyle Font-Names="Verdana" Font-Size="12px" ForeColor="#284775" />  
                                </asp:Login>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                &nbsp;</td>
                        </tr>
                            </table>
                        </td>
            </tr>
            </table>
    
    </center>
    </div>
    <script language="javascript" type="text/javascript">
        document.getElementById("uiAuthLogin_Password").value = "";
    </script>
    </form>
</body>
</html>
