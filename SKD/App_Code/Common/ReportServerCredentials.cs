using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

/// <summary>
/// Summary description for ReportServerCredentials
/// </summary>
public class ReportServerCredentials : IReportServerCredentials
{
    public System.Security.Principal.WindowsIdentity ImpersonationUser
    {
        get
        {
            return null; 
        }
    }

    public System.Net.ICredentials NetworkCredentials
    {
        get
        {
            return new System.Net.NetworkCredential(_username, _password, _domain);
        }
    }

    string _username;
    string _password;
    string _domain;

    public ReportServerCredentials()
    {
        _username = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_USERNAME_REPORT_SERVER].ToString();
        _password = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_PASSWORD_REPORT_SERVER].ToString();
        _domain = System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_DOMAIN_REPORT_SERVER].ToString(); 
    }

	public ReportServerCredentials(string userName, string password, string domain)
	{
        _username = userName;
        _password = password;
        _domain = "http://10.15.2.60/ReportServerEOD";
	}

    public bool GetFormsCredentials(out System.Net.Cookie authCookie,
            out string user, out string password, out string authority)
    {
        authCookie = null;
        user = password = authority = null;
        return false;
    }
}
