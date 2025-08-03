using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;


public class AutoCompleteEventArgs : EventArgs
{
    private string Value;

    private string Text;


    public AutoCompleteEventArgs(string Text, string Value)
    {
        this.Value = Value;
        this.Text = Text;
    }
}

public partial class CustomControls_ucAutocomplete : System.Web.UI.UserControl
{

    #region Member Fields

    bool Initialized = false;

    public delegate void EventHandler(object sender, AutoCompleteEventArgs e);

    public event EventHandler SelectedIndexChanged;

    private bool SelectedIndexChangedFired;

    private string _ContextKey = "@EMPTY";

    private string _ServicePath = string.Empty;

    private string _ServiceMethod = string.Empty;

    private bool _AutoPostBack = false;

    private bool _Enabled = true;

    private bool _IsException = false;


    private int _Count = 500;

    private bool _InlineRender = true;

    private bool _IgnoreCache = false;

    private string _OnClientSelectedIndexChanged = string.Empty;

    private bool _KeepTextWhenNoValue = false;

    private string _LabelText = string.Empty;

    private bool _VisibleText = true;

    private string _ControlName = string.Empty;

    #endregion

    #region Public Properties

    public string AutoCompleteWidth
    {
        set
        {
            if (value != string.Empty)
            {
                txtAutoCompleteText.Width = short.Parse(value);
            }
        }
    }

    public string Value
    {
        get
        {
            if (txtAutoCompleteText.Text.Trim() == string.Empty && hfAutocomplete.Text.Trim() == string.Empty)
            {
                return null;
            }
            else if (txtAutoCompleteText.Text.Trim() != string.Empty && hfAutocomplete.Text.Trim() != string.Empty)
            {
                return hfAutocomplete.Text;
            }
            else if (txtAutoCompleteText.Text.Trim() != string.Empty && hfAutocomplete.Text.Trim() == string.Empty)
            {
                return null;
            }
            else if (txtAutoCompleteText.Text.Trim() == string.Empty && hfAutocomplete.Text.Trim() != string.Empty)
            {
                return hfAutocomplete.Text;
            }
            else
            {
                return string.Empty;
            }
        }

        set
        {
            if (hfAutocomplete.Text != value) _IgnoreCache = true;
            hfAutocomplete.Text = value;            
        }
    }

    public string ServicePath
    {
        get
        {
            if (_ServicePath == string.Empty) return this.Page.ResolveUrl("~/WebServices/wsAutoComplete.asmx");
            return _ServicePath;
        }
        set
        {
            _ServicePath = value;
        }
    }

    public string ServiceMethod
    {
        get
        {

            return _ServiceMethod;
        }
        set
        {
            _ServiceMethod = value;
        }
    }

    public string ContextKey
    {
        get
        {
            return _ContextKey;
        }

        set
        {
            if (_ContextKey != value)
            {
                _IgnoreCache = true;
                this.Clear();
            }
            _ContextKey = value;

        }
    }

    public bool AutoPostBack
    {
        get
        {
            return _AutoPostBack;
        }
        set
        {
            _AutoPostBack = value;
        }
    }

    public string TabIndex
    {
        get
        {
            return txtAutoCompleteText.TabIndex.ToString();
        }
        set
        {

            if (value != string.Empty)
            {
                txtAutoCompleteText.TabIndex = short.Parse(value);
            }
        }
    }

    public bool Enabled
    {
        get
        {
            return _Enabled;
        }
        set
        {
            _Enabled = value;
            txtAutoCompleteText.Enabled = value;
        }
    }

    public bool IsException
    {
        get
        {
            return _IsException;
        }
        set
        {
            _IsException = value;
        }
    }



    public bool InlineRender
    {
        get
        {
            return _InlineRender;
        }
        set
        {
            _InlineRender = value;
        }
    }

    public string HiddenFieldClientID
    {
        get { return hfAutocomplete.ClientID; }
    }

    public string AutoCompleteTextClientID
    {
        get { return txtAutoCompleteText.ClientID; }
    }

    public string Text
    {
        get { return txtAutoCompleteText.Text.Trim(); }
    }

    public int Count
    {
        get
        {
            return _Count;
        }
        set
        {
            _Count = value;
        }
    }

