using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImportSummaryEventArgs
/// </summary>
public class ExportImportSummaryEventArgs
{
    long _totalRow;
    public long TotalRow
    {
        get { return _totalRow; }
        set { _totalRow = value; }
    }

    long _totalError;
    public long TotalError
    {
        get { return _totalError; }
        set { _totalError = value; }
    }

    long _totalSuccess;
    public long TotalSuccess
    {
        get { return _totalSuccess; }
        set { _totalSuccess = value; }
    }

	public ExportImportSummaryEventArgs(long totalRow, long totalError, long totalSuccess)
	{
        _totalRow = totalRow;
        _totalError = totalError;
        _totalSuccess = totalSuccess;
	}
}
