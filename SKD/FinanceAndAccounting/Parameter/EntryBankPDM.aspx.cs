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

public partial class WebUI_New_EntryBankPDM : System.Web.UI.Page
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
                    uiTxtBankCode.Enabled = false;
                    bindData();
                }
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
        Response.Redirect("ViewBankPDM.aspx");
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
                    BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";
                }
               
                BankPDM.ProposeBankPDM(uiTxtBankCode.Text, uiTxtBICode.Text,
                                         uiTxtName.Text, uiTxtBranch.Text, uiTxtDivision.Text, uiTxtAddress.Text,
                                         uiTxtProvince.Text, uiTxtPostCode.Text, uiTxtCountry.Text, uiTxtPhoneNo.Text,
                                         uiTxtFax.Text, uiTxtContact1.Text, uiTxtApporvalDesc.Text, uiTxtContact2.Text,
                                         uiTxtCity.Text, uiTxtStamp.Text, uiTxtDescription.Text,
                                        actionFlag, User.Identity.Name, Convert.ToDecimal(eID));

                Response.Redirect("ViewBankPDM.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            // Guard for editing proposed record
            BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(Convert.ToDecimal(eID));
            if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

            if (Convert.ToString(eID) != "")
            {
                BankPDM.ProposeBankPDM(uiTxtBankCode.Text, uiTxtBICode.Text,
                                         uiTxtName.Text, uiTxtBranch.Text, uiTxtDivision.Text, uiTxtAddress.Text,
                                         uiTxtProvince.Text, uiTxtPostCode.Text, uiTxtCountry.Text, uiTxtPhoneNo.Text,
                                         uiTxtFax.Text, uiTxtContact1.Text, uiTxtApporvalDesc.Text, uiTxtContact2.Text,
                                         uiTxtCity.Text, uiTxtStamp.Text, uiTxtDescription.Text,
                                        "D", User.Identity.Name, Convert.ToDecimal(eID));
            }
            Response.Redirect("ViewBankPDM.aspx");
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
                BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    BankPDM.ApproveBankPDM(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewBankPDM.aspx");
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
                BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    BankPDM.RejectProposedBankPDM(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewBankPDM.aspx");
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

            if (eType == "add")
            {
                //cek apakah bank code sudah ada apa belum
                BankData.BankDataTable dt = new BankData.BankDataTable();
                dt = BankPDM.SelectBankPDMByCode(uiTxtBankCode.Text);

                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("Bank code is already exist.");
                }
            }

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApporvalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(uiTxtBankCode.Text))
            {
                uiBLError.Items.Add("bankcode is required.");
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
            BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(eID);

            uiTxtBankCode.Text = dr.Code;
            uiTxtBICode.Text = dr.BICode;
            uiTxtName.Text = dr.Name;
            uiTxtBranch.Text = dr.Branch;
            uiTxtDivision.Text = dr.Division;
            uiTxtAddress.Text = dr.Address;
            uiTxtProvince.Text = dr.Province;
            uiTxtCity.Text = dr.City;
            uiTxtPostCode.Text = dr.PostCode;
            uiTxtCountry.Text = dr.Country;
            uiTxtPhoneNo.Text = dr.PhoneNo;
            uiTxtFax.Text = dr.Fax;
            uiTxtContact1.Text = dr.Contact1;
            uiTxtContact2.Text = dr.Contact2;
            uiTxtDescription.Text = dr.Description;
            uiTxtStamp.Text = dr.Stamp;

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

            trAction.Visible = mp.IsChecker ||  mp.IsViewer;
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            uiBtnDelete.Visible = mp.IsMaker;
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                BankData.BankRow dr = BankPDM.SelectBankPDMByBankID(Convert.ToDecimal(eID));
                uiTxtBankCode.Enabled = false;
                uiTxtBICode.Enabled = false;
                uiTxtName.Enabled = false;
                uiTxtBranch.Enabled = false;
                uiTxtDivision.Enabled = false;
                uiTxtAddress.Enabled = false;
                uiTxtProvince.Enabled = false;
                uiTxtCity.Enabled = false;
                uiTxtPostCode.Enabled = false;
                uiTxtCountry.Enabled = false;
                uiTxtPhoneNo.Enabled = false;
                uiTxtFax.Enabled = false;
                uiTxtContact1.Enabled = false;
                uiTxtContact2.Enabled = false;
                uiTxtDescription.Enabled = false;
                uiTxtStamp.Enabled = false;
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
