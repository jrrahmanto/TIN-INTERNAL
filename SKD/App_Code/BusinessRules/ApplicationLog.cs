using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

/// <summary>
/// Summary description for ApplicationLog
/// </summary>
public class ApplicationLog
{
    //get all data ApplicationLog from dataset to datagrid
    public static ApplicationLogData.ApplicationLogDataTable SelectApplicationLogByAll(ApplicationLogData.ApplicationLogDataTable dt, DateTime logTime)
    {
        ApplicationLogDataTableAdapters.ApplicationLogTableAdapter ta = new ApplicationLogDataTableAdapters.ApplicationLogTableAdapter();
        try
        {
            ta.FillByLogTime(dt, logTime);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to load application log data", ex);
        }

    }

    public static ApplicationLogData.ApplicationLogDataTable SelectApplicationLogByLogTime(DateTime logTime, string applicationModule, string classification, string userName)
    {
        ApplicationLogDataTableAdapters.ApplicationLogTableAdapter ta = new ApplicationLogDataTableAdapters.ApplicationLogTableAdapter();
        ApplicationLogData.ApplicationLogDataTable dt = new ApplicationLogData.ApplicationLogDataTable();

        try
        {
            ta.FillByApplicationLogAll(dt, logTime, applicationModule, classification, userName);
           // ta.FillByApplicationLogAll(dt, logTime, applicationModule, classification, userName);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load application log data", ex);
        }
    }

    public static void Insert(DateTime logTime, string applicationModule, string classification, string logMessage,
                              string userName, string sourceIP)
    {
        try
        {
            ApplicationLogDataTableAdapters.ApplicationLogTableAdapter ta = new ApplicationLogDataTableAdapters.ApplicationLogTableAdapter();
            
            ta.Insert(logTime, applicationModule, classification, logMessage, userName, sourceIP);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public static ApplicationLogData.vw_appl_moduleDataTable FillModule()
    {
        ApplicationLogDataTableAdapters.vw_appl_moduleTableAdapter ta = new ApplicationLogDataTableAdapters.vw_appl_moduleTableAdapter();
        ApplicationLogData.vw_appl_moduleDataTable dt = new ApplicationLogData.vw_appl_moduleDataTable();
        try
        {
            ta.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    /// <summary>
    /// log for debug
    /// </summary>
    /// <param name="message"></param>
    /// <param name="proses"></param>
    /// <returns></returns>
         
    public static void  CreateAndWriteLog(string message, string proses)
    {

        //create file name for log
        string appPath = "C:\\WebApps\\Logs\\";

        string FileNm = "EOD.log";
        string log = Path.Combine(appPath, "Log");
        string fileLog = string.Empty;


        try
        {


            //validate folder log
            if (!Directory.Exists(log))
                Directory.CreateDirectory(log);

            fileLog = Path.Combine(log, FileNm);


            //validate file is exists or not
            //if does not exists create file and write log
            //else append text

            if (!File.Exists(fileLog))
                File.Create(fileLog).Dispose();//Release all resource afte create file

            //if ((compareDate < systemDate && cod > systemDate))
            //{
            using (TextWriter tw = new StreamWriter(fileLog, true))
            {
                tw.WriteLine(String.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now) + "-->" + message);
                tw.Close();
            }

        }
        catch (Exception ex)
        {
            //CreateAndWriteLogSFTP("Error " + proses, "E");
            //CreateAndWriteLogSFTP("Inner " + ex.InnerException, "E");
            //CreateAndWriteLogSFTP("Stack " + ex.StackTrace, "E");
        }

        //return fileLog;

    }
}
