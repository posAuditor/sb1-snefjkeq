using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class HR_EmployeesList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtEmployeesList
    {
        get
        {
            return (DataTable)Session["dtEmployeesList" + this.WinID];
        }

        set
        {
            Session["dtEmployeesList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvEmployeesList.Columns[6].Visible = this.MyContext.PageData.IsViewDoc;
                gvEmployeesList.Columns[7].Visible = this.MyContext.PageData.IsDelete;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillEmployeesList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillEmployeesList();
            if (acBranch.Enabled) acBranch.AutoCompleteFocus(); else txtSerialsrch.Focus();
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
            txtSerialsrch.Clear();
            acDepartment.Clear();
            acPosition.Clear();
            acName.Clear();
            ddlTerminationStatus.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            this.acDepartment_SelectedIndexChanged(null, null);
            this.FilterEmployees(null, null);
            this.FillEmployeesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployeesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvEmployeesList.PageIndex = e.NewPageIndex;
            gvEmployeesList.DataSource = this.dtEmployeesList;
            gvEmployeesList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployeesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = dc.usp_HR_Employees_Delete(gvEmployeesList.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvEmployeesList.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillEmployeesList();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acPosition.Clear();
            acPosition.Enabled = acDepartment.HasValue;
            acPosition.ContextKey = string.Empty + acDepartment.Value;
            this.FilterEmployees(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterEmployees(object sender, EventArgs e)
    {
        try
        {
            acName.ContextKey = acDepartment.Value + ",," + acBranch.Value;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void lnkChangeState_Click(object sender, EventArgs e)
    {
        int ID = (sender as LinkButton).CommandArgument.ToIntOrDefault();
        var lstHrEmploye = dc.HR_Employees.Where(x => x.Contact_ID == ID).ToList();
        if (lstHrEmploye.Any())
        {
            var emp = lstHrEmploye.First();
            if (emp != null)
            {
                if (emp.IsStoped.ToBooleanOrDefault() == true || emp.IsStoped == null)
                {
                    emp.IsStoped = false;
                }
                else
                {
                    emp.IsStoped = true;
                }
            }
            dc.SubmitChanges();
            FillEmployeesList();
        }
    }

    #endregion

    #region Private Methods

    private void FillEmployeesList()
    {
        byte? TerminationStatus = ddlTerminationStatus.SelectedIndex == 0 ? (byte?)null : ddlTerminationStatus.SelectedValue.ToByte();
        this.dtEmployeesList = dc.usp_HR_EmployeesNew_Select(acBranch.Value.ToNullableInt(), txtSerialsrch.TrimmedText, acName.Text, acDepartment.Value.ToNullableInt(), acPosition.Value.ToNullableInt(), TerminationStatus, txtNationalIDSearsh.TrimmedText).CopyToDataTable();
        gvEmployeesList.DataSource = this.dtEmployeesList;
        gvEmployeesList.DataBind();
    }

    public string SetNameStoped(string value)
    {
        if (value.ToBooleanOrDefault())
        {
            return "توقيف";
        }
        else
        {
            return "تفعيل";
        }
    }

    private void LoadControls()
    {
        acDepartment.ContextKey = string.Empty;

        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvEmployeesList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion


}