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
using System.Text.RegularExpressions;
using System.Globalization;

public partial class RiskManagement_MasterData_EntryManageParameterForAutoSuspension : System.Web.UI.Page
{
    //code di hard code
    string codePIC = "PICSuspendMessage";
    string codeUIC = "UICSuspendMessage";
    string codeEmailCc1 = "EmailCc1SuspendMessage";
    string codeEmailCc2 = "EmailCc2SuspendMessage";
    string codeShortageThreshold = "ShortageThreshold";
    string codeAvailableMarginTolerance = "AvailableMarginTolerance";
    string codeLimitTime = "LimitSuspendTimeBound";
    string codePreMarginingActStatus = "PreMarginingActiveStatus";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            uiBLError.Visible = false;
            if (!Page.IsPostBack)
            {
                uiBLError.Visible = false;
                uiBLSuccess.Visible = false;

                uiBLError.Items.Clear();
                uiBLSuccess.Items.Clear();
                bindData();
            }            
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }
    protected void uiBtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime lastUpdatedDate = DateTime.Now;
            uiBLSuccess.Items.Clear();
            if (IsValidEntry() == true)
            {
                Parameter.UpdateParameterStringValueByCode(codeUIC, uiTxtUIC.Text, User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterStringValueByCode(codePIC, uiTxtPIC.Text, User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterStringValueByCode(codeEmailCc1, uiTxtEmailCC1.Text, User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterStringValueByCode(codeEmailCc2, uiTxtEmailCC2.Text, User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterNumericValueByCode(codeShortageThreshold, Convert.ToDecimal(uiTxtLimitThreshold.Text), User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterNumericValueByCode(codeAvailableMarginTolerance, Convert.ToDecimal(uiTxtAMTole.Text), User.Identity.Name, lastUpdatedDate);
                Parameter.UpdateParameterStringValueByCode(codePreMarginingActStatus, uiDdlActivationModule.SelectedValue, User.Identity.Name, lastUpdatedDate);
                DateTime date = DateTime.Now;
                date = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(uiTxtLimitTime_hh.Text)
                    , Convert.ToInt32(uiTxtLimitTime_mm.Text), 0);
                Parameter.UpdateParameterDateValueByCode(codeLimitTime, date, User.Identity.Name, lastUpdatedDate);

                uiBLSuccess.Items.Add("Parameter updated successfully.");
                uiBLSuccess.Visible = true;
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    protected void UpdateLimitSuspendTimeBound(DateTime lastUpdatedDate)
    {
        
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // redirect on itself
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
        }
    }

    private void bindData()
    {
        try
        {
            uiTxtUIC.Text = SetStringValue(codePIC,"A");
            uiTxtPIC.Text = SetStringValue(codeUIC, "A");
            uiTxtEmailCC1.Text = SetStringValue(codeEmailCc1, "A");
            uiTxtEmailCC2.Text = SetStringValue(codeEmailCc2, "A");
            uiTxtLimitThreshold.Text = SetNumericValue(codeShortageThreshold, "A");
            uiTxtAMTole.Text = SetNumericValue(codeAvailableMarginTolerance, "A");
            uiTxtLimitTime_hh.Text = SetTimeValue_hh(codeLimitTime, "A");
            uiTxtLimitTime_mm.Text = SetTimeValue_mm(codeLimitTime, "A");
            uiDdlActivationModule.SelectedValue = SetStringValue(codePreMarginingActStatus, "A");
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    private bool IsValidEntry()
    {
        try
        {
            bool isValid = true;
            uiBLError.Visible = false;
            uiBLError.Items.Clear();
            MasterPage mp = (MasterPage)this.Master;
            if (string.IsNullOrEmpty(uiTxtUIC.Text))
            {
                uiBLError.Items.Add("Unit In-Charge is required.");
            }

            if (string.IsNullOrEmpty(uiTxtPIC.Text))
            {
                uiBLError.Items.Add("Person In-Charge is required.");
            }

            Regex regex = new Regex("^[0-9]*$");
            if (!string.IsNullOrEmpty(uiTxtEmailCC1.Text))
            {
                if (regex.IsMatch(uiTxtEmailCC1.Text))
                {
                    uiBLError.Items.Add("Format Email CC1 is wrong.");
                }
            }
            else
            {
                uiBLError.Items.Add("Email CC1 is required.");
            }

            if (!string.IsNullOrEmpty(uiTxtEmailCC2.Text))
            {
                if (regex.IsMatch(uiTxtEmailCC2.Text))
                {
                    uiBLError.Items.Add("Format Email CC2 is wrong.");
                }
            }
            //else
            //{
            //    uiBLError.Items.Add("Email CC2 is required.");
            //}

            decimal distance;
            if (!string.IsNullOrEmpty(uiTxtLimitThreshold.Text))
            {
                if (!decimal.TryParse(uiTxtLimitThreshold.Text, out distance))
                {
                    uiBLError.Items.Add("Limit Threshold is not a number.");
                }                
            }
            else
            {
                uiTxtLimitThreshold.Text = "0";
            }

            if (!string.IsNullOrEmpty(uiTxtAMTole.Text))
            {
                if (!decimal.TryParse(uiTxtAMTole.Text, out distance))
                {
                    uiBLError.Items.Add("Available Margin Tolerance is not a number.");
                }
            }
            else
            {
                uiTxtAMTole.Text = "0";
            }

            if (!string.IsNullOrEmpty(uiTxtLimitTime_hh.Text))
            {
                if (Convert.ToInt32(uiTxtLimitTime_hh.Text) < 0 || Convert.ToInt32(uiTxtLimitTime_hh.Text) > 24)
                    uiBLError.Items.Add("Invalid Hours");
            }
            else
            {
                uiTxtLimitTime_hh.Text = "00";
            }

            if (!string.IsNullOrEmpty(uiTxtLimitTime_mm.Text))
            {
                if (Convert.ToInt32(uiTxtLimitTime_mm.Text) < 0 || Convert.ToInt32(uiTxtLimitTime_mm.Text) > 59)
                    uiBLError.Items.Add("Invalid Minute");
            }
            else
            {
                uiTxtLimitTime_mm.Text = "00";
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
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;
            return false;
        }
    }

    private string SetStringValue(string code, string approval)
    {
        ParameterData.ParameterRow dr = Parameter.GetParameterByCodeAndApprovalStatus(code, approval);
        string stringValue = "";
        if (dr.IsStringValueNull())
        {
            stringValue = "";
        }
        else
        {
            stringValue = dr.StringValue.ToString(); 
        }
        return stringValue;
    }

    private string SetNumericValue(string code, string approval)
    {
        ParameterData.ParameterRow dr = Parameter.GetParameterByCodeAndApprovalStatus(code, approval);
        string numericValue = "";
        if (dr.IsNumericValueNull())
        {
            numericValue = "0";
        }
        else
        {
            numericValue = dr.NumericValue.ToString("#,###.##");
        }
        return numericValue;
    }

    private string SetTimeValue_hh(string code, string approval)
    {
        ParameterData.ParameterRow dr = Parameter.GetParameterByCodeAndApprovalStatus(code, approval);
        string numericValue = "";
        if (!dr.IsDateValueNull())
        {
            numericValue = dr.DateValue.ToString("HH",
                                CultureInfo.InvariantCulture);
        }
        return numericValue;
    }

    private string SetTimeValue_mm(string code, string approval)
    {
        ParameterData.ParameterRow dr = Parameter.GetParameterByCodeAndApprovalStatus(code, approval);
        string numericValue = "";
        if (!dr.IsDateValueNull())
        {
            numericValue = dr.DateValue.ToString("mm",
                                CultureInfo.InvariantCulture);
        }
        return numericValue;
    }
}
