using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Summary description for Tools
/// </summary>
public class Tools
{
    public Tools()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string ComputeMD5(string input, string salt)
    {
        return ComputeHash(new MD5CryptoServiceProvider(), salt + input + salt);
    }

    public static string ComputeHash(HashAlgorithm cryptoServiceProvider, string input)
    {
        byte[] result = cryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(input));
        return GetString(result);
    }

    public static string GetString(byte[] input)
    {
        StringBuilder sb = new StringBuilder();
        for (int ii = 0; ii < input.Length; ii++)
        {
            sb.Append(input[ii].ToString("X"));
        }

        return sb.ToString();
    }

    public static void RegisterGenericScript(System.Web.UI.Page page)
    {
        if (!page.ClientScript.IsClientScriptBlockRegistered("GenericLookupScript"))
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script language=\"javascript\">");
            sb.AppendLine("function enable(elname, val) { ");
            sb.AppendLine("   if(document.getElementById(elname)) ");
            sb.AppendLine("      document.getElementById(elname).disabled=val;");
            sb.AppendLine("}");

            sb.AppendLine("function setValue(elname, val) { ");
            sb.AppendLine("   if(document.getElementById(elname)) ");
            sb.AppendLine("      document.getElementById(elname).value=val;");
            sb.AppendLine("}");
            sb.AppendLine("</script>");

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "GenericLookupScript", sb.ToString());

        }
    }
}
