using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for ImportHandler
/// </summary>
public abstract class ImportHandler
{
    public event EventHandler Starting;
    public event EventHandler Finishing; 
    public event EventHandler Cancel;
    public delegate void ImportProgressEventHandler(object sender, ExportImportProgressEventArgs e);
    public event ImportProgressEventHandler Progress;
    public event EventHandler Success;
    public delegate void ImportSummaryEventHandler(object sender, ExportImportSummaryEventArgs e);
    public event ImportSummaryEventHandler Summary;
    public delegate void RowErrorEventHandler(object sender, RowErrorEventArgs e);
    public event RowErrorEventHandler RowError;
    public delegate void RowValidEventHandler(object sender, RowValidEventArgs e);
    public event RowValidEventHandler RowValid;
        
    public abstract void Initialize();
    public abstract void Deinitialize();
    public abstract void StartImport();
    public abstract void ProcessRow(object obj);
    public abstract bool IsFinish();

    private bool _isCancel;
    public bool IsCancel
    {
        get { return _isCancel; }
        set { _isCancel = value; }
    }

    private long _currentProgress;
    public long CurrentProgress
    {
        get { return _currentProgress; }
        set { _currentProgress = value; }
    }

    private long _totalProgress;
    public long TotalProgress
    {
        get { return _totalProgress; }
        set { _totalProgress = value; }
    }

    private long _totalRow;
    public long TotalRow
    {
        get { return _totalRow; }
        set { _totalRow = value; }
    }

    private long _totalError;
    public long TotalError
    {
        get { return _totalError; }
        set { _totalError = value; }
    }

    private long _totalSuccess;
    public long TotalSuccess
    {
        get { return _totalSuccess; }
        set { _totalSuccess = value; }
    }

	public ImportHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public virtual void RaiseRowValidEvent(object obj)
    {
        if (RowValid != null)
        {
            RowValid(this, new RowValidEventArgs(obj));
        }
    }

    public virtual void RaiseRowErrorEvent(object obj, string errMsg, long lineNumber, 
        long totalError, string line)
    {
        if (RowError != null)
        {
            RowError(this, new RowErrorEventArgs(obj, errMsg, lineNumber, totalError, line));
        }
    }

    public virtual void Import()
    {
        try
        {
            Initialize();

            if (Starting != null)
            {
                Starting(this, EventArgs.Empty);
            }

            IsCancel = false;

            while (IsFinish() == false && IsCancel == false)
            {
                StartImport();

                if (Progress != null)
                {
                    Progress(this, new ExportImportProgressEventArgs(this.CurrentProgress, this.TotalProgress));
                }
            }

            if (IsCancel)
            {
                if (Cancel != null)
                {
                    Cancel(this, EventArgs.Empty);
                }
            }

            if (Success != null)
            {
                Success(this, EventArgs.Empty);
            }

            if (Summary != null)
            {
                Summary(this, new ExportImportSummaryEventArgs(this.TotalRow, this.TotalError, this.TotalSuccess));
            }

        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message, ex);
        }
        finally
        {
            if (Finishing != null)
            {
                Finishing(this, EventArgs.Empty);
            }

            Deinitialize();
        }
    }
        
}
