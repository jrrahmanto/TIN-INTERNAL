<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EntryContractStressTestScenario.aspx.cs" Inherits="WebUI_FinanceAndAccounting_EntryManagePostingCode"  %>

<%@ Register src="../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h1>Manage Contract Stress Test Scenario</h1>
    <table cellpadding="1" cellspacing="1" style="width:100%;">
      <tr>
                        <td colspan="3">
                <asp:BulletedList ID="uiBLError" runat="server" ForeColor="Red">
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
                            Commodity
                            Contract</td>
                        <td style="width:10px">
                            :</td>
                        <td>
                            <uc1:CtlContractCommodityLookup ID="CtlContractCommodityLookup" 
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Scenario&nbsp;Type</td>
                        <td>
                        :</td>
                        <td>
                            <asp:DropDownList ID="uiDdlScenarioType" CssClass="required" runat="server" Width="120px">
                                <asp:ListItem Value="P">Percent</asp:ListItem>
                                <asp:ListItem Value="V">Value</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                                        <tr>
                                            <td>
                                                Base Price</td>
                        <td>
                            :</td>
                                            <td>
                                                <asp:TextBox ID="uiTxtBestPrice" CssClass="number"  runat="server" Width="120px"></asp:TextBox>
                                                <cc1:MaskedEditExtender ID="uiTxtBestPrice_MaskedEditExtender" runat="server" 
                                                    AcceptNegative="Left" CultureAMPMPlaceholder="" 
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" DisplayMoney="Left" 
                                                    Enabled="True" ErrorTooltipEnabled="True" InputDirection="RightToLeft" 
                                                    Mask="9,999,999.99" MaskType="Number" TargetControlID="uiTxtBestPrice">
                                                </cc1:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Low</td>
                        <td>
                            :</td>
                                            <td>
                                                -
                                                <asp:TextBox ID="uiTxtLow" runat="server" CssClass="number" Width="108px"></asp:TextBox>
                                                 <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                                                    AcceptNegative="Left" CultureAMPMPlaceholder="" 
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                                    CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                    Enabled="True" ErrorTooltipEnabled="True" InputDirection="RightToLeft" 
                                                    Mask="9,999,999.99" MaskType="Number" TargetControlID="uiTxtLow">
                                                </cc1:MaskedEditExtender>
                                                %</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                High</td>
                        <td>
                            :</td>
                        <td>
                            +<asp:TextBox ID="uiTxtHigh" runat="server" CssClass="number" Width="108px"></asp:TextBox><cc1:FilteredTextBoxExtender 
                                ID="uiTxtHigh_FilteredTextBoxExtender" runat="server" Enabled="True" 
                                FilterType="Numbers" TargetControlID="uiTxtHigh">
                            </cc1:FilteredTextBoxExtender>
                            %</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                        <asp:Button ID="uiBtnSave" CssClass="button_save" runat="server" Text="      Save" 
                                onclick="uiBtnSave_Click"  />
                            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                                Text="      Cancel" onclick="uiBtnCancel_Click" />
                        </td>
                    </tr>
                </table>
                </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
            &nbsp;</td>
        </tr>
    </table>
</asp:Content>

