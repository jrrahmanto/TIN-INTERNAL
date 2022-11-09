using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HighLowPriceResult
/// </summary>
public class HighLowPriceResult
{

    public class RootObject
    {
        public string message { get; set; }
        public Result result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
        public string ceilPrice { get; set; }
        public string floorPrice { get; set; }
        public DateTime startDate { get; set; }
    }

}