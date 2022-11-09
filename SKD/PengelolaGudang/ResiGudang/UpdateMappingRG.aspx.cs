using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PengelolaGudang_ResiGudang_UpdateMappingRG : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void uiBtnDownload_Click(object sender, EventArgs e)
    {
        ShowFile(System.Configuration.ConfigurationManager.AppSettings[Common.SETTING_FOLDER_DOCUMENT_UPLOAD].ToString() + "TemplateResiGudang.xlsx");
    }

    protected void uiBtnUpload_Click(object sender, EventArgs e)
    {

    }

    private void ShowFile(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        
        if (file.Exists)
        {
            Response.ClearContent();            
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = ReturnExtension(file.Extension.ToLower());
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case "jpeg":
                return "image/jpeg";
            case ".bmp":
                return "image/bmp";
            case ".pdf":
                return "application/pdf";
            case ".txt":
                return "text/plain";
            case ".xls":
                return "application/vnd.ms-excel";
            case ".xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            default:
                return "application/octet-stream";
        }
    }
}