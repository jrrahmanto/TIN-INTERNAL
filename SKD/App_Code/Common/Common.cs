using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public const string SETTING_FOLDER_EOD_AK = "FOLDER_EOD_AK";
    public const string SETTING_FOLDER_EOD_ALL = "FOLDER_EOD_ALL";
    public const string SETTING_FOLDER_UPLOAD = "FOLDER_UPLOAD";
    public const string SETTING_REPORT_SERVER = "REPORT_SERVER";
    public const string SETTING_USERNAME_REPORT_SERVER = "REPORT_SERVER_USERNAME";
    public const string SETTING_PASSWORD_REPORT_SERVER = "REPORT_SERVER_PASSWORD";
    public const string SETTING_DOMAIN_REPORT_SERVER = "REPORT_SERVER_DOMAIN";
    public const string SETTING_EOD_GENERATE_TYPE = "EOD_GENERATE_TYPE";
    public const string SETTING_EOD_GENERATE_TYPE_ALL_AK = "EOD_GENERATE_TYPE_ALL_AK";
    public const string SETTING_EOD_GENERATE_TYPE_AK = "EOD_GENERATE_TYPE_AK";
    public const string SETTING_EOD_GENERATE_TYPE_ALL = "EOD_GENERATE_TYPE_ALL";

    public const string SETTING_FOLDER_INVOICE_MONTHLY = "FOLDER_INVOICE_MONTHLY";
    public const string SETTING_FOLDER_INVOICE_ANNUAL = "FOLDER_INVOICE_ANNUAL";
    public const string SETTING_FOLDER_INVOICE_FEE = "FOLDER_INVOICE_FEE";
    public const string SETTING_FOLDER_DOCUMENT = "FOLDER_DOCUMENT";
    public const string SETTING_FOLDER_DOCUMENT_UPLOAD = "FOLDER_DOCUMENT_UPLOAD";
    public const string SETTING_FOLDER_INVOICE_MEMBERSHIP = "FOLDER_INVOICE_MEMBERSHIP";

    public Common()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void CreateFileByFileStream(byte[] bytes, string filepath)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(filepath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
    }

    public static bool IsEndOfMonth(DateTime date)
    {
        bool ret = false;
        DateTime nextDate = date.AddDays(1);
        if (nextDate.Day == 1)
        {
            ret = true;
        }

        return ret;
    }

    public static string GetApprovalDescription(string approvalStatus)
    {
        string approvalDesc;
        switch (approvalStatus)
        { 
            case "A" :
                approvalDesc = "Approved";
                break;
            case "P" :
                approvalDesc = "Proposed";
                break;
            case "R" :
                approvalDesc = "Rejected";
                break;
            default :
                approvalDesc = "";
                break;        
        }

        return approvalDesc;
    }

    public static string GetIPAddress(HttpRequest request)
    {
        //string hostName = Dns.GetHostName();
        //string ipAddress = Dns.GetHostAddresses(hostName).GetValue(0).ToString();
        return request.UserHostAddress.ToString();
    }
}
