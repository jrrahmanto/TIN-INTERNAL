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

public partial class Administration_EventManagement_EntryEventType : System.Web.UI.Page
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
        try
        {
            SetAccessPage();
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
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }
    protected void uiBtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewEventType.aspx");
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (eID.ToString() != "")
            {
                    EventType.DeleteEventType(Convert.ToDecimal(eID), User.Identity.Name);
            }
            Response.Redirect("ViewEventType.aspx");
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
             if (IsValidEntry() == true)
            {
                if (eID != 0)
                {
                        EventType.UpdateEventType(uiTxtEventTypeCode.Text, uiTxtEventTypeName.Text,
                                                string.IsNullOrEmpty(uiTxtSmsMessage.Text) ? false : true, string.IsNullOrEmpty(uiTxtEmailMessage.Text) ? false : true, string.IsNullOrEmpty(uiTxtApplicationMessage.Text) ? false : true, 
                                                uiTxtSmsMessage.Text, uiTxtEmailMessage.Text, 
                                                uiTxtApplicationMessage.Text, User.Identity.Name);
                 }
                else
                {
                        EventType.AddEventType(uiTxtEventTypeCode.Text, uiTxtEventTypeName.Text,
                            string.IsNullOrEmpty(uiTxtSmsMessage.Text) ? false : true, string.IsNullOrEmpty(uiTxtEmailMessage.Text) ? false : true, string.IsNullOrEmpty(uiTxtApplicationMessage.Text) ? false : true, uiTxtSmsMessage.Text, uiTxtEmailMessage.Text,
                                                 uiTxtApplicationMessage.Text, User.Identity.Name);
                }
                Response.Redirect("ViewEventType.aspx");
            }
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

        if (string.IsNullOrEmpty(uiTxtEventTypeCode.Text))
        {
            uiBLError.Items.Add("Event type code is required.");
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
       try
         {
            EventTypeData.EventTypeRow dr = EventType.SelectEventTypeByEventTypeID(Convert.ToDecimal(eID));

            uiTxtEventTypeCode.Text = dr.EventTypeCode;
            uiTxtEventTypeName.Text = dr.Name;
            uiTxtSmsMessage.Text= dr.SMSMessage;
            uiTxtEmailMessage.Text = dr.EmailMessage;
            uiTxtApplicationMessage.Text = dr.ApplicationMessage;
         }
       catch (Exception ex)
       {
           uiBLError.Items.Add(ex.Message);
           uiBLError.Visible = true;

       }
    }

    private void SetAccessPage()
    {
        try
        {
            MasterPage mp = (MasterPage)this.Master;

            if (eType == "edit")
            {
                uiBtnDelete.Visible = mp.IsMaker;
            }
            uiBtnSave.Visible = mp.IsMaker;
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
     }


    #endregion

}
