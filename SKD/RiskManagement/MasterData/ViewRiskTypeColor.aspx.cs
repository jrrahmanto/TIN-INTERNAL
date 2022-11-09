using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class WebUI_New_ViewRiskTypeColor : System.Web.UI.Page
{
    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SortOrder"].ToString();
            }
        }
        set { ViewState["SortOrder"] = value; }
    }

    private RiskProfileData.RiskColorDataTable dtRiskColor
    {
        get { return (RiskProfileData.RiskColorDataTable)ViewState["dtRiskColor"]; }
        set { ViewState["dtRiskColor"] = value; }
    }

    private string RowType
    {
        get { return ViewState["RowType"].ToString(); }
        set { ViewState["RowType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;

        //SetAccessPage();
        //FillRiskColorDataGrid();
        if (!Page.IsPostBack)
        {
            FillRiskColorDataGrid();

            dtRiskColor = new RiskProfileData.RiskColorDataTable();

        }
    }

    protected void uiBtnSearch_Click(object sender, EventArgs e)
    {
        FillRiskColorDataGrid();
    }

    protected void uiBtnAdd_Click(object sender, EventArgs e)
    {
        FillRiskColorDataGrid();

        RowType = "add";

        RiskProfileData.RiskColorRow dr = dtRiskColor.NewRiskColorRow();
        dr.Impact = 0;
        dr.Likelihood = 0;
        dr.ApprovalStatus = "";
        dr.RiskColor = "";
        dr.CreatedBy = "";
        dr.CreatedDate = DateTime.Now;
        dr.LastUpdatedBy = "";
        dr.LastUpdatedDate = DateTime.Now;
        dtRiskColor.AddRiskColorRow(dr);

        int newEditIndex = uiDgRiskColor.Rows.Count;
        uiDgRiskColor.EditIndex = newEditIndex;

        BindViewStateToDataGrid();
    }

    protected void uiDgRiskColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgRiskColor.PageIndex = e.NewPageIndex;
        FillRiskColorDataGrid();
    }

    protected void uiDgRiskColor_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (string.IsNullOrEmpty(SortOrder))
        {
            SortOrder = e.SortExpression + " " + "DESC";
        }
        else
        {
            string[] arrSortOrder = SortOrder.Split(" ".ToCharArray()[0]);
            if (arrSortOrder[1] == "ASC")
            {
                SortOrder = e.SortExpression + " " + "DESC";
            }
            else if (arrSortOrder[1] == "DESC")
            {
                SortOrder = e.SortExpression + " " + "ASC";
            }
        }

        FillRiskColorDataGrid();
    }

    protected void uiDgRiskColor_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FillRiskColorDataGrid();

        RowType = "edit";

        uiDgRiskColor.EditIndex = e.NewEditIndex;
        
        BindViewStateToDataGrid();

        TextBox impact = (TextBox)uiDgRiskColor.Rows[e.NewEditIndex].FindControl("uiTxtImpact");
        TextBox likelihood = (TextBox)uiDgRiskColor.Rows[e.NewEditIndex].FindControl("uiTxtLikelihood");
        TextBox riskColor = (TextBox)uiDgRiskColor.Rows[e.NewEditIndex].FindControl("uiTxtRiskColor");
        Button riskColorButton = (Button)uiDgRiskColor.Rows[e.NewEditIndex].FindControl("uiBtnRiskColor");
        TextBox approvalDesc = (TextBox)uiDgRiskColor.Rows[e.NewEditIndex].FindControl("uiTxtApprovalDesc");
        if (impact != null && likelihood != null)
        {
            impact.Enabled = false;
            likelihood.Enabled = false;            
        }
        riskColor.Text = riskColor.Text.Replace("#", "");
        riskColor.Attributes.Add("readonly", "readonly");
        
        MasterPage mp = (MasterPage)this.Master;
        if (mp.IsChecker)
        {
            riskColorButton.Enabled = false;            
        }
        else if (mp.IsMaker)
        {
            approvalDesc.Enabled = false;
        }
    }

    protected void uiDgRiskColor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        if (RowType == "add")
        {
            dtRiskColor.Rows[e.RowIndex].RejectChanges();
        }
        
        uiDgRiskColor.EditIndex = -1;
        BindViewStateToDataGrid();
    }

    protected void uiDgRiskColor_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (IsValidEntry(e.RowIndex, "") == true)
        {
            try
            {
                TextBox impact = (TextBox)uiDgRiskColor.Rows[e.RowIndex].FindControl("uiTxtImpact");
                TextBox likelihood = (TextBox)uiDgRiskColor.Rows[e.RowIndex].FindControl("uiTxtLikelihood");
                TextBox riskColor = (TextBox)uiDgRiskColor.Rows[e.RowIndex].FindControl("uiTxtRiskColor");
                TextBox approvalDesc = (TextBox)uiDgRiskColor.Rows[e.RowIndex].FindControl("uiTxtApprovalDesc");

                if (RowType == "add")
                {
                    RiskProfile.InsertRiskColor(int.Parse(impact.Text), int.Parse(likelihood.Text),
                        "A", "#" + riskColor.Text, User.Identity.Name, DateTime.Now,
                        User.Identity.Name, DateTime.Now, null);
                }
                else if (RowType == "edit")
                {
                    RiskProfile.UpdateRiskColor("#" + riskColor.Text, User.Identity.Name,
                            DateTime.Now, approvalDesc.Text, "A", int.Parse(impact.Text),
                            int.Parse(likelihood.Text));
                }

                uiDgRiskColor.EditIndex = -1;

                FillRiskColorDataGrid();
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            } 
        }
    }

    protected void uiDgRiskColor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "reject")
        {
            uiBLError.Visible = false;

            try
            {
                Label impact = (Label)uiDgRiskColor.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uiLblImpact");
                Label likelihood = (Label)uiDgRiskColor.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uiLblLikelihood");
                Label riskColor = (Label)uiDgRiskColor.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uiLblRiskColor");
                Label approvalStatus = (Label)uiDgRiskColor.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("uiLblApprovalStatus");

                RiskProfile.DeleteRiskColor(int.Parse(impact.Text), int.Parse(likelihood.Text), "A");
                FillRiskColorDataGrid();
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }  

            //if (IsValidEntry(int.Parse(e.CommandArgument.ToString()), e.CommandName))
            //{
            //}   
        }
    }

    protected void uiDgRiskColor_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        MasterPage mp = (MasterPage)this.Master;
        if (mp.IsChecker)
        {
            CommandField cf = (CommandField)uiDgRiskColor.Columns[0];
            cf.EditText = "Approve";
        }
        Label approvalStatus = (Label)e.Row.FindControl("uiLblApprovalStatus");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (approvalStatus.Text)
            {
                case "A":
                    approvalStatus.Text = "Approved";
                    break;
                case "P":
                    approvalStatus.Text = "Proposed";
                    break;
                case "R":
                    approvalStatus.Text = "Rejected";
                    break;
                default :
                    approvalStatus.Text = approvalStatus.Text;
                    break;
            }
        }
    }

    private bool IsValidEntry(int rowIndex, string commandName)
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();

        TextBox impact = (TextBox)uiDgRiskColor.Rows[rowIndex].FindControl("uiTxtImpact");
        TextBox likelihood = (TextBox)uiDgRiskColor.Rows[rowIndex].FindControl("uiTxtLikelihood");
        TextBox riskColor = (TextBox)uiDgRiskColor.Rows[rowIndex].FindControl("uiTxtRiskColor");
        TextBox approvalDesc = (TextBox)uiDgRiskColor.Rows[rowIndex].FindControl("uiTxtApprovalDesc");
        Label approvalStatus = (Label)uiDgRiskColor.Rows[rowIndex].FindControl("uiLblApprovalStatus");

        MasterPage mp = (MasterPage)this.Master;
        int iiOutResult = 0;
        if (int.TryParse(impact.Text, out iiOutResult) == true)
        {
            if (iiOutResult < 1 || iiOutResult > 5)
            {
                uiBLError.Items.Add(string.Format("Row {0} : Impact value must be between 1 and 5.", rowIndex + 1));
            }
        }
        else
        {
            uiBLError.Items.Add(string.Format("Row {0} : Fill impact in numeric value between 1 and 5.", rowIndex + 1));
        }
        if (int.TryParse(likelihood.Text, out iiOutResult) == true)
        {
            if (iiOutResult < 1 || iiOutResult > 5)
            {
                uiBLError.Items.Add(string.Format("Row {0} : Likelihood value must be between 1 and 5.", rowIndex + 1));
            }
        }
        else
        {
            uiBLError.Items.Add(string.Format("Row {0} : Fill Likelihood in numeric value between 1 and 5.", rowIndex + 1));
        }
        //if (mp.IsMaker)
        //{            
        //    if (approvalStatus.Text == "A" || approvalStatus.Text == "R" ||
        //        approvalStatus.Text == "")
        //    {
        //        if (int.TryParse(impact.Text, out iiOutResult) == true)
        //        {
        //            if (iiOutResult < 1 || iiOutResult > 5)
        //            {
        //                uiBLError.Items.Add(string.Format("Row {0} : Impact value must be between 1 and 5.", rowIndex + 1));
        //            }
        //        }
        //        else
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : Fill impact in numeric value between 1 and 5.", rowIndex + 1));
        //        }
        //        if (int.TryParse(likelihood.Text, out iiOutResult) == true)
        //        {
        //            if (iiOutResult < 1 || iiOutResult > 5)
        //            {
        //                uiBLError.Items.Add(string.Format("Row {0} : Likelihood value must be between 1 and 5.", rowIndex + 1));
        //            }
        //        }
        //        else
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : Fill Likelihood in numeric value between 1 and 5.", rowIndex + 1));
        //        }
        //        if (approvalStatus.Text == "")
        //        {
        //            int iiOutImpactResult = 0;
        //            int iiOutLikelihoodResult = 0;
        //            if (int.TryParse(impact.Text, out iiOutImpactResult) &&
        //                int.TryParse(likelihood.Text, out iiOutLikelihoodResult) == true)
        //            {
        //                if (!(iiOutImpactResult < 1 || iiOutImpactResult > 5) &&
        //                    !(iiOutLikelihoodResult < 1 || iiOutLikelihoodResult > 5))
        //                {
        //                    RiskProfileData.RiskColorDataTable dtRiskColor = RiskProfile.GetRiskColorByImpactAndLikelihood(iiOutImpactResult, iiOutLikelihoodResult);
        //                    if (dtRiskColor.Count > 0)
        //                    {
        //                        uiBLError.Items.Add(string.Format("Row {0} : Impact and likelihood already exist.", rowIndex + 1));
        //                    }
        //                }
        //            }
        //        }
                
        //        if (string.IsNullOrEmpty(riskColor.Text))
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : Risk color is required.", rowIndex + 1));
        //        }
        //    }
        //    else if (approvalStatus.Text == "P")
        //    {
        //        uiBLError.Items.Add(string.Format("Row {0} : This record still waiting for approval", rowIndex + 1));
        //    }
        //}
        //else if (mp.IsChecker)
        //{
        //    if (commandName == "reject")
        //    {
        //        if (approvalStatus.Text == "R")
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : This record has been rejected", rowIndex + 1));
        //        }
        //        else if (approvalStatus.Text == "A")
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : This record has been approved", rowIndex + 1));
        //        }
        //    }
        //    else
        //    {
        //        if (approvalStatus.Text == "R")
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : This record has been rejected", rowIndex + 1));
        //        }
        //        else if (approvalStatus.Text == "A")
        //        {
        //            uiBLError.Items.Add(string.Format("Row {0} : This record has been approved", rowIndex + 1));
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(approvalDesc.Text))
        //            {
        //                uiBLError.Items.Add(string.Format("Row {0} : Approval description is required.", rowIndex + 1));
        //            }
        //        }
        //    }            
        //}

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void FillRiskColorDataGrid()
    {
        uiDgRiskColor.DataSource = ObjectDataSourceRiskColor;
        IEnumerable dv = (IEnumerable)ObjectDataSourceRiskColor.Select();
        DataView dve = (DataView)dv;

        if (!string.IsNullOrEmpty(SortOrder))
        {
            dve.Sort = SortOrder;
        }

        dtRiskColor = (RiskProfileData.RiskColorDataTable)dve.Table;
        
        uiDgRiskColor.DataSource = dve;
        uiDgRiskColor.DataBind();
    }

    private void BindViewStateToDataGrid()
    {
        uiDgRiskColor.DataSource = dtRiskColor;
        uiDgRiskColor.DataBind();
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnAdd.Visible = mp.IsMaker;
        uiDgRiskColor.Columns[0].Visible = mp.IsMaker || mp.IsChecker;
        uiDgRiskColor.Columns[1].Visible = mp.IsChecker;
        uiDgRiskColor.Columns[6].Visible = mp.IsChecker;
    }

    protected void uiDgRiskColor_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
}
