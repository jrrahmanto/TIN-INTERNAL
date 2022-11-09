using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_New_ViewDisiplinBOD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
        }
    }
    protected void uiDgDisiplinBOD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Response.Redirect("EntryDisiplinBOD.aspx");
        }
    }

    private void BindData()
    {
        BODDiciplineData.BODDiciplineDataTable dt = new BODDiciplineData.BODDiciplineDataTable();
        try
        {
            Nullable<decimal> cmid = null;
            if (CtlClearingMemberLookupBODDicipline.LookupTextBoxID != "")
            {
                cmid = Convert.ToDecimal(CtlClearingMemberLookupBODDicipline.LookupTextBoxID);
            }
            BODDicipline.FillBySearchCriteria(cmid, uiTxbName.Text, uiDdlApprovalStatus.SelectedValue);
            uiDgDisiplinBOD.DataSource = dt;
            uiDgDisiplinBOD.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dt.Dispose();
        }
    }


    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryDisiplinBOD.aspx");
    }
}
