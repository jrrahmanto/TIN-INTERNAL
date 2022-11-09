using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


/// <summary>
/// Summary description for QueryEODTableAdapter
/// </summary>
/// 

namespace EODDataTableAdapters
{
    public partial class QueryEODTableAdapter
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

        public SqlTransaction BeginTransaction(SqlConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                throw new ArgumentException("Connection must be open to begin a transaction");
            }

            SqlTransaction trans = connection.BeginTransaction();
            
            for (int ii = 0; ii < this.CommandCollection.Length; ii++)
            {
                this.CommandCollection[ii].Connection = connection;
                this.CommandCollection[ii].Transaction = trans;
            }

            return trans;
        }
    }

    public partial class uspEODValidationTableAdapter
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

    public partial class EODExchangeInfoTableAdapter
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


