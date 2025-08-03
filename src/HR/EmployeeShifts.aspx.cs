using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_EmployeeShifts : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtEmployeeShifts
    {
        get
        {
            return (DataTable)Session["dtEmployeeShifts" + this.WinID];
        }

        set
        {
            Session["dtEmployeeShifts" + this.WinID] = value;
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

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtFromDateSrch.Focus();
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
            txtFromDateSrch.Clear();
            txtToDateSrch.Clear();
            acEmployeeSrch.Clear();
            acShiftSrch.Clear();
            acDepartmentSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployeeShifts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvEmployeeShifts.PageIndex = e.NewPageIndex;
            gvEmployeeShifts.DataSource = this.dtEmployeeShifts;
            gvEmployeeShifts.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployeeShifts_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtEmployeeShifts.Select("ID=" + gvEmployeeShifts.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            ddlType.SelectedValue = dr["Department_ID"].ToExpressString() == string.Empty ? "1" : "0";
            this.ddlType_SelectedIndexChanged(null, null);

            acEmployeeOrDepartment.Value = dr["Department_ID"].ToExpressString() == string.Empty ? dr["Contact_ID"].ToExpressString() : dr["Department_ID"].ToExpressString();
            acShift.Value = dr["Shift_ID"].ToExpressString();
            txtFromDate.Text = dr["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            txtToDate.Text = dr["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            this.EditID = gvEmployeeShifts.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvEmployeeShifts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_EmployeeShifts_Delete(gvEmployeeShifts.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvEmployeeShifts.DataKeys[e.RowIndex]["ContactName"].ToExpressString() + ":" + gvEmployeeShifts.DataKeys[e.RowIndex]["ShiftName"].ToExpressString(), dc);
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
            txtFromDate.Clear();
            txtToDate.Clear();
            acEmployeeOrDepartment.Clear();
            acShift.Clear();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
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

            if (txtFromDate.Text.ToDate() > txtToDate.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateFromTo, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            int? Department_ID = ddlType.SelectedValue == "0" ? acEmployeeOrDepartment.Value.ToInt() : (int?)null;
            int? Contact_ID = ddlType.SelectedValue == "1" ? acEmployeeOrDepartment.Value.ToInt() : (int?)null;

            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_EmployeeShifts_Insert(Contact_ID, Department_ID, acShift.Value.ToInt(), txtFromDate.Text.ToDate(), txtToDate.Text.ToDate());
            }
            else
            {
                result = dc.usp_HR_EmployeeShifts_Update(this.EditID, Contact_ID, Department_ID, acShift.Value.ToInt(), txtFromDate.Text.ToDate(), txtToDate.Text.ToDate());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PeriodAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acEmployeeOrDepartment.Text + " : " + acShift.Text, dc);
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
            txtFromDate.Clear();
            txtToDate.Clear();
            acEmployeeOrDepartment.Clear();
            acShift.Clear();
            mpeCreateNew.Show();
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
        acDepartmentSrch.ContextKey = acShiftSrch.ContextKey = acShift.ContextKey = string.Empty;
        acEmployeeSrch.ContextKey = acEmployeeOrDepartment.ContextKey = "E,,,";
        this.ddlType_SelectedIndexChanged(null, null);
    }

    private void Fill()
    {
        this.dtEmployeeShifts = dc.usp_HR_EmployeeShifts_Select(acEmployeeSrch.Value.ToNullableInt(), acDepartmentSrch.Value.ToNullableInt(), acShiftSrch.Value.ToNullableInt(), txtFromDateSrch.Text.ToDate(), txtToDateSrch.Text.ToDate()).CopyToDataTable();
        gvEmployeeShifts.DataSource = this.dtEmployeeShifts;
        gvEmployeeShifts.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvEmployeeShifts.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvEmployeeShifts.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}