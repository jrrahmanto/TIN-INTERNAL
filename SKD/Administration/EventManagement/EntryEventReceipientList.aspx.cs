using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

public partial class Administration_EventManagement_EntryEventReceipientList : System.Web.UI.Page
{
    private string currentID
    {
        get
        {
            return Request.QueryString["id"];
        }
    }

    private string eType
    {
        get { return Request.QueryString["eType"].ToString(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetAccessPage();
        uiBLError.Visible = false;
        if (!IsPostBack)
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

    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
           if(IsValidEntry() == true)
           {
                if (currentID != null)
                {
                    EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();
                    dt = EventReceipientList.SelectEventReceipientListByEventReceipientListID(dt, Convert.ToDecimal(currentID));
                   
                    if (string.IsNullOrEmpty(CtlUserMemberLookup1.LookupTextBoxID))
                    {
                        EventReceipientList.UpdateEventReceipientList(uiTxtEventReceipientName.Text, uiTxtPhoneNumber.Text,
                        string.IsNullOrEmpty(uiTxtEmailAddress.Text) ? "" : uiTxtEmailAddress.Text, null, User.Identity.Name);
                    }
                    else
                    {
                       EventReceipientList.UpdateEventReceipientList(uiTxtEventReceipientName.Text, uiTxtPhoneNumber.Text,
                        string.IsNullOrEmpty(uiTxtEmailAddress.Text) ? "" : uiTxtEmailAddress.Text, new Guid(CtlUserMemberLookup1.LookupTextBoxID), User.Identity.Name);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(CtlUserMemberLookup1.LookupTextBoxID))
                    {
                        EventReceipientList.AddEventReceipientList(uiTxtEventReceipientName.Text, uiTxtPhoneNumber.Text,
                            string.IsNullOrEmpty(uiTxtEmailAddress.Text) ? "" : uiTxtEmailAddress.Text, null, User.Identity.Name);
                    }
                    else
                    {
                        EventReceipientList.AddEventReceipientList(uiTxtEventReceipientName.Text, uiTxtPhoneNumber.Text,
                              string.IsNullOrEmpty(uiTxtEmailAddress.Text) ? "" : uiTxtEmailAddress.Text, new Guid(CtlUserMemberLookup1.LookupTextBoxID), User.Identity.Name);
                    }
                }
                Response.Redirect("ViewEventReceipientList.aspx");
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
        Response.Redirect("ViewEventReceipientList.aspx");
    }

    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (currentID != "")
            {
                EventReceipientList.DeleteEventReceipientList(Convert.ToDecimal(currentID), User.Identity.Name);
            }
            Response.Redirect("ViewEventReceipientList.aspx");
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
            if (currentID == null)
            {
                if (string.IsNullOrEmpty(uiTxtEventReceipientName.Text))
                {
                    uiBLError.Items.Add("Event receipient name is required.");
                }
            }

            if (uiTxtPhoneNumber.Text == "" && uiTxtEmailAddress.Text == "" && CtlUserMemberLookup1.LookupTextBox == "")
            {
                uiBLError.Items.Add("Either one of the delivery channel (phone, email, or user) should be filled.");
            }

            if (!string.IsNullOrEmpty(uiTxtEmailAddress.Text))
            {
                //check email character
                EventReceipientList.isValidEmail(uiTxtEmailAddress.Text);
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

            EventReceipientListData.EventRecipientListDataTable dt = new EventReceipientListData.EventRecipientListDataTable();
            dt = EventReceipientList.SelectEventReceipientListByEventReceipientListID(dt, Convert.ToDecimal(currentID));
            
            if (dt.Rows.Count > 0)
            {
                uiTxtEventReceipientName.Text = dt[0].EventRecipientName;
                
                if (eType == "edit")
                {

                    if (!dt[0].IsPhoneNumberNull())
                    {
                        uiTxtPhoneNumber.Text = dt[0].PhoneNumber;
                    }
                    else
                    {
                        uiTxtPhoneNumber.Text = "";
                    }

                    if (!dt[0].IsEmailAddressNull())
                    {
                        uiTxtEmailAddress.Text = dt[0].EmailAddress;
                    }
                    else
                    {
                        uiTxtEmailAddress.Text = "";
                    }
       
                    if (!dt[0].IsUserIDNull())
                    {
                        UserData.UserProfileDataTable dtUser = new UserData.UserProfileDataTable();
                        dtUser = SKDUser.GetUserByUserID(dt[0].UserID);
                        CtlUserMemberLookup1.LookupTextBox = dtUser[0].UserName;
                    }
                    else
                    {
                        CtlUserMemberLookup1.LookupTextBox = "";
                    }
                }
            }
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

            if (currentID != null)
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
