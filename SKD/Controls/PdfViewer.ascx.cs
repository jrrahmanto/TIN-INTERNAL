using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Controls_PdfViewer : System.Web.UI.UserControl
{
    private string _filepath;
    public string FilePath
    {
        get
        {
            return _filepath;
        }
        set
        {
            _filepath = value;
        }
    }   


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void RenderControl(HtmlTextWriter writer)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<iframe src=" + Convert.ToString(FilePath) + " ");
            sb.Append("width=800px height=600px");
            sb.Append("<View PDF: <a href=" + Convert.ToString(FilePath) + "</a></p> ");
            sb.Append("</iframe>");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(Convert.ToString(sb));
            writer.RenderEndTag();
        }
        catch (Exception ex)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(ex.Message);
            writer.RenderEndTag();
        }
    }
    
}
