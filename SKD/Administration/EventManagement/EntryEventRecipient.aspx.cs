using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class Administration_EventManagement_EntryEventRecipient : System.Web.UI.Page
{
    private string eventTypeID
    {
        get
        {
            return Request.QueryString["typeID"];
        }
    }

    //private string eventReceipientListID
    //{
    //    get
    //    {
    //        return Request.QueryString["recListID"];
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SetAccessPage();
            uiBLError.Visible = false;
            if (!IsPostBack)
            {
                if (eventTypeID != null)
                {
                    bindData();
                }
                else
                {
                    IEnumerable dv = (IEnumerable)ObjectDataSourceEventRecipientName.Select();
                    DataView dve = (DataView)dv;
                    EventReceipientListData.EventRecipientListDataTable dtEvent = (EventReceipientListData.EventRecipientListDataTable)dve.Table;

                    uiChkRecipientList.DataSource = ObjectDataSourceEventRecipientName;
                    uiChkRecipientList.DataValueField = "EventRecipientListID";
                    uiChkRecipientList.DataTextField = "EventRecipientName";
                    uiChkRecipientList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            uiBLError.Items.Add(ex.Message);
            uiBLError.Visible = true;

        }
    }
    protected void uiBtnSave_Click(object sender, EventArgs e)
    {
        //delete dulu
        //looping 
        //add

        try
        {
            if (IsValidEntry() == true)
            {
                //delete ID yang mau diganti
                if (eventTypeID != null)
                {
                    EventRecipientData.EventTypeRecipientDataTable dt = new EventRecipientData.EventTypeRecipientDataTable();
                    dt = EventRecipient.SelectEventRecipientByEventTypeID(dt, Convert.ToDecimal(eventTypeID));

                    List<decimal> RecipientListItem = new List<decimal>();
                    foreach (EventRecipientData.EventTypeRecipientRow list in dt.Rows)
                    {                        
                        RecipientListItem.Add(list.EventRecipientListID);
                    }

                    EventRecipient.DeleteRecipient(Convert.ToDecimal(uiDdlEventTypeCode.SelectedValue), RecipientListItem,
                                                User.Identity.Name);
                }

                //new checklist
                List<string> RecipientListNew = new List<string>();
                
                RecipientListNew.Clear();
     
                foreach (ListItem listItem in uiChkRecipientList.Items)
                {
                    if (listItem.Selected == true)
                    {
                        RecipientListNew.Add(listItem.Value);
                    }
                }

                EventRecipient.AddRecipient(Convert.ToDecimal(uiDdlEventTypeCode.SelectedValue), RecipientListNew,
                                               User.Identity.Name);

                Response.Redirect("ViewEventRecipient.aspx");
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
        Response.Redirect("ViewEventRecipient.aspx");
    }


    protected void uiBtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (eventTypeID != "")
            {
                EventType.DeleteEventType(Convert.ToDecimal(eventTypeID), User.Identity.Name);
            }
            Response.Redirect("ViewEventRecipient.aspx");
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

            //load berdasarkan id eventtypenya saja
            EventRecipientData.EventTypeRecipientDataTable dt = new EventRecipientData.EventTypeRecipientDataTable();
            dt = EventRecipient.SelectEventRecipientByEventTypeID(dt, Convert.ToDecimal(eventTypeID));
           
            List<decimal> RecipientListItem = new List<decimal>();
            StringCollection sc = new StringCollection();
            foreach (EventRecipientData.EventTypeRecipientRow list in dt.Rows)
            {
                uiDdlEventTypeCode.SelectedValue = dt[0].EventTypeID.ToString();
                uiChkRecipientList.SelectedValue = dt[0].EventRecipientListID.ToString();
                sc.Add(list.EventRecipientListID.ToString());
            }

            IEnumerable dv = (IEnumerable)ObjectDataSourceEventRecipientName.Select();
            DataView dve = (DataView)dv;
            EventReceipientListData.EventRecipientListDataTable dtEvent = (EventReceipientListData.EventRecipientListDataTable)dve.Table;

            foreach (EventReceipientListData.EventRecipientListRow drEvent in dtEvent)
            {
                uiChkRecipientList.Items.Add(new ListItem(drEvent.EventRecipientName, drEvent.EventRecipientListID.ToString()));
            }

            foreach (ListItem li in uiChkRecipientList.Items)
            {
                if (sc.Contains(li.Value))
                {
                    li.Selected = true;
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

            if (eventTypeID != null )
            {
                //uiBtnDelete.Visible = mp.IsMaker;
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
