using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TradefeedData
/// </summary>

namespace TradefeedDataTableAdapters
{
    public partial class QueriesTableAdapter
    {
        //
        // TODO: Add constructor logic here
        //
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