    public string ErrorMessage
    {
        get
        {
            return rfvAutoComplete.ErrorMessage;
        }
        set
        {
            rfvAutoComplete.ErrorMessage = value;
        }
    }

    public string ValidationGroup
    {
        get
        {
            return rfvAutoComplete.ValidationGroup;
        }
        set
        {
            rfvAutoComplete.ValidationGroup = value;
        }
    }

    public bool IsRequired
    {
        get
        {
            return rfvAutoComplete.Enabled;
        }
        set
        {
            rfvAutoComplete.Enabled = value;
        }
    }

    public bool IgnoreCache
    {
        get
        {
            return this._IgnoreCache;
        }
        set
        {
            this._IgnoreCache = value;
        }
    }

    public string CssClass
    {
        get
        {
            return txtAutoCompleteText.CssClass;
        }
        set
        {
            txtAutoCompleteText.CssClass = value;
        }
    }

    public bool HasValue
    {
        get
        {
            return (this.Value != null);
        }
    }

    public string OnClientSelectedIndexChanged
    {
        get
        {
            return this._OnClientSelectedIndexChanged;
        }
        set
        {
            this._OnClientSelectedIndexChanged = value;
        }
    }

    public bool KeepTextWhenNoValue
    {

        get
        {
            return this._KeepTextWhenNoValue;
        }
        set
        {
            this._KeepTextWhenNoValue = value;
        }
    }

    public string LabelText
    {

        get
        {
            return this._LabelText;
        }
        set
        {
            this._LabelText = value;
        }
    }

    public bool IsHideable
    {
        get
        {
            return _xxHideControlxx.Visible;
        }
        set
        {
            _xxHideControlxx.Visible = value;
        }
    }

    public bool VisibleText
    {

        get
        {
            return this._VisibleText;
        }
        set
        {
            this._VisibleText = value;
        }
    }


    public bool IsBtnPlus
    {
        get
        {
            return _xxPlusBtnxx.Visible;
        }
        set
        {
            _xxPlusBtnxx.Visible = value;
        }
    }
    public string ControlName
    {
        get
        {
            return _ControlName;
        }
        set
        {
            _ControlName = value;
        }
    }



    public string PlaceHolder
    {
        set { txtAutoCompleteText.Attributes["placeholder"] = value;}        
    }
    #endregion



