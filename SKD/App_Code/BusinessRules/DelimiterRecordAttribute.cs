using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DelimiterRecordAttribute
/// </summary>
public class DelimiterRecordAttribute : Attribute
{
    protected long _ordinal;
    public long Ordinal
    {
        get {return _ordinal;}
        set {_ordinal = value;}
    }

    protected string _format;
    public string Format
    {
        get { return _format; }
        set { _format = value; }
    }

    public DelimiterRecordAttribute(long ordinal, string format)
    {
        _ordinal = ordinal;
        _format = format;
    }

}
