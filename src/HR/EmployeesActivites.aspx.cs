using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class HR_EmployeesActivites : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private int Contact_ID
    {
        get
        {
            if (ViewState["Contact_ID"] == null) return 0;
            return (int)ViewState["Contact_ID"];
        }

        set
        {
            ViewState["Contact_ID"] = value;
        }
    }

    private DataTable dtComplaints
    {
        get
        {
            if (Session["dtComplaints" + this.WinID] == null)
            {
                Session["dtComplaints" + this.WinID] = dc.usp_HR_Complaints_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtComplaints" + this.WinID];
        }

        set
        {
            Session["dtComplaints" + this.WinID] = value;
        }
    }

    private int ComplaintsEditID
    {
        get
        {
            if (ViewState["ComplaintsEditID"] == null) return 0;
            return (int)ViewState["ComplaintsEditID"];
        }

        set
        {
            ViewState["ComplaintsEditID"] = value;
        }
    }

    private DataTable dtVacations
    {
        get
        {
            if (Session["dtVacations" + this.WinID] == null)
            {
                Session["dtVacations" + this.WinID] = dc.usp_HR_TakenVacations_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtVacations" + this.WinID];
        }

        set
        {
            Session["dtVacations" + this.WinID] = value;
        }
    }

    private int VacationsEditID
    {
        get
        {
            if (ViewState["VacationsEditID"] == null) return 0;
            return (int)ViewState["VacationsEditID"];
        }

        set
        {
            ViewState["VacationsEditID"] = value;
        }
    }

    private DataTable dtSancations
    {
        get
        {
            if (Session["dtSancations" + this.WinID] == null)
            {
                Session["dtSancations" + this.WinID] = dc.usp_HR_Sanctions_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtSancations" + this.WinID];
        }

        set
        {
            Session["dtSancations" + this.WinID] = value;
        }
    }

    private int SancationsEditID
    {
        get
        {
            if (ViewState["SancationsEditID"] == null) return 0;
            return (int)ViewState["SancationsEditID"];
        }

        set
        {
            ViewState["SancationsEditID"] = value;
        }
    }

    private DataTable dtIncentives
    {
        get
        {
            if (Session["dtIncentives" + this.WinID] == null)
            {
                Session["dtIncentives" + this.WinID] = dc.usp_HR_TakenIncentive_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtIncentives" + this.WinID];
        }

        set
        {
            Session["dtIncentives" + this.WinID] = value;
        }
    }

    private int IncentivesEditID
    {
        get
        {
            if (ViewState["IncentivesEditID"] == null) return 0;
            return (int)ViewState["IncentivesEditID"];
        }

        set
        {
            ViewState["IncentivesEditID"] = value;
        }
    }

    private DataTable dtAbasenceInMissions
    {
        get
        {
            if (Session["dtAbasenceInMissions" + this.WinID] == null)
            {
                Session["dtAbasenceInMissions" + this.WinID] = dc.usp_HR_AbsenceInMission_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtAbasenceInMissions" + this.WinID];
        }

        set
        {
            Session["dtAbasenceInMissions" + this.WinID] = value;
        }
    }

    private int AbasenceInMissionsEditID
    {
        get
        {
            if (ViewState["AbasenceInMissionsEditID"] == null) return 0;
            return (int)ViewState["AbasenceInMissionsEditID"];
        }

        set
        {
            ViewState["AbasenceInMissionsEditID"] = value;
        }
    }

    private DataTable dtExcuses
    {
        get
        {
            if (Session["dtExcusess" + this.WinID] == null)
            {
                Session["dtExcusess" + this.WinID] = dc.usp_HR_Excuses_Select(this.Contact_ID).CopyToDataTable();
            }
            return (DataTable)Session["dtExcusess" + this.WinID];
        }

        set
        {
            Session["dtExcusess" + this.WinID] = value;
        }
    }

    private int ExcusesEditID
    {
        get
        {
            if (ViewState["ExcusessEditID"] == null) return 0;
            return (int)ViewState["ExcusessEditID"];
        }

        set
        {
            ViewState["ExcusessEditID"] = value;
        }
    }


    private int? Branch_ID
    {
        get
        {
            return (int?)ViewState["Branch_ID"];
        }

        set
        {
            ViewState["Branch_ID"] = value;
        }
    }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.SetEditMode();
            this.CheckSecurity();

            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.FillEmployeeData();
            }
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
            this.BindGridViews();
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
            ddlStatus.SelectedIndex = 0;
            txtDateToSrch.Focus();
            this.BindGridViews();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSave_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        int result = 0;
        try
        {

            //Complaints
            foreach (DataRow r in this.dtComplaints.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Complaints_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_Complaints_Insert(this.Contact_ID, r["AgainstContact_ID"].ToInt(), r["Date"].ToDate(), r["Reason"].ToExpressString());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_Complaints_Update(r["ID"].ToInt(), r["AgainstContact_ID"].ToInt(), r["Date"].ToDate(), r["Reason"].ToExpressString());
                }
            }

            //Vacations Delete
            foreach (DataRow r in this.dtVacations.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_TakenVacations_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
            }
            //Absence in mission Delete
            foreach (DataRow r in this.dtAbasenceInMissions.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_AbsenceInMission_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
            }
            //Excuses Delete
            foreach (DataRow r in this.dtExcuses.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Excuses_delete(r["ID", DataRowVersion.Original].ToInt());
                }
            }
            //Vacations
            foreach (DataRow r in this.dtVacations.Rows)
            {
                if (r.RowState == DataRowState.Added)
                {
                    result = dc.usp_HR_TakenVacations_Insert(this.Contact_ID, r["VacationType_ID"].ToInt(), r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["Reason"].ToExpressString(), r["ApprovedBY_ID"].ToInt(), r["IsAccepted"].ToBoolean(), r["Notes"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidVacation + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                    if (result == -10)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.VacationBalanceNotEnough + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    result = dc.usp_HR_TakenVacations_Update(r["ID"].ToInt(), this.Contact_ID, r["VacationType_ID"].ToInt(), r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["Reason"].ToExpressString(), r["ApprovedBY_ID"].ToInt(), r["IsAccepted"].ToBoolean(), r["Notes"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidVacation + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                    if (result == -10)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.VacationBalanceNotEnough + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            //Sancations
            foreach (DataRow r in this.dtSancations.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_Sanctions_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_Sanctions_Insert(this.Contact_ID, r["SanctionType_ID"].ToInt(), r["Date"].ToDate(), r["Reason"].ToExpressString(), r["ApprovedBY_Id"].ToInt(), r["EmployeeComment"].ToExpressString(), r["SancationProcedure_ID"].ToInt(), r["SalaryDeducation"].ToDecimalOrDefault(), r["SalaryDeducationType_ID"].ToInt());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_Sanctions_Update(r["ID"].ToInt(), r["SanctionType_ID"].ToInt(), r["Date"].ToDate(), r["Reason"].ToExpressString(), r["ApprovedBY_Id"].ToInt(), r["EmployeeComment"].ToExpressString(), r["SancationProcedure_ID"].ToInt(), r["SalaryDeducation"].ToDecimalOrDefault(), r["SalaryDeducationType_ID"].ToInt());
                }
            }

            //Incentive
            foreach (DataRow r in this.dtIncentives.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_HR_TakenIncentive_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_HR_TakenIncentive_Insert(this.Contact_ID, r["incentive_ID"].ToInt(), r["Date"].ToDate(), r["Notes"].ToExpressString());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_HR_TakenIncentive_Update(r["ID"].ToInt(), r["incentive_ID"].ToInt(), r["Date"].ToDate(), r["Notes"].ToExpressString());
                }
            }

            //Absence in mission
            foreach (DataRow r in this.dtAbasenceInMissions.Rows)
            {
                if (r.RowState == DataRowState.Added)
                {
                    result = dc.usp_HR_AbsenceInMission_Insert(this.Contact_ID, r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["Reason"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidAbseneceInMission + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    result = dc.usp_HR_AbsenceInMission_Update(r["ID"].ToInt(), this.Contact_ID, r["FromDate"].ToDate(), r["ToDate"].ToDate(), r["Reason"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidAbseneceInMission + string.Format(" ({0:d/M/yyyy} - {1:d/M/yyyy})", r["FromDate"].ToDate(), r["ToDate"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            //Excuses
            foreach (DataRow r in this.dtExcuses.Rows)
            {
                if (r.RowState == DataRowState.Added)
                {
                    result = dc.usp_HR_Excuses_Insert(this.Contact_ID, r["Date"].ToDate(), r["FromTime"].ToDate().Value.TimeOfDay, r["ToTime"].ToDate().Value.TimeOfDay, r["IsMission"].ToBoolean(), r["IsAccepted"].ToBoolean(), r["ApprovedBY_ID"].ToInt(), r["Reason"].ToExpressString(), r["Notes"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidExcuse + string.Format(" ({0:hh:mm tt} - {1:hh:mm tt})", r["FromTime"].ToDate(), r["ToTime"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    result = dc.usp_HR_Excuses_Update(r["ID"].ToInt(), this.Contact_ID, r["Date"].ToDate(), r["FromTime"].ToDate().Value.TimeOfDay, r["ToTime"].ToDate().Value.TimeOfDay, r["IsMission"].ToBoolean(), r["IsAccepted"].ToBoolean(), r["ApprovedBY_ID"].ToInt(), r["Reason"].ToExpressString(), r["Notes"].ToExpressString());
                    if (result == -7)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.InvalidExcuse + string.Format(" ({0:hh:mm tt} - {1:hh:mm tt})", r["FromTime"].ToDate(), r["ToTime"].ToDate()), string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            LogAction(Actions.Edit, lblEmpName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.EmployeesActivites + "?ID=" + this.Contact_ID.ToExpressString(), PageLinks.EmployeesList);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(PageLinks.EmployeesList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #region Complaints

    protected void gvComplaints_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvComplaints.PageIndex = e.NewPageIndex;
            gvComplaints.DataSource = this.dtComplaints;
            gvComplaints.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvComplaints_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.ComplaintsEditID = gvComplaints.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtComplaints.Select("ID=" + this.ComplaintsEditID.ToExpressString())[0];
            acComplaintAgainst.Value = r["AgainstContact_ID"].ToExpressString();
            txtComplaintDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtComplaintReason.Text = r["Reason"].ToExpressString();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvComplaints_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvComplaints.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtComplaints.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvComplaints.DataSource = this.dtComplaints;
            gvComplaints.DataBind();
            this.ClearComplaintForm();
            this.ComplaintsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddComplaint_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.ComplaintsEditID == 0)
            {
                r = this.dtComplaints.NewRow();
                r["ID"] = this.dtComplaints.GetID("ID");

            }
            else
            {
                r = this.dtComplaints.Select("ID=" + this.ComplaintsEditID)[0];
            }

            r["AgainstContact_ID"] = acComplaintAgainst.Value.ToInt();
            r["Date"] = txtComplaintDate.Text.ToDate();
            r["Reason"] = txtComplaintReason.TrimmedText;
            r["AgainstName"] = acComplaintAgainst.Text;
            if (this.ComplaintsEditID == 0)
            {
                this.dtComplaints.Rows.Add(r);
            }
            gvComplaints.DataSource = this.dtComplaints;
            gvComplaints.DataBind();
            this.ClearComplaintForm();
            this.ComplaintsEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearComplaint_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearComplaintForm();
            this.ComplaintsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearComplaintForm()
    {
        acComplaintAgainst.Clear();
        txtComplaintReason.Clear();
        txtComplaintDate.Clear();
    }

    #endregion

    #region Vacations

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
            this.VacationsEditID = gvVacations.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtVacations.Select("ID=" + this.VacationsEditID.ToExpressString())[0];
            acVacationType.Value = r["VacationType_ID"].ToStringOrEmpty();
            acVacationApprovedBY.Value = r["ApprovedBY_ID"].ToStringOrEmpty();
            txtVacationFromDate.Text = r["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            txtVacationToDate.Text = r["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            txtVacationReason.Text = r["Reason"].ToExpressString();
            txtVacationNotes.Text = r["Notes"].ToExpressString();
            chkVacationIsAccepted.Checked = r["IsAccepted"].ToBoolean();

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
            int ID = gvVacations.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtVacations.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvVacations.DataSource = this.dtVacations;
            gvVacations.DataBind();
            this.ClearVacationsForm();
            this.VacationsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddVacations_click(object sender, EventArgs e)
    {
        try
        {
            if (txtVacationFromDate.Text.ToDate() > txtVacationToDate.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateFromTo, string.Empty);
                return;
            }
            DataRow r = null;
            if (this.VacationsEditID == 0)
            {
                r = this.dtVacations.NewRow();
                r["ID"] = this.dtVacations.GetID("ID");

            }
            else
            {
                r = this.dtVacations.Select("ID=" + this.VacationsEditID)[0];
            }

            r["FromDate"] = txtVacationFromDate.Text.ToDate();
            r["ToDate"] = txtVacationToDate.Text.ToDate();
            r["Reason"] = txtVacationReason.TrimmedText;
            r["Notes"] = txtVacationNotes.TrimmedText;
            r["IsAccepted"] = chkVacationIsAccepted.Checked;
            r["ApprovedBy_ID"] = acVacationApprovedBY.Value.ToIntOrDBNULL();
            r["VacationType_ID"] = acVacationType.Value.ToIntOrDBNULL();
            r["VacationTypeName"] = acVacationType.Text;
            if (this.VacationsEditID == 0)
            {
                this.dtVacations.Rows.Add(r);
            }
            gvVacations.DataSource = this.dtVacations;
            gvVacations.DataBind();
            this.ClearVacationsForm();
            this.VacationsEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearVacations_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearVacationsForm();
            this.VacationsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearVacationsForm()
    {
        txtVacationFromDate.Clear();
        txtVacationToDate.Clear();
        acVacationApprovedBY.Clear();
        acVacationType.Clear();
        txtVacationReason.Clear();
        txtVacationNotes.Clear();
        chkVacationIsAccepted.Checked = false;
    }

    #endregion

    #region Sancations

    protected void gvSancations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSancations.PageIndex = e.NewPageIndex;
            gvSancations.DataSource = this.dtSancations;
            gvSancations.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSancations_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.SancationsEditID = gvSancations.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtSancations.Select("ID=" + this.SancationsEditID.ToExpressString())[0];
            acSancationType.Value = r["SanctionType_ID"].ToStringOrEmpty();
            acSancationTakenProcedure.Value = r["SancationProcedure_ID"].ToStringOrEmpty();
            ddlSancationSalaryDeducationValueType.SelectedValue = r["SalaryDeducationType_ID"].ToStringOrEmpty();
            txtSancationSalaryDeduction.Text = r["SalaryDeducation"].ToExpressString();
            txtSancationDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtSancationReason.Text = r["Reason"].ToExpressString();
            txtSancationEmployeeComment.Text = r["EmployeeComment"].ToExpressString();
            acSanctionApprovedBy.Value = r["ApprovedBy_ID"].ToExpressString();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSancations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvSancations.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtSancations.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvSancations.DataSource = this.dtSancations;
            gvSancations.DataBind();
            this.ClearSancationsForm();
            this.SancationsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddSancations_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.SancationsEditID == 0)
            {
                r = this.dtSancations.NewRow();
                r["ID"] = this.dtSancations.GetID("ID");

            }
            else
            {
                r = this.dtSancations.Select("ID=" + this.SancationsEditID)[0];
            }

            r["ApprovedBy_ID"] = acSanctionApprovedBy.Value.ToInt();
            r["SanctionType_ID"] = acSancationType.Value.ToInt();
            r["SancationTypeName"] = acSancationType.Text;
            r["ProcedureName"] = acSancationTakenProcedure.Text;
            r["ValueTypeName"] = ddlSancationSalaryDeducationValueType.SelectedItem.Text;
            r["SancationProcedure_ID"] = acSancationTakenProcedure.Value.ToIntOrDBNULL();
            r["SalaryDeducationType_ID"] = ddlSancationSalaryDeducationValueType.SelectedValue.ToInt();
            r["SalaryDeducation"] = txtSancationSalaryDeduction.Text.ToDecimalOrDefault();
            r["Date"] = txtSancationDate.Text.ToDate();
            r["Reason"] = txtSancationReason.Text.ToExpressString();
            r["EmployeeComment"] = txtSancationEmployeeComment.Text;
            if (this.SancationsEditID == 0)
            {
                this.dtSancations.Rows.Add(r);
            }
            gvSancations.DataSource = this.dtSancations;
            gvSancations.DataBind();
            this.ClearSancationsForm();
            this.SancationsEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSancations_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearSancationsForm();
            this.SancationsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearSancationsForm()
    {
        acSancationType.Clear();
        txtSancationDate.Clear();
        txtSancationReason.Clear();
        acSanctionApprovedBy.Clear();
        acSancationTakenProcedure.Clear();
        txtSancationEmployeeComment.Clear();
        txtSancationSalaryDeduction.Clear();
    }

    #endregion

    #region Incentives

    protected void gvIncentives_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvIncentives.PageIndex = e.NewPageIndex;
            gvIncentives.DataSource = this.dtIncentives;
            gvIncentives.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvIncentives_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.IncentivesEditID = gvIncentives.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtIncentives.Select("ID=" + this.IncentivesEditID.ToExpressString())[0];
            acIncentive.Value = r["Incentive_ID"].ToStringOrEmpty();
            txtIncentiveDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtIncentiveNotes.Text = r["Notes"].ToStringOrEmpty();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvIncentives_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvIncentives.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtIncentives.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvIncentives.DataSource = this.dtIncentives;
            gvIncentives.DataBind();
            this.ClearIncentivesForm();
            this.IncentivesEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddIncentives_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.IncentivesEditID == 0)
            {
                r = this.dtIncentives.NewRow();
                r["ID"] = this.dtIncentives.GetID("ID");

            }
            else
            {
                r = this.dtIncentives.Select("ID=" + this.IncentivesEditID)[0];
            }

            r["Incentive_ID"] = acIncentive.Value.ToInt();
            r["IncentiveName"] = acIncentive.Text;
            r["Date"] = txtIncentiveDate.Text.ToDate();
            r["Notes"] = txtIncentiveNotes.Text;
            if (this.IncentivesEditID == 0)
            {
                this.dtIncentives.Rows.Add(r);
            }
            gvIncentives.DataSource = this.dtIncentives;
            gvIncentives.DataBind();
            this.ClearIncentivesForm();
            this.IncentivesEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearIncentives_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearIncentivesForm();
            this.IncentivesEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearIncentivesForm()
    {
        acIncentive.Clear();
        txtIncentiveDate.Clear();
        txtIncentiveNotes.Clear();
    }

    #endregion

    #region AbasenceInMissions

    protected void gvAbasenceInMission_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAbasenceInMission.PageIndex = e.NewPageIndex;
            gvAbasenceInMission.DataSource = this.dtAbasenceInMissions;
            gvAbasenceInMission.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAbasenceInMission_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.AbasenceInMissionsEditID = gvAbasenceInMission.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtAbasenceInMissions.Select("ID=" + this.AbasenceInMissionsEditID.ToExpressString())[0];
            txtAbsenceInMissionFromDate.Text = r["FromDate"].ToDate().Value.ToString("d/M/yyyy");
            txtAbsenceInMissionToDate.Text = r["ToDate"].ToDate().Value.ToString("d/M/yyyy");
            txtAbasenceInMissionReason.Text = r["Reason"].ToStringOrEmpty();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAbasenceInMission_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvAbasenceInMission.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtAbasenceInMissions.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvAbasenceInMission.DataSource = this.dtAbasenceInMissions;
            gvAbasenceInMission.DataBind();
            this.ClearAbasenceInMissionsForm();
            this.AbasenceInMissionsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddAbasenceInMission_click(object sender, EventArgs e)
    {
        try
        {
            if (txtAbsenceInMissionFromDate.Text.ToDate() > txtAbsenceInMissionToDate.Text.ToDate())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateFromTo, string.Empty);
                return;
            }
            DataRow r = null;
            if (this.AbasenceInMissionsEditID == 0)
            {
                r = this.dtAbasenceInMissions.NewRow();
                r["ID"] = this.dtAbasenceInMissions.GetID("ID");

            }
            else
            {
                r = this.dtAbasenceInMissions.Select("ID=" + this.AbasenceInMissionsEditID)[0];
            }

            r["FromDate"] = txtAbsenceInMissionFromDate.Text.ToDate();
            r["ToDate"] = txtAbsenceInMissionToDate.Text.ToDate();
            r["Reason"] = txtAbasenceInMissionReason.Text;
            if (this.AbasenceInMissionsEditID == 0)
            {
                this.dtAbasenceInMissions.Rows.Add(r);
            }
            gvAbasenceInMission.DataSource = this.dtAbasenceInMissions;
            gvAbasenceInMission.DataBind();
            this.ClearAbasenceInMissionsForm();
            this.AbasenceInMissionsEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearAbasenceInMission_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearAbasenceInMissionsForm();
            this.AbasenceInMissionsEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearAbasenceInMissionsForm()
    {
        txtAbsenceInMissionFromDate.Clear();
        txtAbsenceInMissionToDate.Clear();
        txtAbasenceInMissionReason.Clear();
    }

    #endregion

    #region Excuses

    protected void gvExcuses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExcuses.PageIndex = e.NewPageIndex;
            gvExcuses.DataSource = this.dtExcuses;
            gvExcuses.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExcuses_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.ExcusesEditID = gvExcuses.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtExcuses.Select("ID=" + this.ExcusesEditID.ToExpressString())[0];
            acExcuseApprovedBy.Value = r["ApprovedBy_ID"].ToStringOrEmpty();
            txtExcuseDate.Text = r["Date"].ToDate().Value.ToString("d/M/yyyy");
            txtExcuseNotes.Text = r["Notes"].ToStringOrEmpty();
            txtExcuseReason.Text = r["Reason"].ToStringOrEmpty();
            chkExcuseInMission.Checked = r["IsMission"].ToBoolean();
            chkExcuseIsAccepted.Checked = r["IsAccepted"].ToBoolean();
            txtExcuseFrom.Text = r["FromTime"].ToDate().Value.ToString("hh:mm");
            txtExcuseTo.Text = r["ToTime"].ToDate().Value.ToString("hh:mm");
            ddlExcuseFrom.SelectedValue = r["FromTime"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);
            ddlExcuseTo.SelectedValue = r["ToTime"].ToDate().Value.ToString("tt", System.Globalization.CultureInfo.InvariantCulture);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExcuses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvExcuses.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtExcuses.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvExcuses.DataSource = this.dtExcuses;
            gvExcuses.DataBind();
            this.ClearExcusesForm();
            this.ExcusesEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddExcuse_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.ExcusesEditID == 0)
            {
                r = this.dtExcuses.NewRow();
                r["ID"] = this.dtExcuses.GetID("ID");

            }
            else
            {
                r = this.dtExcuses.Select("ID=" + this.ExcusesEditID)[0];
            }

            r["ApprovedBy_ID"] = acExcuseApprovedBy.Value.ToIntOrDBNULL();
            r["Date"] = txtExcuseDate.Text.ToDate();
            r["Notes"] = txtExcuseNotes.Text;
            r["Reason"] = txtExcuseReason.Text;
            r["IsMission"] = chkExcuseInMission.Checked;
            r["IsAccepted"] = chkExcuseIsAccepted.Checked;
            r["FromTime"] = DateTime.ParseExact(txtExcuseFrom.Text + " " + ddlExcuseFrom.SelectedValue, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            r["ToTime"] = DateTime.ParseExact(txtExcuseTo.Text + " " + ddlExcuseTo.SelectedValue, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            if (this.ExcusesEditID == 0)
            {
                this.dtExcuses.Rows.Add(r);
            }
            gvExcuses.DataSource = this.dtExcuses;
            gvExcuses.DataBind();
            this.ClearExcusesForm();
            this.ExcusesEditID = 0;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearExcuse_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearExcusesForm();
            this.ExcusesEditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void ClearExcusesForm()
    {
        txtExcuseDate.Clear();
        acExcuseApprovedBy.Clear();
        txtExcuseFrom.Clear();
        txtExcuseTo.Clear();
        chkExcuseInMission.Checked = false;
        chkExcuseIsAccepted.Checked = false;
        txtExcuseNotes.Clear();
        txtExcuseReason.Clear();
    }

    #endregion

    private void LoadControls()
    {
        var Employee = dc.usp_HR_Employees_SelectByID(this.Contact_ID).FirstOrDefault();
        lblEmpName.Text = Employee.Name;
        this.Branch_ID = Employee.Branch_ID;

        this.dtComplaints = null;
        this.dtVacations = null;
        this.dtSancations = null;
        this.dtIncentives = null;
        this.dtAbasenceInMissions = null;
        this.dtExcuses = null;

        acComplaintAgainst.ContextKey = "E," + this.Branch_ID + ",,";
        acVacationApprovedBY.ContextKey = "E," + this.Branch_ID + ",,";
        acSanctionApprovedBy.ContextKey = "E," + this.Branch_ID + ",,";
        acExcuseApprovedBy.ContextKey = "E," + this.Branch_ID + ",,";

        acVacationType.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.VacationType.ToInt().ToExpressString();
        acSancationType.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.SancationsTypes.ToInt().ToExpressString();
        acSancationTakenProcedure.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.SancationProcedures.ToInt().ToExpressString();
        acIncentive.ContextKey = string.Empty;
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.Contact_ID = Request["ID"].ToInt();
        }
    }

    private void FillEmployeeData()
    {
        this.BindGridViews();
    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = this.MyContext.PageData.IsEdit;
    }

    private void BindGridViews()
    {
        var filteredComplaints = from data in this.dtComplaints.AsEnumerable()
                                 where data.Field<DateTime>("Date") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("Date")) && data.Field<DateTime>("Date") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("Date"))
                                 select new
                                 {
                                     ID = data.Field<int>("ID"),
                                     Contact_ID = data.Field<int>("Contact_ID"),
                                     AgainstContact_ID = data.Field<int>("AgainstContact_ID"),
                                     Date = data.Field<DateTime>("Date"),
                                     Reason = data.Field<string>("Reason"),
                                     AgainstName = data.Field<string>("AgainstName")

                                 };

        var filteredVacations = from data in this.dtVacations.AsEnumerable()
                                where data.Field<DateTime>("FromDate") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("FromDate")) && data.Field<DateTime>("ToDate") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("ToDate"))
                                && data.Field<bool>("IsAccepted") == (ddlStatus.SelectedIndex == 0 ? data.Field<bool>("IsAccepted") : ddlStatus.SelectedValue.ToBoolean())
                                select new
                                {
                                    ID = data.Field<int>("ID"),
                                    Contact_ID = data.Field<int>("Contact_ID"),
                                    VacationType_ID = data.Field<int>("VacationType_ID"),
                                    FromDate = data.Field<DateTime>("FromDate"),
                                    ToDate = data.Field<DateTime>("ToDate"),
                                    Reason = data.Field<string>("Reason"),
                                    ApprovedBy_ID = data.Field<int>("ApprovedBy_ID"),
                                    Notes = data.Field<string>("Notes"),
                                    VacationTypeName = data.Field<string>("VacationTypeName"),
                                    IsAccepted = data.Field<bool>("IsAccepted")

                                };

        var filteredSancation = from data in this.dtSancations.AsEnumerable()
                                where data.Field<DateTime>("Date") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("Date")) && data.Field<DateTime>("Date") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("Date"))
                                select new
                                {
                                    ID = data.Field<int>("ID"),
                                    Contact_ID = data.Field<int>("Contact_ID"),
                                    SanctionType_ID = data.Field<int>("SanctionType_ID"),
                                    Date = data.Field<DateTime>("Date"),
                                    Reason = data.Field<string>("Reason"),
                                    SancationTypeName = data.Field<string>("SancationTypeName"),
                                    ProcedureName = data.Field<string>("ProcedureName"),
                                    ApprovedBy_ID = data.Field<int>("ApprovedBy_ID"),
                                    EmployeeComment = data.Field<string>("EmployeeComment"),
                                    SancationProcedure_ID = data.Field<int>("SancationProcedure_ID"),
                                    SalaryDeducationType_ID = data.Field<int>("SalaryDeducationType_ID"),
                                    ValueTypeName = data.Field<string>("ValueTypeName"),
                                    SalaryDeducation = data.Field<decimal>("SalaryDeducation")

                                };

        var filteredIncentives = from data in this.dtIncentives.AsEnumerable()
                                 where data.Field<DateTime>("Date") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("Date")) && data.Field<DateTime>("Date") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("Date"))
                                 select new
                                 {
                                     ID = data.Field<int>("ID"),
                                     Contact_ID = data.Field<int>("Contact_ID"),
                                     Incentive_ID = data.Field<int>("Incentive_ID"),
                                     Date = data.Field<DateTime>("Date"),
                                     Notes = data.Field<string>("Notes"),
                                     IncentiveName = data.Field<string>("IncentiveName"),

                                 };

        var filteredAbsenceInMission = from data in this.dtAbasenceInMissions.AsEnumerable()
                                       where data.Field<DateTime>("FromDate") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("FromDate")) && data.Field<DateTime>("ToDate") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("ToDate"))
                                       select new
                                       {
                                           ID = data.Field<int>("ID"),
                                           Contact_ID = data.Field<int>("Contact_ID"),
                                           FromDate = data.Field<DateTime>("FromDate"),
                                           ToDate = data.Field<DateTime>("ToDate"),
                                           Reason = data.Field<string>("Reason")

                                       };

        var filteredExcuses = from data in this.dtExcuses.AsEnumerable()
                              where data.Field<DateTime>("Date") >= (txtDateFromSrch.Text.ToDate() ?? data.Field<DateTime>("Date")) && data.Field<DateTime>("Date") <= (txtDateToSrch.Text.ToDate() ?? data.Field<DateTime>("Date"))
                              && data.Field<bool>("IsAccepted") == (ddlStatus.SelectedIndex == 0 ? data.Field<bool>("IsAccepted") : ddlStatus.SelectedValue.ToBoolean())
                              select new
                              {
                                  ID = data.Field<int>("ID"),
                                  Contact_ID = data.Field<int>("Contact_ID"),
                                  Date = data.Field<DateTime>("Date"),
                                  FromTime = data.Field<DateTime>("FromTime"),
                                  ToTime = data.Field<DateTime>("ToTime"),
                                  Reason = data.Field<string>("Reason"),
                                  ApprovedBy_ID = data.Field<int>("ApprovedBy_ID"),
                                  Notes = data.Field<string>("Notes"),
                                  IsMission = data.Field<bool>("IsMission"),
                                  IsAccepted = data.Field<bool>("IsAccepted")

                              };


        gvComplaints.DataSource = filteredComplaints.CopyToDataTable();
        gvComplaints.DataBind();

        gvVacations.DataSource = filteredVacations.CopyToDataTable();
        gvVacations.DataBind();

        gvSancations.DataSource = filteredSancation.CopyToDataTable();
        gvSancations.DataBind();

        gvIncentives.DataSource = filteredIncentives.CopyToDataTable();
        gvIncentives.DataBind();

        gvAbasenceInMission.DataSource = filteredAbsenceInMission.CopyToDataTable();
        gvAbasenceInMission.DataBind();

        gvExcuses.DataSource = filteredExcuses.CopyToDataTable();
        gvExcuses.DataBind();
    }


}