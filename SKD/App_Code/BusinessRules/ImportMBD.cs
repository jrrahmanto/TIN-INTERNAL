using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImportMBD
/// </summary>
public class ImportMBD : TextImportHandler
{
    private string user;
    private decimal CMID;
    private DateTime bussDate;

    private MBDData.MBDDataTable dt;

    public ImportMBD(Type recordType, string userName, decimal cmID, DateTime bussinessDate)
        : base(recordType)
    {
        user = userName;
        CMID = cmID;
        bussDate = bussinessDate;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void ProcessRow(object obj)
    {
        try
        {
            string [] arrMBD = this.Parse(obj.ToString());
            if (dt == null)
            {
                dt = new MBDData.MBDDataTable();
            }
           
            //dt.AddMBDRow(CMID, DateTime.Now, bussDate, "P", decimal.Parse(arrMBD[0]), decimal.Parse(arrMBD[1]),
            //             user, DateTime.Now, DateTime.Now, null, -1, "I", "");
        }
        catch (Exception)
        {
            throw;
        }
    }

    private string[] Parse(string message)
    {
        string[] listofMbd = message.Split(new char[] { ',' });
        return listofMbd;
    }

    public override void Deinitialize()
    {
        try
        {
            MBD.Proposed(CMID, dt[0].EffectiveStartDate, dt[0].MBDValue, dt[0].MBDFund, dt[0].ActionFlag,
                         user, null);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        base.Deinitialize();
    }

}
