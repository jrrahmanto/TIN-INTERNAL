using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RowValidEventArgs
/// </summary>
public class RowValidEventArgs
{
    public object _obj;
    public object Obj
    {
        get { return _obj; }
        set { _obj = value; }
    }

	public RowValidEventArgs(object obj)
	{
        _obj = obj;
	}
}
