using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_New_EntryRole : System.Web.UI.Page
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
        if (!Page.IsPostBack)
        {
            if (currentID != null)
            {
                //get data from group access page for update
                GroupData.ApplicationPageDataTable dt = new GroupData.ApplicationPageDataTable();
                dt.Clear();
                dt = Group.getApplicationPage(Convert.ToDecimal(currentID));
                uiDgEntryGroup.DataSource = dt;
                uiDgEntryGroup.DataBind();
                bindData();
            }
            else
            {
                //get data from group access page for new entry
                GroupData.ApplicationPageDataTable dt = new GroupData.ApplicationPageDataTable();
                dt.Clear();
                dt = Group.getInsertApplicationPage();
                uiDgEntryGroup.DataSource = dt;
                uiDgEntryGroup.DataBind();
            }
        }

        
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        List<string> checkedList = new List<string>();
        foreach (GridViewRow GridRow in uiDgEntryGroup.Rows)
        {
            CheckBox chkViewer = new CheckBox();
            chkViewer = ((CheckBox)GridRow.FindControl("uiChkViewer"));
            CheckBox chkMaker = new CheckBox();
            chkMaker = ((CheckBox)GridRow.FindControl("uiChkMaker"));
            CheckBox chkChecker = new CheckBox();
            chkChecker = ((CheckBox)GridRow.FindControl("uiChkChecker"));

            if (chkViewer.Checked)
            {
                checkedList.Add(GridRow.Cells[0].Text + "|" + "Viewer");
            }
            if (chkMaker.Checked)
            {
                checkedList.Add(GridRow.Cells[0].Text + "|" + "Maker");
            }
            if (chkChecker.Checked)
            {
                checkedList.Add(GridRow.Cells[0].Text + "|" + "Checker");
            }
        }

        try
        {
            if (!IsValidEntry())
            {
                if (currentID != null)
                {
                    Group.UpdateGroup(uiTxbGroup.Text, uiTxbDesc.Text, currentID, User.Identity.Name, checkedList);
                }
                else
                {
                    Group.InsertGroup(uiTxbGroup.Text, uiTxbDesc.Text, User.Identity.Name, checkedList);
                }
                Response.Redirect("ViewRole.aspx");
            }
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
        
    }

    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewRole.aspx");
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Group.DeleteGroup(currentID, User.Identity.Name);
            Response.Redirect("ViewRole.aspx");
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    #region "    Supporting Method    "


    private bool IsValidEntry()
    {
        bool isError = false;

        uiBlError.Visible = false;
        uiBlError.Items.Clear();

        // ---------- Validate required field ----------
        if (uiTxbGroup.Text == "")
        {
            uiBlError.Items.Add("Group name is required.");
        }
         
        // Show visibility of error message
        if (uiBlError.Items.Count > 0)
        {
            uiBlError.Visible = true;
            isError = true;
        }

        return isError;
    }

    // DisplayErrorMessage
    // Purpose      : Display error message based on exception
    // Parameter    : Exception
    // Return       : -
    private void DisplayErrorMessage(Exception ex)
    {
        uiBlError.Items.Clear();
        uiBlError.Items.Add(ex.Message);
        uiBlError.Visible = true;
    }

    private void SetControlAccessByMakerChecker()
    {
        MasterPage mp = (MasterPage)this.Master;

        bool pageMaker = mp.IsMaker;
        bool pageChecker = mp.IsChecker;
        bool pageViewer = mp.IsViewer;

        // Set control visibility
        uiBtnSave.Visible = pageMaker;
    }

    private void bindData()
    {
        GroupDataTableAdapters.GroupTableAdapter ta = new GroupDataTableAdapters.GroupTableAdapter();
        GroupData.GroupDataTable dt = new GroupData.GroupDataTable();

        try
        {  
            //get data from group
            ta.FillByGroupID(dt, Convert.ToDecimal(currentID));
            if (dt.Count > 0)
            {
                uiTxbDesc.Text = dt[0].Description;
                uiTxbGroup.Text = dt[0].GroupName;
            }
        }
        catch (Exception ex)
        {
            DisplayErrorMessage(ex);
        }
    }

    //private void getSOPage()
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        DisplayErrorMessage(ex);
    //        throw;
    //    }
    //}

    #endregion
}
