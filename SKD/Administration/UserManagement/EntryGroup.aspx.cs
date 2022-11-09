using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Transactions;

public partial class UserManagement_EntryGroup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uiFldMapPath.Value = Server.MapPath("~");
    }


    protected void Button1_Click(object sender, EventArgs e)
    {

       ;
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
               
                foreach (GridViewRow dr in uiDgExchangeMember.Rows)
                {

                    System.Web.Security.Roles.CreateRole(Tools.ComputeMD5(dr.Cells[0].Text, "") + "_" + dr.Cells[1].Text);
                 
                }
                scope.Complete();
            }
            
        }
        catch (Exception ex)
        {
            
            throw ex;
        }
       
        
    }
}
