using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

public partial class ClearingAndSettlement_TransferPosition_EntryTransferSPAApproval : System.Web.UI.Page
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

    private string TransferType
    {
        get { return (string)ViewState["TransferType"]; }
        set { ViewState["TransferType"] = value; }
    }

    private decimal TransferNo
    {
        get { return (decimal)ViewState["TransferNo"]; }
        set { ViewState["TransferNo"] = value; }
    }

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

    public string Menu
    {
        get
        {
            if (string.IsNullOrEmpty(Request.QueryString["menu"]))
            {
                return "";
            }
            else
            {
                return Request.QueryString["menu"];
            }
        }
    }

    private TransferSpaData.TransferSpaRequestDataTable dtTransferRequest
    {
        get { return (TransferSpaData.TransferSpaRequestDataTable)ViewState["dtTransferRequest"]; }
        set { ViewState["dtTransferRequest"] = value; }
    }

    private StringCollection scTransferID
    {
        get { return (StringCollection)ViewState["scTransferID"]; }
        set { ViewState["scTransferID"] = value; }
    }

    private const string TRANSFER_TYPE_AA = "AA";
    private const string TRANSFER_TYPE_MM = "MM";

    protected void Page_Load(object sender, EventArgs e)
    {
        uiBLError.Visible = false;
        SetAccessPage();

        if (!Page.IsPostBack)
        {
            if (eType == "edit")
            {
                BindDataToForm();

                FillTransferRequestDataGrid();

                SetEnabledControls(false);
                SetVisibleControls(false);
            }
        }
        
    }
    

    protected void uiDgContractPosition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label investor = (Label)e.Row.FindControl("uiLblInvestor");
            Label commodity = (Label)e.Row.FindControl("uiLblCommodity");
            //Change Investor ID to Investor Code
            investor.Text = Investor.GetNameInvestorByInvestorID(int.Parse(investor.Text));

            //Change Commodity ID to Commodity Name
            commodity.Text = Commodity.GetCommodityNameByCommID(int.Parse(commodity.Text));
        }
    }

    protected void uiDgTransferRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Change Investor ID to Investor Code
            e.Row.Cells[3].Text = Investor.GetNameInvestorByInvestorID(int.Parse(e.Row.Cells[3].Text));
            e.Row.Cells[4].Text = Contract.GetContractDescByContractID(int.Parse(e.Row.Cells[4].Text));
            e.Row.Cells[5].Text = (e.Row.Cells[5].Text == "CF"? "Carry Forward" : (e.Row.Cells[5].Text == "NT" ? "New Trade" : ""));
            e.Row.Cells[6].Text = (e.Row.Cells[6].Text == "B" ? "Buy" : (e.Row.Cells[6].Text == "S" ? "Sell" : ""));
        }
    }

    protected void uiDgTransferRequest_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        dtTransferRequest.Rows[e.NewSelectedIndex].Delete();

        BindViewStateToDataGrid();
    }


    /// <summary>
    /// Approve TransferSPA Request
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("approve") == true)
        {
            try
            {
                TransferSpa.ApproveRejectTranferSpa(eID, TransferType, "A", uiTxtDescription.Text, User.Identity.Name);

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTransferSpaApproval.aspx");
                }
                else
                {
                    Response.Redirect("ViewTransferSpaApproval.aspx?menu=hide");
                }  
            }
            catch (Exception ex)
            {
                uiBLError.Visible = true;
                uiBLError.Items.Add(ex.Message);
            }
        }
    }

    /// <summary>
    /// Reject TransferSPA Request
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry("reject") == true)
        {
            try
            {
                TransferSpa.ApproveRejectTranferSpa(eID, TransferType, "R", uiTxtDescription.Text, User.Identity.Name);

                if (string.IsNullOrEmpty(Menu))
                {
                    Response.Redirect("ViewTransferSpaApproval.aspx");
                }
                else
                {
                    Response.Redirect("ViewTransferSpaApproval.aspx?menu=hide");
                }  
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
        if (string.IsNullOrEmpty(Menu))
        {
            Response.Redirect("ViewTransferSpaApproval.aspx");
        }
        else
        {
            Response.Redirect("ViewTransferSpaApproval.aspx?menu=hide");
        }  
    }
    
    private bool IsValidEntry(string command)
    {
        bool isValid = true;
        uiBLError.Items.Clear();
        uiBLError.Visible = false;

        IEnumerable dv = (IEnumerable)ObjectDataSourceTransferPosition.Select();
        Object[] obj = (Object[])dv;
        TransferSpaData.TransferSpaPositionRow drTransferSpaPosition = (TransferSpaData.TransferSpaPositionRow)obj[0];
        if (command == "approve")
        {
            if (drTransferSpaPosition.ApprovalStatus == "A")
            {
                uiBLError.Items.Add("This record has been approved.");
            }
            if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }
        if (command == "reject")
        {
            if (drTransferSpaPosition.ApprovalStatus == "R")
            {
                uiBLError.Items.Add("This record has been rejected.");
            }
            if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
            {
                uiBLError.Items.Add("Reject description is required.");
            }
        }       

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void FillTransferRequestDataGrid()
    {
        uiDgTransferRequest.DataSource = ObjectDataSourceTransferRequest;
        IEnumerable dv = (IEnumerable)ObjectDataSourceTransferRequest.Select();
        DataView dve = (DataView)dv;

        dtTransferRequest = (TransferSpaData.TransferSpaRequestDataTable)dve.Table;

        BindViewStateToDataGrid();
    }

    private void BindViewStateToDataGrid()
    {
        uiDgTransferRequest.DataSource = dtTransferRequest;
        uiDgTransferRequest.DataBind();

    }

    private void BindDataToForm()
    {
        IEnumerable dv = (IEnumerable)ObjectDataSourceTransferPosition.Select();
        Object[] obj = (Object[])dv;
        TransferSpaData.TransferSpaPositionRow drTransferSpaPosition = (TransferSpaData.TransferSpaPositionRow)obj[0];
        TransferNo = drTransferSpaPosition.TransferNo;
        TransferType = drTransferSpaPosition.TransferType;
        CtlClearingMemberLookupSource.SetClearingMemberValue(drTransferSpaPosition.SourceCMID.ToString()
                    , ClearingMember.GetClearingMemberCodeByClearingMemberID(drTransferSpaPosition.SourceCMID));
        CtlClearingMemberLookupDest.SetClearingMemberValue(drTransferSpaPosition.DestCMID.ToString()
                    , ClearingMember.GetClearingMemberCodeByClearingMemberID(drTransferSpaPosition.DestCMID));
        if (!drTransferSpaPosition.IsDescriptionNull())
        {
            uiTxtDescription.Text = drTransferSpaPosition.Description;
        }
    }

    private void SetEnabledControls(bool b)
    {
        CtlClearingMemberLookupSource.DisabledLookupButton = !b;
        CtlClearingMemberLookupDest.DisabledLookupButton = !b;
        uiTxtDescription.Enabled = b;

    }

    private void SetVisibleControls(bool b)
    {
        uiDgTransferRequest.Columns[0].Visible = b;
    }

    private void SetAccessPage()
    {
        MasterPage mp = (MasterPage)this.Master;
        uiBtnApprove.Visible = mp.IsChecker;
        uiBtnReject.Visible = mp.IsChecker;
    }


    protected void uiDgTransferRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        uiDgTransferRequest.PageIndex = e.NewPageIndex;
        BindViewStateToDataGrid();
    }

    //protected void uiBtnSave_Click(object sender, EventArgs e)
    //{
    //    if (IsValidEntry("") == true)
    //    {
    //        try
    //        {
    //            Transfer.UpdateTransferPosition(eID, 0, "P", DateTime.Now, TransferType, decimal.Parse(CtlClearingMemberLookupSource.LookupTextBoxID),
    //                decimal.Parse(CtlClearingMemberLookupDest.LookupTextBoxID), uiTxtDescription.Text, User.Identity.Name,
    //                DateTime.Now, User.Identity.Name, DateTime.Now, null, dtTransferRequest);

    //            if (string.IsNullOrEmpty(Menu))
    //            {
    //                Response.Redirect("ViewTransferSpaApproval.aspx");
    //            }
    //            else
    //            {
    //                Response.Redirect("ViewTransferSpaApproval.aspx?menu=hide");
    //            }      
    //        }
    //        catch (Exception ex)
    //        {
    //            uiBLError.Visible = true;
    //            uiBLError.Items.Add(ex.Message);
    //        }
    //    }
    //}
}
