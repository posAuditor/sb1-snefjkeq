using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Comp_CurrentDocs : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtCurrentDocs
    {
        get
        {
            return (DataTable)Session["dtCurrentDocs" + this.WinID];
        }

        set
        {
            Session["dtCurrentDocs" + this.WinID] = value;
        }
    }

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.Fill();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region Control Events

    protected void gvDocs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDocs.PageIndex = e.NewPageIndex;
            gvDocs.DataSource = this.dtCurrentDocs;
            gvDocs.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    #endregion

    #region Private Methods

    private void Fill()
    {
        this.dtCurrentDocs = dc.usp_CurrentDocuments_Select(MyContext.CurrentCulture.ToByte(), MyContext.UserProfile.Branch_ID).CopyToDataTable();
        gvDocs.DataSource = this.dtCurrentDocs;
        gvDocs.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
    }

    #endregion
}