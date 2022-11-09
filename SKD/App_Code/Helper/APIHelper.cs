using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for APIHelper
/// </summary>
public class APIHelper
{
    

    public APIHelper()
    {
    }

    public static string Post(string url, List<KeyValuePair<string, string>> listHeader,
        List<KeyValuePair<string, string>> listParam)
    {
        Uri uri = new Uri(url);
        string baseAddress = string.Format("{0}://{1}", uri.Scheme, uri.Authority);
        WebClient client = new WebClient();
        client.BaseAddress = baseAddress;
        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        if (listHeader != null)
        {
            foreach (var header in listHeader)
            {
                client.Headers[header.Key] = header.Value;
            }
        }

        string response = "";
        string query = "";

        if (listParam != null)
        {
            List<string> listQuery = new List<string>();
            foreach (var param in listParam)
            {
                listQuery.Add(string.Format("{0}={1}", param.Key, param.Value));
            }
            query = string.Join("&", listQuery.ToArray());
        }
        
        response = client.UploadString(url, query);

        return response;
    }

    public static string Get(string url, List<KeyValuePair<string, string>> listHeader,
        List<KeyValuePair<string, string>> listParam)
    {
        Uri uri = new Uri(url);
        string baseAddress = string.Format("{0}://{1}", uri.Scheme, uri.Authority);
        WebClient client = new WebClient();
        client.BaseAddress = baseAddress;
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        if (listHeader != null)
        {
            foreach (var header in listHeader)
            {
                client.Headers[header.Key] = header.Value;
            }
        }

        string response = "";
        string query = "";
        if (listParam != null)
        {
            List<string> listQuery = new List<string>();
            foreach (var param in listParam)
            {
                listQuery.Add(string.Format("{0}={1}", param.Key, param.Value));
            }
            query = string.Join("&", listQuery.ToArray());
        }

        response = client.DownloadString(string.Format("{0}?{1}", url, query));
        
        return response;
    }
    
}