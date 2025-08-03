using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_DepEmpSystems : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtDepEmpSystems
    {
        get
        {
            return (DataTable)Session["dtDepEmpSystems" + this.WinID];
        }

        set
        {
            Session["dtDepEmpSystems" + this.WinID] = value;
        }
    }

    private int EditID
    {
        get
        {
            if (ViewState["EditID"] == null) return 0;
            return (int)ViewState["EditID"];
        }

        set
        {
            ViewState["EditID"] = value;
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
            acDepartmentSrch.Clear();
            acEmployeeSrch.Clear();
            acSystemSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSystems.PageIndex = e.NewPageIndex;
            gvSystems.DataSource = this.dtDepEmpSystems;
            gvSystems.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtDepEmpSystems.Select("ID=" + gvSystems.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            ddlType.SelectedValue = dr["Department_ID"].ToExpressString() == string.Empty ? "1" : "0";
            this.ddlType_SelectedIndexChanged(null, null);

            acEmployeeOrDepartment.Value = dr["Department_ID"].ToExpressString() == string.Empty ? dr["Contact_ID"].ToExpressString() : dr["Department_ID"].ToExpressString();
            ddlSystemType.SelectedValue = dr["SystemType_ID"].ToExpressString();
            this.ddlSystemType_SelectedIndexChanged(null, null);
            acSystem.Value = dr["System_ID"].ToExpressString();
            this.EditID = gvSystems.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_DepartmentsSystems_Delete(gvSystems.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvSystems.DataKeys[e.RowIndex]["SystemName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            acSystem.Clear();
            acEmployeeOrDepartment.Clear();
            this.EditID = 0;
            mpeCreateNew.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;

            int? Department_ID = ddlType.SelectedValue == "0" ? acEmployeeOrDepartment.Value.ToInt() : (int?)null;
            int? Contact_ID = ddlType.SelectedValue == "1" ? acEmployeeOrDepartment.Value.ToInt() : (int?)null;
            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_DepartmentsSystems_Insert(Department_ID, Contact_ID, acSystem.Value.ToInt());
            }
            else
            {
                result = dc.usp_HR_DepartmentsSystems_Update(this.EditID, Department_ID, Contact_ID, acSystem.Value.ToInt());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.SystemAlreayExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acEmployeeOrDepartment.Text , dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            acSystem.Clear();
            acEmployeeOrDepartment.Clear();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlType.SelectedValue == "0")
            {
                acEmployeeOrDepartment.ServiceMethod = "GetHRDepartments";
                acEmployeeOrDepartment.ContextKey = string.Empty;
            }
            else
            {
                acEmployeeOrDepartment.ServiceMethod = "GetContactNames";
                acEmployeeOrDepartment.ContextKey = "E,,,";
            }
            if (sender != null) mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlSystemType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acSystem.ContextKey = ddlSystemType.SelectedIndex == 0 ? string.Empty : ddlSystemType.SelectedValue;
            if (sender != null) mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
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
        acSystem.ContextKey = acSystemSrch.ContextKey = acDepartmentSrch.ContextKey = string.Empty;
        acEmployeeSrch.ContextKey = "E,,,";
        this.ddlType_SelectedIndexChanged(null, null);
    }

    private void Fill()
    {
        this.dtDepEmpSystems = dc.usp_HR_DepartmentsSystems_Select(acDepartmentSrch.Value.ToNullableInt(), acEmployeeSrch.Value.ToNullableInt(), acSystemSrch.Value.ToNullableInt()).CopyToDataTable();
        gvSystems.DataSource = this.dtDepEmpSystems;
        gvSystems.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvSystems.Columns[3].Visible = MyContext.PageData.IsEdit;
        gvSystems.Columns[4].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}