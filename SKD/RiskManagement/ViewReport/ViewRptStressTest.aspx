<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="ViewRptStressTest.aspx.cs" Inherits="WebUI_Fajar_StressTest" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:Panel ID="uiPanelTitle" runat="server" Width="100%" >
            <center >
            <table border="1" cellspacing="0" cellpadding="0" style="text-align:center; width:400px;">
                <tr>
                    <td style="text-align:center; vertical-align:middle;">
                        <span style="font-size:larger; font-weight:bold; text-align:center">Using 
                            <select>
                                <option>Scenario 1</option>
                                <option>Scenario 2</option>
                                <option>Scenario 3</option>
                                <option>Scenario 4</option>
                            </select>
                        </span> 
                        <span id="uiLblInfo"></span>
                        <br />
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
                        <asp:GridView ID="uiDgManageExchange0" runat="server" 
                            AutoGenerateColumns="False" MouseHoverRowHighlightEnabled="True" 
                            RowHighlightColor="" Width="100%">
                            <RowStyle CssClass="tblRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="contract" HeaderText="Commodity Contract" 
                                    SortExpression="contract" />
                                <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                                <asp:BoundField DataField="base" HeaderText="Base" SortExpression="base" />
                                <asp:BoundField DataField="low" HeaderText="Low" SortExpression="low" />
                                <asp:BoundField DataField="high" HeaderText="High" SortExpression="high" />
                            </Columns>
                            <headerstyle CssClass="tblHeaderStyle" />
                            <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                        </asp:GridView>
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
        </cc1:CollapsiblePanelExtender>

                <table cellpadding="1" cellspacing="1" style="width:100%;">
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Commodity Contract</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="DropDownList1" runat="server">
                                <asp:ListItem>Show All</asp:ListItem>
                                <asp:ListItem>Olein 2009011</asp:ListItem>
                                <asp:ListItem>Olein 2009012</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="form-content-menu">
                        <td class="form-content-menu">
                            Clearing Member</td>
                        <td class="separator">
                            :</td>
                        <td class="right_search_criteria">
                            <asp:DropDownList ID="DropDownList2" runat="server">
                                <asp:ListItem>Show All</asp:ListItem>
                                <asp:ListItem>PT AAA</asp:ListItem>
                                <asp:ListItem>PT BBB</asp:ListItem>
                                <asp:ListItem>PT CCC</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="right_search_criteria">
                        <td class="form-content-menu">
                        </td>
                        <td class="separator">
                        </td>
                        <td>
                            <asp:Button ID="uiBtnSearch" CssClass="button_search" runat="server" Text="     Search" />
                        </td>
                    </tr>
                </table>
                        
  <asp:GridView ID="uiDgManageExchange" runat="server" 
                    AutoGenerateColumns="False" Width="100%" 
                    MouseHoverRowHighlightEnabled="True" 
                    RowHighlightColor="">
                    <RowStyle CssClass="tblRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="kontrak" HeaderText="Commodity Contract" SortExpression="kontrak" />
                        <asp:BoundField DataField="member" HeaderText="Clearing Member" SortExpression="member" />
                        <asp:BoundField DataField="account" HeaderText="Account" SortExpression="account" />
                        <asp:BoundField HeaderText="Open Interest"></asp:BoundField>
                        <asp:BoundField HeaderText="Total Exposure"></asp:BoundField>
                        <asp:BoundField HeaderText="Low"></asp:BoundField>
                        <asp:BoundField HeaderText="High"></asp:BoundField>
                    </Columns>
                    <headerstyle CssClass="tblHeaderStyle" />
                    <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                </asp:GridView>        
</asp:Content>
