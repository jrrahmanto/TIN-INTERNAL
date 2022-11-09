using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;

public partial class AuditAndCompliance_ViewTradeFeedProxyLog : System.Web.UI.Page
{
    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiDdlExchange.Focus();
    }
    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillTradeFeedLogDataGrid();
    }
    protected void uiDgTradeFeedLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTradeFeedLog.PageIndex = e.NewPageIndex;
        FillTradeFeedLogDataGrid();
    }
    protected void uiDgTradeFeedLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Change exchange id to exchange name
            Label exchange = (Label)e.Row.FindControl("uiLblExchange");
            exchange.Text = Exchange.GetExchangeNameByExchangeId(decimal.Parse(exchange.Text));
        }
    }
    protected void uiDgTradeFeedLog_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            GridViewRow gvr = ((GridView)e.CommandSource).Rows[int.Parse(e.CommandArgument.ToString())];
            Label exchange = (Label)gvr.FindControl("uiLblExchangeId");
            Label logDate = (Label)gvr.FindControl("uiLblLogDate");
            Label logSeq = (Label)gvr.FindControl("uiLblLogSeq");

            try
            {
                byte[] logFile = TradeFeedProxyLog.GetLogFile(decimal.Parse(exchange.Text), 
                    DateTime.Parse(logDate.Text), int.Parse(logSeq.Text));
                
                MemoryStream ms = new MemoryStream(logFile);
                
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                
                string log =  System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length).Replace("\0","");
                string filename = string.Format("{0}_{1}.txt", DateTime.Parse(logDate.Text).ToString("yyyymmdd"), logSeq.Text);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/text";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.Output.Write(log);
                Response.Flush();
                Response.Close();

                ApplicationLog.Insert(DateTime.Now, "Tradefeed proxy log", "I", "User Download Tradefeed proxy log", Page.User.Identity.Name, Common.GetIPAddress(this.Request));
            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.Message, ex);
            }
            
        }
    }
    protected void uiDgTradeFeedLog_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (string.IsNullOrEmpty(SortOrder))
        {
            SortOrder = e.SortExpression + " " + "DESC";
        }
        else
        {
            string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
            if (arrSortOrder[1] == "ASC")
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else if (arrSortOrder[1] == "DESC")
            {
                SortOrder = e.SortExpression + " " + "ASC";
            }
        }

        FillTradeFeedLogDataGrid();
    }    
    private void FillTradeFeedLogDataGrid()
    {
        uiDgTradeFeedLog.DataSource = ObjectDataSourceTradeFeedLog;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTradeFeedLog.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        uiDgTradeFeedLog.DataSource = dve;
        uiDgTradeFeedLog.DataBind();
    }
    
}
