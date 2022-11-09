<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RedoEOD.aspx.cs" Inherits="ClearingAndSettlement_RedoEOD" %>

<%@ Register src="../Controls/CtlCalendarPickUp.ascx" tagname="CtlCalendarPickUp" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Undo End of Day</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Wizard ID="uiWzStartOfDay" runat="server" ActiveStepIndex="0" 
                    DisplaySideBar="False" Height="159px" Width="100%"
                    BackColor="#F7F6F3" 
                    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                    onactivestepchanged="uiWzStartOfDay_ActiveStepChanged" 
                    onnextbuttonclick="uiWzStartOfDay_NextButtonClick" 
                    onpreviousbuttonclick="uiWzStartOfDay_PreviousButtonClick" 
                    onfinishbuttonclick="uiWzStartOfDay_FinishButtonClick" 
                            FinishCompleteButtonText="Next" Font-Names="Verdana"  >
                            <StepStyle BorderWidth="0px" ForeColor="#5D7B9D" />
                            <WizardSteps>
                                <asp:WizardStep runat="server" StepType="Start" Title="Set Parameter">
                                 <tr>
            <td>
            <div class="shadow_view">
            <div class="box_view">
                                    <table class="table-row">
                                        
                                        <tr>
                                            <td colspan="3">
                                                <asp:BulletedList ID="uiBLErrorSetParameter" runat="server" ForeColor="Red">
                                                </asp:BulletedList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Business Date</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <uc1:CtlCalendarPickUp ID="CtlCalendarPickUp1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    </div>
                </div>
            </td>
        </tr>
                                </asp:WizardStep>
                                <asp:WizardStep runat="server" StepType="Finish" Title="Send File">
                                <tr>
            <td>
            <div class="shadow">
            <div class="box  ">
                                    <table class="table-row">
                                        <tr>
                                            <td>
                                                <asp:BulletedList ID="uiBLErrorSendFile" runat="server" ForeColor="Red">
                                                </asp:BulletedList>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Click next to process Undo End Of Day</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    </div>
                </div>
            </td>
        </tr>
                                </asp:WizardStep>
                                <asp:WizardStep runat="server" StepType="Complete" Title="File Sended">
                                <tr>
            <td>
            <div class="shadow">
            <div class="box  ">
                                    <table class="table-row">
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Undo End of Day is finished
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    </div>
                </div>
            </td>
        </tr>
                                </asp:WizardStep>
                            </WizardSteps>
                            <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
                            <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                                ForeColor="#284775" />
                            <SideBarStyle BackColor="#7C6F57" BorderWidth="0px" Font-Size="0.9em" 
                                VerticalAlign="Top" />
                            <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" 
                                Font-Size="0.9em" ForeColor="White" HorizontalAlign="Left" />
                        </asp:Wizard>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <asp:Label ID="uiLblProgress" runat="server" Font-Size="Larger" 
                                        Text="Undo End of Day is in progress"></asp:Label>
                        <br />
                        <img src="../Images/ajax-loader2.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
</asp:Content>

