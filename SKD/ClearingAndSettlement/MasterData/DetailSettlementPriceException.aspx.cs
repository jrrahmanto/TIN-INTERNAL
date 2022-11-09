using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class ClearingAndSettlement_MasterData_DetailSettlementPriceException : System.Web.UI.Page
{

    private decimal ExchangeID
    {
        get { return decimal.Parse(Request.QueryString["exchangeId"]); }
    }

    private decimal SPID
    {
        get { return decimal.Parse(Request.QueryString["SPID"]); }
    }

    private DateTime BusinessDate
    {
        get { return DateTime.Parse(Request.QueryString["businessDate"]); }
    }

    private string ApprovalStatus
    {
        get { return Request.QueryString["approvalStatus"]; }
    }

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
    }
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            SettlementPriceException _tfExceptionHandler = new SettlementPriceException();

            try
            {
                TextBox txtApprovalDesc = new TextBox();
                txtApprovalDesc = ((TextBox)uiDvSettlementPriceException.FindControl("uiTxtApprovalDesc"));

                _tfExceptionHandler.Approve(ExchangeID,
                    BusinessDate,
                    SPID, User.Identity.Name);

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewSettlementPriceException.aspx");
                }
                else
                {
                    Response.Redirect("ViewSettlementPriceException.aspx?menu=hide");
                }
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        } 
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry())
        {
            SettlementPriceException _spExceptionHandler = new SettlementPriceException();
            try
            {
                
              TextBox txtApprovalDesc = new TextBox();
              txtApprovalDesc = ((TextBox)uiDvSettlementPriceException.FindControl("uiTxtApprovalDesc"));

                    _spExceptionHandler.Reject(ExchangeID,
                                           BusinessDate,
                                           SPID, txtApprovalDesc.Text, User.Identity.Name);
                

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewSettlementPriceException.aspx");
                }
                else
                {
                    Response.Redirect("ViewSettlementPriceException.aspx?menu=hide");
                }
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }     
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Menu))
        {
            Response.Redirect("ViewSettlementPriceException.aspx");
        }
        else
        {
            Response.Redirect("ViewSettlementPriceException.aspx?menu=hide");
        }       
    }

    private bool IsValidEntry()
    {
        bool isValid = true;

        SettlementPriceException _tfExceptionHandler = new SettlementPriceException();
        SettlementPriceExceptionData.SPExceptionDataTable dt = _tfExceptionHandler.GetData(ExchangeID, BusinessDate, SPID);
       
       
       TextBox txtApprovalDesc = new TextBox();
       txtApprovalDesc = ((TextBox)uiDvSettlementPriceException.FindControl("uiTxtApprovalDesc"));

       if (txtApprovalDesc.Text == "")
        {
                txtApprovalDesc.BorderColor = System.Drawing.Color.Red;
                txtApprovalDesc.ToolTip = "Please fill approval desctiption.";
                uiBLError.Items.Add("Approval description is required.");
        }
       
        if (dt[0].ApprovalStatus == "A")
        {
            uiBLError.Items.Add("This record has been approved.");
        }
        else if (dt[0].ApprovalStatus == "R")
        {
            uiBLError.Items.Add("This record has been rejected.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }
}
