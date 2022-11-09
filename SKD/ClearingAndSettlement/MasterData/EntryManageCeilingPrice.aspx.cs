using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClearingAndSettlement_MasterData_EntryManageCeilingPrice : System.Web.UI.Page
{
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

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageCeilingPrice.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if(eID != 0 && IsValidEntry())
            {
                CeilingPrice.UpdateCeilingPrice(Convert.ToDecimal(uiTxtCeilingPrice.Text), Convert.ToDecimal(uiTxtFloorPrice.Text), eID, User.Identity.Name);
                Session["CeilingPrice"] = Convert.ToDecimal(uiTxtCeilingPrice.Text).ToString("#,###.##");
                Session["FloorPrice"] = Convert.ToDecimal(uiTxtFloorPrice.Text).ToString("#,###.##");
            }
            Response.Redirect("ViewManageCeilingPrice.aspx");
        }
        catch(Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        bool isValid = true;

        if((uiTxtCeilingPrice.Text == null || uiTxtCeilingPrice.Text == ""))
        {
            uiTxtCeilingPrice.Text = "0";
        }
        else
        {
            if (Convert.ToDecimal(uiTxtCeilingPrice.Text) <= 0)
            {
                uiTxtCeilingPrice.Text = "0";
            }
        }

        if ((uiTxtFloorPrice.Text == null || uiTxtFloorPrice.Text == ""))
        {
            uiTxtFloorPrice.Text = "0";
        }
        else
        {
            if (Convert.ToDecimal(uiTxtFloorPrice.Text) <= 0)
            {
                uiTxtFloorPrice.Text = "0";
            }
        }

        return isValid;
    }

    private void bindData()
    {
        CeilingPriceData.CeilingPriceRow dr = CeilingPrice.GetCeilingPriceById(Convert.ToDecimal(eID));
        uiTxtCeilingPrice.Text = dr.CeilingPrice.ToString("#,###.##");
        uiTxtFloorPrice.Text = dr.FloorPrice.ToString("#,###.##");
    }

    private void SetAccessPage()
    {
        
    }

    #endregion
}