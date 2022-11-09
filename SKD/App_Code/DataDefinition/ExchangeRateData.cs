using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ExchangeRateData
/// </summary>
/// 
//public class ExchangeRateData
//{
//    public ExchangeRateData()
//    {
//        //
//        // TODO: Add constructor logic here
//        //
//    }
//}

namespace ExchangeRateDataTableAdapters
{
    partial class ExchangeRateTableAdapter
    {
        public void setConnection(SqlConnection conn)
        {
            if (conn ==null)
            {
                throw new ArgumentException("Connection is not set!");
            }
            Connection = conn;
            
        }

        public void SetTransaction(SqlTransaction trans)
        {
            if (trans == null)
            {
                throw new ArgumentException("Transaction is not set");   
            }
            if (Adapter.SelectCommand != null)
            {
                Adapter.SelectCommand.Connection = trans.Connection;
                Adapter.SelectCommand.Transaction = trans;
            }
            if (Adapter.InsertCommand != null)
            {
                Adapter.InsertCommand.Connection = trans.Connection;
                Adapter.InsertCommand.Transaction = trans;
            }
            if (Adapter.UpdateCommand != null)
            {
                Adapter.UpdateCommand.Connection = trans.Connection;
                Adapter.UpdateCommand.Transaction = trans;
            }
             if (Adapter.DeleteCommand != null)
            {
                Adapter.DeleteCommand.Connection = trans.Connection;
                Adapter.DeleteCommand.Transaction = trans;
            }
             if (_commandCollection != null)
             {
                 foreach (SqlCommand cmd in _commandCollection)
                 {
                     cmd.Connection = trans.Connection;
                     cmd.Transaction = trans;
                 }
             }

        }
    }
}

   