using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Data.OleDb;

/// <summary>
/// Summary description for ExcelImportHandler
/// </summary>
public abstract class ExcelImportHandler : ImportHandler
{
    Type _recordType;
    private Type RecordType
    {
        get { return _recordType; }
    }

    private string _excelSheetName;
    public string ExcelSheetName
    {
        get { return _excelSheetName; }
        set { _excelSheetName = value; }
    }

    private string _filename;
    public string Filename
    {
        get { return _filename; }
        set { _filename = value; }
    }

    private OleDbDataReader _reader;
    private OleDbDataReader Reader
    {
        get { return _reader; }
        set { _reader = value; }
    }

    private OleDbConnection _conn;
    private OleDbConnection Conn
    {
        get { return _conn; }
        set { _conn = value; }
    }

    private long _currentLine;
    public long CurrentLine
    {
        get { return _currentLine; }
        set { _currentLine = value; }
    }

    private string _line;
    private string Line
    {
        get { return _line; }
        set { _line = value; }
    }

    private StreamReader _streamReader;
    private StreamReader StreamReader
    {
        get { return _streamReader; }
        set { _streamReader = value; }
    }

    private string _delimiter;
    private string Delimiter
    {
        get { return _delimiter; }
        set { _delimiter = value; }
    }

    private bool _isHeaderRowData;
    public bool IsHeaderRowData
    {
        get { return _isHeaderRowData; }
        set { _isHeaderRowData = value; }
    }

    private bool _isImportParse = true;
    public bool IsImportParse
    {
        get { return _isImportParse; }
        set { _isImportParse = value; }
    }

    public ExcelImportHandler(Type recordType)
    {
        if (recordType == null)
        {
            throw new ApplicationException("Record type cannot be empty.");
        }
        
        _recordType = recordType;
    }

    public override void Initialize()
    {
        //Initialize
        this.Delimiter = "\t";

        OleDbCommand cmd;
        string hdr;

        this.TotalProgress = File.ReadAllLines(this.Filename).Length;

        if (string.IsNullOrEmpty(ExcelSheetName))
        {
            ExcelSheetName = "Sheet1$";
        }
        if (IsHeaderRowData)
        {
            hdr = "NO";
        }
        else
        {
            hdr = "YES";
        }

        this.Conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + 
            this.Filename + "\";Extended Properties='Excel 8.0;IMEX=1;HDR=" + hdr + "';");
        cmd = new OleDbCommand("select * from [" + ExcelSheetName + "]", this.Conn);

        try
        {
            this.Conn.Open();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to open connection: " + ex.Message, ex);
        }
        try
        {
            this.Reader = cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to read: " + ex.Message, ex);
        }
    }

    public override bool IsFinish()
    {
        return !this.Reader.Read();
    }

    public override void StartImport()
    {
        if (IsImportParse == true)
        {
            ImportParse();
        }
        else
        {
            ImportNonParse();
        }
    }

    private void ImportNonParse()
    {
        this.TotalRow++;
        this.CurrentLine++;

        object obj = null;

        try
        {
            StringBuilder sb = new StringBuilder();
            for (int iiCol = 0; iiCol < this.Reader.FieldCount; iiCol++)
            {
                if (iiCol != 0)
                {
                    sb.Append(this.Delimiter);
                }
                if (this.Reader.IsDBNull(iiCol))
                {
                    continue;
                }
                sb.Append(this.Reader[iiCol]);
            }

            sb.Append("\r\n");

            this.Line = sb.ToString();
            this.CurrentProgress = this.Line.Length;

            obj = this.Line;

            ProcessRow(obj);

            RaiseRowValidEvent(obj);

            this.TotalSuccess++;
        }
        catch (Exception ex)
        {
            this.TotalError++;

            string errMsg = "";
            string exMessage = ex.Message;
            if (ex.Message.Contains("SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM"))
            {
                exMessage = "Date is not in valid date format.";
            }
           
            errMsg = string.Format("Error line {0} : {1}", this.CurrentLine + 1, exMessage);
            RaiseRowErrorEvent(obj, errMsg, this.CurrentLine + 1, this.TotalError, this.Line);
        }
    }


    private void ImportParse()
    {
        this.TotalRow++;
        this.CurrentLine++;

        object obj = null;

        try
        {
            StringBuilder sb = new StringBuilder();
            for (int iiCol = 0; iiCol < this.Reader.FieldCount; iiCol++)
            {
                if (iiCol != 0)
                {
                    sb.Append(this.Delimiter);
                }
                if (this.Reader.IsDBNull(iiCol))
                {
                    continue;
                }
                sb.Append(this.Reader[iiCol]);
            }

            sb.Append("\r\n");

            this.Line = sb.ToString();
            this.CurrentProgress = this.Line.Length;

            obj = LineToObject(this.Line);

            ProcessRow(obj);

            RaiseRowValidEvent(obj);

            this.TotalSuccess++;
        }
        catch (Exception ex)
        {
            this.TotalError++;

            string errMsg = "";
            errMsg = string.Format("Error line {0} : {1}", this.CurrentLine, ex.Message);

            RaiseRowErrorEvent(obj, errMsg, this.CurrentLine, this.TotalError, this.Line);
        }
    }

    public override void Deinitialize()
    {
        //Deinitialize;
        CurrentProgress = TotalProgress;

        if (Conn != null) Conn.Close();
        if (Reader != null) Reader.Close();
    }

    public object LineToObject(string line)
    {
        object retObj = null;
        string delimiter;
        string fieldName = "";
        string dataType = "";
        try
        {
            delimiter = this.Delimiter;

            if (line.Contains(delimiter))
            {
                retObj = Activator.CreateInstance(RecordType);
                FieldInfo[] fields = retObj.GetType().GetFields();
                string[] values = line.Split(delimiter.ToCharArray());
                for (int ii = 0; ii < fields.Length; ii++)
                {
                    FieldInfo field = fields[ii];
                    string typename = field.FieldType.Name;
                    fieldName = field.Name;
                    dataType = typename;
                    if (typename == "Int16" || typename == "Int32" || typename == "Int64")
                    {
                        field.SetValue(retObj, int.Parse(values[ii]));
                    }
                    else if (typename == "String")
                    {
                        field.SetValue(retObj, values[ii]);
                    }
                    else if (typename == "Boolean")
                    {
                        field.SetValue(retObj, bool.Parse(values[ii]));
                    }
                    else if (typename == "Decimal")
                    {
                        //field.SetValue(retObj, decimal.Parse(values[ii]));
                        field.SetValue(retObj, decimal.Parse(values[ii], System.Globalization.NumberStyles.Float)); 
                    }
                    else
                    {
                        throw new ApplicationException("Data type not supported.");
                    }
                }
            }
            else
            {
                throw new ApplicationException("Delimiter not matched.");
            }

            return retObj;
        }
        catch
        {
            throw new ApplicationException("Failed to convert line to object. Field: " + fieldName + ". Data Type: " + dataType + ". Value: ");
        }
    }

}
