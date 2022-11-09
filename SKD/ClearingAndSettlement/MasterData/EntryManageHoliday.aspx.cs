using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class WebUI_ClearingAndSettlement_EntryManageHoliday : System.Web.UI.Page
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
       
        uiBLError.Visible = false;

        if (!Page.IsPostBack)
        {
            if (eType == "add")
            {
                uiBtnDelete.Visible = false;
            }
            else if (eType == "edit")
            {
                bindData();
            }

            if (uiDdlHolidayType.SelectedValue == "G")
            {
                trProduct.Visible = false;
                trExchange.Visible = false;
            }
        }

        SetAccessPage();
    }
    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {

            if (eID.ToString() != "")
            {
                // Guard for editing proposed record
                HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("This record is not allowed to be deleted. Please wait for checker approval.");

                if (uiDdlHolidayType.SelectedValue == "G")
                {
                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                   uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                         null, null, Convert.ToDecimal(eID), "D", User.Identity.Name);
                }
                else if (uiDdlHolidayType.SelectedValue == "E")
                {
                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                   uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                         Convert.ToDecimal(uiDdlExchange.SelectedValue),
                                    null, Convert.ToDecimal(eID), "D", User.Identity.Name);
                }
                else if (uiDdlHolidayType.SelectedValue == "P")
                {
                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text),
                                    uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                          null,
                                          Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID),
                                          Convert.ToDecimal(eID), "D", User.Identity.Name);
                }
                
            }
            Response.Redirect("ViewManageHoliday.aspx");

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnApprove_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (eID.ToString() != "")
                {
                    Holiday.ApproveHolidayId(Convert.ToDecimal(eID), User.Identity.Name, uiTxbApporvalDesc.Text);
                }
                Response.Redirect("ViewManageHoliday.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;
            }
        }
    }

    protected void uiBtnReject_Click(object sender, EventArgs e)
    {
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (eID.ToString() != "")
                {
                    if (uiDdlHolidayType.SelectedValue == "G")
                    {
                        Holiday.RejectProposedHolidayDate(Convert.ToDecimal(eID), User.Identity.Name,
                            null, null);
                    }
                    else if (uiDdlHolidayType.SelectedValue == "E")
                    {
                        Holiday.RejectProposedHolidayDate(Convert.ToDecimal(eID), User.Identity.Name,
                           Convert.ToDecimal(uiDdlExchange.SelectedValue), null);
                    }
                    else if (uiDdlHolidayType.SelectedValue == "P")
                    {
                        Holiday.RejectProposedHolidayDate(Convert.ToDecimal(eID), User.Identity.Name,
                            null, Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID));
                    }
                }
                Response.Redirect("ViewManageHoliday.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }


    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewManageHoliday.aspx");
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        string actionFlag = "I";
        try
        {
            if (eID != 0)
            {

                // Guard for editing proposed record
                HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");
                actionFlag = "U";
            }
            if (actionFlag == "I")
            {
                if (uiDdlHolidayType.SelectedValue == "G")
                    {

                        //validation for global if already exist
                        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                        dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                uiDdlHolidayType.SelectedValue, null, null);

                        if (dt.Count > 0)
                        {
                            uiBLError.Items.Add("holiday date is already exist");
                            uiBLError.Visible = true;
                            return;
                        }

                        Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                       uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                             null, null, Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                    }
                else if (uiDdlHolidayType.SelectedValue == "E")
                    {
                        //validation for exchange if already exist
                        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                        dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                uiDdlHolidayType.SelectedValue, Convert.ToDecimal(uiDdlExchange.SelectedValue), null);
                        if (dt.Count > 0)
                        {
                            uiBLError.Items.Add("holiday date is already exist");
                            uiBLError.Visible = true;
                            return;
                        }

                        Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                       uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                             Convert.ToDecimal(uiDdlExchange.SelectedValue),
                                        null, Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                    }
                else if (uiDdlHolidayType.SelectedValue == "P")
                    {
                        //validation for exchange if already exist
                        HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                        dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                uiDdlHolidayType.SelectedValue, null, Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID));
                        if (dt.Count > 0)
                        {
                            uiBLError.Items.Add("holiday date is already exist");
                            uiBLError.Visible = true;
                            return;
                        }

                        Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text),
                                        uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                              null,
                                              Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID),
                                              Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                    }
                
            }
            else if (actionFlag == "U")
            {
                if (uiDdlHolidayType.SelectedValue == "G")
                {
                    //validation for global if already exist
                    HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                    dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                            uiDdlHolidayType.SelectedValue, null, null);

                    if (dt.Count > 0)
                    {
                        uiBLError.Items.Add("holiday date is already exist");
                        uiBLError.Visible = true;
                        return;
                    }

                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                   uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                         null, null, Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                }
                else if (uiDdlHolidayType.SelectedValue == "E")
                {
                    //validation for exchange if already exist
                    HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                    dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                            uiDdlHolidayType.SelectedValue, Convert.ToDecimal(uiDdlExchange.SelectedValue), null);
                    if (dt.Count > 0)
                    {
                        uiBLError.Items.Add("holiday date is already exist");
                        uiBLError.Visible = true;
                        return;
                    }

                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                                   uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                         Convert.ToDecimal(uiDdlExchange.SelectedValue),
                                    null, Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                }
                else if (uiDdlHolidayType.SelectedValue == "P")
                {
                    //validation for exchange if already exist
                    HolidayData.HolidayDataTable dt = new HolidayData.HolidayDataTable();
                    dt = Holiday.SelectHolidayByHolidayDateAndHolidayType(Convert.ToDateTime(CtlCalendarPickUp1.Text).Date,
                            uiDdlHolidayType.SelectedValue, null, Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID));
                    if (dt.Count > 0)
                    {
                        uiBLError.Items.Add("holiday date is already exist");
                        uiBLError.Visible = true;
                        return;
                    }

                    Holiday.ProposeHolidayDate(Convert.ToDateTime(CtlCalendarPickUp1.Text),
                                    uiTxtDescription.Text, uiDdlHolidayType.SelectedValue,
                                          null,
                                          Convert.ToDecimal(CtlCommodityLookup1.LookupTextBoxID),
                                          Convert.ToDecimal(eID), actionFlag, User.Identity.Name);
                }
            }
            
           

            Response.Redirect("ViewManageHoliday.aspx");

        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    
    }

    #region SupportingMethod

    private bool IsValidEntry()
    {
        bool isValid = true;
        uiBLError.Visible = false;
        uiBLError.Items.Clear();
        MasterPage mp = (MasterPage)this.Master;

        if (mp.IsChecker)
        {
            if (string.IsNullOrEmpty(uiTxbApporvalDesc.Text))
            {
                uiBLError.Items.Add("Approval description is required.");
            }
        }

        if(mp.IsMaker)
        {
            if (string.IsNullOrEmpty(CtlCalendarPickUp1.Text))
            {
                uiBLError.Items.Add("Holiday date is required.");
            }
        }

        if (uiBLError.Items.Count > 0)
        {
            isValid = false;
            uiBLError.Visible = true;
        }

        return isValid;
    }

    private void bindData()
    {
        HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));
  

            CtlCalendarPickUp1.SetCalendarValue(dr.HolidayDate.ToString("dd-MMM-yyyy"));
            if (dr.IsDescriptionNull())
            {
                uiTxtDescription.Text = "";
            }
            else
            {
                uiTxtDescription.Text = dr.Description;
            }
            uiDdlHolidayType.SelectedValue = dr.HolidayType;
            if (uiDdlHolidayType.SelectedValue == "G")
            {
                trExchange.Visible = false;
                trProduct.Visible = false;
            }
            else if (uiDdlHolidayType.SelectedValue == "E")
            {
                trProduct.Visible = false;
                uiDdlExchange.SelectedValue = dr.ExchangeId.ToString();
                uiDdlExchange.Enabled = true;
            }
            else if (uiDdlHolidayType.SelectedValue == "P")
            {
                trExchange.Visible = false;
                uiDdlExchange.Enabled = false;
                //CtlCommodityLookup1.SetDisabledCommodity(true);
                CtlCommodityLookup1.SetCommodityValue(dr.CommodityID.ToString(),dr.CommodityCode.ToString());
                trProduct.Visible = true;
            }

            string actionDesc = "";
            if (!dr.IsActionFlagNull())
            {

                if (dr.ActionFlag == "I")
                {
                    actionDesc = "Insert";
                }
                else if (dr.ActionFlag == "U")
                {
                    actionDesc = "Update";
                }
                else if (dr.ActionFlag == "D")
                {
                    actionDesc = "Delete";
                }
            }
            uiTxbAction.Text = actionDesc;
        
    }

    protected void uiDdlHolidayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (uiDdlHolidayType.SelectedValue == "G")
        {
            trProduct.Visible = false;
            trExchange.Visible = false;
        }
        else if (uiDdlHolidayType.SelectedValue == "E")
        {
            trProduct.Visible = false;
            trExchange.Visible = true;
            uiDdlExchange.Enabled = true;
        }
        else if (uiDdlHolidayType.SelectedValue == "P")
        {
            trProduct.Visible = true;
            trExchange.Visible = false;
            uiDdlExchange.Enabled = false;
        }
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

        //// set disabled for other controls other than approval description, for checker
        if (mp.IsChecker)
        {
            HolidayData.HolidayRow dr = Holiday.SelectHolidayByHolidayDate(Convert.ToDecimal(eID));

            CtlCalendarPickUp1.SetDisabledCalendar(true);
            uiTxtDescription.Enabled = false;
            uiTxtDescription.ReadOnly = true;
            uiDdlHolidayType.Enabled = false;
            

            if (dr.HolidayType == "G")
            {
                CtlCalendarPickUp1.SetDisabledCalendar(true);
                uiDdlExchange.Enabled = false;
                trProduct.Visible = false;
            }

            if (dr.HolidayType == "E")
            {
                CtlCalendarPickUp1.SetDisabledCalendar(true);
                trProduct.Visible = false;
                trExchange.Visible = true;
                uiDdlExchange.Enabled = false;
                uiDdlExchange.SelectedValue = dr.ExchangeId.ToString();
            }

            if (dr.HolidayType == "P")
            {
                CtlCalendarPickUp1.SetDisabledCalendar(true);
                trExchange.Visible = false;
                CtlCommodityLookup1.SetDisabledCommodity(true);
                CtlCommodityLookup1.SetCommodityValue(dr.CommodityID.ToString(), dr.CommodityCode.ToString());
                trProduct.Visible = true;
            }
        }
    }


    #endregion
}
