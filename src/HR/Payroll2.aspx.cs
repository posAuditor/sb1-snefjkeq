using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Payroll2 : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtPayroll
    {
        get
        {
            return (DataTable)Session["dtPayroll" + this.WinID];
        }

        set
        {
            Session["dtPayroll" + this.WinID] = value;
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

    private decimal BasicSalary
    {
        get
        {
            if (ViewState["BasicSalary"] == null) return 0;
            return (decimal)ViewState["BasicSalary"];
        }

        set
        {
            ViewState["BasicSalary"] = value;
        }
    }

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
            this.Fill();
            txtDate.Focus();
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
            acDepartment.Clear();
            acName.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            this.FilterEmployees(null, null);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPayRoll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPayRoll.PageIndex = e.NewPageIndex;
            gvPayRoll.DataSource = this.dtPayroll;
            gvPayRoll.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPayRoll_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditID = gvPayRoll.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            var payroll = dc.usp_HR_PayRoll_SelectByID(this.EditID).FirstOrDefault();

            this.Contact_ID = payroll.Contact_ID.Value;
            this.BasicSalary = payroll.BasicSalary.Value;
            lblHeader.Text = payroll.ContactName + " - " + payroll.Month.ToExpressString() + "/" + payroll.Year.ToExpressString() + " - " + this.BasicSalary.ToExpressString();

            txtOverTime.Text = payroll.OvertimePeriod.ToExpressString();
            txtOverTimeValue.Text = payroll.OvertimeValue.ToExpressString();

            txtDayOffWork.Text = payroll.DayOffWorkPeriod.ToExpressString();
            txtDayOffWorkValue.Text = payroll.DayOffWorkValue.ToExpressString();

            txtDelay.Text = payroll.DelayPeriod.ToExpressString();
            txtDelayValue.Text = payroll.DelayValue.ToExpressString();

            txtAbsence.Text = payroll.AbsencePeriod.ToExpressString();
            txtAbsenceValue.Text = payroll.AbsenceValue.ToExpressString();

            txtExcuse.Text = payroll.ExcusePeriod.ToExpressString();
            txtExcuseValue.Text = payroll.ExcuseValue.ToExpressString();

            txtDeducatedDaysOff.Text = payroll.VacationWithDeducationPeriod.ToExpressString();
            txtDeducatedDaysOffValue.Text = payroll.VacationWithDeducationValue.ToExpressString();

            txtDeducatedWorkDays.Text = payroll.WorkWithDeductionPeriod.ToExpressString();
            txtDeducatedWorkDaysValue.Text = payroll.WorkWithDeductionValue.ToExpressString();

            txtIncentives.Text = payroll.Incentives.ToExpressString();
            // txtAllowances.Text = payroll.Allowances.ToExpressString();
            txtAllowances.Text = dc.usp_EmployeAllowance_Select(payroll.Contact_ID).ToList().Sum(x => x.MonthlyAllowance).ToExpressString();

            var listEmployeeExpence = dc.usp_EmployeExpense_Select(payroll.Contact_ID).ToList();



            txtTaxes.Text = payroll.Taxes.ToExpressString();
            txtInsurance.Text = payroll.Insurance.ToExpressString();
            //payroll.Insurance.ToExpressString();


            //txtTaxes.Text = payroll.Taxes.ToExpressString();
            //txtInsurance.Text = "0";//listEmployeeExpence.Where(x => x.TypeEmployeExpense == 9).Sum(x => x.MonthlyEmployeExpense).ToExpressString();
            ////payroll.Insurance.ToExpressString();

            txtLoans.Text = payroll.Loans.ToExpressString();
            txtSancations.Text = payroll.Sancations.ToExpressString();


            // txtOtherAdditions.Text = listEmployeeExpence.Where(x => x.TypeEmployeExpense == 10).Sum(x => x.MonthlyEmployeExpense).ToExpressString();

            //txtOtherAdditions.Text = payroll.OtherAdditions.ToExpressString();
            txtOtherDeducations.Text = payroll.OtherDeducations.ToExpressString();

            lblGrossTotal.Text = payroll.GrossTotal.ToExpressString();

            txtNotes.Text = payroll.Notes;
            //this.CalcGrossTotal(null, null);
            btnSaveNew.Visible = payroll.DocStatus_ID == DocStatus.Current.ToByte() && MyContext.PageData.IsEdit;
            mpeCreateNew.Show();
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

            result = dc.usp_HR_PayRoll_Update(
                this.EditID, DocStatus.Current.ToByte(),
                txtAbsence.Text.ToIntOrDefault(),
                txtAbsenceValue.Text.ToDecimalOrDefault(),
                txtOverTime.Text.ToIntOrDefault(),
                txtOverTimeValue.Text.ToDecimalOrDefault(),
                txtDayOffWork.Text.ToIntOrDefault(),
                txtDayOffWorkValue.Text.ToDecimalOrDefault(),
                txtDelay.Text.ToIntOrDefault(),
                txtDelayValue.Text.ToDecimalOrDefault(),
                txtExcuse.Text.ToIntOrDefault(),
                txtExcuseValue.Text.ToDecimalOrDefault(),
                txtDeducatedWorkDays.Text.ToIntOrDefault(),
                txtDeducatedWorkDaysValue.Text.ToDecimalOrDefault(),
                txtDeducatedDaysOff.Text.ToIntOrDefault(),
                txtDeducatedDaysOffValue.Text.ToDecimalOrDefault(),
                txtIncentives.Text.ToDecimalOrDefault(),
                txtAllowances.Text.ToDecimalOrDefault(),
                txtTaxes.Text.ToDecimalOrDefault(),
                txtLoans.Text.ToDecimalOrDefault(),
                txtSancations.Text.ToDecimalOrDefault(),
                txtInsurance.Text.ToDecimalOrDefault(),
                txtOtherAdditions.Text.ToDecimalOrDefault(),
                txtOtherDeducations.Text.ToDecimalOrDefault(),
                lblGrossTotal.Text.ToDecimalOrDefault(),
                txtNotes.TrimmedText
                );
            DataRow r = this.dtPayroll.Select("ID=" + this.EditID)[0];
            r["GrossTotal"] = lblGrossTotal.Text.ToDecimalOrDefault();
            gvPayRoll.DataSource = this.dtPayroll;
            gvPayRoll.DataBind();

            LogAction(Actions.Edit, lblHeader.Text, dc);
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            if (this.dtPayroll != null)
            {
                this.dtPayroll.Rows.Clear();
                gvPayRoll.DataSource = this.dtPayroll;
                gvPayRoll.DataBind();
            }
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            string serial = string.Empty;
            var company = dc.usp_Company_Select().FirstOrDefault();
            decimal? TotalSalary = 0;
            decimal? TotalSalaryOperation = 0;

            if (txtDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                return;
            }

            foreach (GridViewRow gvRow in gvPayRoll.Rows)
            {
                if (!((CheckBox)gvRow.FindControl("chkApprove")).Checked) continue;

                var payroll = dc.usp_HR_PayRoll_SelectByID(gvPayRoll.DataKeys[gvRow.RowIndex]["ID"].ToInt()).FirstOrDefault();

                if (payroll.DocStatus_ID == DocStatus.Approved.ToInt()) continue;

                TotalSalary = payroll.GrossTotal + payroll.Loans + payroll.Taxes + payroll.Insurance;

                TotalSalaryOperation = payroll.Loans + payroll.Taxes + payroll.Insurance + (payroll.GrossTotal >= 0 ? payroll.GrossTotal : 0) + (TotalSalary < 0 ? TotalSalary * -1 : 0);

                if (TotalSalaryOperation <= 0) continue;
                int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Salary.ToInt(), company.Currency_ID, TotalSalaryOperation, TotalSalaryOperation, 1, payroll.Notes);


                //Allowance


                var AllowanceValues = dc.usp_EmployeAllowance_Select(payroll.Contact_ID).Where(c => c.Account_ID != null).ToList();
                decimal TotalMonthlyAllowance = 0;
                foreach (var item in AllowanceValues)
                {
                    TotalMonthlyAllowance += item.MonthlyAllowance.ToDecimalOrDefault();
                    //راتب مستحق
                    if (item.MonthlyAllowance != 0) dc.usp_OperationDetails_Insert(Result, item.Account_ID,  item.MonthlyAllowance, 0, item.MonthlyAllowance,0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());


                }

                //رواتب واجور
                dc.usp_OperationDetails_Insert(Result, company.HRSalariesAccountID, TotalSalary - TotalMonthlyAllowance, 0, TotalSalary - TotalMonthlyAllowance, 0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());


                //راتب مستحق
                if (payroll.GrossTotal != 0) dc.usp_OperationDetails_Insert(Result, payroll.SalaryChartOfAccount_ID, 0, payroll.GrossTotal , 0, payroll.GrossTotal , null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());







                ////رواتب واجور
                //dc.usp_OperationDetails_Insert(Result, company.HRSalariesAccountID, TotalSalary, 0, TotalSalary, 0, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());






                ////راتب مستحق
                //if (payroll.GrossTotal != 0) dc.usp_OperationDetails_Insert(Result, payroll.SalaryChartOfAccount_ID, 0, payroll.GrossTotal, 0, payroll.GrossTotal, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                //تامينات
                if (payroll.Insurance != 0) dc.usp_OperationDetails_Insert(Result, company.HRSocialSecurityAccountID, 0, payroll.Insurance, 0, payroll.Insurance, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                //ضرائب
                if (payroll.Taxes != 0) dc.usp_OperationDetails_Insert(Result, company.HRWorkTaxAccountID, 0, payroll.Taxes, 0, payroll.Taxes, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());

                if (payroll.Loans != 0)
                {
                    if (payroll.LoanChartOfAccount_ID == null)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.EmpHasNoLoansAccount + " (" + payroll.ContactName + ")", string.Empty);
                        trans.Rollback();
                        return;
                    }

                    dc.usp_OperationDetails_Insert(Result, payroll.LoanChartOfAccount_ID, 0, payroll.Loans, 0, payroll.Loans, null, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt());
                }

                dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), payroll.CostCenter_ID, txtDate.Text.ToDate(), TotalSalary, payroll.ID, DocumentsTableTypes.HRPayroll.ToInt(), payroll.Notes);

                dc.usp_HR_PayRoll_Update(payroll.ID, DocStatus.Approved.ToByte(), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                ((CheckBox)gvRow.FindControl("chkApprove")).Enabled = false;
                LogAction(Actions.Approve, payroll.ContactName + " " + payroll.Year.ToExpressString() + "-" + payroll.Month.ToExpressString(), dc);
            }
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void CalcGrossTotal(object sender, EventArgs e)
    {
        try
        {
            decimal additions = this.BasicSalary + txtOverTimeValue.Text.ToDecimalOrDefault() +
                txtDayOffWorkValue.Text.ToDecimalOrDefault() +
                txtIncentives.Text.ToDecimalOrDefault() +
                txtAllowances.Text.ToDecimalOrDefault() +
                txtOtherAdditions.Text.ToDecimalOrDefault();

            decimal deducations = txtDelayValue.Text.ToDecimalOrDefault() +
                txtExcuseValue.Text.ToDecimalOrDefault() +
                txtAbsenceValue.Text.ToDecimalOrDefault() +
                txtDeducatedDaysOffValue.Text.ToDecimalOrDefault() +
                txtDeducatedWorkDaysValue.Text.ToDecimalOrDefault() +
                txtTaxes.Text.ToDecimalOrDefault() +
                txtInsurance.Text.ToDecimalOrDefault() +
                txtLoans.Text.ToDecimalOrDefault() +
                txtSancations.Text.ToDecimalOrDefault() +
                txtOtherDeducations.Text.ToDecimalOrDefault();

            lblGrossTotal.Text = (additions - deducations).ToExpressString();
            lblGrossTotal1.Text = (deducations).ToExpressString();
            lblGrossTotal2.Text = (additions).ToExpressString();
            mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOverTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtOverTimeValue.Text = dc.fun_HR_OverTime(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtOverTime.Text.ToIntOrDefault()).FirstOrDefault().OverTimeValue.ToExpressString();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDayOffWork_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDayOffWorkValue.Text = dc.fun_HR_DayOffWork(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDayOffWork.Text.ToIntOrDefault()).FirstOrDefault().DayOffWorkValue.ToExpressString();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDelay_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDelayValue.Text = dc.fun_HR_Delay(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDelay.Text.ToIntOrDefault()).FirstOrDefault().DelayValue.ToExpressString();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtAbsence_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtAbsenceValue.Text = dc.fun_HR_Absence(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtAbsence.Text.ToIntOrDefault()).FirstOrDefault().AbsenceValue.ToExpressString();
            this.CalcIncentives();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtExcuse_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtExcuseValue.Text = dc.fun_HR_Excuse(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtExcuse.Text.ToIntOrDefault()).FirstOrDefault().ExcuseValue.ToExpressString();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDeducatedWorkDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDeducatedWorkDaysValue.Text = dc.fun_HR_DeducatedWorkDays(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDeducatedWorkDays.Text.ToIntOrDefault()).FirstOrDefault().DeducatedWorkDaysValue.ToExpressString();
            this.CalcIncentives();
            this.CalcGrossTotal(null, null);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtDeducatedDaysOff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDeducatedDaysOffValue.Text = dc.fun_HR_DeducatedDaysOff(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), txtDeducatedDaysOff.Text.ToIntOrDefault()).FirstOrDefault().DeducatedDaysOffValue.ToExpressString();
            this.CalcIncentives();
            this.CalcGrossTotal(null, null);
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
        ddlYear.Items.Insert(0, new ListItem((DateTime.Now.Year - 1).ToExpressString(), (DateTime.Now.Year - 1).ToExpressString()));
        ddlYear.Items.Insert(1, new ListItem((DateTime.Now.Year).ToExpressString(), (DateTime.Now.Year).ToExpressString()));
        ddlYear.Items.Insert(2, new ListItem((DateTime.Now.Year + 1).ToExpressString(), (DateTime.Now.Year + 1).ToExpressString()));
        ddlYear.SelectedValue = (DateTime.Now.Year).ToExpressString();

        acDepartment.ContextKey = string.Empty;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        this.FilterEmployees(null, null);
        gvPayRoll.DataSource = this.dtPayroll;
        gvPayRoll.DataBind();


        txtOverTimeValue.ForeColor =
                txtDayOffWorkValue.ForeColor =
                txtIncentives.ForeColor =
                txtAllowances.ForeColor =
                txtOtherAdditions.ForeColor = System.Drawing.Color.Green;

        txtDelayValue.ForeColor =
                txtExcuseValue.ForeColor =
                txtAbsenceValue.ForeColor =
                txtDeducatedDaysOffValue.ForeColor =
                txtDeducatedWorkDaysValue.ForeColor =
                txtTaxes.ForeColor =
                txtInsurance.ForeColor =
                txtLoans.ForeColor =
                txtSancations.ForeColor =
                txtOtherDeducations.ForeColor = System.Drawing.Color.Red;
    }

    private void Fill()
    {
        if (new DateTime(ddlYear.SelectedValue.ToInt(), ddlMonth.SelectedValue.ToInt(), 1) > DateTime.Now.Date.AddDays(2))
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            return;
        }
        if (new DateTime(ddlYear.SelectedValue.ToInt(), ddlMonth.SelectedValue.ToInt(), 1) > MyContext.FiscalYearEndDate)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateOutsideFiscalYear, string.Empty);
            return;
        }
        var EmpList = dc.usp_HR_PayRoll_Select(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), acName.Value.ToNullableInt(), acDepartment.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), false).ToList();
        foreach (var Emp in EmpList)
        {
            dc.usp_HR_PayRoll_Calc(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), Emp.Contact_ID);
        }
        this.dtPayroll = dc.usp_HR_PayRoll_Select(ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), acName.Value.ToNullableInt(), acDepartment.Value.ToNullableInt(), acBranch.Value.ToNullableInt(), true).CopyToDataTable();
        gvPayRoll.DataSource = this.dtPayroll;
        gvPayRoll.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvPayRoll.Columns[6].Visible = MyContext.PageData.IsEdit;
        btnSearch.Visible = MyContext.PageData.IsAdd;
        btnApprove.Visible = MyContext.PageData.IsApprove;
    }

    private void CalcIncentives()
    {
        txtIncentives.Text = (dc.fun_HR_OtherSalaryValues(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt()).FirstOrDefault().Incentives +
            dc.fun_HR_AttendanceIncentives(this.Contact_ID, ddlMonth.SelectedValue.ToInt(), ddlYear.SelectedValue.ToInt(), (txtAbsence.Text.ToIntOrDefault() + txtDeducatedDaysOff.Text.ToIntOrDefault()))).ToExpressString();

    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
    }

    #endregion
}