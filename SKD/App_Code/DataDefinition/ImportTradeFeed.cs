using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImportTradeFeed
/// </summary>
/// 
namespace TradefeedDataTableAdapters
{
    public partial class RawTradeFeedTableAdapter
    {
        public int CommandTimeOut
        {
            set
            {
                for (int ii = 0; ii < this.CommandCollection.Length; ii++)
                {
                    this.CommandCollection[ii].CommandTimeout = value;
                }
            }
        }
    }

}
