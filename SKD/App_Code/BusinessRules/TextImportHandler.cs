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
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

/// <summary>
/// Summary description for TextImportHandler
/// </summary>
public abstract class TextImportHandler : ImportHandler 
{
    private Type _recordType;
    private Type RecordType
    {
        get { return _recordType; }
    }

    private long _currentLine;
    public long CurrentLine
    {
        get { return _currentLine; }
        set { _currentLine = value; }
    }

    private string _filename;
    public string Filename
    {
        get { return _filename; }
        set { _filename = value; }
    }

    private StreamReader _streamReader;
    private StreamReader StreamReader
    {
        get { return _streamReader; }
        set { _streamReader = value; }
    }

    private string _line;
    private string Line
    {
        get { return _line; }
        set { _line = value; }
    }

    private string _delimiter;
    public string Delimiter
    {
        get { return _delimiter; }
        set { _delimiter = value; }
    }

    private bool _isImportParse = true;
    public bool IsImportParse
    {
        get { return _isImportParse; }
        set { _isImportParse = value; }
    }

	public TextImportHandler(Type recordType) 
	{
        if (recordType == null)
        {
            throw new ApplicationException("Record type cannot be empty.");
        }

        _recordType = recordType;
	}

    public override void Initialize()
    {  
        if (string.IsNullOrEmpty(this.Filename))
        {
            throw new ApplicationException("Filename cannot be empty.");
        }
        if (!File.Exists(this.Filename))
        {
            throw new ApplicationException("File not found.");
        }
        if (string.IsNullOrEmpty(this.Delimiter))
        {
            this.Delimiter = ",";
        }
        try
        {
            this.StreamReader = new StreamReader(this.Filename);
        }
        catch
        {
            throw new ApplicationException("Failed to create stream");
        }

        this.TotalProgress = this.StreamReader.BaseStream.Length;
        this.IsCancel = false;
    }
      
    public override bool IsFinish()
    {
        return this.StreamReader.Peek() < 0;
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
            this.Line = this.StreamReader.ReadLine();
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
            errMsg = string.Format("Error line {0} : {1}", this.CurrentLine, ex.Message);
            
            RaiseRowErrorEvent(obj, errMsg, this.CurrentLine, this.TotalError, this.Line);
        }
    }

    private void ImportParse()
    {
        this.TotalRow++;
        this.CurrentLine++;

        object obj = null;

        try
        {
            this.Line = this.StreamReader.ReadLine();
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

    //public override void ImportSuccess()
    //{
    //    //method called when import success
    //}

    //public override void ImportError(Exception ex)
    //{
    //    //method called when error
    //}

    public override void Deinitialize()
    {
        this.CurrentProgress = this.TotalProgress;

        if (StreamReader != null) StreamReader.Close();
    }

    private object LineToObject(string line)
    {
        object retObj = null;
        string delimiter;

        try
        {
            delimiter = this.Delimiter;

            if (line.Contains(delimiter))
            {
                retObj = Activator.CreateInstance(RecordType);
                FieldInfo[] fields = retObj.GetType().GetFields();
                string[] values = line.Split(delimiter.ToCharArray());
                DelimiterRecordAttribute delimiterProperty;
                for (int ii = 0; ii < fields.Length; ii++)
                {
                    FieldInfo field = fields[ii];
                    delimiterProperty = (DelimiterRecordAttribute)field.GetCustomAttributes(typeof(DelimiterRecordAttribute), false)[0];
                    
                    string typename = field.FieldType.Name;
                    string s;
                    if (typename == "Int16" || typename == "Int32" || typename == "Int64")
                    {
                        if (delimiterProperty.Ordinal != -1)
                        {                           
                            if (string.IsNullOrEmpty(delimiterProperty.Format))
                            {
                                s = values[delimiterProperty.Ordinal];
                            }
                            else
                            { 
                                s = int.Parse(values[delimiterProperty.Ordinal]).ToString(delimiterProperty.Format);
                            }
                            field.SetValue(retObj, int.Parse(s));
                        }
                    }
                    else if (typename == "String")
                    {
                        if (delimiterProperty.Ordinal != -1)
                        {
                            field.SetValue(retObj, values[delimiterProperty.Ordinal]);
                        }
                    }
                    else if (typename == "Boolean")
                    {
                        if (delimiterProperty.Ordinal != -1)
                        {
                            field.SetValue(retObj, bool.Parse(values[delimiterProperty.Ordinal]));
                        }
                    }
                    else if (typename == "Decimal")
                    {
                        if (delimiterProperty.Ordinal != -1)
                        {
                            if (string.IsNullOrEmpty(delimiterProperty.Format))
                            {
                                s = values[delimiterProperty.Ordinal];
                            }
                            else
                            {
                                s = decimal.Parse(values[delimiterProperty.Ordinal]).ToString(delimiterProperty.Format);
                            }
                            field.SetValue(retObj, decimal.Parse(s));
                        }                        
                    }
                    else if (typename == "DateTime")
                    {                        
                        if (delimiterProperty.Ordinal != -1)
                        {
                            if (string.IsNullOrEmpty(delimiterProperty.Format))
                            {
                                s = DateTime.Parse(values[delimiterProperty.Ordinal]).ToString();
                            }
                            else
                            {
                                s = DateTime.Parse(values[delimiterProperty.Ordinal]).ToString(delimiterProperty.Format);
                            }
                            field.SetValue(retObj,  DateTime.Parse(s));
                        }
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
            throw new ApplicationException();
        }
    }
}
