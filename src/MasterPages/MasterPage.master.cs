using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using XPRESS.Common;
using System.Web.Security;

public partial class Items_CategoriesData : System.Web.UI.MasterPage, IMyMasterPage
{
    XpressDataContext dc = new XpressDataContext();

    public MyContext MyContext
    {
        get;
        set;
    }



    private string _Req_ID = null;

    public string Req_ID
    {
        get { return _Req_ID; }
        set { _Req_ID = value; }
    }


    public bool OverrideUserLock
    {
        get
        {
            return (Request.AppRelativeCurrentExecutionFilePath == PageLinks.CompanySettings) && Request.QueryString["StartNewYear"] != null;
        }
    }

    protected string PreferedCulture
    {
        get
        {
            if (Session["PreferedCulture"] == null) return string.Empty;
            return Session["PreferedCulture"].ToString();
        }
    }

    protected string CurrentUser
    {
        get
        {
            return this.Page.User.Identity.Name;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.AddJQuery();
            lnkAttachments.Attributes.Add("DocumentURI", Request.AppRelativeCurrentExecutionFilePath + Request.Url.Query);
            lnkAttachments.Attributes.Add("DocumentPath", Request.AppRelativeCurrentExecutionFilePath);
            lnkAttachments.Attributes.Add("DocumentPathInfo", Request.PathInfo);
            lnkAttachments.Visible = this.MyContext.PageData.IsAttach && (this.Req_ID != null || Request["ID"] != null) && Request["ViewInPopupMode"] == null;
            if (lnkAttachments.Visible) lnkAttachments.Text += string.Format("({0})", dc.usp_Attachments_Select(Request.AppRelativeCurrentExecutionFilePath + Request.Url.Query).Count());

            if (!Page.IsPostBack)
            {
                //ItemsDetailss C = new ItemsDetailss();
                //ToDO Lisence Commanted
                // C.c();

                //var comp = dc.usp_Company_Select().FirstOrDefault();
                ////this.CheckUserLock();
                //if (comp.DepreciationAutomatic.ToBooleanOrDefault())
                //{
                //    this.CalcAssetsDep();
                //}




            }
            this.HideControls();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void AddJQuery()
    {
        var script = new HtmlGenericControl("script");
        script.Attributes.Add("type", "text/javascript");
        script.Attributes.Add("src", Page.ResolveClientUrl("~/Scripts/jquery-1.9.1.min.js"));
        this.Page.Header.Controls.Add(script);
        if (Page.IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "IsPostBack", "var isPostBack = true;", true);
        }
    }

    private void CheckUserLock()
    {
        //User Locks
        if (HttpRuntime.Cache.Get("_LoggedUsers") == null)
        {
            List<LoggedUser> lst = new List<LoggedUser>();
            HttpRuntime.Cache.Add("_LoggedUsers", lst, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60), System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }
        List<LoggedUser> LoggedUsersList = (List<LoggedUser>)HttpRuntime.Cache.Get("_LoggedUsers");

        var LoggedUsers = from data in LoggedUsersList
                          where data.UserID.ToLower().Trim() == MyContext.UserProfile.UserId.ToString().ToLower().Trim()
                          select data;
        if (LoggedUsers.Count() == 0)
        {
            if (!this.OverrideUserLock)
            {
                FormsAuthentication.SignOut();
               // FormsAuthentication.RedirectToLoginPage();
                string jScript = "window.top.location.href = '" + PageLinks.LoginPageForce + "';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "forceParentLoad", jScript, true);
                
                return;
            }
        }

        if (LoggedUsers.Count() != 1 || LoggedUsers.FirstOrDefault().SessionID.ToLower().Trim() != Session.SessionID.ToLower().Trim())
        {
            if (!this.OverrideUserLock)
            {
                // FormsAuthentication.SignOut();
                //  FormsAuthentication.RedirectToLoginPage("UserAlreadyInUse=1");
            }
        }

    }

    private void HideControls()
    {
        string HiddenControls = string.Empty;
        string ControlClient_ID = string.Empty;
        if (Page.IsPostBack && Request.Params.Get("__EVENTTARGET") == "xxHideControlxx")
        {
            ControlClient_ID = Request.Params.Get("__EVENTARGUMENT");
            dc.usp_HiddenControls_Insert(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID, ControlClient_ID);
        }
        var flag = false;
        foreach (var Control in dc.usp_HiddenControls_Select(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID))
        {
            HiddenControls += string.Format("[id*='{0}'],", Control.ControlUniqueID);
            if (Control.ControlUniqueID == "cph_txtCapacities")
            {
                flag = true;
            }
           
        }
        if (HiddenControls == string.Empty) return;
        HiddenControls = HiddenControls.Remove(HiddenControls.Length - 1);
        string loadEvent = this.Page.IsPostBack ? "$(document).ready" : "$(window).load";
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "HideControlsScript", string.Format("{0}(function() {{$(\"{1}\").parents('span').hide();}})\r\n", loadEvent, HiddenControls), true);
        var str = flag ? "$('table.forHide').hide();" : "";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "HideControlsScript", string.Format("{0}(function() {{   $(\"{1}\").hide();$(\"{1}\").next('.PlusBtn').hide();$(\"{1}\").prev('.forHide').hide(); {2}  }})\r\n", loadEvent, HiddenControls, str), true);
   
    
    
    }

    private void CalcAssetsDep()
    {
        var NextDepDate = dc.usp_Company_Select().FirstOrDefault().NextAssetDepDate;
        if (DateTime.Now.Date < NextDepDate) return;

        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            dc.usp_AllAssetsDep_Calc(NextDepDate);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}
