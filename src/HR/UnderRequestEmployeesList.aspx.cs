using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_UnderRequestEmployeesList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtUnderRequestEmployees
    {
        get
        {
            return (DataTable)Session["dtUnderRequestEmployees" + this.WinID];
        }

        set
        {
            Session["dtUnderRequestEmployees" + this.WinID] = value;
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
                this.LoadControls();
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtNameSrch.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtNameSrch.Clear();
            txtDateFrom.Clear();
            txtDateTo.Clear();
            acPositionSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvEmployees.PageIndex = e.NewPageIndex;
            gvEmployees.DataSource = this.dtUnderRequestEmployees;
            gvEmployees.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployees_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_EmployeesUnderRequest_Delete(gvEmployees.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvEmployees.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void LoadControls()
    {
        acPositionSrch.ContextKey = string.Empty;

    }

    private void Fill()
    {
        this.dtUnderRequestEmployees = dc.usp_HR_EmployeesUnderRequest_Select(null,txtNameSrch.TrimmedText,txtDateFrom.Text.ToDate(),txtDateTo.Text.ToDate(),acPositionSrch.Value.ToNullableInt()).CopyToDataTable();
        gvEmployees.DataSource = this.dtUnderRequestEmployees;
        gvEmployees.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
        gvEmployees.Columns[6].Visible = MyContext.PageData.IsViewDoc;
        gvEmployees.Columns[7].Visible = MyContext.PageData.IsDelete;
        lnkadd.Visible = this.MyContext.PageData.IsAdd;
    }

    #endregion
}