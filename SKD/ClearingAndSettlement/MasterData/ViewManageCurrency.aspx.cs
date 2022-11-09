using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUI_ClearingAndSettlement_ViewManageCurrency : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiLblWarning.Visible = false;
        string pageName = Request.Url.AbsolutePath.Replace(".aspx", "");
    }
    protected void uiBtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntryManageCurrency.aspx");
    }

    private List<string> getCheckedRows()
    {
        //get dataset to datagrid
        List<string> uiChkList = new List<string>();

        foreach (GridViewRow GridRow in uiDgManageCurrency.Rows)
        {
            CheckBox selectedCheckBox = new CheckBox();
            selectedCheckBox = ((CheckBox)GridRow.FindControl("uiChkList"));
            if (selectedCheckBox.Checked)
            {
                DataKey dk = uiDgManageCurrency.DataKeys[GridRow.RowIndex];
                decimal value = Convert.ToDecimal(dk.Value);
                uiChkList.Add(value.ToString());
            }
        }
        return uiChkList;
    }

}
