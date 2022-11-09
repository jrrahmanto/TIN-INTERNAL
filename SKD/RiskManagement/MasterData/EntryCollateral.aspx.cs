using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class WebUI_New_EntryCollateral : System.Web.UI.Page
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
     
        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                CtlClearingMemberLookup1.SetDisabledClearingMember(true);
                uiDdlCurrency.Enabled = false;
                uiTxtLodgementNo.Enabled = false;
                bindData();
            }
        }
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
          if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                    Collateral.ProposeCollateral(uiTxtLodgementNo.Text, dr.CMID, DateTime.Parse(CtlCalendarLodgementDate.Text),
                                                      uiDdlCollateralType.SelectedValue, uiDdlLodgementType.SelectedValue, uiTxtIssuer.Text,
                                                      DateTime.Parse(CtlCalendarIssuerDate.Text), decimal.Parse(uiDdlCurrency.SelectedValue),
                                                      DateTime.Parse(CtlCalendarMaturityDate.Text),decimal.Parse(uiTxtNominal.Text, new CultureInfo("en-us")),
                                                      decimal.Parse(uiTxtHaircut.Text, new CultureInfo("en-us")), decimal.Parse(uiTxtEffectiveNominal.Text, new CultureInfo("en-us")), 
                                                      uiTxtApprovalDesc.Text, "D", User.Identity.Name, Convert.ToDecimal(eID), uiDdlIssuerType.SelectedValue);
            }
            Response.Redirect("ViewCollateral.aspx");

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
                CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Collateral.ApproveCollateral(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewCollateral.aspx");
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
                CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Collateral.RejectProposedCollateral(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewCollateral.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewCollateral.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";

        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {

                if (uiTxtHaircut.Text == "")
                {
                    uiTxtHaircut.Text = "0";
                }
                
                if (uiTxtEffectiveNominal.Text == "")
                {
                    uiTxtEffectiveNominal.Text = "0";                
                }

                // Case Update/Revision
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";
    

                    Collateral.ProposeCollateral(uiTxtLodgementNo.Text, dr.CMID, DateTime.Parse(CtlCalendarLodgementDate.Text),
                                                         uiDdlCollateralType.SelectedValue, uiDdlLodgementType.SelectedValue, uiTxtIssuer.Text,
                                                         DateTime.Parse(CtlCalendarIssuerDate.Text), decimal.Parse(uiDdlCurrency.SelectedValue),
                                                         DateTime.Parse(CtlCalendarMaturityDate.Text), Convert.ToDecimal(uiTxtNominal.Text, new CultureInfo("en-us")),
                                                         Convert.ToDecimal(uiTxtHaircut.Text, new CultureInfo("en-us")), Convert.ToDecimal(uiTxtEffectiveNominal.Text, new CultureInfo("en-us")),
                                                         uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID), uiDdlIssuerType.SelectedValue);
                
                }
                else
                {
                    Collateral.ProposeCollateral(uiTxtLodgementNo.Text, decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), DateTime.Parse(CtlCalendarLodgementDate.Text),
                                                     uiDdlCollateralType.SelectedValue, uiDdlLodgementType.SelectedValue, uiTxtIssuer.Text,
                                                     DateTime.Parse(CtlCalendarIssuerDate.Text), decimal.Parse(uiDdlCurrency.SelectedValue),
                                                     DateTime.Parse(CtlCalendarMaturityDate.Text), Convert.ToDecimal(uiTxtNominal.Text, new CultureInfo("en-us")),
                                                     Convert.ToDecimal(uiTxtHaircut.Text, new CultureInfo("en-us")), Convert.ToDecimal(uiTxtEffectiveNominal.Text, new CultureInfo("en-us")),
                                                     uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID), uiDdlIssuerType.SelectedValue);
                }

                Response.Redirect("ViewCollateral.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    #region SupportingMethod

    protected void uiTxtNominal_TextChanged(object sender, EventArgs e)
    {
        uiTxtEffectiveNominal.Text = uiTxtNominal.Text;
        uiTxtHaircut.Text = "0";
    }

    protected void uiTxtHaircut_TextChanged(object sender, EventArgs e)
    {
        if (eType == "edit")
        {
            // Guard for editing proposed record
            CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));

            CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString());
            CtlClearingMemberLookup1.SetDisabledClearingMember(true);

            if (uiTxtHaircut.Text == "")
                uiTxtHaircut.Text = "0";


            decimal hairCut = (100 / 100) - (Convert.ToDecimal(uiTxtHaircut.Text) / 100);

            decimal calculatEffectiveNominal = Convert.ToDecimal(uiTxtNominal.Text, new CultureInfo("en-us")) * hairCut;

            uiTxtEffectiveNominal.Text = calculatEffectiveNominal.ToString("#,##0.##");
        }
        else
        {
            if (uiTxtHaircut.Text == "")
                uiTxtHaircut.Text = "0";

            decimal hairCut = (100 / 100) - (Convert.ToDecimal(uiTxtHaircut.Text) / 100);

            decimal calculatEffectiveNominal = Convert.ToDecimal(uiTxtNominal.Text, new CultureInfo("en-us")) * hairCut;

            uiTxtEffectiveNominal.Text = calculatEffectiveNominal.ToString("#,##0.##");
        }
    }


    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (eType == "add")
        {
            //cek apakah lodgement no sudah ada apa belum
            CollateralData.CollateralDataTable dt = new CollateralData.CollateralDataTable();
            dt = Collateral.SelectCollateralByLodgementNo(uiTxtLodgementNo.Text);

            if (dt.Count > 0)
            {
                uiBLError.Items.Add("Lodgement No is already exist.");
            }

            if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
            {
                isValid = false;
                uiBLError.Items.Add("Clearing Member is required.");
            }

            if (string.IsNullOrEmpty(uiTxtLodgementNo.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Lodgement No is required.");
            }

            if (string.IsNullOrEmpty(CtlCalendarLodgementDate.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Lodgement Date is required.");
            }

            if (string.IsNullOrEmpty(CtlCalendarIssuerDate.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Issuer Date is required.");
            }

            if (string.IsNullOrEmpty(CtlCalendarMaturityDate.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Maturity Date is required.");
            }

            if (string.IsNullOrEmpty(uiTxtNominal.Text))
            {
                isValid = false;
                uiBLError.Items.Add("Nominal is required.");
            }
            if (string.IsNullOrEmpty(uiTxtIssuer.Text.Trim()))
            {
                uiBLError.Items.Add("Issuer is required.");
            }
        }

        if (eType == "edit")
        {
            // Guard for editing proposed record
            CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));

            CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString());
            CtlClearingMemberLookup1.SetDisabledClearingMember(true);

            if (string.IsNullOrEmpty(uiTxtIssuer.Text.Trim()))
            {
                uiBLError.Items.Add("Issuer is required.");
            }
        }


        if (mp.IsChecker)
        {
            if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                isValid = false;
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

    private void bindData()
    {
        CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));

        CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString());
        uiDdlCurrency.SelectedValue = dr.CurrencyID.ToString();
        uiTxtLodgementNo.Text = dr.LodgementNo.ToString();
        CtlCalendarLodgementDate.SetCalendarValue(dr.LodgementDate.ToString("dd-MMM-yyyy"));
        uiDdlCollateralType.SelectedValue = dr.CollateralType;
        uiDdlLodgementType.SelectedValue = dr.LodgementType;
        uiTxtIssuer.Text = dr.Issuer;
        uiDdlIssuerType.SelectedValue = dr.IssuerType;
        CtlCalendarIssuerDate.SetCalendarValue(dr.IssuerDate.ToString("dd-MMM-yyyy"));
        CtlCalendarMaturityDate.SetCalendarValue(dr.MaturityDate.ToString("dd-MMM-yyyy"));
        uiTxtNominal.Text = dr.Nominal.ToString("#,##0.##");
        uiTxtHaircut.Text = dr.Haircut.ToString("#,##0.##");
        uiTxtEffectiveNominal.Text = dr.EffectiveNominal.ToString("#,##0.##");
        
        string actionDesc = "";
        //cek actionflag null
        if (!dr.IsActionFlagNull())
        {
            if (dr.ActionFlag == "I")
            {
                actionDesc = "Insert";
            }
            else if (dr.ActionFlag == "U")
            {
                actionDesc = "Update";
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
            CollateralData.CollateralRow dr = Collateral.SelectCollateralByCollateralID(Convert.ToDecimal(eID));

            CtlClearingMemberLookup1.SetDisabledClearingMember(true);
            uiDdlCurrency.Enabled =  false;
            uiTxtLodgementNo.Enabled = false;
            CtlCalendarLodgementDate.SetDisabledCalendar(true);
            uiDdlCollateralType.Enabled = false;
            uiDdlLodgementType.Enabled = false;
            uiTxtIssuer.Enabled = false;
            uiDdlIssuerType.Enabled = false;
            CtlCalendarIssuerDate.SetDisabledCalendar(true);
            CtlCalendarMaturityDate.SetDisabledCalendar(true);
            uiTxtNominal.Enabled = false;
            uiTxtHaircut.Enabled = false;
            uiTxtEffectiveNominal.Enabled = false;

        }
    }

    #endregion
}
