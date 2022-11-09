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

public partial class FinanceAndAccounting_EntryBuktiPotong : System.Web.UI.Page
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
                    CtlClearingMemberLookup1.SetDisabledClearingMember(true);
                    CtlCalendarPickUp1.SetDisabledCalendar(true);
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

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";

        // Only for maker user, guard by UI
        try
        {
            if (IsValidEntry() == true)
            {
                string monthYear = "";
                int month = 0;
                int year = 0;
                year = int.Parse(CtlYearMonthBuktiPotong1.Year);
                month = int.Parse(CtlYearMonthBuktiPotong1.Month);
                monthYear = string.Format("{0:0000}{1:00}", year, month);

                // Case Update/Revision
                if (eID != 0)
                {
                    // Guard for editing proposed record
                    BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                    actionFlag = "U";

                    BuktiPotong.ProposeBuktiPotong(DateTime.Parse(CtlCalendarPickUp1.Text), decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                                                     uiTxtTaxNo.Text, decimal.Parse(uiTxtTax.Text), monthYear,
                                                    uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));

                }
                else
                {
                     BuktiPotong.ProposeBuktiPotong(DateTime.Parse(CtlCalendarPickUp1.Text), decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                                                     uiTxtTaxNo.Text, decimal.Parse(uiTxtTax.Text), monthYear,
                                                    uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                }

                Response.Redirect("ViewBuktiPotong.aspx");
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
        
            try
            {
                // Guard for editing proposed record
                BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    BuktiPotong.ApproveBuktiPotong(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewBuktiPotong.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
           
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
       
            try
            {
                // Guard for editing proposed record
                BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    BuktiPotong.RejectProposedBuktiPotong(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewBuktiPotong.aspx");
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
            string monthYear = "";
            int month = 0;
            int year = 0;
            year = int.Parse(CtlYearMonthBuktiPotong1.Year);
            month = int.Parse(CtlYearMonthBuktiPotong1.Month);
            monthYear = string.Format("{0:0000}{1:00}", year, month);

            if (Convert.ToString(eID) != "")
            {
                // Guard for editing proposed record
                BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                BuktiPotong.ProposeBuktiPotong(DateTime.Parse(CtlCalendarPickUp1.Text), decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                                                     uiTxtTaxNo.Text, decimal.Parse(uiTxtTax.Text), monthYear,
                                                    uiTxtApprovalDesc.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));
            }
            Response.Redirect("ViewBuktiPotong.aspx");

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewBuktiPotong.aspx");
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

            string monthYear = "";
            int month = 0;
            int year = 0;
            if (CtlYearMonthBuktiPotong1.Year == "" )
            {
                uiBLError.Items.Add("Year is required.");
            }
            else if (CtlYearMonthBuktiPotong1.Month == "")
            {
                uiBLError.Items.Add("Month is required.");
            }
            else
            {
                year = int.Parse(CtlYearMonthBuktiPotong1.Year);
                month = int.Parse(CtlYearMonthBuktiPotong1.Month);
                monthYear = string.Format("{0:0000}{1:00}", year, month);
            }   
            if (eType == "add")
            {
                if (CtlClearingMemberLookup1.LookupTextBoxID != "")
                {
                    //cek apakah bukti potong sudah ada apa belum
                    BuktiPotongData.WithholdingTaxDataTable dt = new BuktiPotongData.WithholdingTaxDataTable();
                    dt = BuktiPotong.SelectBuktiPotongByPK(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUp1.Text));

                    if (dt.Count > 0)
                    {
                        uiBLError.Items.Add("Bukti potong is already exist.");
                    }
                }
                else 
                {
                    if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
                    {
                        uiBLError.Items.Add("Clearing member is required.");
                    }
                }

               
            }


            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                uiBLError.Items.Add("Business date is required.");
            }

            if (string.IsNullOrEmpty(uiTxtTaxNo.Text))
            {
                uiBLError.Items.Add("Tax number is required.");
            }

            if (string.IsNullOrEmpty(uiTxtTax.Text))
            {
                uiBLError.Items.Add("Tax is required.");
            }

            if (string.IsNullOrEmpty(monthYear))
            {
                uiBLError.Items.Add("Year month is required.");
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
            BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
            CtlCalendarPickUp1.SetCalendarValue(dr.BusinessDate.ToString("dd-MMM-yyyy"));
            CtlClearingMemberLookup1.SetDisabledClearingMember(true);
            CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(), dr.CMCode.ToString());
            uiTxtTaxNo.Text = dr.TaxNo;
            uiTxtTax.Text = dr.Tax.ToString("#,##0.##");
            //CtlYearMonthBuktiPotong1.SetDisabledYearMonth(true);
            CtlYearMonthBuktiPotong1.SetMonthYear(int.Parse(dr.Period.Substring(4, 2)), int.Parse(dr.Period.Substring(0, 4)));

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
            uiBtnDelete.Visible = mp.IsMaker;
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                BuktiPotongData.WithholdingTaxRow dr = BuktiPotong.SelectBuktiPotongByTaxID(Convert.ToDecimal(eID));
                CtlClearingMemberLookup1.SetDisabledClearingMember(true);
                CtlCalendarPickUp1.SetDisabledCalendar(true);
                CtlYearMonthBuktiPotong1.SetMonthYear(int.Parse(dr.Period.Substring(4, 2)), int.Parse(dr.Period.Substring(0, 4)));
                CtlYearMonthBuktiPotong1.SetDisabledMonthYear(true);
                uiTxtTax.Enabled = false;
                uiTxtTaxNo.Enabled = false;
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
