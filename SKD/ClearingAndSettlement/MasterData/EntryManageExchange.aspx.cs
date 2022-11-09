using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;


public partial class WebUI_ClearingAndSettlement_EntryManageExchange : System.Web.UI.Page
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
        SetAccessPage();
        uiBLError.Visible = false;

        if (!IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                uiTxtExchangeCode.Enabled = false;
                bindData();
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageExchange.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";
        try
        {
            if (IsValidEntry() == true)
            {
                               
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be edited. Please wait for checker approval.");
                    actionFlag = "U";
                }
                  
                Exchange.ProposeExchange(uiTxtExchangeCode.Text, uiTxtExchangeIP.Text,
                                             uiTxtLocalIP.Text, int.Parse(uiTxtLocalPort.Text),
                                             uiDdlExchangeType.SelectedValue,
                                             uiTxtExhangeName.Text, actionFlag, uiDdlTenderFlag.SelectedValue, 
                                             uiDdlTransferFlag.SelectedValue, User.Identity.Name, DateTime.Now, User.Identity.Name,
                                             DateTime.Now, User.Identity.Name, eID, uiTxbExchangeIPAddressOutbound.Text, uiTxbLocalIPAddressOutbound.Text, int.Parse(uiTxbLocalPortOutbound.Text));
                Response.Redirect("ViewManageExchange.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("This data is already approved. Please select other proposed data.");

                 if (eID.ToString() != "")
                {
                    Exchange.ApproveExchange(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApporvalDesc.Text, User.Identity.Name, DateTime.Now);
                }
                Response.Redirect("ViewManageExchange.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (eID.ToString() != "")
            {
                // Guard for editing proposed record
                ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be deleted. Please wait for checker approval.");

                Exchange.ProposeExchange(uiTxtExchangeCode.Text, uiTxtExchangeIP.Text,
                                         uiTxtLocalIP.Text, int.Parse(uiTxtLocalPort.Text),
                                         uiDdlExchangeType.SelectedValue,
                                         uiTxtExhangeName.Text, "D",uiDdlTenderFlag.SelectedValue, 
                                         uiDdlTransferFlag.SelectedValue, User.Identity.Name, DateTime.Now,
                                         User.Identity.Name, DateTime.Now, User.Identity.Name, Convert.ToDecimal(eID),
                                         uiTxbExchangeIPAddressOutbound.Text, uiTxbLocalIPAddressOutbound.Text, int.Parse(uiTxbLocalPortOutbound.Text));
            }
            Response.Redirect("ViewManageExchange.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("This data is already approved. Please select other proposed data.");

                if (eID.ToString() != "")
                {
                    Exchange.RejectProposedExchange(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewManageExchange.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (eType == "add")
        {
            if (string.IsNullOrEmpty(uiTxtExchangeCode.Text))
            {
                uiBLError.Items.Add("Exchange code is required.");
            }
            else
            {
                //cek apakah bank code sudah ada apa belum
                ExchangeData.ExchangeDataTable dt = new ExchangeData.ExchangeDataTable();
                dt = Exchange.SelectExchangeByExchangeCode(uiTxtExchangeCode.Text);

                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("Exchange code is already exist.");
                }
            }
        }

     
        if (mp.IsChecker)
        {
            if (string.IsNullOrEmpty(uiTxtApporvalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }
        
        
        if (string.IsNullOrEmpty(uiTxtExhangeName.Text))
        {
            uiBLError.Items.Add("Exchange name is required.");
        }

        
        if (string.IsNullOrEmpty(uiTxtExchangeIP.Text))
        {
            uiBLError.Items.Add("Exchange IP address is required.");
        }

        if (string.IsNullOrEmpty(uiTxtLocalPort.Text))
        {
            uiBLError.Items.Add("Local port is required.");
        }

        if (string.IsNullOrEmpty(uiTxtLocalIP.Text))
        {
            uiBLError.Items.Add("Local IP address is required.");
        }
       
        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }


    private void bindData()
    {
        ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));
      
            uiTxtExchangeCode.Text = dr.ExchangeCode;
            uiTxtExhangeName.Text = dr.ExchangeName;
            uiTxtExchangeIP.Text = dr.ExchangeIPAddress;
            uiTxtLocalIP.Text = dr.LocalIPAddress;
            uiTxtLocalPort.Text = dr.LocalPort.ToString();
            uiDdlExchangeType.SelectedValue = dr.ExchangeType.ToString();
            
            if (!dr.IsTenderFlagNull())
            {
                uiDdlTenderFlag.SelectedValue = dr.TenderFlag.ToString();
            }
            else 
            {
                uiDdlTenderFlag.SelectedValue = "N";
            }

            if (!dr.IsTransferFlagNull())
            {
                uiDdlTransferFlag.SelectedValue = dr.TransferFlag.ToString();
            }
            else
            {
                uiDdlTransferFlag.SelectedValue = "N";
            }
            string actionDesc = "";
            if (!dr.IsExchangeIPAddressOutboundNull())
            {
                uiTxbExchangeIPAddressOutbound.Text = dr.ExchangeIPAddressOutbound;
            }
            if (!dr.IsLocalIPAddressOutboundNull())
            {
                uiTxbLocalIPAddressOutbound.Text = dr.LocalIPAddressOutbound;
            }
            if (!dr.IsLocalPortOutboundNull())
            {
                uiTxbLocalPortOutbound.Text = dr.LocalPortOutbound.ToString();
            }
            //cek actionflag null
            if (!dr.IsActionFlagNull())
            {
                if (dr.ActionFlag == "I")
                {
                    actionDesc = "New Record";
                }
                else if (dr.ActionFlag == "U")
                {
                    actionDesc = "Revision";
                }
                else if (dr.ActionFlag == "D")
                {
                    actionDesc = "Delete";
                }
            }
            uiTxtAction.Text = actionDesc;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;

        trAction.Visible = mp.IsChecker || mp.IsViewer;
        trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
       
        if (eType == "edit")
        {
            uiBtnDelete.Visible = mp.IsMaker;
        }
        uiBtnSave.Visible = mp.IsMaker;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;

        // set disabled for other controls other than approval description, for checker
        if (mp.IsChecker)
        {
            ExchangeData.ExchangeRow dr = Exchange.SelectExchangeByExchangeID(Convert.ToDecimal(eID));

            
            uiTxtAction.Enabled = false;
            uiTxtExchangeCode.Enabled = false;
            uiTxtExhangeName.Enabled = false;
            uiTxtExchangeIP.Enabled = false;
            uiTxtLocalIP.Enabled = false;
            uiTxtLocalPort.Enabled = false;
            uiDdlExchangeType.Enabled = false;
            uiDdlTenderFlag.Enabled = false;
            uiDdlTransferFlag.Enabled = false;
        }
    }

    #endregion
}
