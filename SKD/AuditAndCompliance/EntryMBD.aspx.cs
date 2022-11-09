using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AuditAndCompliance_EntryMBD : System.Web.UI.Page
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
        uiBlError.Visible = false;
        
        if (!IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                uiCtlCm.SetDisabledClearingMember(true);
                BindData();
            }
        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (eID != null)
            {
                MBD.Proposed(eID, DateTime.Parse(uiDtpStartDate.Text),
                             decimal.Parse(uiTxbMBDValue.Text), decimal.Parse(uiTxbMBDFund.Text),
                             "U", User.Identity.Name, eID);
            }
            Response.Redirect("ViewMBD.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewMBD.aspx");
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            // Guard for editing proposed record
            MBDData.MBDRow dr = MBD.SelectMBDByMBDID(Convert.ToDecimal(eID));
            if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be deleted. Please wait for checker approval.");
            
            if (eID != null)
            {
                MBD.Proposed(eID, DateTime.Parse(uiDtpStartDate.Text),
                             decimal.Parse(uiTxbMBDValue.Text), decimal.Parse(uiTxbMBDFund.Text),
                             "D", User.Identity.Name, eID);
            }
            Response.Redirect("ViewMBD.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            // Guard for editing proposed record
            MBDData.MBDRow dr = MBD.SelectMBDByMBDID(Convert.ToDecimal(eID));
            if (dr.ApprovalStatus != "P") throw new ApplicationException("This data is already approved. Please select other proposed data.");

            if (eID != null)
            {
                MBD.Approve(eID, uiTxbApporvalDesc.Text,
                           uiTxbAction.Text, User.Identity.Name);
            }
            Response.Redirect("ViewMBD.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        try
        {
            // Guard for editing proposed record
            MBDData.MBDRow dr = MBD.SelectMBDByMBDID(Convert.ToDecimal(eID));
            if (dr.ApprovalStatus != "P") throw new ApplicationException("This data is already approved. Please select other proposed data.");

            if (eID != null)
            {
                MBD.Reject(eID,uiTxbAction.Text, User.Identity.Name);
            }
            Response.Redirect("ViewMBD.aspx");
        }
        catch (Exception ex)
        {
            uiBlError.Items.Add(ex.Message);
        }
    }

    private void BindData()
    {
        //MBDData.MBDDataTable dt = new MBDData.MBDDataTable();
        MBDData.MBDViewDataTable mvdt = new MBDData.MBDViewDataTable();

        try
        {
            mvdt = MBD.FillByMBDID2(eID);

            if (mvdt.Count > 0)
            {
                //uiCtlCm.SetClearingMemberValue(dt[0].CMID.ToString(), dt[0].Code);
                uiCtlCm.SetClearingMemberValue(mvdt[0].CMID.ToString(), mvdt[0].Code);
                uiDtpStartDate.SetCalendarValue(mvdt[0].EffectiveStartDate.ToString("dd-MMM-yyyy"));
                if (!mvdt[0].IsEffectiveEndDateNull())
                {
                    uiDtpEndDate.SetCalendarValue(mvdt[0].EffectiveEndDate.ToString("dd-MMM-yyyy"));
                }
                uiDtpUploadDate.SetCalendarValue(mvdt[0].UploadDate.ToString("dd-MMM-yyyy"));
                uiTxbMBDFund.Text = mvdt[0].MBDFund.ToString("#,##0.###");
                uiTxbMBDValue.Text = mvdt[0].MBDValue.ToString("#,##0.###");
                if (!mvdt[0].IsActionFlagNull())
                {
                    if (mvdt[0].ActionFlag == "I")
                    {
                        uiTxbAction.Text = "Insert";
                    }
                    else if (mvdt[0].ActionFlag == "U")
                    {
                        uiTxbAction.Text = "Update";
                    }
                    else if (mvdt[0].ActionFlag == "Delete")
                    {
                        uiTxbAction.Text = "Delete";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            mvdt.Dispose();
        }
    }


    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            TRAction.Visible = mp.IsChecker || mp.IsViewer;
            TRApproval.Visible = mp.IsChecker || mp.IsViewer;
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
                MBDData.MBDRow dr = MBD.SelectMBDByMBDID(Convert.ToDecimal(eID));

                uiDtpStartDate.SetDisabledCalendar(true);
                uiDtpStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
                uiDtpUploadDate.SetDisabledCalendar(true);
                uiDtpUploadDate.SetCalendarValue(dr.UploadDate.ToString("dd-MMM-yyyy"));
                uiCtlCm.SetDisabledClearingMember(true);
                uiTxbMBDFund.Enabled = false;
                uiTxbMBDValue.Enabled = false;
                
                if (dr.IsEffectiveEndDateNull())
                {
                    uiDtpEndDate.SetDisabledCalendar(true);
                    uiDtpEndDate.SetCalendarValue(null);
                }
                else
                {
                    uiDtpEndDate.SetDisabledCalendar(true);
                    uiDtpEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
                }

            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
