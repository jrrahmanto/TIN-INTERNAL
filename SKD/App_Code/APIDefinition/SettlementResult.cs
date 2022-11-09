using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SettlementResult
/// </summary>
public class SettlementResult
{

    public class Rootobject
    {
        public string message { get; set; }
        public Result result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
    }

}