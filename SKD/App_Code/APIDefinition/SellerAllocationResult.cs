using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SellerAllocationResult
/// </summary>
public class SellerAllocationResult
{


    public class RootObject
    {
        public string message { get; set; }
        public Result result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
        public Item[] items { get; set; }
        public int countperpage { get; set; }
        public int page { get; set; }
        public int totalPage { get; set; }
    }

    public class Item
    {
        public string sellerId { get; set; }
        public string companyName { get; set; }
        public string accountId { get; set; }
        public string productId { get; set; }
        public string volume { get; set; }
        public DateTime date { get; set; }
    }

}