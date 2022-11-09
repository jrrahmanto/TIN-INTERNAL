using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUI_New_EntryDisiplinBOD : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void BindData()
    {
        try
        {
            BODDiciplineData.BODDiciplineDataTable dt = new BODDiciplineData.BODDiciplineDataTable();
            dt = BODDicipline.FillByBODDiciplineID(Convert.ToDecimal(currentID));
            if (dt.Count > 0)
            {

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (currentID == null)
        {
            try
            {
                BODDicipline.ProposedInsert(uiTxbDiciplineNo.Text, uiTxbDesc.Text, 
                                            User.Identity.Name, Convert.ToDecimal(uiDdlName.SelectedValue));
                Response.Redirect("ViewDisiplinBOD.aspx");
            }
            catch (Exception ex)
            {
                uiLblWarning.Text = ex.Message;
                uiLblWarning.Visible = true;
            }
        }
        else
        {
            try
            {
                BODDicipline.ProposedUpdate(uiTxbDiciplineNo.Text, uiTxbDesc.Text,
                                            User.Identity.Name, Convert.ToDecimal(uiDdlName.SelectedValue),
                                            Convert.ToDecimal(currentID));
                Response.Redirect("ViewDisiplinBOD.aspx");
            }
            catch (Exception ex)
            {
                uiLblWarning.Text = ex.Message;
                uiLblWarning.Visible = true;
            }
        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewDisiplinBOD.aspx");
    }
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                BODDicipline.ProposedDelete(uiTxbDiciplineNo.Text, uiTxbDesc.Text,
                                           User.Identity.Name, Convert.ToDecimal(uiDdlName.SelectedValue),
                                           Convert.ToDecimal(currentID));
                Response.Redirect("ViewDisiplinBOD.aspx");
            }
            catch (Exception ex)
            {
                uiLblWarning.Text = ex.Message;
                uiLblWarning.Visible = true;
            }
        }
    }
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                BODDicipline.Approved(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text, 
                                      uiTxbAction.Text, User.Identity.Name);
                Response.Redirect("ViewDisiplinBOD.aspx");
            }
            catch (Exception ex)
            {
                uiLblWarning.Text = ex.Message;
                uiLblWarning.Visible = true;
            }
        }
    }
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (currentID != null)
        {
            try
            {
                BODDicipline.Reject(Convert.ToDecimal(currentID), uiTxbApprovalDesc.Text,
                                      uiTxbAction.Text, User.Identity.Name);
                Response.Redirect("ViewDisiplinBOD.aspx");
            }
            catch (Exception ex)
            {
                uiLblWarning.Text = ex.Message;
                uiLblWarning.Visible = true;
            }
        }
    }
}