    //private short _tabIndex;
    //public short TabIndex
    //{
    //    get
    //    {
    //        return _tabIndex;
    //    }
    //    set
    //    {
    //        _tabIndex = value;
    //        txtAutoCompleteText.TabIndex = (short)(value++);
    //    }
    //}


    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params.Get("__EVENTTARGET") == txtAutoCompleteText.ClientID && Request.Params.Get("__EVENTARGUMENT") == "SelectedIndexChanged")
        {
            TxtAutoCompleteText_TextChanged(this, null);
        }
        this.txtAutoCompleteText.Attributes.Add("onfocus", "javascript:this.select();");
        //this.txtAutoCompleteText.Attributes.Add("propertychange", "javascript:alert(1);");
       
        //_xxPlusBtnxx.Attributes.Add("onClick", "return false;");
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.Initialize();

        if (this.InlineRender)
        {
            AutoCompleteMainContainer.Style.Add(HtmlTextWriterStyle.Display, "inline");
        }

        this.ErrorMessage = (this.ErrorMessage == string.Empty && IsRequired) ? string.Format(Resources.Validations.rfvSelect, this.LabelText) : this.ErrorMessage;
        if (this.VisibleText)
        {
            this.label.Visible = this.LabelText != string.Empty;
        }
        else
        {
            this.label.Visible = false;
        }

        if (this.IsHideable)
        {
            if (this.Page is UICulturePage) txtAutoCompleteText.Style.Add(((UICulturePage)this.Page).MyContext.CurrentCulture == ABCulture.Arabic ? HtmlTextWriterStyle.PaddingLeft : HtmlTextWriterStyle.PaddingRight, "40px;");
            _xxHideControlxx.Attributes.Add("onclick", "return HideControl('" + AutoCompleteMainContainer.ClientID + "');");
            _xxHideControlxx.Attributes.Add("title", Resources.Labels.Hide);
            _xxHideControlxx.Attributes.Add("tabindex", "-1");
        }

        if (this.IsBtnPlus)
        { 
            var SControl = "'cph_" + _ControlName + "'";

            _xxPlusBtnxx.Attributes.Add("onclick", "document.getElementById(" + SControl + ").click();return false;");
            _xxPlusBtnxx.Attributes.Add("title", Resources.Labels.AddNew);
            _xxPlusBtnxx.Attributes.Add("tabindex", "-1");
           
        }




        //if (this.IsBtnPlus)
        //{ 
        //    var SControl = "#cph_" + _ControlName + "";
             
        //    btnPlus.Attributes.Add("onclick", "$('" + SControl + "').show();$('" + SControl + "').css({'position':'fixed','z-index':' 15','top':'50%','left':'50%','margin':'-100px 0 0 -150px'});return false;");
        //    btnPlus.Attributes.Add("title", Resources.Labels.Hide);
        //    btnPlus.Attributes.Add("tabindex", "-1");
            
        //}


    }

    protected override object SaveControlState()
    {
        object[] controlState = new object[9];
        controlState[0] = base.SaveControlState();
        controlState[1] = _ContextKey;
        controlState[2] = _Enabled;
        controlState[3] = _AutoPostBack;
        controlState[4] = _Count;
        controlState[5] = _InlineRender;
        controlState[6] = _ServiceMethod;
        controlState[7] = _OnClientSelectedIndexChanged;
        controlState[8] = _LabelText;

        return controlState;
    }

    protected override void LoadControlState(object savedState)
    {
        object[] controlState = (object[])savedState;
        base.LoadControlState(controlState[0]);
        _ContextKey = controlState[1].ToString();
        _Enabled = Convert.ToBoolean(controlState[2]);
        _AutoPostBack = Convert.ToBoolean(controlState[3]);
        _Count = Convert.ToInt32(controlState[4]);
        _InlineRender = Convert.ToBoolean(controlState[5]);
        _ServiceMethod = Convert.ToString(controlState[6]);
        _OnClientSelectedIndexChanged = Convert.ToString(controlState[7]);
        _LabelText = Convert.ToString(controlState[8]);
    }

    protected override void OnInit(EventArgs e)
    {
        Page.RegisterRequiresControlState(this);
        base.OnInit(e);
    }

    #endregion

    #region Public Methods

    public void Clear()
    {
        txtAutoCompleteText.Text = string.Empty;
        hfAutocomplete.Text = string.Empty;
    }

    public void AutoCompleteFocus()
    {
        txtAutoCompleteText.Focus();
    }

    public void EnableControls(bool flag)
    {
        this.Enabled = flag;
    }

    public void Refresh()
    {
        this._IgnoreCache = true;
    }

    #endregion

    #region Controls Events

    protected void TxtAutoCompleteText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (this.SelectedIndexChanged != null && !SelectedIndexChangedFired)
            {
                AutoCompleteEventArgs args = new AutoCompleteEventArgs(this.Text, this.Value);
                SelectedIndexChangedFired = true;
                this.SelectedIndexChanged(sender, args);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        if (!this.Visible || this.Initialized || this.ContextKey == "@EMPTY") return;
        string loadEvent = this.Page.IsPostBack ? "$(document).ready" : "$(window).load";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.UniqueID + "MakeAutocomplete", loadEvent + "(function() {$('#' + '" + txtAutoCompleteText.ClientID + "').XpressAutocomplete({txtAutocompleteID: '" + txtAutoCompleteText.ClientID +
            "',hfAutocompleteID: '" + hfAutocomplete.ClientID +
            "',AutocompleteListContainerID: '" + AutocompleteListContainer.ClientID +
            "',AutoCompleteServicePath:'" + ServicePath +
            "',AutoCompleteServiceMethod:'" + ServiceMethod +
             "',ContextKey:'" + ContextKey +
              "',Enabled:'" + Enabled +
               "',Count:'" + Count.ToString() +
               "',AutoPostBack:'" + AutoPostBack +
                "',TabIndex:'" + TabIndex +
               "',IgnoreCache:'" + IgnoreCache +
                "',IsException:'" + IsException +
               "',OnClientSelectedIndexChanged:'" + OnClientSelectedIndexChanged +
               "',KeepTextWhenNoValue:'" + KeepTextWhenNoValue +
            "',UniqueID:'" + this.UniqueID + "'});});", true);
        Initialized = true;
    }

    #endregion


}
