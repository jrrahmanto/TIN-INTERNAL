using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class RiskManagement_EntryRiskProfile : System.Web.UI.Page
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
    }

    private string RowType
    {
        get { return ViewState["RowType"].ToString(); }
        set { ViewState["RowType"] = value; }
    }

    private string TempRiskType
    {
        get { return ViewState["TempRiskType"].ToString(); }
        set { ViewState["TempRiskType"] = value; }
    }

    private RiskProfileData.RiskProfileDetailDataTable dtRiskProfile
    {
        get { return (RiskProfileData.RiskProfileDetailDataTable)ViewState["dtRiskProfile"]; }
        set { ViewState["dtRiskProfile"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            dtRiskProfile = new RiskProfileData.RiskProfileDetailDataTable();

            FillRiskProfileDataGrid();

            if (eType == "add")
            {
                //skip
            }
            else if (eType == "edit")
            {
                CtlCalendarPickUpStartDate.DisabledCalendar = true;
                CtlClearingMemberLookup1.DisabledLookupButton = true;
                RiskProfileData.RiskProfileRow dr = RiskProfile.GetRiskProfileByRiskProfileID(eID);

                CtlClearingMemberLookup1.SetClearingMemberValue(dr.CMID.ToString(),
                    ClearingMember.GetClearingMemberCodeByClearingMemberID(dr.CMID));

                CtlCalendarPickUpStartDate.SetCalendarValue(dr.StartDate.ToString("dd-MMM-yyyy"));
                CtlCalendarPickUpEndDate.SetCalendarValue(dr.EndDate.ToString("dd-MMM-yyyy"));
            }
        }
        
    } 

    protected void uiBtnAddNewRow_Click(object sender, EventArgs e)
    {
        if (dtRiskProfile.Count == 0)
        {
            AddRow();
        }
        else
        {
            if (uiDgRiskProfile.EditIndex == -1)
            {
                AddRow();
            }  
        }              
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                RiskProfile.UpdateRiskProfile(decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID), DateTime.Parse(CtlCalendarPickUpStartDate.Text),
                   DateTime.Parse(CtlCalendarPickUpEndDate.Text), eID, User.Identity.Name, DateTime.Now, User.Identity.Name, DateTime.Now,
                   dtRiskProfile);

                Response.Redirect("ViewRiskProfile.aspx");
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
        Response.Redirect("ViewRiskProfile.aspx");
    }

    protected void uiDgRiskProfile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        if (RowType == "add")
        {
            dtRiskProfile.Rows[e.RowIndex].RejectChanges();
        }

        uiDgRiskProfile.EditIndex = -1;        

        BindViewStateToDataGrid();
    }

    protected void uiDgRiskProfile_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RowType = "edit";

        RiskProfileData.RiskProfileDetailRow dr = null;

        uiDgRiskProfile.EditIndex = e.NewEditIndex;

        dr = dtRiskProfile[e.NewEditIndex];
        TempRiskType = dr.RiskTypeID.ToString();

        BindViewStateToDataGrid();

        DisableDataGridRowEnabled(e.NewEditIndex);
    }

    protected void uiDgRiskProfile_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (IsValidDetailEntry(e.RowIndex) == true)
        {
            RiskProfileData.RiskProfileDetailRow dr = null;
            
            DropDownList riskType = (DropDownList)uiDgRiskProfile.Rows[e.RowIndex].FindControl("uiDdlRiskType");
            TextBox impact = (TextBox)uiDgRiskProfile.Rows[e.RowIndex].FindControl("uiTxtImpact");
            TextBox likelihood = (TextBox)uiDgRiskProfile.Rows[e.RowIndex].FindControl("uiTxtLikelihood");
                        
            dr = dtRiskProfile[e.RowIndex];
            dr.RiskProfileID = eID;
            dr.RiskTypeID = decimal.Parse(riskType.SelectedValue);
            dr.Impact = int.Parse(impact.Text);
            dr.Likelihood = int.Parse(likelihood.Text);

            uiDgRiskProfile.EditIndex = -1;

            BindViewStateToDataGrid();
        }
    }

    protected void uiDgRiskProfile_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        dtRiskProfile.Rows[e.RowIndex].Delete();
        dtRiskProfile.AcceptChanges();

        BindViewStateToDataGrid();
    }

    private void AddRow()
    {
        RowType = "add";
        
        //Reserve data
        RiskProfileData.RiskProfileDetailRow dr = dtRiskProfile.NewRiskProfileDetailRow();
        dr.RiskProfileID = eID;
        dr.RiskTypeID = 0;
        dr.Impact = 0;
        dr.Likelihood = 0;
        dtRiskProfile.AddRiskProfileDetailRow(dr);

        TempRiskType = "";

        int newEditIndex = uiDgRiskProfile.Rows.Count;
        uiDgRiskProfile.EditIndex = newEditIndex;

        BindViewStateToDataGrid();

        DisableDataGridRowEnabled(newEditIndex);
    }

    private void DisableDataGridRowEnabled(int rowIndex)
    {
        for (int ii = 0; ii < dtRiskProfile.Count; ii++)
        {
            if (ii != rowIndex)
            {
                uiDgRiskProfile.Rows[ii].Enabled = false;
            }
        }
    }

    private void FillRiskProfileDataGrid()
    {
        uiDgRiskProfile.DataSource = ObjectDataSourceRiskProfile;
        IEnumerable dv = (IEnumerable)ObjectDataSourceRiskProfile.Select();
        DataView dve = (DataView)dv;

        dtRiskProfile = (RiskProfileData.RiskProfileDetailDataTable)dve.Table;

        BindViewStateToDataGrid();
    }

    private void BindViewStateToDataGrid()
    {
        uiDgRiskProfile.DataSource = dtRiskProfile;
        uiDgRiskProfile.DataBind();
    }

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
                
        if (string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID))
        {
            uiBLError.Items.Add("Clearing member is required.");
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text))
        {
            uiBLError.Items.Add("Start date is required.");
        }
        if (string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            uiBLError.Items.Add("End date is required.");
        }
        if (!string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text) &&
            !string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
        {
            if (DateTime.Parse(CtlCalendarPickUpEndDate.Text) <
                DateTime.Parse(CtlCalendarPickUpStartDate.Text))
            {
                uiBLError.Items.Add("End date must greater than start date.");
            }
        }
        if (eType == "add")
        {
            if (!string.IsNullOrEmpty(CtlClearingMemberLookup1.LookupTextBoxID) &&
                !string.IsNullOrEmpty(CtlCalendarPickUpStartDate.Text) &&
                !string.IsNullOrEmpty(CtlCalendarPickUpEndDate.Text))
            {
                RiskProfileData.RiskProfileDataTable dt = RiskProfile.ValidateUpdateRiskProfile(
                    decimal.Parse(CtlClearingMemberLookup1.LookupTextBoxID),
                    DateTime.Parse(CtlCalendarPickUpStartDate.Text),
                    DateTime.Parse(CtlCalendarPickUpEndDate.Text));
                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("Period is invalid.");
                }
            }
        }
        
        

        if (dtRiskProfile.Count == 0)
        {
            uiBLError.Items.Add("Risk type is required.");
        }
        else
        {
            for (int ii = 0; ii < dtRiskProfile.Count ; ii++)
            {
                isValid = IsValidDetailEntry(ii);
                if (isValid == false)
                {
                    uiBLError.Items.Add("Risk type is invalid.");
                }
            }
        }

        if (uiBLError.Items.Count > 0)
        {
            uiBLError.Visible = true;
            isValid = false;
        }

        return isValid;
    }

    private bool IsValidDetailEntry(int rowIndex)
    {
        bool isValid = true;
        uiBLErrorDetail.Visible = false;
        uiBLErrorDetail.Items.Clear();

        DropDownList riskType = (DropDownList)uiDgRiskProfile.Rows[rowIndex].FindControl("uiDdlRiskType");
        TextBox impact = (TextBox)uiDgRiskProfile.Rows[rowIndex].FindControl("uiTxtImpact");
        TextBox likelihood = (TextBox)uiDgRiskProfile.Rows[rowIndex].FindControl("uiTxtLikelihood");

        if (riskType != null)
        {
            if (riskType.SelectedValue == "0")
            {
                uiBLErrorDetail.Items.Add(string.Format("Row {0} : Risk type is required.", rowIndex + 1));
            }
            else
            {
                foreach (RiskProfileData.RiskProfileDetailRow dr in dtRiskProfile)
                {
                    if (riskType.SelectedValue == dr.RiskTypeID.ToString() && riskType.SelectedValue != TempRiskType)
                    {
                        uiBLErrorDetail.Items.Add(string.Format("Row {0} : Risk type you choosen already exist.", rowIndex + 1));
                        break;
                    }
                }
            }
        }
        int iiOutResult = 0;
        if (impact != null)
        {

            if (int.TryParse(impact.Text, out iiOutResult) == true)
            {
                if (iiOutResult < 1 || iiOutResult > 5)
                {
                    uiBLErrorDetail.Items.Add(string.Format("Row {0} : Impact value must be between 1 and 5.", rowIndex + 1));
                }
            }
            else
            {
                uiBLErrorDetail.Items.Add(string.Format("Row {0} : Fill impact in numeric value between 1 and 5.", rowIndex + 1));
            }            
        }
        if (likelihood != null)
        {
            if (int.TryParse(likelihood.Text, out iiOutResult) == true)
            {
                if (iiOutResult < 1 || iiOutResult > 5)
                {
                    uiBLErrorDetail.Items.Add(string.Format("Row {0} : Likelihood value must be between 1 and 5.", rowIndex + 1));
                }
            }
            else
            {
                uiBLErrorDetail.Items.Add(string.Format("Row {0} : Fill Likelihood in numeric value between 1 and 5.", rowIndex + 1));
            }            
        }

        if (uiBLErrorDetail.Items.Count > 0)
        {
            isValid = false;
            uiBLErrorDetail.Visible = true;
        }

        return isValid;
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            RiskProfile.DeleteRiskProfileByRiskProfileID(eID);
            Response.Redirect("ViewRiskProfile.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Visible = true;
            uiBLError.Items.Add(ex.Message);
        }
    }
}
