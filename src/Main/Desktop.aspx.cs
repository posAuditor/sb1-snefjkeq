using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.Text;
using System.IO;
using System.Web.Security;

public partial class Main_Desktop : UICulturePage
{
    protected MyContext MyLocalContext = new MyContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!User.Identity.IsAuthenticated) //FormsAuthentication.RedirectToLoginPage();
            {
                string jScript = "window.top.location.href = '" + PageLinks.LoginPageForce + "';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "forceParentLoad", jScript, true);
            }
            this.SetWorkingMode();
            
            if (!Page.IsPostBack)
            { 
                this.SetUserLock();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
    private void SetUserLock()
    {
        if (Cache.Get("_LoggedUsers") == null)
        {
            List<LoggedUser> lst = new List<LoggedUser>();
            Cache.Add("_LoggedUsers", lst, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60), System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }
        List<LoggedUser> LoggedUsersList = (List<LoggedUser>)Cache.Get("_LoggedUsers");

        var LoggedUsers = from data in LoggedUsersList
                          where data.UserID.ToLower().Trim() == MyLocalContext.UserProfile.UserId.ToString().ToLower().Trim()
                          select data;
        LoggedUsers.ToList().ForEach(x => LoggedUsersList.Remove(x));
        LoggedUser LoggedUser = new LoggedUser() { SessionID = Session.SessionID.ToLower().Trim(), UserID = MyLocalContext.UserProfile.UserId.ToString().ToLower().Trim(), LastRefresh = DateTime.Now };
        LoggedUsersList.Add(LoggedUser);
    }
    private void SetWorkingMode()
    {
        switch (MyLocalContext.Features.WorkingMode)
        {
            case (byte)WorkingMode.HR:
                //home_logo.Src = "~/Images/logo_hr.png";
                this.Title = "Auditor HR";
                break;
            case (byte)WorkingMode.Stores:
                //home_logo.Src = "~/Images/logo_stores.png";
                this.Title = "Auditor Stores";
                break;
            case (byte)WorkingMode.Xpress:
                // home_logo.Src = "~/Images/logo.png";
                this.Title = "Auditor Erp";
                break;
        }
    }
}