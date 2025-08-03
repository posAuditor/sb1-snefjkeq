using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Vacations : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtVacations
    {
        get
        {
            return (DataTable)Session["dtVacations" + this.WinID];
        }

        set
        {
            Session["dtVacations" + this.WinID] = value;
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
            acDepartmentSrch.AutoCompleteFocus();
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
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            acDepartmentSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvVacations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvVacations.PageIndex = e.NewPageIndex;
            gvVacations.DataSource = this.dtVacations;
            gvVacations.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvVacations_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtVacations.Select("ID=" + gvVacations.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            lstDepartments.SelectedValue = dr["Department_ID"].ToExpressString();
            lstDepartments.Attributes.Add("disabled", "");
            if (dr["DateFrom"].ToExpressString() != string.Empty) txtDateFrom.Text = dr["DateFrom"].ToDate().Value.ToString("d/M/yyyy");
            if (dr["DateTo"].ToExpressString() != string.Empty) txtDateTo.Text = dr["DateTo"].ToDate().Value.ToString("d/M/yyyy");
            if (dr["WeekDayFrom"].ToExpressString() != string.Empty) ddlWeekDayFrom.SelectedValue = dr["WeekDayFrom"].ToExpressString();
            if (dr["WeekDayTo"].ToExpressString() != string.Empty) ddlWeekDayTo.SelectedValue = dr["WeekDayTo"].ToExpressString();
            ddlVacationType.SelectedValue = (dr["WeekDayTo"].ToExpressString() != string.Empty) ? "0" : "1";
            this.ddlVacationType_SelectedIndexChanged(null, null);

            this.EditID = gvVacations.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlVacationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divWeekly.Visible = ddlVacationType.SelectedValue == "0";
            divAnnulally.Visible = ddlVacationType.SelectedValue == "1";
            txtDateFrom.Clear();
            txtDateTo.Clear();
            mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvVacations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_Vacations_Delete(gvVacations.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, string.Empty, dc);
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
            txtDateFrom.Clear();
            txtDateTo.Clear();
            lstDepartments.ClearSelection();
            lstDepartments.Attributes.Remove("disabled");
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
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            int? WeekDayFrom = ddlVacationType.SelectedValue == "0" ? ddlWeekDayFrom.SelectedValue.ToInt() : (int?)null;
            int? WeekDayTo = ddlVacationType.SelectedValue == "0" ? ddlWeekDayTo.SelectedValue.ToInt() : (int?)null;

            if (lstDepartments.GetSelectedIndices().Count() == 0)
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.SelectDepartment, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            if (ddlVacationType.SelectedValue == "1" && txtDateFrom.Text.ToDate() > txtDateTo.Text.ToDate())
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.DateFromTo, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            if (this.EditID == 0) //insert
            {
                foreach (ListItem item in lstDepartments.Items)
                {
                    if (item.Selected) result = dc.usp_HR_Vacations_Insert(item.Value.ToInt(), ddlVacationType.SelectedValue.ToByte(), txtDateFrom.Text.ToDate(), txtDateTo.Text.ToDate(), WeekDayFrom, WeekDayTo);
                    if (result == -2)
                    {
                        trans.Rollback();
                        UserMessages.Message(null, Resources.UserInfoMessages.VacationAlreadyExists + " (" + item.Text.Trim() + ")", string.Empty);
                        mpeCreateNew.Show();
                        return;
                    }
                }
            }
            else
            {
                result = dc.usp_HR_Vacations_Update(this.EditID, lstDepartments.SelectedValue.ToInt(), ddlVacationType.SelectedValue.ToByte(), txtDateFrom.Text.ToDate(), txtDateTo.Text.ToDate(), WeekDayFrom, WeekDayTo);
            }
            if (result == -2)
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.VacationAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, string.Empty, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);

            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            txtDateFrom.Clear();
            txtDateTo.Clear();
            lstDepartments.ClearSelection();
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
        var lst = dc.usp_HR_DepartmentsPadded_Select().ToList();
        lst.ForEach(x => x.Name = HttpUtility.HtmlDecode(x.Name));
        acDepartmentSrch.ContextKey = string.Empty;
        lstDepartments.DataSource = lst;
        lstDepartments.DataTextField = "Name";
        lstDepartments.DataValueField = "ID";
        lstDepartments.DataBind();
    }

    private void Fill()
    {
        this.dtVacations = dc.usp_HR_Vacations_Select(null, acDepartmentSrch.Value.ToNullableInt(), ddlVacationTypeSrch.SelectedValue.ToByte(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), ddlWeekDayFromSrch.SelectedValue.ToInt(), ddlWeekDayToSrch.SelectedValue.ToInt()).CopyToDataTable();
        gvVacations.DataSource = this.dtVacations;
        gvVacations.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvVacations.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvVacations.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}