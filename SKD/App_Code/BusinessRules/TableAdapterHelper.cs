using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Reflection;

/// <summary>
/// Summary description for TableAdapterHelper
/// </summary>
public class TableAdapterHelper
{
	public TableAdapterHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void SetAllCommandTimeouts(object adapter, int timeout)
    {
        var commands = adapter.GetType().InvokeMember(
                "CommandCollection",
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, adapter, new object[0]);
        var sqlCommand = (SqlCommand[])commands;
        foreach (var cmd in sqlCommand)
        {
            cmd.CommandTimeout = timeout;
        }
    }

    /// <summary>
    /// Latest function for set tmeout of table adapter
    /// </summary>
    /// <param name="adapter"></param>
    /// <param name="timeout"></param>
    public static void SetAllCommandTimeouts2(object adapter, int timeout)
    {
        var commands = adapter.GetType().InvokeMember(
                "CommandCollection",
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, adapter, new object[0]);
        var sqlCommand = (IDbCommand[])commands;
        foreach (var cmd in sqlCommand)
        {
            cmd.CommandTimeout = timeout;
        }
    }

}
