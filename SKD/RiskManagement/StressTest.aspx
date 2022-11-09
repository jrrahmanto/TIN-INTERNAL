<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="StressTest.aspx.cs" Inherits="WebUI_Fajar_StressTest" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../Lookup/CtlCommodityLookup.ascx" tagname="CtlCommodityLookup" tagprefix="uc1" %>
<%@ Register src="../Lookup/CtlContractCommodityLookup.ascx" tagname="CtlContractCommodityLookup" tagprefix="uc2" %>
<%@ Register src="../Lookup/CtlClearingMemberLookup.ascx" tagname="CtlClearingMemberLookup" tagprefix="uc3" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1>Stress Test</h1>
      
            <%--<div class="shadow_view" visible="false">
            <div class="box_view" width="800" visible="false">
            <table class="table-row" visible="false">
                <tr visible="false">
                    <td style="text-align:center; vertical-align:middle;" visible="false">
                        <span style="font-size:larger; font-weight:bold; text-align:center">Using 
                            <asp:DropDownList ID="uiDdlScenario" runat="server" 
                            DataSourceID="ScenarioData" DataTextField="ScenarioName" 
                            DataValueField="ScenarioID">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="ScenarioData" runat="server" DeleteMethod="Delete" 
                            InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
                            SelectMethod="GetData" 
                            TypeName="StressTestScenarioDataTableAdapters.StressTestScenarioTableAdapter" 
                            UpdateMethod="Update">
                            <DeleteParameters>
                                <asp:Parameter Name="Original_ScenarioName" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="ScenarioDate" Type="DateTime" />
                                <asp:Parameter Name="CreatedBy" Type="String" />
                                <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                                <asp:Parameter Name="Original_ScenarioName" Type="String" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ScenarioName" Type="String" />
                                <asp:Parameter Name="ScenarioDate" Type="DateTime" />
                                <asp:Parameter Name="CreatedBy" Type="String" />
                                <asp:Parameter Name="CreatedDate" Type="DateTime" />
                                <asp:Parameter Name="LastUpdatedBy" Type="String" />
                                <asp:Parameter Name="LastUpdatedDate" Type="DateTime" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
