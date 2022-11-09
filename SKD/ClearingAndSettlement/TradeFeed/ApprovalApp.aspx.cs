using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_TradeFeed_ApprovalApp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBlError.Items.Clear();
        uiBlError.Visible = false;

        SetAccessPage();

        if (!Page.IsPostBack)
        {
            if (eType == "edit")
            {
                BindData();
            }
            else if (eType == "adjust")
            {
                BindData();
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (eType == "edit")
        {
            Response.Redirect("APP.aspx");
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            EODTradeProgress.ApproveApp(decimal.Parse(uiTxbProgressID.Text), uiDtpBussDate.Text);

            Response.Redirect("APP.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            EODTradeProgress.RejectApp(decimal.Parse(uiTxbProgressID.Text), uiDtpBussDate.Text);

            Response.Redirect("APP.aspx");
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetEnabledControls(bool b)
    {
        uiTxbProgressID.Enabled = b;
        uiDtpBussDate.SetDisabledCalendar(!b);
        uiTxbPrice.Enabled = b;
    }

    private void SetVisibleControls(bool b)
    {
        uiBtnApprove.Visible = b;
        uiBtnReject.Visible = b;
    }

    private void BindData()
    {
        try
        {
            EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter ta = new EODTradeProgressDataTableAdapters.EODTradeProgressTableAdapter();
            EODTradeProgressData.EODTradeProgressDataTable dt = new EODTradeProgressData.EODTradeProgressDataTable();

            dt = ta.GetDataAppByProgressID(ProgressID);

            if (dt.Count > 0)
            {
                uiTxbProgressID.Text = dt[0].ProgressID.ToString();
                uiTxbExchangeRef.Text = dt[0].ExchangeRef;
                uiTxbBuyerId.Text = dt[0].BuyerId;
                uiTxbSellerId.Text = dt[0].SellerId;
                uiTxbProductCode.Text = dt[0].ProductCode;
                uiTxbVolume.Text = dt[0].Volume.ToString();
                uiTxbPrice.Text = dt[0].Price.ToString();

                uiLblApp.Text = dt[0].AppFormPath;
            }
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
            uiBlError.Visible = true;
        }
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;

        SetEnabledControls(false);
    }

    protected void uiBtnApp_Click(object sender, EventArgs e)
    {
        if (uiLblApp.Text != "" && uiLblApp.Text != null)
        {
            ShowFile(@"F:/Document/" + uiLblApp.Text);
        }
    }

    private void ShowFile(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = ReturnExtension(file.Extension.ToLower());
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".pdf":
                return "application/pdf";
            default:
                return "application/octet-stream";
        }
    }

    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    private decimal ProgressID
    {
        get { return decimal.Parse(Request.QueryString["progressId"].ToString()); }
    }

    private string eType
    {
        get { return Request.QueryString["eType"]; }
    }

    private string CreatedBy
    {
        get { return ViewState["CreatedBy"].ToString(); }
        set { ViewState["CreatedBy"] = value; }
    }

    private DateTime CreatedDate
    {
        get { return (DateTime)ViewState["CreatedDate"]; }
        set { ViewState["CreatedDate"] = value; }
    }
}