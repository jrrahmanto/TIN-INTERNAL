<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RandomizedTenderDetail.aspx.cs" Inherits="WebUI_New_RandomizedTenderDetail" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

        </asp:Content>
        
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <h1>Randomized Tender Detail</h1>
<table cellpadding="1" cellspacing="1" style="width:100%;">
<tr>
            <td>
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
            <td >
                    <Asp:GridView ID="uiDgRequestTender" runat="server" 
                        AutoGenerateColumns="False" MouseHoverRowHighlightEnabled="True" 
                        RowHighlightColor="" Width="800px" 
                        DataKeyNames="BuyerInvID,MatchDate,Price,TradePosition,TenderID" 
                        onrowdatabound="uiDgRequestTender_RowDataBound">
                        <RowStyle CssClass="tblRowStyle" />
                        <Columns>
                            <asp:TemplateField HeaderText="Buyer Account" SortExpression="BuyerInvID">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BuyerInvID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="uiLblBuyerInvId" runat="server" Text='<%# Bind("BuyerInvID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Match Date" DataField="MatchDate" 
                                SortExpression="MatchDate" ReadOnly="True" ItemStyle-HorizontalAlign="Center" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Price" HeaderText="Price" 
                                SortExpression="Price" ReadOnly="True" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Trade Position" DataField="TradePosition" 
                                SortExpression="TradePosition" ReadOnly="True" />
                            <asp:BoundField HeaderText="TenderID" DataField="TenderID" ReadOnly="True" 
                                SortExpression="TenderID" Visible="False" />
                            <asp:BoundField HeaderText="Tender Result Type" DataField="TenderResultType" 
                                SortExpression="TenderResultType" />
                            <asp:BoundField HeaderText="Quantity" DataField="Quantity" 
                                SortExpression="Quantity" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <headerstyle CssClass="tblHeaderStyle" />
                        <AlternatingRowStyle CssClass="tblAlternatingRowStyle" />
                    </Asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSourceTenderRandomizedDetail" 
                        runat="server" SelectMethod="GetTenderResultByTenderId" TypeName="Tender">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="tenderId" QueryStringField="tenderId" 
                                Type="Decimal" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
        <td>
            <asp:Button ID="uiBtnRerun" runat="server" CssClass="button_rerunrandom" 
                text="   Re-Run Randomize" onclick="uiBtnRerun_Click" />
&nbsp;
            <asp:Button ID="uiBtnCancel" runat="server" CssClass="button_cancel"  
                Text="      Cancel" onclick="uiBtnCancel_Click" />
        &nbsp;</td>
        </tr>
         </table>
        </div>
 </div>
   </td>
        </tr>
 
 </table>
</asp:Content>

