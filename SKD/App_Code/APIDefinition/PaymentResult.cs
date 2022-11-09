using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PaymentResult
/// </summary>
public class PaymentResult
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