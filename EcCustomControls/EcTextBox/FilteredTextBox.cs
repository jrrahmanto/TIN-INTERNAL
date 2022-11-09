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

using AjaxControlToolkit;


namespace EcCustomControls.EcTextBox
{
    [DefaultProperty("Text"), ToolboxData("<{0}:FilteredTextBox runat=\"server\"></{0}:FilteredTextBox>")]
    public class FilteredTextBox : TextBox
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]

        private FilteredTextBoxExtender _filteredTextBoxExt;


        #region "-- Properties --"
        /// <summary>
        /// A the type of filter to apply, as a comma-separated combination of Numbers, 
        /// LowercaseLetters, UppercaseLetters, and Custom. If Custom is specified, 
        /// the ValidChars field will be used in addition to other settings such as Numbers.
        /// </summary>
        private FilterTypes _filterType = FilterTypes.Custom;
        public FilterTypes FilterType
        {
            get { return _filterType; }
            set { _filterType = value; }
        }

        /// <summary>
        /// A the filter mode to apply, either ValidChars (default) or InvalidChars. 
        /// If set to InvalidChars, FilterType must be set to Custom; if set to ValidChars, 
        /// FilterType must contain Custom.
        /// </summary>
        private FilterModes _filterMode = FilterModes.ValidChars;
        public FilterModes FilterMode
        {
            get { return _filterMode; }
            set { _filterMode = value; }
        }

        /// <summary>
        /// A string consisting of all characters considered valid for the text field, 
        /// if "Custom" is specified as the filter type. Otherwise this parameter is ignored.
        /// </summary>
        private string _validChar = "";
        public string ValidChar
        {
            get { return _validChar; }
            set { _validChar = value; }
        }

        /// <summary>
        /// A string consisting of all characters considered invalid for the text field, 
        /// if "Custom" is specified as the filter type and "InvalidChars" as the filter mode. 
        /// Otherwise this parameter is ignored.
        /// </summary>
        private string _invalidChar;
        public string InvalidChar
        {
            get { return _invalidChar; }
            set { _invalidChar = value; }
        }

        /// <summary>
        /// An integer containing the interval (in milliseconds) in which 
        /// the field's contents are filtered, defaults to 250.
        /// </summary>
        private int _filterInterval = 250;
        public int FilterInterval
        {
            get { return _filterInterval; }
            set { _filterInterval = value; }
        }

        /// <summary>
        /// Formatting textbox when blur event fired
        /// </summary>
        private EnumTextBoxFormat _filterTextBox = EnumTextBoxFormat.Normal;
        public EnumTextBoxFormat FilterTextBox
        {
            get { return _filterTextBox; }
            set { _filterTextBox = value; }
        }
        #endregion

        #region "-- Constuctor --"
        public FilteredTextBox()
        {


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            switch (_filterTextBox)
            {
                case EnumTextBoxFormat.Normal:
                    break;
                case EnumTextBoxFormat.Custom:
                    _filteredTextBoxExt = new FilteredTextBoxExtender();
                    _filteredTextBoxExt.TargetControlID = this.ID;
                    _filteredTextBoxExt.FilterType = _filterType;
                    _filteredTextBoxExt.FilterMode = _filterMode;
                    _filteredTextBoxExt.ValidChars = _validChar;
                    _filteredTextBoxExt.FilterInterval = _filterInterval;
                    this.Controls.Add(_filteredTextBoxExt);
                    break;
                case EnumTextBoxFormat.Money:
                    _filteredTextBoxExt = new FilteredTextBoxExtender();
                    _filteredTextBoxExt.TargetControlID = this.ID;
                    _filteredTextBoxExt.FilterType = FilterTypes.Custom | FilterTypes.Numbers;
                    _filteredTextBoxExt.FilterMode = FilterModes.ValidChars;
                    _filteredTextBoxExt.ValidChars = ".,-";
                    _filteredTextBoxExt.FilterInterval = _filterInterval;
                    this.Controls.Add(_filteredTextBoxExt);

                    SetMoneyFunction();
                    break;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            switch (_filterTextBox)
            {
                case EnumTextBoxFormat.Normal:
                    break;
                case EnumTextBoxFormat.Custom:
                    break;
                case EnumTextBoxFormat.Money:
                    this.Page.ClientScript.RegisterClientScriptInclude("FormatCurrency",
                    this.Page.ClientScript.GetWebResourceUrl(typeof(FilteredTextBox),
                        "EcCustomControls.js.FormatCurrency.js"));
                    break;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (_filteredTextBoxExt != null)
            {
                _filteredTextBoxExt.RenderControl(writer);
            }
        }
        #endregion

        #region "-- Supporting Method --"
        private void SetMoneyFunction()
        {
            this.Attributes.Add("onblur", "this.value=formatCurrency(this.value);");
        }
        #endregion
    }
}
