using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Shifts : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtShifts
    {
        get
        {
            return (DataTable)Session["dtShifts" + this.WinID];
        }

        set
        {
            Session["dtShifts" + this.WinID] = value;
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
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvShifts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvShifts.PageIndex = e.NewPageIndex;
            gvShifts.DataSource = this.dtShifts;
            gvShifts.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvShifts_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtShifts.Select("ID=" + gvShifts.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtWorkFrom.Text = dr["WorkFrom"].ToDate().Value.ToString("hh:mm");
            ddlWorkFrom.SelectedValue = dr["WorkFrom"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);

            txtWorkTo.Text = dr["WorkTo"].ToDate().Value.ToString("hh:mm");
            ddlWorkTo.SelectedValue = dr["WorkTo"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);

            if (dr["BreakFrom"].ToExpressString() != string.Empty)
            {
                txtBreakFrom.Text = dr["BreakFrom"].ToDate().Value.ToString("hh:mm");
                ddlBreakFrom.SelectedValue = dr["BreakFrom"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (dr["BreakTo"].ToExpressString() != string.Empty)
            {
                txtBreakTo.Text = dr["BreakTo"].ToDate().Value.ToString("hh:mm");
                ddlBreakTo.SelectedValue = dr["BreakTo"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (dr["delayFrom"].ToExpressString() != string.Empty)
            {
                txtDelayFrom.Text = dr["delayFrom"].ToDate().Value.ToString("hh:mm");
                ddlDelayFrom.SelectedValue = dr["delayFrom"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (dr["AbsenceFrom"].ToExpressString() != string.Empty)
            {
                txtAbsenceFrom.Text = dr["AbsenceFrom"].ToDate().Value.ToString("hh:mm");
                ddlAbsenceFrom.SelectedValue = dr["AbsenceFrom"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (dr["DefaultCheckIn"].ToExpressString() != string.Empty)
            {
                txtDefaultWorkFrom.Text = dr["DefaultCheckIn"].ToDate().Value.ToString("hh:mm");
                ddlDefaultWorkFrom.SelectedValue = dr["DefaultCheckIn"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            if (dr["DefaultCheckOut"].ToExpressString() != string.Empty)
            {
                txtDefaultWorkTo.Text = dr["DefaultCheckOut"].ToDate().Value.ToString("hh:mm");
                ddlDefaultWorkTo.SelectedValue = dr["DefaultCheckOut"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            }

            this.EditID = gvShifts.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvShifts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_Shifts_Delete(gvShifts.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvShifts.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
            txtName.Clear();
            txtWorkFrom.Clear();
            txtWorkTo.Clear();
            txtDefaultWorkFrom.Clear();
            txtDefaultWorkTo.Clear();
            txtDelayFrom.Clear();
            txtAbsenceFrom.Clear();
            txtBreakFrom.Clear();
            txtBreakTo.Clear();
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

            //if (txtWorkTo.ToTimeSpan(ddlWorkTo.SelectedValue) < txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)

            //    || txtBreakFrom.ToTimeSpan(ddlBreakFrom.SelectedValue) < txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)
            //    || txtBreakFrom.ToTimeSpan(ddlBreakFrom.SelectedValue) < txtDefaultWorkFrom.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue)
            //    || txtBreakTo.ToTimeSpan(ddlBreakTo.SelectedValue) < txtBreakFrom.ToTimeSpan(ddlBreakFrom.SelectedValue)
            //    || txtDelayFrom.ToTimeSpan(ddlDelayFrom.SelectedValue) <= txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)
            //    || txtDelayFrom.ToTimeSpan(ddlDelayFrom.SelectedValue) <= txtDefaultWorkFrom.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue)
            //    || txtAbsenceFrom.ToTimeSpan(ddlAbsenceFrom.SelectedValue) <= txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)
            //    || txtAbsenceFrom.ToTimeSpan(ddlAbsenceFrom.SelectedValue) <= txtDefaultWorkFrom.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue)
            //    || txtDefaultWorkFrom.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue) < txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)
            //    || txtDefaultWorkTo.ToTimeSpan(ddlDefaultWorkTo.SelectedValue) < txtWorkFrom.ToTimeSpan(ddlWorkFrom.SelectedValue)
            //    || txtDefaultWorkTo.ToTimeSpan(ddlDefaultWorkTo.SelectedValue) < txtDefaultWorkFrom.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue))
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.ShiftNotValid, string.Empty);
            //    mpeCreateNew.Show();
            //    return;
            //}

            if (this.EditID == 0) //insert
            {
                result = dc.usp_HR_Shifts_Insert(txtName.TrimmedText, txtWorkFrom.Text.ToTimeSpan(ddlWorkFrom.SelectedValue), txtWorkTo.Text.ToTimeSpan(ddlWorkTo.SelectedValue), txtBreakFrom.Text.ToTimeSpan(ddlBreakFrom.SelectedValue), txtBreakTo.Text.ToTimeSpan(ddlBreakTo.SelectedValue), txtDelayFrom.Text.ToTimeSpan(ddlDelayFrom.SelectedValue), txtAbsenceFrom.Text.ToTimeSpan(ddlAbsenceFrom.SelectedValue), txtDefaultWorkFrom.Text.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue), txtDefaultWorkTo.Text.ToTimeSpan(ddlDefaultWorkTo.SelectedValue));
            }
            else
            {
                result = dc.usp_HR_Shifts_Update(this.EditID, txtName.TrimmedText, txtWorkFrom.Text.ToTimeSpan(ddlWorkFrom.SelectedValue), txtWorkTo.Text.ToTimeSpan(ddlWorkTo.SelectedValue), txtBreakFrom.Text.ToTimeSpan(ddlBreakFrom.SelectedValue), txtBreakTo.Text.ToTimeSpan(ddlBreakTo.SelectedValue), txtDelayFrom.Text.ToTimeSpan(ddlDelayFrom.SelectedValue), txtAbsenceFrom.Text.ToTimeSpan(ddlAbsenceFrom.SelectedValue), txtDefaultWorkFrom.Text.ToTimeSpan(ddlDefaultWorkFrom.SelectedValue), txtDefaultWorkTo.Text.ToTimeSpan(ddlDefaultWorkTo.SelectedValue));
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.ShiftAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
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
            txtName.Clear();
            txtWorkFrom.Clear();
            txtWorkTo.Clear();
            txtDefaultWorkFrom.Clear();
            txtDefaultWorkTo.Clear();
            txtDelayFrom.Clear();
            txtAbsenceFrom.Clear();
            txtBreakFrom.Clear();
            txtBreakTo.Clear();
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
    }

    private void Fill()
    {
        this.dtShifts = dc.usp_HR_Shifts_Select(null, txtNameSrch.TrimmedText).CopyToDataTable();
        gvShifts.DataSource = this.dtShifts;
        gvShifts.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvShifts.Columns[9].Visible = MyContext.PageData.IsEdit;
        gvShifts.Columns[10].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}