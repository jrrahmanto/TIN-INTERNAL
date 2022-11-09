using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryRiskType : System.Web.UI.Page
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
        //disable end date
        //CtlCalendarEndDate.SetDisabledCalendar(true);

        if (!IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                uiTxtRiskTypeCode.Enabled = false;
                bindData();
            }
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewRiskType.aspx");
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
                    //// Guard for editing proposed record
                    //RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));
                    //if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    ////guard for number record
                    //RiskTypeDataTableAdapters.RiskTypeTableAdapter ta = new RiskTypeDataTableAdapters.RiskTypeTableAdapter();
                    //RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
                    //decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(uiTxtRiskTypeCode.Text, DateTime.Parse(CtlCalendarStartDate.Text), eID));
                    //if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");

                    //actionFlag = "U";
                    RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));

                    RiskType.UpdateRiskType(uiTxtRiskType.Text, uiTxtDescription.Text, User.Identity.Name, 
                        DateTime.Now, uiTxtRiskTypeCode.Text, "A", dr.EffectiveStartDate);
                }
                else
                {
                    RiskType.InsertRiskType(uiTxtRiskTypeCode.Text, uiTxtRiskType.Text,
                        uiTxtApprovalDesc.Text, DateTime.Now.Date, null, uiTxtDescription.Text,
                        "", User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now, 0);
                }

                //if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                //{
                //    RiskType.ProposeRiskType(uiTxtRiskTypeCode.Text, uiTxtRiskType.Text, uiTxtApprovalDesc.Text,
                //                             Convert.ToDateTime(CtlCalendarStartDate.Text), null,
                //                             uiTxtDescription.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                //}
                //else
                //{
                //    RiskType.ProposeRiskType(uiTxtRiskTypeCode.Text, uiTxtRiskType.Text, uiTxtApprovalDesc.Text,
                //                                Convert.ToDateTime(CtlCalendarStartDate.Text), Convert.ToDateTime(CtlCalendarEndDate.Text),
                //                                uiTxtDescription.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                //}

                Response.Redirect("ViewRiskType.aspx");
            }
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            if (ex.Message.Contains("Violation of PRIMARY KEY"))
            {
                uiBLError.Items.Add("This record already exist.");
            }
            else
            {
                uiBLError.Items.Add(ex.Message);            
            }     
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {

                    RiskType.RejectProposedRiskTypeID(Convert.ToDecimal(eID), User.Identity.Name);

                }
                Response.Redirect("ViewRiskType.aspx");
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
            if (IsValidEntry() == true)
            {
                // Guard for editing proposed record
                RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    RiskType.ApproveRiskTypeID(Convert.ToDecimal(eID),  User.Identity.Name);
                }
                Response.Redirect("ViewRiskType.aspx");
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
            if (IsValidEntry() == true)
            {

                ////if (Convert.ToString(eID) != "")
                ////{
                ////    // Guard for editing proposed record
                ////    RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));
                ////    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not delete pending approval record.");

                ////    if (string.IsNullOrEmpty(CtlCalendarEndDate.Text))
                ////    {
                ////        RiskType.ProposeRiskType(uiTxtRiskTypeCode.Text, uiTxtRiskType.Text, uiTxtApprovalDesc.Text,
                ////                                Convert.ToDateTime(CtlCalendarStartDate.Text), null,
                ////                                uiTxtDescription.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));

                ////    }
                ////    else
                ////    {
                ////        RiskType.ProposeRiskType(uiTxtRiskTypeCode.Text, uiTxtRiskType.Text, uiTxtApprovalDesc.Text,
                ////                             Convert.ToDateTime(CtlCalendarStartDate.Text), Convert.ToDateTime(CtlCalendarEndDate.Text),
                ////                             uiTxtDescription.Text, "D", User.Identity.Name, Convert.ToDecimal(eID));
                ////    }

                ////}
                RiskType.DeleteRiskTypeByRiskTypeID(eID);

                Response.Redirect("ViewRiskType.aspx");
            }

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    #region SupportingMethod

    private void bindData()
    {

        RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));

        uiTxtRiskTypeCode.Text = dr.RiskTypeCode.ToString();
        uiTxtRiskType.Text = dr.RiskType.ToString();
        uiTxtDescription.Text = dr.Description.ToString();

        MasterPage mp = (MasterPage)this.Master;

        if (mp.IsChecker && eType == "edit")
        {
            CtlCalendarStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd/MM/yyyy"));
        }
        
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


    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        //if (eType == "add")
        //{
        //    //cek apakah risktypecode sudah ada apa belum
        //    RiskTypeData.RiskTypeDataTable dt = new RiskTypeData.RiskTypeDataTable();
        //    dt = RiskType.SelectRiskTypeByRiskTypeCode(uiTxtRiskTypeCode.Text);

        //    if (dt.Count > 0)
        //    {
        //        uiBLError.Items.Add("RiskTypeCode is already exist.");
        //    }
        //}

        //if (mp.IsChecker)
        //{
        //    if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
        //    {
        //        uiBLError.Items.Add("Approval description is required.");
        //    }
        //}

        //if (string.IsNullOrEmpty(CtlCalendarStartDate.Text))
        //{
        //    uiBLError.Items.Add("Effective Start Date is required.");
        //}

        if (string.IsNullOrEmpty(uiTxtRiskTypeCode.Text))
        {
            uiBLError.Items.Add("Risktype code is required.");
        }
        if (string.IsNullOrEmpty(uiTxtRiskType.Text))
        {
            uiBLError.Items.Add("Risk type is required.");
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
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
            RiskTypeData.RiskTypeRow dr = RiskType.SelectRiskTypeByRiskTypeID(Convert.ToDecimal(eID));
            uiTxtAction.Enabled = false;
            uiTxtRiskType.Enabled = false;
            uiTxtRiskTypeCode.Enabled = false;
            uiTxtDescription.Enabled = false;

            if (dr.IsEffectiveEndDateNull())
            {
                CtlCalendarEndDate.DisabledCalendar = true;
                CtlCalendarEndDate.SetCalendarValue(null);
            }
            else
            {
                CtlCalendarEndDate.DisabledCalendar = true;
                CtlCalendarEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MM-yyyy"));
            }
        }
    }

    #endregion

    
}
