using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryDisiplinAK : System.Web.UI.Page
{
    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    private string approval
    {
        get
        {
            return Request.QueryString["status"];
        }
    }

    private string eID
    {
        get
        {
            if (Request.QueryString["eID"] == null)
            {
                return "";
            }
            else
            {
                return Request.QueryString["eID"];
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
            if (!Page.IsPostBack)
            {
                if (eType == "add")
                {
                    uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtSanctionNo.Enabled = false;
                    CtlClearingMemberLookupSanction.SetDisabledClearingMember(true);
                    BindData();
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
        try
        {
            if (IsValidEntry() == true)
            {
                 // Case Update/Revision
                if (eID != "")
                {
                    // Guard for editing proposed record
                    CMSanctionListData.CMSanctionListDataTable dt = Sanction.FillBySanctionNoAndApproval(eID, approval);
                    if (dt[0].ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                    {
                        Sanction.ProposedUpdate(uiTxtSanctionNo.Text, Convert.ToDecimal(CtlClearingMemberLookupSanction.LookupTextBoxID),
                                                 uiTxtDesc.Text, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                                 uiDdlSanctionSource.SelectedValue, uiDdlPenaltyType.SelectedValue,
                                                 uiDdlSanctionType.SelectedValue, Convert.ToDecimal(uitxtFineNominal.Text),
                                                 uiDdlSanctionStatus.SelectedValue, null, eID, User.Identity.Name);
                    }
                    else
                    {
                        Sanction.ProposedUpdate(uiTxtSanctionNo.Text, Convert.ToDecimal(CtlClearingMemberLookupSanction.LookupTextBoxID),
                                                  uiTxtDesc.Text, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                                  uiDdlSanctionSource.SelectedValue, uiDdlPenaltyType.SelectedValue,
                                                  uiDdlSanctionType.SelectedValue, Convert.ToDecimal(uitxtFineNominal.Text),
                                                  uiDdlSanctionStatus.SelectedValue, Convert.ToDateTime(CtlCalendarEndDate.Text), eID, User.Identity.Name);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                    {
                        Sanction.ProposedInsert(uiTxtSanctionNo.Text, Convert.ToDecimal(CtlClearingMemberLookupSanction.LookupTextBoxID),
                                              uiTxtDesc.Text, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                              uiDdlSanctionSource.SelectedValue, uiDdlPenaltyType.SelectedValue,
                                              uiDdlSanctionType.SelectedValue, Convert.ToDateTime(CtlCalendarSanctionDate.Text),
                                              User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now,
                                              Convert.ToDecimal(uitxtFineNominal.Text), uiDdlSanctionStatus.SelectedValue,
                                              null, User.Identity.Name);
                    }
                    else
                    {
                        Sanction.ProposedInsert(uiTxtSanctionNo.Text, Convert.ToDecimal(CtlClearingMemberLookupSanction.LookupTextBoxID),
                                                  uiTxtDesc.Text, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                                  uiDdlSanctionSource.SelectedValue, uiDdlPenaltyType.SelectedValue,
                                                  uiDdlSanctionType.SelectedValue, Convert.ToDateTime(CtlCalendarSanctionDate.Text),
                                                  User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now,
                                                  Convert.ToDecimal(uitxtFineNominal.Text), uiDdlSanctionStatus.SelectedValue,
                                                  Convert.ToDateTime(CtlCalendarEndDate.Text), User.Identity.Name);
                    }
                }
                
                Response.Redirect("ViewDisiplinAK.aspx");
              
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
        Response.Redirect("ViewDisiplinAK.aspx");
    }
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            if (eID != "")
            {
                try
                {
                    Sanction.ProposedDelete(uiTxtSanctionNo.Text,
                                       Convert.ToDecimal(CtlClearingMemberLookupSanction.LookupTextBoxID),
                                       uiTxtDesc.Text, Convert.ToDateTime(CtlCalendarStartDate.Text),
                                       uiDdlSanctionSource.SelectedValue, uiDdlPenaltyType.SelectedValue,
                                       uiDdlSanctionType.SelectedValue, Convert.ToDecimal(uitxtFineNominal.Text),
                                       uiDdlSanctionStatus.SelectedValue, Convert.ToDateTime(CtlCalendarEndDate.Text),
                                       eID, User.Identity.Name);
                    Response.Redirect("ViewDisiplinAK.aspx");
                }
                catch (Exception ex)
                {
                    uiBLError.Items.Add(ex.Message);
                    uiBLError.Visible = false;
                }
            }
        }
    }
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                CMSanctionListData.CMSanctionListDataTable dt = Sanction.FillBySanctionNoAndApproval(eID, approval);
                if (dt[0].ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (eID != "")
                {
                    Sanction.Approve(eID, uiTxbApprovalStatus.Text, uiTxbAction.Text, User.Identity.Name, dt[0].ApprovalStatus);
                    Response.Redirect("ViewDisiplinAK.aspx");
                }
                
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = false;
            }
        }
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            CMSanctionListData.CMSanctionListDataTable dt = Sanction.FillBySanctionNoAndApproval(eID, approval);
            if (dt[0].ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

            if (eID != "")
            {
                Sanction.Reject(eID, uiTxbApprovalStatus.Text, uiTxbAction.Text, User.Identity.Name, dt[0].ApprovalStatus);
                Response.Redirect("ViewDisiplinAK.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = false;
        }
    }

    #region "-------supporting method------"

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            CMSanctionListDataTableAdapters.CMSanctionListTableAdapter ta = new CMSanctionListDataTableAdapters.CMSanctionListTableAdapter();
            CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();

            if (eType == "add")
            {

                if (string.IsNullOrEmpty(uiTxtSanctionNo.Text))
                {
                    isValid = false;
                    uiBLError.Items.Add("Sanction number is required.");
                }

                //cek apakah risktypecode sudah ada apa belum
                dt = Sanction.FillBySanctionNo(uiTxtSanctionNo.Text);

                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("Sanction number is already exist.");
                }

                if (string.IsNullOrEmpty(uitxtFineNominal.Text))
                {
                    uitxtFineNominal.Text = "0";
                }

                if (string.IsNullOrEmpty(CtlClearingMemberLookupSanction.LookupTextBox))
                {
                    isValid = false;
                    uiBLError.Items.Add("Clearing Member is required.");
                }

            }

            if (eType == "edit" && mp.IsChecker)
            {

                CMSanctionListData.CMSanctionListDataTable dtr = Sanction.FillBySanctionNoAndApproval(eID, approval);
                if (dtr[0].ApprovalStatus == "A")
                {
                    isValid = false;
                    uiBLError.Items.Add("Record already approved.");
                }
                else if (dtr[0].ApprovalStatus == "P")
                {
                    if (string.IsNullOrEmpty(uiTxbApprovalStatus.Text))
                    {
                        isValid = false;
                        uiBLError.Items.Add("Approval description is required.");
                    }
                }
            }
            else if (eType == "edit" && mp.IsMaker)
            {
                CMSanctionListData.CMSanctionListDataTable dta = Sanction.FillBySanctionNoAndApproval(eID, null);
                if (dta[0].ApprovalStatus != "A")
                {
                    isValid = false;
                    uiBLError.Items.Add("Can not edit pending approval record.");
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
    
    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            trAction.Visible = mp.IsChecker || mp.IsViewer;
            trApproval.Visible = mp.IsChecker || mp.IsViewer;
            uiBtnDelete.Visible = mp.IsMaker;
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;
            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                CMSanctionListData.CMSanctionListDataTable dt = Sanction.SelectSanctionBySanctionNo(eID, approval);
                CtlClearingMemberLookupSanction.SetClearingMemberValue(dt[0].CMID.ToString(), dt[0].CMCode);
                CtlClearingMemberLookupSanction.SetDisabledClearingMember(true);
                uiDdlSanctionSource.Enabled = false;
                uiDdlSanctionType.Enabled = false;
                uiDdlPenaltyType.Enabled = false;
                uiTxtSanctionNo.Enabled = false;
                CtlCalendarStartDate.SetDisabledCalendar(true);
                CtlCalendarStartDate.SetCalendarValue(dt[0].StartDate.ToString("dd-MMM-yyyy"));
                if (dt[0].IsEndDateNull())
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(dt[0].EndDate.ToString("dd-MMM-yyyy"));
                }
                CtlCalendarSanctionDate.SetDisabledCalendar(true);
                CtlCalendarSanctionDate.SetCalendarValue(dt[0].SanctionDate.ToString("dd-MMM-yyyy"));
                uiDdlSanctionStatus.Enabled = false;
                uitxtFineNominal.Enabled = false;
                uiTxtDesc.Enabled = false;
                uiTxbAction.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private void BindData()
    {
        CMSanctionListData.CMSanctionListDataTable dt = new CMSanctionListData.CMSanctionListDataTable();
        try
        {
            dt = Sanction.FillBySanctionNoAndApproval(eID, approval);
            if (dt.Count > 0)
            {
                CtlClearingMemberLookupSanction.SetClearingMemberValue(dt[0].CMID.ToString(), dt[0].CMCode);
                uiDdlSanctionSource.SelectedValue = dt[0].SanctionSource;
                uiDdlPenaltyType.SelectedValue = dt[0].PenaltyType;
                uiDdlSanctionStatus.SelectedValue = dt[0].SanctionStatus;
                uiDdlSanctionType.SelectedValue = dt[0].SanctionType;
                uiTxtSanctionNo.Text = dt[0].SanctionNo;
                if (!dt[0].IsFineNominalNull())
                {
                        uitxtFineNominal.Text = dt[0].FineNominal.ToString("#,##0.##");
                }

                if (!dt[0].IsDescriptionNull())
                {
                    uiTxtDesc.Text = dt[0].Description;
                }
                CtlCalendarStartDate.SetCalendarValue(dt[0].StartDate.ToString("dd-MMM-yyyy"));
                if (dt[0].IsEndDateNull())
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarEndDate.SetDisabledCalendar(true);
                    CtlCalendarEndDate.SetCalendarValue(dt[0].EndDate.ToString("dd-MMM-yyyy"));
                }

                CtlCalendarSanctionDate.SetCalendarValue(dt[0].SanctionDate.ToString("dd-MMM-yyyy"));
                string actionDesc = "";
                //cek actionflag null
                if (!dt[0].IsActionFlagNull())
                {
                    if (dt[0].ActionFlag == "I")
                    {
                        actionDesc = "New Record";
                    }
                    else if (dt[0].ActionFlag == "U")
                    {
                        actionDesc = "Revision";
                    }
                    else if (dt[0].ActionFlag == "D")
                    {
                        actionDesc = "Delete";
                    }
                }
                uiTxbAction.Text = actionDesc;
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = false;
        }
    }

#endregion

}
