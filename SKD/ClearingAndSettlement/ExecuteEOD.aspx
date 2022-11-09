<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
     CodeFile="ExecuteEOD.aspx.cs" Inherits="WebUI_ClearingAndSettlement_ExecuteEOD" 
     %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/CtlCalendarPickUp.ascx" TagName="CtlCalendarPickUp"
    TagPrefix="uc1" %>
    
    
<%@ Register src="../Controls/CtlMonthYear.ascx" tagname="CtlMonthYear" tagprefix="uc2" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h1>
        Execute EOD</h1>
    <table cellpadding="1" cellspacing="1" style="width: 100%;">
        <tr>
            <td>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Wizard ID="uiWzEOD" runat="server" ActiveStepIndex="0" BackColor="#F7F6F3" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" FinishCompleteButtonText="Next" Font-Names="Verdana"
                            Font-Size="Small" Width="100%" OnFinishButtonClick="Wizard1_FinishButtonClick"
                            OnNextButtonClick="Wizard1_NextButtonClick" Height="215px" DisplaySideBar="False"
                            OnActiveStepChanged="Wizard1_ActiveStepChanged" 
                            OnPreviousButtonClick="uiWzEOD_PreviousButtonClick">
                            <StepStyle BorderWidth="0px" ForeColor="#5D7B9D" />
                            <WizardSteps>
                                <asp:WizardStep ID="WizardStep1" runat="server" Title="Select Start Date and Hint"
                                    StepType="Start">
                                    <tr>
                                        <td>
                                            <div class="shadow_view">
                                                <div class="box_view">
                                                    <table class="table-other">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:BulletedList ID="uiBLWizard1" runat="server" ForeColor="Red">
                                                                </asp:BulletedList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:150px">
                                                                EOD Date:
                                                                                                                                
                                                                </td>
                                                            <td>
                                                                <uc1:CtlCalendarPickUp ID="CtlCalendarPickUpStartDate" runat="server" />
                                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" 
                                                                    Visible="False" />
                                                                <br />
                                                                <div class="hint">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Redo EOD
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="uiChkIsRedo" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td >
                                                                Rerun Randomize</td>
                                                            <td>
                                                                <asp:CheckBox ID="uiChkReRunRandomize" runat="server" Checked="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td rowspan="3" >
                                                                Select Options:
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="uiRbnGlobal" runat="server" Text="Global" Checked="True" GroupName="Hint" />
                                                                <br />
                                                                <div class="hint">
                                                                    All Clearing Members Position and Profit/Loss will be recalculated.
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="uiRbnContract" runat="server" Text="Contract" GroupName="Hint" />
                                                                <br />
                                                                <div class="hint">
                                                                    All Clearing Members Position and Profit/Loss that have transaction on the selected
                                                                    start date will be recalculated.
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="uiRbnTrade" runat="server" Text="Trade" GroupName="Hint" />
                                                                <br />
                                                                <div class="hint">
                                                                    All Clearing Members Position and Profit/Loss that have trades that have been modified
                                                                    on the selected start date will be recalculated.
                                                                </div>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:WizardStep>
                                <%--<asp:WizardStep ID="WizardStep2" runat="server" StepType="Step" Title="Select Contracts">
                                    <tr>
                                        <td>
                                            <div class="shadow">
                                                <div class="box  ">
                                                    <table class="table-row">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:BulletedList ID="uiBLWizard2" runat="server" ForeColor="Red" Visible="False">
                                                                </asp:BulletedList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="uiChkContract" runat="server" RepeatColumns="4" DataSourceID="ObjectDataSourceContractEOD"
                                                                    DataTextField="ContractCode" DataValueField="ContractID" Width="100%">
                                                                </asp:CheckBoxList>
                                                                <asp:ObjectDataSource ID="ObjectDataSourceContractEOD" runat="server" SelectMethod="GetContractEOD"
                                                                    TypeName="EOD" OldValuesParameterFormatString="original_{0}">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="businessDate" SessionField="StartDate" Type="DateTime" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <br />
                                                                <table id="uiTbTradeDate" runat="server" style="width: 100%;">
                                                                    <tr id="Tr1" runat="server">
                                                                        <td id="Td1" runat="server" >
                                                                            Trade Date<uc1:CtlCalendarPickUp ID="CtlCalendarPickUpTradeDate" runat="server" />
                                                                        </td>
                                                                        <td id="Td2" runat="server">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:WizardStep>--%>
                                <asp:WizardStep ID="WizardStep3" runat="server" Title="Redo End of Day" StepType="Step">
                                    <asp:GridView ID="uiDgPrerequisiteStatus" runat="server" AutoGenerateColumns="False"
                                        MouseHoverRowHighlightEnabled="True" RowHighlightColor="" Width="100%">
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="No" HeaderText="No" ReadOnly="True" SortExpression="No"
                                                Visible="False" />
                                            <asp:BoundField DataField="PrerequisitesValidation" HeaderText="PrerequisitesValidation"
                                                SortExpression="PrerequisitesValidation" ReadOnly="True"></asp:BoundField>
                                            <asp:BoundField DataField="StatusValidation" HeaderText="StatusValidation" ReadOnly="True"
                                                SortExpression="StatusValidation" />
                                            <asp:BoundField DataField="Url" HeaderText="Url" ReadOnly="True" SortExpression="Url"
                                                Visible="False" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <input id="Button3" class="button_detail" type="button" value="detail" onclick='javascript:window.open("<%# DataBinder.Eval(Container.DataItem, "Url") %>?menu=hide");'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="tblHeaderStyle" />
                                        <RowStyle CssClass="tblRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourcePrerequisitesEOD" runat="server" SelectMethod="GetNewPrerequisitesEOD"
                                        TypeName="EOD" OldValuesParameterFormatString="original_{0}">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="businessDate" SessionField="StartDate" Type="DateTime" />
                                            <asp:SessionParameter Name="EODType" SessionField="EODType" Type="String" />
                                            <%--<asp:ControlParameter ControlID="uiChkReRunRandomize" Name="isReRunRandomize" 
                                                PropertyName="Checked" Type="Boolean" />--%>
                                            <asp:SessionParameter Name="isRedo" SessionField="IsRedo" Type="Boolean" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <table visible="false" id="tblErrorPrerequisites" runat="server" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="uiLblErrorPrerequisites" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width:100%;">
                                                    
                                                    <tr>
                                                        <td >
                                                            <asp:Button ID="uiBtnRerunPrerequisites0" runat="server" 
                                                                OnClick="uiBtnRerunPrerequisites_Click" Text="Re-run prerequisites" />
                                                        </td>
                                                        <td >
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            &nbsp;</td>
                                                        <td >
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:GridView ID="uiDgExchangeInfo" runat="server" AutoGenerateColumns="False" MouseHoverRowHighlightEnabled="True"
                                        RowHighlightColor="" Width="100%">
                                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                                        <Columns>
                                            <asp:BoundField HeaderText="ExchangeName" DataField="ExchangeName" ReadOnly="True"
                                                SortExpression="ExchangeName" />
                                            <asp:BoundField HeaderText="Total Trade" DataField="TotalTransaction" ReadOnly="True"
                                                SortExpression="TotalTransaction" DataFormatString="{0:#,##0}" />
                                            <asp:BoundField HeaderText="Total Position" DataField="TotalPosition" ReadOnly="True"
                                                SortExpression="TotalPosition" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField HeaderText="Total Buy" DataField="TotalBuy" ReadOnly="True" 
                                                SortExpression="TotalBuy" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField HeaderText="Total Sell" DataField="TotalSell" ReadOnly="True" 
                                                SortExpression="TotalSell" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField HeaderText="VariationMargin" DataField="VariationMargin" ReadOnly="True"
                                                SortExpression="VariationMargin" Visible="False" />
                                        </Columns>
                                        <HeaderStyle CssClass="tblHeaderStyle" />
                                        <RowStyle CssClass="tblRowStyle" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ObjectDataSourceEODExchangeInfo" runat="server" SelectMethod="GetEODExchangeInfo"
                                        TypeName="EOD">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="businessDate" SessionField="StartDate" Type="DateTime" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <br />
                                    <br />
                                    &nbsp;<asp:Label ID="uiLblFinish" runat="server" Text=""></asp:Label><asp:BulletedList
                                        ID="uiBlContract" runat="server" Visible="false">
                                    </asp:BulletedList>
                                    &nbsp;<br />
                                </asp:WizardStep>

                                <asp:WizardStep ID="WizardStep4" runat="server" StepType="Finish" Title="End Of Day Progress">
                                    <tr>
                                        <td>
                                            <div class="shadow">
                                                <div class="box  ">
                                                    <table class="table-other">
                                                        <tr>
                                                            <td>
                                                                <asp:BulletedList ID="uiBLWizardEODProgress" runat="server" ForeColor="Red">
                                                                </asp:BulletedList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Click Next to process End of Day
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <rsweb:ReportViewer ID="uiRptViewer" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                                                    Height="168px" Visible="False" Width="100%">
                                                                </rsweb:ReportViewer>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </asp:WizardStep>
                                <asp:WizardStep ID="WizardStep5" runat="server" StepType="Complete" 
                                    Title="End of Day Calculation finished">
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
                                                                End of Day is finished
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
                                    <%--<asp:Label ID="uiLblFinished" runat="server" Font-Size="Larger" 
                                        Text="End of Day Finished"></asp:Label>--%>
                                </asp:WizardStep>
                            </WizardSteps>
                            <SideBarButtonStyle Width="200px" BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
                            <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
                                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
                            <SideBarStyle BackColor="#7C6F57" BorderWidth="0px" Font-Size="0.9em" VerticalAlign="Top" />
                            <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                                ForeColor="White" HorizontalAlign="Left" />
                        </asp:Wizard>
                        <asp:Button ID ="uibtnGenerate" runat="server" Text="Generate report" OnClick="uibtnGenerate_Click"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                    AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="../Images/ajax-loader2.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
                
            </td>
        </tr>
    </table>
</asp:Content>
