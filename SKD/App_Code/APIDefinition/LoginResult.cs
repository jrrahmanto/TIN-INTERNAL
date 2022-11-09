using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoginResult
/// </summary>
public class LoginResult
{
    public class Rootobject
    {
        public string message { get; set; }
        public Result result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
        public string accessToken { get; set; }
        public string firstName { get; set; }
        public string usergroup { get; set; }
    }

}