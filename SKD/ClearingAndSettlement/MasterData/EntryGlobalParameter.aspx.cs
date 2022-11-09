using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ClearingAndSettlement_MasterData_EntryGlobalParameter : System.Web.UI.Page
{
    private string eType
    { 
        get{ return Request.QueryString["eType"].ToString(); }
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
        try
        {
            uiBLError.Visible = false;
            CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);

            if (!Page.IsPostBack)
            {
                if (eType == "add")
                {
                    //uiBtnDelete.Visible = false;
                }
                else if (eType == "edit")
                {
                    uiTxtParamCode.Enabled = false;
                    CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                    bindData();
                }
            }

            SetAccessPage();
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }


    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewGlobalParamater.aspx");
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
                    // Guard for editing proposed record
                    ParameterData.ParameterGlobalRow dr = Parameter.SelectParameterByParameterID(Convert.ToDecimal(eID));
                    if (dr.ApprovalStatus != "A") throw new ApplicationException("Can not edit pending approval record.");

                    //guard for number record
                    ParameterDataTableAdapters.ParameterGlobalTableAdapter ta = new ParameterDataTableAdapters.ParameterGlobalTableAdapter();
                    ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();
                    decimal NumberRecord = Convert.ToDecimal(ta.GetNumberRecordBeforeStartDate(uiTxtParamCode.Text, DateTime.Parse(CtlCalendarEffectiveStartDate.Text), eID));
                    if (NumberRecord > 0) throw new ApplicationException("Can not set start date less than other approved records.");
                    actionFlag = "U";
                }
            
                if (string.IsNullOrEmpty(CtlCalendarEffectiveEndDate.Text))
                {
                    if (string.IsNullOrEmpty(CtlCalendarDateValue.Text))
                    {
                        if (string.IsNullOrEmpty(uiTxtNumericValue.Text))
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, null, User.Identity.Name,
                                                         DateTime.Now, null, uiTxtStringValue.Text.ToString(),
                                                         User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), null, DateTime.Now,
                                                         uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                        else
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, Convert.ToDecimal(uiTxtNumericValue.Text), User.Identity.Name,
                                                          DateTime.Now, null, uiTxtStringValue.Text.ToString(),
                                                          User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), null, DateTime.Now,
                                                          uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(uiTxtNumericValue.Text))
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, null, User.Identity.Name,
                                                            DateTime.Now, Convert.ToDateTime(CtlCalendarDateValue.Text), uiTxtStringValue.Text,
                                                            User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), null, DateTime.Now,
                                                            uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                        else
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, decimal.Parse(uiTxtNumericValue.Text), User.Identity.Name,
                                                            DateTime.Now, Convert.ToDateTime(CtlCalendarDateValue.Text), uiTxtStringValue.Text,
                                                            User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), null, DateTime.Now,
                                                            uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(CtlCalendarDateValue.Text))
                    {
                        if (string.IsNullOrEmpty(uiTxtNumericValue.Text))
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, null, User.Identity.Name,
                                                           DateTime.Now, null, uiTxtStringValue.Text,
                                                           User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), Convert.ToDateTime(CtlCalendarEffectiveEndDate.Text), DateTime.Now,
                                                           uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                        else
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, decimal.Parse(uiTxtNumericValue.Text), User.Identity.Name,
                                                         DateTime.Now, null, uiTxtStringValue.Text,
                                                         User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), Convert.ToDateTime(CtlCalendarEffectiveEndDate.Text), DateTime.Now,
                                                         uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(uiTxtNumericValue.Text))
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, null, User.Identity.Name,
                                                     DateTime.Now, Convert.ToDateTime(CtlCalendarDateValue.Text), uiTxtStringValue.Text,
                                                     User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), Convert.ToDateTime(CtlCalendarEffectiveEndDate.Text), DateTime.Now,
                                                     uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                        else
                        {
                            Parameter.ProposeParameter(uiTxtParamCode.Text, decimal.Parse(uiTxtNumericValue.Text), User.Identity.Name,
                                                     DateTime.Now, Convert.ToDateTime(CtlCalendarDateValue.Text), uiTxtStringValue.Text,
                                                     User.Identity.Name, Convert.ToDateTime(CtlCalendarEffectiveStartDate.Text), Convert.ToDateTime(CtlCalendarEffectiveEndDate.Text), DateTime.Now,
                                                     uiTxtApprovalDesc.Text, actionFlag, User.Identity.Name, Convert.ToDecimal(eID));
                        }
                      
                    }
                }
                Response.Redirect("ViewGlobalParamater.aspx");
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
        if (IsValidEntry() == true)
        {
            try
            {
                // Guard for editing proposed record
                ParameterData.ParameterGlobalRow dr = Parameter.SelectParameterByParameterID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Parameter.ApproveParameter(Convert.ToDecimal(eID), User.Identity.Name, uiTxtApprovalDesc.Text);
                }

                Response.Redirect("ViewGlobalParamater.aspx");
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
                ParameterData.ParameterGlobalRow dr = Parameter.SelectParameterByParameterID(Convert.ToDecimal(eID));
                if (dr.ApprovalStatus != "P") throw new ApplicationException("Record already approved.");

                if (Convert.ToString(eID) != "")
                {
                    Parameter.RejectProposedParameter(Convert.ToDecimal(eID), User.Identity.Name);
                }
                Response.Redirect("ViewGlobalParamater.aspx");
            }
            catch (Exception ex)
            {
                uiBLError.Items.Add(ex.Message);
                uiBLError.Visible = true;

            }
        }
    }


    #region SupportingMethod

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;

            if (eType == "add")
            {
                //cek apakah bank code sudah ada apa belum
                ParameterData.ParameterGlobalDataTable dt = new ParameterData.ParameterGlobalDataTable();
                dt = Parameter.SelectParameterByCode(uiTxtParamCode.Text);

                if (dt.Count > 0)
                {
                    uiBLError.Items.Add("Parameter code is already exist.");
                }
            }

            if (mp.IsChecker)
            {
                if (string.IsNullOrEmpty(uiTxtApprovalDesc.Text))
                {
                    uiBLError.Items.Add("Approval description is required.");
                }
            }

            if (string.IsNullOrEmpty(uiTxtParamCode.Text))
            {
                uiBLError.Items.Add("Parameter code is required.");
            }

            if (string.IsNullOrEmpty(CtlCalendarEffectiveStartDate.Text))
            {
                uiBLError.Items.Add("Start Date is required.");
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

    private void bindData()
    {
        try
        {
            ParameterData.ParameterGlobalRow dr = Parameter.SelectParameterByParameterID(Convert.ToDecimal(eID));
            uiTxtParamCode.Text = dr.Code;
            if (dr.IsNumericValueNull())
            {
                uiTxtNumericValue.Text = "";
            }
            else
            { uiTxtNumericValue.Text = dr.NumericValue.ToString("#,###.##"); }
            
            if (dr.IsStringValueNull())
            {
                uiTxtStringValue.Text = "";
            }
            else { uiTxtStringValue.Text = dr.StringValue.ToString(); }
            
            CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MMM-yyyy"));
            
            if (dr.IsDateValueNull())
            {
                CtlCalendarDateValue.SetCalendarValue("");
            }
            else 
            {
                CtlCalendarDateValue.SetCalendarValue(dr.DateValue.ToString("dd-MMM-yyyy"));
            }

            if (dr.IsEffectiveEndDateNull())
            {
                CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
            }
            else
            {
                CtlCalendarEffectiveEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MMM-yyyy"));
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
            trApprovalDesc.Visible = mp.IsChecker || mp.IsViewer;
            if (eType == "edit")
            {
                //uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
            uiBtnApprove.Visible = mp.IsChecker;
            uiBtnReject.Visible = mp.IsChecker;


            // set disabled for other controls other than approval description, for checker
            if (mp.IsChecker)
            {
                ParameterData.ParameterGlobalRow dr = Parameter.SelectParameterByParameterID(Convert.ToDecimal(eID));

                uiTxtParamCode.Enabled = false;
                uiTxtParamCode.ReadOnly = true;
                uiTxtNumericValue.Enabled = false;
                uiTxtNumericValue.ReadOnly = true;
                uiTxtStringValue.Enabled = false;
                uiTxtStringValue.ReadOnly = true;
                uiTxtAction.Enabled = false;
                CtlCalendarEffectiveStartDate.SetDisabledCalendar(true);
                CtlCalendarEffectiveStartDate.SetCalendarValue(dr.EffectiveStartDate.ToString("dd-MM-yyyy"));
                if (dr.IsDateValueNull())
                {
                    CtlCalendarDateValue.SetDisabledCalendar(true);
                    CtlCalendarDateValue.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarDateValue.SetDisabledCalendar(true);
                    CtlCalendarDateValue.SetCalendarValue(dr.DateValue.ToString("dd-MM-yyyy"));
                }

                if (dr.IsEffectiveEndDateNull())
                {
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    CtlCalendarEffectiveEndDate.SetCalendarValue(null);
                }
                else
                {
                    CtlCalendarEffectiveEndDate.SetDisabledCalendar(true);
                    CtlCalendarEffectiveEndDate.SetCalendarValue(dr.EffectiveEndDate.ToString("dd-MM-yyyy"));
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    #endregion
}
