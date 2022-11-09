using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ClearingAndSettlement_MasterData_EntryProduct : System.Web.UI.Page
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
        try
        {
           
            uiBLError.Visible = false;
         
            if (!Page.IsPostBack)
            {
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtProductCode.Enabled = false;
                    bindData();
                }
            }
            SetAccessPage();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";

        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                // Case Update/Revision
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";
                }

                Product.ProposeProduct(uiTxtProductCode.Text, uiDdlExchangeType.SelectedValue, uiTxtProductName.Text,
                                           DateTime.Now, User.Identity.Name, DateTime.Now,
                                           uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID), User.Identity.Name);

                Response.Redirect("ViewManageProduct.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageProduct.aspx");
    }
    
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                Product.ProposeProduct(uiTxtProductCode.Text, uiDdlExchangeType.SelectedValue, uiTxtProductName.Text,
                                       DateTime.Now, User.Identity.Name, DateTime.Now,uiTxtApprovalDesc.Text, "D", User.Identity.Name,
                                       Convert.ToDecimal(eID), User.Identity.Name);
            }
            Response.Redirect("ViewManageProduct.aspx");
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
                ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Product.ApproveParameter(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }

                Response.Redirect("ViewManageProduct.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }     
    }
    
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Product.RejectProposedProduct(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewManageProduct.aspx");
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
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (mp.IsMaker)
            {
                if (eType == "add")
                {
                    if (uiTxtProductCode.Text != "")
                    {
                        //cek apakah bank code sudah ada apa belum
                        ProductData.ProductDataTable dt = new ProductData.ProductDataTable();
                        dt = Product.SelectProductByCode(uiTxtProductCode.Text);

                        if (dt.Count > 0)
                        {
                            uiBLError.Items.Add("Product code is already exist.");
                        }
                    }
                }

                if (string.IsNullOrEmpty(uiTxtProductCode.Text))
                {
                    uiBLError.Items.Add("Product code is required.");
                }

                if (string.IsNullOrEmpty(uiTxtProductName.Text))
                {
                    uiBLError.Items.Add("Product name is required.");
                }

            }

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            
            if (uiBLError.Items.Count > 0)
            {
                isValid = false;
                uiBLError.Visible = true;
            }

            return isValid;

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void bindData()
    {
        try
        {
            ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));
            uiTxtProductCode.Text = dr.ProductCode;
            uiTxtProductName.Text = dr.ProductName;
            uiDdlExchangeType.SelectedValue = dr.ExchangeType;

            string actionDesc = "";
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
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void SetAccessPage()
    {
        try
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
                ProductData.ProductRow dr = Product.SelectProductByProductID(Convert.ToDecimal(eID));

                uiTxtProductCode.Enabled = false;
                uiTxtProductName.Enabled = false;
                uiDdlExchangeType.Enabled = false;
                uiTxtAction.Enabled = false;
             }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion


}
