using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;

/// <summary>
/// Summary description for SubCategory
/// </summary>
public class SubCategory
{
  

    #region "Supporting Method"
    public static SubCategoryData.SubCategoryDataTable GetSubCategoryByCodeAndDesc(string code, string desc)
    {
        SubCategoryData .SubCategoryDataTable dt = new SubCategoryData.SubCategoryDataTable();
        SubCategoryDataTableAdapters.SubCategoryTableAdapter ta = new SubCategoryDataTableAdapters.SubCategoryTableAdapter();

        try
        {
            ta.FillByCodeAndDesc(dt, code, desc);

            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static string GetCodeBySubCategoryID(decimal subcategoryID)
    {
        SubCategoryData.SubCategoryDataTable dt = new SubCategoryData.SubCategoryDataTable();
        SubCategoryDataTableAdapters.SubCategoryTableAdapter ta = new SubCategoryDataTableAdapters.SubCategoryTableAdapter();
        string code = "";

        try
        {
            ta.FillBySubCategoryID(dt, subcategoryID);
            if (dt.Count > 0)
                code = dt[0].SubCategoryCode;
            
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }

        return code;

    }
    #endregion


}
