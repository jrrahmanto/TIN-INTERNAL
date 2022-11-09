using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using EcCustomControls.EcEnum;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;

using AjaxControlToolkit;


namespace EcCustomControls.EcPanel
{
    [DefaultProperty("Text"), ToolboxData("<{0}:ProgressUpdatePanel runat=\"server\"></{0}:ProgressUpdatePanel>")]
    public class ProgressUpdatePanel : UpdatePanel
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]

        private UpdatePanelAnimationExtender _updatePanelAnimationExt;
        private AnimationExtender _animationExt;
        private Animation _animation;
        private ModalPopupExtender _modalPopupExt;

        #region "-- Properties --"
        private EnumUpdatePanelType _updatePanelType = EnumUpdatePanelType.Update;
        public EnumUpdatePanelType UpdatePanelType
        {
            get { return _updatePanelType; }
            set { _updatePanelType = value; }
        }

        #endregion

        public ProgressUpdatePanel()
        { 
            
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            
        }

        Panel tbl;
        Image img;
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("<div id='divImage' style='display:none'>");
            //sb.AppendLine("<asp:Image ID='img1' runat='server' ImageUrl='~/images/ajax-loader2.gif' />");
            //sb.AppendLine("Processing...");
            //sb.AppendLine("</div>");
            ////this.ContentTemplateContainer.Page.Response.Write(sb.ToString());

            //HtmlGenericControl htm = new HtmlGenericControl();
            //tbl = new Panel();

            //tbl.ID = "tblProgressUpdatePanel";

            //img = new Image();
            //img.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(ProgressUpdatePanel), "EcCustomControls.Images.ajax-loader2.gif");
            //tbl.Controls.Add(img);

            //this.ContentTemplateContainer.Controls.Add(tbl);

            //HtmlTableRow tr = new HtmlTableRow();
            //HtmlTableCell td = new HtmlTableCell();
            
            //td.Controls.Add(img);
            //tr.Controls.Add(td);
            //tbl.Controls.Add(tr);
            
            //tbl.Visible = false;
            

            //if (!Page.ClientScript.IsStartupScriptRegistered("loadUpdatePanelScript"))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "loadUpdatePanelScript",
            //        string.Format("load({0});", tbl.ClientID), true);
            //}
            
            //this.ContentTemplateContainer.Controls.Add(imgBtn);
            //if (_updatePanelType == EnumUpdatePanelType.Update)
            //{
            //    this.UpdateMode = UpdatePanelUpdateMode.Always;

            //    _modalPopupExt = new ModalPopupExtender();
            //    _modalPopupExt.TargetControlID = this.ID;
            //    _modalPopupExt.PopupControlID = this.ID;
            //    _modalPopupExt.BackgroundCssClass = "modalBackground";
            //    _modalPopupExt.DropShadow = true;
            //    this.Controls.Add(_modalPopupExt);
            //    //_updatePanelAnimationExt = new UpdatePanelAnimationExtender();
            //    //_updatePanelAnimationExt.TargetControlID = this.ID;

            //    //_animation = new Animation();
            //    //_animation.Name = this.ID;
            //    //_updatePanelAnimationExt.Animations = this._animation.Name;
            //    //this.Controls.Add(_updatePanelAnimationExt);
            //}
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //this.Page.ClientScript.RegisterClientScriptInclude("UpdatePanelProgress",
            //this.Page.ClientScript.GetWebResourceUrl(typeof(ProgressUpdatePanel),
            //    "EcCustomControls.js.UpdatePanelProgress.js"));

            //if (!Page.ClientScript.IsStartupScriptRegistered("loadUpdatePanelScript"))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "loadUpdatePanelScript",
            //        string.Format("load({0});", tbl.ClientID), true);
            //}
        }

        protected override void Render(HtmlTextWriter writer)
        {
            

            base.Render(writer);

            //if (_modalPopupExt != null)
            //{
            //    _modalPopupExt.RenderControl(writer);
            //}


//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine(@"<div id=""tblImage"" style=""display:none"">");
//            sb.AppendLine(@"<asp:Image ID=""img1"" runat=""server"" ImageUrl=""~/images/ajax-loader2.gif"" />");
//            sb.AppendLine(@"Processing...");
//            sb.AppendLine(@"</div>");

////            writer.Write(sb.ToString());

//            StringWriter sw = new StringWriter();
//            sw.Write(sb.ToString());

//            WriteDiv(sw);
            //HtmlTextWriter txtWriter = new HtmlTextWriter(new textw

            //this.ContentTemplateContainer.RenderControl(writer);
        }
    }    
}
