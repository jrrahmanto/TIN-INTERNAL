using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PengelolaGudang_DailyStock_EntryDailyStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {

            }
            else if (eType == "edit")
            {
                bindData();
            }
        }

        SetAccessPage();
    }

    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private decimal eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(Request.QueryString["eID"]);
            }
        }
        set { ViewState["eID"] = value; }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewDailyStock.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry())
            {
                if (eID != 0)
                {
                    StockWarehouseNew.UpdateStockWarehouse(decimal.Parse(uiDtpCommID.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text), txtLocation.Text, decimal.Parse(txtVolume.Text), eID);
                }
                else
                {
                    StockWarehouseNew.InsertStockWarehouse(decimal.Parse(uiDtpCommID.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text), txtLocation.Text, decimal.Parse(txtVolume.Text));
                }
            }
            Response.Redirect("ViewDailyStock.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private bool IsValidEntry()
    {
        bool isValid = true;

        

        return isValid;
    }

    private void bindData()
    {
        DailyStockData.StockWarehouseRow dr = StockWarehouseNew.GetStockWarehouseById(Convert.ToDecimal(eID));
        //CtlCalendarPickUp1.Text = dr.BusinessDate.ToString();
        txtLocation.Text = dr.Location.ToString();
        txtVolume.Text = dr.Volume.ToString();

        CommodityData.CommodityDataTable dt = new CommodityData.CommodityDataTable();
        dt = Commodity.FillByCommodityID2(dr.CommodityID);

        uiDtpCommID.SetCommodityValue(dt[0].CommodityID.ToString(), dt[0].CommodityCode.ToString());
        dt.Dispose();
    }

    private void SetAccessPage()
    {

    }
}