&nbsp;</span><span id="uiLblInfo"></span><br />
                        <a href="ViewStressTestScenario.aspx">Manage Scenarios</a>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center; vertical-align:middle;">

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="uiPanelContent" runat="server">
                        <Asp:GridView ID="uiDgScenario" runat="server" 
                            AutoGenerateColumns="False" MouseHoverRowHighlightEnabled="True" 
                            RowHighlightColor="" Width="780px">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="CommodityName" HeaderText="CommodityName" 
                                    SortExpression="CommodityName" ReadOnly="True" />
                                <asp:BoundField DataField="ScenarioType" HeaderText="ScenarioType" 
                                    SortExpression="ScenarioType" />
                                <asp:BoundField DataField="BasePrice" HeaderText="BasePrice" 
                                    SortExpression="BasePrice" />
                                <asp:BoundField DataField="LowPrice" HeaderText="LowPrice" 
                                    SortExpression="LowPrice" ReadOnly="True" />
                                <asp:BoundField DataField="HighPrice" HeaderText="HighPrice" 
                                    SortExpression="HighPrice" ReadOnly="True" />
                            </Columns>
                            <headerstyle CssClass="tblHeaderStyle" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </Asp:GridView>
                            <asp:ObjectDataSource ID="StressTestData" runat="server" 
                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetByScenarioID" 
                                TypeName="StressTestScenarioDataTableAdapters.vw_view_scenarioTableAdapter">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="scenarioID" QueryStringField="ID" 
                                        Type="Decimal" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            </center>
        </asp:Panel>
        <hr />
        <cc1:CollapsiblePanelExtender ID="Panel1_CollapsiblePanelExtender" 
            runat="server" Enabled="True" TargetControlID="uiPanelContent"
            CollapseControlID="uiLblInfo"
            ExpandControlID="uiLblInfo"
            TextLabelID="uiLblInfo"
            CollapsedText="(click to show contract detail)" 
            ExpandedText="(click to hide contract detail)"
            Collapsed="false"
            ScrollContents="false">
        </cc1:CollapsiblePanelExtender>--%>

                <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr >
                        <td>
                            Commodity Contract</td>
                        <td>
                            :</td>
                        <td>
                            <uc2:CtlContractCommodityLookup ID="uiLookupContract" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Clearing Member</td>
                        <td>
                            :</td>
                        <td>
                            <uc3:CtlClearingMemberLookup ID="uiLookupClearingMember" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" 
                                Text="    Search" onclick="uiBtnSearch_Click" />
                        </td>
                    </tr>
                </table>
                  <table width="100%">
                    <tr>
                        <td>
                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                        
                            
                          
                            <rsweb:ReportViewer ID="uiRptViewer" runat="server" ProcessingMode="Remote" 
                                ShowParameterPrompts="False" Width="100%">
                            </rsweb:ReportViewer>
                          
                            
                          
                        </td>
                    </tr>
                  </table>      
  <Asp:GridView ID="uiDgResult" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="ContractName" HeaderText="ContractName" 
                            SortExpression="ContractName" />
                        <asp:BoundField HeaderText="CMName" DataField="CMName" ReadOnly="True" 
                            SortExpression="CMName"></asp:BoundField>
                        <asp:BoundField HeaderText="Account" DataField="Account" 
                            SortExpression="Account" ReadOnly="True"></asp:BoundField>
                        <asp:BoundField HeaderText="Open Interest" DataField="OpenInterest"
                            SortExpression="OpenInterest" DataFormatString="{0:#,##0.000}">
                        </asp:BoundField>
                        <asp:BoundField DataField="ExposureHigh" 
                            HeaderText="Exposure High" ReadOnly="True"  DataFormatString="{0:#,##0.000}"
                            SortExpression="ExposureHigh">
                        </asp:BoundField>
                        <asp:BoundField DataField="ExposureLow" 
                            HeaderText="Exposure Low" ReadOnly="True"  DataFormatString="{0:#,##0.000}"
                            SortExpression="ExposureLow">
                        </asp:BoundField>
                        <asp:BoundField DataField="SegShortageExcessHigh" 
                            HeaderText="Seg Shortage Excess High" ReadOnly="True"  DataFormatString="{0:#,##0.000}"
                            SortExpression="SegShortageExcessHigh">
                        </asp:BoundField>
                        <asp:BoundField DataField="SegShortageExcessLow" HeaderText="Seg Shortage Excess Low"  DataFormatString="{0:#,##0.000}"
                            ReadOnly="True" SortExpression="SegShortageExcessLow">
                        </asp:BoundField>
                        <asp:BoundField DataField="UnsegShortageExcessHigh" HeaderText="Unseg Shortage Excess High"  DataFormatString="{0:#,##0.000}"
                            ReadOnly="True" SortExpression="UnsegShortageExcessHigh">
                        </asp:BoundField>
                        <asp:BoundField DataField="UnsegShortageExcessLow" 
                            HeaderText="Unseg Shortage Excess Low" ReadOnly="True"  DataFormatString="{0:#,##0.000}"
                            SortExpression="UnsegShortageExcessLow" />
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </Asp:GridView>        
                <asp:ObjectDataSource ID="ContractResultData" runat="server" 
                    OldValuesParameterFormatString="original_{0}" 
                    SelectMethod="GetDataByScenarioID" 
                    TypeName="StressTestScenarioDataTableAdapters.ScenarioContractResultTableAdapter">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="scenarioID" QueryStringField="ID" 
                            Type="Decimal" />
                    </SelectParameters>
                </asp:ObjectDataSource>
        
</asp:Content>
