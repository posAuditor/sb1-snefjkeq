using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Attendance : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtAttendance
    {
        get
        {
            return (DataTable)Session["dtAttendance" + this.WinID];
        }

        set
        {
            Session["dtAttendance" + this.WinID] = value;
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void FilterEmployees(object sender, EventArgs e)
    {
        try
        {
            acEmployeeSrch.ContextKey = acDepartmentSrch.Value + ",," + acBranch.Value;
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
            txtMachineIDSrch.Focus();
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
            txtMachineIDSrch.Clear();
            acDepartmentSrch.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterEmployees(null, null);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAttendance.PageIndex = e.NewPageIndex;
            gvAttendance.DataSource = this.dtAttendance;
            gvAttendance.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtAttendance.Select("ID=" + gvAttendance.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acEmployee.Value = dr["Contact_ID"].ToExpressString();
            txtDate.Text = dr["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtCheckInTime.Text = dr["CheckInTime"].ToDate().Value.ToString("hh:mm");
            ddlCheckInTime.SelectedValue = dr["CheckInTime"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            if (dr["CheckOutTime"].ToExpressString() != string.Empty)
            {
                txtCheckOutTime.Text = dr["CheckOutTime"].ToDate().Value.ToString("hh:mm");
                ddlCheckOutTime.SelectedValue = dr["CheckOutTime"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }
            this.EditID = gvAttendance.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            acEmployee.Enabled = false;
            txtDate.Enabled = false;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAttendance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_Attendance_Delete(gvAttendance.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvAttendance.DataKeys[e.RowIndex]["ContactName"].ToExpressString(), dc);
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
            txtDate.Clear();
            txtCheckInTime.Clear();
            acEmployee.Clear();
            txtCheckOutTime.Clear();
            this.EditID = 0;
            acEmployee.Enabled = true;
            txtDate.Enabled = true;
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
            if (txtDate.Text.ToDate() > DateTime.Now.Date.AddDays(2))
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            int result = 0;
            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_Attendance_Insert(acEmployee.Value.ToInt(), txtDate.Text.ToDate(), txtCheckInTime.Text.ToTimeSpan(ddlCheckInTime.SelectedValue), txtCheckOutTime.Text.ToTimeSpan(ddlCheckOutTime.SelectedValue));
            }
            else
            {
                result = dc.usp_HR_Attendance_Update(this.EditID, acEmployee.Value.ToInt(), txtDate.Text.ToDate(), txtCheckInTime.Text.ToTimeSpan(ddlCheckInTime.SelectedValue), txtCheckOutTime.Text.ToTimeSpan(ddlCheckOutTime.SelectedValue));
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            if (result == -7)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.InvalidAttendanceDay, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, acEmployee.Text, dc);
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
            txtDate.Clear();
            txtCheckInTime.Clear();
            acEmployee.Clear();
            txtCheckOutTime.Clear();
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
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
        acDepartmentSrch.ContextKey = string.Empty;
        acEmployee.ContextKey = "E," + MyContext.UserProfile.Branch_ID + ",,";
        txtFromDateSrch.Text = DateTime.Now.AddDays((DateTime.Now.Day - 1) * -1).ToString("d/M/yyyy");
    }

    private void Fill()
    {
        this.dtAttendance = dc.usp_HR_Attendance_Select(acEmployeeSrch.Value.ToNullableInt(), txtMachineIDSrch.TrimmedText, acDepartmentSrch.Value.ToNullableInt(), txtFromDateSrch.Text.ToDate(), txtToDateSrch.Text.ToDate(), acBranch.Value.ToNullableInt()).CopyToDataTable();
        gvAttendance.DataSource = this.dtAttendance;
        gvAttendance.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvAttendance.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvAttendance.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvAttendance.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion
}