using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RowErrorEventArgs
/// </summary>
public class RowErrorEventArgs
{
    object _obj;
    public object Obj
    {
        get { return _obj; }
        set { _obj = value; }
    }

    string _errMsg;
    public string ErrMsg
    {
        get { return _errMsg; }
        set { _errMsg = value; }
    }

    long _lineNumber;
    public long LineNumber
    {
        get { return _lineNumber; }
        set { _lineNumber = value; }
    }

    long _totalError;
    public long TotalError
    {
        get { return _totalError; }
        set { _totalError = value; }
    }

    string _line;
    public string Line
    {
        get { return _line; }
        set { _line = value; }
    }

	public RowErrorEventArgs(object obj, string errMsg, long lineNumber, long totalError,
        string line)
	{
        _obj = obj;
        _errMsg = errMsg;
        _lineNumber = lineNumber;
        _totalError = totalError;
        _line = line;
	}
}
