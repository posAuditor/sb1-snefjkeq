using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.IO;
using Excel;


public partial class Comp_DownloadPos : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    

    #region Page events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    

  private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }



}