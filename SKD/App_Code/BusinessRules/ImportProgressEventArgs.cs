using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImportProgressEventArgs
/// </summary>
public class ExportImportProgressEventArgs
{
    long _totalProgress;
    public long TotalProgress
    {
        get { return _totalProgress; }
        set { _totalProgress = value; } 
    }

    long _currentProgress;
    public long TotalLength
    {
        get { return _currentProgress; }
        set { _currentProgress = value; }
    }

	public ExportImportProgressEventArgs(long currentProgress, long totalProgress)
	{
        _currentProgress = currentProgress;
        _totalProgress = totalProgress;
	}
}
